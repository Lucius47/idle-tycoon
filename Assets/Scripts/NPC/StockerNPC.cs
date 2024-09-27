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

    private State state;
    private Station targetSupplyShelf;
    private Station targetShelf;
    private GenericItemsHolder itemsHolder;

    private void Start()
    {
        state = State.LookingForSupplyShelf;
        MainLoop();
    }

    private void MainLoop()
    {
        switch (state)
        {
            case State.LookingForSupplyShelf:
                StartCoroutine(LookForSupplyShelf());
                break;
            case State.GoingToSupplyShelf:
                StartCoroutine(GoToSupplyShelf());
                break;
            case State.LookingForShelf:
                StartCoroutine(LookForShelf());
                break;
            case State.GoingToShelf:
                GoToShelf();
                break;
        }
    }

    private IEnumerator LookForSupplyShelf()
    {
        while (targetSupplyShelf == null)
        {
            var allStations = FindObjectsByType<Station>(FindObjectsSortMode.None);
            foreach (Station station in allStations)
            {
                if (!station.HasWorker() && station.IsSupplyShelf())
                {
                    targetSupplyShelf = station;
                    break;
                }
            }

            if (targetSupplyShelf)
            {
                state++;
                MainLoop();
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private IEnumerator GoToSupplyShelf()
    {
        //bool isProcessing = true;

        //while (isProcessing)
        {
            if (targetSupplyShelf)
            {
                SetTarget(targetSupplyShelf.transform, () =>
                {
                    Debug.LogError("Taking item from supply shelf");
                    state++;
                    MainLoop();
                });
                //isProcessing = false;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator LookForShelf()
    {
        while (targetShelf == null)
        {
            var allStations = FindObjectsByType<Station>(FindObjectsSortMode.None);
            foreach (Station station in allStations)
            {
                if (!station.HasWorker() && !station.IsSupplyShelf())
                {
                    targetShelf = station;
                    break;
                }
            }

            if (targetShelf)
            {
                state++;
                MainLoop();
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private void GoToShelf()
    {
        //bool isProcessing = true;

        //while (isProcessing)
        {
            if (targetShelf)
            {
                SetTarget(targetShelf.transform, () =>
                {
                    Debug.LogError("Stocking item");
                    //state = State.LookingForSupplyShelf;
                    //targetSupplyShelf = null;
                    //targetShelf = null;
                    //MainLoop();
                });
                //isProcessing = false;
            }
            //yield return new WaitForSeconds(1f);
        }
    }

    private enum State : byte
    {
        LookingForSupplyShelf,
        GoingToSupplyShelf,
        LookingForShelf,
        GoingToShelf,
    }
}
