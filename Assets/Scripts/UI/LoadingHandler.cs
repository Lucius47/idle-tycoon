using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingHandler : MonoBehaviour
{
    [SerializeField] private Slider loadingProgressSlider;
    private AsyncOperation loadingOperation;

    private async void Start()
    {
        loadingProgressSlider.value = 0;
        loadingOperation = SceneManager.LoadSceneAsync(1);
        loadingOperation.allowSceneActivation = false;

        while (loadingProgressSlider.value < 0.6f && Application.isPlaying)
        {
            loadingProgressSlider.value += 0.01f;
            await Task.Delay(50);
        }
    }

    private void Update()
    {
        if (loadingProgressSlider.value > 0.6f)
        {
            loadingProgressSlider.value = loadingOperation.progress;

            if (loadingOperation.progress >= 0.9f)
            {
                loadingProgressSlider.value = 1;
                loadingOperation.allowSceneActivation = true;
            }
        }
    }
}
