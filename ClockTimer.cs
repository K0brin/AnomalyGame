using UnityEngine;
using TMPro;

public class ClockTimer : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI UITimer;
   //[SerializeField] private int StartingTime;
   private int HourTimer;
   private float MinutesTimer;

   public bool TimeIsUp = false;

   void Start()
   {
        HourTimer = 0;
        MinutesTimer = 0;
        TimeIsUp = false;
   }

   void Update()
   {
     int DisplayMinutes = (int) MinutesTimer;
     string MinutesOnTimer = DisplayMinutes.ToString ("00");

     UITimer.text = $"{HourTimer} : {MinutesOnTimer}";
     MinutesTimer += Time.deltaTime * (60f / 89f);

     if (MinutesTimer >= 60f)   //resets the minutes back to zero after reaching Zero
        {
            MinutesTimer = 0;
            HourManager();
        }

        if (HourTimer == 30)
        {
            TimeIsUp = true;
        }
   }

   private void HourManager() // adds one to the hour
    {
        
            HourTimer += 1;
    
    }

}
