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

    public bool AddItem(Items.ItemType _type)
    {
        if (Type != _type)
        {
            //Wrong type
            return false;
        }

        heldItems ??= new List<GameObject>();


        if (heldItems.Count + 1 > itemsHolds.Length * itemsPerRow)
        {
            Debug.LogError("Not enough rows for this amount of items " + this.transform.name);
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

        return true;
    }

    public bool RemoveItem(Items.ItemType _type)
    {
        if (Type != _type || numOfItems < 1)
        {
            return false;
        }

        Destroy(heldItems[^1]);
        heldItems.RemoveAt(heldItems.Count - 1);
        numOfItems--;
        return true;
    }

    // Testing
    public void AddItemBtn()
    {
        AddItem(Type);
    }

    public void RemoveItemBtn()
    {
        RemoveItem(Type);
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
                    AddItem(Type);
                }
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    RemoveItem(Type);
                }
            }
        }
    }

    private IEnumerator AddItemsPerTime()
    {
        while (true)
        {
            AddItem(Type);
            yield return new WaitForSeconds(2f);
        }
    }

    // Testing

    private enum StackingDirection
    {
        X, Y, Z,
    }
}
