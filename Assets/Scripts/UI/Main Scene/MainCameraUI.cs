using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCameraUI : MonoBehaviour
{
    [SerializeField]
    CameraControl _control;

    [SerializeField]
    Dropdown _motifDropdown;

    [SerializeField]
    Button _nextPos;


    // Start is called before the first frame update
    void Start()
    {
        var options = new List<Dropdown.OptionData>();
        foreach (var motif in _control.Motifs) {
            var item = new Dropdown.OptionData(motif.focus.gameObject.name);
            options.Add(item);
        }
        _motifDropdown.options = options;

        _motifDropdown.onValueChanged.AddListener(SetMotif);
        _nextPos.onClick.AddListener(NextPos);
    }

    private void NextPos() {
        _control.CyclePositions();
    }

    private void SetMotif(int motifId) {
        _control.CycleMotif(motifId);
    }

    // Update is called once per frame
    void Update()
    {
        _motifDropdown.value = _control.CurrentIndex;
    }
}
