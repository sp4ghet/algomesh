using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(GlitchRenderer), PostProcessEvent.AfterStack, "Custom/Glitch", true)]
public sealed class Glitch : PostProcessEffectSettings {
    public const float MaxProgress = 1f;
    [Range(0f, 1f), Tooltip("Glitch intensity.")]
    public FloatParameter volume = new FloatParameter { value = 0f };

    [Range(0f, MaxProgress)]
    public FloatParameter progress = new FloatParameter { value = 0f };

    [Range(0f, 0.2f), Tooltip("Glitch pixellation size")]
    public FloatParameter scale = new FloatParameter { value = 0f };
    
}

public sealed class GlitchRenderer : PostProcessEffectRenderer<Glitch> {
    Texture2D m_blankSpectrum;
    Texture2D m_spectrum;
    public override void Render(PostProcessRenderContext context) {

        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Glitch"));
        sheet.properties.SetFloat("_Volume", settings.volume.value);
        sheet.properties.SetFloat("_Progress", settings.progress.value);
        sheet.properties.SetFloat("_Scale", settings.scale.value);

        m_spectrum = AudioReactive.I?.Spectrum != null ? AudioReactive.I.Spectrum : m_blankSpectrum;
        sheet.properties.SetTexture("_Spectrum", m_spectrum);
        
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }

    public override void Init() {
        m_blankSpectrum = new Texture2D(1024, 1, TextureFormat.RFloat, false);
    }

    public override void Release() {
        UnityEngine.Object.DestroyImmediate(m_blankSpectrum);
    }
}
