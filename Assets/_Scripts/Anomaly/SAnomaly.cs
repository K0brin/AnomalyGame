using System;
using Unity.VisualScripting;
using UnityEngine;

public class SAnomaly : MonoBehaviour
{
    [SerializeField] private AnomalySO anomalyData;  // Reference to the ScriptableObject
    [SerializeField] private Transform movedTransform; //Location of object when 'moved'
    private bool isModified = false;  // Tracks if anomaly is not normal anymore
    private GameObject currentPrefab;
    private GameObject extraPrefab;

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

        switch (newState)
        {
            case "Normal":
            {
                anomalyData.anomalyName = "Normal";
                isModified = false;  // Allow for anomaly to change.

                //Replace with norm prefab
                ReplacePrefab(anomalyData.normalPrefab);
            }
            break;
            case "Missing":
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
            break;

            case "Moved":
                {
                    // Set the anomaly to "Moved"
                    anomalyData.anomalyName = "Moved";
                    isModified = true;

                    // Re-Locate Object
                    currentPrefab.transform.position = movedTransform.position;
                    
                }
            break;

            case "Replaced":
                {
                    // Set the anomaly to "Replaced"
                    anomalyData.anomalyName = "Replaced";
                    isModified = true;

                    // Replace with the replaced prefab
                    ReplacePrefab(anomalyData.replacedPrefab);
                }
            break;

            case "Extra":
                {
                    anomalyData.anomalyName = "Extra";
                    isModified = true;

                    //add new object without deleting old
                    ExtraPrefab(anomalyData.extraPrefab);
                }
            break;

            default:
            {
                if (!isModified && newState != "Normal")
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
            break;
        }


        //old code made into switch

        /*if (newState == "Normal")
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
        }*/
    }

    public void RevertState()
    {
        String currentState = anomalyData.anomalyName;

        switch (currentState)
        {
            case "Normal":
            {
                Debug.Log("ERROR: state shows normal in revert function");
            }
            break;
            case "Missing":
            {
                // Re-Activate the prefab
                if (currentPrefab != null)
                {
                    currentPrefab.SetActive(true);
                }
            }
            break;

            case "Moved":
                {
                    // Re-Locate Object to original transform
                    currentPrefab.transform.position = this.gameObject.transform.parent.transform.position;
                }
            break;

            case "Replaced":
                {
                    // Replace with the normal prefab
                    ReplacePrefab(anomalyData.normalPrefab);
                }
            break;

            case "Extra":
                {
                    //delete new object, leave old
                    Destroy(extraPrefab);
                }
            break;

            default:
            {
                if (!isModified && currentState != "Normal")
                {

                        // Replace with the replaced prefab
                        ReplacePrefab(anomalyData.replacedPrefab);
                }
                else
                {       
                    Debug.LogWarning("Anomaly cannot change state again until it is reset to 'Normal'.");
                }
            }
            break;
        }

        //Set the Anomaly to "Normal"
        anomalyData.anomalyName = "Normal";
        isModified = false;

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

    private void ExtraPrefab(GameObject newPrefab)
    {
        extraPrefab = Instantiate(newPrefab, movedTransform.position, transform.rotation);
        extraPrefab.transform.SetParent(transform.parent); //keep in same room
    }

    public string GetAnomalyState()
    {
        return anomalyData != null ? anomalyData.anomalyName : "Unknown";
    }

    public string GetAnomalyRoom()
    {
        return anomalyData != null ? this.transform.parent.name : "Unknown";
    }

    public void ResetToNormal()
    {
        ChangeState("Normal");
        Debug.Log("Anomaly has been reset to 'Normal'.");
    }
}