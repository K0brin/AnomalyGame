using UnityEngine;
using UnityEngine.SceneManagement;

public class SSceneLoader : MonoBehaviour

{
    public void QuitGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        Debug.Log("Loading Menu....");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void LoadLevel01()
    {
        Debug.Log("Loading Level 1....");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);


    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SGameInput.Instance != null)
            SGameInput.Instance.RefreshCursor();
    }

}