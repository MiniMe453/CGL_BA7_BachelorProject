using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Arduino;


public class CarController : MonoBehaviour
{

    private ArduinoInput m_throttleControl;
    private ArduinoInput m_steeringControl;
    private float m_ForwardAxis;
    private float m_SteeringAxis;

    public List<WheelCollider> wheelColliders = new List<WheelCollider>();
    public float motorForce;
    public float steeringAngle;
    public float maxSpeed = 3f;
    public float brakeForce;
    
    private float axis = 0;

    void Start()
    {
        m_throttleControl = new ArduinoInput(InputType.Analog, 127, 1, "Motor Throttle Control");
        m_throttleControl.EOnValueChanged += OnThrottleInputChanged;
        m_throttleControl.EnableInput();

        m_steeringControl = new ArduinoInput(InputType.Analog, 128, 2, "Steering Angle Control");
        m_steeringControl.EOnValueChanged += OnThrottleInputChanged;
        m_steeringControl.EnableInput();
        
    }    

    void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();

        //Debug.LogError(gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude);
        //Debug.LogError(m_ForwardAxis);
    }

    void OnThrottleInputChanged(float newValue, int pin)
    {
        if(pin == 127)
        {
            m_ForwardAxis = (newValue + 1  - 512)/512;

            if(Mathf.Abs(m_ForwardAxis) < .1)
                m_ForwardAxis = 0;
        }

        if(pin == 128)
        {
            m_SteeringAxis = (newValue + 1 - 460)/460;

            if(Mathf.Abs(m_SteeringAxis) < 0.04)
                m_SteeringAxis = 0;

            if(m_SteeringAxis > 1)
                m_SteeringAxis = 1;
        }
    }

    void GetInput()
    {
        // axis = 0;

        // if(Input.GetKeyDown(KeyCode.A))
        // {
        //     axis = -1;
        // }

        // if(Input.GetKeyDown(KeyCode.D))
        // {
        //     axis = 1;
        // }
    }

    void HandleMotor()
    {
        float currentBrakeForce = m_ForwardAxis == 0? brakeForce : 0;
        float currSpeed = gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude;

        float speedMult = 1 - (currSpeed/maxSpeed);

        foreach(WheelCollider w in wheelColliders)
        {
            w.motorTorque = m_ForwardAxis * motorForce * speedMult;
            w.brakeTorque = currentBrakeForce;

                        Debug.LogError(w.name + " " + w.brakeTorque);
        }
    }

    void ApplyBraking(float force)
    {
        foreach(WheelCollider w in wheelColliders)
        {
            w.brakeTorque = force;

            Debug.LogError(w.name + " " + w.brakeTorque);
        }
    }

    void HandleSteering()
    {
        float currentSteerAngle = steeringAngle * m_SteeringAxis;

        for(int i = 0; i < wheelColliders.Count; i++)
        {
                wheelColliders[i].steerAngle = -currentSteerAngle;
        }
    }
}
