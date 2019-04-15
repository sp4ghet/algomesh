using UnityEngine;
using System.Collections;

public class GradientGenerator{

    static GradientAlphaKey[] alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1, 0) };     

    public static Gradient RandomGradient() {
        Vector4 r = new Vector4(
            Random.Range(0.5f, 1)
            , Random.Range(1f, 2.0f)
            , Random.Range(0, Mathf.PI)
            , Random.Range(0f, 0.2f));
        Vector4 g = new Vector4(
            Random.Range(0.5f, 1)
            , Random.Range(0.5f, 2.0f)
            , Random.Range(0, Mathf.PI)
            , Random.Range(0f, 0.2f));
        Vector4 b = new Vector4(
            Random.Range(0.5f, 1)
            , Random.Range(0.5f, 2.0f)
            , Random.Range(0, Mathf.PI)
            , Random.Range(0f, 0.2f)); 

        return CosineGradient(r, g, b);
    }

    public static Gradient CosineGradient(Vector4 r, Vector4 g, Vector4 b) {
        int keys = 7;
        GradientColorKey[] colorKeys = new GradientColorKey[keys];
        
        for(int i=0; i < keys; i++) {
            float t = (float)i / keys;
            float red = CosineWave(r.x, r.y, r.z, r.w, t);
            float green = CosineWave(g.x, g.y, g.z, g.w, t);
            float blue = CosineWave(b.x, b.y, b.z, b.w, t);
            Color c = new Color(red, green, blue);
            colorKeys[i] = new GradientColorKey(c, t);
        }

        Gradient gradient = new Gradient();
        gradient.SetKeys(colorKeys, alphaKeys);
        
        return gradient;
    }

    static float CosineWave(float amp, float freq, float phase, float offset, float t) {
        return amp * Mathf.Cos(freq * t * 2 * Mathf.PI + phase) + offset;
    }
}


