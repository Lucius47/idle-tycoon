using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stores", menuName = "Stores/New Stores Item")]
public class Stores : ScriptableObject
{
    #region Instance

    private static Stores _instance;

    public static Stores Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<Stores>("Stores");
            }
            return _instance;
        }
    }

    #endregion

    public List<StoreX> stores = new();
}
