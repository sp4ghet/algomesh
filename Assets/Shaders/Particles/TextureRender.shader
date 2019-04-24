Shader "Hidden/sp4ghet/TextureRender"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard addshadow fullforwardshadows
        #pragma multi_compile_instancing
        #pragma instancing_options procedural:setup
        // Use shader model 3.0 target, to get nicer looking lighting
        //#pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float4 color : COLOR;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        int _UseColor;
        float4x4 _ObjectToWorld;
        float4x4 _WorldToObject;

        float _ObjectScale;


        // put more per-instance properties here
        #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
        StructuredBuffer<float4> _ParticlePosBuffer;
        StructuredBuffer<float4> _ParticleColorBuffer;
        #endif

        float rand(float n){
            return frac(sin(n) * 43758.5453123);
        }

        // オイラー角（ラジアン）を回転行列に変換
        float4x4 eulerAnglesToRotationMatrix(float3 angles)
        {
            float ch = cos(angles.y); float sh = sin(angles.y); // heading
            float ca = cos(angles.z); float sa = sin(angles.z); // attitude
            float cb = cos(angles.x); float sb = sin(angles.x); // bank

            // Ry-Rx-Rz (Yaw Pitch Roll)
            return float4x4(
                ch * ca + sh * sb * sa, -ch * sa + sh * sb * ca, sh * cb, 0,
                cb * sa, cb * ca, -sb, 0,
                -sh * ca + ch * sb * sa, sh * sa + ch * sb * ca, ch * cb, 0,
                0, 0, 0, 1
            );
        }

        void setup()
        {
        #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
            float4 pos = _ParticlePosBuffer[unity_InstanceID];
            pos.w = 1;
            float4x4 object2world = (float4x4)0;

            _ObjectToWorld._14_24_34 += mul(_ObjectToWorld, pos);
            unity_ObjectToWorld =  _ObjectToWorld;
            object2world = (float4x4)0;
            object2world._11_22_33 = _ObjectScale;
            object2world._44 = 1;

            float3 randomRot = float3(rand((float)unity_InstanceID/98.1834), rand((float)unity_InstanceID/19.984), rand((float)unity_InstanceID/9.091));
            object2world = mul(eulerAnglesToRotationMatrix(randomRot), object2world);

            unity_ObjectToWorld = mul(unity_ObjectToWorld, object2world);
        #endif
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c;
            
            c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
            float4 pointColor = _ParticleColorBuffer[unity_InstanceID];
            if(_UseColor){
                c *= pointColor;            
            }
            #endif
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
