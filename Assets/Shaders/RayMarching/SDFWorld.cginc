// These are here to please the visual studio plugin
#include "Primitives.cginc"
#include "Utils.cginc"
#include "UnityCG.cginc"

#define E 2.71828182

float _Bpm;
sampler2D _MainTex;

float DistanceFunction(float3 pos)
{
    float d;
    pos.y += 2;

    float size = 1.;
    int2 idx = pos.xz/size - fmod(pos.xz, size);
    float fftRadius = 40;
    float x = length(idx)/40;
    float yOffset = tex2D(_MainTex, float2(x, 0)).r * (1-step(1, x));

    float3 newPos = repeat(pos, size);

    d = roundBox(float3(newPos.x, pos.y - yOffset, newPos.z), (float3).6*size, .1*size);
    // d = sphere(float3(newPos.x, pos.y - yOffset, newPos.z), .4*size);

    return d;
}