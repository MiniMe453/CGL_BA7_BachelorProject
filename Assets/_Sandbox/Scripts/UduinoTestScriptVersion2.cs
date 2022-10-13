using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Arduino;
using Uduino;

public class UduinoTestScriptVersion2 : MonoBehaviour
{
    private ArduinoInput buttonTestInput;
    private ArduinoInput testInput2;
    private ArduinoInput testInput3;
    private ArduinoInput testInput4;

    private List<ArduinoInput> inputs = new List<ArduinoInput>();
    private List<int> ledPins = new List<int>() {52, 50, 48, 46};

    void Start()
    {
        buttonTestInput = new ArduinoInput(InputType.Digital, 53, 0, "Button Test Input");
        testInput2 = new ArduinoInput(InputType.Digital, 51, 1, "Test Input 2");
        testInput3 = new ArduinoInput(InputType.Digital, 49, 2, "Test Input 3");
        testInput4 = new ArduinoInput(InputType.Digital, 47, 3, "Test Input 4");


        inputs.Add(buttonTestInput);
        inputs.Add(testInput2);
        inputs.Add(testInput3);
        inputs.Add(testInput4);

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
            LEDManager.SetLEDMode(i, 0);
        }
        
        //This doesn't work because we need to manually send and decode the data from here
        //We aren't using the default Uduion sketch anymore.
        LEDManager.SetLEDMode(ledPins[0], 1);

        Debug.Log("Connected");
    }

    void OnApplicationQuit()
    {
        foreach(int i in ledPins)
        {
            UduinoManager.Instance.digitalWrite(i, 0);
        }
    }

    private void OnButtonPressed(int pin)
    {
        foreach(int i in ledPins)
        {
            if(i == pin - 1)
            {
                Debug.Log("Button with pin number " + pin.ToString() + " was pressed!");
                LEDManager.SetLEDMode(i, 1);
            }
            else
            {
                LEDManager.SetLEDMode(i, 0);
            }
        }
    }
}
