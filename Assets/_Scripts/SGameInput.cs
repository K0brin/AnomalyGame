using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class SGameInput : MonoBehaviour
{
    public static SGameInput Instance { get; private set; }

    //event for camera switch
    public event EventHandler<Vector2> OnCameraSwitchInput;

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
    }

    private void OnDisable()
    {
        playerInputActions.Disable();  
        playerInputActions.Player.SwitchCamera.performed -= SwitchCameraInput;
    }

    private void SwitchCameraInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        
        OnCameraSwitchInput?.Invoke(this, input);
    }
}

