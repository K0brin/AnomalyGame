using UnityEngine;

public class SPlayer : MonoBehaviour
{
    private void OnEnable()
    {
       
        SGameInput.Instance.OnCameraSwitchInput += HandleCameraSwitchInput;
    }

    private void OnDisable()
    {
        
        SGameInput.Instance.OnCameraSwitchInput -= HandleCameraSwitchInput;
    }

    private void HandleCameraSwitchInput(object sender, Vector2 input)
    {
        //forward the camera switch input to the SCameraManager
        SCameraManager.Instance.HandleCameraSwitch(input);
    }

}
