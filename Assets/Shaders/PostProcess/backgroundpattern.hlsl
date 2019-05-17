#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
#include "Assets/Shaders/Util/colors.hlsl"
TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

uniform float _Blend;

float4 frag(VaryingsDefault attr) : SV_Target
{
    float2 resolution = _ScreenParams.xy;
    float2 uv = attr.texcoord;


    float4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
    return c;
}