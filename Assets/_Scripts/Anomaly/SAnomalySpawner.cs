using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using TMPro;

public class SAnomalySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] anomalyPrefabs;  
    [SerializeField] private float changeInterval = 5f;  // Seconds before anomaly change
    [SerializeField] private GameObject warningUI;
    [SerializeField] AudioSource warningAudio;
    private float typeTime = 0.1f;
    private float timer = 0f;
    private List<SAnomaly> normalAnomalies = new List<SAnomaly>();  // List of current normal anomalies
    private List<SAnomaly> anomaliesNotNormal = new List<SAnomaly>();  // List of current non-normal anomalies
    private int anomaliesNotNormalCount = 0;  // Counter for anomalies not normal, have report system remove from count when submitting is successful
    public bool gameOver = false;  

    void Start()
    {
        warningUI.SetActive(false);
        InitializeAnomalies();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= changeInterval)
        {
            timer = 0f;  // Reset timer
            ChangeRandomAnomalyState();
            CheckGameOverCondition();
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

        selectedAnomaly.ChangeState(newState);

        //change complete
        Debug.Log($"[AnomalySpawner] Anomaly {selectedAnomaly.name} state changed to '{newState}'.");

        if (selectedAnomaly.GetAnomalyState() != "Normal")
        {
            normalAnomalies.Remove(selectedAnomaly);
            anomaliesNotNormal.Add(selectedAnomaly);
            anomaliesNotNormalCount++;  // Increase the count of anomalies that aren't "Normal"
        }
    }


    //called by report script
    //AnomalyState: "Missing", "Moved", "Replaced", "Extra"
    //RoomName: Garage, LivingRoom, Backyard, Kitchen
    private void EraseAnomaly(string AnomalyState, string RoomName)
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
        Debug.Log(anomaliesNotNormalCount);
        if (anomaliesNotNormalCount == 3)
        {
            if (!gameOver)
            {
                gameOver = true;
                Debug.Log("Game Over! Anomalies took over!");
            }
        }
        else if(anomaliesNotNormalCount == 1) //should be 2
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

    public void ResetGame()
    {
        gameOver = false;
        anomaliesNotNormalCount = 0;
        InitializeAnomalies();  // Reset anomalies 
    }
}