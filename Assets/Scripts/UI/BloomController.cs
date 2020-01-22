using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using System;

public class BloomController : MonoBehaviour
{
    [SerializeField]
    Slider _intensity;

    Bloom bloom;

    // Start is called before the first frame update
    void Start()
    {
        bloom = PostProcessManager.I.Bloom;

        _intensity.onValueChanged.AddListener(SetIntensity);

    }

    private void SetIntensity(float intensity) {
        bloom.intensity.value = intensity * PostProcessManager.I.MaxBloomIntensity;
        bloom.diffusion.value = intensity * 10;
    }

    // Update is called once per frame
    void Update()
    {
        _intensity.value = bloom.intensity.value / PostProcessManager.I.MaxBloomIntensity;
    }
}
