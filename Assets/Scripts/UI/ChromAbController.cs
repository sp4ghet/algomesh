using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using System;

public class ChromAbController : MonoBehaviour
{
    [SerializeField]
    Slider _intensity;

    ChromaticAberration chromaticAberration;

    // Start is called before the first frame update
    void Start()
    {
        chromaticAberration = PostProcessManager.I.ChromaticAberration;
        _intensity.onValueChanged.AddListener(SetIntensity);
        
    }

    private void SetIntensity(float intensity) {
        chromaticAberration.intensity.value = intensity;
    }

    // Update is called once per frame
    void Update()
    {
        _intensity.value = chromaticAberration.intensity.value;
    }
}
