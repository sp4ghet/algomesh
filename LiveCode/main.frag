precision mediump float;

uniform float time;
uniform vec2 resolution;

#define PI 3.14159265

float noise(vec2 p){
  return sin(p.x)+cos(p.y);
}

vec2 rot2(vec2 uv, float a){
  vec2 c = vec2(cos(a), sin(a));
  return mat2(c.x, -c.y, c.y, c.x) * uv;
}

float map(vec3 p){
  vec3 p2=vec3(atan(p.x, p.y), length(p.xy + sin(time+p.z/5.)) - sin(time)*sin(time)*2., p.z);
  p2.x += sin(p.z + time)*3.;
  // p2 = p;

  // sphere
  float d = length(p + vec3(1.,1.,0.)*sin(time)) - .25;

  //plane (using p2 which makes it a tunnel)
  d = min(d,dot(p2 - noise(p2.xy*5.)*.05, vec3(0.,-1.,0.)) + 2.);
  return d;
}

vec3 normal(vec3 p, float d){
  vec2 eps = vec2(0., 1e-4);
  return normalize(vec3(map(p+eps.yxx) - d, map(p+eps.xyx) - d, map(p+eps.xxy) - d));
}

vec3 fog(vec3 rgb, float d, vec3 rd, vec3 lDir){
  float b = .2;
  float b2 = sin(time);
  float fogAmount = 1.0 - exp( -d*b );
  float sunAmount = 1. - exp(-max(dot(-rd,lDir), 0.) * b2);
  vec3  fogColor  = mix(vec3(0.5,0.6,0.7), vec3(1., .1, .55), pow(sunAmount, 1.));
  return mix( rgb, fogColor, fogAmount );
}


void main(){
  vec2 uv = gl_FragCoord.xy / resolution;
  uv = (uv*2.) - 1.;

  uv.y *= resolution.y/resolution.x;

  int uvMat = int(floor(fract(time/8.) * 16.));
  if(uvMat == 0){
      uv.x = abs(uv.x);
  }else if(uvMat == 1){
      uv.y = abs(uv.y);
  }else if(uvMat > 5 && uvMat < 8){
    uv = abs(uv);
  }

  //chromatic aberattion loop
  vec3 c = vec3(0.);
  for(int i=0; i < 3; i++){
    uv *= 1.1;
    vec3 ro = vec3(0.,0.,-5.);
    vec3 rd = normalize(vec3(uv, 1.));

    // t is total distance
    // d is step distance
    float t = 0., d = 0.;
    vec3 p;
    // raymarching loop
    for(int i=0; i < 100; i++){
      p = ro + rd * t;
      d = map(p);
      t += d;
      if(d < 0.001 || d > 100.){break;}
    }

    //lighting calculations
    vec3 n = normal(p, d);

    vec3 lPos = vec3(3., 3., -3.);
    vec3 sunPos = vec3(0., 0., 5.);
    vec3 sunDir = normalize(sunPos - p);
    vec3 lDir = normalize(lPos - p);

    vec3 col = vec3(0.); // ambient
    col += max(vec3(0.), vec3(1.) * dot(n, lDir)); //directional

    // add fog
    col = fog(col, length(p-ro), rd, sunDir);
    c[i] = col[i];
  }

  // c *= vec3(1.6, 1.2, 1.2);
  c = clamp(c, 0., 1.);
  gl_FragColor =  vec4(c, 1.);
}
