using DG.Tweening;
using TMPro;
using UnityEngine;


public class StationBase : MonoBehaviour
{
    public string stationName;
    [SerializeField] private Station station;
    [SerializeField] private Transform stationOrigin;
    [SerializeField] private UnityEngine.UI.Image fillImage;

    public int cost = 100;
    public int remainingCost;
    [SerializeField] private TextMeshProUGUI costText;

    private void Start()
    {
        station.gameObject.SetActive(false);

        costText.text = $"${cost}";
        remainingCost = cost;
    }

    public int BuildStation()
    {
        if (remainingCost > 0)
        {
            remainingCost--;
            costText.text = $"${remainingCost}";
            fillImage.fillAmount = 1 - (float)remainingCost / (float)cost;

            if (remainingCost == 0)
            {
                station.gameObject.SetActive(true);

                station.transform.localScale = Vector3.one * 0.1f;
                station.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);

                StoreManager.Instance.AddStation(station, transform.parent.position, transform.parent.rotation, stationName);
                Destroy(gameObject);
            }

            return 1;
        }
        else return 0;
    }
}
