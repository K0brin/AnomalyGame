using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class SVirtualMouseUI : MonoBehaviour
{
    public static SVirtualMouseUI Instance {get; private set;}
    [SerializeField] private RectTransform canvasRectTransform;
    private VirtualMouseInput virtualMouseInput;

    private void Awake()
    {
        Instance = this;
        virtualMouseInput = GetComponent<VirtualMouseInput>();
    }

    private void Start()
    {

        SGameInput.Instance.OnGameDeviceChanged += GameInput_OnGameDeviceChanged;

    }
    
    private void Update()
    {
        transform.localScale = Vector3.one * 1f/canvasRectTransform.localScale.x;
    }
    private void LateUpdate()
    {
        Vector2 virtualMousePosition = virtualMouseInput.virtualMouse.position.value;
        virtualMousePosition.x = Mathf.Clamp(virtualMousePosition.x, 0f, Screen.width);
        virtualMousePosition.y = Mathf.Clamp(virtualMousePosition.y, 0f, Screen.height);
        
        InputState.Change(virtualMouseInput.virtualMouse.position, virtualMousePosition);

    }

    private void GameInput_OnGameDeviceChanged(object sender, EventArgs e)
    {
        UpdateVisibility();
    }


    private void UpdateVisibility()
    {
        if (SGameInput.Instance.GetActiveGameDevice() == SGameInput.GameDevice.Gamepad)
        {
            Show();
        }
        else if (gameObject != null)
        {
            Hide();
        }
    }

    private void Hide()
    {
        // Only hide if the object is still alive
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }

    private void Show()
    {
        // Only show if the object is still alive
        if (gameObject != null)
        {
            gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if (SGameInput.Instance != null)
            SGameInput.Instance.OnGameDeviceChanged -= GameInput_OnGameDeviceChanged;
    }
}
