using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTimer;
using Uduino;
using Rover.Arduino;
public class TestingSemaphoreTimeout : MonoBehaviour
{
    bool isOn = false;
    void Start()
    {
        UduinoManager.Instance.OnBoardConnected += OnBoardConnected;
    }

    void OnBoardConnected(UduinoDevice device)
    {
        UduinoManager.Instance.pinMode(13, PinMode.Output);
        Timer.Register(1f, () => SendEvent(), isLooped: true);
    }

    void SendEvent()
    {
        isOn = !isOn;
        //UduinoManager.Instance.digitalWrite(13,isOn? 255 : 0);
        LEDManager.SetLEDMode(48, isOn? 1 : 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
