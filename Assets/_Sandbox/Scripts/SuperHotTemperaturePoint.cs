using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Temperature;

public class SuperHotTemperaturePoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TemperatureNode node = new TemperatureNode();
        node.temperature = 100f;
        node.position = transform.position;

        Temperature.AddTemperatureNode(node);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
