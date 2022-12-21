using System;
using TMPro;
using UnityEngine;
using Voodoo.Data.BinaryPrefs;

namespace Voodoo.Events
{
    public class UICountdown : MonoBehaviour
   {
      public delegate void CountdownEnded(); 
      public event CountdownEnded OnCountdownEnded;
      
      public string buttonKey;
      public TextMeshProUGUI countdownDisplayText;
      public bool saveThisCountdown = true;
      public float tickPeriod = 1;
      
      private const string waitForDelayExpiredKey = "_waitForDelay";
      private string countdownDisplayValue;
      private DateTime endDate = DateTime.Now;
      private bool waitForDelayExpired;

      private Timer timer;
      
      private void Start()
      {
         if (timer == null)
         {
            timer = new Timer(tickPeriod, true);            
         }
         
         if (!String.IsNullOrEmpty(buttonKey))
         {
            string _saveString = String.Empty;
            if (saveThisCountdown)
            {
               _saveString = BinaryPrefs.GetString(buttonKey);
               waitForDelayExpired = BinaryPrefs.GetBool(buttonKey + waitForDelayExpiredKey, false);
            }

            if (!String.IsNullOrEmpty(_saveString))
            {
               SetEndDate(Convert.ToDateTime(_saveString));

               if(DateTime.Compare(DateTime.Now, endDate) < 0)
               {
                  StartCountdown();
               }
               else if(waitForDelayExpired)
               {
                  ResetCountdown();
               }
            }
         }
         else
         {
            Debug.LogError("You need to specify a buttonName for the save");
         }
      }

      private void NewTick(Timer _timer)
      {
         countdownDisplayValue = TimeManager.GetCountdownDisplayValue(DateTime.Now, endDate);
         
         UpdateUI();
         
         if (DateTime.Compare(DateTime.Now, endDate) > 0)
         {
            timer.Stop();
            OnCountdownEnded?.Invoke();
            if (saveThisCountdown)
            {
               BinaryPrefs.SetString(buttonKey, endDate.ToString());
            }
         }
      }

      private void UpdateUI()
      {
         if (countdownDisplayText != null)
         {
            countdownDisplayText.text = countdownDisplayValue;
         }
      }

      /// <summary>
      /// Return true if the countdown is not done
      /// </summary>
      /// <returns></returns>
      public bool IsCountdownRunning()
      {
         if (TimeManager.GetIntervalSeconds(DateTime.Now, endDate) > 0)
         {
            return true;
         }

         return false;
      }

      /// <summary>
      /// Start the countdown, need to have a End Date
      /// </summary>
      public void StartCountdown()
      {
         if (timer == null)
         {
            timer = new Timer(tickPeriod, true);
         }
         
         if (saveThisCountdown)
         {
            BinaryPrefs.SetString(buttonKey, endDate.ToString());
         }

         countdownDisplayValue = TimeManager.GetCountdownDisplayValue(DateTime.Now, endDate);

         waitForDelayExpired = true;
         if (saveThisCountdown)
         {
            BinaryPrefs.SetBool(buttonKey + waitForDelayExpiredKey, waitForDelayExpired);
         }

         timer.Looped += NewTick;
         timer.Start();

         UpdateUI();
      }
      
      /// <summary>
      /// Stop the countdown
      /// </summary>
      public void StopCountdown()
      {
          timer.Stop();
      }
      
      /// <summary>
      /// Reset the countdown, set the End Date to Now, trigger the OnEndWait event
      /// </summary>
      public void ResetCountdown()
      {
         SetEndDate(DateTime.Now);

         if (saveThisCountdown)
         {
            BinaryPrefs.SetString(buttonKey, endDate.ToString());
         }

         timer.Stop();
         
         waitForDelayExpired = false;

         if (saveThisCountdown)
         {
            BinaryPrefs.SetBool(buttonKey + waitForDelayExpiredKey, waitForDelayExpired);
         }

         OnCountdownEnded?.Invoke();
      }
      
      
      /// <summary>
      /// Get the End Date
      /// </summary>
      /// <returns></returns>
      public DateTime GetEndDate()
      {
         return endDate;
      }
      
      #region End Date Setters
      
      /// <summary>
      /// Set a new End Date
      /// </summary>
      /// <param name="_endDate"></param>
      public void SetEndDate(DateTime _endDate)
      {
         endDate = _endDate;
      }
      
      /// <summary>
      /// Set a new End Date by adding a TimeSpan
      /// </summary>
      /// <param name="_timeSpan"></param>
      public void AddTimeSpan(TimeSpan _timeSpan)
      {
         endDate = endDate.Add(_timeSpan);
      }

      /// <summary>
      /// Reset the End Date to DateTime.Now
      /// </summary>
      public void ResetEndDate()
      {
         endDate = DateTime.Now;
      }
      

      /// <summary>
      /// Set a new End Date to the first second of tomorrow
      /// </summary>
      public void SetEndDateForTomorrow()
      {
         endDate = DateTime.Today.AddDays(1);
      }
      
      /// <summary>
      /// Add days to the End Date
      /// </summary>
      /// <param name="_days"></param>
      public void AddDays(int _days = 1)
      {
         endDate = endDate.AddDays(_days);
      }
      
      /// <summary>
      /// Add hours to the End Date
      /// </summary>
      /// <param name="_hours"></param>
      public void AddHours(int _hours = 1)
      {
         endDate = endDate.AddHours(_hours);
      }
      
      /// <summary>
      /// Add minutes to the End Date
      /// </summary>
      /// <param name="_minutes"></param>
      public void AddMinutes(int _minutes = 1)
      {
         endDate = endDate.AddMinutes(_minutes);
      }
      
      /// <summary>
      /// Add seconds to the End Date
      /// </summary>
      /// <param name="_seconds"></param>
      public void AddSeconds(int _seconds = 1)
      {
         endDate = endDate.AddSeconds(_seconds);
      }

      #endregion

   }
}