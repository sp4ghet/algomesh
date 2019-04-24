using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Klak.Motion;
using Kino.PostProcessing;
using System;
using BoidsSimulationOnGPU;

public class SceneController : MonoBehaviour {

    private SceneController I;

    [SerializeField]
    PostProcessVolume ppVolume;

    [SerializeField]
    RaymarchingRenderer worldRaymarch;

    [SerializeField]
    RaymarchingObject raymarchObject;

    [SerializeField]
    GPUBoids boids;

    [SerializeField]
    TextureParticle gpgpuParticle;

    [SerializeField]
    RadialMeshGenerator radialMesh;

    [SerializeField]
    List<QuadTreeInstancing> quadtreePoppers;

    private PostProcessProfile profile;
    private Glitch glitch;
    private Inversion inversion;
    private Warp warp;
    private ChromaticAberration chromaticAberration;
    private Recolor recolor;

    float lastBpm = 0;

    public void SetSpectrum(Texture2D spectrum) {
        worldRaymarch.Spectrum = spectrum;
    }

    public void SetWarp(float warpValue) {
        warpValue = Mathf.Clamp01(warpValue);
        warp.warp.value = warpValue;
    }

    private void setBpm(float bpm) {
        AudioReactiveManager.I.Bpm = bpm;
        worldRaymarch.Bpm = bpm;
        radialMesh.Bpm = bpm;
        gpgpuParticle.Bpm = bpm;
    }

    public void SetBpm() {
        float sinceLastBpm = Time.time - lastBpm;
        lastBpm = Time.time;
        if (sinceLastBpm > 2f) {
            return;
        }

        float bpm = 60f / sinceLastBpm;
        setBpm(bpm);
    }

    public void SetInversion(float value) {
        inversion.state.value = value;
    }

    public void SetGlitch(float knobValue) {
        glitch.progress.value = knobValue * Glitch.MaxProgress;
        chromaticAberration.intensity.value = knobValue;
    }

    public void NewRecolor() {
        var grad = GradientGenerator.RandomGradient();
        recolor.fillGradient.value = grad;
    }

    public void RecolorOpacity(float value) {
        recolor.fillOpacity.value = value;
    }

    private float volumeLerp(float t) {
       return Mathf.Lerp(10, 40, t);
    }

    public void SetLowThresh(float value) {
        AudioReactiveManager.I.LowThresh = volumeLerp(value);
        radialMesh.MeshThreshold = volumeLerp(value);
    }

    public void SetBandThresh(float value) {
        AudioReactiveManager.I.BandThresh = volumeLerp(value);
    }

    public void SetHighThresh(float value) {
        AudioReactiveManager.I.HighThresh = volumeLerp(value);
    }

    private void OnEnable() {
        if(I == null) {
            I = this;
        }
        else {
            Debug.LogError("There should only be one instance of SceneController in the scene");
        }
    }

    // Use this for initialization
    void Start() {
        profile = ppVolume.profile;
        profile.TryGetSettings(out glitch);
        profile.TryGetSettings(out inversion);
        profile.TryGetSettings(out warp);
        profile.TryGetSettings(out chromaticAberration);
        profile.TryGetSettings(out recolor);


        Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON.
        // Check if additional displays are available and activate each.
        //if (Display.displays.Length > 1)
        //    Display.displays[1].Activate();
        if (Display.displays.Length > 2) {
            Display.displays[2].Activate();
        }
        setBpm(120);
    }

    private void Update() {
        SetSpectrum(AudioReactive.I.Spectrum);
    }

}