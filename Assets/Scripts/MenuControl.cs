using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using TMPro;
using System;

public class MenuControl : MonoBehaviour
{

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text controllerSenTextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;
    [SerializeField] private int defaultSen = 4;
    public int mainControllerSen = 4;

    

    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 1;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;


    private int _qualityLevel;
    private bool _Isfullscreen;
    private float _brightnessLevel;
      private float tempVolume;
    private float savedVolume;
     private int tempResolutionIndex;
    private int savedResolutionIndex;




    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null;
    [SerializeField] private float confirmationDisplayTime = 2.0f;


    [Header("Level To load")]
    public string _newGameLevel;
    public string _levelToLoad;
    [SerializeField] private GameObject _noSaveGameDialog = null;

    [Header("Resulution DropDown")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;


    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
         
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue(); 

        savedVolume = PlayerPrefs.GetFloat("Volume", defaultVolume);
        tempVolume = savedVolume;
        AudioListener.volume = savedVolume;
        volumeSlider.value = savedVolume;
        volumeTextValue.text = (savedVolume * 100).ToString("0") + "%";

         savedResolutionIndex = currentResolutionIndex;
        tempResolutionIndex = savedResolutionIndex;

    }

    public void SetResolution(int resolutionIndex)
    {
         tempResolutionIndex = resolutionIndex;
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void LoadGameDialogYes()
    {
        if(PlayerPrefs.HasKey("SavedLevel"))
        {
            _levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(_levelToLoad);
        }
        else
        {
            _noSaveGameDialog.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }
   
   public void SetVolume(float volume) 
   {
     tempVolume = volume;
        AudioListener.volume = volume;
        volumeTextValue.text = (volume * 100).ToString("0") + "%";

   }

   public void VolumeApply()
   {
      savedVolume = tempVolume;
        PlayerPrefs.SetFloat("Volume", savedVolume);
        StartCoroutine(ConfirmationBox());
       
   }

   public void SetControllerSen(float senstivity)
   {
       mainControllerSen = Mathf.RoundToInt(senstivity);
       controllerSenTextValue.text = senstivity.ToString("0");
   }   

   public void GameplayApply()
   {

      
       PlayerPrefs.SetFloat("masterSen", mainControllerSen);
         StartCoroutine(ConfirmationBox());
   }   

     public void SetBrightness(float brightness)
   {
       _brightnessLevel = brightness;
       brightnessTextValue.text = brightness.ToString("0.0");

    }
      
    public void SetFullscreen(bool isFullscreen)
    {
        _Isfullscreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void GraphicsApply()
    {
         PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
         //change your brightness with your post processing 

         PlayerPrefs.SetInt("masterQuality", _qualityLevel);
            QualitySettings.SetQualityLevel(_qualityLevel);

            PlayerPrefs.SetInt("masterFullscreen", (_Isfullscreen ? 1 : 0));
            Screen.fullScreen = _Isfullscreen;
            
            StartCoroutine(ConfirmationBox());

    }

   public void ResetButton(String MenuType)

   {    
            if(MenuType == "Graphics")
            {
                //reset brightness value
                brightnessSlider.value = defaultBrightness;
                brightnessTextValue.text = defaultBrightness.ToString("0.0");
                
                qualityDropdown.value = 1;
                QualitySettings.SetQualityLevel(1);

                fullscreenToggle.isOn = false;
                Screen.fullScreen = false;

                Resolution currentResolution = Screen.currentResolution;
                Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
                resolutionDropdown.value = resolutions.Length;
                GraphicsApply();
            }

         if(MenuType == "Audio")
         {
              AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = (defaultVolume * 100).ToString("0") + "%";
            VolumeApply();
         }
         
         if(MenuType == "Gameplay")
         {
             controllerSenTextValue.text = defaultSen.ToString("0");
                controllerSenSlider.value = defaultSen;
                mainControllerSen = defaultSen;
            
                GameplayApply();

         }

   }

       
   public IEnumerator ConfirmationBox()
{
    if (confirmationPrompt != null)
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(confirmationDisplayTime); // Use the configurable field
        confirmationPrompt.SetActive(false);
    }
    else
    {
        Debug.LogWarning("Confirmation prompt is not assigned in the Inspector.");
    }
}
   public void BackButton()
    {
        AudioListener.volume = savedVolume;
        volumeSlider.value = savedVolume;
        volumeTextValue.text = (savedVolume * 100).ToString("0") + "%";

        Resolution resolution = resolutions[savedResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        resolutionDropdown.value = savedResolutionIndex;

    Debug.Log("Graphics settings not saved. Please click Apply to save the graphics settings.");
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(_newGameLevel);
    }
}

