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
    WaveStick waveStick;

    [SerializeField]
    TextureParticle gpgpuParticle;

    [SerializeField]
    RadialMeshGenerator radialMesh;

    [SerializeField]
    GameObject quadTreePopperParent;

    [SerializeField]
    List<QuadTreeInstancing> quadtreePoppers;

    [SerializeField]
    CameraControl camControl;

    [SerializeField]
    Klak.Ndi.NdiReceiver liveCode;

    [SerializeField]
    Kvant.Warp spaceWarp;

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
        gpgpuParticle.Lifetime = 60f / bpm;
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

    int motifIdx;
    public void NextCameraMotif() {
        motifIdx = (motifIdx + 1) % camControl.MotifCount;
        camControl.CycleMotif(motifIdx);
    }

    internal void NextCameraPosition() {
        camControl.CyclePositions();
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

    public void SetLowThresh(float value) {
        AudioReactiveManager.I.LowThresh = Mathf.Lerp(40, 85, value);
        radialMesh.MeshThreshold = Mathf.Lerp(40, 85, value);
    }

    public void SetBandThresh(float value) {
        AudioReactiveManager.I.BandThresh = Mathf.Lerp(35, 80, value);
    }

    public void SetHighThresh(float value) {
        AudioReactiveManager.I.HighThresh = Mathf.Lerp(30, 70, value);
    }


    public void SetWaveStickScale(float knobValue) {
        waveStick.gameObject.SetActive(knobValue > float.Epsilon);
        waveStick.transform.localScale = Vector3.one * knobValue;
    }

    public void SetRadialMeshScale(float knobValue) {
        radialMesh.gameObject.SetActive(knobValue > float.Epsilon);
        radialMesh.transform.localScale = Vector3.one * knobValue;
    }

    public void SetObjectSpaceScale(float value) {
        raymarchObject.gameObject.SetActive(value > float.Epsilon);
        raymarchObject.transform.localScale = Vector3.one * 3f * value;
    }

    public void SetQuadTreeScale(float value) {
        quadTreePopperParent.transform.localScale = Vector3.one * value;
        var pos = quadTreePopperParent.transform.position;
        quadTreePopperParent.transform.position = new Vector3(pos.x, 16 * value, pos.z);
    }

    Vector3 originalLiveCodeScale;
    public void SetLiveCodeScale(float value) {
        liveCode.transform.localScale = originalLiveCodeScale * value;
    }


    public void ToggleBoids() {
        boids.gameObject.SetActive(!boids.gameObject.activeInHierarchy);
    }

    public void SetFloorSensitivity(float value) {
        worldRaymarch.AudioReactiveSensitivity = value;
    }

    public void SetGridSize(float value) {
        worldRaymarch.GridSize = value;
    }

    public void SetTextureCurl(float intensity) {
        gpgpuParticle.CurlIntensity = intensity;
    }

    public void SetTextureForward(float intensity) {
        gpgpuParticle.ForwardIntensity = intensity;
    }

    public void SetTextureExplode(float intensity) {
        gpgpuParticle.ExplodeIntensity = intensity;
    }


    bool gpgpuDelegateActive;
    float gpgpuTempo = 0;
    AudioReactiveManager.Pitch gpgpuPitch = AudioReactiveManager.Pitch.High;
    void cycler(float t) {
        if (t > 0f) { return; }
        gpgpuParticle.CyclePCache();
    }

    public void SetObjectSpaceLerp(float value) {
        raymarchObject.Lerp = value;
    }

    void CyclePCacheWithSavedTempo() {
        gpgpuDelegateActive = true;
        AudioReactiveManager.I.UnsubPitchAndTempo(gpgpuPitch, gpgpuTempo, cycler);
        AudioReactiveManager.I.SubPitchAndTempo(gpgpuPitch, gpgpuTempo, cycler);
    }

    public void SetPCacheTempo(float value) {
        AudioReactiveManager.I.UnsubPitchAndTempo(gpgpuPitch, gpgpuTempo, cycler);
        AudioReactiveManager.I.UnsubPitchAndTempo(AudioReactiveManager.Pitch.Band, gpgpuTempo, gpgpuParticle.Curl);
        gpgpuTempo = value;
        if(value < 1f) {
            AudioReactiveManager.I.SubPitchAndTempo(AudioReactiveManager.Pitch.Band, gpgpuTempo, gpgpuParticle.Curl);
        }
        if (gpgpuDelegateActive) {
            AudioReactiveManager.I.SubPitchAndTempo(gpgpuPitch, gpgpuTempo, cycler);
        }
    }

    public void SetPCache(float value) {
        if(value == 1f) {
            CyclePCacheWithSavedTempo();
            return;
        }
        gpgpuDelegateActive = false;
        AudioReactiveManager.I.UnsubPitchAndTempo(gpgpuPitch, gpgpuTempo, cycler);

        gpgpuParticle.SetPCache(value);
    }

    public void SetPCacheScale(float value) {
        gpgpuParticle.transform.localScale = Vector3.one * value;
    }
    
    public void MoveForward(float value) {
        float speed = -10f * value; 
        worldRaymarch.Speed = speed;
        spaceWarp.throttle = value;
        spaceWarp.speed = speed*20f;
        waveStick.Speed = -speed+5;
        quadtreePoppers.ForEach(x => x.Speed = -speed + 30);
        radialMesh.Speed = -speed+7;
    }


    // Runs OnEnable (runs in editor stuff and before Start)
    void OnEnable() {
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
        originalLiveCodeScale = liveCode.transform.localScale;


        Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON.
        // Check if additional displays are available and activate each.
        //if (Display.displays.Length > 1)
        //    Display.displays[1].Activate();
        if (Display.displays.Length > 2) {
            Display.displays[2].Activate();
        }
        setBpm(120);

        quadtreePoppers.ForEach(x => { AudioReactiveManager.I.SubLowTempoFromValue(2.9f/7f, x.NewInstance); });

    }

    private void Update() {
        SetSpectrum(AudioReactive.I.Spectrum);
    }

}