#define PI 3.14159265

vec3 tunnel(vec3 p){
  return vec3(atan(p.x, p.y), -length(p.xy), p.z);
}

vec3 rotate(vec3 p, float angle, vec3 axis)
{
    vec3 a = normalize(axis);
    float s = sin(angle);
    float c = cos(angle);
    float r = 1.0 - c;
    mat3 m = mat3(
        a.x * a.x * r + c,
        a.y * a.x * r + a.z * s,
        a.z * a.x * r - a.y * s,
        a.x * a.y * r - a.z * s,
        a.y * a.y * r + c,
        a.z * a.y * r + a.x * s,
        a.x * a.z * r + a.y * s,
        a.y * a.z * r - a.x * s,
        a.z * a.z * r + c
    );
    return m * p;
}

vec3 logSphericalPlane(vec3 p, inout float mul){
  vec3 p3 = p;
  float r = length(p3.xz);
  float scl = 6. / PI;
  mul = r / scl;
  p3 = vec3(log(r), p3.y / mul, atan(p3.x, p3.z));
  p3.xz *= scl;
  p3.x -= time;
  float size = 1.;
  p3.xz = mod(p3.xz, size) * 2. - size;

  return p3;
}

float plane(vec3 p, vec3 normal, float distFromCamera){
  return dot(p, normal) + distFromCamera;
}

float sphere(vec3 p, float radius){
  return length(p) - radius;
}
