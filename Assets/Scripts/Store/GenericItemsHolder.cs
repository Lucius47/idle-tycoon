using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericItemsHolder : MonoBehaviour
{
    public Item item;
    public int numOfItems;

    [SerializeField] private Transform[] itemsHolds;
    [SerializeField] private int itemsPerRow = 1000; // add all items in one row if not set
    [SerializeField] private float gap = 0.2f;
    [SerializeField] private StackingDirection stackingDirection;

    public bool supplier = false;

    private List<Item> heldItems = new();

    public bool AddItem(Item _item, out Transform addedTrans)
    {
        if (item.itemType != _item.itemType)
        {
            //Wrong type
            addedTrans = null;
            return false;
        }

        heldItems ??= new List<Item>();


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
                //Item item = Instantiate(Items.Instance.GetItem(_item), row.position, row.rotation, row);
                //item.transform.localPosition = new Vector3(
                //    (row.childCount - 1) * gap * (stackingDirection == StackingDirection.X ? 1 : 0),
                //    (row.childCount - 1) * gap * (stackingDirection == StackingDirection.Y ? 1 : 0),
                //    (row.childCount - 1) * gap * (stackingDirection == StackingDirection.Z ? 1 : 0));

                var newItem = new Item(_item.itemType, row.position, row.rotation, row);

                newItem.itemTransform.localPosition = new Vector3(
                        (row.childCount - 1) * gap * (stackingDirection == StackingDirection.X ? 1 : 0),
                        (row.childCount - 1) * gap * (stackingDirection == StackingDirection.Y ? 1 : 0),
                        (row.childCount - 1) * gap * (stackingDirection == StackingDirection.Z ? 1 : 0));

                heldItems.Add(newItem);
                numOfItems++;
                break;
            }
        }

        addedTrans = heldItems[^1].itemTransform;
        return true;
    }

    public bool RemoveItem(Item _item, out Transform removedTrans)
    {
        if (item != _item || numOfItems < 1)
        {
            removedTrans = null;
            return false;
        }

        var lastItemTransform = heldItems[^1].itemTransform;
        Destroy(heldItems[^1].itemTransform.gameObject);
        heldItems.RemoveAt(heldItems.Count - 1);
        numOfItems--;
        removedTrans = lastItemTransform;
        return true;
    }

    // Testing
    public void AddItemBtn()
    {
        AddItem(item, out Transform _);
    }

    public void RemoveItemBtn()
    {
        RemoveItem(item, out Transform _);
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
                    AddItem(item, out Transform _);
                }
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    RemoveItem(item, out Transform _);
                }
            }
        }
    }

    private IEnumerator AddItemsPerTime()
    {
        while (true)
        {
            AddItem(item, out Transform _);
            yield return new WaitForSeconds(2f);
        }
    }

    // Testing

    private enum StackingDirection
    {
        X, Y, Z,
    }
}
