using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsSceneController : MonoBehaviour, ISceneController
{

    [SerializeField]
    Canvas _sceneMenu;

    [SerializeField]
    GameObject _effects;

    [SerializeField]
    CameraControl _camControl;


    public void Activate() {
        _sceneMenu.gameObject.SetActive(true);
        _effects.SetActive(true);
        _camControl.gameObject.SetActive(true);
    }

    public void AudioReactive(ref float[] logSample, Texture2D spectrum) {
        throw new System.NotImplementedException();
    }

    public void CameraNext() {
        _camControl.CyclePositions();
    }

    public void Deactivate() {
        _sceneMenu.gameObject.SetActive(false);
        _effects.SetActive(false);
        _camControl.gameObject.SetActive(false);
    }

    public void Init(GlobalState state) {
        _camControl.Cam = state.MainCam;

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
    
}
