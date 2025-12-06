using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class SGameInput : MonoBehaviour
{
    public static SGameInput Instance { get; private set; }

    //event for camera switch
    public event EventHandler<Vector2> OnCameraSwitchInput;

    [SerializeField] private SPauseMenu pauseMenu;

    private InputSystem_Actions playerInputActions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  
        }

        playerInputActions = new InputSystem_Actions();  
    }

    private void OnEnable()
    {
        playerInputActions.Enable(); 

        playerInputActions.Player.SwitchCamera.performed += SwitchCameraInput;
        playerInputActions.Player.Pause.performed += PauseGame;
    }

    private void OnDisable()
    {
        playerInputActions.Disable();  
        playerInputActions.Player.SwitchCamera.performed -= SwitchCameraInput;
        playerInputActions.Player.Pause.performed -= PauseGame;
    }

    private void SwitchCameraInput(InputAction.CallbackContext context)
    {
        if(!SPauseMenu.GameIsPaused)
        {
            Vector2 input = context.ReadValue<Vector2>();
            
            OnCameraSwitchInput?.Invoke(this, input);
        }
    }

     private void PauseGame(InputAction.CallbackContext context)
    {
        pauseMenu.TogglePause();
    }
}

