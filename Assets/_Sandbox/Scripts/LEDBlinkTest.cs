using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTimer;
using Rover.Arduino;
using Uduino;

public class LEDBlinkTest : MonoBehaviour
{
    bool isOn;

    void Start()
    {
        UduinoManager.Instance.OnBoardConnected += OnBoardConnected;           
    }

    void OnBoardConnected(UduinoDevice device)
    {
        Timer.Register(1f, () => {
            isOn = !isOn;

            if(isOn)
            {
                LEDManager.SetLEDMode(22, 1);
            }
            else
            {
                LEDManager.SetLEDMode(22, 0);
            }
        }, isLooped: true);
    }
}
