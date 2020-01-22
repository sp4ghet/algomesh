using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaymarchObjectUI : MonoBehaviour
{

    [SerializeField]
    RaymarchingObject _raymarch;

    [SerializeField]
    Toggle _enableToggle;

    [SerializeField]
    Slider _shape;

    // Start is called before the first frame update
    void Start()
    {
        _enableToggle.onValueChanged.AddListener(SetEnabled);
        _shape.onValueChanged.AddListener(SetShape);
    }

    private void SetShape(float lerp) {
        _raymarch.Lerp = lerp;
    }

    private void SetEnabled(bool enabled) {
        _raymarch.gameObject.SetActive(enabled);
    }

    // Update is called once per frame
    void Update()
    {
        _shape.value = _raymarch.Lerp;
        _enableToggle.isOn = _raymarch.isActiveAndEnabled;
        
    }
}
