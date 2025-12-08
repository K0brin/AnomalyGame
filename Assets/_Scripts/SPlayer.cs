using System;
using UnityEngine;

public class SPlayer : MonoBehaviour
{
    public static SPlayer Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);  
            return;
        }

        Instance = this;  
        DontDestroyOnLoad(gameObject);  
    }

    private void Start()
    {
       
        SGameInput.Instance.OnCameraSwitchInput += HandleCameraSwitchInput;
        SGameInput.Instance.OnPausePressed += HandleTogglePause;
    }

    private void OnDisable()
    {
        
        SGameInput.Instance.OnCameraSwitchInput -= HandleCameraSwitchInput;
        SGameInput.Instance.OnPausePressed -= HandleTogglePause;
    }

    private void HandleCameraSwitchInput(object sender, Vector2 input)
    {
        //forward the camera switch input to the SCameraManager
        SCameraManager.Instance.HandleCameraSwitch(input);
    }

    private void HandleTogglePause(object sender, EventArgs e)
    {
         if (SPauseMenu.Instance != null)
        {
            // Toggle the pause menu if the instance is not null
            SPauseMenu.Instance.TogglePause();
        }
        else
        {
            Debug.LogError("SPauseMenu Instance is null");
        }
    }

}
