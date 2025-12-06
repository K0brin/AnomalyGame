using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using TMPro;
using UnityEditor;
using UnityEngine.LightTransport;
using NUnit.Framework.Internal;

public class SAnomalySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] anomalyPrefabs;  
    [SerializeField] private float minChangeInterval = 5;
    [SerializeField] private float maxChangeInterval = 10;
    private float changeInterval;  // Seconds before anomaly change
    [SerializeField] private GameObject warningUI;
    [SerializeField] AudioSource warningAudio;
    private float typeTime = 0.1f;
    private float timer = 0f;

    //MG changed protection levels so ReportSystem can access it
    public List<SAnomaly> normalAnomalies = new List<SAnomaly>();  // List of current normal anomalies
    public List<SAnomaly> anomaliesNotNormal = new List<SAnomaly>();  // List of current non-normal anomalies

    private int anomaliesNotNormalCount = 0;  // Counter for anomalies not normal, have report system remove from count when submitting is successful
    public bool gameOver = false;  
    SCameraManager cameraManager;

    void Start()
    {
        cameraManager = GameObject.FindGameObjectWithTag("CameraManager").GetComponent<SCameraManager>();
        warningUI.SetActive(false);
        InitializeAnomalies();
        RandomizeChangeInterval();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= changeInterval)
        {
            timer = 0f;  // Reset timer
            ChangeRandomAnomalyState();
            CheckGameOverCondition();
            RandomizeChangeInterval();
        }

    }

    private void InitializeAnomalies()
    {
        normalAnomalies.Clear();  
        anomaliesNotNormal.Clear();
        anomaliesNotNormalCount = 0; // Reset counter

        foreach (var prefab in anomalyPrefabs)
        {
            if (prefab == null) continue;

            SAnomaly anomalyComponent = prefab.GetComponent<SAnomaly>();
            if (anomalyComponent != null)
            {
                anomalyComponent.ChangeState("Normal");

                // If the anomaly is normal add to norm list
                if (anomalyComponent.GetAnomalyState() == "Normal")
                {
                    normalAnomalies.Add(anomalyComponent);
                }
                else
                {
                    anomaliesNotNormal.Add(anomalyComponent);
                    anomaliesNotNormalCount++; 
                }

                string roomName = prefab.transform.parent != null ? prefab.transform.parent.name : "Unknown Room";
                Debug.Log($"Anomaly '{anomalyComponent.name}' spawned in room {roomName} and set to 'Normal'.");
            }
            else
            {
                Debug.LogWarning($"Prefab {prefab.name} does not have the SAnomaly component attached.");
            }
        }
    }

    private void ChangeRandomAnomalyState()
    {
        if (normalAnomalies.Count == 0)
        {
            Debug.LogWarning("No anomalies in the 'Normal' state to change.");
            return;
        }

        // Pick a random normal anomaly from norm list
        int randomIndex = Random.Range(0, normalAnomalies.Count);
        SAnomaly selectedAnomaly = normalAnomalies[randomIndex];

        // Randomly set state of anomaly
        string[] possibleStates = new string[] { "Missing", "Moved", "Replaced", "Extra" };
        string newState = possibleStates[Random.Range(0, possibleStates.Length)];

        //change attempt
        Debug.Log($"[AnomalySpawner] Trying to change anomaly {selectedAnomaly.name} from '{selectedAnomaly.GetAnomalyState()}' to '{newState}'.");


        bool canSpawn = true;
        canSpawn = true;
        foreach( var anomaly in anomaliesNotNormal)
        {
            if (anomaly.GetAnomalyRoom() == selectedAnomaly.GetAnomalyRoom() && anomaly.GetAnomalyState() == newState)
            {
                //if same type in same room
                canSpawn = false;
                break;
            }
        }

        //don't spawn anomaly when player is looking
        string activeCamera = "Empty";
        switch (cameraManager.GetCurrentCam())
        {
            case 0: activeCamera = "LivingRoom";  break; //living room
            case 1: activeCamera = "Garage";    break; //garage
            case 2: activeCamera = "Kitchen";    break; //kitchen
            case 3: activeCamera = "Backyard";    break; //backyard
        }

        if(activeCamera == selectedAnomaly.GetAnomalyRoom())
        {
            canSpawn = false;
            Debug.Log($"Anomaly Attempted to Spawn Where Player Was Looking: {activeCamera}");
        }
        else if(activeCamera == "Empty")
        {
            Debug.Log("Value failed to change");
        }


        if (canSpawn)
        {
            selectedAnomaly.ChangeState(newState);
            //change complete
            Debug.Log($"[AnomalySpawner] Anomaly {selectedAnomaly.name} state changed to '{newState}'.");
            Debug.Log($"[SPAWN LOG] Anomaly spawned: Type = '{selectedAnomaly.GetAnomalyState()}', Room = '{selectedAnomaly.GetAnomalyRoom()}'");

            if (selectedAnomaly.GetAnomalyState() != "Normal")
            {
                normalAnomalies.Remove(selectedAnomaly);
                anomaliesNotNormal.Add(selectedAnomaly);
                anomaliesNotNormalCount++;  // Increase the count of anomalies that aren't "Normal"
            }

        }
        else
        {
            ChangeRandomAnomalyState();
            return;
        }
    }


    //called by report script
    //AnomalyState: "Missing", "Moved", "Replaced", "Extra"
    //RoomName: Garage, LivingRoom, Backyard, Kitchen
    //MG changed EraseAnomaly's protection level so that ReportSystem can access it
    public void EraseAnomaly(string AnomalyState, string RoomName)
    {
        if (anomaliesNotNormal.Count == 0)
        {
            Debug.LogWarning("No active anomalies to erase.");
            return;
        }

        //check each active anomaly
        foreach(var activeAnomaly in anomaliesNotNormal)
        {
            Debug.Log(activeAnomaly.GetAnomalyRoom());
            //if state and name match
            if(activeAnomaly.GetAnomalyState() == AnomalyState && activeAnomaly.GetAnomalyRoom() == RoomName)
            {
                //revert anomaly back to normal
                activeAnomaly.RevertState();
                
                //remove from active, add to normal
                anomaliesNotNormal.Remove(activeAnomaly);
                normalAnomalies.Add(activeAnomaly);
                anomaliesNotNormalCount++; //reduce amount of not normal anomalies
                Debug.Log($"Anomaly of Type{AnomalyState} and In Room{RoomName} Successfully Reverted");
                break;
            }
        }
    }

    private void CheckGameOverCondition()
    {
        if (anomaliesNotNormalCount >= 3)
        {
            if (!gameOver)
            {
                gameOver = true;
                Debug.Log("Game Over! Anomalies took over!");
            }
        }
        else if(anomaliesNotNormalCount == 2) //should be 2
        {
            warningUI.SetActive(true);
            StartCoroutine(PlayWarning());
        }
    }

    private IEnumerator PlayWarning()
    {
        warningAudio.Play();
        string inputText = "THIS IS AN EMERGENCY WARNING!";
        StartCoroutine(TypeWriter(inputText));
        yield return new WaitForSeconds(typeTime * inputText.Length + 2);
        warningAudio.Play();
        inputText = "WE ARE RECIEVING READINGS OF MULTIPLE ACTIVE ANOMALIES IN YOUR AREA";
        StartCoroutine(TypeWriter(inputText));
        yield return new WaitForSeconds(typeTime * inputText.Length + 2);
        warningAudio.Play();
        inputText = "PLEASE LOCATE THE ANOMALIES AND SEND REPORTS ASAP";
        StartCoroutine(TypeWriter(inputText));
        yield return new WaitForSeconds(typeTime * inputText.Length + 2);
        warningUI.SetActive(false);
    }

    private IEnumerator TypeWriter(string inputText)
    {
        TextMeshProUGUI warningText = warningUI.GetComponentInChildren<TextMeshProUGUI>();
        warningText.text = "";
        string leadingChar = "";

        foreach(char a in inputText)
        {
            if (warningText.text.Length > 0)
			{
				warningText.text = warningText.text.Substring(0, warningText.text.Length - leadingChar.Length);
			}
			warningText.text += a;
			warningText.text += leadingChar;
			yield return new WaitForSeconds(typeTime);
        }
        warningAudio.Stop();
    }

    private void RandomizeChangeInterval()
    {
        changeInterval = Random.Range(minChangeInterval, maxChangeInterval);
    }

    public void ResetGame()
    {
        gameOver = false;
        anomaliesNotNormalCount = 0;
        InitializeAnomalies();  // Reset anomalies 
    }
}