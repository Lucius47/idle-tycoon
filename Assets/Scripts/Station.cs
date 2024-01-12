using UnityEngine;

public class Station : MonoBehaviour
{
    public Transform customerWaitPoint;
    public StationType stationType;

    public enum StationType
    {
        Shelf,
        Counter,
    }
}
