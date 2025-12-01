using System.Collections;
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
    public EntitySpawner spawner;

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
        roomDropdown.ClearOptions();
        anomalyDropdown.ClearOptions();

        var rooms = new System.Collections.Generic.List<string>();
        foreach (var room in spawner.roomHolders)
            rooms.Add(room.roomName);

        var anomalies = new System.Collections.Generic.List<string>();
        foreach (var s in spawner.scriptableObjects)
            anomalies.Add(s.typeName);

        roomDropdown.AddOptions(rooms);
        anomalyDropdown.AddOptions(anomalies);

        Debug.Log("Dropdowns populated with all rooms and anomaly types.");
    }

    public void OnSendReportButtonClicked()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(ReportSequence());
    }

    private IEnumerator ReportSequence()
    {
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

        reportSendingTextPanel.SetActive(false);
        reportButton.gameObject.SetActive(true);
    }
    private bool CheckAnomaly(string playerRoom, string playerAnomaly)
    {
        foreach (var e in spawner.activeEntities)
        {
            Debug.Log($"Checking entity: Room = '{e.roomName}', Type = '{e.typeName}'");

            if (e.roomName.Equals(playerRoom, System.StringComparison.OrdinalIgnoreCase) &&
                e.typeName.Equals(playerAnomaly, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log("Match found!");
                return true;
            }
        }

        Debug.Log("No match found.");
        return false;
    }
}






