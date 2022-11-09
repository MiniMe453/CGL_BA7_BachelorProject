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
        public event Action<float, int> EOnValueChanged;
        public event Action<int> EOnButtonPressed;
        public event Action<int> EOnButtonReleased;
        public event Action<int> EOnButtonHeld;
        private InputType m_inputType;
        public InputType InputType {get {return m_inputType;} }
        private Timer m_inputUpdateTimer;
        private float m_oldValue = -999;
        private int m_pin;
        private int m_id;
        private string m_inputName;
        public string InputName {get {return m_inputName;} }
        private UduinoDevice m_device;
        private bool m_inputEnabled = true;
        private bool m_canHoldButton = false;
        private float m_holdDuration;
        private float m_holdTimer;
        private float m_timeSinceStartHold;
        private bool m_holdStarted = false;

        public ArduinoInput(InputType type, int pin, int id, bool buttonHold = false, float holdTimer = 1f, string name = "Arduino Input")
        {
            m_pin = pin;
            m_id = id;
            m_inputType = type;
            m_inputName = name;
            m_canHoldButton = buttonHold;
            m_holdDuration = holdTimer;

            ArduinoInputDatabase.RegisterInput(this);
        }

        public ArduinoInput(InputType type, int pin, int id, string name = "Arduino Input")
        {
            m_pin = pin;
            m_id = id;
            m_inputType = type;
            m_inputName = name;
            m_canHoldButton = false;
            m_holdDuration = 0;

            ArduinoInputDatabase.RegisterInput(this);
        }


        public void SetupInputPins(UduinoDevice device)
        {
            // m_device = device;

            // switch(m_inputType)
            // {
            //     case InputType.Digital:
            //         UduinoManager.Instance.pinMode(m_pin, PinMode.Input);
            //         break;
            //     case InputType.Analog:
            //         UduinoManager.Instance.pinMode(m_pin, PinMode.Input);
            //         break;
            // }
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
        }

        public void DisableInput()
        {
            m_inputEnabled = false;
        }

        private void ReadDigitalInput()
        {
            if(ArduinoInputDecoder.LastMessage.Count <= 0)
                return;

            if(m_id > ArduinoInputDecoder.LastMessage[0].Length - 1)
            {
                Debug.LogError("Input with ID " + m_id + " exceeds last message length " + ArduinoInputDecoder.LastMessage);
                return;
            }

            float currentValue = float.Parse(ArduinoInputDecoder.LastMessage[0][m_id].ToString());

            // if(currentValue == 1f && !m_buttonHoldStarted)
            // {
            //     m_buttonHoldStarted = true;
            // }
            // else if (m_buttonHoldStarted)
            // {
            //     m_buttonHoldStarted = false;
            // }

            // if(m_buttonHoldStarted)
            // {
            //     m_holdTimer += Time.deltaTime;

            //     if(m_holdTimer > m_holdDuration)
            //     {
            //         EOnButtonHeld?.Invoke(m_pin);
            //         return;
            //     }
            // }

            if(currentValue != m_oldValue)
            {
                if(currentValue == 1f)
                {
                    EOnButtonPressed?.Invoke(m_pin);
                    m_timeSinceStartHold = Time.time;    
                    m_holdStarted = true;
                }
                else
                {
                    EOnButtonReleased?.Invoke(m_pin);
                    m_holdStarted = false;
                }

                m_oldValue = currentValue;
            }

            if(m_canHoldButton && currentValue == 1f)
            {
                if(Time.time - m_timeSinceStartHold > m_holdDuration && m_holdStarted)
                {
                    m_holdTimer = 0;
                    EOnButtonHeld?.Invoke(m_pin);
                    m_holdStarted = false;
                    return;
                }
            }
        }

        private void ReadAnalogInput()
        {
            if(ArduinoInputDecoder.LastMessage.Count <= 0)
                return;

            float currentValue = float.Parse(ArduinoInputDecoder.LastMessage[m_id].ToString());

            if(currentValue != m_oldValue)
            {
                EOnValueChanged?.Invoke(currentValue, m_pin);
                m_oldValue = currentValue;
            }                
        }
    }

    public static class ArduinoInputDatabase
    {
        private static List<ArduinoInput> m_arduinoInputs = new List<ArduinoInput>();
        public static List<ArduinoInput> ArduinoInputs { get {return m_arduinoInputs;} }
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
            if(!Rover.OS.OperatingSystem.AllowUserControl)
                return;

            foreach(ArduinoInput input in m_arduinoInputs)
            {
                input.CheckInputValue();
            }
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
        private static List<string> m_lastMessage = new List<string>() {"","","","",""};
        public static List<string> LastMessage { get {return m_lastMessage;} }
        static ArduinoInputDecoder()
        {
            UduinoManager.Instance.OnDataReceived += OnMessageReceived;
        }

        private static void OnMessageReceived(string data, UduinoDevice device)
        {
            if(data[0] != '_')
                return;
                
            data = data.Remove(0,1);

            string[] split = data.Split(' ');

            for(int i = 0; i<split.Length;i++)
            {
                m_lastMessage[i] = split[i];
            }

            EOnSerialMessageRecieved?.Invoke(data);
        }
    }
}
