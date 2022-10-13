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

        static LEDManager()
        {
            for (int i = 0; i < ledPinStates.Length; i++)
            {
                ledPinStates[i] = 0;
            }
        }

        public static void SetLEDMode(int pin, int value)
        {
            ledPinStates[(pin - 22) / 2] = value;

            UduinoManager.Instance.sendCommand("led", ledPinStates);
        }

        public static void SetLEDMode(int[] pin, int[] value)
        {
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
    }
}

