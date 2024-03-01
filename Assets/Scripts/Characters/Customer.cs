using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(NPCMovement))]
public class Customer : MonoBehaviour
{
    [SerializeField] private Transform itemTransform;

    private NPCMovement npcMovement;
    private Items.ItemType currentItem;

    private string[] nickNames;
    private string nickName;

    private void Awake()
    {
        npcMovement = GetComponent<NPCMovement>();
    }

    private void Start()
    {
        // Add 10 english nicknames to the list
        nickNames = new string[]
        {
            "John",
            "Mary",
            "James",
            "Patricia",
            "Robert",
            "Jennifer",
            "Michael",
            "Linda",
            "William",
            "Elizabeth",
        };

        nickName = nickNames[Random.Range(0, nickNames.Length)];

        state = State.LookingForShelf;
        MainLoop();
    }

    private enum State
    {
        LookingForShelf,
        GoingToShelf,
        BrowsingItems,
        LookingForCounter,
        GoingToCounter,
        WaitingInLine,
        PerformingCheckout,
        Leaving
    }

    private State state = State.LookingForShelf;

    private void MainLoop()
    {
        switch (state)
        {
            case State.LookingForShelf:
                {
                    StartCoroutine(LookForShelf());
                    break;
                }
            case State.GoingToShelf:
                {
                    StartCoroutine(GotoShelf());
                    break;
                }
            case State.BrowsingItems:
                {
                    StartCoroutine(BrowseItems());
                    break;
                }
            case State.LookingForCounter:
                {
                    StartCoroutine(LookForCounter());
                    break;
                }
            case State.GoingToCounter:
                {
                    StartCoroutine(GotoCounter());
                    break;
                }
            case State.WaitingInLine:
                {
                    StartCoroutine(WaitInLine());
                    break;
                }
            case State.PerformingCheckout:
                {
                    StartCoroutine(PerformCheckout());
                    break;
                }
            case State.Leaving:
                {
                    break;
                }
        }
    }

    private Station currentShelf;
    private IEnumerator LookForShelf()
    {
        while (currentShelf == null)
        {
            foreach (Station station in StoreManager.Instance.Stations)
            {
                if (station.stationType == Station.StationType.Shelf && station.IsAnySpotAvailable())
                {
                    currentShelf = station;
                    break;
                }
            }

            if (currentShelf)
            {
                state++;
                MainLoop();
                yield break; // Exits the coroutine
            }
            else
            {
                yield return new WaitForSeconds(1f); // Wait and then continue the loop
            }
        }

        //state++;
        //MainLoop();
    }

    private IEnumerator GotoShelf()
    {
        bool isProcessing = true;

        while (isProcessing)
        {
            if (currentShelf)
            {
                var target = currentShelf.GetWaitPoint(this);

                if (target)
                {
                    // Set the target and use a callback to update the state and break the loop
                    npcMovement.SetTarget(target, () =>
                    {
                        state++;
                        isProcessing = false; // Break the loop
                    });

                    // Wait for the callback to be called before proceeding
                    yield return new WaitUntil(() => !isProcessing);
                }
                else
                {
                    currentShelf = null;
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
            isProcessing = currentShelf != null && isProcessing;
        }

        MainLoop(); // Call MainLoop after breaking out of the loop
    }

    private bool hasItem2;
    private IEnumerator BrowseItems()
    {
        var currentShelfStationItemsHolder = currentShelf.GetComponent<GenericItemsHolder>();

        // Keep browsing items until an item is successfully obtained
        while (!hasItem2)
        {
            yield return new WaitForSeconds(0.5f); // Wait for a bit before trying again

            // Attempt to remove an item from the shelf
            if (currentShelfStationItemsHolder.RemoveItem(currentShelfStationItemsHolder.Type, out Transform removedTrans))
            {
                // for the animation, spawn an item, move it to the player, destroy it
                var itemForAnim = Instantiate(Items.Instance.GetItem(currentShelfStationItemsHolder.Type), removedTrans.position, removedTrans.rotation);
                itemForAnim.transform.DOMove(itemTransform.position, 0.5f).OnComplete(() => Destroy(itemForAnim));

                currentItem = currentShelfStationItemsHolder.Type;
                Instantiate(Items.Instance.GetItem(currentItem), itemTransform.position, itemTransform.rotation, itemTransform);
                hasItem2 = true; // Item successfully obtained
            }
            // If the item is not obtained, this loop will continue, effectively "browsing" again
        }

        state++; // Update state after obtaining an item
        MainLoop(); // Call MainLoop after breaking out of the loop
    }

    private Station currentCounterStation;
    private IEnumerator LookForCounter()
    {
        bool isSearching = true;

        while (isSearching)
        {
            if (!currentCounterStation)
            {
                //currentCounterStation = StoreManager.Instance.Stations
                //    .Where(x => x.stationType == Station.StationType.Counter)
                //    .OrderBy(x => Random.value)
                //    .FirstOrDefault();

                foreach (Station station in StoreManager.Instance.Stations)
                {
                    if (station.stationType == Station.StationType.Counter)
                    {
                        currentCounterStation = station;
                        break;
                    }
                }

                if (currentCounterStation)
                {
                    state++;
                    isSearching = false; // Found a counter station, so stop searching
                }
                else
                {
                    yield return new WaitForSeconds(1f); // Wait for a bit before trying again
                }
            }
            else
            {
                state++;
                isSearching = false; // Already have a counter station, so stop searching
            }
        }

        MainLoop(); // Call MainLoop after breaking out of the loop
    }

    private IEnumerator GotoCounter()
    {
        bool isProcessing = true;

        while (isProcessing)
        {
            if (currentCounterStation)
            {
                var target = currentCounterStation.GetWaitPoint(this);

                if (target)
                {
                    // Set the target and use a callback to update the state and break the loop
                    npcMovement.SetTarget(target, () =>
                    {
                        state++;
                        isProcessing = false; // Break the loop
                    });

                    // Wait for the callback to be called before proceeding
                    yield return new WaitUntil(() => !isProcessing);
                }
                else
                {
                    currentCounterStation = null;
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
            isProcessing = currentCounterStation != null && isProcessing;
        }

        MainLoop(); // Call MainLoop after breaking out of the loop
    }

    private IEnumerator WaitInLine()
    {
        if (currentCounterStation && currentCounterStation.IsCustomerPresent(this))
        {
            // Debug.LogError($"{nickName} is waiting in line");
            state++;
            MainLoop();
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            state--;
            MainLoop();
        }
    }


    private bool checkoutComplete2;
    private IEnumerator PerformCheckout()
    {
        var currentCounter = currentCounterStation.GetComponent<Counter>();
        bool isProcessing = true;

        while (isProcessing)
        {
            if (currentCounter && !checkoutComplete2 && currentCounter.RequestCheckout())
            {
                if (itemTransform.childCount > 0)
                {
                    Destroy(itemTransform.GetChild(0).gameObject); // Ensure there's an item to destroy
                }

                Instantiate(Items.Instance.boxPrefab, itemTransform.position, itemTransform.rotation, itemTransform);
                checkoutComplete2 = true;
                state++;
                MainLoop();

                npcMovement.SetTarget(StoreManager.Instance.storeExit, () =>
                {
                    Destroy(gameObject);
                });

                isProcessing = false; // Exit the loop
            }
            else
            {
                yield return new WaitForSeconds(0.5f); // Wait for a bit before trying again
            }
        }
    }


    public TMPro.TextMeshProUGUI debugText;
    private void Update()
    {
        debugText.text = state.ToString();
    }
}
