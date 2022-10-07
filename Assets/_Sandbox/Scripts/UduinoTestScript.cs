using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;

public class UduinoTestScript : MonoBehaviour
{
    // Start is called before the first frame update
    UduinoDevice uDevice;
    int lastValue;
    void Start()
    {
        //UduinoManager.Instance.OnValueReceived += OnValueReceived;
        UduinoManager.Instance.OnBoardConnected += OnBoardConnected;

    }

    void OnBoardConnected(UduinoDevice device)
    {
        uDevice = device;
        Debug.Log("Board Connected " + uDevice.name);
        UduinoManager.Instance.pinMode(53, PinMode.Input_pullup); 
        UduinoManager.Instance.pinMode(52, PinMode.Output);
    }

    void Update()
    {
        if(uDevice == null)
            return;
        
        int value = UduinoManager.Instance.digitalRead(53);

        if(value == lastValue)
            return;
       
        lastValue = value;

        if(lastValue == 0)
            UduinoManager.Instance.digitalWrite(52, 0);
        else if (lastValue == 1)
            UduinoManager.Instance.digitalWrite(52,255);

        Debug.Log("The arduino value is " + lastValue.ToString());
    }
}
