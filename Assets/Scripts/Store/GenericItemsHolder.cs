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
            return false;
        }

        heldItems ??= new List<Item>();


        if (heldItems.Count + 1 > itemsHolds.Length * itemsPerRow)
        {
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
            return false;
        }

        heldItems ??= new List<Item>();


        if (heldItems.Count + 1 > itemsHolds.Length * itemsPerRow)
        {
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

        numOfItems--;

        var removedItem = heldItems[^1];
        heldItems.RemoveAt(heldItems.Count - 1);
        return removedItem;
    }


    private void Start()
    {
        if (supplier)
        {
            StartCoroutine(AddItemsPerTime());
        }
    }

    private IEnumerator AddItemsPerTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // the first item was spawned too large.
            AddItem(itemType);
        }
    }

    private enum StackingDirection
    {
        X, Y, Z,
    }
}
