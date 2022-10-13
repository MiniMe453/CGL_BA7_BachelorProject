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
            for(int i = 0; i < ledPinStates.Length; i++)
            {
                ledPinStates[i] = 0;
            }
        }
        
        public static void SetLEDMode(int pin, int value)
        {  
            ledPinStates[(pin - 22)/2] = value;

            UduinoManager.Instance.sendCommand("led", ledPinStates);
        }
    }
}

