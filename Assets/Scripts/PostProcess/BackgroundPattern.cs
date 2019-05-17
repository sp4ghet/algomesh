using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(BackgroundPatternRenderer), PostProcessEvent.BeforeStack, "Custom/BackgroundPattern", true)]
public sealed class BackgroundPattern : PostProcessEffectSettings {

    [Range(0f, 1f), Tooltip("Blend")]
    public FloatParameter blend = new FloatParameter { value = 0f };
}

public sealed class BackgroundPatternRenderer : PostProcessEffectRenderer<BackgroundPattern> {
    public override void Render(PostProcessRenderContext context) {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/BackgroundPattern"));
        sheet.properties.SetFloat("_Blend", settings.blend.value);
        

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }

    public override DepthTextureMode GetCameraFlags() {
        return DepthTextureMode.Depth;
    }

}
