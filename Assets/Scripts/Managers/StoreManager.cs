using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.CoreUtils;

/// <summary>
/// 1. Keep track of all the stations that are built and the items in those stations.
/// 2. Save this information to a file.
/// 3. Load this information from a file if it's available.
/// </summary>
public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance { get; private set; }
    public List<StationHolder> Stations { get; } = new List<StationHolder>();

    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Transform storeEntrance;
    public Transform storeExit;

    [SerializeField] private int numOfCustomers = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnCustomers());
    }


    private IEnumerator SpawnCustomers()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);

            foreach (StationHolder stationHolder in Stations)
            {
                if (stationHolder.station.stationType == Station.StationType.Shelf && stationHolder.station.IsAnySpotAvailable())
                {
                    Instantiate(customerPrefab, storeEntrance.position, storeEntrance.rotation);
                    break;
                }
            }
        }
    }

    public void AddStation(Station _station, Vector3 _position, Quaternion _rotation)
    {
        Stations.Add(new StationHolder { station = _station, position = _position, rotation = _rotation });
    }

    public class StationHolder
    {
        public Station station;
        public Vector3 position;
        public Quaternion rotation;
    }
}
