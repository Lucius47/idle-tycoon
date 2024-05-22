using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(GenericItemsHolder))]
public class ItemsExchanger : MonoBehaviour
{
    [SerializeField] private float delayPerExchange = 0.1f;
    [SerializeField] private float animationTime = 0.2f;

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
                var item = supplier.RemoveItem(supplier.itemType);

                if (item != null)
                {
                    if (receiver.AddItem(item/*, out Transform _newPos*/))
                    {
                        VibrationManager.Vibrate(50);

                        //item.itemTransform.DOPath(
                        //new Vector3[] {
                        //    item.itemTransform.position,
                        //    new Vector3(((item.itemTransform.position + _newPos.position) / 2).x,
                        //        ((item.itemTransform.position + _newPos.position) / 2).y + 1.5f,
                        //        ((item.itemTransform.position + _newPos.position) / 2).z),
                        //    _newPos.position
                        //}
                        //, animationTime)/*.OnComplete(() => Destroy(item.itemTransform.gameObject))*/;
                    }
                }
            }
            yield return new WaitForSeconds(delayPerExchange);
        }
    }
}
