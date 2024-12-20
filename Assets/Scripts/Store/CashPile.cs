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

    //public int amount = 5;

    public void SetUp(int _amount, bool _destroyOnEmpty)
    {
        DestroyOnEmpty = _destroyOnEmpty;
        AddCashInstantly(_amount);
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

            x--;
            if (x < 0)
            {
                x = (int)cashPileSize.x - 1;
                z--;

                if (z < 0)
                {
                    z = (int)cashPileSize.y - 1;
                    y--;
                }
            }

            return 1;
        }

        else
        {
            lastCashPos = Vector3.zero;
            return 0;
        }
    }

    int x = 0;
    int z = 0;
    int y = 0;

    public void AddCashInstantly(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject cash = Instantiate(cashPrefab, transform.position, Quaternion.identity, cashOrigin);
            var cashPos = cashOrigin.position + new Vector3(x * gap.x, y * gap.y, z * gap.z);

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
                z++;

                if (z == cashPileSize.y)
                {
                    z = 0;
                    y++;
                }
            }
        }
    }
}
