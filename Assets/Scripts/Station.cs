using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    [SerializeField] private Transform[] customerWaitPoints;
    private bool[] waitPointOccupiedState;
    public StationType stationType;
    [SerializeField] private int maxCustomers = 5;
    private int currentCustomers = 0;
    private List<Customer> customers = new List<Customer>();

    public enum StationType
    {
        Shelf,
        Counter,
    }

    //internal Transform GetWaitPoint()
    //{
    //    for (int i = 0; i < customerWaitPoints.Length; i++)
    //    {
    //        if (!waitPointOccupiedState[i])
    //        {
    //            waitPointOccupiedState[i] = true;
    //            return customerWaitPoints[i];
    //        }
    //    }

    //    return null;
    //}

    internal Transform GetWaitPoint(string nickName = null)
    {
        if (currentCustomers < maxCustomers && !waitPointOccupiedState[currentCustomers])
        {
            waitPointOccupiedState[currentCustomers] = true;
            return customerWaitPoints[currentCustomers];
        }
        return null;
    }

    internal bool IsAnySpotAvailable()
    {
        return currentCustomers < maxCustomers;
        //for (int i = 0; i < waitPointOccupiedState.Length; i++)
        //{
        //    if (!waitPointOccupiedState[i]) return true;
        //}
        //return false;
    }

    private void Awake()
    {
        waitPointOccupiedState = new bool[customerWaitPoints.Length];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentCustomers < maxCustomers)
        {
            if (other.TryGetComponent(out Customer customer))
            {
                currentCustomers++;
                customers.Add(customer);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentCustomers > 0)
        {
            if (other.TryGetComponent(out Customer customer))
            {
                waitPointOccupiedState[currentCustomers - 1] = false;
                currentCustomers--;
                customers.Remove(customer); // TODO
            }
        }
    }

    internal bool IsCustomerPresent(Customer customer)
    {
        return customers.Contains(customer);
    }
}
