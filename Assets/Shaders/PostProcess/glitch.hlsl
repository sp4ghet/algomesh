#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
#include "Assets/Shaders/Util/colors.hlsl"
#include "Assets/Shaders/PostProcess/2dsdf.hlsl"
#include "Assets/Shaders/Util/2dnoise.hlsl"
TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

TEXTURE2D_SAMPLER2D(_Control, sampler_Control);
TEXTURE2D_SAMPLER2D(_Spectrum, sampler_Spectrum);

uniform float _Volume;
uniform float _Progress;
uniform float _Scale;


float rand(float n){
    return frac(sin(n) * 43758.5453123);
}

float noise(float p){
    float fl = floor(p);
    float fc = frac(p);
    return lerp(rand(fl), rand(fl + 1.0), fc);
}

float4 Frag(VaryingsDefault attr) : SV_Target
{
    float2 resolution = _ScreenParams.xy;
    float2 uv = attr.texcoord;
    float2 st = attr.texcoord * 2 - 1;

    // Draw SDF for distortion effect
    for(int i=0; i < floor(4 - _Progress * 4); i++){
        st = kaleidoscope(st, _Time.x);
    }

    float r = dot(st, st);
    float a = atan2(st.y, st.x)/(PI);
    st = float2(a,r);
    float2 grid = float2(5, log(r)*20.);
    float2 st_i = floor(st*grid);
    st.x += .5*fmod(st_i.y, 2.);
    float2 st_f = frac(st*grid);
    st_f -= .5;
    float shape = rhombSDF(st_f);
    float4 distortion_flow = SAMPLE_TEXTURE2D(_Spectrum, sampler_Spectrum, float2(shape*0.35, 0.));
    
    // shader parameters
    float squareSize = (1 - _Progress + 0.15);
    float phase = _Volume;
    float scale = _Scale;
    float flow = distortion_flow.x*fmod(_Time.y*0.001, 1.);
    float progress = _Progress * 100;

    float4 distortTex = SAMPLE_TEXTURE2D(_Control, sampler_Control, uv);
    float2 distort = float2(distortTex.r*0.4,uv.y);
    float pixels = 100. * squareSize;
    float2 newUV = (uv*resolution) / min(resolution.x, resolution.y);
    newUV = round(newUV*pixels) / pixels;
    float4 pixelTexture = float4(rand(pnoise(newUV + _Time.y, 20.)), rand(pnoise(newUV + _Time.y + _Time.x, 20.)), 0, 1);
    float3 tmp = RGBtoHSL(pixelTexture.rgb);
    pixelTexture.rgb = HSLtoRGB(tmp);
    tmp.x += phase*HALF_PI;
    pixelTexture.rgb = HSLtoRGB(tmp);
    float2 offset = float2(0.,0.);
    if (pixelTexture.r>0.5) {
        offset.x += scale;        
    } else{
        offset.x -= scale;
    }
    if (pixelTexture.g<0.5) {
        offset.y += scale;
    } else{
        offset.y -= scale;
    }
    offset.y += 0.001 * flow;
    float4 fromC = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, frac(offset + uv + rotate(float2(flow, 0.), scale)));
    float4 toC = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
    float4 c = lerp(fromC, toC, progress+1);
    return c;
}