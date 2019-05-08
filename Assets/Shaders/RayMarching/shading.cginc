#ifndef shading_h
#define shading_h

float3 HUEtoRGB(in float H){
	H=frac(H);
	float R = abs(H * 6 - 3) - 1;
	float G = 2 - abs(H * 6 - 2);
	float B = 2 - abs(H * 6 - 4);
	return saturate(float3(R,G,B));
}
float3 HSVtoRGB(in float3 HSV){
	float3 RGB = HUEtoRGB(HSV.x);
	return ((RGB - 1) * HSV.y + 1) * HSV.z;
}

GBufferOut shading(float3 pos, float3 normal, float mat){
    
    GBufferOut o;
    o.diffuse = 0;
    o.emission = 0;
    if(mat < EPSILON){
        pos.z -= 100;
        pos.z += _SpeedProgress;
        float manhattan = abs(pos.x) - abs(pos.z);
        manhattan = manhattan;

        o.diffuse = _Color * step(fmod(abs(manhattan), 3), 1.5);
    }else{
        o.diffuse.a = 1;
        o.emission.rgb = HSVtoRGB(float3(pos.z/10., .75, .5)) * .35;
    }
    o.specular = float4((float3)1,1);
    return o;
}

#endif //shading_h