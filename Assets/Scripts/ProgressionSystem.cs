using UnityEngine;

public class ProgressionSystem : MonoBehaviour
{
    // player can build stations.
    // keep track of built stations.
    // keep track of all items in the stations.
    // restore the stations when the player loads the game.

    public static ProgressionSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // load the progression data
    }

    public void SaveProgression()
    {
        // save the progression data

    }
}
