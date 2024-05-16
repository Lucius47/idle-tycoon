using DG.Tweening;
using UnityEngine;

public class CashPile : MonoBehaviour
{
    public int Cash { get; private set; }

    [SerializeField] private GameObject cashPrefab;
    [SerializeField] private Transform cashOrigin;

    [SerializeField] private Vector2 cashPileSize = new Vector2(4, 6);
    [SerializeField] private Vector3 gap = new Vector3(0.2f, 0.2f, 0.5f);

    [SerializeField] private bool DestroyOnEmpty = false;

    public int xxx = 5;

    private void Start()
    {
        AddCashInstantly(xxx);
    }

    public int GetCash(out Vector3 lastCashPos)
    {
        if (Cash > 0)
        {
            lastCashPos = cashOrigin.GetChild(cashOrigin.childCount - 1).position;

            Cash--;
            Destroy(cashOrigin.GetChild(cashOrigin.childCount - 1).gameObject);

            if (Cash == 0 && DestroyOnEmpty)
            {
                Destroy(gameObject);
            }

            return 1;
        }

        else
        {
            lastCashPos = Vector3.zero;
            return 0;
        }
    }

    public void AddCashInstantly(int amount)
    {
        int x = 0;
        int y = 0;
        int z = 0;
        for (int i = 0; i < amount; i++)
        {
            GameObject cash = Instantiate(cashPrefab, transform.position, Quaternion.identity, cashOrigin);
            var cashPos = cashOrigin.position + new Vector3(x * gap.x, z * gap.y, y * gap.z);

            cash.transform.DOPath(
                    new Vector3[] {
                        cash.transform.position,
                        new Vector3(((cash.transform.position + cashPos) / 2).x,
                            ((cash.transform.position + cashPos) / 2).y + 1.5f, // Add some height to the cash
                            ((cash.transform.position + cashPos) / 2).z),
                        cashPos
                    }, 0.2f);

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
}
