using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Arduino;
using Uduino;

public class UduinoTestScriptVersion2 : MonoBehaviour
{
    private ArduinoInput buttonTestInput;
    private ArduinoInput testInput2;

    private List<ArduinoInput> inputs = new List<ArduinoInput>();
    private List<int> ledPins = new List<int>() {52, 50};
    private int ledPin = 52;
    private bool pressed;

    void Start()
    {
        buttonTestInput = new ArduinoInput(InputType.Digital, 53, "Button Test Input");
        testInput2 = new ArduinoInput(InputType.Digital, 51, "Test Input 2");

        inputs.Add(buttonTestInput);
        inputs.Add(testInput2);

        UduinoManager.Instance.OnBoardConnected += OnBoardConnected;

        foreach(ArduinoInput i in inputs)
        {
            i.EOnButtonPressed += OnButtonPressed;
            //i.EOnButtonReleased += OnButtonReleased;
            i.EnableInput();
        }
    }

    void OnBoardConnected(UduinoDevice device)
    {
        foreach(int i in ledPins)
        {
            Uduino.UduinoManager.Instance.pinMode(i, PinMode.Output); 
            UduinoManager.Instance.digitalWrite(i, 0);
        }
        
        UduinoManager.Instance.digitalWrite(ledPins[0], 255);

        Debug.Log("Connected");
    }

    void OnApplicationQuit()
    {
        foreach(int i in ledPins)
        {
            UduinoManager.Instance.digitalWrite(i, 0);
        }
    }

    void OnButton1Pressed()
    {

    }

    void OnButton2Pressed()
    {

    }

    private void OnButtonPressed(int pin)
    {
        foreach(int i in ledPins)
        {
            if(i == pin - 1)
            {
                UduinoManager.Instance.digitalWrite(i, 255);
            }
            else
            {
                UduinoManager.Instance.digitalWrite(i, 0);
            }
        }
    }

    // private void OnButtonReleased(int pin)
    // {
    //     Debug.Log("Button released!");
    //                 UduinoManager.Instance.digitalWrite(ledPin, 0);
    //     pressed = false;
    // }

//     void Update()
//     {
//         if(pressed)
// x
//         else

//     }
}
