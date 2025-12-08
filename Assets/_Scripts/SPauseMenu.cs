using UnityEngine;

public class SPauseMenu : MonoBehaviour
{
    public static SPauseMenu Instance { get; private set; }

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    private SAudioManager audioManager;
    private float originalVolume = 0.2f;
    private float reducedVolume = 0.025f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return; 
        }
        Instance = this;

        audioManager = FindFirstObjectByType<SAudioManager>();

        if (pauseMenuUI == null)
        {
            Debug.LogWarning("Pause Menu UI not assigned in the inspector.");
        }
    }

    private void Start()
    {
        Resume();

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

        if (audioManager != null)
        {
            audioManager.SetVolume("Level_01", originalVolume);
        }

    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        if (audioManager != null)
        {
            audioManager.SetVolume("Level_01", reducedVolume);
        }

    }

    
}
