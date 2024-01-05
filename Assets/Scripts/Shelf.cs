using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    [SerializeField] Items.ItemType itemType;
    public bool isStorageShelf = false;

    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private List<Transform> rows = new List<Transform>();
    [SerializeField] private int itemsPerRow = 5;
    [SerializeField] private float gap = 0.5f;

    private List<GameObject> Items;

    private ItemsHolder itemsHolder;
    private bool isPlayerInTrigger = false;

    //public int xxx = 5;

    private void Start()
    {
        //if (isStorageShelf) AddItem(xxx);
        Items = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (itemsHolder != null)
            {
                // Another player is already in trigger
                return;
            }
            itemsHolder = other.GetComponent<ItemsHolder>();
            isPlayerInTrigger = true;

            if (isStorageShelf)
            {
                RemoveItem();
            }
            else
            {
                AddItemByPlayer();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (itemsHolder == other.GetComponent<ItemsHolder>())
            // if the player that is leaving is the same player that is in trigger
            {
                itemsHolder = null;
                isPlayerInTrigger = false;
            }

        }
    }

    public async void AddItem()
    {
        Items ??= new List<GameObject>();


        if (Items.Count + 1 > rows.Count * itemsPerRow)
        {
            Debug.LogError("Not enough rows for this amount of items");
            return;
        }

        foreach (var row in rows)
        {
            if (row.childCount < itemsPerRow)
            {
                GameObject item = Instantiate(itemPrefab, row.position, row.rotation, row);
                item.transform.localPosition = new Vector3((row.childCount - 1) * gap, 0, 0);
                Items.Add(item);
                await Task.Delay(100);
                break;
            }
        }
    }

    private async void AddItemByPlayer()
    {
        while (itemsHolder != null && itemsHolder.type == itemType && itemsHolder.number > 0 && isPlayerInTrigger)
        {
            if (itemsHolder.RemoveItem(itemType, 1))
            {
                AddItem();
                await Task.Delay(100);
            }
        }
    }

    private async void RemoveItem()
    {
        while (Items.Count > 0 && isPlayerInTrigger)
        {
            itemsHolder.AddItem(itemType, 1);
            Destroy(Items[Items.Count - 1]);
            Items.RemoveAt(Items.Count - 1);
            await Task.Delay(100);
        }
    }
}
