using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.IO;


namespace Rover.Interface
{
    public enum DeveloperErrorMessages { INVALID_COMMAND, INVALID_ARGUMENT, PLAYER_NOT_IN_SCENE, NO_VAILD_MACHINE, INVALID_MACHINE_ID, INVALID_ERROR_ID, ERROR_NOT_FOUND };
    public class DeveloperCommandDatabase
    {
        public static List<DeveloperCommandBase> commandList = new List<DeveloperCommandBase>();

        public static void RegisterDeveloperCommand(DeveloperCommandBase command)
        {
            foreach (DeveloperCommandBase commandBase in commandList)
            {
                if (commandBase.commandID == command.commandID) return;
            }

            commandList.Add(command);
        }
    }

}
