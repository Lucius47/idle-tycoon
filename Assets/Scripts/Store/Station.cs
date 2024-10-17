using System;
using UnityEngine;

public class Station : MonoBehaviour
{
    [SerializeField] private Transform[] customerWaitPoints;
    public StationType stationType;
    private WaitPoint[] waitPoints;

    #region Unity Callbacks
    private void Awake()
    {
        waitPoints = new WaitPoint[customerWaitPoints.Length];

        for (int i = 0; i < waitPoints.Length; i++)
        {
            waitPoints[i] = new()
            {
                Transform = customerWaitPoints[i]
            };
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Customer customer))
        {
            for (int i = 0; i < waitPoints.Length; i++)
            {
                if (waitPoints[i].CustomerTransform == customer.transform)
                {
                    waitPoints[i].IsOccupied = true;
                    break;
                }
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Customer customer))
        {
            for (int i = 0; i < waitPoints.Length; i++)
            {
                if (waitPoints[i].CustomerTransform == customer.transform)
                {
                    waitPoints[i].IsOccupied = false;
                    waitPoints[i].CustomerTransform = null;
                    break;
                }
            }
        }
    }
    #endregion

    internal Transform GetWaitPoint(Customer customer)
    {
        for (int i = 0; i < waitPoints.Length; i++)
        {
            if (!waitPoints[i].CustomerTransform)
            {
                waitPoints[i].CustomerTransform = customer.transform;
                return waitPoints[i].Transform;
            }
            else if (waitPoints[i].CustomerTransform == customer.transform)
            {
                return waitPoints[i].Transform;
            }
        }

        return null;
    }

    internal bool IsAnySpotAvailable()
    {
        for (int i = 0; i < waitPoints.Length; i++)
        {
            if (!waitPoints[i].IsOccupied) return true;
        }
        return false;
    }

    [SerializeField] private Transform workerPosition;

    internal virtual Transform GetWorkerPosition()
    {
        if (workerPosition)
        {
            return workerPosition;
        }
        else
        {
            Debug.LogError("Worker position not set for " + gameObject.name);
            return transform;
        }
    }

    internal bool IsCustomerPresent(Customer customer)
    {
        for (int i = 0; i < waitPoints.Length; i++)
        {
            if (waitPoints[i].CustomerTransform == customer.transform
                && waitPoints[i].IsOccupied
                ) return true;
        }
        return false;
    }

    internal virtual bool HasWorker()
    {
        return false;
    }

    public enum StationType
    {
        ShoesShelf,
        ShirtsStand,
        Counter,
    }

    class WaitPoint
    {
        public Transform Transform { get; set; }
        public Transform CustomerTransform { get; set; }
        public bool IsOccupied { get; set; }
    }
}
