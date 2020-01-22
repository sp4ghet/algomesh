using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public enum InversionMask : int {
    None = 0,
    Eye,
    Star,
    Knot,
    Logo
}

[Serializable]
[PostProcess(typeof(InversionRenderer), PostProcessEvent.AfterStack, "Custom/Inversion", true)]
public sealed class Inversion : PostProcessEffectSettings {
    public const int maskCount = 4;
    
    [Range(0f, 1f), Tooltip("InversionState")]
    public FloatParameter state = new FloatParameter { value = 0f };

    public BoolParameter useColor = new BoolParameter { value = true };
    public BoolParameter useUV = new BoolParameter { value = true };

    public TextureParameter logo = new TextureParameter { value = null };
}

public sealed class InversionRenderer : PostProcessEffectRenderer<Inversion> {
    public override void Render(PostProcessRenderContext context) {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Inversion"));
        sheet.properties.SetFloat("_State", settings.state.value);
        sheet.properties.SetInt("_UseUV", settings.useUV.value ? 1 : 0);
        sheet.properties.SetInt("_UseColor", settings.useColor.value ? 1 : 0);

        Texture2D logo = settings.logo.value == null ? new Texture2D(0, 0, TextureFormat.RGBA32, false) : (Texture2D)settings.logo.value;
        sheet.properties.SetTexture("_Logo", logo);
        
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
    
}
