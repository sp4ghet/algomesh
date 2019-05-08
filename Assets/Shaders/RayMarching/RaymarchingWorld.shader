Shader "Raymarching/World"
{

Properties
{
    _MainTex ("Main Texture", 2D) = "" {}
    _Color ("Color", Color) = (0,0,0,0)
    _Floor ("Floor Sensitivity", Float) = 0
    _Grid ("Grid Size", Float) = 0
}

SubShader
{

Cull Off

Pass
{
    Tags { "LightMode" = "Deferred" }

    Stencil
    {
        Comp Always
        Pass Replace
        Ref 255
    }

    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #pragma target 3.0
    #pragma multi_compile ___ UNITY_HDR_ON

    #define PI 3.14159265358979
    #define EPSILON 1e-5

    float4 _Color;
    float _Scale;    
    float _Floor;
    float _Grid;
    float _Bpm;
    float _GridRotation;
    float _SpeedProgress;
    sampler2D _MainTex;

    #include "SDFWorld.cginc"
    #include "Raymarching.cginc"
    #include "shading.cginc"


    GBufferOut frag(VertOutput i)
    {
        float3 rayDir = GetRayDirection(i.screenPos);

        float3 camPos = GetCameraPosition();
        float maxDist = GetCameraMaxDistance();

        float distance = 0.0;
        float len = 0.0;
        float3 pos = camPos + _ProjectionParams.y * GetCameraForward();
        float smallStep = .5;


        float3 finalPos = (float3)0;
        int finalIter = 0;
        float2 dMat;
        for (int i = 0; i < 200; ++i) {
            dMat = DistanceFunction(pos);
            distance = dMat.x;
            len += distance * smallStep;
            pos += rayDir * distance * smallStep;
            if (distance < 0.001 || len > maxDist) {
                finalPos = pos;
                finalIter = i;
            };
        }

        if (distance > 0.001) discard;

        float depth = GetDepth(finalPos);
        float3 normal = GetNormalOfDistanceFunction(finalPos);
        

        GBufferOut o = shading(pos, normal, dMat.y);
        o.depth    = depth;
        o.normal   = float4(normal, 1.0);
#ifndef UNITY_HDR_ON
        o.emission = exp2(-o.emission);
#endif

        return o;
    }

    ENDCG
}

}

Fallback Off
}