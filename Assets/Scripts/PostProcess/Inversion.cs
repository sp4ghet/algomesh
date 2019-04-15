using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(InversionRenderer), PostProcessEvent.AfterStack, "Custom/Inversion", true)]
public sealed class Inversion : PostProcessEffectSettings {
    
    [Range(0f, 1f), Tooltip("InversionState")]
    public FloatParameter state = new FloatParameter { value = 0f };

    public TextureParameter logo = new TextureParameter { value = null };
}

public sealed class InversionRenderer : PostProcessEffectRenderer<Inversion> {
    public override void Render(PostProcessRenderContext context) {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Inversion"));
        sheet.properties.SetFloat("_State", settings.state.value);
        Texture2D logo = settings.logo.value == null ? new Texture2D(0, 0, TextureFormat.RGBA32, false) : (Texture2D)settings.logo.value;
        sheet.properties.SetTexture("_Logo", logo);
        
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
    
}
