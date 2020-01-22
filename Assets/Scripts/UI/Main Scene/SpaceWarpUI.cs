using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceWarpUI : MonoBehaviour
{
    [SerializeField]
    Kvant.Warp _warp;

    [SerializeField]
    Slider _speedSlider;

    [SerializeField]
    Slider _throttleSlider;

    // Start is called before the first frame update
    void Start()
    {

        _speedSlider.onValueChanged.AddListener(SetSpeed);
        _throttleSlider.onValueChanged.AddListener(SetThrottle);
        
    }

    private void SetThrottle(float throttle) {
        
        _warp.throttle = throttle;
    }

    private void SetSpeed(float speed) {
        _warp.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        _speedSlider.value = _warp.speed;
        _throttleSlider.value = _warp.throttle;
        
    }
}
