using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(GenericItemsHolder))]
public class ItemsExchanger : MonoBehaviour
{
    [SerializeField] private float delayPerExchange = 0.1f;

    GenericItemsHolder otherHolder;
    private Coroutine exchangeItemsCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out GenericItemsHolder holder))
        {
            otherHolder = holder;
            if (otherHolder.supplier)
            {
                exchangeItemsCoroutine = StartCoroutine(ExchangeItems(otherHolder, GetComponent<GenericItemsHolder>()));
            }
            else
            {
                exchangeItemsCoroutine = StartCoroutine(ExchangeItems(GetComponent<GenericItemsHolder>(), otherHolder));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out GenericItemsHolder holder))
        {
            if (otherHolder == holder)
            {
                otherHolder = null;
                StopCoroutine(exchangeItemsCoroutine);
            }
        }
    }

    private IEnumerator ExchangeItems(GenericItemsHolder supplier, GenericItemsHolder receiver)
    {
        while (otherHolder)
        {
            if (supplier.numOfItems > 0)
            {
                if (receiver.AddItem(supplier.Type))
                {
                    supplier.RemoveItem(supplier.Type);
                    //Handheld.Vibrate();
                    VibrationManager.Vibrate(50);
                }
            }
            yield return new WaitForSeconds(delayPerExchange);
        }
    }
}
