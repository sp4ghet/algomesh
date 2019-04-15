// These are here to please the visual studio plugin
#include "Primitives.cginc"
#include "Utils.cginc"
#include "UnityCG.cginc"

#define E 2.71828182
#define SCALE 6./PI

float _Bpm;
sampler2D _MainTex;

float DistanceFunction(float3 pos)
{
    float d;
    pos.y += 2;
    float radius = length(pos.xz);

    float angle = atan2(pos.z, pos.x);
    
    float2 pos2d = float2(log(radius), angle);
    pos2d *= SCALE;
    pos2d.x -= _Time.y;
    pos2d = frac(pos2d) - 0.5;

    float mul = SCALE / radius;

    float fftRadius = 40;
    float x = radius/fftRadius;
    float yOffset = tex2D(_MainTex, float2(x, 0)).r;

    float3 logSphericalPos = float3(pos2d.x, mul * (pos.y - yOffset * 0.5 * step(radius, fftRadius)), pos2d.y);

    d = box(rotate(logSphericalPos, 0, float3(0,0,1)), float3(1, mul, 1) * 0.9);

    d /= mul;

    return d;
}