
vec3 fog(vec3 rgb, float d, vec3 rd, vec3 lDir){
  float b = .2;
  float fogAmount = 1.0 - exp( -d*b );
  float sunAmount = max(dot(-rd,lDir), 0.);
  vec3  fogColor  = mix(vec3(0.5,0.6,0.7), vec3(1., .1, .55), pow(sunAmount, 1.));
  return mix( rgb, fogColor, fogAmount );
}
