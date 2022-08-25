using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTimer;
using System;
using Rover.Settings;

namespace Rover.Can
{
    public struct CanFrame
    {
        public ushort nodeID;
        public object[] data;
    }

    public class CanChannel
    {
        public event Action<CanFrame> CAN_MESSAGE_RECIEVED;
        public List<CanNode> nodes = new List<CanNode>();

        public void SendCANFrame(CanFrame frame)
        {
            CAN_MESSAGE_RECIEVED?.Invoke(frame);
        }

        public void AddCANNode(CanNode node)
        {
            nodes.Add(node);
        }
    }

    public class CanNode : MonoBehaviour
    {
        private ushort m_id;
        public ushort ID { get { return m_id; } }
        private CanChannel channel;
        private List<ushort> messageMask = new List<ushort>();
        private Timer CANTimer;
        protected List<object> canData = new List<object>();
        private float m_timeSinceLastMesssage;
        public float TimeSinceLastMessage { get { return m_timeSinceLastMesssage; } }

        protected void InitializeCANNode(ushort canID, CanChannel canChannel)
        {
            m_id = canID;
            channel = canChannel;
            channel.CAN_MESSAGE_RECIEVED += ReadCANFrame;
            channel.AddCANNode(this);
        }

        void FixedUpdate()
        {
            m_timeSinceLastMesssage += Time.deltaTime;
        }
        ///<summary>
        ///Starts a timer. A CAN Frame will be sent every X seconds if this is used.
        ///</summary>
        protected void StartCANTimer(float delay)
        {
            CANTimer = Timer.Register(delay, () => PrepareCANFrame(), isLooped: true);
        }

        protected void DestroyCANNode()
        {
            if (CANTimer != null)
                CANTimer.Cancel();

            channel.CAN_MESSAGE_RECIEVED -= ReadCANFrame;
        }

        protected virtual void PrepareCANFrame()
        {
            SendCANFrame();
        }

        private void SendCANFrame()
        {
            CanFrame frame = new CanFrame();
            frame.nodeID = m_id;
            frame.data = canData.ToArray();
            canData.Clear();

            channel.SendCANFrame(frame);
        }

        private void ReadCANFrame(CanFrame frame)
        {
            if (!messageMask.Contains(frame.nodeID) && messageMask.Count != 0)
                return;

            OnCANFrameRead(frame.data);
            m_timeSinceLastMesssage = 0f;
        }

        ///<summary>
        ///If used, the node will only read messages that are included inside the filter.
        ///</summary>
        protected void SetRXFilter(List<ushort> filterIDs)
        {
            messageMask = filterIDs;
        }

        protected virtual void OnCANFrameRead(object[] data)
        {

        }
    }

    public static class CanNetwork
    {
        public static CanChannel Can0 = new CanChannel();
    }
}

