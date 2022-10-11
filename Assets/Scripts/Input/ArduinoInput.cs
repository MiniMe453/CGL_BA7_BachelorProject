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
        public event Action<int> EOnButtonPressed;
        public event Action<int> EOnButtonReleased;
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

            ArduinoInputDatabase.RegisterInput(this);
        }

        public void SetupInputPins(UduinoDevice device)
        {
            m_device = device;

            switch(m_inputType)
            {
                case InputType.Digital:
                    UduinoManager.Instance.pinMode(m_pin, PinMode.Input);
                    break;
                case InputType.Analog:
                    UduinoManager.Instance.pinMode(m_pin, PinMode.Input);
                    break;
            }

            //m_inputUpdateTimer = Timer.Register(GameSettings.INPUT_TIMER_DELAY, () => ArduinoInputUpdate(), isLooped: true);
            
            // if(!m_inputEnabled)
            //     DisableInput();
        }

        public void CheckInputValue()
        {   
            if(!m_inputEnabled)
                return;

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

            // if(m_inputUpdateTimer != null)
            //     m_inputUpdateTimer.Resume();
        }

        public void DisableInput()
        {
            m_inputEnabled = false;

            // if(m_inputUpdateTimer != null)
            //     m_inputUpdateTimer.Pause();
        }

        private void ReadDigitalInput()
        {
            float currentValue = (float)UduinoManager.Instance.digitalRead(m_pin, GameSettings.INPUT_BUNDLE_NAME);

            if(currentValue != m_oldValue)
            {
                if(currentValue == 1f)
                    EOnButtonPressed?.Invoke(m_pin);
                else
                    EOnButtonReleased?.Invoke(m_pin);

                m_oldValue = currentValue;
            }
        }

        private void ReadAnalogInput()
        {
            float currentValue = UduinoManager.Instance.analogRead(m_pin, GameSettings.INPUT_BUNDLE_NAME);

            if(currentValue != m_oldValue)
            {
                EOnValueChanged?.Invoke(currentValue);
                m_oldValue = currentValue;
            }                
        }
    }

    public static class ArduinoInputDatabase
    {
        private static List<ArduinoInput> m_arduinoInputs = new List<ArduinoInput>();
        public static List<ArduinoInput> ArduinoInputs { get {return m_arduinoInputs;} }
        public static event Action EOnInputReadUpdate;
        private static Timer inputUpdateTimer;

        static ArduinoInputDatabase()
        {
            UduinoManager.Instance.OnBoardConnected += OnBoardConnected;
        }

        private static void OnBoardConnected(UduinoDevice device)
        {
            foreach(ArduinoInput input in m_arduinoInputs)
            {
                input.SetupInputPins(device);
            }

            inputUpdateTimer = Timer.Register(GameSettings.INPUT_TIMER_DELAY, () => {OnInputReadTimerUpdate();}, isLooped: true);
        }

        private static void OnInputReadTimerUpdate()
        {
            foreach(ArduinoInput input in m_arduinoInputs)
            {
                input.CheckInputValue();
            }

            UduinoManager.Instance.SendBundle(GameSettings.INPUT_BUNDLE_NAME);
        }

        public static void RegisterInput(ArduinoInput input)
        {
            if(m_arduinoInputs.IndexOf(input) == -1)
            {
                m_arduinoInputs.Add(input);
            }
        }
    }

    public static class ArduinoInputDecoder
    {
        public static event Action<string> EOnSerialMessageRecieved;
        static ArduinoInputDecoder()
        {
            UduinoManager.Instance.OnDataReceived += OnMessageReceived;
        }

        private static void OnMessageReceived(string data, UduinoDevice device)
        {
            EOnSerialMessageRecieved?.Invoke(data);
        }
    }
}
