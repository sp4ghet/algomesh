/*{ "audio": true }*/
precision mediump float;
uniform float time;
uniform vec2  resolution;
uniform sampler2D texture;
uniform sampler2D spectrum;
uniform sampler2D samples;
uniform float volume;

void main (void) {
    vec2 uv = gl_FragCoord.xy / resolution.xy;
    vec2 p = uv*2. - 1.;
    p.y *= resolution.y/resolution.x;
    vec4 color = texture2D(texture, uv);

    float freq = texture2D(spectrum, vec2(uv.x, .5)).r;
    float wave = texture2D(samples, vec2(uv.x, .5)).r;

    float s = smoothstep(0.0, abs(wave - uv.y), 0.01);
    // float g = 1. - step(0.01, abs(freq - uv.y));
    // float b = 1. - step(0.01, abs(volume - uv.y));



    float r = 0.;
    float g = 0.;
    float b = 0.;

    float vRad = 0.3;
    vRad = volume + 0.2;
    r = smoothstep(0., abs(length(p - .01) - vRad), 0.005);
    vec4 circle = vec4(r);
    g = smoothstep(0., abs(length(p - .02) - vRad), 0.005);
    b = smoothstep(0., abs(length(p - .015) - vRad), 0.005);
    vec4 shiftedCircle = vec4(r,g,b, 0.);


    // spectrum circle stuff
    vec2 rP = abs(p*.234);
    vec2 gP = abs(p*.34);
    vec2 bP = abs(p*.43);

    float cir = length(p);
    vec4 spec = vec4(cir);
    spec.r -= texture2D(spectrum, rP).r * 0.5;
    spec.g -= texture2D(spectrum, gP).r * 0.5;
    spec.b -= texture2D(spectrum, bP).r * 0.5;
    spec =  smoothstep(vec4(0.), abs(spec - vec4(0.15)), vec4(0.01));

    r = smoothstep(0.0, abs(wave - uv.y), 0.01);
    wave = texture2D(samples, vec2(uv.x * 1.02, .5)).r;
    g = smoothstep(0.0, abs(wave - uv.y), 0.01);
    wave = texture2D(samples, vec2(uv.x * 1.05, .5)).r;
    b = smoothstep(0.0, abs(wave - uv.y), 0.01);
    vec4 shiftWave = vec4(r, g, b, 1.);

    vec4 c = vec4(0.);
    // c += circle;
    c = shiftedCircle;
    // c += spec;
    // c += s;
    c += shiftWave;
    c = clamp(c, 0., 1.);


    gl_FragColor = c;
}
