using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class MidiInput : MonoBehaviour {

    [SerializeField] SceneController sceneController;

    //Launch Control XL
    // Top Knob Row
    //
    // 13 14 15 16 17 18 19 20
    // 29 30 31 32 33 34 35 36
    // 49 50 51 52 53 54 55 56
    //
    // Sliders
    //
    // 77 78 79 80 81 82 83 84
    //
    // Buttons(Notes) Bottom Rows
    // (All Notes Channel 9)
    // 41 42 43 45 57 58 59 60
    // 73 74 75 76 89 90 91 92


    enum MidiNotes : int {
        bpmTap = 42
        ,recolorTap = 73
    }

    enum MidiKnobs : int {
        warpSlider = 77
        ,glitchSlider = 78
        ,inversionKnob = 49
        ,recolorSlider = 79
        ,lowThreshKnob = 13
        ,bandThreshKnob = 14
        ,highThreshKnob = 15
    }

    void NoteOn(MidiChannel channel, int note, float velocity) {
        MidiNotes _note = (MidiNotes)note;
        switch (_note) {
        case MidiNotes.bpmTap:
            sceneController.SetBpm();
            break;
        case MidiNotes.recolorTap:
            sceneController.NewRecolor();
            break;
        }
    }

    void NoteOff(MidiChannel channel, int note) {
        MidiNotes _note = (MidiNotes)note;
        switch (_note) {
        }
    }

    void Knob(MidiChannel channel, int knobNumber, float knobValue) {
        MidiKnobs knob = (MidiKnobs)knobNumber;
        switch (knob) {
        case MidiKnobs.warpSlider:
            sceneController.SetWarp(knobValue);
            break;
        case MidiKnobs.glitchSlider:
            sceneController.SetGlitch(knobValue);
            break;
        case MidiKnobs.inversionKnob:
            sceneController.SetInversion(knobValue);
            break;
        case MidiKnobs.recolorSlider:
            sceneController.RecolorOpacity(knobValue);
            break;
        case MidiKnobs.lowThreshKnob:
            sceneController.SetLowThresh(knobValue);
            break;
        case MidiKnobs.bandThreshKnob:
            sceneController.SetBandThresh(knobValue);
            break;
        case MidiKnobs.highThreshKnob:
            sceneController.SetHighThresh(knobValue);
            break;
        }
    }

    void OnEnable() {
        MidiMaster.noteOnDelegate += NoteOn;
        MidiMaster.noteOffDelegate += NoteOff;
        MidiMaster.knobDelegate += Knob;
    }

    void OnDisable() {
        MidiMaster.noteOnDelegate -= NoteOn;
        MidiMaster.noteOffDelegate -= NoteOff;
        MidiMaster.knobDelegate -= Knob;
    }
}
