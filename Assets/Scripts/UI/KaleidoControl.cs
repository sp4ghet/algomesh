using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class KaleidoControl : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown splitDropdown;

    // Start is called before the first frame update
    void Start()
    {
        splitDropdown.onValueChanged.AddListener(SetKaleidoSplit);
        
    }

    private void SetKaleidoSplit(int split) {
        PostProcessManager.I.SetKaleidoSplit(split);
    }

    // Update is called once per frame
    void Update()
    {
        splitDropdown.value = PostProcessManager.I.Kaleido.split.value;


    }
}
