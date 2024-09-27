using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    [SerializeField] private float _openTime = 0.3f;

    [SerializeField] private Transform _door1;
    [SerializeField] private Transform _door2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out StoreWorker worker))
        {
            _door1.DORotate(new Vector3(0, -90, 0), _openTime);
            _door2.DORotate(new Vector3(0, 90, 0), _openTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out StoreWorker worker))
        {
            _door1.DORotate(new Vector3(0, 0, 0), _openTime);
            _door2.DORotate(new Vector3(0, 0, 0), _openTime);
        }
    }
}
