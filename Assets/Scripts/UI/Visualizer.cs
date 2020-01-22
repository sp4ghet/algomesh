using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Visualizer : MonoBehaviour
{

    LineRenderer lineRenderer;
    RectTransform _transform;
    Vector3[] _vertices;

    int _logSampleCount;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        _transform = (RectTransform)transform;
        _logSampleCount = AudioReactive.I.LogSamples.Length;
        _vertices = new Vector3[_logSampleCount];
        lineRenderer.positionCount = _logSampleCount;
    }

    // Update is called once per frame
    void Update()
    {

        for(int i=0; i < _logSampleCount; i++) {
            float h = AudioReactive.I.LogSamples[i];
            float visW = _transform.rect.width;
            float visH = _transform.rect.height / 2f;
            float x = ((float)i / _logSampleCount) * visW - (visW / 2f);
            _vertices[i] = new Vector3(x, h * visH - (visH / 2f), 0);
        }

        lineRenderer.SetPositions(_vertices);
    }
}
