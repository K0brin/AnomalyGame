using UnityEngine;
using TMPro;
using System;

public class FNAFTimer : MonoBehaviour
{  
   [SerializeField] private TextMeshProUGUI TimerDisplay; 
   private int StartTime = 12;
   private float TimeUntilNextHour;
   private float DisplayTime;

    void Start()
    {
        DisplayTime = StartTime;
        TimeUntilNextHour = 0f;
    }

    void Update()
    {
        TimerDisplay.text = $"{DisplayTime} AM";
        TimeUntilNextHour += Time.deltaTime;

        if(TimeUntilNextHour >= 89f)
        {
            HourManager();
            TimeUntilNextHour = 0f;
        }
    }

    private void HourManager()
    {
        if(DisplayTime == 12)
        {
            DisplayTime = 1;
        }
        else
        {
            DisplayTime += 1;
        }
    }
}
