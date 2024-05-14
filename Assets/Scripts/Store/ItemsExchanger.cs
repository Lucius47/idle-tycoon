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
                if (receiver.AddItem(supplier.Type, out Transform addedTrans))
                {
                    supplier.RemoveItem(supplier.Type, out Transform removedTrans);
                    VibrationManager.Vibrate(50);

                    // play transfer animation. Spawn an item. Move it to the other holder. Destroy it.
                    var item = Instantiate(Items.Instance.GetItem(supplier.Type), removedTrans.position, removedTrans.rotation);

                    item.transform.DOPath(
                    new Vector3[] {
                        removedTrans.position,
                        new Vector3(((removedTrans.position + addedTrans.position) / 2).x,
                            ((removedTrans.position + addedTrans.position) / 2).y + 1.5f,
                            ((removedTrans.position + addedTrans.position) / 2).z),
                        addedTrans.position
                    }, 0.2f).OnComplete(() => Destroy(item));
                }
            }
            yield return new WaitForSeconds(delayPerExchange);
        }
    }
}
