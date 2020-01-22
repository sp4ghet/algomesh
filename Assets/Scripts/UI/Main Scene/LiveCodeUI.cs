using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Klak.Ndi;
using System;

public class LiveCodeUI : MonoBehaviour
{
    [SerializeField]
    NdiReceiver _liveCode;

    [SerializeField]
    Toggle _enabled;

    // Start is called before the first frame update
    void Start()
    {
        _enabled.onValueChanged.AddListener(SetEnabled);
    }

    private void SetEnabled(bool isEnabled) {
        _liveCode.gameObject.SetActive(isEnabled);
    }

    // Update is called once per frame
    void Update()
    {
        _enabled.isOn = _liveCode.isActiveAndEnabled;
        
    }
}
