using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneController {
    void Init(GlobalState state);
    void Activate();
    void Deactivate();
    void MidiNoteOn(MidiInput.LaunchNotes note);
    void MidiNoteOff(MidiInput.LaunchNotes note);
    void MidiKnob(MidiInput.LaunchKnobs knob);
    void AudioReactive(ref float[] logSample, Texture2D spectrum);
    void CameraNext();
}
