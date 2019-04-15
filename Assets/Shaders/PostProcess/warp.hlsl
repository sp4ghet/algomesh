#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
#include "Assets/Shaders/Util/3dnoise.hlsl"
TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
TEXTURE2D_SAMPLER2D(_Backbuffer, sampler_Backbuffer);

float _Blend;

float2 nabla(float2 p, float freq){
  float3 pos = float3(p.x*freq, p.y*freq, sin(_Time.x));
  return curlC(pos).xy*SP4GHET_EPSILON + SP4GHET_EPSILON*-(p*2 - 1);
}

float4 Frag(VaryingsDefault attr) : SV_Target
{
    float2 displacement = nabla(attr.texcoord, _Blend*50);
    float2 uv = attr.texcoord+displacement*_Blend*5;
    float4 main = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, attr.texcoord);
    float4 color = SAMPLE_TEXTURE2D(_Backbuffer, sampler_Backbuffer, frac(uv));

    return color*min(0.99, _Blend*10) + main*max(0.01, 1-_Blend*10);
}
