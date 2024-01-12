using System.Collections;
using UnityEngine;

public class CashCollector : MonoBehaviour
{
    [SerializeField] private float delayPerExchange = 0.05f;

    private CashPile cashPile;

    Coroutine collectCashCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CashPile pile))
        {
            cashPile = pile;
            collectCashCoroutine = StartCoroutine(CollectCash());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CashPile pile))
        {
            if (cashPile == pile)
            {
                cashPile = null;
                StopCoroutine(collectCashCoroutine);
            }
        }
    }

    private IEnumerator CollectCash()
    {
        while (cashPile)
        {
            if (cashPile.Cash > 0)
            {
                PlayerStats.Cash += cashPile.GetCash();
                //Handheld.Vibrate();
                VibrationManager.Vibrate(50);
            }
            yield return new WaitForSeconds(delayPerExchange);
        }
    }
}
