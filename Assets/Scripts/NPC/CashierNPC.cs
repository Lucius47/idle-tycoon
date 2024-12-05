using DG.Tweening;
using System.Collections;
using UnityEngine;

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
        state = State.LookingForCounter;
        MainLoop();
    }

    private void MainLoop()
    {
        switch (state)
        {
            case State.LookingForCounter:
                StartCoroutine(LookForCounter());
                break;
            case State.GoingToCounter:
                StartCoroutine(GotoCounter());
                break;
            case State.AtTheCounter:
                break;
        }
    }

    private IEnumerator LookForCounter()
    {
        while (targetStation == null)
        {
            var allStations = FindObjectsByType<Station>(FindObjectsSortMode.None);
            foreach (Station station in allStations)
            {
                if (station is Counter && !station.HasWorker())
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

    private IEnumerator GotoCounter()
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

                        //if (targetStation is Counter counter)
                        //{
                        //    // Stay at the counter.
                        //}

                        // face the counter
                        transform.DORotate(target.forward, 1); // temporary
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

    private enum State : byte
    {
        LookingForCounter,
        GoingToCounter,
        AtTheCounter
    }
}
