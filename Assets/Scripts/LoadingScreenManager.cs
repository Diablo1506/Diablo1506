using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager Instance;
    public GameObject m_LoadingScreenObject;
    public Slider ProgressBar;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void SwitchToScene(int id)
    {
        m_LoadingScreenObject.SetActive(true);
        ProgressBar.value = 1f;
        StartCoroutine(SwitchToSceneAsyc(id));
    }

    IEnumerator SwitchToSceneAsyc(int id)
    {
        AsyncOperation asyncload = SceneManager.LoadSceneAsync(id);
        while (!asyncload.isDone)
        {
            ProgressBar.value = asyncload.progress;
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        m_LoadingScreenObject.SetActive(false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
