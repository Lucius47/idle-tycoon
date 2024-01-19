using UnityEngine;
using DG.Tweening;

public class ShadowTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.TryGetComponent(out PlayerMovement player))
        {
            transform.DOScale(1.2f, 0.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
        {
            transform.DOScale(1f, 0.5f);
        }
    }
}
