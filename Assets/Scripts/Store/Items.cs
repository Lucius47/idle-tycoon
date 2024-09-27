using UnityEngine;

[CreateAssetMenu()]
public class Items : ScriptableObject
{
    [SerializeField] private ItemMono shoesPrefab;
    [SerializeField] private ItemMono shirtPrefab;

    public GameObject boxPrefab;
    public GameObject cashPrefab;

    [Space]
    [SerializeField] private StationSpawner shoeShelfPrefab;
    [SerializeField] private StationSpawner shirtsStandPrefab;
    [SerializeField] private StationSpawner counterPrefab;
    [SerializeField] private CashPile cashPilePrefab;

    #region Instance
    private static Items _instance;

    public static Items Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<Items>("Items") as Items;

                //foreach (var item in _instance.itemsArray)
                //{
                //    _instance.sellableItems.Add(item);
                //}
            }
            return _instance;
        }
    }
    #endregion


    public enum ItemType
    {
        //None,
        Shoes,
        Shirt,
    }

    public ItemMono GetItem(ItemType type)
    {
        return type switch
        {
            ItemType.Shoes => shoesPrefab,
            ItemType.Shirt => shirtPrefab,
            _ => null
        };
    }

    public StationSpawner GetStation(Station.StationType stationType)
    {
        return stationType switch
        {
            Station.StationType.ShoesShelf => shoeShelfPrefab,
            Station.StationType.ShirtsStand => shirtsStandPrefab,
            Station.StationType.Counter => counterPrefab,
            _ => null,
        };
    }

    public CashPile GetCashPile()
    {
        return cashPilePrefab;
    }
}
