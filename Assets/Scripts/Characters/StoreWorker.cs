using UnityEngine;

public class StoreWorker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out GenericItemsHolder holder))
        {
            if (TryGetComponent<GenericItemsHolder>(out var myHolder))
            {
                if (myHolder.numOfItems == 0)
                {
                    myHolder.itemType = holder.itemType;
                }
            }
        }
    }
}
