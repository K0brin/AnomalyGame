using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;

public class SCameraManager : MonoBehaviour
{
    public static SCameraManager Instance { get; private set; }

    [SerializeField] private List<Camera> cameras;  //list of cameras
    [SerializeField] private TextMeshProUGUI mRoomNameText;

    private int mCurrentCam = 0;  // tracks active cam

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  //ensures only one instance of SCameraManager exists
        }

        //start with all cams off
        foreach (Camera cam in cameras)
        {
            cam.gameObject.SetActive(false);
        }

        //turn first cam on
        cameras[mCurrentCam].gameObject.SetActive(true);
        UpdateRoomNameUI();
    }

    private void UpdateRoomNameUI()
    {
        if (mRoomNameText != null && cameras.Count > mCurrentCam)
        {
            mRoomNameText.text = cameras[mCurrentCam].name;
        }
    }

    public void HandleCameraSwitch(Vector2 input)
    {

        if (input.x > 0)  
        {
            SwitchCameraFoward();
        }
        else if (input.x < 0)  
        {
            SwitchCameraBackward();
        }
    }


    public void SwitchCameraFoward()
    {
        //disable past cam
        cameras[mCurrentCam].gameObject.SetActive(false);

        //update the current cam based on direction
        mCurrentCam = (mCurrentCam + 1) % cameras.Count;

        //enable current cam
        cameras[mCurrentCam].gameObject.SetActive(true);
        UpdateRoomNameUI();
        PlayClickedAudio();
    }

     public void SwitchCameraBackward()
    {
        //disable past cam
        cameras[mCurrentCam].gameObject.SetActive(false);

        //update the current cam based on direction
        mCurrentCam = (mCurrentCam - 1 + cameras.Count) % cameras.Count;

        //enable current cam
        cameras[mCurrentCam].gameObject.SetActive(true);
        UpdateRoomNameUI();
        PlayClickedAudio();

    }

    private void PlayClickedAudio()
    {
        SAudioManager audioManager = FindAnyObjectByType<SAudioManager>();
        audioManager.Play("click_005");
    }

    public int GetCurrentCam()
    {
        return mCurrentCam;
    }
}
