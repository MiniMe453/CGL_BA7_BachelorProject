using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Arduino;

public class UduinoTestScript3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ArduinoInputDecoder.EOnSerialMessageRecieved += TestFunct;
    }

    void TestFunct(string data)
    {
        Debug.Log(data);
    }
}
