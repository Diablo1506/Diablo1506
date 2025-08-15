using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused;
    
    


    void Start()
    {
        pauseMenu.SetActive(false); // Ensure the pause menu is hidden at the start

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Check if the Escape key is pressed
        {
            if(isPaused)
            {
                ResumeGame(); // Resume the game if it's paused
            }
            else
            {
                PauseGame(); // Pause the game if it's not paused
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true); // Show the pause menu
        Time.timeScale = 0f; // Pause the game
        isPaused = true; // Set the paused state to true
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f; // Resume the game
        isPaused = false; // Set the paused state to false
    }
    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Boxing_Gym_Demo");
    }

    public void GotoMainMenu()
    {
        Time.timeScale = 1f; // Ensure the game is unpaused before loading the main menu
        SceneManager.LoadScene("Demo1");
    }

    public void GotoTrainigGym()
    {
        Time.timeScale = 1f; // Ensure the game is unpaused before loading the training gym
        SceneManager.LoadScene("TrainingGym");
    }




}
