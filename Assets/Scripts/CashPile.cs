//using System.Threading.Tasks;
using UnityEngine;

public class CashPile : MonoBehaviour
{
    public int Cash { get; private set; }

    [SerializeField] private GameObject cashPrefab;
    [SerializeField] private Transform cashOrigin;

    [SerializeField] private Vector2 cashPileSize = new Vector2(4, 6);
    [SerializeField] private Vector3 gap = new Vector3(0.2f, 0.2f, 0.5f);

    //private bool isPlayerInTrigger = false;

    public int xxx = 5;

    private void Start()
    {
        AddCashInstantly(xxx);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        isPlayerInTrigger = true;
    //        RemoveCash();
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        isPlayerInTrigger = false;
    //    }
    //}

    public int GetCash()
    {
        if (Cash > 0)
        {
            Cash--;
            Destroy(cashOrigin.GetChild(cashOrigin.childCount - 1).gameObject);

            if (Cash == 0)
            {
                Destroy(gameObject);
            }

            return 1;
        }
        else return 0;
    }

    //private async void RemoveCash()
    //{
    //    while (Cash > 0 && isPlayerInTrigger)
    //    {
    //        await Task.Delay(100);
    //        Cash--;
    //        PlayerStats.Cash++;
    //        Destroy(cashOrigin.GetChild(cashOrigin.childCount - 1).gameObject);

    //        if (Cash == 0)
    //        {
    //            Destroy(gameObject);
    //        }
    //    }
    //}

    public void AddCashInstantly(int amount)
    {
        int x = 0;
        int y = 0;
        int z = 0;
        for (int i = 0; i < amount; i++)
        {
            GameObject cash = Instantiate(cashPrefab, transform.position, Quaternion.identity, cashOrigin);
            cash.transform.position = cashOrigin.position + new Vector3(x * gap.x, z * gap.y, y * gap.z);
            Cash++;

            x++;

            if (x == cashPileSize.x)
            {
                x = 0;
                y++;

                if (y == cashPileSize.y)
                {
                    y = 0;
                    z++;
                }
            }
        }
    }

    //public async void AddCash(int amount)
    //{
    //    int x = 0;
    //    int y = 0;
    //    int z = 0;
    //    for (int i = 0; i < amount; i++)
    //    {
    //        await Task.Delay(100);
    //        GameObject cash = Instantiate(cashPrefab, transform.position, Quaternion.identity, cashOrigin);
    //        cash.transform.position = cashOrigin.position + new Vector3(x * gap.x, z * gap.y, y * gap.z);
    //        Cash++;

    //        x++;

    //        if (x == cashPileSize.x)
    //        {
    //            x = 0;
    //            y++;

    //            if (y == cashPileSize.y)
    //            {
    //                y = 0;
    //                z++;
    //            }
    //        }
    //        //cash.transform.position += new Vector3(Mathf.Cos(i * 2 * Mathf.PI / amount), ypos, Mathf.Sin(i * 2 * Mathf.PI / amount)) * 0.5f;
    //    }
        
    //}
}
