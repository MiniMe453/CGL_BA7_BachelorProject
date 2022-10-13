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
        private int[] ledPins = { 22, 24, 26, 28 };
        private int[] ledPinStates = { 0, 0, 0, 0 };
        public int numOfCasts = 12;
        public float sphereRadius = 2;
        public float castDistance = 10;
        private Timer proximityTimer;

        void Start()
        {
            proximityTimer = Timer.Register(GameSettings.PROXIMITY_CHECK_DELAY, () => CheckRoverProximity(), isLooped: true);
        }

        void CheckRoverProximity()
        {
            for (int i = 0; i < numOfCasts; i++)
            {
                RaycastHit hit;

                if (Physics.SphereCast(transform.position, sphereRadius, Quaternion.AngleAxis(i * (360f / numOfCasts), Vector3.up) * transform.forward, out hit, castDistance))
                {
                    if (i >= 11 || i <= 1)
                    {

                    }
                }
            }
        }
    }

}
