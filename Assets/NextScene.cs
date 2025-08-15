using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
  void  OnEnable()
  {
    // only specify the sceneName or sceneBuildIndex to load the scene with the single scene mode
    
        SceneManager.LoadScene("Boxing arena - pavilion", LoadSceneMode.Additive);
  }

}

  