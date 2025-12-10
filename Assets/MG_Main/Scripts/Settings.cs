using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public SAudioManager audioManager; //plug in SAudioManager in Inspector

    [Header("Audio")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;

    [Header("Display")]
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    private Resolution[] resolutions;

    private void Start()
    {
        SetupDisplay();
        SetupAudio();
    }

    private void SetupAudio()
    {
        if (audioManager == null) return;


        masterVolumeSlider.SetValueWithoutNotify(audioManager.masterVolume);
        musicVolumeSlider.SetValueWithoutNotify(audioManager.musicVolume);

        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetMasterVolume(float value)
    {
        audioManager.SetVolume("Master", value);
    }

    public void SetMusicVolume(float value)
    {
        audioManager.SetVolume("Music", value);
    }

    public void SetCaseOhVolume(float value)
    {
        audioManager.SetVolume("CaseOh", value);
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

        int savedRes = PlayerPrefs.GetInt("ResolutionIndex", currentResIndex);
        bool savedFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        resolutionDropdown.SetValueWithoutNotify(savedRes);
        resolutionDropdown.RefreshShownValue();
        fullscreenToggle.SetIsOnWithoutNotify(savedFullscreen);

        ApplyFullscreen(savedFullscreen);
        ApplyResolution(savedRes);

        resolutionDropdown.onValueChanged.AddListener(ApplyResolution);
        fullscreenToggle.onValueChanged.AddListener(ApplyFullscreen);
    }

    private void ApplyResolution(int index)
    {
        PlayerPrefs.SetInt("ResolutionIndex", index);

        Resolution r = resolutions[index];
        Screen.SetResolution(r.width, r.height, fullscreenToggle.isOn);
    }

    private void ApplyFullscreen(bool value)
    {
        PlayerPrefs.SetInt("Fullscreen", value ? 1 : 0);
        Screen.fullScreen = value;
    }
}

