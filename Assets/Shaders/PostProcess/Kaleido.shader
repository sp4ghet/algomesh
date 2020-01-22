Shader "Hidden/Kaleido"
{
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag
                #include "kaleido.hlsl"

            ENDHLSL
        }
    }
}
