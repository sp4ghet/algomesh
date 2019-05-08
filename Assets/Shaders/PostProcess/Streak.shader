Shader "Hidden/Streak"
{
    Properties
    {
        _Speed ("Speed", Float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLINCLUDE
            #include "streak.hlsl"
            ENDHLSL

            HLSLPROGRAM

            #pragma vertex VertDefault
            #pragma fragment frag
            ENDHLSL
        }
    }
}
