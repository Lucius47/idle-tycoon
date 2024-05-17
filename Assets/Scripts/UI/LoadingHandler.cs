using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingHandler : MonoBehaviour
{
    [SerializeField] private Slider loadingProgressSlider;
    private AsyncOperation loadingOperation;

    private void Start()
    {
        loadingOperation = SceneManager.LoadSceneAsync(1);
        loadingOperation.allowSceneActivation = false;
    }

    private void OnSceneLoaded(AsyncOperation operation)
    {
        Debug.Log("Scene loaded");
    }

    private void Update()
    {
        loadingProgressSlider.value = loadingOperation.progress;
        if (loadingOperation.progress >= 0.9f)
        {
            loadingProgressSlider.value = 1;
            loadingOperation.allowSceneActivation = true;
        }
    }
}
