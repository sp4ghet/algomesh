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

    public delegate void AudioReactiveProgress(float t);

    public AudioReactiveProgress LowTwoMeasureDelegate;
    public AudioReactiveProgress LowWholeDelegate;
    public AudioReactiveProgress LowHalfDelegate;
    public AudioReactiveProgress LowQuarterDelegate;
    public AudioReactiveProgress LowEigthDelegate;
    public AudioReactiveProgress LowTripletDelegate;
    public AudioReactiveProgress LowTripletEigthDelegate;

    public AudioReactiveProgress BandTwoMeasureDelegate;
    public AudioReactiveProgress BandWholeDelegate;
    public AudioReactiveProgress BandHalfDelegate;
    public AudioReactiveProgress BandQuarterDelegate;
    public AudioReactiveProgress BandEigthDelegate;
    public AudioReactiveProgress BandTripletDelegate;
    public AudioReactiveProgress BandTripletEigthDelegate;

    public AudioReactiveProgress HighTwoMeasureDelegate;
    public AudioReactiveProgress HighWholeDelegate;
    public AudioReactiveProgress HighHalfDelegate;
    public AudioReactiveProgress HighQuarterDelegate;
    public AudioReactiveProgress HighEigthDelegate;
    public AudioReactiveProgress HighTripletDelegate;
    public AudioReactiveProgress HighTripletEigthDelegate;

    public enum Pitch {
        Low,
        Band,
        High
    }

    public void SubPitchAndTempo(Pitch pitch, float t, AudioReactiveProgress callback) {
        switch (pitch) {
            case Pitch.Low:
                SubLowTempoFromValue(t, callback);
                break;
            case Pitch.Band:
                SubBandTempoFromValue(t, callback);
                break;
            case Pitch.High:
                SubHighTempoFromValue(t, callback);
                break;
        }
    }

    public void UnsubPitchAndTempo(Pitch pitch, float t, AudioReactiveProgress callback) {
        switch (pitch) {
        case Pitch.Low:
            UnsubLowTempoFromValue(t, callback);
            break;
        case Pitch.Band:
            UnsubBandTempoFromValue(t, callback);
            break;
        case Pitch.High:
            UnsubHighTempoFromValue(t, callback);
            break;
        }
    }



    public void SubLowTempoFromValue(float t, AudioReactiveProgress callback) {

        if (t < 1f / 7f) {
            LowTripletEigthDelegate += callback;
        }else if(t < 2f / 7f) {
            LowTripletDelegate += callback;
        }else if(t < 3f / 7f) {
            LowEigthDelegate += callback;
        }else if(t < 4f / 7f) {
            LowQuarterDelegate += callback;
        }else if(t < 5f / 7f) {
            LowHalfDelegate += callback;
        }else if(t < 6f / 7f) {
            LowWholeDelegate += callback;
        }else{
            LowTwoMeasureDelegate += callback;
        }
    }

        public void UnsubLowTempoFromValue(float t, AudioReactiveProgress callback) {

        if (t < 1f / 7f) {
            LowTripletEigthDelegate -= callback;
        }else if(t < 2f / 7f) {
            LowTripletDelegate -= callback;
        }else if(t < 3f / 7f) {
            LowEigthDelegate -= callback;
        }else if(t < 4f / 7f) {
            LowQuarterDelegate -= callback;
        }else if(t < 5f / 7f) {
            LowHalfDelegate -= callback;
        }else if(t < 6f / 7f) {
            LowWholeDelegate -= callback;
        }else{
            LowTwoMeasureDelegate -= callback;
        }
    }

    public void SubBandTempoFromValue(float t, AudioReactiveProgress callback) {

        if (t < 1f / 7f) {
            BandTripletEigthDelegate += callback;
        }else if(t < 2f / 7f) {
            BandTripletDelegate += callback;
        }else if(t < 3f / 7f) {
            BandEigthDelegate += callback;
        }else if(t < 4f / 7f) {
            BandQuarterDelegate += callback;
        }else if(t < 5f / 7f) {
            BandHalfDelegate += callback;
        }else if(t < 6f / 7f) {
            BandWholeDelegate += callback;
        }else{
            BandTwoMeasureDelegate += callback;
        }
    }

        public void UnsubBandTempoFromValue(float t, AudioReactiveProgress callback) {

        if (t < 1f / 7f) {
            BandTripletEigthDelegate -= callback;
        }else if(t < 2f / 7f) {
            BandTripletDelegate -= callback;
        }else if(t < 3f / 7f) {
            BandEigthDelegate -= callback;
        }else if(t < 4f / 7f) {
            BandQuarterDelegate -= callback;
        }else if(t < 5f / 7f) {
            BandHalfDelegate -= callback;
        }else if(t < 6f / 7f) {
            BandWholeDelegate -= callback;
        }else{
            BandTwoMeasureDelegate -= callback;
        }
    }

    public void SubHighTempoFromValue(float t, AudioReactiveProgress callback) {

        if (t < 1f / 7f) {
            HighTripletEigthDelegate += callback;
        }else if(t < 2f / 7f) {
            HighTripletDelegate += callback;
        }else if(t < 3f / 7f) {
            HighEigthDelegate += callback;
        }else if(t < 4f / 7f) {
            HighQuarterDelegate += callback;
        }else if(t < 5f / 7f) {
            HighHalfDelegate += callback;
        }else if(t < 6f / 7f) {
            HighWholeDelegate += callback;
        }else{
            HighTwoMeasureDelegate += callback;
        }
    }

        public void UnsubHighTempoFromValue(float t, AudioReactiveProgress callback) {

        if (t < 1f / 7f) {
            HighTripletEigthDelegate -= callback;
        }else if(t < 2f / 7f) {
            HighTripletDelegate -= callback;
        }else if(t < 3f / 7f) {
            HighEigthDelegate -= callback;
        }else if(t < 4f / 7f) {
            HighQuarterDelegate -= callback;
        }else if(t < 5f / 7f) {
            HighHalfDelegate -= callback;
        }else if(t < 6f / 7f) {
            HighWholeDelegate -= callback;
        }else{
            HighTwoMeasureDelegate -= callback;
        }
    }

    private IEnumerator LowTwoMeasure() {
        float t = 0;
        while (t < 1f) {
            if(LowTwoMeasureDelegate != null){
                LowTwoMeasureDelegate(t);
            }
            t += Time.deltaTime / (singleBeat * 8);
            t = Mathf.Clamp01(t);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator LowWhole() {
        float t = 0;
        while (t < 1f) {
            if(LowWholeDelegate != null){
                LowWholeDelegate(t);
            }
            t += Time.deltaTime / (singleBeat * 4);
            t = Mathf.Clamp01(t);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator LowHalf() {
        float t = 0;
        while (t < 1f) {
            if(LowHalfDelegate != null){
                LowHalfDelegate(t);
            }
            t += Time.deltaTime / (singleBeat * 2);
            t = Mathf.Clamp01(t);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator LowQuarter() {
        float t = 0;
        while (t < 1) {
            if(LowQuarterDelegate != null){
                LowQuarterDelegate(t);
            }
            t += Time.deltaTime / singleBeat;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator LowEigth() {
        float t = 0;
        while (t < 1f) {
            if(LowEigthDelegate != null){
                LowEigthDelegate(t);
            }
            t += Time.deltaTime / (singleBeat / 2f);
            t = Mathf.Clamp01(t);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator LowTriplet() {
        float t = 0;
        while (t < 1f) {
            if(LowTripletDelegate != null){
                LowTripletDelegate(t);
            }
            t += Time.deltaTime / (singleBeat / 3);
            t = Mathf.Clamp01(t);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator LowTripletEigth() {
        float t = 0;
        while (t < 1f) {
            if(LowTripletEigthDelegate != null){
                LowTripletEigthDelegate(t);
            }
            t += Time.deltaTime / (singleBeat / 6);
            t = Mathf.Clamp01(t);
            yield return new WaitForEndOfFrame();
        }
    }


    private IEnumerator BandTwoMeasure() {
        float t = 0;
        while (t < 1f) {
            if(BandTwoMeasureDelegate != null){
                BandTwoMeasureDelegate(t);
            }
            t += Time.deltaTime / (singleBeat * 8);
            t = Mathf.Clamp01(t);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator BandWhole() {
        float t = 0;
        while (t < 1f) {
            if(BandWholeDelegate != null){
                BandWholeDelegate(t);
            }
            t += Time.deltaTime / (singleBeat * 4);
            t = Mathf.Clamp01(t);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator BandHalf() {
        float t = 0;
        while (t < 1f) {
            if(BandHalfDelegate != null){
                BandHalfDelegate(t);
            }
            t += Time.deltaTime / (singleBeat * 2);
            t = Mathf.Clamp01(t);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator BandQuarter() {
        float t = 0;
        while (t < 1) {
            if(BandQuarterDelegate != null){
                BandQuarterDelegate(t);
            }
            t += Time.deltaTime / singleBeat;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator BandEigth() {
        float t = 0;
        while (t < 1f) {
            if(BandEigthDelegate != null){
                BandEigthDelegate(t);
            }
            t += Time.deltaTime / (singleBeat / 2f);
            t = Mathf.Clamp01(t);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator BandTriplet() {
        float t = 0;
        while (t < 1f) {
            if(BandTripletDelegate != null){
                BandTripletDelegate(t);
            }
            t += Time.deltaTime / (singleBeat / 3);
            t = Mathf.Clamp01(t);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator BandTripletEigth() {
        float t = 0;
        while (t < 1f) {
            if(BandTripletEigthDelegate != null){
                BandTripletEigthDelegate(t);
            }
            t += Time.deltaTime / (singleBeat / 6);
            t = Mathf.Clamp01(t);
            yield return new WaitForEndOfFrame();
        }
    }


    private IEnumerator HighTwoMeasure() {
        float t = 0;
        while (t < 1f) {
            if(HighTwoMeasureDelegate != null){
                HighTwoMeasureDelegate(t);
            }
            t += Time.deltaTime / (singleBeat * 8);
            t = Mathf.Clamp01(t);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator HighWhole() {
        float t = 0;
        while (t < 1f) {
            if(HighWholeDelegate != null){
                HighWholeDelegate(t);
            }
            t += Time.deltaTime / (singleBeat * 4);
            t = Mathf.Clamp01(t);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator HighHalf() {
        float t = 0;
        while (t < 1f) {
            if(HighHalfDelegate != null){
                HighHalfDelegate(t);
            }
            t += Time.deltaTime / (singleBeat * 2);
            t = Mathf.Clamp01(t);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator HighQuarter() {
        float t = 0;
        while (t < 1) {
            if(HighQuarterDelegate != null){
                HighQuarterDelegate(t);
            }
            t += Time.deltaTime / singleBeat;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator HighEigth() {
        float t = 0;
        while (t < 1f) {
            if(HighEigthDelegate != null){
                HighEigthDelegate(t);
            }
            t += Time.deltaTime / (singleBeat / 2f);
            t = Mathf.Clamp01(t);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator HighTriplet() {
        float t = 0;
        while (t < 1f) {
            if(HighTripletDelegate != null){
                HighTripletDelegate(t);
            }
            t += Time.deltaTime / (singleBeat / 3);
            t = Mathf.Clamp01(t);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator HighTripletEigth() {
        float t = 0;
        while (t < 1f) {
            if(HighTripletEigthDelegate != null){
                HighTripletEigthDelegate(t);
            }
            t += Time.deltaTime / (singleBeat / 6);
            t = Mathf.Clamp01(t);
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
    void Hoge(float t){
        if(t > 0.1){return;}
        Debug.Log("hoge");
    }

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
        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat*8) < Time.deltaTime && timing - lowTwoMeasureTiming > safety) {
            lowTwoMeasureTiming = timing;
            StartCoroutine(LowTwoMeasure());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat * 4) < Time.deltaTime && timing - lowWholeTiming > safety) {
            lowWholeTiming = timing;
            StartCoroutine(LowWhole());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat * 2) < Time.deltaTime && timing - lowHalfTiming > safety) {
            lowHalfTiming = timing;
            StartCoroutine(LowHalf());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % singleBeat < Time.deltaTime &&  timing - lowQuarterTiming > safety) {
            lowQuarterTiming = timing;
            StartCoroutine(LowQuarter());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat/2) < Time.deltaTime && timing - lowEigthTiming > safety) {
            lowEigthTiming = timing;
            StartCoroutine(LowEigth());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat / 3) < Time.deltaTime && timing - lowTripletTiming > safety) {
            lowTripletTiming = timing;
            StartCoroutine(LowTriplet());
        }

        if (AudioReactive.I.RmsLow > lowThresh && timing % (singleBeat / 6) < Time.deltaTime && timing - lowTripEigthTiming > safety) {
            lowTripEigthTiming = timing;
            StartCoroutine(LowTripletEigth());
        }

        // band pass
        if (AudioReactive.I.RmsBand > bandThresh && timing % (singleBeat*8) < Time.deltaTime && timing - bandTwoMeasureTiming > safety) {
            bandTwoMeasureTiming = timing;
            StartCoroutine(BandTwoMeasure());
        }

        if (AudioReactive.I.RmsBand > bandThresh && timing % (singleBeat * 4) < Time.deltaTime && timing - bandWholeTiming > safety) {
            bandWholeTiming = timing;
            StartCoroutine(BandWhole());
        }

        if (AudioReactive.I.RmsBand > bandThresh && timing % (singleBeat * 2) < Time.deltaTime && timing - bandHalfTiming > safety) {
            bandHalfTiming = timing;
            StartCoroutine(BandHalf());
        }

        if (AudioReactive.I.RmsBand > bandThresh && timing % singleBeat < Time.deltaTime &&  timing - bandQuarterTiming > safety) {
            bandQuarterTiming = timing;
            StartCoroutine(BandQuarter());
        }

        if (AudioReactive.I.RmsBand > bandThresh && timing % (singleBeat/2) < Time.deltaTime && timing - bandEigthTiming > safety) {
            bandEigthTiming = timing;
            StartCoroutine(BandEigth());
        }

        if (AudioReactive.I.RmsBand > bandThresh && timing % (singleBeat / 3) < Time.deltaTime && timing - bandTripletTiming > safety) {
            bandTripletTiming = timing;
            StartCoroutine(BandTriplet());
        }

        if (AudioReactive.I.RmsBand > bandThresh && timing % (singleBeat / 6) < Time.deltaTime && timing - bandTripEigthTiming > safety) {
            bandTripEigthTiming = timing;
            StartCoroutine(BandTripletEigth());
        }

        // high pass
        if (AudioReactive.I.RmsHigh > highThresh && timing % (singleBeat*8) < Time.deltaTime && timing - highTwoMeasureTiming > safety) {
            highTwoMeasureTiming = timing;
            StartCoroutine(HighTwoMeasure());
        }

        if (AudioReactive.I.RmsHigh > highThresh && timing % (singleBeat * 4) < Time.deltaTime && timing - highWholeTiming > safety) {
            highWholeTiming = timing;
            StartCoroutine(HighWhole());
        }

        if (AudioReactive.I.RmsHigh > highThresh && timing % (singleBeat * 2) < Time.deltaTime && timing - highHalfTiming > safety) {
            highHalfTiming = timing;
            StartCoroutine(HighHalf());
        }

        if (AudioReactive.I.RmsHigh > highThresh && timing % singleBeat < Time.deltaTime &&  timing - highQuarterTiming > safety) {
            highQuarterTiming = timing;
            StartCoroutine(HighQuarter());
        }

        if (AudioReactive.I.RmsHigh > highThresh && timing % (singleBeat/2) < Time.deltaTime && timing - highEigthTiming > safety) {
            highEigthTiming = timing;
            StartCoroutine(HighEigth());
        }

        if (AudioReactive.I.RmsHigh > highThresh && timing % (singleBeat / 3) < Time.deltaTime && timing - highTripletTiming > safety) {
            highTripletTiming = timing;
            StartCoroutine(HighTriplet());
        }

        if (AudioReactive.I.RmsHigh > highThresh && timing % (singleBeat / 6) < Time.deltaTime && timing - highTripEigthTiming > safety) {
            highTripEigthTiming = timing;
            StartCoroutine(HighTripletEigth());
        }


    }
}
