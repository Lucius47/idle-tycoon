using UnityEngine;

public class Station : MonoBehaviour
{
    [SerializeField] private Transform[] customerWaitPoints;
    public StationType stationType;

    private WaitPoint[] waitPoints;

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

    private void Awake()
    {
        waitPoints = new WaitPoint[customerWaitPoints.Length];

        for (int i = 0; i < waitPoints.Length; i++)
        {
            waitPoints[i] = new WaitPoint();
            waitPoints[i].Transform = customerWaitPoints[i];
        }
    }

    private void OnTriggerEnter(Collider other)
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

    private void OnTriggerExit(Collider other)
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

    public enum StationType
    {
        Shelf,
        Counter,
    }

    class WaitPoint
    {
        public Transform Transform { get; set; }
        public Transform CustomerTransform { get; set; }
        public bool IsOccupied { get; set; }
    }
}
