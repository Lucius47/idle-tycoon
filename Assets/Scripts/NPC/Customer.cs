using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Customer : NPCMovement
{
    [SerializeField] private Transform itemTransform;

    private Items.ItemType currentItemType;

    private string[] nickNames;
    private string nickName;

    private void Start()
    {
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

        StartCoroutine(LookForShelf());
    }

    //private enum State : int
    //{
    //    LookingForShelf,
    //    GoingToShelf,
    //    BrowsingItems,
    //    LookingForCounter,
    //    GoingToCounter,
    //    WaitingInLine,
    //    PerformingCheckout,
    //    Leaving
    //}

    //private State state = State.LookingForShelf;

    private Station currentShelf;
    private IEnumerator LookForShelf()
    {
        while (currentShelf == null)
        {
            foreach (StoreManager.StationHolder stationHolder in StoreManager.Instance.Stations)
            {
                if ((stationHolder.station is Shelf shelf)
                    && stationHolder.station.IsAnySpotAvailable()
                    && !shelf.IsSupplyShelf())
                {
                    currentShelf = stationHolder.station;
                    break;
                }
            }

            if (currentShelf)
            {
                StartCoroutine(GotoShelf());
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }

        //state++;
        //MainLoop();
    }

    private IEnumerator GotoShelf()
    {
        bool hasReached = false;

        if (currentShelf)
        {
            var target = currentShelf.GetWaitPoint(this);

            if (target)
            {
                SetTarget(target, () =>
                {
                    //state++;
                    StartCoroutine(BrowseItems());
                    hasReached = true;
                });

                yield return new WaitUntil(() => hasReached);
                yield break;
            }
            else
            {
                currentShelf = null;
                StartCoroutine(LookForShelf());
                yield break;
            }
        }
        else
        {
            StartCoroutine(LookForShelf());
            yield break;
        }
    }

    private IEnumerator BrowseItems()
    {
        var currentShelfStationItemsHolder = currentShelf.GetComponent<GenericItemsHolder>();
        var itemForAnimation = currentShelfStationItemsHolder.RemoveItem(currentShelfStationItemsHolder.itemType);

        while (itemForAnimation == null)
        {
            itemForAnimation = currentShelfStationItemsHolder.RemoveItem(currentShelfStationItemsHolder.itemType);

            if (itemForAnimation != null)
            {
                itemForAnimation.itemTransform.DOMove(itemTransform.position, 0.5f).OnComplete(() => Destroy(itemForAnimation.itemTransform.gameObject));
                currentItemType = currentShelfStationItemsHolder.itemType;
                new Item(currentItemType, itemTransform.position, itemTransform.rotation, itemTransform);
                StartCoroutine(LookForCounter());
                yield break;
            }

            yield return new WaitForSeconds(0.5f);
        }

        //if (itemForAnimation != null)
        //{
        //    itemForAnimation.itemTransform.DOMove(itemTransform.position, 0.5f).OnComplete(() => Destroy(itemForAnimation.itemTransform.gameObject));
        //    currentItemType = currentShelfStationItemsHolder.itemType;
        //    // Instantiate(Items.Instance.GetItem(currentItem), itemTransform.position, itemTransform.rotation, itemTransform);
        //    new Item(currentItemType, itemTransform.position, itemTransform.rotation, itemTransform);

        //    //hasItem2 = true; // Item successfully obtained
        //    //state++;
        //    //state = State.LookingForCounter; // for some reason, state++ doesn't work here, even though it does
        //    // everywhere else.
        //    StartCoroutine(LookForCounter());
        //}
        //else
        //{
        //    StartCoroutine(BrowseItems());
        //}
    }

    private Station currentCounterStation;
    private IEnumerator LookForCounter()
    {
        foreach (StoreManager.StationHolder stationHolder in StoreManager.Instance.Stations)
        {
            if (stationHolder.station is Counter)
            {
                currentCounterStation = stationHolder.station;
                break;
            }
        }

        if (currentCounterStation)
        {
            StartCoroutine(GotoCounter());
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(LookForCounter());
        }
    }

    private IEnumerator GotoCounter()
    {
        bool hasReached = false;

        if (currentCounterStation)
        {
            var target = currentCounterStation.GetWaitPoint(this);

            if (target)
            {
                SetTarget(target, () =>
                {
                    //state++;
                    StartCoroutine(WaitInLine());
                    hasReached = true;
                });

                yield return new WaitUntil(() => hasReached);
                yield break;
            }
            else
            {
                currentCounterStation = null;
                StartCoroutine(LookForCounter());
                yield break;
            }
        }
        else
        {
            StartCoroutine(LookForCounter());
            yield break;
        }
    }

    private IEnumerator WaitInLine()
    {
        if (currentCounterStation && currentCounterStation.IsCustomerPresent(this))
        {
            // Debug.LogError($"{nickName} is waiting in line");
            StartCoroutine(PerformCheckout());
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(GotoCounter());
        }
    }


    private bool checkoutComplete;
    private IEnumerator PerformCheckout()
    {
        var currentCounter = currentCounterStation.GetComponent<Counter>();
        bool isProcessing = true;

        while (isProcessing)
        {
            if (currentCounter && !checkoutComplete && currentCounter.RequestCheckout())
            {
                if (itemTransform.childCount > 0)
                {
                    Destroy(itemTransform.GetChild(0).gameObject); // Ensure there's an item to destroy
                }

                Instantiate(Items.Instance.boxPrefab, itemTransform.position, itemTransform.rotation, itemTransform);
                checkoutComplete = true;
                //state++;
                //MainLoop();

                SetTarget(StoreManager.Instance.storeExit, () =>
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


    //public TMPro.TextMeshProUGUI debugText;
    //private void Update()
    //{
    //    debugText.text = state.ToString();
    //}
}
