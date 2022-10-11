using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Arduino;
using Uduino;

public class UduinoTestScript3 : MonoBehaviour
{
    public int[] ledPins = new int[] {52, 50, 48, 46};
    void Start()
    {
        ArduinoInputDecoder.EOnSerialMessageRecieved += TestFunct;
    }

    void TestFunct(string data)
    {
        Debug.Log(data);
    }
}
