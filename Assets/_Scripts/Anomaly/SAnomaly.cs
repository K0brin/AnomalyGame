using UnityEngine;

public class SAnomaly : MonoBehaviour
{
    [SerializeField] private AnomalySO anomalyData;  // Reference to the ScriptableObject
    private bool isModified = false;  // Tracks if anomaly is not normal anymore
    private GameObject currentPrefab;

    private void Start()
    {
        if (anomalyData != null)
        {
            // start anomlies as normal prefab
            ChangeState("Normal");
        }
        else
        {
            Debug.LogWarning("No AnomalySO assigned");
        }
    }

    public void ChangeState(string newState)
    {
        if (newState == "Normal")
        {
            anomalyData.anomalyName = "Normal";
            isModified = false;  // Allow for anomaly to change.

            //Replace with norm prefab
            ReplacePrefab(anomalyData.normalPrefab);
        }
        else if (newState == "Missing")
        {
            // Set the anomaly to "Missing"
            anomalyData.anomalyName = "Missing";
            isModified = true;

            // Deactivate the prefab
            if (currentPrefab != null)
            {
                currentPrefab.SetActive(false);
            }
        }
        else if (!isModified && newState != "Normal")
        {
            // Prevent changes if the anomaly is not normal
            anomalyData.anomalyName = newState;
            isModified = true;

            // Replace with the replaced prefab
            ReplacePrefab(anomalyData.replacedPrefab);
        }
        else
        {
            Debug.LogWarning("Anomaly cannot change state again until it is reset to 'Normal'.");
        }
    }

    private void ReplacePrefab(GameObject newPrefab)
    {
        if (currentPrefab != null)
        {
            // remove previous prefab
            Destroy(currentPrefab);
        }

        if (newPrefab != null)
        {
            // Instantiate the new prefab 
            currentPrefab = Instantiate(newPrefab, transform.position, transform.rotation);
            currentPrefab.transform.SetParent(transform.parent); //keep in same room
        }
        else
        {
            Debug.LogWarning("Prefab to replace is null.");
        }
    }

    public string GetAnomalyState()
    {
        return anomalyData != null ? anomalyData.anomalyName : "Unknown";
    }

    public void ResetToNormal()
    {
        ChangeState("Normal");
        Debug.Log("Anomaly has been reset to 'Normal'.");
    }
}