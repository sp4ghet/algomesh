#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
#include "Assets/Shaders/Util/colors.hlsl"
TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

uniform float _Speed;


float rand(float n){
    return frac(sin(n) * 43758.5453123);
}

float noise(float p){
    float fl = floor(p);
    float fc = frac(p);
    return lerp(rand(fl), rand(fl + 1.0), fc);
}

 fixed4 Recolor(half intensity)
{
    half3 c = _Color.rgb * max(intensity, .5);
    return fixed4(GammaToLinearSpace(c), _Color.a);
}

float4 frag(VaryingsDefault attr) : SV_Target
{
    float2 resolution = _ScreenParams.xy;
    float2 uv = attr.texcoord;

    float speed = lerp(1, 4, rand(uv.y * 10000));
    float4 c = Recolor(frac(uv.x / 8 + speed * _Time.y) < _Speed * 0.5);

    c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
    return c;
}