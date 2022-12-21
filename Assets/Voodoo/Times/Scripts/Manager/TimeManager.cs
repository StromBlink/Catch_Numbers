using System;
using System.Collections.Generic;
using UnityEngine;
using Voodoo.Data.BinaryPrefs;

namespace Voodoo.Events
{
    public static class TimeManager
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            MonoBehaviourTimeKeeper timeKeeper = new GameObject().AddComponent<MonoBehaviourTimeKeeper>();
            timeKeeper.gameObject.name = "TimeKeeper";
            timeKeeper.fixedUpdate += FixedUpdate;
        }
        
        private const string LastDayKey           = "TimeManager_LastDay";
        private const string DayCountKey          = "TimeManager_DayCount";
        private const string SessionCountKey      = "TimeManager_SessionCount";
        private const string DailySessionCountKey = "TimeManager_DailySessionCount";

        private const string suffixDays           = "days";
        private const string suffixHours          = "h";
        private const string suffixMinutes        = "m";
        private const string suffixSeconds        = "s";

        private static readonly List<int>   pausedContext  = new List<int>();
        private static readonly List<Timer> timers         = new List<Timer>();
        private static readonly List<Timer> timersToAdd    = new List<Timer>();
        private static readonly List<Timer> timersToRemove = new List<Timer>();

        public static int DayCount          => BinaryPrefs.GetInt(DayCountKey, 1);
        public static int DailySessionCount => BinaryPrefs.GetInt(DailySessionCountKey);
        public static int SessionCount      => BinaryPrefs.GetInt(SessionCountKey);

        static TimeManager()
        {
            double lastOADay = BinaryPrefs.GetDouble(LastDayKey);
            DateTime lastDay = lastOADay == 0.0 ? DateTime.MinValue : DateTime.FromOADate(lastOADay);
            bool isNextDay = DateTime.Now.Year != lastDay.Year ||
                             DateTime.Now.Month != lastDay.Month ||
                             DateTime.Now.Day != lastDay.Day;
       
            if (lastDay == DateTime.MinValue || isNextDay)
            {
                IncreaseDayCount();
            }
            else
            {
                BinaryPrefs.SetInt(DailySessionCountKey, BinaryPrefs.GetInt(DailySessionCountKey) + 1);
                BinaryPrefs.SetInt(SessionCountKey, BinaryPrefs.GetInt(SessionCountKey) + 1);
            }
        }

        /// <summary>
        /// Reset prefs to initial values
        /// </summary>
        public static void ResetValues()
        {
            BinaryPrefs.SetDouble(LastDayKey, DateTime.Now.ToOADate());
            BinaryPrefs.SetInt(DayCountKey, 1);
            BinaryPrefs.SetInt(DailySessionCountKey, 1);
            BinaryPrefs.SetInt(SessionCountKey, 1);
        }

        /// <summary>
        /// Force Daycount increase in game
        /// </summary>
        public static void IncreaseDayCount()
        {
            BinaryPrefs.SetDouble(LastDayKey, DateTime.Now.ToOADate());
            BinaryPrefs.SetInt(DayCountKey, BinaryPrefs.GetInt(DayCountKey) + 1);
            BinaryPrefs.SetInt(DailySessionCountKey, 1);
            BinaryPrefs.SetInt(SessionCountKey, BinaryPrefs.GetInt(SessionCountKey) + 1);
        }

        /// <summary>
        /// Force Daycount increase in prefs
        /// </summary>
        public static void IncreaseDayCountExternal()
        {
            BinaryPrefs.SetDouble(LastDayKey, DateTime.Now.ToOADate());
            BinaryPrefs.SetInt(DayCountKey, BinaryPrefs.GetInt(DayCountKey) + 1);
            BinaryPrefs.SetInt(DailySessionCountKey, 0);
        }

        /// <summary>
        /// Add a timer to the updated collection of timer, should never be called outside of the timer class
        /// </summary>
        /// <param name="timer">timer instance to add</param>
        public static void AddTimer(Timer timer)
        {
            if (timers.IndexOf(timer) >= 0 && timersToAdd.IndexOf(timer) >= 0) 
            {
                return;
            }

            timersToAdd.Add(timer);
        }

        /// <summary>
        /// Remove a timer from the updated collection of timer, should never be called outside of the timer class
        /// </summary>
        /// <param name="timer">timer instance to remove</param>
        public static void RemoveTimer(Timer timer)
        {
            if (timers.IndexOf(timer) < 0 && timersToRemove.IndexOf(timer) < 0)
            {
                return;
            }

            timersToRemove.Add(timer);
        }
                
        private static void FixedUpdate()
        {
            for (int i = 0; i < timersToAdd.Count; i++)
            {
                timers.Add(timersToAdd[i]);
            }
            timersToAdd.Clear();

            for (int i = 0; i < timersToRemove.Count; i++)
            {
                timers.Remove(timersToRemove[i]);
            }
            timersToRemove.Clear();

            if (timers.Count == 0)
            {
                return;
            }
            
            for (int i = 0; i < timers.Count; i++)
            {
                timers[i].Update(Time.fixedDeltaTime);
            }
        }
        
        /// <summary>
        /// Return a TimeSpan of the interval between startDate and endDate.
        /// If negative, it will return a TimeSpan.Zero.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static TimeSpan GetInterval(DateTime startDate, DateTime endDate)
        {
            TimeSpan timeSpan = endDate.Subtract(startDate);

            if (timeSpan.TotalSeconds < 0)
            {
                timeSpan = TimeSpan.Zero;
            }

            return timeSpan;
        }
        
        /// <summary>
        /// Return the interval between startDate and endDate in seconds
        /// If negative, it will return a 0
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static double GetIntervalSeconds(DateTime startDate, DateTime endDate)
        {
            return GetInterval(startDate, endDate).TotalSeconds;
        }

        /// <summary>
        /// Return true if the startDate is superior or equal to the endDate
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="_endTime"></param>
        /// <returns></returns>
        public static bool IsDelayExpired(DateTime startDate, DateTime endDate)
        {
            int value = DateTime.Compare(startDate, endDate); 
  
            // checking 
            if (value >= 0)
            {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Return the interval between startDate and endTime in a string format 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static string GetCountdownDisplayValue(DateTime startDate, DateTime endTime)
        {
            TimeSpan timeSpan = endTime.Subtract(startDate);
            
            if (timeSpan.TotalDays > 1.0)
            {
                if (timeSpan.Hours > 0 || timeSpan.Minutes > 0 || timeSpan.Seconds>0)
                {
                    return (int)(timeSpan.TotalDays+1) + suffixDays;
                }

                return (int)timeSpan.TotalDays + suffixDays;
            }
            if (timeSpan.TotalHours > 1.0)
            {
                if (timeSpan.Minutes > 0 || timeSpan.Seconds>0)
                {
                    return (int)(timeSpan.TotalHours+1) + suffixHours;
                }

                return (int)timeSpan.TotalHours + suffixHours;
            }
            if (timeSpan.TotalMinutes > 1.0)
            {
                if (timeSpan.Seconds > 0)
                {
                    return (int)(timeSpan.TotalMinutes+1) + suffixMinutes;
                }

                return (int)timeSpan.TotalMinutes + suffixMinutes;

            }
            if (timeSpan.TotalSeconds > 1.0)
            {
                return (int)timeSpan.TotalSeconds + suffixSeconds;
            }

            return String.Empty;

        }        
    }
}
