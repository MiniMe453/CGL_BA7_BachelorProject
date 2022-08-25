using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Can;

public class SensorFailTest : CanNode
{
    void Awake()
    {
        InitializeCANNode(0x00ac, CanNetwork.Can0);
    }

    void Start()
    {
        StartCANTimer(0.6f);
    }

    protected override void PrepareCANFrame()
    {
        canData.Add("Hello world!");
        base.PrepareCANFrame();
    }
}
