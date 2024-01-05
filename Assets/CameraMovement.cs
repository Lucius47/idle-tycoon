using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float smoothSpeed = 5f;

    private void LateUpdate()
    {
        // move the camera to the player's position
        transform.position = Vector3.Lerp(transform.position, playerTransform.position, smoothSpeed * Time.deltaTime);
    }
}
