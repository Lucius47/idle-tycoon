using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary>
/// 1. Keep track of all the stations that are built and the items in those stations.
/// 2. Save this information to a file.
/// 3. Load this information from a file if it's available.
/// </summary>
public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance { get; private set; }
    List<Items.ItemType> stations = new List<Items.ItemType>();

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

        Load();

        foreach (var station in stations)
        {
            Instantiate(Items.Instance.GetItem(station));
        }
    }

    public void AddStation(GameObject staion)
    {
        stations.Add(staion.GetComponent<GenericItemsHolder>().Type);
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void Save()
    {
        SerializableInventory serializableDictionary = new SerializableInventory();
        foreach (var station in stations)
        {
            serializableDictionary.keys.Add(station);
        }

        System.IO.File.WriteAllText(Application.dataPath + "/" + "StoreData.json", JsonUtility.ToJson(serializableDictionary));
        Debug.LogError(Application.dataPath + "/" + "StoreData.json");
    }

    private void Load()
    {
        if (System.IO.File.Exists(Application.dataPath + "/" + "StoreData.json"))
        {
            string json = System.IO.File.ReadAllText(Application.dataPath + "/" + "StoreData.json");
            SerializableInventory serializableDictionary = JsonUtility.FromJson<SerializableInventory>(json);

            for (int i = 0; i < serializableDictionary.keys.Count; i++)
            {
                stations.Add(serializableDictionary.keys[i]);
            }
        }
        else
        {
            Debug.LogError("Cannot find file.");
        }
    }
}

[System.Serializable]
public class SerializableInventory
{
    public List<Items.ItemType> keys = new List<Items.ItemType>();
}