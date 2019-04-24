#include "Assets/Shaders/Util/Unity/UnityCG.hlsl"

struct VertInput
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
};

struct VertOutput
{
    float4 vertex      : SV_POSITION;
    float4 screenPos   : TEXCOORD1;
    float4 worldPos    : TEXCOORD2;
    float3 worldNormal : TEXCOORD3;
};

struct GeometryOutput{
    VertOutput vert;
    float3 barycentric : TEXCOORD9;
};

struct VertShadowInput
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
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

float4 _Color;

VertOutput vert_object(VertInput v)
{
    VertOutput o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.screenPos = o.vertex;
    o.worldPos = mul(unity_ObjectToWorld, v.vertex);
    o.worldNormal = v.normal;
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


GBufferOut frag(GeometryOutput input)
{
    GBufferOut o;

    float3 bary = input.barycentric;
    float3 delta = fwidth(bary);
    bary = step(delta*.5, bary);
    float wire = min(bary.x, min(bary.y, bary.z));
    float4 color = _Color * (wire);

    o.diffuse = color;
    o.specular = float4(0,0,0,0);
    o.emission = color*unity_AmbientSky*.25 + color*.25;
    o.normal = float4(input.vert.worldNormal, 1);;
    o.depth = GetDepth(input.vert.worldPos.xyz);
    return o;
}

[maxvertexcount(3)] 
void geom(triangle VertOutput i[3],
          inout TriangleStream<GeometryOutput> stream)
{
    float3 p1 = i[0].worldPos.xyz;
    float3 p2 = i[1].worldPos.xyz;
    float3 p3 = i[2].worldPos.xyz;

    float3 triNormal = normalize(cross(p2-p1, p3-p1));
    // triNormal = float3(0,0,0);

    GeometryOutput g1, g2, g3;
    g1.vert = i[0];
    g2.vert = i[1];
    g3.vert = i[2];

    g1.vert.worldNormal = triNormal;
    g2.vert.worldNormal = triNormal;
    g3.vert.worldNormal = triNormal;

    g1.barycentric = float3(1,0,0);
    g2.barycentric = float3(0,1,0);
    g3.barycentric = float3(0,0,1);

    stream.Append(g1);
	stream.Append(g2);
	stream.Append(g3);
}