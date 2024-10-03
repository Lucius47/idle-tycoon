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

    private Station targetShelf;
    private GenericItemsHolder myHolder;

    private void Start()
    {
        StartCoroutine(LookForSupplyShelf());
    }


    private IEnumerator LookForSupplyShelf()
    {
        while (targetShelf == null)
        {
            var allStations = FindObjectsByType<Station>(FindObjectsSortMode.None);
            foreach (Station station in allStations)
            {
                // Shelf shelf = station as Shelf;
                if (station is Shelf shelf && !station.HasWorker() && shelf.IsSupplyShelf())
                {
                    targetShelf = station;
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
        while (targetShelf == null)
        {
            var allStations = FindObjectsByType<Station>(FindObjectsSortMode.None);
            foreach (Station station in allStations)
            {
                if (station is Shelf shelf && !station.HasWorker() && !shelf.IsSupplyShelf()/* && station.GetItemType() == myHolder.itemType*/)
                {
                    targetShelf = station;
                    break;
                }
            }

            if (targetShelf)
            {
                SetTarget(targetShelf.transform, () =>
                {
                    targetShelf = null;
                    StartCoroutine(LookForSupplyShelf());
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
