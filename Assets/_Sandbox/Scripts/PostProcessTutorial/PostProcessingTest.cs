using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessingTest : MonoBehaviour
{
    [SerializeField]
    private Material postProcessMaterial;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, postProcessMaterial);
    }
}
