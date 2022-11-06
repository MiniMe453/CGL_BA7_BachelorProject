using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Arduino;

namespace Rover.Systems
{
    public enum JoystickControlMode {RVR, CAM};
    public class System_MTR : MonoBehaviour
    {
        private ArduinoInput m_throttleControl;
        private ArduinoInput m_horizontalControl;
        private ArduinoInput m_verticalControl;
        private ArduinoInput m_RVRButton;
        private ArduinoInput m_CAMButton;
        private ArduinoInput m_brakeSwitch;
        private JoystickControlMode m_joystickControlMode = JoystickControlMode.RVR;

        private float m_ForwardAxis;
        private float m_HorizontalAxis;
        private float m_VerticalAxis;

        [Header("Vehicle Variables")]
        private Rigidbody m_rigidbody;
        public List<WheelCollider> wheelColliders = new List<WheelCollider>();
        public float motorForce;
        public float steeringAngle;
        public float maxSpeed = 3f;
        public float brakeForce;
        private bool m_brakeActivated;

        [Header("Camera Variables")]
        public float cameraRotateSpeed;
        public GameObject YAxisObject;
        public GameObject XAxisObject;
        public GameObject navigationCamera;
        public GameObject photoCamera;
        private float m_xRot;
        private float m_yRot;

        #region Metadata
        private static float m_roverVelocity;
        public static float RoverVelocity {get{return m_roverVelocity;}}
        private static float m_roverPitch;
        public static float RoverPitch {get{return m_roverPitch;}}
        private static float m_roverRoll;
        public static float RoverRoll {get{return m_roverRoll;}}

        #endregion

        void Start()
        {
            m_throttleControl = new ArduinoInput(InputType.Analog, 127, 1, "Motor Throttle Control");
            m_throttleControl.EOnValueChanged += OnValueChanged;

            m_horizontalControl = new ArduinoInput(InputType.Analog, 128, 2, "Horizontal Joystick Axis");
            m_horizontalControl.EOnValueChanged += OnValueChanged;

            m_verticalControl = new ArduinoInput(InputType.Analog, 129, 3, "Vertical Joystick Axis");
            m_verticalControl.EOnValueChanged += OnValueChanged;

            m_CAMButton = new ArduinoInput(InputType.Digital, 39, 7, "Cam Button");
            m_CAMButton.EOnButtonPressed += OnButtonPressed;

            m_RVRButton = new ArduinoInput(InputType.Digital, 41, 6, "Rvr Button");
            m_RVRButton.EOnButtonPressed += OnButtonPressed;

            m_brakeSwitch = new ArduinoInput(InputType.Digital, 37, 8, "Brake Switch");
            m_brakeSwitch.EOnButtonPressed += OnButtonPressed;
            m_brakeSwitch.EOnButtonReleased += OnButtonReleased;

            m_rigidbody = gameObject.GetComponent<Rigidbody>();
            
        }    

        void FixedUpdate()
        {
            HandleMotor();

            switch(m_joystickControlMode)
            {
                case JoystickControlMode.RVR:
                    HandleSteering();
                    break;
                case JoystickControlMode.CAM:
                    RotateCamera();
                    break;
            }

            m_roverVelocity = m_rigidbody.velocity.sqrMagnitude;
            m_roverPitch = gameObject.transform.localRotation.x;
            m_roverRoll = gameObject.transform.localRotation.z;
        }

        void OnValueChanged(float newValue, int pin)
        {
            if(pin == 127)
            {
                m_ForwardAxis = (newValue + 1  - 512)/512;

                if(Mathf.Abs(m_ForwardAxis) < .1)
                    m_ForwardAxis = 0;
            }

            if(pin == 128)
            {
                m_HorizontalAxis = (newValue + 1 - 460)/460;

                if(Mathf.Abs(m_HorizontalAxis) < 0.04)
                    m_HorizontalAxis = 0;

                if(m_HorizontalAxis > 1)
                    m_HorizontalAxis = 1;
            }

            if(pin == 129)
            {
                m_VerticalAxis = (newValue + 1 - 512)/512;

                if(Mathf.Abs(m_VerticalAxis) < 0.05)
                    m_VerticalAxis = 0;
            }
        }
        void OnButtonPressed(int pin)
        {
            if(pin == 39)
            {
                m_joystickControlMode = JoystickControlMode.CAM;
                //ApplyBraking(brakeForce);
                photoCamera.SetActive(true);
                navigationCamera.SetActive(false);
                LEDManager.SetLEDMode(38, 1);
                LEDManager.SetLEDMode(40, 0);
            } else if (pin == 41)
            {
                m_joystickControlMode = JoystickControlMode.RVR;
                navigationCamera.SetActive(true);
                photoCamera.SetActive(false);
                LEDManager.SetLEDMode(38, 0);
                LEDManager.SetLEDMode(40, 1);
            } else if(pin == 37)
            {
                m_brakeActivated = true;
                LEDManager.SetLEDMode(44, 1);
            }
        }

        void OnButtonReleased(int pin)
        {
            if(pin == 37)
            {
                m_brakeActivated = false;
                LEDManager.SetLEDMode(44, 0);
            }
        }

        void HandleMotor()
        {
            if(m_brakeActivated)
            {
                ApplyBraking(brakeForce);
                return;
            }
            
            float currentBrakeForce = m_ForwardAxis == 0? brakeForce : 0;
            float currSpeed = gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude;

            float speedMult = 1 - (currSpeed/maxSpeed);

            foreach(WheelCollider w in wheelColliders)
            {
                w.motorTorque = m_ForwardAxis * motorForce * speedMult;
                w.brakeTorque = currentBrakeForce;
            }
        }

        void ApplyBraking(float force)
        {
            foreach(WheelCollider w in wheelColliders)
            {
                w.brakeTorque = force;
            }
        }

        void HandleSteering()
        {
            float currentSteerAngle = steeringAngle * m_HorizontalAxis;

            for(int i = 0; i < wheelColliders.Count; i++)
            {
                    wheelColliders[i].steerAngle = -currentSteerAngle;
            }
        }

        void RotateCamera()
        {
            m_yRot += -m_HorizontalAxis * cameraRotateSpeed * Time.deltaTime;
            m_xRot += -m_VerticalAxis * cameraRotateSpeed * Time.deltaTime;

            YAxisObject.transform.localRotation = Quaternion.Euler(0, 0, m_yRot);
            XAxisObject.transform.localRotation = Quaternion.Euler(m_xRot, 0, 0);
        }
    }

}
