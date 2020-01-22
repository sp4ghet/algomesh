precision mediump float;

uniform float time;
uniform vec2 resolution;

#define PI 3.14159265

// RAYMARCHING STUFF
#pragma glslify: import('./distance.frag')
#pragma glslify: import('./shading.frag')

vec2 rot2(vec2 uv, float a){
  vec2 c = vec2(cos(a), sin(a));
  return mat2(c.x, -c.y, c.y, c.x) * uv;
}

float map(vec3 p){

  float d = 10000.;
  vec3 p1 = abs(p);
  // d = sphere(p1 + vec3(1.,1.,0.)*(-1. + sin(time)) - vec3(sin(time)+1., cos(time) + 1., 0.)*.5, .25);

  vec3 p2 = tunnel(p);
  // d = min(d, plane(p2, vec3(0, 1, 0), 2.));

  vec3 p3 = p*1.;
  p3 = rotate(p3, -PI*.5, vec3(1,0,0));
  p3 += vec3(sin(time), cos(time), 0.)*.25;
  p3 = rotate(p3, sin(time)*.5, vec3(1,1,1));
  float mul;
  p3 = logSphericalPlane(p3, mul);
  d = min(d, (length(p3) - 1.)*mul);
  return d;
}

vec3 normal(vec3 p, vec3 rd){
  p -= rd*.05;
  vec2 eps = vec2(0., 1e-6);
  return normalize(vec3(map(p+eps.yxx) - map(p-eps.yxx),
                        map(p+eps.xyx) - map(p-eps.xyx),
                        map(p+eps.xxy) - map(p-eps.xxy)));
}

vec3 calcNormal(vec3 p ){
    const float h = 1e-6; // or some other value
    const vec2 k = vec2(1,-1);
    return normalize( k.xyy*map( p + k.xyy*h ) +
                      k.yyx*map( p + k.yyx*h ) +
                      k.yxy*map( p + k.yxy*h ) +
                      k.xxx*map( p + k.xxx*h ) );
}

vec3 shading(vec3 p, vec3 normal, vec3 rayOrigin, vec3 rayDir){
  vec3 lPos = vec3(3., 3., -3.);
  vec3 sunPos = vec3(0., 0., 5.);
  vec3 sunDir = normalize(sunPos - p);
  vec3 lDir = normalize(lPos - p);

  vec3 col = vec3(0.1); // ambient
  col += max(vec3(0.), vec3(1.) * dot(normal, lDir)); //directional
  col = fog(col, length(p-rayOrigin), rayDir, sunDir);

  return col;
}

vec3 rayMarching(vec2 uv){

    vec3 ro = vec3(0, 0, -5);
    // ro = vec3(sin(time)*5.,sin(time) + 5.,cos(time)*5.);
    vec3 rd = vec3(uv, 1.);
    // rd = normalize(rotate(vec3(uv, 1.), PI*.2, vec3(1,0,0)));
    // rd = rotate(rd, time - PI, vec3(0., 1., 0.));

    // t is total distance
    // d is step distance
    float t = 0., d = 0.;

    vec3 p;
    // raymarching loop
    for(int i=0; i < 100; i++){
      p = ro + rd * t;
      d = map(p);
      t += d*.75;
      if(d < 0.001 || d > 100.){break;}
    }

    vec3 n = calcNormal(p);

    return shading(p, n, ro, rd);
}

vec3 chromaticAberration(vec2 uv){
  vec3 c;
  const int iter = 1;
  for(int i=0; i < 3*iter; i++){
    uv *= 1.05;
    c[i] += rayMarching(uv)[i];
  }
  c = c / float(iter);
  return c;
}

// END RAYMARCHING STUFF

void main(){
  vec2 uv = gl_FragCoord.xy / resolution;
  uv = (uv*2.) - 1.;
  uv.y *= resolution.y/resolution.x;

  vec3 c = vec3(0.);
  c = chromaticAberration(uv);

  // c *= vec3(1.6, 1.2, 1.2);
  c = clamp(c, 0., 1.);
  gl_FragColor =  vec4(c, 1.);
}
