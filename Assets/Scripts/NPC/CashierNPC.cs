using System;
using System.Collections;
using UnityEngine;
using static StoreManager;

[RequireComponent(typeof(NPCMovement))]
public class CashierNPC : MonoBehaviour
{
    private CashierState cashierState;
    private StockerState stockerState;
    private Counter currentCounter;

    private NPCMovement npcMovement;

    private void Awake()
    {
        npcMovement = GetComponent<NPCMovement>();
    }

    private void Start()
    {
        cashierState = CashierState.LookingForCounter;
        CashierLoop();
    }

    private void CashierLoop()
    {
        switch (cashierState)
        {
            case CashierState.LookingForCounter:
                StartCoroutine(LookForCounter());
                break;
            case CashierState.GoingToCounter:
                StartCoroutine(GotoCounter());
                break;
            case CashierState.AtTheCounter:
                break;
        }
    }

    private IEnumerator LookForCounter()
    {
        if (currentCounter == null)
        {
            var allCounters = FindObjectsByType<Counter>(FindObjectsSortMode.None);
            foreach (Counter counter in allCounters)
            {
                if (!counter.HasWorker())
                {
                    currentCounter = counter;
                    break;
                }
            }

            if (currentCounter)
            {
                cashierState++;
                CashierLoop();
                yield break; // Exits the coroutine
            }
            else
            {
                yield return new WaitForSeconds(1f);
                StockerLoop();
                yield break;
            }
        }

        //state++;
        //MainLoop();
    }

    private IEnumerator GotoCounter()
    {
        bool isProcessing = true;

        while (isProcessing)
        {
            if (currentCounter)
            {
                var target = currentCounter.GetWorkerPosition();

                if (target)
                {
                    // Set the target and use a callback to update the state and break the loop
                    npcMovement.SetTarget(target, () =>
                    {
                        cashierState++;
                        isProcessing = false; // Break the loop
                    });

                    // Wait for the callback to be called before proceeding
                    yield return new WaitUntil(() => !isProcessing);
                }
                else
                {
                    currentCounter = null;
                    cashierState--;
                    yield return new WaitForSeconds(0.5f); // Wait before the next iteration
                }
            }
            else
            {
                cashierState--;
                yield return new WaitForSeconds(0.5f); // Wait before the next iteration
            }

            // Check if we need to break the loop or continue
            isProcessing = currentCounter != null && isProcessing;
        }

        CashierLoop(); // Call MainLoop after breaking out of the loop
    }

    private enum CashierState : byte
    {
        LookingForCounter,
        GoingToCounter,
        AtTheCounter
    }

    private Station currentTarget;

    private void StockerLoop()
    {
        switch (stockerState)
        {
            case StockerState.LookingForSupplyShelf:
                StartCoroutine(LookForSupplyShelf());
                break;
            case StockerState.GoingToSupplyShelf:
                StartCoroutine(GotoSupplyShelf());
                break;
            case StockerState.LookingForShelf:
                StartCoroutine(LookForShelf());
                break;
            case StockerState.GoingToShelf:
                StartCoroutine(GotoShelf());
                break;
        }
    }

    private IEnumerator GotoShelf()
    {
        throw new NotImplementedException();
    }

    private IEnumerator LookForShelf()
    {
        throw new NotImplementedException();
    }

    private IEnumerator LookForSupplyShelf()
    {
        while (currentTarget == null)
        {
            foreach (StationHolder stationHolder in StoreManager.Instance.Stations)
            {
                if ((stationHolder.station.stationType == Station.StationType.ShoesShelf
                    || stationHolder.station.stationType == Station.StationType.ShirtsStand)
                    && stationHolder.station.IsSupplyShelf())
                {
                    currentTarget = stationHolder.station;
                    break;
                }
            }

            if (currentTarget)
            {
                stockerState++;
                StockerLoop();
                yield break; // Exits the coroutine
            }
            else
            {
                yield return new WaitForSeconds(1f); // Wait and then continue the loop
            }
        }
    }

    private IEnumerator GotoSupplyShelf()
    {
        bool isProcessing = true;

        while (isProcessing)
        {
            if (currentTarget)
            {
                var target = currentTarget.GetWorkerPosition();

                if (target)
                {
                    // Set the target and use a callback to update the state and break the loop
                    npcMovement.SetTarget(target, () =>
                    {
                        stockerState++;
                        isProcessing = false; // Break the loop
                    });

                    // Wait for the callback to be called before proceeding
                    yield return new WaitUntil(() => !isProcessing);
                }
                else
                {
                    currentTarget = null;
                    stockerState--;
                    yield return new WaitForSeconds(0.5f); // Wait before the next iteration
                }
            }
            else
            {
                stockerState--;
                yield return new WaitForSeconds(0.5f); // Wait before the next iteration
            }

            // Check if we need to break the loop or continue
            isProcessing = currentTarget != null && isProcessing;
        }

        StockerLoop(); // Call MainLoop after breaking out of the loop
    }

    private enum StockerState : byte
    {
        LookingForSupplyShelf,
        GoingToSupplyShelf,
        LookingForShelf,
        GoingToShelf
    }
}
