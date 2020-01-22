using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveStickUI : MonoBehaviour
{
    [SerializeField]
    WaveStick _waveStick;

    [SerializeField]
    Toggle _enabled;

    // Start is called before the first frame update
    void Start()
    {
        _enabled.onValueChanged.AddListener(SetEnabled);
        
    }

    private void SetEnabled(bool isEnabled) {
        _waveStick.gameObject.SetActive(isEnabled);
    }

    // Update is called once per frame
    void Update()
    {
        _enabled.isOn = _waveStick.isActiveAndEnabled;
        
    }
}
