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
        private static OSState m_osState;
        public static OSState OSState { get { return m_osState; } }
        public static void SetOSState(OSState newState)
        {
            if (m_osState != newState)
            {
                m_osState = newState;
                EOnOperationSystemStateChange?.Invoke(m_osState);
            }
        }
    }
}

