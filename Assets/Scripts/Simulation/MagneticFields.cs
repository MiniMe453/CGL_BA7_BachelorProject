using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover.Magnet
{
    public class MagneticNode
    {
        public Vector3 position;
        public float strength;
    }

    //External to the rover. This is not internal to the rover.
    public static class Magnet
    {
        private static List<MagneticNode> m_magneticNodes = new List<MagneticNode>();
        public static List<MagneticNode> MagneticNodes { get { return m_magneticNodes; } }
        private static float backgroundTempAvg = 10f;
        public static void AddMagneticNode(MagneticNode node)
        {
            m_magneticNodes.Add(node);
        }

        public static float ReadTemperatureFromLocation(Vector3 location)
        {
            int nodeCount = 0;
            float nodeTempFull = 0f;
            location.y = 0;
            float backgroundTemp = Random.Range(backgroundTempAvg - 0.5f, backgroundTempAvg + 0.5f);

            foreach (MagneticNode node in m_magneticNodes)
            {
                if (Vector3.Distance(location, node.position) > 50f)
                    continue;

                //Calculate the distance between the point and the location of the sensor. Invert it.
                float tmp = 1 - Vector3.Distance(location, node.position) / 50f;
                //Calculate the weighted value of the temperature of that node 
                tmp *= node.strength;

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

