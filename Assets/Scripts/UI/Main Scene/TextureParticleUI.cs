using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureParticleUI : MonoBehaviour
{
    [SerializeField]
    TextureParticle _texParticle;

    [SerializeField]
    Toggle _enabledToggle;

    [SerializeField]
    Dropdown _model;

    [SerializeField]
    Toggle _audioReactiveModelSwitch;

    [SerializeField]
    Button _modelSwitchButton;

    [SerializeField]
    Toggle _audioReactivePop;

    [SerializeField]
    Button _popButton;

    // Start is called before the first frame update
    void Start()
    {
        _enabledToggle.onValueChanged.AddListener(SetEnabled);

        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        foreach(var name in _texParticle.ModelNames) {
            options.Add(new Dropdown.OptionData(name));
        }
        _model.options = options;
        _model.onValueChanged.AddListener(SetModel);

        _audioReactiveModelSwitch.onValueChanged.AddListener(SetAudioSwitchModel);

        _modelSwitchButton.onClick.AddListener(SwitchModel);

        _audioReactivePop.onValueChanged.AddListener(SetAudioPop);

        _popButton.onClick.AddListener(Pop);
        
    }

    private void Pop() {
        if (!_texParticle.isActiveAndEnabled) return;
        _texParticle.Pop();
    }
    
    private void PopOn(float t) {
        if (t == 0f) {
            Pop();
        }
    }

    private void SetAudioPop(bool arg0) {
        if (!_texParticle.isActiveAndEnabled) return;

        if (arg0) {

            AudioReactiveManager.I.LowQuarterDelegate += PopOn;
        }
        else {
            AudioReactiveManager.I.LowQuarterDelegate -= PopOn;
        }
    }

    private void SwitchModel() {
        if (!_texParticle.isActiveAndEnabled) return;
        _texParticle.CyclePCache();
    }

    private void SetAudioSwitchModel(bool arg0) {
        if (!_texParticle.isActiveAndEnabled) return;
        throw new NotImplementedException();
    }

    private void SetModel(int modelIdx) {
        if (!_texParticle.isActiveAndEnabled) return;
        _texParticle.SetPCache(modelIdx);
    }

    private void SetEnabled(bool isEnabled) {
        _texParticle.gameObject.SetActive(isEnabled);
    }

    // Update is called once per frame
    void Update()
    {
        _model.value = _texParticle.CurrentIndex;
        _enabledToggle.isOn = _texParticle.isActiveAndEnabled;
    }
}
