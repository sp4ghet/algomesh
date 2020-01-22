using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RmsThresholdSlider : MonoBehaviour
{
    [SerializeField]
    Lasp.FilterType filterType;

    [SerializeField]
    Slider _threshold;

    [SerializeField]
    Slider _valSlider;


    void UpdateThresh(float thresh) {
        AudioReactiveManager.I.SetThresh(filterType, thresh);
    }

    // Start is called before the first frame update
    void Start()
    {
        _threshold.onValueChanged.AddListener(UpdateThresh);
    }

    // Update is called once per frame
    void Update()
    {
        _valSlider.value = AudioReactive.I.GetRms(filterType);
        _threshold.value = AudioReactiveManager.I.GetThresh(filterType);
    }
}
