using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InversionControl : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown _mask;

    [SerializeField]
    Toggle _color;

    [SerializeField]
    Toggle _uv;

    Inversion inversion;


    void SetMask(int id) {
        float state = (float)id / Inversion.maskCount;
        inversion.state.value = state;
    }

    private void SetUV(bool use) {
        inversion.useUV.value = use;
    }

    private void SetColor(bool use) {
        inversion.useColor.value = use;
    }

    // Start is called before the first frame update
    void Start()
    {
        inversion = PostProcessManager.I.Inversion;

        _mask.onValueChanged.AddListener(SetMask);
        _color.onValueChanged.AddListener(SetColor);
        _uv.onValueChanged.AddListener(SetUV);
    }

    int MaskIdFromState(float state) {
        return Mathf.RoundToInt(state * Inversion.maskCount);
    }

    // Update is called once per frame
    void Update()
    {
        _color.isOn = inversion.useColor.value;
        _uv.isOn = inversion.useUV.value;
        _mask.value = MaskIdFromState(inversion.state.value);
    }
}
