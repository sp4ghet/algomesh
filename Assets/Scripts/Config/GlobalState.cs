using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GlobalState : MonoBehaviour {

    static GlobalState _instance;

    [SerializeField]
    float _bpm;

    [SerializeField, ColorUsage(false, true)]
    Vector4 baseColor;

    [SerializeField]
    private float warp;

    [SerializeField]
    private float glitch;

    [SerializeField]
    private float inversion;

    public static GlobalState I { get {return _instance;} }

    #region PublicProperties

    public float Bpm {
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
    
    public float Warp {
        get {
            return warp;
        }

        set {
            warp = value;
        }
    }

    public float Glitch {
        get {
            return glitch;
        }
        set {
            glitch = value;
        }
    }

    public float Inversion { get => inversion; set => inversion = value; }

    #endregion


    #region MonoBehaviour

    private void OnEnable() {
        if (GlobalState.I == null) {
            GlobalState._instance = this;
        }
    }

    #endregion
}
