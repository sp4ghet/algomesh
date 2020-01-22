using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarpSlider : MonoBehaviour
{
    [SerializeField]
    Slider warpSlider;

    Warp warp;

    void SetWarp(float warpLevel) {
        warp.warp.value = warpLevel;
    }

    // Start is called before the first frame update
    void Start()
    {
        warp = PostProcessManager.I.Warp;
        warpSlider.onValueChanged.AddListener(SetWarp);
    }

    // Update is called once per frame
    void Update()
    {
        warpSlider.value = warp.warp.value;
        
    }
}
