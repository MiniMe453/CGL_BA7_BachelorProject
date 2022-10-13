using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Arduino;
using Rover.Settings;
using UnityTimer;

namespace Rover.Systems
{
    public class Sensor_Proximity : MonoBehaviour
    {
        private int[] m_ledPins = { 22, 24, 26, 28 };
        private int[] m_ledPinStates = { 0, 0, 0, 0 };
        public int numOfCasts = 16;
        public float sphereRadius = 2;
        public float castDistance = 10;
        private Timer m_proximityTimer;
        private float m_angleIncrement;
        private int m_numOfCastsPerDirection;
        private List<GameObject> objectsInRange = new List<GameObject>();

        void Start()
        {
            m_proximityTimer = Timer.Register(GameSettings.PROXIMITY_CHECK_DELAY, () => CheckRoverProximity(), isLooped: true);
            m_angleIncrement = 360f / numOfCasts;
            m_numOfCastsPerDirection = numOfCasts / 4;
        }

        void CheckRoverProximity()
        {
            bool sensorActivated = false;

            for (int i = 0; i < numOfCasts; i++)
            {
                RaycastHit hit;

                if (Physics.SphereCast(transform.position, sphereRadius, Quaternion.AngleAxis((i * m_angleIncrement) - Mathf.CeilToInt(m_numOfCastsPerDirection / 2), Vector3.up) * transform.forward, out hit, castDistance))
                {
                    sensorActivated = true;

                    if (i >= 0 && i < (m_numOfCastsPerDirection - 1))
                        SetLEDPinStates(1, 0, 0, 0);
                    else if (i >= m_numOfCastsPerDirection && i <= m_numOfCastsPerDirection * 2 - 1)
                        SetLEDPinStates(0, 1, 0, 0);
                    else if (i >= m_numOfCastsPerDirection * 2 && i <= m_numOfCastsPerDirection * 3 - 1)
                        SetLEDPinStates(0, 0, 1, 0);
                    else if (i >= m_numOfCastsPerDirection * 3 && i <= m_numOfCastsPerDirection * 4 - 1)
                        SetLEDPinStates(0, 0, 0, 1);

                    Debug.LogError(m_ledPinStates);
                    break;
                }
            }

            if (sensorActivated)
                LEDManager.SetLEDMode(m_ledPins, m_ledPinStates);
        }

        private void SetLEDPinStates(int front, int right, int back, int left)
        {
            m_ledPinStates[0] = front;
            m_ledPinStates[1] = right;
            m_ledPinStates[2] = back;
            m_ledPinStates[3] = left;
        }

        void OnTriggerEnter(Collider other)
        {
            objectsInRange.Add(other.gameObject);
        }
    }
}
