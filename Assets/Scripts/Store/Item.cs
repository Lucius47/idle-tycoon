using System;
using UnityEngine;

[Serializable]
public class Item
{
    public Items.ItemType itemType;
    public Transform itemTransform;

    public Item(Items.ItemType itemType, Vector3 position, Quaternion rotation, Transform itemParent = null)
    {
        this.itemType = itemType;

        GameObject gameObject = GameObject.Instantiate(Items.Instance.GetItem(itemType), position, rotation);

        if (itemParent != null)
        {
            gameObject.transform.parent = itemParent;
        }

        itemTransform = gameObject.transform;
    }
}
