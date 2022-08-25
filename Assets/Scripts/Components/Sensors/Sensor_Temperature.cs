using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Can;
using UnityTimer;

public class Sensor_Temperature : CanNode
{
    void Awake()
    {
        InitializeCANNode(0x000a, CanNetwork.Can0);
    }

    void Start()
    {
        StartCANTimer(0.75f);
        Timer.Register(10f, () => DestroyCANNode());
    }

    protected override void PrepareCANFrame()
    {
        canData.Add(100);

        base.PrepareCANFrame();
    }
}
