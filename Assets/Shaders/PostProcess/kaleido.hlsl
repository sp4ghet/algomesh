#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
#include "Assets/Shaders/PostProcess/2dsdf.hlsl"

TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

uniform int _Split;

float4 Frag(VaryingsDefault attr) : SV_Target
{
	float2 resolution = _ScreenParams.xy;
	float2 uv = attr.texcoord;
	float2 st = attr.texcoord * 2 - 1;

	// Draw SDF for distortion effect
	for(int i=0; i < _Split; i++){
		st = kaleidoscope(st, _Time.x * 3);
	}

    uv = (st + 1) / 2;

    return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
}
