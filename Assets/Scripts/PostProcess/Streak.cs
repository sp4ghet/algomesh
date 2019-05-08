using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(StreakRenderer), PostProcessEvent.BeforeStack, "Custom/Streak", true)]
public sealed class Streak : PostProcessEffectSettings {

    [Range(0f, 10f), Tooltip("Streak speed")]
    public FloatParameter speed = new FloatParameter { value = 0f };
}

public sealed class StreakRenderer : PostProcessEffectRenderer<Streak> {
    public override void Render(PostProcessRenderContext context) {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Streak"));
        sheet.properties.SetFloat("_Speed", settings.speed.value);
        

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }

}
