using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericItemsHolder : MonoBehaviour
{
    public Items.ItemType Type;
    public int numOfItems;

    [SerializeField] private Transform[] itemsHolds;
    [SerializeField] private int itemsPerRow = 1000; // add all items in one row if not set
    [SerializeField] private float gap = 0.2f;
    [SerializeField] private StackingDirection stackingDirection;

    public bool supplier = false;

    private List<GameObject> heldItems = new();

    public bool AddItem(Items.ItemType _type, out Transform addedTrans)
    {
        if (Type != _type)
        {
            //Wrong type
            addedTrans = null;
            return false;
        }

        heldItems ??= new List<GameObject>();


        if (heldItems.Count + 1 > itemsHolds.Length * itemsPerRow)
        {
            Debug.LogError("Not enough rows for this amount of items " + this.transform.name);
            addedTrans = null;
            return false;
        }

        foreach (var row in itemsHolds)
        {
            if (row.childCount < itemsPerRow)
            {
                GameObject item = Instantiate(Items.Instance.GetItem(_type), row.position, row.rotation, row);
                item.transform.localPosition = new Vector3(
                    (row.childCount - 1) * gap * (stackingDirection == StackingDirection.X ? 1 : 0),
                    (row.childCount - 1) * gap * (stackingDirection == StackingDirection.Y ? 1 : 0),
                    (row.childCount - 1) * gap * (stackingDirection == StackingDirection.Z ? 1 : 0));
                heldItems.Add(item);
                numOfItems++;
                break;
            }
        }

        addedTrans = heldItems[^1].transform;
        return true;
    }

    public bool RemoveItem(Items.ItemType _type, out Transform removedTrans)
    {
        if (Type != _type || numOfItems < 1)
        {
            removedTrans = null;
            return false;
        }

        var lastItemTransform = heldItems[^1].transform;
        Destroy(heldItems[^1]);
        heldItems.RemoveAt(heldItems.Count - 1);
        numOfItems--;
        removedTrans = lastItemTransform;
        return true;
    }

    // Testing
    public void AddItemBtn()
    {
        AddItem(Type, out Transform _);
    }

    public void RemoveItemBtn()
    {
        RemoveItem(Type, out Transform _);
    }


    private void Start()
    {
        if (supplier)
        {
            StartCoroutine(AddItemsPerTime());
        }
    }

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    AddItem(Type, out Transform _);
                }
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    RemoveItem(Type, out Transform _);
                }
            }
        }
    }

    private IEnumerator AddItemsPerTime()
    {
        while (true)
        {
            AddItem(Type, out Transform _);
            yield return new WaitForSeconds(2f);
        }
    }

    // Testing

    private enum StackingDirection
    {
        X, Y, Z,
    }
}
