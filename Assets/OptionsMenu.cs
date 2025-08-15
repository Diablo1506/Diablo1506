using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuUI : MonoBehaviour
{


    public GameObject PausePanel; // Reference to the Pause Panel

    void Update()
    {

    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void Resume()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1; // Resume the game
    }
}
