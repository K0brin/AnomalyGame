using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReportSystem : MonoBehaviour
{
    [Header("UI Elements")]
    public Image stunImage;
    public Button reportButton;        // This button appears after a report
    public TextMeshProUGUI noAnomalyText;

    [Header("Dropdowns")]
    public TMP_Dropdown roomDropdown;
    public TMP_Dropdown anomalyDropdown;

    [Header("References")]
    public EntitySpawner spawner;
    public GameObject logMenu;          // Assign the Log Menu panel here

    [Header("Settings")]
    public float displayTime = 3f;
    public float reportDelay = 5f;

    private Coroutine currentCoroutine;

    public void Awake()
    {
        // Hide UI elements initially
        stunImage.gameObject.SetActive(false);
        noAnomalyText.gameObject.SetActive(false);
        reportButton.gameObject.SetActive(false);
        //reportButton.onClick.AddListener(OnReportButtonClicked);
    }

    // Called when the player clicks the report button to start a new report
    public void OnReportButtonClicked()
    {
        gameObject.SetActive(true);
        StartCoroutine(DelayedReport());
    }
    private IEnumerator DelayedReport()
    {
        // Wait the specified time
        yield return new WaitForSeconds(reportDelay);

        // Now start the report
        StartNewReport();
    }

    public void StartNewReport()
    {
        // Reset UI state
        stunImage.gameObject.SetActive(false);
        noAnomalyText.gameObject.SetActive(false);
        reportButton.gameObject.SetActive(false);

        // Hide Log Menu and show ReportSending text
        if (logMenu != null) logMenu.SetActive(false);
        gameObject.SetActive(true);  // ReportSendingText panel

        // Stop any running coroutine
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(ReportPending());
    }

    private IEnumerator ReportPending()
    {
        // Get player's dropdown selections
        string selectedRoom = roomDropdown.options[roomDropdown.value].text;
        string selectedAnomaly = anomalyDropdown.options[anomalyDropdown.value].text;

        bool anomalyExists = CheckAnomaly(selectedRoom, selectedAnomaly);

        if (anomalyExists)
        {
            stunImage.gameObject.SetActive(true);
            spawner.AttemptDeleteEntity(selectedRoom, selectedAnomaly);
            yield return new WaitForSeconds(displayTime);
            stunImage.gameObject.SetActive(false);
        }
        else
        {
            noAnomalyText.text = $"No anomaly of type {selectedAnomaly} found in {selectedRoom}";
            noAnomalyText.gameObject.SetActive(true);
            yield return new WaitForSeconds(displayTime);
            noAnomalyText.gameObject.SetActive(false);
        }

        // Optionally show the ReportButton if you want to allow another report
        reportButton.gameObject.SetActive(true);

        // **Hide the ReportSendingText panel itself** so it no longer blocks the UI
        gameObject.SetActive(false);

        currentCoroutine = null;
    }


    private bool CheckAnomaly(string playerRoom, string playerAnomaly)
    {
        for (int i = 0; i < spawner.roomActivated.Count; i++)
        {
            if (spawner.roomActivated[i] == playerRoom && spawner.typeActivated[i] == playerAnomaly)
                return true;
        }
        return false;
    }
}


