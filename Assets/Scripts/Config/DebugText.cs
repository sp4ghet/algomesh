using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugText : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI fpsText;


    // Use this for initialization
    void Start () {
        StartCoroutine(FpsCounter());
	}

    IEnumerator FpsCounter() {
        while (true) {
            fpsText.text = $"fps: {Mathf.Round(1 / Time.deltaTime)}";
            yield return new WaitForSeconds(0.1f);
        }
    }

	// Update is called once per frame
	void Update () {
        
	}
}
