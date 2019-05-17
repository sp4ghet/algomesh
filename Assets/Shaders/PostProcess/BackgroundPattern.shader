Shader "Hidden/BackgroundPattern"
{
    Properties
    {
        _Blend ("Blend", Float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLINCLUDE
            #include "backgroundpattern.hlsl"
            ENDHLSL

            HLSLPROGRAM

            #pragma vertex VertDefault
            #pragma fragment frag
            ENDHLSL
        }
    }
}
