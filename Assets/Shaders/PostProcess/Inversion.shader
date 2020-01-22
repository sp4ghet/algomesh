Shader "Hidden/Inversion"
{
    HLSLINCLUDE
    #include "inversion.hlsl"
    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag


            ENDHLSL
        }
    }
}
