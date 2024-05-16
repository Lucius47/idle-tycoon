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

    public void SetUp(bool isBuilt, Station.StationType stationType, string _name)
    {
        station.stationType = stationType;
        stationBuilder.stationName = _name;

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
