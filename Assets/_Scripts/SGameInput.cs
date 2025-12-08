using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class SGameInput : MonoBehaviour
{
    public static SGameInput Instance { get; private set; }

    public event EventHandler<Vector2> OnCameraSwitchInput;
    public event EventHandler OnGameDeviceChanged;
    public event EventHandler OnPausePressed;

    public enum GameDevice
    {
        KeyboardMouse,
        Gamepad,
    }

    private InputSystem_Actions playerInputActions;
    private GameDevice activeGameDevice;
    private float lastDeviceSwitchTime;
    private float deviceSwitchCooldown = 0.15f;

    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);  
            return; 
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);  

        playerInputActions = new InputSystem_Actions();

        InputSystem.onActionChange += InputSystem_OnActionChange;
        playerInputActions.Enable();
        
        playerInputActions.Player.SwitchCamera.performed += SwitchCameraInput;
        playerInputActions.Player.Pause.performed += PauseGame;


        Debug.Log("SGameInput.Instance has been initialized.");
    }

    // Handle input system action changes
    private void InputSystem_OnActionChange(object arg1, InputActionChange inputActionChange)
    {
        if (playerInputActions == null) return;

        if (inputActionChange != InputActionChange.ActionPerformed || arg1 is not InputAction inputAction)
            return;

        InputDevice device = inputAction.activeControl.device;

        if (device.displayName == "VirtualMouse") 
            return;
        if (device is Gamepad gp && gp.allControls.Count == 0) 
            return; // dead/disconnected

        //only switch if cooldown passed
        if (Time.unscaledTime - lastDeviceSwitchTime < deviceSwitchCooldown)
            return;

        if (device is Gamepad gamepad)
        {
            //deadzones
            float leftStick = gamepad.leftStick.ReadValue().magnitude;
            float rightStick = gamepad.rightStick.ReadValue().magnitude;

            if (leftStick > 0.2f || rightStick > 0.2f)
            {
                ChangeActiveGameDevice(GameDevice.Gamepad);
                lastDeviceSwitchTime = Time.unscaledTime;
            }
        }
        else
        {
            ChangeActiveGameDevice(GameDevice.KeyboardMouse);
            lastDeviceSwitchTime = Time.unscaledTime;
        }
}

    // Update the active game device and trigger the event
    private void ChangeActiveGameDevice(GameDevice activeGameDevice)
    {
        this.activeGameDevice = activeGameDevice;
        Debug.Log("New Active Game Device: " + activeGameDevice);

        Cursor.visible = activeGameDevice == GameDevice.KeyboardMouse;
        OnGameDeviceChanged?.Invoke(this, EventArgs.Empty);
    }

    // Get the current active game device
    public GameDevice GetActiveGameDevice()
    {
        return activeGameDevice;
    }

    public void RefreshCursor()
    {
        Cursor.visible = activeGameDevice == GameDevice.KeyboardMouse;
    }

    // Enable input actions
    // private void OnEnable()
    // {
    //     if (playerInputActions != null)
    //     {
    //         playerInputActions.Enable();
    //         playerInputActions.Player.SwitchCamera.performed += SwitchCameraInput;
    //         playerInputActions.Player.Pause.performed += PauseGame;

            
    //         InputSystem.onActionChange += InputSystem_OnActionChange;
    //     }
    //     else
    //     {
    //         Debug.LogError("playerInputActions is null! Ensure input system is properly initialized.");
    //     }
    // }

    // Disable input actions
    // private void OnDisable()
    // {
    //     if (playerInputActions == null)
    //     return;   //Prevent the null reference on destroyed duplicates

    //     if (playerInputActions != null)
    //     {
    //         playerInputActions.Player.SwitchCamera.performed -= SwitchCameraInput;
    //         playerInputActions.Player.Pause.performed -= PauseGame;

    //         InputSystem.onActionChange -= InputSystem_OnActionChange;
    //         playerInputActions.Disable();
    //     }
    //     else
    //     {
    //         Debug.LogError("playerInputActions is null! Can't disable actions.");
    //     }
    // }

    private void SwitchCameraInput(InputAction.CallbackContext context)
    {
        if (!SPauseMenu.GameIsPaused)
        {
            Vector2 input = context.ReadValue<Vector2>();
            OnCameraSwitchInput?.Invoke(this, input);
        }
    }

    private void PauseGame(InputAction.CallbackContext context)
    {
        Debug.Log("Pause button pressed!");
        OnPausePressed?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        InputSystem.onActionChange -= InputSystem_OnActionChange;

        if(playerInputActions != null)
        {
            playerInputActions.Player.SwitchCamera.performed -= SwitchCameraInput;
            playerInputActions.Player.Pause.performed -= PauseGame;

            playerInputActions.Disable();
        }
    }
}