using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
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
        public List<Vector3> positions;
        public Transform focus;
    }

    #region Editable Fields
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
            return m_motifs.Count;
        }
    }
    #endregion

    #region Public Methods
    public void CycleMotif(int idx) {
        if (m_currentMotif == null) {
            m_currentMotif = m_origin;
            return;
        }
        if (idx > m_motifs.Count) { idx = idx % m_motifs.Count; }
        m_currentMotif = m_motifs[idx];
    }

    public void CyclePositions() {
        //assumes m_currentMotif is set
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
        m_prevPos = transform.position;
        m_initialPos = transform.position;
        TargetPos = transform.position;
        for (int i=0; i < m_brownianTime.Length; i++) {
            m_brownianTime[i] = UnityEngine.Random.Range(-10000.0f, 0.0f);
        }

        m_origin = new CameraPosition();
        m_origin.focus = GameObject.Find("Origin").transform;
        m_origin.lookAtFocus = true;
        m_origin.positions = new List<Vector3>(new Vector3[] { transform.position, new Vector3(0, 0, -7) });
        m_currentMotif = m_origin;

        m_motifs.Insert(0, m_origin);

        CycleMotif(0);
    }



    // Update is called once per frame
    void Update()
    {
        Vector3 target = TargetPos;

        if (m_currentMotif.shouldFollow) {
            target = m_currentMotif.focus.position + TargetPos; 
        }

        if (m_enableBrownian) {
            target += BrownianPosition(0.2f, 3, 0.5f, Vector3.one);
        }

        if (m_enablePos) {
            transform.position = Vector3.SmoothDamp(transform.position, target, ref m_posVelocity, m_posSmoothTime);
        }
        else {
            m_currentMotif = m_origin;
        }

        if (m_currentMotif.lookAtFocus) {
            transform.rotation = Quaternion.LookRotation(m_currentMotif.focus.position - transform.position);
        }
        else {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        }

    }
    #endregion
}
