#ifndef CURVES_H
#define CURVES_H

float impulse( float k, float x )
{
    const float h = k*x;
    return h*exp(1.0-h);
}

float almostIdentity( float x, float m, float n )
{
    if( x>m ) return x;

    float a = 2.0*n - m;
    float b = 2.0*m - 3.0*n;
    float t = x/m;

    return (a*t + b)*t*t + n;
}

    
float gain(float x, float k) 
{
    float a = 0.5*pow(2.0*((x<0.5)?x:1.0-x), k);
    return (x<0.5)?a:1.0-a;
}

float nrand(float2 uv)
{
    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
}

#endif //CURVES_H