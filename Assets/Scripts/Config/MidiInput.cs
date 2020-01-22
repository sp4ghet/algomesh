using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class MidiInput : MonoBehaviour {


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

    //Arturia Beatstep
    //
    // Knobs
    //  10 74 71 76 77 93 73 75
    // 114 18 19 16 17 91 79 72
    //
    // Buttons
    // 44 45 46 47 48 49 50 51
    // 36 37 38 39 40 41 42 43

    [SerializeField]
    MidiChannel LaunchControl = MidiChannel.Ch9;
    [SerializeField]
    MidiChannel Arturia = MidiChannel.Ch1;

    public enum LaunchNotes : int {

    }

    public enum LaunchKnobs : int {
        Bloom = 77,
        ChromAb = 78,
        GlitchProgress = 79,
        MainSpeed = 84,
        LowThresh = 49,
        BandThresh = 29,
        HighThresh = 13

    }

    public enum ArturiaNotes : int {
        BpmTap = 44,
        LensDistort = 36,
        CameraNext = 37,
        TexturePop = 43
    }

    public enum ArturiaKnobs : int {

    }

    void NoteOn(MidiChannel channel, int note, float velocity) {
        if (channel == LaunchControl) {
            LaunchNotes _note = (LaunchNotes)note;
            switch (_note) {
            default:
                break;
            }
        }
        if (channel == Arturia) {
            var _note = (ArturiaNotes)note;
            switch (_note) {
            case ArturiaNotes.BpmTap:
                SceneMan.I.BpmTap();
                break;
            case ArturiaNotes.CameraNext:
                SceneMan.I.CameraNext();
                break;
            case ArturiaNotes.LensDistort:
                PostProcessManager.I.BarrelPop();
                break;
            case ArturiaNotes.TexturePop:
                SceneMan.I.MainTexturePop();
                break;
            }
        }
    }

    void NoteOff(MidiChannel channel, int note) {
        if (channel == LaunchControl) {
            LaunchNotes _note = (LaunchNotes)note;
            switch (_note) {
            default: break;
            }
        }
        if (channel == Arturia) {
            var _note = (ArturiaNotes)note;
            switch (_note) {
            default: break;
            }
        }
    }

    void Knob(MidiChannel channel, int knobNumber, float knobValue) {
        if(channel == LaunchControl) {
            LaunchKnobs knob = (LaunchKnobs)knobNumber;
            switch (knob) {
            case LaunchKnobs.Bloom:
                PostProcessManager.I.BloomControl(knobValue);
                break;
            case LaunchKnobs.ChromAb:
                PostProcessManager.I.ChromAbControl(knobValue);
                break;
            case LaunchKnobs.MainSpeed:
                SceneMan.I.SetMainSpeed(knobValue);
                break;
            case LaunchKnobs.GlitchProgress:
                PostProcessManager.I.GlitchProgress(knobValue);
                break;
            case LaunchKnobs.LowThresh:
                AudioReactiveManager.I.SetThresh(Lasp.FilterType.LowPass, knobValue * 100);
                break;
            case LaunchKnobs.BandThresh:
                AudioReactiveManager.I.SetThresh(Lasp.FilterType.BandPass, knobValue * 100);
                break;
            case LaunchKnobs.HighThresh:
                AudioReactiveManager.I.SetThresh(Lasp.FilterType.HighPass, knobValue * 100);
                break;
            }
        }
        if(channel == Arturia) {
            ArturiaKnobs knob = (ArturiaKnobs)knobNumber;
            switch (knob) {
            default: break;
            }
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
