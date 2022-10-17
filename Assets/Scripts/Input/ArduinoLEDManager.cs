using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Uduino;
using UnityTimer;
using System.IO.Ports;

namespace Rover.Arduino
{
    public static class LEDManager
    {
        private static object[] ledPinStates = new object[16];
        private static bool m_pinStatesUpdated = false;
        private static bool applicationQuitting = false;

        static LEDManager()
        {
            for (int i = 0; i < ledPinStates.Length; i++)
            {
                ledPinStates[i] = 0;
            }

            //Application.quitting += OnApplicationQuit;

            Timer.Register(0.1f, () => SendLEDCommand(), isLooped: true);
        }

        private static void SendLEDCommand()
        {
            if(UduinoManager.Instance.isConnected() && m_pinStatesUpdated)
            {
                m_pinStatesUpdated = false;
                UduinoManager.Instance.sendCommand("led", ledPinStates);
            }
        }

        public static void SetLEDMode(int pin, int value)
        {
            if (applicationQuitting)
                return;

           if((pin - 22) % 2 == 0)
                ledPinStates[(pin - 22) / 2] = value;
            else
                ledPinStates[pin - 22] = value; 

            m_pinStatesUpdated = true;
        }

        public static void SetLEDMode(int[] pin, int[] value)
        {
            if (applicationQuitting)
                return;

            if (pin.Length != value.Length)
            {
                Debug.LogError("LEDManager: Pin array and value array are not the same length!");
                return;
            }

            for (int i = 0; i < pin.Length; i++)
            {
                if((pin[i] - 22) % 2 == 0)
                    ledPinStates[(pin[i] - 22) / 2] = value[i];
                else
                    ledPinStates[pin[i] - 22] = value[i]; 
            }

            m_pinStatesUpdated = true;
        }

        // private static void OnApplicationQuit()
        // {
        //     applicationQuitting = true;
        //     for (int i = 0; i < ledPinStates.Length; i++)
        //     {
        //         ledPinStates[i] = 0;
        //     }

        //     UduinoManager.Instance.sendCommand("led", ledPinStates);
        // }
    }
}

