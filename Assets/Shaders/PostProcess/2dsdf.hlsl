#ifndef TWO_D_SDF_H
#define TWO_D_SDF_H

#include "Assets/Shaders/Util/constants.hlsl"

// All SDFs expect UV to be 0 at center of screen
// bottom left being negative and top right being positive
// SIGNED_DISTANCE_FIELDS
float rectSDF(float2 st, float2 size){
  return max(abs(st).x * size.x, abs(st).y * size.y);
}

float crossSDF(float2 st, float s){
  float2 size = float2(.25, s);
  return min(rectSDF(st, size.xy),
    rectSDF(st, size.yx));
}

float circleSDF(float2 uv){
  return length(uv);
}

float vesicaSDF(float2 uv, float w){
  float2 offset = float2(w*.5, 0.);
  return max(circleSDF(uv+offset),
            circleSDF(uv-offset));
}

float raySDF(float2 uv, int count){
  return frac(atan2(uv.x, uv.y)/SP4GHET_TWO_PI*float(count));
}

float polySDF(float2 uv, int vertices){
  float a = atan2(uv.x, uv.y)+SP4GHET_PI;
  float r = length(uv);
  float v = SP4GHET_TWO_PI / float(vertices);
  return cos(floor(.5+a/v)*v-a)*r;
}

float triSDF(float2 uv){
  return max(abs(uv.x) * .866025 + uv.y * .5,
              -uv.y * .5);
}

float rhombSDF(float2 uv){
  float2 offset = float2(0., .1);
  return max(triSDF(uv-offset),
    triSDF(float2(uv.x, -uv.y)+offset));
}

float starSDF(float2 uv, int V, float s){
  float a = atan2(uv.y, uv.x)/SP4GHET_TWO_PI;
  float seg = a * float(V);
  a = ((floor(seg) + .5)/float(V) +
    lerp(s, -s, step(.5, frac(seg)))) * SP4GHET_TWO_PI;
  return abs(dot(float2(cos(a), sin(a)),uv));
}

float heartSDF(float2 uv){
  uv -= float2(0, .3);
  float r = length(uv)*5.;
  uv = normalize(uv);
  return r - ((uv.y*pow(abs(uv.x), 0.67))/
    (uv.y+1.5)-(2.)*uv.y+1.26);
}

float flowerSDF(float2 uv, int N){
  float r = length(uv)*2.;
  float a = atan2(uv.y, uv.x);
  float v = float(N)*.5;

  return 1.-(abs(cos(a*v))*.5 + .5)/r;
}

float spiralSDF(float2 uv, float t){
  float r = length(uv);
  float a = atan2(uv.y, uv.x);

  return abs(sin(frac(log(r)*t + a*.159)));
}

//END_SIGNED_DISTANCE_FIELDS

//STEPPERS

float fill(float x, float size){
  return 1.-step(size, x);
}

float stroke(float x, float s, float w){
  float d = step(s, x+w*.5) -
            step(s, x-w*.5);
  return clamp(d, 0, 1);
}

float flip(float v, float pct){
  return lerp(v, 1-v, pct);
}

float3 bridge(float3 c, float d, float s, float w){
 c *= 1. - stroke(d,s,w*2.);
 return c + stroke(d,s,w);
}

// END_STEPPERS

// TRANSFORM_UV

float2 rotate(float2 uv, float angle){
   float2x2 rotMat = float2x2(cos(angle), -sin(angle),
                   sin(angle), cos(angle));
    return mul(rotMat, uv);
}

float2 kaleidoscope(float2 uv, float lTime){
    float2 p = abs(uv*1.5) - 1.0;

    p = rotate(p, lTime);

    return p;
}

// END_TRANSFORM_UV

#endif