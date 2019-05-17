float4 _Scale;
float _Hit;

float ifs(float3 pos){
    float d2 = 1/(1e-10);
    d2 = torus(pos, float2(0.5, 0.01));
    //d2 = smoothMin(d2, torus(rotate(pos, PI/2, float3(0,1,0)), float2(0.25,0.05)), 16);

    return d2;
}


float DistanceFunction(float3 pos)
{

    // d1 --------------------
    float3 warpPos = twistY(pos, 0.5*sin(_Time.y*0.75)); 
    warpPos = rotate(warpPos, _Time.z, float3(1,1,1));
    float torusT = torus(warpPos, float2(0.5,0.25)*_Scale*0.5);

    float3 orbit = float3(sin(_Time.x*5.145)*0.15,cos(_Time.x*5.145)*0.15,0)*_Scale;
    float sphereT = smoothMin(sphere(pos-orbit, 0.25*_Scale), sphere(pos+orbit, 0.25*_Scale), 16);
    
    float d1 = subtraction(torusT, sphereT);
    d1 = lerp(d1, subtraction(sphereT, torusT), (sin(_Time.y*1.14)+1)/2 - 1);
    
    //d2 ---------------------
    float d2 = 1e10;
    float3 pos2 = pos;
    float3 shaft = normalize(float3(1,1,1));
    for(int i=0; i < 10; i++){
        pos2 = rotate(pos2, float(i)*(PI/10) + _Time.x, shaft);
        pos2 *= 0.9;
        d2 = min(d2, ifs(pos2));
    }
    

    return lerp(d1, d2, _Hit);
}