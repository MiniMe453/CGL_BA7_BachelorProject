using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Arduino;
using Uduino;

namespace Rover.Systems
{
    public enum CameraMode {Cam1, Cam2, Cam3, Cam4};
    public class System_CAM : MonoBehaviour
    {
        private static CameraMode m_cameraMode;
        public static CameraMode SelectedCameraMode {get {return m_cameraMode;}}
        private ArduinoInput m_Cam1Button;
        private ArduinoInput m_Cam2Button;
        private ArduinoInput m_Cam3Button;
        private ArduinoInput m_Cam4Button;
        private ArduinoInput m_CapturePhotoButton;

        private List<ArduinoInput> inputs = new List<ArduinoInput>();
        private List<int> m_ledPins = new List<int>() {52, 50, 48, 46};

        void Start()
        {
            m_Cam1Button = new ArduinoInput(InputType.Digital, 53, 0, "Cam 1 Button");
            m_Cam2Button = new ArduinoInput(InputType.Digital, 51, 1, "Cam 2 Button");
            m_Cam3Button = new ArduinoInput(InputType.Digital, 49, 2, "Cam 3 Button");
            m_Cam4Button = new ArduinoInput(InputType.Digital, 47, 3, "Cam 4 Button");
            m_CapturePhotoButton = new ArduinoInput(InputType.Digital, 45, 4, "Take Photo Button");


            inputs.Add(m_Cam1Button);
            inputs.Add(m_Cam2Button);
            inputs.Add(m_Cam3Button);
            inputs.Add(m_Cam4Button);
            inputs.Add(m_CapturePhotoButton);

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
            foreach(int i in m_ledPins)
            {
                LEDManager.SetLEDMode(i, 0);
            }
            
            //This doesn't work because we need to manually send and decode the data from here
            //We aren't using the default Uduion sketch anymore.
            LEDManager.SetLEDMode(m_ledPins[0], 1);

            Debug.Log("Connected");
        }

        public void SelectNewCameraMode(CameraMode newMode)
        {
            if(newMode == SelectedCameraMode)
                return;

            m_cameraMode = newMode;

            Debug.LogError("Selected Rover Camera is now "+m_cameraMode);
        }

        private void OnButtonPressed(int pin)
        {
            if(pin >= 47)
            {
                foreach(int i in m_ledPins)
                {
                    if(i == pin - 1)
                    {
                        LEDManager.SetLEDMode(i, 1);
                    }
                    else
                    {
                        LEDManager.SetLEDMode(i, 0);
                    }
                }
                
                SelectNewCameraMode((CameraMode)((pin-47)/2));
                return;
            }

                
            Debug.Log("Start showing the picture!");
        }
    }

}

