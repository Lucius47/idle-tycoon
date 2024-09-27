using Unity.VisualScripting;
using UnityEngine;

public class StationSpawner : MonoBehaviour
{
    public Station station;
    public StationBase stationBuilder;

    private void Awake()
    {
        if (station == null)
        {
            station = GetComponentInChildren<Station>();
        }
        if (stationBuilder == null)
        {
            stationBuilder = GetComponentInChildren<StationBase>();
        }
    }

    public void SetUp(bool isBuilt, Station.StationType stationType, string _name, int cost)
    {
        station.stationType = stationType;
        stationBuilder.stationName = _name;
        stationBuilder.cost = cost;

        if (_name.Contains("supplier"))
        {
            station.GetComponent<GenericItemsHolder>().supplier = true;
        }

        if (isBuilt)
        {
            station.gameObject.SetActive(true);
            stationBuilder.gameObject.SetActive(false);
        }
        else
        {
            station.gameObject.SetActive(false);
            stationBuilder.gameObject.SetActive(true);
        }
    }
}
