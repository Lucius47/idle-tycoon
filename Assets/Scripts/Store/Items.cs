using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Items : ScriptableObject
{
    [SerializeField] private GameObject shoesPrefab;
    //private HashSet<Item> sellableItems = new HashSet<Item>();
    [SerializeField] private Item clothes;
    [SerializeField] private Item hats;

    public GameObject boxPrefab;
    public GameObject cashPrefab;

    [Space]
    [SerializeField] private StationSpawner shoeShelfPrefab;
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
        Clothes,
        Hats
    }

    public GameObject GetItem(ItemType type)
    {
        return shoesPrefab;
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

    public CashPile GetCashPile()
    {
        return cashPilePrefab;
    }
}
