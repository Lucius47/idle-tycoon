using UnityEngine;

[CreateAssetMenu()]
public class Items : ScriptableObject
{
    [SerializeField] private GameObject shoes;
    [SerializeField] private GameObject clothes;
    [SerializeField] private GameObject hats;

    public GameObject boxPrefab;

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


    public enum ItemType
    {
        //None,
        Shoes,
        Clothes,
        Hats,
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
}
