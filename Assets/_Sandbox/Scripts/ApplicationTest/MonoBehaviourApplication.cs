using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Interface;
using UnityEngine.InputSystem;

namespace Rover.OS
{
    public abstract class MonoBehaviourApplication : MonoBehaviour
    {
        private DeveloperCommand LOAD_APPLICATION;
        public string appCommand;
        public string appDescription;
        private int m_appID;
        public int AppID { get { return m_appID; } }
        public InputActionMap applicationInputs;
        private OSState m_prevOSState;

        void Awake()
        {
            m_appID = AppDatabase.RegisterApp(this);

            LOAD_APPLICATION = new DeveloperCommand(
                appCommand,
                appDescription,
                appCommand,
                () => Command_LoadAppWithID()
            );

            applicationInputs.AddAction("quitApp", binding: "<Keyboard>/q");

            applicationInputs["quitApp"].performed += Action_Quit;

            Init();
        }

        protected virtual void OnValidate()
        {

        }

        protected virtual void Init()
        {

        }

        public void LoadApp()
        {
            m_prevOSState = OperatingSystem.OSState;
            OperatingSystem.SetOSState(OSState.Application);
            applicationInputs.Enable();
            OnAppLoaded();
        }

        public void QuitApp()
        {
            OperatingSystem.SetOSState(m_prevOSState);
            applicationInputs.Disable();
            OnAppQuit();
        }

        protected virtual void OnAppLoaded()
        {

        }

        protected virtual void OnAppQuit()
        {

        }

        private void Command_LoadAppWithID()
        {
            AppDatabase.GetAppFromID(m_appID).LoadApp();
        }

        private void Action_Quit(InputAction.CallbackContext context)
        {
            QuitApp();
        }
    }

    public static class AppDatabase
    {
        private static List<MonoBehaviourApplication> m_appList = new List<MonoBehaviourApplication>();

        public static List<MonoBehaviourApplication> Applications { get { return m_appList; } }

        public static int RegisterApp(MonoBehaviourApplication app)
        {
            m_appList.Add(app);

            return m_appList.Count - 1;
        }

        public static MonoBehaviourApplication GetAppFromID(int id)
        {
            return m_appList[id];
        }
    }
}

