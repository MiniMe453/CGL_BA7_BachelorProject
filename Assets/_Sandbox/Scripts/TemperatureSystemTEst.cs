using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover.Temperature
{
    public struct TemperatureNode
    {
        public Vector3 position;
        public float temperature;
    }
    public class TemperatureSystemTEst : MonoBehaviour
    {
        public int size;

        // void Start()
        // {
        //     for (int i = 0; i < size; i++)
        //     {
        //         for (int j = 0; j < size; j++)
        //         {
        //             TemperatureNode tmpNode = new TemperatureNode();
        //             tmpNode.temperature = Random.Range(10f, 15f);
        //             tmpNode.position = new Vector3(i * 3f, 0f, j * 3f);

        //             Temperature.AddTemperatureNode(tmpNode);
        //         }
        //     }
        // }

        // void OnDrawGizmos()
        // {
        //     foreach (TemperatureNode node in Temperature.TemperatureNodes)
        //     {
        //         Gizmos.DrawSphere(node.position, 0.25f);
        //     }
        // }
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
            float backgroundTemp = Random.Range(backgroundTempAvg - 1f, backgroundTempAvg + 1f);

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

