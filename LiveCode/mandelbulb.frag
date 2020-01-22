precision mediump float;

precision mediump float;
uniform float time;
uniform vec2  resolution;
uniform sampler2D texture;
uniform sampler2D spectrum;
uniform sampler2D samples;
uniform float volume;

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

float map(vec3 p){
  vec3 w = p;
  float m = dot(w,w);

  vec4 trap = vec4(abs(w),m);
	float dz = 1.;


	for( int i=0; i<4; i++ )
  {
    float power = 3. + 5. * abs(cos(volume));
    dz = power*pow(sqrt(m),power-1.)*dz + 1.0;
		// dz = 8.0*pow(m,3.5)*dz + 1.0;

    float r = length(w) + 5. * volume;
    float b = power*acos( w.y/r)  +  time*3.;
    float a = power*atan( w.x, w.z ) + volume*5.;
    w = p + pow(r,power) * vec3( sin(b)*sin(a), cos(b), sin(b)*cos(a) );
    trap = min( trap, vec4(abs(w),m) );

    m = dot(w,w);
		if( m > 256.0 )
            break;
  }

  return 0.25*log(m)*sqrt(m)/dz;
}

void main(){

    vec2 uv = (gl_FragCoord.xy / resolution.xy) * 2.0 - 1.0;
    uv.x *= resolution.x / resolution.y;

    vec4 c = vec4(uv, 0.75, 1.0);

    vec3 ro = vec3(0. , 0., cos(0.)) * 2.;
    vec3 rd = vec3(uv, (1.-dot(uv,uv)));
    rd = rotate(rd, 3.14159265, vec3(1., 0., 0.));

    // t is total distance
    // d is step distance
    float t = 0., d = 0.;
    vec3 p;
    // raymarching loop
    float i = 0.;
    for(int j = 0; j < 100; j++){
      i += 1.;
      p = ro + rd * t;
      d = map(p);
      t += d*.75;

      if(d < 0.001 || d > 100.){break;}
    }

    vec4 blue = vec4(0.5411, 0.90196, 0.8745, 1.);
    vec4 pink = vec4(1.0, 0.72156, 0.7686, 1.0);

    c = (1. - vec4(i / 150.)) * blue * 1.2;
    if(d > 100.) { c =  pink; }

    gl_FragColor = c;
}
