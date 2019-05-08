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
        bpmTap = 41
        , cameraChangeMotif = 42
        , cameraChangePosition = 43
        , textureCurl = 57
        , textureForward = 58
        , textureExplode = 59
        
        , recolorTap = 73
        , quadTreeMode = 74
        , toggleBoids = 75
    }

    enum MidiKnobs : int {
         lowThreshKnob = 13
        , bandThreshKnob = 14
        , highThreshKnob = 15
        , fftAmp = 16
        , boidsAudioSensitivity = 17
        , pCacheSelection = 20

        , cameraPosTempo = 29
        , radialMeshTempo = 31
        , quadTreeTempo = 34
        , objectSpaceTempo = 35
        , pCacheTempo = 36

        , inversionKnob = 49
        , liveCodeScale = 50
        , floorSDF = 52
        , spatialGridSDF = 53
        , objectSDF = 55
        , pCacheParticles = 56

        , warpSlider = 77
        , glitchSlider = 78
        , recolorSlider = 79
        , waveStickScale = 81
        , radialMeshScale = 80
        , quadTreeScale = 82
        , moveForward = 83
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
        case MidiNotes.cameraChangeMotif:
            sceneController.NextCameraMotif();
            break;
        case MidiNotes.cameraChangePosition:
            sceneController.NextCameraPosition();
            break;
        case MidiNotes.toggleBoids:
            sceneController.ToggleBoids();
            break;
        case MidiNotes.textureCurl:
            sceneController.SetTextureCurl(1);
            break;
        case MidiNotes.textureForward:
            sceneController.SetTextureForward(1);
            break;
        case MidiNotes.textureExplode:
            sceneController.SetTextureExplode(1);
            break;
        }
    }

    void NoteOff(MidiChannel channel, int note) {
        MidiNotes _note = (MidiNotes)note;
        switch (_note) {
        case MidiNotes.textureCurl:
            sceneController.SetTextureCurl(0);
            break;
        case MidiNotes.textureForward:
            sceneController.SetTextureForward(0);
            break;
        case MidiNotes.textureExplode:
            sceneController.SetTextureExplode(0);
            break;
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
        case MidiKnobs.waveStickScale:
            sceneController.SetWaveStickScale(knobValue);
            break;
        case MidiKnobs.radialMeshScale:
            sceneController.SetRadialMeshScale(knobValue);
            break;
        case MidiKnobs.floorSDF:
            sceneController.SetFloorSensitivity(knobValue);
            break;
        case MidiKnobs.spatialGridSDF:
            sceneController.SetGridSize(knobValue);
            break;
        case MidiKnobs.objectSDF:
            sceneController.SetObjectSpaceScale(knobValue);
            break;
        case MidiKnobs.quadTreeScale:
            sceneController.SetQuadTreeScale(knobValue);
            break;
        case MidiKnobs.liveCodeScale:
            sceneController.SetLiveCodeScale(knobValue);
            break;
        case MidiKnobs.pCacheParticles:
            sceneController.SetPCacheScale(knobValue);
            break;
        case MidiKnobs.pCacheSelection:
            sceneController.SetPCache(knobValue);
            break;
        case MidiKnobs.pCacheTempo:
            sceneController.SetPCacheTempo(knobValue);
            break;
        case MidiKnobs.moveForward:
            sceneController.MoveForward(knobValue);
            break;
        case MidiKnobs.objectSpaceTempo:
            break;
        case MidiKnobs.quadTreeTempo:
            break;
        case MidiKnobs.radialMeshTempo:
            break;
        case MidiKnobs.fftAmp:
            break;
        case MidiKnobs.cameraPosTempo:
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
