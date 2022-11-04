using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.DateTime;
using Uduino;
using Rover.Systems;

public class WriteDataToDisplay : MonoBehaviour
{
    public object[] lcdData = new object[2];
    void Start()
    {
        lcdData[0] = "";
        lcdData[1] = "";
        TimeManager.EOnDateTimeUpdated += OnNewTime;
    }

    void OnNewTime(DateTimeStruct time)
    {
        string timeStr = time.Years.ToString() + "y:" + time.Days.ToString() + "d:"+time.Hours.ToString("00")+":"+time.Minutes.ToString("00")+":"+time.Seconds.ToString("00");
        string gpsStr = System_GPS.GPSCoordinates.x.ToString("00.000") + ":" + System_GPS.GPSCoordinates.y.ToString("00.000");
        lcdData[0] = timeStr;
        lcdData[1] = gpsStr;

        UduinoManager.Instance.sendCommand("lcd", lcdData);
    }
}
