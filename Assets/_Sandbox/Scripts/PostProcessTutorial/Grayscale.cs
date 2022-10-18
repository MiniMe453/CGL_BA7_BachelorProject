using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(GrayscaleRenderer), PostProcessEvent.AfterStack, "Custom/Grayscale")]
public sealed class Grayscale : PostProcessEffectSettings
{
    [Range(0f, 10000f), Tooltip("Grayscale effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 200f };
    public ColorParameter nearColor = new ColorParameter();
    public ColorParameter distantColor = new ColorParameter();
}

public sealed class GrayscaleRenderer : PostProcessEffectRenderer<Grayscale>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Grayscale"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        sheet.properties.SetColor("_NearColor", settings.nearColor);
        sheet.properties.SetColor("_DistanceColor", settings.distantColor);
        sheet.properties.SetMatrix("_camToWorld", Camera.current.cameraToWorldMatrix);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}