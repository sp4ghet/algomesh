Shader "Hidden/Glitch"
{
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag
                #include "glitch.hlsl"
            ENDHLSL
        }
    }
}
