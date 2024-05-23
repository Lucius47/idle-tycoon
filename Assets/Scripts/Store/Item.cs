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


        ItemMono itemMono = GameObject.Instantiate(Items.Instance.GetItem(itemType), position, rotation);
        itemMono.SetUp();

        if (itemParent != null)
        {
            itemMono.transform.SetParent(itemParent);
        }

        itemTransform = itemMono.transform;
    }

    public void UpdateParent(Transform itemParent)
    {
        itemTransform.parent = itemParent;
    }
}
