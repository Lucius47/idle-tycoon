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
                if (!station.HasWorker() && station.IsSupplyShelf())
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

    private IEnumerator LookForShelf()
    {
        while (targetShelf == null)
        {
            var allStations = FindObjectsByType<Station>(FindObjectsSortMode.None);
            foreach (Station station in allStations)
            {
                if (!station.HasWorker() && !station.IsSupplyShelf()/* && station.GetItemType() == myHolder.itemType*/)
                {
                    targetShelf = station;
                    break;
                }
            }

            if (targetShelf)
            {
                SetTarget(targetShelf.transform, () =>
                {
                    Debug.LogError("Stocking item");
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
