using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReportSystem : MonoBehaviour
{
    [Header("UI Elements")]
    public Image stunImage;
    public Button reportButton;
    public TextMeshProUGUI noAnomalyText;

    [Header("Panels")]
    public GameObject logMenu;
    public GameObject reportSendingTextPanel;

    [Header("Dropdowns")]
    public TMP_Dropdown roomDropdown;
    public TMP_Dropdown anomalyDropdown;

    [Header("References")]
    public SAnomalySpawner spawner;

    [Header("Settings")]
    public float displayTime = 3f;
    public float reportDelay = 1f;

    private Coroutine currentCoroutine;
    private bool anomalyExistsFlag;

    void Awake()
    {
        stunImage.gameObject.SetActive(false);
        noAnomalyText.gameObject.SetActive(false);
        reportSendingTextPanel.SetActive(false);

        reportButton.onClick.AddListener(OnReportButtonClicked);
    }

    public void OnReportButtonClicked()
    {
        PopulateDropdownsWithAllOptions();
        logMenu.SetActive(true);
    }

    private void PopulateDropdownsWithAllOptions()
    {
        Debug.Log("Report Button clicked");
        roomDropdown.ClearOptions();
        anomalyDropdown.ClearOptions();

        var rooms = new List<string>();
        var anomalies = new List<string> { "Missing", "Moved", "Replaced", "Extra" };

        
        HashSet<string> roomSet = new HashSet<string>();
        foreach (var anomaly in spawner.normalAnomalies)
        {
            roomSet.Add(anomaly.GetAnomalyRoom());
            Debug.Log($"Normal anomaly found in room: {anomaly.GetAnomalyRoom()}");
        }
        foreach (var anomaly in spawner.anomaliesNotNormal)
        {
            roomSet.Add(anomaly.GetAnomalyRoom());
            Debug.Log($"Active anomaly found in room: {anomaly.GetAnomalyRoom()}");
        }

        rooms.AddRange(roomSet);

        roomDropdown.AddOptions(rooms);
        anomalyDropdown.AddOptions(anomalies);

        Debug.Log($"Dropdowns populated. Rooms: {string.Join(", ", rooms)} | Anomalies: {string.Join(", ", anomalies)}");
    }

    public void OnSendReportButtonClicked()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(ReportSequence());
    }

    private IEnumerator ReportSequence()
    {
        Debug.Log("Report Sequence commencing");
        reportSendingTextPanel.SetActive(true);
        logMenu.SetActive(false);
        reportButton.gameObject.SetActive(false);

        yield return new WaitForSeconds(reportDelay);

        string selectedRoom = roomDropdown.options[roomDropdown.value].text.Trim();
        string selectedAnomaly = anomalyDropdown.options[anomalyDropdown.value].text.Trim();

        Debug.Log($"Player selected: Room = '{selectedRoom}', Anomaly = '{selectedAnomaly}'");

        anomalyExistsFlag = CheckAnomaly(selectedRoom, selectedAnomaly);

        if (anomalyExistsFlag)
        {
            Debug.Log("Anomaly exists! Triggering stun and removing anomaly.");
            stunImage.gameObject.SetActive(true);
            spawner.EraseAnomaly(selectedAnomaly, selectedRoom);
            Debug.Log($"EraseAnomaly called for Room = '{selectedRoom}', Anomaly = '{selectedAnomaly}'");
            yield return new WaitForSeconds(displayTime);
            stunImage.gameObject.SetActive(false);
            Debug.Log("Stun display completed.");
        }
        else
        {
            Debug.Log("No anomaly found. Displaying warning message.");
            noAnomalyText.text = $"No anomaly of type {selectedAnomaly} found in {selectedRoom}";
            noAnomalyText.gameObject.SetActive(true);
            yield return new WaitForSeconds(displayTime);
            noAnomalyText.gameObject.SetActive(false);
            Debug.Log("No anomaly message display completed.");
        }

        reportSendingTextPanel.SetActive(false);
        reportButton.gameObject.SetActive(true);
    }

    private bool CheckAnomaly(string playerRoom, string playerAnomaly)
    {
        foreach (var e in spawner.anomaliesNotNormal)
        {
            Debug.Log($"Checking anomaly: Room = '{e.GetAnomalyRoom()}', Type = '{e.GetAnomalyState()}'");

            if (e.GetAnomalyRoom().Equals(playerRoom, System.StringComparison.OrdinalIgnoreCase) &&
                e.GetAnomalyState().Equals(playerAnomaly, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log("Match found!");
                return true;
            }
        }

        Debug.Log("No match found.");
        return false;
    }
}







