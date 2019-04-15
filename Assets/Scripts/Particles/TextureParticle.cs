using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TextureParticle : MonoBehaviour
{
    const int BLOCK_SIZE = 256;

    [SerializeField, Range(BLOCK_SIZE, BLOCK_SIZE*BLOCK_SIZE)]
    int m_particleCount;

    [SerializeField]
    ComputeShader m_textureParticleShader;

    [SerializeField]
    UnityEditor.VFX.Utils.PointCacheAsset m_pCache;

    [SerializeField]
    Vector2 m_aspectRatio;

    [SerializeField]
    Mesh m_mesh;

    [SerializeField]
    Material m_mat;

    [SerializeField]
    AnimationCurve m_spawnCurve;

    [SerializeField]
    float m_Scale;

    ComputeBuffer m_posBuffer;
    ComputeBuffer m_colorBuffer;

    uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
    ComputeBuffer m_argsBuffer;

    bool m_useColor;
    private float m_lifetime = 20f;
    float m_bpm = 120;

    Texture2D m_defaultColorTex;

    public float Bpm {
        get => m_bpm;
        set {
            m_lifetime = 60f / (value/4);
            m_bpm = value;
            }
        }

    void InitBuffer() {
        m_particleCount = m_particleCount > m_pCache.PointCount ? m_pCache.PointCount : m_particleCount;
        m_particleCount = m_particleCount - m_particleCount%BLOCK_SIZE;
        m_posBuffer = new ComputeBuffer(m_particleCount, Marshal.SizeOf(typeof(Vector4)));
        m_colorBuffer = new ComputeBuffer(m_particleCount, Marshal.SizeOf(typeof(Vector4)));
        m_argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint),
                ComputeBufferType.IndirectArguments);
    }

    void ReleaseBuffer() {
        m_posBuffer?.Release();
        m_posBuffer = null;
        m_argsBuffer?.Release();
        m_argsBuffer = null;
        m_colorBuffer?.Release();
        m_colorBuffer = null;
    }

    
    private void SpawnParticles(float lifeCycleTime) {
        int threadGroupSize = m_particleCount / BLOCK_SIZE;

        var tex = m_pCache.surfaces[0];
        Texture2D tex2 = m_defaultColorTex;
        if (m_pCache.surfaces.Length > 1) {
            tex2 = m_pCache.surfaces[1];
        }
        m_useColor = m_pCache.surfaces.Length > 1;

        var cs = m_textureParticleShader;
        int kernelPos = cs.FindKernel("SpawnParticles");
        cs.SetTexture(kernelPos, "SpawnTex", tex);

        cs.SetBool("_UseColor", m_useColor);
        cs.SetTexture(kernelPos, "SpawnColor", tex2);
        cs.SetBuffer(kernelPos, "_ParticleColorBuffer", m_colorBuffer);
        cs.SetInt("_PCacheTexWidth", tex.width);
        cs.SetFloat("_AspectRatio", m_aspectRatio.x / m_aspectRatio.y);
        cs.SetBuffer(kernelPos, "_ParticlePosBuffer", m_posBuffer);

        cs.SetInt("_ParticleCount", m_particleCount);
        cs.SetFloat("_SpawnRate", m_spawnCurve.Evaluate(lifeCycleTime / m_lifetime));

        cs.SetFloat("_Scale", transform.localScale.x);

        cs.Dispatch(kernelPos, threadGroupSize, 1, 1);
    }

    void Run(float lifeCycleTime) {
        int threadGroupSize = m_particleCount / BLOCK_SIZE;

        var cs = m_textureParticleShader;
        int kernelPos = cs.FindKernel("ParticlePos");
        cs.SetBuffer(kernelPos, "_ParticlePosBuffer", m_posBuffer);

        cs.SetFloat("_Time", Time.time);
        cs.SetFloat("_DeltaTime", Time.deltaTime);
        cs.SetFloat("_Lifetime", m_lifetime);
        cs.SetVector("_Scale", transform.localScale);
        cs.Dispatch(kernelPos, threadGroupSize, 1, 1);
    }

    void Render() {
        // 指定したメッシュのインデックス数を取得
        uint numIndices = (m_mesh != null) ?
            (uint)m_mesh.GetIndexCount(0) : 0;
        args[0] = numIndices; // メッシュのインデックス数をセット
        args[1] = (uint)m_particleCount; // インスタンス数をセット
        m_argsBuffer.SetData(args); // バッファにセット

        m_mat.SetBuffer("_ParticlePosBuffer", m_posBuffer);
        m_mat.SetBuffer("_ParticleColorBuffer", m_colorBuffer);
        m_mat.SetInt("_UseColor", m_useColor ? 1 : 0);
        m_mat.SetMatrix("_ObjectToWorld", transform.localToWorldMatrix);
        m_mat.SetMatrix("_WorldToObject", transform.worldToLocalMatrix);
        m_mat.SetFloat("_ObjectScale", m_Scale);
        Graphics.DrawMeshInstancedIndirect(
                m_mesh,
                0,
                m_mat,
                new Bounds(Vector3.zero, Vector3.one * 100f),
                m_argsBuffer
            );
    }

    // Start is called before the first frame update
    void Start()
    {
        m_defaultColorTex = new Texture2D(1, 1);
        InitBuffer();
        SpawnParticles(0);
    }

    // Update is called once per frame
    void Update()
    {
        float lifecycleTime = Time.time % m_lifetime;
        SpawnParticles(lifecycleTime);
        Run(lifecycleTime);
        Render();
    }

    private void OnDisable() {
        ReleaseBuffer();
    }
}
