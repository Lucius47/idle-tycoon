using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cashText;
    
    private void Update()
    {
        cashText.text = $"${PlayerStats.Cash}";
    }
}
