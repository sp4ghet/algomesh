Shader "Custom/QuadTree"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
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
            #pragma target 4.6

            #include "Assets/Shaders/quadtree.hlsl"
            #pragma multi_compile ___ UNITY_HDR_ON
            #pragma vertex vert_object
            #pragma geometry geom
            #pragma fragment frag
            ENDHLSL
        }

    }
}
