using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaymarchWorldUI : MonoBehaviour
{
    [SerializeField]
    RaymarchingRenderer _world;

    [SerializeField]
    Slider _audioSens;

    [SerializeField]
    Slider _speed;

    // Start is called before the first frame update
    void Start()
    {
        _audioSens.onValueChanged.AddListener(SetSens);
        _speed.onValueChanged.AddListener(SetSpeed);
    }

    private void SetSpeed(float speed) {
        _world.Speed = speed;
    }

    private void SetSens(float sens) {
        _world.AudioReactiveSensitivity = sens;
    }

    // Update is called once per frame
    void Update()
    {
        _audioSens.value = _world.AudioReactiveSensitivity;
        _speed.value = _world.Speed;
    }
}
