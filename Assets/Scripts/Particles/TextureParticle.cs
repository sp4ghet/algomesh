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
    List<Copy.VFX.Utils.PointCacheAsset> m_pCaches;

    [SerializeField]
    Vector2 m_aspectRatio;

    [SerializeField]
    Mesh m_mesh;

    [SerializeField]
    Material m_mat;

    [SerializeField]
    AnimationCurve m_spawnCurve;

    [SerializeField]
    int m_maxSpawnCount;

    [SerializeField]
    float m_Scale;

    [SerializeField]
    private float m_lifetime = 1.5f;

    int currentIndex = 0;
    Copy.VFX.Utils.PointCacheAsset m_pCache;

    float m_curlIntensity;
    float m_forwardIntensity;
    float m_explodeIntensity;

    ComputeBuffer m_posBuffer;
    ComputeBuffer m_colorBuffer;
    ComputeBuffer m_aliveBuffer;
    int[] aliveBufferArray = new int[] { 0 };

    uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
    ComputeBuffer m_argsBuffer;

    bool m_useColor;

    Texture2D m_defaultColorTex;

    int Alive { get => aliveBufferArray[0]; set => aliveBufferArray[0] = value; }
    public float CurlIntensity { get => m_curlIntensity; set => m_curlIntensity = value; }
    public float ForwardIntensity { get => m_forwardIntensity; set => m_forwardIntensity = value; }
    public float ExplodeIntensity { get => m_explodeIntensity; set => m_explodeIntensity = value; }
    public float Lifetime { get => m_lifetime; set => m_lifetime = value; }

    public void SetPCache(float t) {
        currentIndex = Mathf.FloorToInt(t * m_pCaches.Count) % m_pCaches.Count;
        m_pCache = m_pCaches[currentIndex];
    }

    public void CyclePCache() {
        currentIndex = (currentIndex + 1) % m_pCaches.Count;
        m_pCache = m_pCaches[currentIndex];
    }

    
    public void Curl(float t) {
        CurlIntensity = 1f-t;
    }

    void InitBuffer() {
        m_particleCount = m_particleCount > m_pCache.PointCount ? m_pCache.PointCount : m_particleCount;
        m_particleCount = m_particleCount - m_particleCount%BLOCK_SIZE;
        m_posBuffer = new ComputeBuffer(m_particleCount, Marshal.SizeOf(typeof(Vector4)));
        m_colorBuffer = new ComputeBuffer(m_particleCount, Marshal.SizeOf(typeof(Vector4)));
        m_argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint),
                ComputeBufferType.IndirectArguments);
        m_aliveBuffer = new ComputeBuffer(1, Marshal.SizeOf(typeof(int)));
    }

    void ReleaseBuffer() {
        m_posBuffer?.Release();
        m_posBuffer = null;
        m_argsBuffer?.Release();
        m_argsBuffer = null;
        m_colorBuffer?.Release();
        m_colorBuffer = null;
        m_aliveBuffer?.Release();
        m_aliveBuffer = null;
    }

    
    private void SpawnParticles(float lifeCycleTime) {
        int threadGroupSize = m_particleCount / BLOCK_SIZE;
        int spawnCount = (int)(m_maxSpawnCount * m_spawnCurve.Evaluate(lifeCycleTime));
        //int spawnCount = 500;

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
        cs.SetFloat("_AspectRatio", m_aspectRatio.x / m_aspectRatio.y);

        cs.SetBuffer(kernelPos, "_ParticleColorBuffer", m_colorBuffer);
        cs.SetInt("_PCacheTexWidth", tex.width);
        cs.SetBuffer(kernelPos, "_ParticlePosBuffer", m_posBuffer);

        cs.SetFloat("_LifeTime", m_lifetime);
        cs.SetInt("_ParticleCount", m_particleCount);
        cs.SetInt("_AliveCount", Alive);
        cs.SetBuffer(kernelPos, "_AliveCounter", m_aliveBuffer);
        cs.SetInt("_SpawnRate", spawnCount);

        cs.Dispatch(kernelPos, threadGroupSize, 1, 1);
        // this needs to be after the simulation
        m_aliveBuffer.GetData(aliveBufferArray);
        Alive = Mathf.Min(m_particleCount, aliveBufferArray[0]);
        Alive = Mathf.Max(0, Alive);
        m_aliveBuffer.SetData(aliveBufferArray);
    }

    void Run(float lifeCycleTime) {
        int threadGroupSize = m_particleCount / BLOCK_SIZE;

        var cs = m_textureParticleShader;
        int kernelPos = cs.FindKernel("ParticlePos");
        cs.SetBuffer(kernelPos, "_ParticlePosBuffer", m_posBuffer);

        cs.SetBuffer(kernelPos, "_AliveCounter", m_aliveBuffer);
        cs.SetInt("_AliveCount", Alive);
        cs.SetFloat("_Time", Time.time);
        cs.SetFloat("_DeltaTime", Time.deltaTime);
        cs.SetFloat("_LifeTime", m_lifetime);

        cs.SetFloat("_CurlIntensity", m_curlIntensity);
        cs.SetFloat("_ForwardIntensity", m_forwardIntensity);
        cs.SetFloat("_ExploreIntensity", m_explodeIntensity);

        cs.Dispatch(kernelPos, threadGroupSize, 1, 1);
    }

    void Render() {
        uint numIndices = (m_mesh != null) ?
            (uint)m_mesh.GetIndexCount(0) : 0;
        args[0] = numIndices;
        args[1] = (uint)Alive+1;
        m_argsBuffer.SetData(args);

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

    void Loop(float t) {
        SpawnParticles(t);
        Run(t);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_pCache = m_pCaches[0];
        m_defaultColorTex = new Texture2D(1, 1);
        InitBuffer();
        SpawnParticles(0);
    }

    // Update is called once per frame
    void Update()
    {
        float lifecycleTime = (Time.time % m_lifetime) / m_lifetime;
        Loop(lifecycleTime);
        Render();
    }

    private void OnDisable() {
        ReleaseBuffer();
    }
}
