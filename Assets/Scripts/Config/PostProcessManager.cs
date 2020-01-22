using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Kino.PostProcessing;
using System;

public class PostProcessManager : MonoBehaviour
{
    public static PostProcessManager I;

    [SerializeField]
    private PostProcessVolume ppVolume;

    [Space]
    [SerializeField]
    private float maxBloomIntensity = 15;

    private Glitch glitch;
    private Warp warp;
    private Bloom bloom;
    private Inversion inversion;
    private Recolor recolor;
    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;
    private Kaleido kaleido;

    public Glitch Glitch { get => glitch; set => glitch = value; }
    public Warp Warp { get => warp; set => warp = value; }
    public Bloom Bloom { get => bloom; set => bloom = value; }
    public Inversion Inversion { get => inversion; set => inversion = value; }
    public Recolor Recolor { get => recolor; set => recolor = value; }
    public ChromaticAberration ChromaticAberration { get => chromaticAberration; set => chromaticAberration = value; }
    public float MaxBloomIntensity { get => maxBloomIntensity; set => maxBloomIntensity = value; }
    public Kaleido Kaleido { get => kaleido; set => kaleido = value; }

    public void BloomControl(float t) {
        bloom.intensity.value = t * MaxBloomIntensity;
        bloom.diffusion.value = t * 10;
    }

    public void ChromAbControl(float t) {
        chromaticAberration.intensity.value = t;
    }

    public void GlitchProgress(float t) {
        glitch.progress.value = t;
    }

    Coroutine barrelRoutine;
    public void BarrelPop() {
        if (barrelRoutine != null) return;
        barrelRoutine = StartCoroutine(BarrelPopCoroutine(0.15f));
    }

    private IEnumerator BarrelPopCoroutine(float duration) {
        float t = 0f;
        while(t < duration) {
            t += Time.deltaTime;
            Barrel(t/duration);
            yield return null;
        }
        Barrel(1f);
        barrelRoutine = null;
    }

    public void Barrel(float t) {
        chromaticAberration.intensity.value = 1 - t;
        lensDistortion.intensity.value = 50f - 50f * t;
    }

    public void SetKaleidoSplit(int split) {
        kaleido.split.value = split;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        if(I != null && I != this) {
            Destroy(gameObject);
        }
        I = this;

        var profile = ppVolume.profile;
        profile.TryGetSettings(out glitch);
        profile.TryGetSettings(out warp);
        profile.TryGetSettings(out bloom);
        profile.TryGetSettings(out inversion);
        profile.TryGetSettings(out recolor);
        profile.TryGetSettings(out chromaticAberration);
        profile.TryGetSettings(out lensDistortion);
        profile.TryGetSettings(out kaleido);
        
    }

    private void Start() {
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
