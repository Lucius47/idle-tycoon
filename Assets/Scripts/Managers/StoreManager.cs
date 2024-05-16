using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StoreManager;

/// <summary>
/// 1. Keep track of all the stations that are built and the items in those stations.
/// 2. Save this information to a file.
/// 3. Load this information from a file if it's available.
/// </summary>
public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance { get; private set; }
    public List<StationHolder> Stations = new List<StationHolder>();

    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Transform storeEntrance;
    public Transform storeExit;

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
        LoadStations();
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

    private void LoadStations()
    {
        StoreX storeX;

        if (PlayerPrefs.GetInt("firstTime", 1) == 1)
        {
            PlayerPrefs.SetInt("firstTime", 0);
            PlayerPrefs.Save();
            storeX = Stores.Instance.stores[0];
            System.IO.File.WriteAllText(Application.persistentDataPath + "/Stations.json", JsonUtility.ToJson(storeX));
        }
        else
        {
            storeX = JsonUtility.FromJson<StoreX>(System.IO.File.ReadAllText(Application.persistentDataPath + "/Stations.json"));
        }


        if (storeX == null)
        {
            return;
        }

        foreach (StationHolderX stationHolderX in storeX.stations)
        {
            StationSpawner stationSpawner = Instantiate(Items.Instance.GetStation(stationHolderX.station.stationType), 
                stationHolderX.position, stationHolderX.rotation);

            stationSpawner.SetUp(stationHolderX.isBuilt, stationHolderX.station.stationType, stationHolderX.name);
            
            if (stationHolderX.isBuilt)
            {
                Stations.Add(new StationHolder { station = stationSpawner.station, 
                    position = stationHolderX.position, rotation = stationHolderX.rotation });
            }
        }
    }

    public void AddStation(Station _station, Vector3 _position, Quaternion _rotation, string _name)
    {
        Stations.Add(new StationHolder { station = _station, position = _position, rotation = _rotation });

        StoreX storeX = JsonUtility.FromJson<StoreX>(System.IO.File.ReadAllText(Application.persistentDataPath + "/Stations.json"));

        foreach (StationHolderX stationHolder in storeX.stations)
        {
            if (stationHolder.name == _name)
            {
                stationHolder.isBuilt = true;
                break;
            }
        }

        string json = JsonUtility.ToJson(storeX);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/Stations.json", json);
    }

    [Serializable]
    public class StationHolder
    {
        public Station station;
        public Vector3 position;
        public Quaternion rotation;
    }
}

[Serializable]
public class StoreX
{
    public List<StationHolderX> stations = new();
}

[Serializable]
public class StationHolderX
{
    public string name;
    public StationX station;
    public Vector3 position;
    public Quaternion rotation;

    public bool isBuilt;
}

[Serializable]
public class StationX
{
    public Station.StationType stationType;
}