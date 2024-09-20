using UnityEngine;

public class ItemMono : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private int[] materialIndices;

    [SerializeReference] private GameObject _model1;
    [SerializeReference] private GameObject _model2;

    private Color mainColor;

    public void SetUp()
    {
        mainColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials[materialIndices[i]].color = mainColor;
        }
    }

    internal void Exchange()
    {
        // display different models based on if the item is in the shelf or with a character.
        if (_model1 == null || _model2 == null) return;
        _model1.SetActive(!_model1.activeSelf);
        _model2.SetActive(!_model2.activeSelf);
    }
}
