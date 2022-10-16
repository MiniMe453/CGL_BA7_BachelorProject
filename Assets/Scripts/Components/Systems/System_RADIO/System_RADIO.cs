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

        void Start()
        {
            m_RadioKnob = new ArduinoInput(InputType.Analog, 255, 4, "Radio Knob");
            m_RadioKnob.EOnValueChanged += OnKnobValueChanged;

            m_RadioButton = new ArduinoInput(InputType.Digital, 11, 5, "Radio Button");
            m_RadioButton.EOnButtonPressed += OnButtonPressed;
        }

        void OnKnobValueChanged(float value, int pin)
        {
            float percentage = value/1024;

            float newFreq = receiverData.frequencyMin + ((receiverData.frequencyMax - receiverData.frequencyMin) * percentage);

            m_currentFreq = newFreq;
            
        }

        void OnButtonPressed(int pin)
        {
            Debug.LogError("Radio Button Pressed");
        }

        void Update()
        {
            if(Mathf.Abs(m_currentFreq - receiverData.Frequency) > 0.1)
            {
                receiverData.Frequency += signalSmoothingSpeed * Time.deltaTime * Mathf.Sign(m_currentFreq - receiverData.Frequency);
            }

            Debug.LogError(receiverData.Frequency);
        }
    }
}