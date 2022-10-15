using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Arduino;

public class EncoderTesting : MonoBehaviour
{
    private ArduinoInput m_encoderButton;


    void Start()
    {
        m_encoderButton = new ArduinoInput(InputType.Digital, 11, 5, "Radio Button");

        m_encoderButton.EOnButtonPressed += OnEncoderButtonPressed;
        m_encoderButton.EnableInput();
    }


    void OnEncoderButtonPressed(int pin)
    {
        Debug.LogError("Encoder Button Pressed");
    }

    void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
    }

    void GetInput()
    {

    }

    void HandleMotor()
    {

    }

    void HandleSteering()
    {

    }
}
