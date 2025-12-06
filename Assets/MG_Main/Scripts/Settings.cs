using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public SAudioManager audioManager;

    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;

    private void Start()
    {
        if (audioManager != null)
        {
            masterVolumeSlider.value = audioManager.GetVolume("Master");
            musicVolumeSlider.value = audioManager.GetVolume("Music");
        }

        //listeners for slider changes
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
}
