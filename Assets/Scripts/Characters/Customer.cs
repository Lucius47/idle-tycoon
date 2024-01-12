using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(NPCMovement))]
public class Customer : MonoBehaviour
{
    [SerializeField] private Transform itemTransform;

    private NPCMovement npcMovement;
    private Station currentStation;
    private Items.ItemType currentItem;
    private bool hasItem = false;


    private void Awake()
    {
        npcMovement = GetComponent<NPCMovement>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(SearchForShelves), 0f, 5f);
    }

    private void SearchForShelves()
    {
        if (currentStation == null)
        {
            GoToShelf();
        }
        else
        {
            CancelInvoke(nameof(SearchForShelves));
        }
    }

    private void GoToShelf()
    {
        // get a random station of type shelf from the storemanager.stations
        currentStation = StoreManager.Instance.Stations.Where(
            x => x.stationType == Station.StationType.Shelf).OrderBy(x => Random.value).FirstOrDefault();

        if (currentStation != null)
        {
            npcMovement.SetTarget(currentStation.customerWaitPoint, ReachedTarget);
        }
    }

    private void GoToCounter()
    {
        // get a random station of type counter from the storemanager.stations
        currentStation = StoreManager.Instance.Stations.Where(
                       x => x.stationType == Station.StationType.Counter).OrderBy(x => Random.value).FirstOrDefault();
        
        if (currentStation != null)
        {
            npcMovement.SetTarget(currentStation.customerWaitPoint, ReachedTarget);
        }
    }

    private void ReachedTarget()
    {
        if (currentStation.stationType == Station.StationType.Shelf)
        {
            var currentShelfStationItemsHolder = currentStation.GetComponent<GenericItemsHolder>();
            StartCoroutine(GetItemFromShelf(currentShelfStationItemsHolder));
        }
        else if (currentStation.stationType == Station.StationType.Counter)
        {
            // 
        }
    }

    private IEnumerator GetItemFromShelf(GenericItemsHolder currentShelfStationItemsHolder)
    {
        // get a random item from the station
        if (currentShelfStationItemsHolder.RemoveItem(currentShelfStationItemsHolder.Type))
        {
            currentItem = currentShelfStationItemsHolder.Type;
            Instantiate(Items.Instance.GetItem(currentItem), itemTransform.position, itemTransform.rotation, itemTransform);
            hasItem = true;
            GoToCounter();
        }

        if (!hasItem)
        {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(GetItemFromShelf(currentShelfStationItemsHolder));
        }
    }
}
