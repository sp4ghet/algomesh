Shader "Custom/QuadTree"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            Tags { "LightMode" = "Deferred" }

            Stencil
            {
                Comp Always
                Pass Replace
                Ref 128
            }

            HLSLPROGRAM 
            #include "Assets/Shaders/quadtree.hlsl"
            #pragma multi_compile ___ UNITY_HDR_ON
            #pragma vertex vert_object
            #pragma fragment frag
            ENDHLSL
        }

        
        Pass
        {
            Tags { "LightMode" = "Deferred" }

            Stencil
            {
                Comp Always
                Pass Replace
                Ref 128
            }

            HLSLPROGRAM 
            #include "Assets/Shaders/quadtree.hlsl"
            #pragma multi_compile ___ UNITY_HDR_ON
            #pragma vertex vert_object
            #pragma fragment frag
            ENDHLSL
        }

    }
}
