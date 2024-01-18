using UnityEngine;

public class Counter : MonoBehaviour
{
    private StoreWorker currentWorker;
    private bool isWorkerPresent = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isWorkerPresent)
        {
            if (other.TryGetComponent(out StoreWorker worker))
            {
                currentWorker = worker;
                isWorkerPresent = true;
            }
        }

        if (other.TryGetComponent(out Customer customer))
        {
            // perform customer logic
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isWorkerPresent)
        {
            if (other.TryGetComponent(out StoreWorker worker))
            {
                if (currentWorker == worker)
                {
                    currentWorker = null;
                    isWorkerPresent = false;
                }
            }
        }
    }

    internal bool RequestCheckout()
    {
        if (isWorkerPresent && currentWorker != null)
        {
            PlayerStats.Cash += 10; ;
            return true;
        }
        return false;
    }
}
