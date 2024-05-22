using UnityEngine;

public class ItemMono : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private int[] materialIndices;

    private Color mainColor;

    public void SetUp()
    {
        mainColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials[materialIndices[i]].color = mainColor;
        }
    }
}
