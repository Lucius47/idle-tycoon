using UnityEngine;

[CreateAssetMenu()]
public class Items : ScriptableObject
{
    [SerializeField] private GameObject shoes;
    [SerializeField] private GameObject clothes;
    [SerializeField] private GameObject hats;

    public GameObject boxPrefab;
    public GameObject cashPrefab;

    [Space]
    [SerializeField] private StationSpawner shoeShelfPrefab;
    [SerializeField] private StationSpawner counterPrefab;

    #region Instance
    private static Items _instance;

    public static Items Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<Items>("Items") as Items;
            }
            return _instance;
        }
    }
    #endregion


    public enum ItemType
    {
        //None,
        Shoes,
        Clothes,
        Hats
    }

    public GameObject GetItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Shoes:
                return shoes;
            case ItemType.Clothes:
                return clothes;
            case ItemType.Hats:
                return hats;
            default:
                return null;
        }
    }

    public StationSpawner GetStation(Station.StationType stationType)
    {
        return stationType switch
        {
            Station.StationType.Shelf => shoeShelfPrefab,
            Station.StationType.Counter => counterPrefab,
            _ => null,
        };
    }
}
