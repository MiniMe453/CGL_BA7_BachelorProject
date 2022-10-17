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
        private bool m_stateModified = false;
        private int m_prevQuadrant = 0;
        private Timer m_proximityTimer;
        private List<GameObject> objectsInRange = new List<GameObject>();

        void Start()
        {
            m_proximityTimer = Timer.Register(GameSettings.PROXIMITY_CHECK_DELAY, () => CheckRoverProximity(), isLooped: true);
        }

        void CheckRoverProximity()
        {
            float angle = 0f;
            bool sensorActivated = false;

            if (objectsInRange.Count > 0)
            {
                sensorActivated = true;

                foreach (GameObject obj in objectsInRange)
                {
                    Vector2 playerPos = new Vector2(transform.forward.x, transform.forward.z);
                    Vector2 objPos = new Vector2(obj.transform.position.x, obj.transform.position.z);

                    float angle_a = Mathf.Atan2(playerPos.y, playerPos.x);
                    float angle_b = Mathf.Atan2(objPos.y, objPos.x);

                    angle = angle_b - angle_a;

                    if (angle < Mathf.PI / 4 && angle > -Mathf.PI / 4 && m_ledPinStates[0] != 1)
                        SetLEDPinStates(0, 1);
                    if (angle > Mathf.PI / 4 && angle < (Mathf.PI / 4) * 3 && m_ledPinStates[1] != 1)
                        SetLEDPinStates(1, 1);
                    if (angle > (Mathf.PI / 4) * 3 || angle < (-Mathf.PI / 4) * 3 && m_ledPinStates[2] != 1)
                        SetLEDPinStates(2, 1);
                    if (angle > (-Mathf.PI / 4) * 3 && angle < -Mathf.PI / 4 && m_ledPinStates[3] != 1)
                        SetLEDPinStates(3, 1);
                }
            }

            if (sensorActivated && m_stateModified)
            {
                Debug.Log("QuadrantChanged");
                m_stateModified = false;
                LEDManager.SetLEDMode(m_ledPins, m_ledPinStates);
            }
            else if (m_stateModified && !sensorActivated)
            {
                Debug.Log("Proximity Sensor: Reset Pin States");
                m_stateModified = false;
                m_ledPinStates = new int[] { 0, 0, 0, 0 };
                LEDManager.SetLEDMode(m_ledPins, m_ledPinStates);
            }
        }

        private void SetLEDPinStates(int index, int value)
        {
            if(index != m_prevQuadrant)
            {
                for(int i = 0;i < m_ledPinStates.Length;i++)
                {
                    m_ledPinStates[i] = i == index? 1 : 0;
                }

                m_prevQuadrant = index;
                m_stateModified = true;  
            }

        }

        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer != GameSettings.PROXIMITY_LAYER_INDEX)
                return;
            
            objectsInRange.Add(other.gameObject);
        }

        void OnTriggerExit(Collider other)
        {
            if(other.gameObject.layer != GameSettings.PROXIMITY_LAYER_INDEX);

            objectsInRange.Remove(other.gameObject);
        }
    }
}
