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

    [Header("Dropdowns")]
    public TMP_Dropdown roomDropdown;      // Player selects room type here
    public TMP_Dropdown anomalyDropdown;   // Player selects anomaly type here

    [Header("References")]
    public EntitySpawner spawner; // reference to EntitySpawner

    [Header("Settings")]
    public float initialDelay = 5f;
    public float displayTime = 3f;

    private Coroutine currentCoroutine;

    private void Awake()
    {
        stunImage.gameObject.SetActive(false);
        noAnomalyText.gameObject.SetActive(false);
        reportButton.gameObject.SetActive(false);

        reportButton.onClick.AddListener(OnReportButtonClicked);
    }

    private void OnReportButtonClicked()
    {
        TriggerReport();
    }

    public void TriggerReport()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(ReportPending());
    }

    private IEnumerator ReportPending()
    {
        yield return new WaitForSeconds(initialDelay);
        Debug.Log("Checking for anomalies...");

        // Get the player's selections from dropdowns
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

        reportButton.gameObject.SetActive(true);
        gameObject.SetActive(false);

        currentCoroutine = null;
    }

    private bool CheckAnomaly(string playerRoom, string playerAnomaly)
    {
        // Check spawner lists for matching room and anomaly type
        for (int i = 0; i < spawner.roomActivated.Count; i++)
        {
            if (spawner.roomActivated[i] == playerRoom && spawner.typeActivated[i] == playerAnomaly)
            {
                return true;
            }
        }
        return false;
    }
}

