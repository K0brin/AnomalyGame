using UnityEngine;
using System.Collections.Generic;

public class SAnomalySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] anomalyPrefabs;  
    [SerializeField] private float changeInterval = 5f;  // Seconds before anomaly change
    private float timer = 0f;
    private List<SAnomaly> activeAnomalies = new List<SAnomaly>();  // List of current normal anomalies
    private int anomaliesNotNormalCount = 0;  // Counter for anomalies not normal, have report system remove from count when submitting is successful
    public bool gameOver = false;  

    void Start()
    {
        InitializeAnomalies();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= changeInterval)
        {
            timer = 0f;  // Reset timer
            ChangeRandomAnomalyState();
        }

        // move out of update
        CheckGameOverCondition();
    }

    private void InitializeAnomalies()
    {
        activeAnomalies.Clear();  
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
                    activeAnomalies.Add(anomalyComponent);
                }
                else
                {
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
        if (activeAnomalies.Count == 0)
        {
            Debug.LogWarning("No anomalies in the 'Normal' state to change.");
            return;
        }

        // Pick a random normal anomaly from norm list
        int randomIndex = Random.Range(0, activeAnomalies.Count);
        SAnomaly selectedAnomaly = activeAnomalies[randomIndex];

        // Randomly set state of anomaly
        string[] possibleStates = new string[] { "Missing", "Moved", "Replaced", "Extra" };
        string newState = possibleStates[Random.Range(0, possibleStates.Length)];

        //change attempt
        Debug.Log($"[AnomalySpawner] Trying to change anomaly {selectedAnomaly.name} from '{selectedAnomaly.GetAnomalyState()}' to '{newState}'.");

        selectedAnomaly.ChangeState("Extra");

        //change complete
        Debug.Log($"[AnomalySpawner] Anomaly {selectedAnomaly.name} state changed to '{newState}'.");

        if (selectedAnomaly.GetAnomalyState() != "Normal")
        {
            activeAnomalies.Remove(selectedAnomaly);
            anomaliesNotNormalCount++;  // Increase the count of anomalies that aren't "Normal"
        }
    }

    private void CheckGameOverCondition()
    {
        if (anomaliesNotNormalCount == 3)
        {
            if (!gameOver)
            {
                gameOver = true;
                Debug.Log("Game Over! Anomalies took over!");
            }
        }
    }

    public void ResetGame()
    {
        gameOver = false;
        anomaliesNotNormalCount = 0;
        InitializeAnomalies();  // Reset anomalies 
    }
}