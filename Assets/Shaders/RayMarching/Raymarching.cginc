// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#ifndef raymarching_h
#define raymarching_h

#include "UnityCG.cginc"

float3 GetCameraPosition()    { return _WorldSpaceCameraPos;      }
float3 GetCameraForward()     { return -UNITY_MATRIX_V[2].xyz;    }
float3 GetCameraUp()          { return UNITY_MATRIX_V[1].xyz;     }
float3 GetCameraRight()       { return UNITY_MATRIX_V[0].xyz;     }
float  GetCameraFocalLength() { return abs(UNITY_MATRIX_P[1][1]); }
float  GetCameraMaxDistance() { return _ProjectionParams.z - _ProjectionParams.y; }

float2 GetScreenPos(float4 screenPos)
{
#if UNITY_UV_STARTS_AT_TOP
     screenPos.y *= -1.0;
#endif
    screenPos.x *= _ScreenParams.x / _ScreenParams.y;
    return screenPos.xy / screenPos.w;
}

float3 GetRayDirection(float4 screenPos)
{
    float2 sp = GetScreenPos(screenPos);

    float3 camPos      = GetCameraPosition();
    float3 camDir      = GetCameraForward();
    float3 camUp       = GetCameraUp();
    float3 camSide     = GetCameraRight();
    float  focalLen    = GetCameraFocalLength();
    float  maxDistance = GetCameraMaxDistance();

    return normalize((camSide * sp.x) + (camUp * sp.y) + (camDir * focalLen));
}

float3 GetRayDirectionForShadow(float4 screenPos)
{
    float4 sp = screenPos;

#if UNITY_UV_STARTS_AT_TOP
    sp.y *= -1.0;
#endif
    sp.xy /= sp.w;

    float3 camPos      = GetCameraPosition();
    float3 camDir      = GetCameraForward();
    float3 camUp       = GetCameraUp();
    float3 camSide     = GetCameraRight();
    float  focalLen    = GetCameraFocalLength();
    float  maxDistance = GetCameraMaxDistance();

    return normalize((camSide * sp.x) + (camUp * sp.y) + (camDir * focalLen));
}

float GetDepth(float3 pos)
{
    float4 vpPos = mul(UNITY_MATRIX_VP, float4(pos, 1.0));
// #if defined(SHADER_TARGET_GLSL)
    // return (vpPos.z / vpPos.w) * 0.5 + 0.5;
// #else 
    return vpPos.z / vpPos.w;
// #endif 
}

float3 GetNormalOfDistanceFunction(float3 pos)
{
    float d = 0.001;
    return 0.5 + 0.5 * normalize(float3(
        DistanceFunction(pos + float3(  d, 0.0, 0.0)) - DistanceFunction(pos + float3( -d, 0.0, 0.0)),
        DistanceFunction(pos + float3(0.0,   d, 0.0)) - DistanceFunction(pos + float3(0.0,  -d, 0.0)),
        DistanceFunction(pos + float3(0.0, 0.0,   d)) - DistanceFunction(pos + float3(0.0, 0.0,  -d))));
}

#ifdef OBJ_RAYMARCH
inline float3 ToLocal(float3 pos)
{
    return mul(unity_WorldToObject, float4(pos, 1.0)).xyz * _Scale;
}

inline float ObjectSpaceDistanceFunction(float3 pos)
{
    return DistanceFunction(ToLocal(pos));
}

float3 GetNormalOfObjectSpaceDistanceFunction(float3 pos)
{
    float d = 0.001;
    return 0.5 + 0.5 * normalize(float3(
        ObjectSpaceDistanceFunction(pos + float3(  d, 0.0, 0.0)) - ObjectSpaceDistanceFunction(pos + float3( -d, 0.0, 0.0)),
        ObjectSpaceDistanceFunction(pos + float3(0.0,   d, 0.0)) - ObjectSpaceDistanceFunction(pos + float3(0.0,  -d, 0.0)),
        ObjectSpaceDistanceFunction(pos + float3(0.0, 0.0,   d)) - ObjectSpaceDistanceFunction(pos + float3(0.0, 0.0,  -d))));
}
#endif // OBJ_RAYMARCH

struct VertInput
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
};

struct VertOutput
{
    float4 vertex    : SV_POSITION;
    float4 screenPos : TEXCOORD0;
};

struct VertObjectOutput
{
    float4 vertex      : SV_POSITION;
    float4 screenPos   : TEXCOORD0;
    float4 worldPos    : TEXCOORD1;
    float3 worldNormal : TEXCOORD2;
};

struct VertShadowInput
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float2 uv1    : TEXCOORD1;
};

struct VertShadowOutput
{
    V2F_SHADOW_CASTER;
    float4 screenPos : TEXCOORD1;
    float4 worldPos  : TEXCOORD2;
    float3 normal    : TEXCOORD3;
};

struct GBufferOut
{
    half4 diffuse  : SV_Target0; // rgb: diffuse,  a: occlusion
    half4 specular : SV_Target1; // rgb: specular, a: smoothness
    half4 normal   : SV_Target2; // rgb: normal,   a: unused
    half4 emission : SV_Target3; // rgb: emission, a: unused
    float depth    : SV_Depth;
};

VertOutput vert(VertInput v)
{
    VertOutput o;
    o.vertex = v.vertex;
    o.screenPos = o.vertex;
    return o;
}

VertObjectOutput vert_object(VertInput v)
{
    VertObjectOutput o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.screenPos = o.vertex;
    o.worldPos = mul(unity_ObjectToWorld, v.vertex);
    o.worldNormal = mul(unity_ObjectToWorld, v.normal);
    return o;
}

VertShadowOutput vert_shadow(VertShadowInput v)
{
    VertShadowOutput o;
    TRANSFER_SHADOW_CASTER_NORMALOFFSET(o);
    o.screenPos = o.pos;
    o.worldPos = mul(unity_ObjectToWorld, v.vertex);
    o.normal = v.normal;
    return o;
}

#endif