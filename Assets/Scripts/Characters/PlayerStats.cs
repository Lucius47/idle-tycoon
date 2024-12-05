
using UnityEngine;

public static class PlayerStats
{
    public static int Cash { get { return PlayerPrefs.GetInt("Cash", 0); } set { PlayerPrefs.SetInt("Cash", value); } }
}
