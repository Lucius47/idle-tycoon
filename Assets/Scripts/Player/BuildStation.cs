using System.Collections;
using UnityEngine;

public class BuildStation : MonoBehaviour
{
    [SerializeField] private float delay = 0.05f;

    private StationBase stationBase;

    Coroutine buildStationCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out StationBase _stationBase))
        {
            stationBase = _stationBase;
            buildStationCoroutine = StartCoroutine(Build());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out StationBase _stationBase))
        {
            if (stationBase == _stationBase)
            {
                stationBase = null;
                StopCoroutine(buildStationCoroutine);
            }
        }
    }

    private IEnumerator Build()
    {
        while (stationBase)
        {
            if (PlayerStats.Cash > 0 && stationBase.remainingCost > 0)
            {
                PlayerStats.Cash -= stationBase.BuildStation();
                //Handheld.Vibrate();
                VibrationManager.Vibrate(50);
            }
            yield return new WaitForSeconds(delay);
        }
    }
}
