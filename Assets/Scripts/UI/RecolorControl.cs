using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kino.PostProcessing;

public class RecolorControl : MonoBehaviour
{
    [SerializeField]
    Toggle _isEnabled;

    [SerializeField]
    Button _changeGradient;

    [SerializeField]
    RawImage _gradient;

    Recolor recolor;
    Color[] gradient;
    Texture2D gradTexture;

    // Start is called before the first frame update
    void Start()
    {
        recolor = PostProcessManager.I.Recolor;

        _isEnabled.onValueChanged.AddListener(SetRecolor);
        _changeGradient.onClick.AddListener(SetNewGradient);
        gradTexture = new Texture2D(160, 1);
        _gradient.texture = gradTexture;
        gradient = new Color[160];
        
    }

    private void SetNewGradient() {
        recolor.fillGradient.value = GradientGenerator.RandomGradient();
    }

    private void SetRecolor(bool isOn) {
        recolor.enabled.value = isOn;
    }

    // Update is called once per frame
    void Update()
    {
        _isEnabled.isOn = recolor.enabled.value;
        for (int i = 0; i < gradient.Length; i++) {
            float t = i / (gradient.Length - 1f);
            gradient[i] = recolor.fillGradient.value.Evaluate(t);
            
        }
        gradTexture.SetPixels(gradient);
        gradTexture.Apply();

    }
}
