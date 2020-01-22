using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChirashiUI : MonoBehaviour
{
    [SerializeField]
    RaymarchingRenderer _chirashi;

    [SerializeField]
    Slider _sizeSlider;


    // Start is called before the first frame update
    void Start()
    {
        _sizeSlider.onValueChanged.AddListener(SetSize);
    }

    private void SetSize(float size) {
        _chirashi.GridSize = size;
    }

    // Update is called once per frame
    void Update()
    {
        _sizeSlider.value = _chirashi.GridSize;
        
    }
}
