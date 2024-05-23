using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericItemsHolder : MonoBehaviour
{
    public Items.ItemType itemType;
    public int numOfItems;

    [SerializeField] private Transform[] itemsHolds;
    [SerializeField] private int itemsPerRow = 1000; // add all items in one row if not set
    [SerializeField] private float gap = 0.2f;
    [SerializeField] private StackingDirection stackingDirection;

    public bool supplier = false;

    private List<Item> heldItems = new();

    public bool AddItem(Items.ItemType _itemType)
    {
        if (itemType != _itemType)
        {
            //Wrong type
            return false;
        }

        heldItems ??= new List<Item>();


        if (heldItems.Count + 1 > itemsHolds.Length * itemsPerRow)
        {
            Debug.LogError("Not enough rows for this amount of items " + this.transform.name);
            return false;
        }

        foreach (var row in itemsHolds)
        {
            if (row.childCount < itemsPerRow)
            {
                var newItem = new Item(_itemType, row.position, row.rotation, row);

                newItem.itemTransform.localPosition = new Vector3(
                        (row.childCount - 1) * gap * (stackingDirection == StackingDirection.X ? 1 : 0),
                        (row.childCount - 1) * gap * (stackingDirection == StackingDirection.Y ? 1 : 0),
                        (row.childCount - 1) * gap * (stackingDirection == StackingDirection.Z ? 1 : 0));

                heldItems.Add(newItem);
                numOfItems++;
                break;
            }
        }

        return true;
    }

    public bool AddItem(Item _item)
    {
        if (itemType != _item.itemType)
        {
            //Wrong type
            return false;
        }

        heldItems ??= new List<Item>();


        if (heldItems.Count + 1 > itemsHolds.Length * itemsPerRow)
        {
            //Debug.LogError("Not enough rows for this amount of items " + this.transform.name);
            return false;
        }

        foreach (var row in itemsHolds)
        {
            if (row.childCount < itemsPerRow)
            {
                _item.UpdateParent(row);
                _item.itemTransform.localRotation = Quaternion.identity;

                //_item.itemTransform.localPosition += new Vector3(
                //                   (row.childCount - 1) * gap * (stackingDirection == StackingDirection.X ? 1 : 0),
                //                   (row.childCount - 1) * gap * (stackingDirection == StackingDirection.Y ? 1 : 0),
                //                   (row.childCount - 1) * gap * (stackingDirection == StackingDirection.Z ? 1 : 0));

                // use DoTween to move the item to the new position

                var newPos = 
                    new Vector3(
                               (row.childCount - 1) * gap * (stackingDirection == StackingDirection.X ? 1 : 0),
                               (row.childCount - 1) * gap * (stackingDirection == StackingDirection.Y ? 1 : 0),
                               (row.childCount - 1) * gap * (stackingDirection == StackingDirection.Z ? 1 : 0)
                               );

                _item.itemTransform.DOLocalPath(new Vector3[]
                {
                    _item.itemTransform.localPosition, // start
                    new Vector3(( // mid point (slightly raised on y-axis)
                        (newPos + _item.itemTransform.localPosition) / 2).x,
                        ((newPos + _item.itemTransform.localPosition) / 2).y + 1.5f,
                        ((newPos + _item.itemTransform.localPosition) / 2).z),
                    newPos // end
                }, 0.5f);

                heldItems.Add(_item);
                numOfItems++;
                break;
            }
        }
        
        return true;
    }

    public Item RemoveItem(Items.ItemType _itemType)
    {
        if (itemType != _itemType || numOfItems < 1)
        {
            return null;
        }

        //var lastItemTransform = heldItems[^1].itemTransform;
        //Destroy(heldItems[^1].itemTransform.gameObject);
        numOfItems--;
        //removedTrans = lastItemTransform;

        var removedItem = heldItems[^1];
        heldItems.RemoveAt(heldItems.Count - 1);
        return removedItem;
        //return true;
    }

    //// Testing
    //public void AddItemBtn()
    //{
    //    AddItem(itemType, out Transform _);
    //}

    //public void RemoveItemBtn()
    //{
    //    RemoveItem(itemType, out Transform _);
    //}


    private void Start()
    {
        if (supplier)
        {
            StartCoroutine(AddItemsPerTime());
        }
    }

    //private void Update()
    //{
    //    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    if (Physics.Raycast(ray, out RaycastHit hit))
    //    {
    //        if (hit.collider.gameObject == gameObject)
    //        {
    //            if (Input.GetKeyDown(KeyCode.Space))
    //            {
    //                AddItem(itemType, out Transform _);
    //            }
    //            if (Input.GetKeyDown(KeyCode.Backspace))
    //            {
    //                RemoveItem(itemType, out Transform _);
    //            }
    //        }
    //    }
    //}

    private IEnumerator AddItemsPerTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // the first item was spawned too large.
            AddItem(itemType);
        }
    }

    // Testing

    private enum StackingDirection
    {
        X, Y, Z,
    }
}
