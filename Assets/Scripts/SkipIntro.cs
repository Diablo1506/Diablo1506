using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SkipIntro : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component

    void Start()
    {
        // Add an event handler for when the video reaches the end
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void Update()
    {
        // Check if the space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Stop the video and perform any additional actions
            videoPlayer.Stop();
            OnIntroSkipped();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Actions to perform after the video ends
        Debug.Log("Video ended!");
        LoadMainMenu();
    }

    void OnIntroSkipped()
    {
        // Actions to perform after skipping the intro
        Debug.Log("Intro skipped!");
        LoadMainMenu();
    }

    void LoadMainMenu()
    {
        // Load the main menu scene asynchronously
        SceneManager.LoadSceneAsync("Demo1");
       
    }


}