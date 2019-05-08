using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;


public class RaymarchingRenderer : MonoBehaviour
{
    Dictionary<Camera, CommandBuffer> cameras_ = new Dictionary<Camera, CommandBuffer>();
    Mesh quad_;

    [SerializeField] Material material = null;
    [SerializeField] CameraEvent pass = CameraEvent.BeforeGBuffer;

    private float bpm = 120;
    private Color mainColor = Color.white;
    [SerializeField]
    private Texture2D spectrum;

    private float floorSens = 0f;
    private float gridSize = 0f;
    private float speed = 0f;

    float progress;
    float rotationAngle;

    public float Bpm { get => bpm; set => bpm = value; }
    public Color MainColor { get => mainColor; set => mainColor = value; }
    public Texture2D Spectrum { get => spectrum; set => spectrum = value; }
    public float AudioReactiveSensitivity { get => floorSens; set => floorSens = value; }
    public float GridSize { get => gridSize; set => gridSize = value; }
    public float Speed { get => speed; set => speed = value; }

    Mesh GenerateQuad()
    {
        //simple quad mesh
        var mesh = new Mesh();
        mesh.vertices = new Vector3[4] {
            new Vector3( 1.0f , 1.0f,  0.0f),
            new Vector3(-1.0f , 1.0f,  0.0f),
            new Vector3(-1.0f ,-1.0f,  0.0f),
            new Vector3( 1.0f ,-1.0f,  0.0f),
        };
        mesh.triangles = new int[6] { 0, 1, 2, 2, 3, 0 };
        return mesh;
    }

    void CleanUp()
    {
        foreach (var pair in cameras_) {
            var camera = pair.Key;
            var buffer = pair.Value;
            if (camera) {
                camera.RemoveCommandBuffer(pass, buffer);
            }
        }
        cameras_.Clear();
    }

    void OnEnable()
    {
        spectrum = new Texture2D(1, 1, TextureFormat.R8, false, true);
        spectrum.SetPixel(0, 0, Color.white);
        spectrum.Apply();
        CleanUp();
    }

    void OnDisable()
    {
        CleanUp();
    }

    void OnWillRenderObject()
    {
        UpdateCommandBuffer();
    }

    float prevSound;

    void UpdateCommandBuffer()
    {
        var act = gameObject.activeInHierarchy && enabled;
        if (!act) {
            OnDisable();
            return;
        }


        rotationAngle += Time.deltaTime * Mathf.Abs(prevSound - AudioReactive.I.RmsHigh);
        prevSound = AudioReactive.I.RmsHigh;
        progress += speed * Time.deltaTime;

        material.SetFloat("_Floor", floorSens);
        material.SetFloat("_Grid", gridSize);
        material.SetFloat("_GridRotation", rotationAngle);
        material.SetFloat("_SpeedProgress", progress);
        material.SetFloat("_Bpm", Bpm);
        material.SetColor("_Color", MainColor);
        material.SetTexture("_MainTex", spectrum);

        var camera = Camera.current;
        if (!camera || cameras_.ContainsKey(camera)) return;

        if (!quad_) quad_ = GenerateQuad();

        var buffer = new CommandBuffer();
        buffer.name = "Raymarching";
        buffer.DrawMesh(quad_, Matrix4x4.identity, material, 0, 0);
        camera.AddCommandBuffer(pass, buffer);
        cameras_.Add(camera, buffer);
    }
}
