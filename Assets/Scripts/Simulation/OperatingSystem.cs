using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Rover.OS
{
    public enum OSState { CommandLine, Application, RoverControl };
    public static class OperatingSystem
    {
        public static event Action<OSState> EOnOperationSystemStateChange;
        private static OSState m_osState = OSState.RoverControl;
        public static OSState OSState { get { return m_osState; } }
        private static bool m_allowUserControl = true;
        public static bool AllowUserControl {get{return m_allowUserControl;}}
        public static void SetOSState(OSState newState)
        {
            if (m_osState != newState)
            {
                m_osState = newState;
                EOnOperationSystemStateChange?.Invoke(m_osState);
            }
        }
        public static void SetUserControl(bool newControl)
        {
            m_allowUserControl = newControl;
        }
    }
}

