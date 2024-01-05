using System.Threading.Tasks;
using TMPro;
using UnityEngine;


public class BuildShelf : MonoBehaviour
{
    [SerializeField] private GameObject shelfPrefab;
    [SerializeField] private Transform shelfOrigin;
    [SerializeField] private UnityEngine.UI.Image fillImage;

    private bool isPlayerInTrigger = false;

    [SerializeField] private int cost = 100;
    [SerializeField] private TextMeshProUGUI costText;

    private void Start()
    {
        costText.text = $"${cost}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            BuildShoeShelf();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }

    private async void BuildShoeShelf()
    {
        while (cost > 0 && PlayerStats.Cash > 0 && isPlayerInTrigger)
        {
            await Task.Delay(100);
            // decrement from player cash while cost is greater than 0, slowly fill the image
            cost--;
            costText.text = $"${cost}";
            PlayerStats.Cash--;
            fillImage.fillAmount = 1 - (float)cost / 100;

            if (cost == 0)
            {
                // instantiate the shelf prefab
                Instantiate(shelfPrefab, shelfOrigin.position, shelfOrigin.rotation);
                // destroy this game object
                Destroy(gameObject);
            }
        }
    }
}
