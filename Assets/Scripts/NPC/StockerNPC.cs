using System.Collections;
using UnityEngine;

public class StockerNPC : NPCMovement
{
    // Look for supply shelf
    // Go to supply shelf
    // Take item from supply shelf
    // Go to the shelf
    // Put item on the shelf
    // Repeat

    private Shelf targetShelf;
    private GenericItemsHolder myHolder;

    private void Start()
    {
        myHolder = GetComponent<GenericItemsHolder>();
        StartCoroutine(LookForEmptyShelf());
    }

    private IEnumerator LookForEmptyShelf()
    {
        yield return new WaitForSeconds(0.5f);

        var allShelves = FindObjectsByType<Shelf>(FindObjectsSortMode.None);

        foreach (var shelf in allShelves)
        {
            if (!shelf.IsSupplyShelf() && !shelf.IsFull())
            {
                myHolder.itemType = shelf.GetItemType();
                StartCoroutine(LookForSupplyShelf());
                yield break;
            }
        }
    }


    private IEnumerator LookForSupplyShelf()
    {
        yield return new WaitForSeconds(0.5f);

        while (targetShelf == null)
        {
            var allShelves = FindObjectsByType<Shelf>(FindObjectsSortMode.None);
            foreach (Shelf shelf in allShelves)
            {
                // Shelf shelf = station as Shelf;
                if (!shelf.HasWorker() && shelf.IsSupplyShelf() && shelf.GetItemType() == myHolder.itemType)
                {
                    targetShelf = shelf;
                    break;
                }
            }

            if (targetShelf)
            {
                SetTarget(targetShelf.GetWorkerPosition(), () =>
                {
                    targetShelf = null;
                    StartCoroutine(LookForShelf());
                });
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    //public void ProcessEmptySupplyShelf<T>(T station) where T : Shelf
    //{
    //    if (!station.HasWorker() && station.IsSupplyShelf())
    //    {
    //        // Do something with the empty supply shelf
    //        // ...
    //    }
    //}

    private IEnumerator LookForShelf()
    {
        yield return new WaitForSeconds(0.5f);

        while (targetShelf == null)
        {
            var allShelves = FindObjectsByType<Shelf>(FindObjectsSortMode.None);
            foreach (Shelf shelf in allShelves)
            {
                if (!shelf.HasWorker() && !shelf.IsSupplyShelf() && shelf.GetItemType() == myHolder.itemType)
                {
                    targetShelf = shelf;
                    break;
                }
            }

            if (targetShelf)
            {
                SetTarget(targetShelf.transform, () =>
                {
                    targetShelf = null;
                    StartCoroutine(LookForEmptyShelf());
                });
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
