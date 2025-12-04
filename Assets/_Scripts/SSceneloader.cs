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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadLevel01()
    {
        Debug.Log("Loading Level 1....");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


    }

}