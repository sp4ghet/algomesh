using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(WarpRenderer), PostProcessEvent.AfterStack, "Custom/Warp", true)]
public sealed class Warp : PostProcessEffectSettings {
    [Range(0f, 1f), Tooltip("Warp.")]
    public FloatParameter warp = new FloatParameter { value = 0f };
}

public sealed class WarpRenderer : PostProcessEffectRenderer<Warp> {
    RenderTexture m_prevFrame;
    UnityEngine.Rendering.RenderTargetIdentifier m_prevTarget;
    public override void Render(PostProcessRenderContext context) {
        if(m_prevFrame == null) {
            m_prevFrame = RenderTexture.GetTemporary(context.width, context.height);
            m_prevTarget = new UnityEngine.Rendering.RenderTargetIdentifier(m_prevFrame);
            context.command.Blit(context.source, m_prevTarget);
        }
        var sheet = context.propertySheets.Get(Shader.Find("Custom/Warp"));
        sheet.properties.SetFloat("_Blend", settings.warp.value);
        sheet.properties.SetTexture("_Backbuffer", m_prevFrame);
        
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        context.command.Blit(context.destination, m_prevTarget);
        m_prevFrame.IncrementUpdateCount();
    }

    public override void Release() {
        base.Release();
        RenderTexture.ReleaseTemporary(m_prevFrame);
        m_prevFrame = null;
    }
}