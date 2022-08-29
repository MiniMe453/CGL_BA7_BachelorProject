using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualizeLines : MonoBehaviour
{
    public bool visualizeLines = false;
    public GameObject imageTemplate;

    void OnValidate()
    {
        if (visualizeLines)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (i % 2 == 0)
                {
                    Transform child = transform.GetChild(i);
                    Instantiate(imageTemplate, child);
                }
            }
        }
    }
}
