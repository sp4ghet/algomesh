using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class BPMText : MonoBehaviour
{
    TMP_Text _text;

    void SetBpm(int bpm) {
        _text.text = $"bpm: {bpm}";
    }

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        SetBpm(GlobalState.I.Bpm);
    }
}
