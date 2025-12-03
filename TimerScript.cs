using UnityEngine;
using TMPro;
using System;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TimerText;

    private VictoryManager victoryManager;

    private float MinuteProgression;
    private int MinuteTimer = 0;
    private int HourTimer = 0;

    void Start()
    {
        HourTimer = 0;
        MinuteTimer = 0;
        MinuteProgression = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        TimePasses();

        TimerText.text = $"{HourTimer:00} : {MinuteTimer:00}";

        if(HourTimer == 6)
        {
            GameWon();
        }

    }

    private void GameWon()
    {
       victoryManager.HaveWon = true;
    }

    private void TimePasses()
    {
        MinuteProgression += 0.2f * Time.deltaTime;

        if (MinuteProgression >= 1f)
        {
            MinuteTimer += 1;
            MinuteProgression = 0f;
        } 

        if(MinuteTimer >= 60)
        {
            MinuteTimer = 0;
            HourTimer += 1;
        }
    }
}
