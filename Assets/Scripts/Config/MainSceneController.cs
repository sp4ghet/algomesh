using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainSceneController : MonoBehaviour, ISceneController {
    [SerializeField]
    private GameObject effects;

    [SerializeField]
    private Canvas sceneUI;

    [SerializeField]
    private CameraControl camControl;

    [SerializeField]
    RaymarchingRenderer _world;

    [SerializeField]
    Kvant.Warp _spaceWarp;

    [SerializeField]
    TextureParticle _texParticle;

    public void Activate() {
        effects.SetActive(true);
        sceneUI.gameObject.SetActive(true);
        camControl.gameObject.SetActive(true);
    }

    public void AudioReactive(ref float[] logSample, Texture2D spectrum) {
        throw new System.NotImplementedException();
    }

    public void CameraNext() {
        camControl.CyclePositions();
    }

    public void Deactivate() {
        effects.SetActive(false);
        sceneUI.gameObject.SetActive(false);
        camControl.gameObject.SetActive(false);
    }

    public void Init(GlobalState state) {
        sceneUI.worldCamera = state.DebugCam;
        camControl.Cam = state.MainCam;
    }

    public void MidiKnob(MidiInput.LaunchKnobs knob) {
        throw new System.NotImplementedException();
    }

    public void MidiNoteOff(MidiInput.LaunchNotes note) {
        throw new System.NotImplementedException();
    }

    public void MidiNoteOn(MidiInput.LaunchNotes note) {
        throw new System.NotImplementedException();
    }


    public void SetSpeed(float t) {
        _spaceWarp.throttle = t;
        _world.Speed = 5 * t;
    }

    public void TexturePop() {
        _texParticle.Pop();
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


}
