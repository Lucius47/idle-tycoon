using DG.Tweening;
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
                PlayerStats.Cash += cashPile.GetCash(out Vector3 lastCashPos);

                var cash = Instantiate(Items.Instance.cashPrefab, lastCashPos, Quaternion.identity);
                cash.transform.DOPath(
                    new Vector3[] { 
                        lastCashPos, 
                        new Vector3(((lastCashPos + transform.position) / 2).x, 
                            ((lastCashPos + transform.position) / 2).y + 1.5f, // Add some height to the cash
                            ((lastCashPos + transform.position) / 2).z), 
                        transform.position 
                    }, 0.2f).OnComplete(() => Destroy(cash));

                VibrationManager.Vibrate(50);
            }
            yield return new WaitForSeconds(delayPerExchange);
        }
    }
}
