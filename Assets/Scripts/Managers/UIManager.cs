using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    internal static UIManager Instance;

    [SerializeField] private TextMeshProUGUI cashText;
    [SerializeField] private GameObject upgradesPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        cashText.text = $"${PlayerStats.Cash}";
    }

    internal void ShowUpgradesPanel(bool state)
    {
        upgradesPanel.SetActive(state);
    }
}
