using UnityEngine;

public class StoreUpgrades : MonoBehaviour
{
    [SerializeField] private GameObject npcCashier;
    [SerializeField] private GameObject npcStocker;
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

    public void HireCashier()
    {
        if (PlayerStats.Cash < 50)
        {
            Debug.LogError("You need 50$");
            return;
        }

        SpawnCashier();
        StoreManager.Instance.AddCashier();
        PlayerStats.Cash -= 50;
    }


    internal void SpawnCashier()
    {
        Instantiate(npcCashier);
    }

    public void HireStocker()
    {
        if (PlayerStats.Cash < 50)
        {
            Debug.LogError("You need 50$");
            return;
        }

        SpawnStocker();
        StoreManager.Instance.AddStocker();
        PlayerStats.Cash -= 50;
    }


    internal void SpawnStocker()
    {
        Instantiate(npcStocker);
    }
}
