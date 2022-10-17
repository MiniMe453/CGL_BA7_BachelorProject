using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Rover.Systems
{
    public class RoverMainSystem
    {
        private bool m_power;
        public bool Power { get { return m_power; } }
        //public event Action<bool> EOnPowerChanged;
        private List<RoverSubsystem> m_subSystems = new List<RoverSubsystem>();
        public List<RoverSubsystem> Subsystems { get { return m_subSystems; } }

        public void RegisterSubsystem(RoverSubsystem subsystem)
        {
            m_subSystems.Add(subsystem);
        }

    }

    public class RoverSubsystem
    {
        private ushort m_id;
        public ushort ID { get { return m_id; } }
        private string m_name;
        public string Name { get { return m_name; } }
    }
}

