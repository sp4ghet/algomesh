using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioReactive : MonoBehaviour {

    public static AudioReactive I;

    [SerializeField]
    FFTWindow window = FFTWindow.BlackmanHarris;

    private AudioSource audioSource;

    const int sampleCount = 1024;
    float[] samples;
    float[] pastAmplifier = new float[256];
    int currentIndex = 0;

    Texture2D spectrum;
    Color[] colors;

    Lasp.FilterType highPass = Lasp.FilterType.HighPass;
    Lasp.FilterType bandPass = Lasp.FilterType.BandPass;
    Lasp.FilterType lowPass  = Lasp.FilterType.LowPass;
    Lasp.FilterType byPass   = Lasp.FilterType.Bypass;

    const float kSilence = -40; // -40 dBFS = silence

    float[] _highWave, _bandWave, _lowWave, _byWave;
    const int waveSize = 512;

    float peakHigh, peakBand, peakLow, peakBy;
    float rmsHigh, rmsBand, rmsLow, rmsBy;

    #region properties

    public static int WaveSize => waveSize;
    public static float KSilence => kSilence;

    public float PeakHigh {
        get {
            return peakHigh;
        }
    }
    public float PeakBand {
        get {
            return peakBand;
        }
    }
    public float PeakLow {
        get {
            return peakLow;
        }
    }
    public float PeakBy {
        get {
            return peakBy;
        }
    }

    public float RmsHigh {
        get {
            return rmsHigh;
        }
    }
    public float RmsBand {
        get {
            return rmsBand;
        }
    }
    public float RmsLow {
        get {
            return rmsLow;
        }
    }
    public float RmsBy {
        get {
            return rmsBy;
        }
    }

    public float[] Samples { get => samples; }
    public Texture2D Spectrum { get => spectrum;}

    #endregion

    #region Methods
    
    private float WeightedAverage(float currentMax, float currentAverage) {
        currentIndex = (currentIndex+1) % pastAmplifier.Length;
        pastAmplifier[currentIndex] = (currentMax + currentAverage) * 0.5f;
        if (Time.frameCount < pastAmplifier.Length) {
            return pastAmplifier[currentIndex];
        }

        float weight = 2f/pastAmplifier.Length;
        float weightedAverage = 0;
        float weightSum = 0;
        for (int i=0; i < pastAmplifier.Length; i++) {
            // move down from current index until 0 and loopback to last index
            int index = currentIndex >= i ? currentIndex - i : pastAmplifier.Length + currentIndex - i;
            weight = -(2f * i) / (pastAmplifier.Length * pastAmplifier.Length) + (2f / pastAmplifier.Length);
            weightSum += weight;
            weightedAverage += pastAmplifier[index] * weight;
        }
        return weightedAverage;
    }

    #endregion

    #region MonoBehaviour

    private void OnEnable() {
        if (I == null) {
            I = this;
        }

        _highWave = new float[WaveSize];
        _bandWave = new float[WaveSize];
        _lowWave = new float[WaveSize];
        _byWave = new float[WaveSize];
        samples = new float[sampleCount];
        int pixels = Mathf.FloorToInt(sampleCount / Mathf.Exp(1));
        spectrum = new Texture2D(pixels, 1, TextureFormat.RFloat, false, true);
        spectrum.hideFlags = HideFlags.HideAndDontSave;
        colors = new Color[pixels];

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(null, true, 100, 44100);
        audioSource.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) { }
        Debug.Log("start playing... position is " + Microphone.GetPosition(null));
        audioSource.Play();
        
        audioSource.GetSpectrumData(samples, 0, window);
    }

    // Use this for initialization
    void Start () {
       
    }
    
    // Update is called once per frame
    void Update () {
        peakHigh = Lasp.AudioInput.GetPeakLevelDecibel(highPass) - kSilence;
        peakBand = Lasp.AudioInput.GetPeakLevelDecibel(bandPass) - kSilence;
        peakLow = Lasp.AudioInput.GetPeakLevelDecibel(lowPass) - kSilence;
        peakBy = Lasp.AudioInput.GetPeakLevelDecibel(byPass) - kSilence;

        rmsHigh = Lasp.AudioInput.CalculateRMSDecibel(highPass) - kSilence;
        rmsBand = Lasp.AudioInput.CalculateRMSDecibel(bandPass) - kSilence;
        rmsLow = Lasp.AudioInput.CalculateRMSDecibel(lowPass) - kSilence;
        rmsBy = Lasp.AudioInput.CalculateRMSDecibel(byPass) - kSilence;

        Lasp.AudioInput.RetrieveWaveform(highPass, _highWave);
        Lasp.AudioInput.RetrieveWaveform(bandPass, _bandWave);
        Lasp.AudioInput.RetrieveWaveform(lowPass, _lowWave);
        Lasp.AudioInput.RetrieveWaveform(byPass, _byWave);

        _highWave = _highWave.Select(x => x - kSilence).ToArray();
        _bandWave = _bandWave.Select(x => x - kSilence).ToArray();
        _lowWave = _lowWave.Select(x => x - kSilence).ToArray();
        _byWave = _byWave.Select(x => x - kSilence).ToArray();

        audioSource.GetSpectrumData(samples, 0, window);
        
        for(int j=0; j < colors.Length; j++) {
            colors[j] = Color.black;
        }
        
        for(int i = 0; i < samples.Length; i++) {
            var x = samples[i];
            float ratio = Mathf.Pow((float)i / sampleCount , 0.5f);
            int index = Mathf.FloorToInt( Mathf.Abs(ratio * sampleCount - 2) / Mathf.Exp(1));
            if(i < sampleCount / 10f) {
                colors[i] += Color.white * x;
            }
            if (index == 0) {continue;}
            colors[index] += Color.white * x;
        }
        var magnitudes = colors.Select(x => new Vector3(x.r, x.g, x.b).magnitude);
        float ampWeightedAve = WeightedAverage(magnitudes.Max(), magnitudes.Sum() / colors.Length);
        
        spectrum.SetPixels(colors.Select(x => x/ampWeightedAve).ToArray());
        spectrum.Apply();
    }
    #endregion
}
