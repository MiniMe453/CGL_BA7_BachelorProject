using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.OS;
using UnityEngine.InputSystem;

public class roverchangemodetest : MonoBehaviour
{
    public InputActionMap actionMap;
    void Start()
    {
        OperatingSystem.SetOSState(OSState.CommandLine);

        actionMap["rvrMode"].performed += Action_Rover;
        actionMap["cmdMode"].performed += Action_CommandLine;

        actionMap.Enable();
    }

    void Action_Rover(InputAction.CallbackContext context)
    {
        OperatingSystem.SetOSState(OSState.RoverControl);
    }

    void Action_CommandLine(InputAction.CallbackContext context)
    {
        OperatingSystem.SetOSState(OSState.CommandLine);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
