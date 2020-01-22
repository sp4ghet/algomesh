using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalState {

    public static GlobalState I;

    [SerializeField]
    int _bpm;

    [SerializeField, ColorUsage(false, true)]
    Color baseColor;

    [SerializeField]
    private Camera mainCam;

    [SerializeField]
    private Camera debugCam;

    #region PublicProperties

    public int Bpm {
        get {
            return _bpm;
        }

        set {
            _bpm = value;
        }
    }
    public Color BaseColor {
        get {
            return baseColor;
        }

        set {
            baseColor = value;
        }
    }
    
    public Camera DebugCam { get => debugCam; set => debugCam = value; }
    public Camera MainCam { get => mainCam; set => mainCam = value; }

    #endregion

}
