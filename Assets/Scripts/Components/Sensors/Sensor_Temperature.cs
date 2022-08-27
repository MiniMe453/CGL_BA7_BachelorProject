using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Can;
using UnityTimer;
using Rover.Temperature;

public class Sensor_Temperature : MonoBehaviourCan
{
    protected override void Init()
    {
        Timer.Register(0.5f, () => ReadTemperatureData(), isLooped: true);
    }

    void ReadTemperatureData()
    {
        object[] data = new object[] { Temperature.ReadTemperatureFromLocation(transform.position) };

        node.CANData = data;
        node.SendCANFrame();
    }
}
