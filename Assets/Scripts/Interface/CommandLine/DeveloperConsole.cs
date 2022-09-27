using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.IO;
using Rover.OS;

namespace Rover.Interface
{
    public class DeveloperConsole : MonoBehaviour
    {
        public static DeveloperConsole instance;
        bool showConsole;
        public bool IsConsoleActive { get { return showConsole; } }
        public float consoleHeight = 40;
        string input = "_";
        private int currCommandIndex;
        public InputActionMap developerActions;
        public Texture2D opaqueBG;
        public Font consoleFont;
        public GUIStyle testStyle;

        public static DeveloperCommand<string> PRINT_HELLO_WORLD = new DeveloperCommand<string>(
            "PRT",
            "Prints a string to console",
            "PRT <string>",
            (x) => DevCommandPrintHelloWorld(x)
            );

        public static DeveloperCommand CREATE_COMMAND_FILE = new DeveloperCommand(
            "CMDFILE",
            "Ouputs file of all commands",
            "CMDFILE",
            () => Command_CreateCommandFile(),
            "Command CSV file created!"
            );


        public static DeveloperCommand HELP = new DeveloperCommand(
            "HELP",
            "Shows all commands",
            "HELP",
            () => Command_Help()
            );

        private List<string> commandHistory = new List<string>();
        private List<Color> commandHistoryColor = new List<Color>();

        void Awake()
        {
            developerActions["ToggleConsole"].performed += OnToggleConsole;
            developerActions["AcceptCommand"].performed += OnEntryAccepted;
            developerActions["GoUp"].performed += GoUpThroughHistory;
            developerActions["GoDown"].performed += GoDownThroughHistory;

            Rover.OS.OperatingSystem.EOnOperationSystemStateChange += OnOperatingSystemStateChange;

            if (instance == null)
            {
                instance = this;
            }
        }

        void OnEnable()
        {
            developerActions.Enable();
        }

        void OnDisable()
        {
            developerActions.Disable();
        }

        private void OnToggleConsole(InputAction.CallbackContext context)
        {
            showConsole = !showConsole;

            input = "";
        }

        private void OnOperatingSystemStateChange(OSState newState)
        {
            switch (newState)
            {
                case OSState.CommandLine:
                    showConsole = true;
                    break;
                case OSState.Application:
                    showConsole = false;
                    break;
                case OSState.RoverControl:
                    break;
                default:
                    break;
            }
        }

        private void OnEntryAccepted(InputAction.CallbackContext context)
        {
            if (showConsole)
            {
                input.ToLower();
                UpdateCommandHistory("> " + input, Color.grey);
                HandleInput();
            }
        }

        private void GoUpThroughHistory(InputAction.CallbackContext context)
        {
            if (currCommandIndex - 1 < 0) return;

            currCommandIndex--;

            while (!commandHistory[currCommandIndex].Contains("> "))
            {
                if (currCommandIndex - 1 < 0) return;

                currCommandIndex--;
            }

            string newInputString = commandHistory[currCommandIndex].Remove(0, 2);

            input = newInputString;
        }

        private void GoDownThroughHistory(InputAction.CallbackContext context)
        {
            if (currCommandIndex + 1 > commandHistory.Count - 1)
            {
                currCommandIndex = commandHistory.Count;
                input = "";
                return;
            }

            currCommandIndex++;

            //Skip messages the player didn't send themselves
            while (!commandHistory[currCommandIndex].Contains("> "))
            {
                if (currCommandIndex + 1 > commandHistory.Count - 1)
                {
                    currCommandIndex = commandHistory.Count;
                    input = "";
                    return;
                }

                currCommandIndex++;
            }

            string newInputString = commandHistory[currCommandIndex].Remove(0, 2);

            input = newInputString;
        }

        private void OnGUI()
        {
            if (!showConsole) return;


            // GUIStyle textStyle = new GUIStyle(GUI.skin.textField);
            // textStyle.fontSize = 20;
            // textStyle.font = consoleFont;
            // textStyle.normal.background = opaqueBG;
            GUI.skin.settings.cursorColor = Color.clear;
            GUI.backgroundColor = Color.black;

            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", testStyle);

            GUI.backgroundColor = new Color(0, 0, 0, 0);

            GUI.SetNextControlName("DebugConsole");
            input = GUI.TextField(new Rect(0, Screen.height + 10, Screen.width - 20f, (consoleHeight / 4) * 3), input, 255, testStyle);
            GUI.FocusControl("DebugConsole");

            if (input.Length > 55)
                input = input.Remove(input.Length - 1, 1);

            string inputModified = ">" + input;

            if (input.Length < 55)
                inputModified += "_";

            testStyle.normal.textColor = Color.white;
            GUI.Box(new Rect(10f, Screen.height - (consoleHeight), Screen.width - 20f, (consoleHeight / 4) * 3), inputModified, testStyle);

            UpdateCommandHistoryDisplay(testStyle);

            //if (input != "") ShowAvailableCommands(textStyle); //Uncomment this line if you want to show all available commands while the user is typing
        }

        private void HandleInput()
        {
            bool showUnrecognizeCommandMessage = true;
            string[] properties = input.Split(' ');
            List<object> parameters = new List<object>();

            bool errorFound = false;

            foreach (DeveloperCommandBase commandBase in DeveloperCommandDatabase.commandList)
            {
                if (commandBase.commandID.ToLower() == properties[0].ToLower())
                {
                    errorFound = ParseCommandForErrors(properties, commandBase as DeveloperCommand != null);
                    if (errorFound) break;

                    if (commandBase as DeveloperCommand != null && !errorFound)
                    {
                        (commandBase as DeveloperCommand).Invoke();

                        if (commandBase.returnMessage != "") UpdateCommandHistory("Return: " + commandBase.returnMessage, Color.green);

                        showUnrecognizeCommandMessage = false;
                        break;
                    }

                    if (!errorFound)
                    {
                        //starting at 1 because we want to skip the initial argument as this is irrelevant to the execution
                        for (int i = 1; i < properties.Length; i++)
                        {
                            parameters.Add(properties[i]);
                        }

                        ExecuteCommand(commandBase, parameters);
                        showUnrecognizeCommandMessage = false;
                        break;
                    }
                }
            }

            if (showUnrecognizeCommandMessage && !errorFound)
            {
                ThrowCommandError(DeveloperErrorMessages.INVALID_COMMAND);
            }

            input = "";
        }

        private bool ParseCommandForErrors(string[] cmdProperties, bool usesOneInput)
        {
            if ((cmdProperties.Length <= 1 || cmdProperties[1] == "") && !usesOneInput)
            {
                ThrowCommandError(DeveloperErrorMessages.INVALID_ARGUMENT);
                return true;
            }

            return false;
        }

        private void ExecuteCommand(DeveloperCommandBase commandBase, List<object> parameter)
        {
            if (commandBase as DeveloperCommand<string> != null)
            {
                (commandBase as DeveloperCommand<string>).Invoke(parameter[0].ToString());
            }

            if (commandBase as DeveloperCommand<float> != null)
            {
                if (float.TryParse(parameter[0].ToString(), out float result))
                {
                    (commandBase as DeveloperCommand<float>).Invoke(result);

                }
                else
                {
                    ThrowCommandError(DeveloperErrorMessages.INVALID_ARGUMENT);
                    return;
                }
            }

            if (commandBase as DeveloperCommand<int> != null)
            {
                if (int.TryParse(parameter[0].ToString(), out int result))
                {
                    (commandBase as DeveloperCommand<int>).Invoke(result);
                }
                else
                {
                    ThrowCommandError(DeveloperErrorMessages.INVALID_ARGUMENT);
                    return;
                }
            }

            if (commandBase as DeveloperCommand<long> != null)
            {
                if (long.TryParse(parameter[0].ToString(), out long result))
                {
                    (commandBase as DeveloperCommand<long>).Invoke(result);
                }
                else
                {
                    ThrowCommandError(DeveloperErrorMessages.INVALID_ARGUMENT);
                    return;
                }
            }

            if (commandBase as DeveloperCommand<object[]> != null)
            {
                (commandBase as DeveloperCommand<object[]>).Invoke(parameter.ToArray());
            }

            if (commandBase.returnMessage != "") UpdateCommandHistory("Return: " + commandBase.returnMessage, Color.green);
        }

        public void UpdateCommandHistory(string inputValue, Color textColor)
        {
            commandHistory.Add(inputValue);
            commandHistoryColor.Add(textColor);
            currCommandIndex = commandHistory.Count;

            CheckCommandHistoryLength();
        }

        public void ThrowCommandError(DeveloperErrorMessages error)
        {
            commandHistory.Add("Error: " + error.ToString());
            commandHistoryColor.Add(Color.red);
            Debug.LogError("ERROR: DEVELOPER_CONSOLE - " + error.ToString());

            CheckCommandHistoryLength();
        }

        private void CheckCommandHistoryLength()
        {
            if (commandHistory.Count <= 20) return;

            int numToRemove = commandHistory.Count - 20;

            for (int i = 0; i <= numToRemove; i++)
            {
                commandHistory.RemoveAt(0);
                commandHistoryColor.RemoveAt(0);
            }
        }

        private void UpdateCommandHistoryDisplay(GUIStyle textStyle)
        {
            for (int i = commandHistory.Count - 1; i >= 0; i--)
            {
                string label = commandHistory[i];
                float y = (Screen.height - (consoleHeight)) - (consoleHeight * ((commandHistory.Count - 1) - i));
                textStyle.normal.textColor = commandHistoryColor[i];

                Rect labelRect = new Rect(10f, y - consoleHeight, Screen.width - 20f, (consoleHeight / 4) * 3);

                GUI.Label(labelRect, label, textStyle);
            }
        }

        private void ShowAvailableCommands(GUIStyle textStyle)
        {
            GUI.backgroundColor = Color.black;
            textStyle.normal.textColor = Color.white;

            List<string> availableCommandsList = new List<string>();

            foreach (DeveloperCommandBase command in DeveloperCommandDatabase.commandList)
            {
                if (command.commandFormat.ToLower().Contains(input.ToLower()))
                {
                    availableCommandsList.Add(command.commandFormat);
                }
            }

            if (availableCommandsList.Count == 0) return;

            for (int i = 0; i < availableCommandsList.Count; i++)
            {
                string label = availableCommandsList[i];
                float y = (Screen.height - (consoleHeight)) - (consoleHeight * i);
                Rect labelRect = new Rect(10f, y - consoleHeight, Screen.width, consoleHeight);
                GUI.Label(labelRect, label, textStyle);
            }
        }

        public void WriteFullLine(char character, Color textColor)
        {
            string fullLine = "";

            for (int i = 0; i < 56; i++)
            {
                fullLine += character;
            }

            UpdateCommandHistory(fullLine, textColor);
        }

        private static void DevCommandPrintHelloWorld(string value)
        {
            DeveloperConsole.instance.WriteFullLine('%', Color.red);
            DeveloperConsole.instance.UpdateCommandHistory(value, Color.white);
            DeveloperConsole.instance.WriteFullLine('=', Color.blue);
            Debug.LogError(value);
        }

        private static void Command_CreateCommandFile()
        {
            List<string> fileLines = new List<string>() { "Name,Description,Format,Return Message" };

            foreach (DeveloperCommandBase commandBase in DeveloperCommandDatabase.commandList)
            {
                fileLines.Add(commandBase.commandID + "," + commandBase.commandDescription + "," + commandBase.commandFormat + "," + commandBase.returnMessage);
            }

            var fullPath = Path.Combine(Application.dataPath + "\\GameData\\OutputFiles", "devCommands.csv");

            File.WriteAllLines(fullPath, fileLines);
        }

        private static void Command_Help()
        {
            string line = "";

            for (int i = 0; i < DeveloperCommandDatabase.commandList.Count; i++)
            {
                line = DeveloperCommandDatabase.commandList[i].commandID + ": " + DeveloperCommandDatabase.commandList[i].commandDescription;
                DeveloperConsole.instance.UpdateCommandHistory(line, Color.white);
                line = "";
            }
        }
    }
}
