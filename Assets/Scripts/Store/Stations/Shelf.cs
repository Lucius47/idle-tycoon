using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : Station
{
    public bool IsSupplyShelf()
    {
        if (TryGetComponent<GenericItemsHolder>(out var holder))
        {
            return holder.supplier;
        }
        else
        {
            return false;
        }
    }

    public Items.ItemType GetItemType()
    {
        if (TryGetComponent<GenericItemsHolder>(out var holder))
        {
            return holder.itemType;
        }
        else
        {
            Debug.LogError("No GenericItemsHolder found on " + gameObject.name);
            throw new NotImplementedException();
        }
    }
}
