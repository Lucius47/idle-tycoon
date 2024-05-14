using DG.Tweening;
using TMPro;
using UnityEngine;


public class StationBase : MonoBehaviour
{
    [SerializeField] private Station stationPrefab;
    [SerializeField] private Transform stationOrigin;
    [SerializeField] private UnityEngine.UI.Image fillImage;

    [SerializeField] private int cost = 100;
    public int remainingCost;
    [SerializeField] private TextMeshProUGUI costText;

    private void Start()
    {
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
                var newStation = Instantiate(stationPrefab, stationOrigin.position, stationOrigin.rotation);

                newStation.transform.localScale = Vector3.one * 0.1f;
                newStation.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);

                StoreManager.Instance.AddStation(newStation, newStation.transform.position, newStation.transform.rotation);
                Destroy(gameObject);
            }

            return 1;
        }
        else return 0;
    }
}
