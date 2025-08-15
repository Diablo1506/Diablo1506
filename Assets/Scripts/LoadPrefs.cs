using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MenuControl MenuControl;

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("Brightness Setting")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 1;

    [Header("Quality Level Setting")]
    [SerializeField] private TMP_Dropdown qualityDropdown;

    [Header("Fullscreen Setting")]
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Sensitivity Setting")]
    [SerializeField] private TMP_Text controllerSenTextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;


    private void Awake()
    {
        if (!canUse) return;

        // Load Volume Settings
        float localVolume = PlayerPrefs.GetFloat("masterVolume", 1.0f);
        volumeTextValue.text = localVolume.ToString("0.0");
        volumeSlider.value = localVolume;
        AudioListener.volume = localVolume;

        // Load Quality Settings
        int localQuality = PlayerPrefs.GetInt("masterQuality", 2);
        qualityDropdown.value = localQuality;
        QualitySettings.SetQualityLevel(localQuality);

        // Load Fullscreen Settings
        int localFullscreen = PlayerPrefs.GetInt("masterFullscreen", 1);
        fullscreenToggle.isOn = localFullscreen == 1;
        Screen.fullScreen = localFullscreen == 1;

        // Load Brightness Settings
        float localBrightness = PlayerPrefs.GetFloat("masterBrightness", defaultBrightness);
        brightnessTextValue.text = localBrightness.ToString("0.0");
        brightnessSlider.value = localBrightness;

        // Load Sensitivity Settings
        float localSensitivity = PlayerPrefs.GetFloat("masterSen", 5.0f);
        controllerSenTextValue.text = localSensitivity.ToString("0");
        controllerSenSlider.value = localSensitivity;

        // Assign Sensitivity to MenuControl
        if (MenuControl != null)
        {
            MenuControl.mainControllerSen = Mathf.RoundToInt(localSensitivity);
        }
        else
        {
            Debug.LogWarning("MenuControl is not assigned in the Inspector.");
        }
   
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
        PlayerPrefs.SetFloat("masterVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetBrightness(float brightness)
    {
        brightnessTextValue.text = brightness.ToString("0.0");
        PlayerPrefs.SetFloat("masterBrightness", brightness);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("masterFullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("masterQuality", qualityIndex);
        PlayerPrefs.Save();
    }

    public void SetSensitivity(float sensitivity)
    {
        controllerSenTextValue.text = sensitivity.ToString("0");
        PlayerPrefs.SetFloat("masterSen", sensitivity);
        PlayerPrefs.Save();
    }

}