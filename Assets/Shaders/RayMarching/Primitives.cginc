#ifndef primitives_h
#define primitives_h

float RecursiveTetrahedron(float3 p)
{
    const float3 a1 = float3( 1.0,  1.0,  1.0);
    const float3 a2 = float3(-1.0, -1.0,  1.0);
    const float3 a3 = float3( 1.0, -1.0, -1.0);
    const float3 a4 = float3(-1.0,  1.0, -1.0);

    const float scale = 2.0;
    float d;
    for (int n = 0; n < 20; ++n) {
        float3 c = a1; 
        float minDist = length(p - a1);
        d = length(p - a2); if (d < minDist) { c = a2; minDist = d; }
        d = length(p - a3); if (d < minDist) { c = a3; minDist = d; }
        d = length(p - a4); if (d < minDist) { c = a4; minDist = d; }
        p = scale * p - c * (scale - 1.0);
    }
 
    return length(p) * pow(scale, float(-n));
}

float sphere(float3 pos, float radius)
{
    return length(pos) - radius;
}

float roundBox(float3 pos, float3 size, float round)
{
    return length(max(abs(pos) - size * 0.5, 0.0)) - round;
}

float box(float3 pos, float3 size)
{
    return roundBox(pos, size, 0);
}

float torus(float3 pos, float2 radius)
{
    float2 r = float2(length(pos.xy) - radius.x, pos.z);
    return length(r) - radius.y;
}

float floor(float3 pos)
{
    return dot(pos, float3(0.0, 1.0, 0.0)) + 1.0;
}

float cylinder(float3 pos, float2 r)
{
    float2 d = abs(float2(length(pos.xy), pos.z)) - r;
    return min(max(d.x, d.y), 0.0) + length(max(d, 0.0)) - 0.1;
}

	
float sdCylinder( float3 p, float3 c )
{
  return length(p.xy-c.xy)-c.z;
}

float sdEllipsoid( in float3 p, in float3 r )
{
    return (length( p/r ) - 1.0) * min(min(r.x,r.y),r.z);
}

float displacement(float3 p){
    return sin(20*p.x)*sin(20*p.y)*sin(20*p.z);
}

#endif