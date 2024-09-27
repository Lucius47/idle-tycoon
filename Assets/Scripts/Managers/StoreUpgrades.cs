using UnityEngine;

public class StoreUpgrades : MonoBehaviour
{
    [SerializeField] private GameObject npcStoreWorker;
    internal static StoreUpgrades Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.ShowUpgradesPanel(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.ShowUpgradesPanel(false);
        }
    }

    public void HireWorker()
    {
        if (PlayerStats.Cash < 50)
        {
            Debug.LogError("You need 50$");
            return;
        }

        SpawnWorker();
        StoreManager.Instance.AddWorker();
        PlayerStats.Cash -= 50;
    }


    internal void SpawnWorker()
    {
        Instantiate(npcStoreWorker);
    }
}
