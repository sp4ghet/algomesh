#include "Assets/Shaders/Util/Unity/UnityCG.hlsl"
#include "Assets/Shaders/Util/Unity/UnityLightingCommon.hlsl"
#include "Assets/Shaders/PostProcess/2dsdf.hlsl"
#include "Assets/Shaders/Util/constants.hlsl"

struct VertInput
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float2 uv : TEXCOORD0;
};

struct VertObjectOutput
{
    float4 vertex      : SV_POSITION;
    float2 uv          : TEXCOORD0;
    float4 screenPos   : TEXCOORD1;
    float4 worldPos    : TEXCOORD2;
    float3 worldNormal : TEXCOORD3;
};

struct VertShadowInput
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float2 uv1    : TEXCOORD1;
};

struct VertShadowOutput
{
    V2F_SHADOW_CASTER;
    float4 screenPos : TEXCOORD1;
    float4 worldPos  : TEXCOORD2;
    float3 normal    : TEXCOORD3;
};

struct GBufferOut
{
    half4 diffuse  : SV_Target0; // rgb: diffuse,  a: occlusion
    half4 specular : SV_Target1; // rgb: specular, a: smoothness
    half4 normal   : SV_Target2; // rgb: normal,   a: unused
    half4 emission : SV_Target3; // rgb: emission, a: unused
    float depth    : SV_Depth;
};

sampler2D _MainTex;
float4 _MainTex_ST;

VertObjectOutput vert_object(VertInput v)
{
    VertObjectOutput o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.screenPos = o.vertex;
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    o.worldPos = mul(unity_ObjectToWorld, v.vertex);
    o.worldNormal = mul(unity_ObjectToWorld, float4(v.normal,1));
    return o;
}

float GetDepth(float3 pos)
{
    float4 vpPos = mul(UNITY_MATRIX_VP, float4(pos, 1.0));
#if defined(SHADER_TARGET_GLSL)
    return (vpPos.z / vpPos.w) * 0.5 + 0.5;
#else 
    return vpPos.z / vpPos.w;
#endif 
}

float4 _LightColor;
float4x4 _LightMatrix0;

float tri(float2 uv){
    return smoothstep(0.99, 1.01, uv.y/uv.x);
}


float hex(float2 uv){
    float d = polySDF(uv, 6);
    return fill(d, 0.6);
}



GBufferOut frag(VertObjectOutput i)
{
    GBufferOut o;
    // sample the texture
    fixed4 tree = tex2D(_MainTex, i.uv);

    float2 uv = tree.rg;
    float2 duv = ddx(uv) + ddy(uv);
    float2 st = (uv * 2) - 1;

    int numStates = 4;
    int state = int(floor( tree.b * 3));

    float d = 0;
    
    if(state == 0){
        d = tri(uv);
    }else if(state == 1){
        d = hex(st);
    }else if(state == 2){
        d = fill(heartSDF(st), 0.75);
    }else if(state == 3){    
        d = stroke(circleSDF(st), 0.5, 0.03);
    }


    float4 color = float4(0,0, d, 1);
    //float4 color = d;

    o.diffuse = color;
    o.specular = float4(0,0,0,0);
    o.emission = color * 0.2;
    o.normal = float4(i.worldNormal * 0.5 + 0.5, 1);;
    o.depth = GetDepth(i.worldPos.xyz);
    return o;
}
