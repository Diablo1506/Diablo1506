using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManuManager : MonoBehaviour
{
    public MonoBehaviour optionsMenu; // Replace with MonoBehaviour or the correct type if OptionsMenu is defined elsewhere

    // Use this for initialization
    void Start()
    {
        
    }

     void Update()
    {
     if (Input.GetKeyDown(KeyCode.Escape))
     {
         optionsMenu.gameObject.SetActive(!optionsMenu.gameObject.activeSelf);
     }
     
    }
}
