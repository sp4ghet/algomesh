using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactiveManager : MonoBehaviour
{

    public static AudioReactiveManager I;

    private float singleBeat = 0.5f;
    private float bpm = 120;
    private float phase = 0;

    private float lowThresh = 25;
    private float bandThresh = 25;
    private float highThresh = 25;

    public float Bpm { get => bpm; set => bpm = value; }
    public float LowThresh { get => lowThresh; set => lowThresh = value; }
    public float BandThresh { get => bandThresh; set => bandThresh = value; }
    public float HighThresh { get => highThresh; set => highThresh = value; }

    float highTwoMeasureTiming = 0;
    float highWholeTiming = 0;
    float highHalfTiming = 0;
    float highQuarterTiming = 0;
    float highEigthTiming = 0;
    float highTripletTiming = 0;
    float highTripEigthTiming = 0;

    float bandTwoMeasureTiming = 0;
    float bandWholeTiming = 0;
    float bandHalfTiming = 0;
    float bandQuarterTiming = 0;
    float bandEigthTiming = 0;
    float bandTripletTiming = 0;
    float bandTripEigthTiming = 0;

    float lowTwoMeasureTiming = 0;
    float lowWholeTiming = 0;
    float lowHalfTiming = 0;
    float lowQuarterTiming = 0;
    float lowEigthTiming = 0;
    float lowTripletTiming = 0;
    float lowTripEigthTiming = 0;

    private IEnumerator LowTwoMeasure() {
        float t = 0;
        while (t < singleBeat * 8) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator LowWhole() {
        float t = 0;
        while (t < singleBeat * 4) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator LowHalf() {
        float t = 0;
        while (t < singleBeat * 2) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator LowQuarter() {
        float t = 0;
        while (t < singleBeat) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator LowEigth() {
        float t = 0;
        while (t < singleBeat / 2f) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator LowTriplet() {
        float t = 0;
        while (t < singleBeat / 3) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator LowTripletEigth() {
        float t = 0;
        while (t < singleBeat / 6) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }


    private IEnumerator BandTwoMeasure() {
        float t = 0;
        while (t < singleBeat * 8) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator BandWhole() {
        float t = 0;
        while (t < singleBeat * 4) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator BandHalf() {
        float t = 0;
        while (t < singleBeat * 2) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator BandQuarter() {
        float t = 0;
        while (t < singleBeat) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator BandEigth() {
        float t = 0;
        while (t < singleBeat / 2f) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator BandTriplet() {
        float t = 0;
        while (t < singleBeat / 3) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator BandTripletEigth() {
        float t = 0;
        while (t < singleBeat / 6) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }


    private IEnumerator HighTwoMeasure() {
        float t = 0;
        while (t < singleBeat * 8) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator HighWhole() {
        float t = 0;
        while (t < singleBeat * 4) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator HighHalf() {
        float t = 0;
        while (t < singleBeat * 2) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator HighQuarter() {
        float t = 0;
        while (t < singleBeat) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator HighEigth() {
        float t = 0;
        while (t < singleBeat / 2f) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator HighTriplet() {
        float t = 0;
        while (t < singleBeat / 3) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator HighTripletEigth() {
        float t = 0;
        while (t < singleBeat / 6) {
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnEnable() {
        if (I == null) {
            I = this;
        }
        else {
            Debug.LogError("There should only be one instance of AudioReactiveManager in the scene");
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        singleBeat = 60f / bpm;
        float nudge = singleBeat * phase;
        float timing = Time.time + nudge;

        float safety = Time.deltaTime * 4;


        // low pass
        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat*8) < Time.deltaTime && timing - highTwoMeasureTiming > safety) {
            highTwoMeasureTiming = timing;
            StartCoroutine(LowTwoMeasure());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat * 4) < Time.deltaTime && timing - highWholeTiming > safety) {
            highWholeTiming = timing;
            StartCoroutine(LowWhole());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat * 2) < Time.deltaTime && timing - highHalfTiming > safety) {
            highHalfTiming = timing;
            StartCoroutine(LowHalf());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % singleBeat < Time.deltaTime &&  timing - highQuarterTiming > safety) {
            highQuarterTiming = timing;
            StartCoroutine(LowQuarter());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat/2) < Time.deltaTime && timing - highEigthTiming > safety) {
            highEigthTiming = timing;
            StartCoroutine(LowEigth());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat / 3) < Time.deltaTime && timing - highTripletTiming > safety) {
            highTripletTiming = timing;
            StartCoroutine(LowTriplet());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat / 6) < Time.deltaTime && timing - highTripEigthTiming > safety) {
            highTripEigthTiming = timing;
            StartCoroutine(LowTripletEigth());
        }

        // band pass
        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat*8) < Time.deltaTime && timing - highTwoMeasureTiming > safety) {
            highTwoMeasureTiming = timing;
            StartCoroutine(BandTwoMeasure());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat * 4) < Time.deltaTime && timing - highWholeTiming > safety) {
            highWholeTiming = timing;
            StartCoroutine(BandWhole());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat * 2) < Time.deltaTime && timing - highHalfTiming > safety) {
            highHalfTiming = timing;
            StartCoroutine(BandHalf());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % singleBeat < Time.deltaTime &&  timing - highQuarterTiming > safety) {
            highQuarterTiming = timing;
            StartCoroutine(BandQuarter());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat/2) < Time.deltaTime && timing - highEigthTiming > safety) {
            highEigthTiming = timing;
            StartCoroutine(BandEigth());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat / 3) < Time.deltaTime && timing - highTripletTiming > safety) {
            highTripletTiming = timing;
            StartCoroutine(BandTriplet());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat / 6) < Time.deltaTime && timing - highTripEigthTiming > safety) {
            highTripEigthTiming = timing;
            StartCoroutine(BandTripletEigth());
        }

        // high pass
        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat*8) < Time.deltaTime && timing - highTwoMeasureTiming > safety) {
            highTwoMeasureTiming = timing;
            StartCoroutine(HighTwoMeasure());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat * 4) < Time.deltaTime && timing - highWholeTiming > safety) {
            highWholeTiming = timing;
            StartCoroutine(HighWhole());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat * 2) < Time.deltaTime && timing - highHalfTiming > safety) {
            highHalfTiming = timing;
            StartCoroutine(HighHalf());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % singleBeat < Time.deltaTime &&  timing - highQuarterTiming > safety) {
            highQuarterTiming = timing;
            StartCoroutine(HighQuarter());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat/2) < Time.deltaTime && timing - highEigthTiming > safety) {
            highEigthTiming = timing;
            StartCoroutine(HighEigth());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat / 3) < Time.deltaTime && timing - highTripletTiming > safety) {
            highTripletTiming = timing;
            StartCoroutine(BandTriplet());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat / 6) < Time.deltaTime && timing - highTripEigthTiming > safety) {
            highTripEigthTiming = timing;
            StartCoroutine(BandTripletEigth());
        }


    }
}
