// These are here to please the visual studio plugin
#include "Primitives.cginc"
#include "Utils.cginc"
#include "UnityCG.cginc"

#define E 2.71828182

float soundFloor(float3 pos){
    float d;

    pos.y += 2;

    float size = .5;
    int2 idx = pos.xz/size - fmod(pos.xz, size);
    float fftRadius = 15/size;
    float x = abs(idx.x) + abs(idx.y);
	x /= fftRadius;
    float yOffset = tex2D(_MainTex, float2(x, 0)).r * (1-step(1, x));

    float3 newPos = repeat(pos, size);

    newPos = float3(newPos.x, pos.y - yOffset*_Floor, newPos.z);
    //newP = rotate(newP, yOffset, normalize(float3(1,1,1)));

    d = roundBox(newPos, (float3).7*size, .15*size);  
    return d;
}

float spatialGrid(float3 pos){
    float d;

    float gridSize = 1;
    pos.z += _SpeedProgress % (gridSize*100);
    
    float3 idx = pos / gridSize - pos % gridSize;
    pos.z += sin(idx.x + idx.y)*.25;
    pos.x += cos(idx.z + idx.y)*.25;
    pos.y += sin(idx.z + idx.x)*.25;

    pos = repeat(pos, gridSize);
    
    pos = rotate(pos, _GridRotation, float3(1,1,1));

    d = sdOctahedron(pos, .15 * _Grid);

    return d;
}

float2 DistanceFunction(float3 pos)
{
    float d = 999999999.;
    float mat = 0;
    float soundPlane = soundFloor(pos);
    float grid = spatialGrid(pos);
    mat = step(grid, soundPlane);
    d = min(soundPlane, grid);

    return float2(d, mat);
}