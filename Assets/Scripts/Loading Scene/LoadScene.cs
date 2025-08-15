using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LoadScene : MonoBehaviour
{
    void  OnEnable()
  {
    // only specify the sceneName or sceneBuildIndex to load the scene with the single scene mode
    
        SceneManager.LoadScene("Boxing_Gym_Demo", LoadSceneMode.Additive);
  }
}
