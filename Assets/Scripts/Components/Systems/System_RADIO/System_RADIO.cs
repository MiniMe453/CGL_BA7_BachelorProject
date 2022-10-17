using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Arduino;

namespace Rover.Systems
{
    public class System_RADIO : MonoBehaviour
    {
        private ArduinoInput m_RadioKnob;
        private ArduinoInput m_RadioButton;
        public RadioReceiverData receiverData; 

        private float m_currentFreq;
        public float signalSmoothingSpeed = 50;
        private RadioManager.ERadioTypes m_selectedRadioBand = RadioManager.ERadioTypes.FM;
        private int[] m_ledPins = {42, 36, 34};

        void Start()
        {
            m_RadioKnob = new ArduinoInput(InputType.Analog, 255, 4, "Radio Knob");
            m_RadioKnob.EOnValueChanged += OnKnobValueChanged;

            m_RadioButton = new ArduinoInput(InputType.Digital, 11, 5, "Radio Button");
            m_RadioButton.EOnButtonPressed += OnButtonPressed;
        }

        void OnKnobValueChanged(float value, int pin)
        {
            float percentage = value/512;

            float newFreq = receiverData.frequencyMin + ((receiverData.frequencyMax - receiverData.frequencyMin) * percentage);

            receiverData.Frequency = newFreq;
            Debug.Log(receiverData.Frequency);
        }

        void OnButtonPressed(int pin)
        {
            if((int)m_selectedRadioBand == 2)
                m_selectedRadioBand = 0;
            else
                m_selectedRadioBand++;

            switch(m_selectedRadioBand)
            {
                case RadioManager.ERadioTypes.FM:
                    LEDManager.SetLEDMode(m_ledPins, new int[] {1,0,0});
                    break;
                case RadioManager.ERadioTypes.AM:
                    LEDManager.SetLEDMode(m_ledPins, new int[] {0,1,0});
                    break;
                case RadioManager.ERadioTypes.Test:
                    LEDManager.SetLEDMode(m_ledPins, new int[] {0,0,1});
                    break;
            }
        }
    }
}