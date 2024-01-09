//using System.Threading.Tasks;
using TMPro;
using UnityEngine;


public class StationBase : MonoBehaviour
{
    [SerializeField] private GameObject stationPrefab;
    [SerializeField] private Transform stationOrigin;
    [SerializeField] private UnityEngine.UI.Image fillImage;

    //private bool isPlayerInTrigger = false;

    [SerializeField] private int cost = 100;
    public int remainingCost;
    [SerializeField] private TextMeshProUGUI costText;

    private void Start()
    {
        costText.text = $"${cost}";
        remainingCost = cost;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        isPlayerInTrigger = true;
    //        BuildStation();
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        isPlayerInTrigger = false;
    //    }
    //}

    //private async void BuildStation()
    //{
    //    while (remainingCost > 0 && PlayerStats.Cash > 0 && isPlayerInTrigger)
    //    {
    //        await Task.Delay(50);
    //        Handheld.Vibrate();
    //        // decrement from player cash while cost is greater than 0, slowly fill the image
    //        remainingCost--;
    //        costText.text = $"${remainingCost}";
    //        PlayerStats.Cash--;
    //        fillImage.fillAmount = 1 - (float)remainingCost / (float)cost;

    //        if (remainingCost == 0)
    //        {
    //            // instantiate the station prefab
    //            Instantiate(stationPrefab, stationOrigin.position, stationOrigin.rotation);
    //            // destroy this game object
    //            Destroy(gameObject);
    //        }
    //    }
    //}

    public int BuildStation()
    {
        if (remainingCost > 0)
        {
            remainingCost--;
            costText.text = $"${remainingCost}";
            fillImage.fillAmount = 1 - (float)remainingCost / (float)cost;

            if (remainingCost == 0)
            {
                Instantiate(stationPrefab, stationOrigin.position, stationOrigin.rotation);
                Destroy(gameObject);
            }

            return 1;
        }
        else return 0;
    }
}
