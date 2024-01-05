using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(GenericItemsHolder))]
public class ItemsExchanger : MonoBehaviour
{
    [SerializeField] private float delayPerExchange = 0.1f;

    GenericItemsHolder otherHolder;

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("Trigger enter");
        if (other.TryGetComponent(out GenericItemsHolder holder))
        {
            otherHolder = holder;
            if (otherHolder.supplier)
            {
                StartCoroutine(ExchangeItems(otherHolder, GetComponent<GenericItemsHolder>()));
            }
            else
            {
                StartCoroutine(ExchangeItems(GetComponent<GenericItemsHolder>(), otherHolder));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.LogError("Trigger exit");
        if (other.TryGetComponent(out GenericItemsHolder holder))
        {
            if (otherHolder == holder)
            {
                otherHolder = null;
                StopCoroutine(ExchangeItems(GetComponent<GenericItemsHolder>(), otherHolder));
                StopCoroutine(ExchangeItems(otherHolder, GetComponent<GenericItemsHolder>()));
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
                }
            }
            yield return new WaitForSeconds(delayPerExchange);
        }
    }
}
