using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public SAudioManager audioManager;

    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;

    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    private Resolution[] resolutions;

    private void Start()
    {
        SetupDisplay();

        if (audioManager != null)
        {
            masterVolumeSlider.SetValueWithoutNotify(audioManager.GetVolume("Level_01"));
            musicVolumeSlider.SetValueWithoutNotify(audioManager.GetVolume("Level_01"));
        }

        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetMasterVolume(float value)
    {
        audioManager.SetVolume("Level_01", value);
    }

    public void SetMusicVolume(float value)
    {
        audioManager.SetVolume("Level_01", value);
    }

    private void SetupDisplay()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        // Load saved values or defaults
        int savedRes = PlayerPrefs.GetInt("ResolutionIndex", currentResIndex);
        bool savedFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        // Set UI values without triggering events
        resolutionDropdown.SetValueWithoutNotify(savedRes);
        resolutionDropdown.RefreshShownValue();
        fullscreenToggle.SetIsOnWithoutNotify(savedFullscreen);

        // Apply settings AFTER UI values are set
        ApplyFullscreen(savedFullscreen);
        ApplyResolution(savedRes);

        // Add listeners
        resolutionDropdown.onValueChanged.AddListener(ApplyResolution);
        fullscreenToggle.onValueChanged.AddListener(ApplyFullscreen);
    }

    // Apply resolution change
    private void ApplyResolution(int index)
    {
        PlayerPrefs.SetInt("ResolutionIndex", index);

        Resolution r = resolutions[index];
        Screen.SetResolution(r.width, r.height, fullscreenToggle.isOn);
    }

    // Apply fullscreen change
    private void ApplyFullscreen(bool value)
    {
        PlayerPrefs.SetInt("Fullscreen", value ? 1 : 0);

        Screen.fullScreen = value;
        Debug.Log("Fullscreen applied: " + Screen.fullScreen);
    }
}
