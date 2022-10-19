using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Settings;

namespace Rover.Systems
{
    public class System_GPS : MonoBehaviour
    {
        private static float m_gpsCoordY;
        private static float m_gpsCoordX;
        public static Vector2 GPSCoordinates {get {return new Vector2(m_gpsCoordX, m_gpsCoordY);} }
        private static float m_heading;
        public static float Heading { get { return m_heading; } }
        
        void Update()
        {
            m_gpsCoordX = GameSettings.GPS_COORD_X_MIN + ((gameObject.transform.position.x/GameSettings.TERRAIN_X_MAX) * (GameSettings.GPS_COORD_X_MAX - GameSettings.GPS_COORD_X_MIN));
            m_gpsCoordY = GameSettings.GPS_COORD_Y_MIN + ((gameObject.transform.position.z/GameSettings.TERRAIN_Y_MAX) * (GameSettings.GPS_COORD_Y_MAX - GameSettings.GPS_COORD_Y_MIN));

            // m_heading = Mathf.Abs(gameObject.transform.rotation.y);
            // Debug.LogError(m_heading);

        }
    }

}
