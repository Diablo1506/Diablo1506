using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoader : MonoBehaviour
{
    [Header("Menu Screen")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject MenuControl;

    [Header("Slider")]
    [SerializeField] private Slider loadingSlider;

    public void LooadLevelBtn(string LevelToLoad)
    {
        MenuControl.SetActive(false);
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelAsync(LevelToLoad));

       
    }

    IEnumerator LoadLevelAsync(string LevelToLoad)
    {
        AsyncOperation loadoperation = SceneManager.LoadSceneAsync(LevelToLoad);

        while (!loadoperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadoperation.progress / 0.9f);
            loadingSlider.value = progressValue;

            yield return null;
        }
    }
}
