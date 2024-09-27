using UnityEngine;

public class Counter : Station
{
    [SerializeField] private CashPile cashPile; // This is the cash pile that the counter is associated with

    private StoreWorker currentWorker;
    private bool isWorkerPresent = false;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (!isWorkerPresent)
        {
            if (other.TryGetComponent(out StoreWorker worker))
            {
                currentWorker = worker;
                isWorkerPresent = true;
            }
        }

        //if (other.TryGetComponent(out Customer customer))
        //{
        //    // perform customer logic
        //}
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
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
            cashPile.AddCashInstantly(10);
            return true;
        }
        return false;
    }

    internal override bool HasWorker()
    {
        return isWorkerPresent;
    }
}
