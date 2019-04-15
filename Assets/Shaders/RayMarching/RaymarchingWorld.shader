Shader "Raymarching/World"
{

Properties
{
    _MainTex ("Main Texture", 2D) = "" {}
    _Color ("Color", Color) = (0,0,0,0)
}

SubShader
{

Tags { "RenderPipeline"="HDRenderPipeline" "RenderType" = "Opaque" "DisableBatching" = "True" "Queue" = "Geometry+10" }
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

    #include "SDFWorld.cginc"
    float _Scale;
    #include "Raymarching.cginc"

    
    float4 _Color;

    GBufferOut frag(VertOutput i)
    {
        float3 rayDir = GetRayDirection(i.screenPos);

        float3 camPos = GetCameraPosition();
        float maxDist = GetCameraMaxDistance();

        float distance = 0.0;
        float len = 0.0;
        float3 pos = camPos + _ProjectionParams.y * rayDir;
        float smallStep = .5;
        
        for (int i = 0; i < 200; ++i) {
            distance = DistanceFunction(pos);
            len += distance * smallStep;
            pos += rayDir * distance * smallStep;
            if (distance < 0.001 || len > maxDist) break;
        }

        if (distance > 0.001) discard;

        float depth = GetDepth(pos);
        float3 normal = GetNormalOfDistanceFunction(pos);
        
        GBufferOut o;
        o.diffuse  = _Color;
        o.specular = 0;
        o.emission = float4(0,0,0,0);
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