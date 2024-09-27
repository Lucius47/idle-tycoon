using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class BGRotator : MonoBehaviour
{
    RectTransform rectTransform;
    [SerializeField] private float rotationSpeed = 50;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        var screenSize = new Vector2(Screen.width, Screen.height);
        var distanceToCorner = Mathf.Sqrt(Mathf.Pow(screenSize.x/2, 2) + Mathf.Pow(screenSize.y / 2, 2));

        transform.parent.GetComponent<CanvasScaler>().referenceResolution = screenSize;

        //Debug.Log(screenSize);
        //Debug.Log(Mathf.Pow(screenSize.x / 2, 2) + ", " + Mathf.Pow(screenSize.y / 2, 2));
        //Debug.Log(distanceToCorner * 2);

        rectTransform.sizeDelta = new Vector2(distanceToCorner * 2, distanceToCorner * 2);
    }

    private void LateUpdate()
    {
        rectTransform.Rotate(-Vector3.forward, Time.deltaTime * rotationSpeed);
    }
}
