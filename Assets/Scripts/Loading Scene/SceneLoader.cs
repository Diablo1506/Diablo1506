using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
public class SceneLoader : MonoBehaviour
{

        public GameObject Gamelevel;
    public Slider loadingBar;
    public void LoadScene(int LevelIndex)
    {
     StartCoroutine(LoadSceneAsynchronously(LevelIndex)); 
    }
    IEnumerator LoadSceneAsynchronously(int LevelIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(LevelIndex);
        Gamelevel.SetActive(true);
        while (!operation.isDone)
        {
            loadingBar.value = operation.progress;
            yield return null;
        }
    }

    void onEnable()
    {
        Gamelevel.SetActive(false);
    }
  
}
