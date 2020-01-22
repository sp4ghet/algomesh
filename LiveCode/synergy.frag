precision mediump float;

uniform float time;
uniform float volume;
uniform sampler2D spectrum;
uniform sampler2D backbuffer;
uniform vec2 resolution;

#pragma glslify: import('./distance.frag')
#pragma glslify: import('./shading.frag')

void main(){
  vec2 uv = gl_FragCoord.xy / resolution;
  vec2 p = (uv*2.) - 1.;
  p.y *= resolution.y/resolution.x;

  float rad  = 0.2 +  volume;
  float x = length(p);
  x = smoothstep(0., abs(x - rad), 0.01);

  vec4 c = vec4(0.);
  c = vec4(1.) * x;
  c = clamp(c, 0., 1.);
  gl_FragColor =  c;
}
