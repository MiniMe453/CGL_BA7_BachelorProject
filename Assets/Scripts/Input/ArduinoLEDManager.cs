using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Uduino;

namespace Rover.Arduino
{
    public static class LEDManager
    {
        private static object[] ledPinStates = new object[16];
        private static bool applicationQuitting = false;

        static LEDManager()
        {
            for (int i = 0; i < ledPinStates.Length; i++)
            {
                ledPinStates[i] = 0;
            }

            Application.quitting += OnApplicationQuit;
        }

        public static void SetLEDMode(int pin, int value)
        {
            if (applicationQuitting)
                return;

            ledPinStates[(pin - 22) / 2] = value;

            UduinoManager.Instance.sendCommand("led", ledPinStates);
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
                ledPinStates[(pin[i] - 22) / 2] = value[i];
            }

            UduinoManager.Instance.sendCommand("led", ledPinStates);
        }

        private static void OnApplicationQuit()
        {
            applicationQuitting = true;
            for (int i = 0; i < ledPinStates.Length; i++)
            {
                ledPinStates[i] = 0;
            }

            UduinoManager.Instance.sendCommand("led", ledPinStates);
        }
    }
}

