using UnityEngine;

public class Counter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out StoreWorker worker))
        {
            // perform cashier logic
        }

        if (other.TryGetComponent(out Customer customer))
        {
            // perform customer logic
        }
    }
}
