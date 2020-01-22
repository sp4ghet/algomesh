using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(KaleidoRenderer), PostProcessEvent.AfterStack, "Custom/Kaleido", true)]
public sealed class Kaleido : PostProcessEffectSettings {

    public IntParameter split = new IntParameter { value = 0 };

}

public sealed class KaleidoRenderer : PostProcessEffectRenderer<Kaleido> {
    public override void Render(PostProcessRenderContext context) {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Kaleido"));
        sheet.properties.SetInt("_Split", settings.split.value);
        

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }

}
