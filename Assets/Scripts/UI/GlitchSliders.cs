using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlitchSliders : MonoBehaviour
{
    [SerializeField]
    Slider _progress;

    [SerializeField]
    Slider _scale;
    

    Glitch glitch;

    void SetScale(float scale) {
        glitch.scale.value = scale;
    }

    void SetProgress(float progress) {
        glitch.progress.value = progress;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        glitch = PostProcessManager.I.Glitch;

        _progress.onValueChanged.AddListener(SetProgress);
        _scale.onValueChanged.AddListener(SetScale);

        _progress.maxValue = Glitch.MaxProgress;
        _scale.maxValue = Glitch.MaxScale;
    }

    // Update is called once per frame
    void Update()
    {
        _progress.value = glitch.progress.value;
        _scale.value = glitch.scale.value;
    }
}
