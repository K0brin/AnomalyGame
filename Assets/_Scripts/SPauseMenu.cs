using UnityEngine;

public class SPauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    private SAudioManager audioManager;
    private float originalVolume = 0.2f;
    private float reducedVolume = 0.025f;

    private void Awake()
    {
        audioManager = FindFirstObjectByType<SAudioManager>();
    }

    private void Start()
    {
        Resume();
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void TogglePause()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }


    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        if (audioManager != null && audioManager.IsPlaying("Level_01"))
        {
            audioManager.SetVolume("Level_01", originalVolume);
        }

    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        if (audioManager != null && audioManager.IsPlaying("Level_01"))
        {
            audioManager.SetVolume("Level_01", reducedVolume);
        }

    }
}
