using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.OS;
using Rover.Interface;
using UnityEngine.InputSystem;

namespace Rover.OS
{
    public class TemperatureApplication : MonoBehaviourApplication
    {
        public Canvas canvas;
        public TemperatureSensor_Reader reader;
        private bool showingListView;
        protected override void Init()
        {
            applicationInputs["pause"].performed += PauseGraph;
            applicationInputs["start"].performed += StartGraph;
            applicationInputs["clear"].performed += ClearGraph;
            applicationInputs["list"].performed += ShowListView;
        }

        private void PauseGraph(InputAction.CallbackContext context)
        {
            reader.isGraphPaused = true;
        }

        private void StartGraph(InputAction.CallbackContext context)
        {
            reader.isGraphPaused = false;
        }

        private void ClearGraph(InputAction.CallbackContext context)
        {
            reader.ClearGraph();
        }

        private void ShowListView(InputAction.CallbackContext context)
        {
            reader.SwitchView();
        }

        protected override void OnAppLoaded()
        {
            UIManager.AddToViewport(canvas);
        }
        protected override void OnAppQuit()
        {
            UIManager.RemoveFromViewport(canvas);
        }
    }
}

