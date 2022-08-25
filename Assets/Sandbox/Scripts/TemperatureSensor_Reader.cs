using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Can;
using Rover.Settings;

public class TemperatureSensor_Reader : CanNode
{
    private List<int> temperatureHistory = new List<int>();
    bool timeoutMessageShown = false;
    void Awake()
    {
        InitializeCANNode(0x00bb, CanNetwork.Can0);
        SetRXFilter(new List<ushort>() { 0x000a });
    }

    protected override void OnCANFrameRead(object[] data)
    {
        temperatureHistory.Add((int)data[0]);
        Debug.Log($"Message Recieved: {(int)(TimeSinceLastMessage * 100)}, {(int)data[0]}");
    }

    void Update()
    {
        if (TimeSinceLastMessage > GameSettings.MESSAGE_TIMEOUT && !timeoutMessageShown)
        {
            timeoutMessageShown = true;
            Debug.LogError("Time since last message is high. Perhaps there is a problem with the temperature sensor!");
        }
    }
}
