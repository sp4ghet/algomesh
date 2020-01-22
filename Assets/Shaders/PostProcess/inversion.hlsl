#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
#include "Assets/Shaders/PostProcess/2dsdf.hlsl"
#include "Assets/Shaders/Util/constants.hlsl"
TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
TEXTURE2D_SAMPLER2D(_Logo, sampler_Logo);

uniform float _State;
uniform int _UseColor;
uniform int _UseUV;

float pointyOctagon(float2 uv){
  float color = 0;
  float back = starSDF(uv, 16, .1);
  color += fill(back, .45);
  float l = 0.;
  for(int i = 0; i < 8; i++){
    float2 rot_uv = rotate(uv, float(i)*PI/4.);
    rot_uv.y -= .45;
    float tri = polySDF(rot_uv, 3);
    color += fill(tri, .2);
    l += stroke(tri, .2, .02);
  }
  color *= 1. -l;
  color -= stroke(polySDF(uv, 8), .1, .02);

  return color;
}


float eye(float2 uv){
    float color = 0;
    float rays = stroke(raySDF(rotate(uv, PI/4.), 36), .5, .2);
    float vesica = vesicaSDF(uv, .7);
    float x = _Time.y * 0.2;
    float E_x = SP4GHET_E * 100;
    float blinkRate = pow(E_x, pow(E_x, sin(x)) / E_x) / E_x;
    float eye = vesicaSDF(rotate(uv, HALF_PI) * float2(1+blinkRate*5, 1), lerp(0.7, 0.9, blinkRate));
    float pupil = circleSDF(uv-float2(0., .1));

    color += rays;
    color *= fill(vesica, .9);
    color *= 1. - fill(eye, .67);
    color += stroke(pupil, .3, .03) * fill(eye, .65);
    color += stroke(eye, .65, .03);
    return color;
}

float twistSquare(float2 uv){
    uv = uv.yx;
    float3 color = float3(0,0,0);
    float angle = -PI/4.;
    float inv = step(0., uv.y);
    uv = rotate(uv, angle);
    uv = lerp(+uv, -uv, step(.5, inv));
    uv -= .4;

    float2 scale = float2(1,1);
    for(int i = 0; i < 5; i++){
        float rect = rectSDF(uv, scale);
        float size = .25;
        size -= abs(.1 * float(i) - .2);
        color = bridge(color, rect, size, .05);
        uv += .2;
    }
    return color.r;
}

float logo(float2 uv){
    // center texture on screen
    float2 st = uv*2 - .5;
    return SAMPLE_TEXTURE2D(_Logo, sampler_Logo, st).r 
        * step(0, st.x) * step(st.x, 1) * step(0, st.y) * step(st.y, 1); // cutoff extra bits
}

float4 Frag(VaryingsDefault attr) : SV_Target
{
    float2 uv = attr.texcoord;
    float2 st = (uv * _ScreenParams.xy * 2 - _ScreenParams.xy) / min(_ScreenParams.x, _ScreenParams.y);
    
    float state = _State * 4 - EPSILON;
    int pair = int(round(state));

    float d = 0;
    if(pair == 1){
        d = eye(st);
    }else if(pair == 2){
        d = pointyOctagon(st*0.9);
    }else if(pair == 3){
		d = twistSquare(st*0.6);
    }
    else if (pair == 4) {
        d = logo(uv);
    }

	d = clamp(d, 0, 1);

    if (_UseUV) {
		uv.y = lerp(uv.y, 1-uv.y, d);
    }
    float4 screen = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
    float4 color = screen;
    if (_UseColor) {
        color = lerp(screen, 1 - screen, d);
    }
    
    return color;
}
