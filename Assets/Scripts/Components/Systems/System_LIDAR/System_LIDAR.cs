using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Arduino;

public class System_LIDAR : MonoBehaviour
{
    private ArduinoInput m_LidarButton;
    void Awake()
    {
        m_LidarButton = new ArduinoInput(InputType.Digital, 43, 9, "LIDAR Button");
        m_LidarButton.EOnButtonPressed += OnButtonPressed;
    }

    void OnButtonPressed(int pin)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
