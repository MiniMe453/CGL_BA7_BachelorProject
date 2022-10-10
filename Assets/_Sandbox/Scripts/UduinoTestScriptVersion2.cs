using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Arduino;
using Uduino;

public class UduinoTestScriptVersion2 : MonoBehaviour
{
    private ArduinoInput buttonTestInput;
    private int ledPin = 52;
    private bool pressed;

    void Start()
    {
        buttonTestInput = new ArduinoInput(InputType.Digital, 53, "Button Test Input");
        UduinoManager.Instance.OnBoardConnected += OnBoardConnected;

        buttonTestInput.EOnButtonPressed += OnButtonPressed;
        buttonTestInput.EOnButtonReleased += OnButtonReleased;
        buttonTestInput.EnableInput();
    }

    void OnBoardConnected(UduinoDevice device)
    {
        Uduino.UduinoManager.Instance.pinMode(ledPin, PinMode.Output);
        Debug.Log("Connected");
    }

    private void OnButtonPressed()
    {
        Debug.Log("Button pressed!");
                UduinoManager.Instance.digitalWrite(ledPin, 255);
        pressed = true;
    }

    private void OnButtonReleased()
    {
        Debug.Log("Button released!");
                    UduinoManager.Instance.digitalWrite(ledPin, 0);
        pressed = false;
    }

//     void Update()
//     {
//         if(pressed)
// x
//         else

//     }
}
