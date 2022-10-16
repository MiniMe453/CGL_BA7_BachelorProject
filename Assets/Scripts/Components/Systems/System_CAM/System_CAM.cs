using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Arduino;
using Uduino;
using Rover.Settings;
using Rover.Can;
using Rover.DateTime;
using Rover.Interface;

namespace Rover.Systems
{
    public enum CameraMode {Cam1, Cam2, Cam3, Cam4};

    public struct Struct_CameraPhoto
    {
        public string name;
        public Texture2D photo;
    }
    public class System_CAM : MonoBehaviourCan
    {
        private static CameraMode m_cameraMode = CameraMode.Cam1;
        public static CameraMode SelectedCameraMode {get {return m_cameraMode;}}
        private ArduinoInput m_Cam1Button;
        private ArduinoInput m_Cam2Button;
        private ArduinoInput m_Cam3Button;
        private ArduinoInput m_Cam4Button;
        private ArduinoInput m_CapturePhotoButton;
        private List<ArduinoInput> inputs = new List<ArduinoInput>();
        private List<int> m_ledPins = new List<int>() {52, 50, 48, 46};
        public List<Camera> cameraList;
        private static int m_screenshotCount;
        public List<Struct_CameraPhoto> m_photos = new List<Struct_CameraPhoto>();
        private CanNode m_canNode = new CanNode(0x2000, "Camera System", CanNetwork.Can0);

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
            SelectNewCameraMode(CameraMode.Cam1);

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

                
            if(System_MTR.RoverVelocity > 0.01)
            {
                Debug.Log(System_MTR.RoverVelocity);
                UIManager.ShowMessageBox("STOP THE ROVER", Color.red, 1f);
            }
            else
            {
                TakeCameraPhoto(cameraList[(int)m_cameraMode]);
            }
        }

        void TakeCameraPhoto(Camera selectedCamera)
        {
            m_screenshotCount++;
            selectedCamera.gameObject.SetActive(true);

            RenderTexture rt = new RenderTexture(GameSettings.GAME_RES_X, GameSettings.GAME_RES_Y, 24);
            selectedCamera.targetTexture = rt;
            Texture2D cameraPhoto = new Texture2D(GameSettings.GAME_RES_X, GameSettings.GAME_RES_Y, TextureFormat.RGB24, false);
            selectedCamera.Render();
            RenderTexture.active = rt;
            
            cameraPhoto.ReadPixels(new Rect(0,0,GameSettings.GAME_RES_X, GameSettings.GAME_RES_Y),0,0);
            cameraPhoto.Apply();

            string photoName = $"{m_cameraMode.ToString()}_{TimeManager.TimeToStringTime(TimeManager.GetCurrentDateTime(), "_")}_{m_screenshotCount.ToString("000")}";

            Struct_CameraPhoto photoMetadata = new Struct_CameraPhoto();
            photoMetadata.name = photoName;
            photoMetadata.photo = cameraPhoto;

            m_photos.Add(photoMetadata);

            selectedCamera.targetTexture = null;
            RenderTexture.active = null;
            selectedCamera.gameObject.SetActive(false);
            Destroy(rt);

            CanFrame frame = new CanFrame();
            frame.data = new object[] {};
            frame.nodeID = CanIDs.SYSTEM_CAM;
            frame.timestamp = TimeManager.GetCurrentDateTime();
            CanNetwork.Can0.SendCANFrame(frame);
        }
    }

}

