using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class BGRotator : MonoBehaviour
{
    RectTransform rectTransform;
    [SerializeField] private float rotationSpeed = 50;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        rectTransform.Rotate(-Vector3.forward, Time.deltaTime * rotationSpeed);
    }
}
