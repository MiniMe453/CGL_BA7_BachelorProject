using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.DateTime;
using Uduino;

public class WriteToArduinoDisplayTest : MonoBehaviour
{
    public object[] lcdData = new object[2];
    public int counter;
    void Start()
    {
        lcdData[0] = "";
        lcdData[1] = "";
        TimeManager.EOnDateTimeUpdated += OnNewTime;
    }

    void OnNewTime(DateTimeStruct time)
    {
        counter++;
        string tmp = time.Years.ToString() + "y:" + time.Days.ToString() + "d:"+time.Hours.ToString("00")+":"+time.Minutes.ToString("00")+":"+time.Seconds.ToString("00");
        lcdData[0] = tmp;
        Debug.Log(tmp);
        lcdData[1] = counter.ToString("00000");

        UduinoManager.Instance.sendCommand("lcd", lcdData);
    }
}
