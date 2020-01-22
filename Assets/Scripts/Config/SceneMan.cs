using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMan : MonoBehaviour
{
    public static SceneMan I;

    [SerializeField]
    GlobalState _state;

    Scene _main;
    Scene _logo;
    Scene _boids;
    Scene _current;
    ISceneController _currentSceneController;
    List<GameObject> _currentObjects = new List<GameObject>();

    public enum SceneId : int{
        main = 0,
        logo = 1,
        boids = 2,
    }


    #region Public Methods
    public void SetActiveScene(int sceneId) {
        SceneId id = (SceneId)sceneId;
        switch (id) {
        case SceneId.main:
            SetActiveScene(_main);
            break;
        case SceneId.logo:
            SetActiveScene(_logo);
            break;
        case SceneId.boids:
            SetActiveScene(_boids);
            break;
        }
    }

    float prevBpm = 0f;
    public void BpmTap() {
        float diff = Mathf.Abs(Time.time - prevBpm);
        prevBpm = Time.time;
        if (diff > 4f) { return; }

        SetBpm(Mathf.RoundToInt(60f / diff));
    }

    public void SetBpm(int bpm) {
        _state.Bpm = bpm;
        AudioReactiveManager.I.Bpm = _state.Bpm;
    }

    public void SetMainSpeed(float t) {
        if(_current.name == _main.name) {
            var _mainSc = (MainSceneController)_currentSceneController;
            _mainSc.SetSpeed(t);
        }
    }

    public void CameraNext() {
        _currentSceneController.CameraNext();
    }

    public void MainTexturePop() {
        if (_current.name == _main.name) {
            var _mainSc = (MainSceneController)_currentSceneController;
            _mainSc.TexturePop();
        }
    }

    #endregion

    #region Private Methods
    bool IsSceneController(GameObject _candidate) {
        var sc = _candidate.GetComponent<ISceneController>();
        return sc != null;
    }

    void SetActiveScene(Scene scene) {
        _currentSceneController?.Deactivate();
        _current = scene;

        SceneManager.SetActiveScene(scene);
        scene.GetRootGameObjects(_currentObjects);
        _currentSceneController = _currentObjects.Find(IsSceneController).GetComponent<ISceneController>();
        _currentSceneController.Activate();
    }



    void InitializeScene(Scene scene) {
        var gameObjects = new List<GameObject>();
        scene.GetRootGameObjects(gameObjects);
        var sc = gameObjects.Find(IsSceneController).GetComponent<ISceneController>();
        sc.Init(_state);
        sc.Deactivate();
    }

    void Loaded(Scene scene, LoadSceneMode mode) {
        InitializeScene(scene);

        switch (scene.name) {
        case "main":
            _main = scene;
            SetActiveScene(scene);
            break;
        case "logo":
            _logo = scene;
            break;
        case "boids":
            _boids = scene;
            
            break;
        }
    }

    IEnumerator WaitOneFrame(Scene scene) {
        while(!scene.isLoaded) yield return null;
        Loaded(scene, LoadSceneMode.Additive);
    }

    void LoadedCoroutine(Scene scene) {
        StartCoroutine(WaitOneFrame(scene));
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if(I != null && I != this) {
            Destroy(gameObject);
            return;
        }
        I = this;
        GlobalState.I = _state;
        AudioReactiveManager.I.Bpm = _state.Bpm;

        _main = SceneManager.GetSceneByName("main");
        if(!_main.IsValid()) {
            SceneManager.LoadScene("main", LoadSceneMode.Additive);
        }
        else {
            LoadedCoroutine(_main);
        }

        _logo = SceneManager.GetSceneByName("logo");
        if(!_logo.IsValid()) {
            SceneManager.LoadScene("logo", LoadSceneMode.Additive);
        }
        else {
            LoadedCoroutine(_logo);
        }
        _boids = SceneManager.GetSceneByName("boids");
        if (!_boids.IsValid()) {
            SceneManager.LoadScene("boids", LoadSceneMode.Additive);
        }
        else {
            LoadedCoroutine(_boids);
        }

        SceneManager.sceneLoaded += Loaded;

        // displays
        Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON.
        // Check if additional displays are available and activate each.
        if (Display.displays.Length > 1) {
            Display.displays[1].Activate();
        }
        Display.displays[0].SetRenderingResolution(1920, 1080);
        
        //if (Display.displays.Length > 2) {
        //    Display.displays[2].Activate();
        //}

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
