using UnityEngine;
using TMPro;
using System;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private VictoryManager victoryManager; //JB
    [SerializeField] private float mDuration = 0.2f;

    //private VictoryManager victoryManager; //JB changed from tag due to scene reloads creating null with tags.

    private float MinuteProgression;
    private int MinuteTimer = 0;
    private int HourTimer = 0;

    void Start()
    {
        HourTimer = 0;
        MinuteTimer = 0;
        MinuteProgression = 0f;
        //victoryManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<VictoryManager>(); //JB changed from tag due to scene reloads creating null with tags.

    }

    // Update is called once per frame
    void Update()
    {
        
        if (!SPauseMenu.GameIsPaused)        //JB added check for game pause
        {
            TimePasses();  
        }

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
        MinuteProgression += mDuration * Time.deltaTime;

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
