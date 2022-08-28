using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Linq;

namespace Rover.Interface
{
    public class DeveloperCommandBase
    {
        private string _commandID;
        private string _commandDescription;
        private string _commandFormat;
        private string _returnMessage;

        public string commandID { get { return _commandID; } }
        public string commandDescription { get { return _commandDescription; } }
        public string commandFormat { get { return _commandFormat; } }
        public string returnMessage { get { return _returnMessage; } }

        public DeveloperCommandBase(string id, string description, string format, string returnMessage = "")
        {
            _commandID = id;
            _commandDescription = description;
            _commandFormat = format;
            _returnMessage = returnMessage;
            DeveloperCommandDatabase.RegisterDeveloperCommand(this);
        }
    }

    public class DeveloperCommand : DeveloperCommandBase
    {
        private Action command;

        public DeveloperCommand(string id, string description, string format, Action command, string returnMessage = "") : base(id, description, format, returnMessage)
        {
            this.command = command;
        }

        public void Invoke()
        {
            command?.Invoke();
        }
    }

    public class DeveloperCommand<T1> : DeveloperCommandBase
    {
        private Action<T1> command;

        public DeveloperCommand(string id, string description, string format, Action<T1> command, string returnMessage = "") : base(id, description, format, returnMessage)
        {
            this.command = command;
        }

        public void Invoke(T1 value)
        {
            command?.Invoke(value);
        }
    }
}
