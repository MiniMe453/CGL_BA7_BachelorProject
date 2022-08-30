using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover.Temperature
{
    public class TemperatureNode
    {
        public Vector3 position;
        public float temperature;
    }

    public static class Temperature
    {
        private static List<TemperatureNode> m_temperatureNodes = new List<TemperatureNode>();
        public static List<TemperatureNode> TemperatureNodes { get { return m_temperatureNodes; } }
        private static float backgroundTempAvg = 10f;
        public static void AddTemperatureNode(TemperatureNode node)
        {
            m_temperatureNodes.Add(node);
        }

        public static float ReadTemperatureFromLocation(Vector3 location)
        {
            int nodeCount = 0;
            float nodeTempFull = 0f;
            location.y = 0;
            float backgroundTemp = Random.Range(backgroundTempAvg - 0.5f, backgroundTempAvg + 0.5f);

            foreach (TemperatureNode node in m_temperatureNodes)
            {
                if (Vector3.Distance(location, node.position) > 50f)
                    continue;

                //Calculate the distance between the point and the location of the sensor. Invert it.
                float tmp = 1 - Vector3.Distance(location, node.position) / 50f;
                //Calculate the weighted value of the temperature of that node 
                tmp *= node.temperature;

                nodeCount++;
                nodeTempFull += tmp;
            }

            if (nodeCount > 0)
                return (nodeTempFull / nodeCount) + backgroundTemp;
            else
                return backgroundTemp;
        }
    }
}

