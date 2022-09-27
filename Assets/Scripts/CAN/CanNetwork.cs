using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTimer;
using System;
using Rover.Settings;
using Rover.DateTime;

namespace Rover.Can
{
    public struct CanFrame
    {
        public ushort nodeID;
        public DateTimeStruct timestamp;
        public object[] data;
    }

    public class CanChannel
    {
        public event Action<CanFrame> CAN_MESSAGE_RECIEVED;
        private List<CanNode> m_nodes = new List<CanNode>();
        public List<CanNode> Nodes { get { return m_nodes; } }

        public void SendCANFrame(CanFrame frame)
        {
            CAN_MESSAGE_RECIEVED?.Invoke(frame);
        }

        public void AddCANNode(CanNode node)
        {
            m_nodes.Add(node);
        }
    }

    public class CanNode
    {
        private ushort m_id;
        public ushort ID { get { return m_id; } }
        private string m_m_nodeName;
        public string m_NodeName { get { return m_m_nodeName; } }
        private CanChannel channel;
        private List<ushort> m_messageMask = new List<ushort>();
        private Timer CANTimer;
        private object[] m_canData;
        public object[] CANData { get { return m_canData; } set { m_canData = value; } }
        private float m_timeSinceLastMesssage;
        public float TimeSinceLastMessage { get { return m_timeSinceLastMesssage; } }
        private MonoBehaviourCan parent;

        public CanNode(ushort m_canID, string m_nodeName, CanChannel canChannel, MonoBehaviourCan parentClass, bool m_receiveCanFrame = true)
        {
            m_id = m_canID;
            m_m_nodeName = m_nodeName;
            parent = parentClass;
            channel = canChannel;
            channel.AddCANNode(this);

            if (m_receiveCanFrame)
            {
                parentClass.EOnFixedUpdate += OnFixedUpdate;
                channel.CAN_MESSAGE_RECIEVED += ReadCANFrame;
            }

        }

        void OnFixedUpdate()
        {
            m_timeSinceLastMesssage += Time.deltaTime;
        }
        ///<summary>
        ///Starts a timer. A CAN Frame will be sent every X seconds if this is used.
        ///</summary>
        protected void StartCANTimer(float delay)
        {
            CANTimer = Timer.Register(delay, () => SendCANFrame(), isLooped: true);
        }

        protected void DestroyCANNode()
        {
            if (CANTimer != null)
                CANTimer.Cancel();

            channel.CAN_MESSAGE_RECIEVED -= ReadCANFrame;
        }

        public void SendCANFrame()
        {
            CanFrame frame = new CanFrame();
            frame.nodeID = m_id;
            frame.timestamp = TimeManager.GetCurrentDateTime();
            frame.data = m_canData;

            channel.SendCANFrame(frame);
        }

        private void ReadCANFrame(CanFrame frame)
        {
            if (!m_messageMask.Contains(frame.nodeID))
            {
                Debug.LogWarning($"{parent.name} recieved a message, but the message mask has not been set up properly!");
                return;
            }

            parent.OnCANFrameRecieved(frame);
            m_timeSinceLastMesssage = 0f;
        }

        ///<summary>
        ///If used, the node will only read messages that are included inside the filter.
        ///</summary>
        public void SetRXFilter(List<ushort> filterIDs)
        {
            m_messageMask = filterIDs;
        }
    }

    public static class CanNetwork
    {
        public static CanChannel Can0 = new CanChannel();
    }

    public class MonoBehaviourCan : MonoBehaviour
    {
        [Header("CAN Network Variables")]
        [SerializeField]
        private ushort m_canID;
        [SerializeField]
        private string m_nodeName;
        [SerializeField]
        private bool m_receiveCanFrame = true;
        [SerializeField]
        private List<ushort> m_messageMask = new List<ushort>();
        protected CanNode node;
        public event Action EOnFixedUpdate;

        void Awake()
        {
            node = new CanNode(m_canID, m_nodeName, CanNetwork.Can0, this, m_receiveCanFrame);

            if (m_messageMask.Count > 0)
                node.SetRXFilter(m_messageMask);

            Init();
        }

        protected virtual void Init()
        {

        }
        protected virtual void FixedUpdate()
        {
            EOnFixedUpdate?.Invoke();
        }

        public virtual void OnCANFrameRecieved(CanFrame frame)
        {

        }
    }
}

