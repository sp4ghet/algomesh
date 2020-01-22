using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using Klak.Math;

public class CameraControl : MonoBehaviour
{
    public enum CameraMode : int{
        focusCenter = 0,
        manualCycle = 1,
        tempoCycle = 2
    }

    [Serializable]
    public class CameraPosition{
        public bool lookAtFocus;
        public bool shouldFollow;
        public bool randomPos;
        public float randomRadius;
        public List<Vector3> positions;
        public Transform focus;
    }

    #region Editable Fields
    [SerializeField]
    Camera _cam;

    [SerializeField]
    bool m_enablePos;

    [SerializeField]
    List<CameraPosition> m_motifs;
    
    [Space]

    [SerializeField, Range(0f, 2f)]
    float m_posSmoothTime;

    [SerializeField]
    bool m_enableBrownian;
    #endregion

    #region Private Fields
    float[] m_brownianTime = new float[3];

    Vector3 m_initialPos;

    Vector3 m_prevPos;
    Vector3 m_posVelocity = Vector3.zero;
    Vector3 m_targetPos;

    int m_currentIndex;
    CameraPosition m_currentMotif;
    CameraPosition m_origin;
    #endregion

    #region Properties
    

    public Vector3 TargetPos {
        get => m_targetPos;
        set {
            m_posVelocity = Vector3.zero;
            m_prevPos = transform.position;
            m_targetPos = value;
        }
    }

    public int MotifCount {
        get {
            return Motifs.Count;
        }
    }

    public Camera Cam {
        get => _cam;
        set => _cam = value;
    }
    public List<CameraPosition> Motifs { get => m_motifs; set => m_motifs = value; }
    public int CurrentIndex { get => m_currentIndex; set => m_currentIndex = value; }

    #endregion

    #region Public Methods
    public void CycleMotif(int idx) {
        if (m_currentMotif == null) {
            m_currentMotif = m_origin;
            return;
        }
        if (idx > Motifs.Count) { idx = idx % Motifs.Count; }
        m_currentMotif = Motifs[idx];
        m_currentIndex = idx;
        CyclePositions();
    }

    public void CyclePositions() {
        //assumes m_currentMotif is set
        if (m_currentMotif.randomPos) {
            var randomRotation = Quaternion.Euler(
                UnityEngine.Random.Range(-90f, 90f),
                UnityEngine.Random.Range(-90f, 90f),
                UnityEngine.Random.Range(-90f, 90f));
            var target = randomRotation * TargetPos.normalized * UnityEngine.Random.Range(2f, m_currentMotif.randomRadius);
            target = new Vector3(target.x, Mathf.Max(target.y, -1f), target.z);
            TargetPos = target;
            return;
        }
        int idx = m_currentMotif.positions.FindIndex(x => x == TargetPos);
        TargetPos = m_currentMotif.positions[(idx + 1) % m_currentMotif.positions.Count];
    }
    #endregion

    #region Private Methods

    private Vector3 BrownianPosition(float frequency, int fractalOctave, float amplitude, Vector3 scale) {
        var dt = Time.deltaTime;
        for (var i = 0; i < 3; i++)
            m_brownianTime[i] += frequency * dt;

        var n = new Vector3(
            Perlin.Fbm(m_brownianTime[0], fractalOctave),
            Perlin.Fbm(m_brownianTime[1], fractalOctave),
            Perlin.Fbm(m_brownianTime[2], fractalOctave));

        n = Vector3.Scale(n, scale);
        n *= amplitude * (1 / 0.75f);
        return n;
    }

    #endregion

    #region MonoBehaviours
    // Start is called before the first frame update
    void Start()
    {
        m_prevPos = _cam.transform.position;
        m_initialPos = _cam.transform.position;
        TargetPos = _cam.transform.position;
        for (int i=0; i < m_brownianTime.Length; i++) {
            m_brownianTime[i] = UnityEngine.Random.Range(-10000.0f, 0.0f);
        }

        m_origin = Motifs[0];
        m_origin.positions = new List<Vector3>(new Vector3[] { _cam.transform.position, new Vector3(0, 0, -7) });
        m_currentMotif = m_origin;

        CycleMotif(0);
    }



    // Update is called once per frame
    void Update()
    {
        Vector3 gotoTarget = TargetPos;

        if (m_currentMotif.shouldFollow) {
            gotoTarget = m_currentMotif.focus.position + TargetPos; 
        }

        if (m_enableBrownian) {
            gotoTarget += BrownianPosition(0.2f, 3, 0.5f, Vector3.one);
        }

        if (m_enablePos) {
            _cam.transform.position = Vector3.SmoothDamp(_cam.transform.position, gotoTarget, ref m_posVelocity, m_posSmoothTime);
        }

        if (m_currentMotif.lookAtFocus) {
            _cam.transform.rotation = Quaternion.LookRotation(m_currentMotif.focus.position - _cam.transform.position);
        }
        else {
            _cam.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        }

    }
    #endregion
}
