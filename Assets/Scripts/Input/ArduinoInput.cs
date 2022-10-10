using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;
using System;
using UnityTimer;
using Rover.Settings;

namespace Rover.Arduino
{
    public enum InputType {Digital, Analog};
    public class ArduinoInput
    {
        public event Action<float> EOnValueChanged;
        public event Action EOnButtonPressed;
        public event Action EOnButtonReleased;
        private InputType m_inputType;
        public InputType InputType {get {return m_inputType;} }
        private Timer m_inputUpdateTimer;
        private float m_oldValue = -999;
        private int m_pin;
        private string m_inputName;
        public string InputName {get {return m_inputName;} }
        private UduinoDevice m_device;
        private bool m_inputEnabled;

        public ArduinoInput(InputType type, int pin, string name = "Arduino Input")
        {
            m_pin = pin;
            m_inputType = type;
            m_inputName = name;

            UduinoManager.Instance.OnBoardConnected += OnBoardConnected;

            ArduinoInputDatabase.RegisterInput(this);
        }

        private void OnBoardConnected(UduinoDevice device)
        {
            m_device = device;

            switch(m_inputType)
            {
                case InputType.Digital:
                    UduinoManager.Instance.pinMode(m_pin, PinMode.Input_pullup);
                    break;
                case InputType.Analog:
                    UduinoManager.Instance.pinMode(m_pin, PinMode.Input);
                    break;
            }

            m_inputUpdateTimer = Timer.Register(GameSettings.INPUT_TIMER_DELAY, () => ArduinoInputUpdate(), isLooped: true);
            
            if(!m_inputEnabled)
                DisableInput();
        }

        private void ArduinoInputUpdate()
        {   
            switch(m_inputType)
            {
                case InputType.Digital:
                    ReadDigitalInput();
                    break;
                case InputType.Analog:
                    ReadAnalogInput();
                    break;
            }
        }

        public void EnableInput()
        {
            m_inputEnabled = true;

            if(m_inputUpdateTimer != null)
                m_inputUpdateTimer.Resume();
        }

        public void DisableInput()
        {
            m_inputEnabled = false;

            if(m_inputUpdateTimer != null)
                m_inputUpdateTimer.Pause();
        }

        private void ReadDigitalInput()
        {
            float currentValue = (float)UduinoManager.Instance.digitalRead(m_pin);
            
            Debug.Log(currentValue);

            if(currentValue != m_oldValue)
            {
                if(currentValue == 1f)
                    EOnButtonPressed?.Invoke();
                else
                    EOnButtonReleased?.Invoke();

                m_oldValue = currentValue;
            }
        }

        private void ReadAnalogInput()
        {
            float currentValue = UduinoManager.Instance.analogRead(m_pin);

            if(currentValue != m_oldValue)
            {
                EOnValueChanged?.Invoke(currentValue);
                m_oldValue = currentValue;
            }                
        }
    }

    public static class ArduinoInputDatabase
    {
        public static List<ArduinoInput> arduinoInputs = new List<ArduinoInput>();


        public static void RegisterInput(ArduinoInput input)
        {
            if(arduinoInputs.IndexOf(input) == -1)
            {
                arduinoInputs.Add(input);
            }
        }
    }
}
