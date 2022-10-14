using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.OS;
using Rover.Systems;
using UnityEngine.UI;
using Rover.Can;
using UnityTimer;
using TMPro;
using UnityEngine.InputSystem;
using Rover.Settings;

namespace Rover.OS
{
    public class System_CAM_PhotoViewer : MonoBehaviourApplication
    {
        public Canvas canvas;
        public System_CAM cameraSystem;
        public RawImage loadingPhoto;
        public RawImage photo;
        public TextMeshProUGUI photoName;
        public GameObject overlay;
        private int m_currentPhotoCount;

        protected override void Init()
        {
            CanNetwork.Can0.CAN_MESSAGE_RECIEVED += OnMessageReceived;

            applicationInputs.AddAction("goleft", binding:"<Keyboard>/leftArrow");
            applicationInputs.AddAction("goright", binding:"<Keyboard>/rightArrow");

            applicationInputs["goleft"].performed += NavigateLeft;
            applicationInputs["goright"].performed += NavigateRight;
        }

        void OnMessageReceived(CanFrame frame)
        {
            if(frame.nodeID != CanIDs.SYSTEM_CAM)
                return;

            LoadApp();
        }

        protected override void OnAppLoaded()
        {
            LoadPhoto(cameraSystem.m_photos.Count-1);
            m_currentPhotoCount = cameraSystem.m_photos.Count - 1;
            UIManager.AddToViewport(canvas, 100);
            OperatingSystem.SetUserControl(false);
        }

        protected override void OnAppQuit()
        {
            UIManager.RemoveFromViewport(canvas);
            OperatingSystem.SetUserControl(true);
        }

        private void LoadPhotoUpdate(float secondElapsed, bool loadFromInput = false)
        {
            float percentage = secondElapsed / (loadFromInput? GameSettings.PHOTO_LOAD_TIME : GameSettings.PHOTO_VIEWER_LOAD_TIME);
        }

        private void LoadPhoto(int index)
        {
            photo.texture = cameraSystem.m_photos[index].photo;
            photoName.text = cameraSystem.m_photos[index].name;
        }

        private void NavigateLeft(InputAction.CallbackContext callback)
        {
            if(m_currentPhotoCount + 1 > cameraSystem.m_photos.Count - 1)
                return;

            loadingPhoto.texture = cameraSystem.m_photos[m_currentPhotoCount].photo;
            m_currentPhotoCount++;
            LoadPhoto(m_currentPhotoCount);
        }

        private void NavigateRight(InputAction.CallbackContext callback)
        {
            if(m_currentPhotoCount - 1 < 0 )
                return;
            
            loadingPhoto.texture = cameraSystem.m_photos[m_currentPhotoCount].photo;
            m_currentPhotoCount--;
            LoadPhoto(m_currentPhotoCount);
        }
    }
}

