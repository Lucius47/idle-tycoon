using DG.Tweening;
using UnityEngine;

public class AnimateOnInteraction : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (target != null)
                target.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
            else
                transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (target != null)
                target.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
            else
                transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        }
    }
}
