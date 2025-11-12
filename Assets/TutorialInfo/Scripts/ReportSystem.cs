using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReportSystem : MonoBehaviour
{
    public Image stunImage; //if detects anomaly, plays image for 5 seconds
    public Button reportButton;
    public TextMeshProUGUI noAnomalyText;
    private Coroutine currentCoroutine;
    
    // bool for detecting if anomaly bIsActive (or just reference bIsActive)

    public void Awake()
    {
        stunImage.gameObject.SetActive(false);
        noAnomalyText.gameObject.SetActive(false);
        reportButton.gameObject.SetActive(false);
        TriggerReport();
    }
    private void TriggerReport()
    {
        if(!gameObject.activeSelf)
            gameObject.SetActive(true);

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(ReportPending());
    }

    private IEnumerator ReportPending()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("5 seconds have passed!");
        // if anomaly detected code
        // stunImage.gameObject.SetActive(true);
        // if not
        // noAnomalyText.text = "No anomaly of type" + anomalyType + "found in" + roomType;
        reportButton.gameObject.SetActive(true);
        gameObject.SetActive(false);
        currentCoroutine = null;
    }
}
