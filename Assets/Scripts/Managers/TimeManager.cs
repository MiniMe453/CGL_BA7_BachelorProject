using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTimer;
using System;

namespace Rover.DateTime
{
    public struct DateTimeStruct
    {
        public int Years;
        public int Days;
        public int Hours;
        public int Minutes;
        public int Seconds;

    }
    public static class TimeManager
    {
        private static float m_timeScale = 1f;
        public static float TimeScale { get { return m_timeScale; } set { m_timeScale = value; } }
        public static DateTimeStruct dateTime;
        public static event Action<string> EOnDateTimeUpdated;

        static TimeManager()
        {
            Timer.Register(1f, () => UpdateTime(), isLooped: true);

            DateTimeStruct tmp = new DateTimeStruct();
            tmp.Years = 5;
            tmp.Days = 167;
            tmp.Hours = 8;
            tmp.Minutes = 34;
            tmp.Seconds = 15;

            dateTime = tmp;
        }

        private static void UpdateTime()
        {
            DateTimeStruct tmp = dateTime;

            tmp.Seconds++;

            if (tmp.Seconds == 60)
            {
                tmp.Seconds = 0;
                tmp.Minutes++;
                if (tmp.Minutes == 60)
                {
                    tmp.Minutes = 0;
                    tmp.Hours++;
                    if (tmp.Hours == 23)
                    {
                        tmp.Hours = 0;
                        tmp.Days++;
                        if (tmp.Days == 365)
                        {
                            tmp.Days = 0;
                            tmp.Years++;
                        }
                    }
                }
            }

            dateTime = tmp;

            EOnDateTimeUpdated?.Invoke(TimeToStringFull(dateTime));
        }

        public static string TimeToStringFull(DateTimeStruct dateTimeStruct)
        {
            string year = dateTimeStruct.Years.ToString();
            string day = dateTimeStruct.Days.ToString();
            string hour = dateTimeStruct.Hours.ToString();
            string minute = dateTimeStruct.Minutes.ToString();
            string seconds = dateTimeStruct.Seconds.ToString();

            if (dateTimeStruct.Hours < 10)
                hour = "0" + hour;

            if (dateTimeStruct.Minutes < 10)
                minute = "0" + minute;

            if (dateTimeStruct.Seconds < 10)
                seconds = "0" + seconds;

            if (dateTimeStruct.Days < 10)
                day = "00" + day;
            else if (dateTimeStruct.Days < 100)
                day = "0" + day;

            if (dateTimeStruct.Years < 10)
                year = "0" + year;


            return year + "y, " + day + "d, " + hour + ":" + minute + ":" + seconds;
        }

        public static string TimeToStringYD(DateTimeStruct dateTimeStruct)
        {
            string year = dateTimeStruct.Years.ToString();
            string day = dateTimeStruct.Days.ToString();

            if (dateTimeStruct.Days < 10)
                day = "00" + day;
            else if (dateTimeStruct.Days < 100)
                day = "0" + day;

            if (dateTimeStruct.Years < 10)
                year = "0" + year;

            return year + "y, " + day + "d";
        }

        public static string TimeToStringTime(DateTimeStruct dateTimeStruct)
        {
            string hour = dateTimeStruct.Hours.ToString();
            string minute = dateTimeStruct.Minutes.ToString();
            string seconds = dateTimeStruct.Seconds.ToString();

            if (dateTimeStruct.Hours < 10)
                hour = "0" + hour;

            if (dateTimeStruct.Minutes < 10)
                minute = "0" + minute;

            if (dateTimeStruct.Seconds < 10)
                seconds = "0" + seconds;


            return hour + ":" + minute + ":" + seconds;
        }

        public static DateTimeStruct GetCurrentDateTime()
        {
            return dateTime;
        }
    }
}
