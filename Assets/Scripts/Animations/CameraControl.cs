using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Klak.Math;

public class CameraControl : MonoBehaviour
{
    public enum CameraMode : int{
        focusCenter = 0,
        manualCycle = 1,
        tempoCycle = 2
    }

    [Serializeable]
    public class CameraPosition{
        bool lookAtFocus;
        List<Vector3> positions;
        Transform focus;
    }

    #region Editable Fields
    [SerializeField]
    bool m_enablePos;

    [SerializeField]
    Vector3 m_minRandomPosition;

    [SerializeField]
    Vector3 m_maxRandomPosition;
    
    [Space]

    [SerializeField, Range(0f, 2f)]
    float m_posSmoothTime;

    [SerializeField]
    List<Vector3> cameraPositions;

    [SerializeField]
    List<Transform> lookAtTargets;

    [SerializeField]
    bool m_enableBrownian;
    #endregion

    #region Private Fields
    float[] m_brownianTime = new float[3];

    Vector3 m_initialPos;

    Vector3 m_prevPos;
    Vector3 m_posVelocity = Vector3.zero;
    Vector3 m_targetPos;
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
    #endregion

    #region Methods
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

    void Randomize() {
        float x = Random.Range(m_minRandomPosition.x, m_maxRandomPosition.x);
        float y = Random.Range(m_minRandomPosition.y, m_maxRandomPosition.y);
        float z = Random.Range(m_minRandomPosition.z, m_maxRandomPosition.z);
        TargetPos = new Vector3(x, y, z);
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
            m_brownianTime[i] = Random.Range(-10000.0f, 0.0f);
        }
    }



    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % 1000 == 0) {
            Randomize();
        }

        Vector3 target = TargetPos;


        if (m_enableBrownian) {
            target = TargetPos + BrownianPosition(0.2f, 3, 0.5f, Vector3.one);
        }

        if (m_enablePos) {
            transform.position = Vector3.SmoothDamp(transform.position, target, ref m_posVelocity, m_posSmoothTime);
        }

        transform.rotation = Quaternion.LookRotation(lookAtTarget.position - transform.position);
    }
    #endregion
}
