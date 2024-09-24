using System;
using System.Collections;
using UnityEngine;
using static StoreManager;

[RequireComponent(typeof(NPCMovement))]
public class CashierNPC : MonoBehaviour
{
    private State state;
    private Station targetStation;

    private NPCMovement npcMovement;

    private void Awake()
    {
        npcMovement = GetComponent<NPCMovement>();
    }

    private void Start()
    {
        state = State.LookingForStation;
        MainLoop();
    }

    private void MainLoop()
    {
        switch (state)
        {
            case State.LookingForStation:
                StartCoroutine(LookForStation());
                break;
            case State.GoingToStation:
                StartCoroutine(GotoStation());
                break;
            case State.AtTheCounter:
                break;
        }
    }

    private IEnumerator LookForStation()
    {
        while (targetStation == null)
        {
            var allStations = FindObjectsByType<Station>(FindObjectsSortMode.None);
            foreach (Station station in allStations)
            {
                if (!station.HasWorker() && station.IsSupplyShelf())
                {
                    targetStation = station;
                    break;
                }
            }

            if (targetStation)
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

        //state++;
        //CashierLoop();
    }

    private IEnumerator GotoStation()
    {
        bool isProcessing = true;

        while (isProcessing)
        {
            if (targetStation)
            {
                var target = targetStation.GetWorkerPosition();

                if (target)
                {
                    // Set the target and use a callback to update the state and break the loop
                    npcMovement.SetTarget(target, () =>
                    {
                        state++;
                        isProcessing = false; // Break the loop

                        if (targetStation is Counter counter)
                        {
                            // Stay at the counter.
                        }
                        else
                        {
                            PerformStockerDuties();
                        }
                    });

                    // Wait for the callback to be called before proceeding
                    yield return new WaitUntil(() => !isProcessing);
                }
                else
                {
                    targetStation = null;
                    state--;
                    yield return new WaitForSeconds(0.5f); // Wait before the next iteration
                }
            }
            else
            {
                state--;
                yield return new WaitForSeconds(0.5f); // Wait before the next iteration
            }

            // Check if we need to break the loop or continue
            isProcessing = targetStation != null && isProcessing;
        }

        MainLoop(); // Call MainLoop after breaking out of the loop
    }

    private void PerformStockerDuties()
    {
        var allStations = FindObjectsByType<Station>(FindObjectsSortMode.None);
        foreach (Station station in allStations)
        {
            if (!station.IsSupplyShelf())
            {
                var target = station.GetWorkerPosition();
                if (target)
                {
                    npcMovement.SetTarget(target, () =>
                    {
                        state = State.LookingForStation;
                        MainLoop();
                    });
                    break;
                }
            }
        }
    }

    private enum State : byte
    {
        LookingForStation,
        GoingToStation,
        AtTheCounter
    }
}
