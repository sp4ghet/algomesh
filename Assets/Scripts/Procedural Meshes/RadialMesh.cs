using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RadialMesh : MonoBehaviour {

    public enum RadialState : int {
        random = 0
        ,simple = 1
        ,orderly = 2
        ,off = 3
    }

    private RadialState radialMeshMode;
    private bool bendTunnel;

	const float TAU = Mathf.PI * 2;
    const float PI = Mathf.PI;

	[SerializeField]
	float speed = 1;
	[SerializeField]
	float m_duration = 10;
	[SerializeField]
	float popDuration = 2;
	[SerializeField]
	Easing.Ease easing = Easing.Ease.EaseOutBack;
    [SerializeField]
    int subdivisions = 100;
    [SerializeField]
	MeshFilter filter;

    Material glow;
    int emissionId;
    Color emissionColor;

	Easing.Function easingFunction;

	Mesh mesh;
	Vector3[] vertices;
	int[] tris;
    
	const int e = 4; //edges, or vertices per subdivision
	const int tv = 24;
	float maxRadius = 10;

	float startTime;

	float rotationSpeed;

    public int Subdivisions {
        get {
            return subdivisions;
        }

        set {
            subdivisions = value;
        }
    }

    public Color EmissionColor { get => emissionColor; set => emissionColor = value; }
    public RadialState RadialMeshMode { get => radialMeshMode; set => radialMeshMode = value; }
    public float Speed { get => speed; set => speed = value; }

    IEnumerator Pop(float duration) {
		for (float t = 0; t < duration; t += Time.deltaTime) {
			float radius = easingFunction(0, maxRadius, t / duration);
			float side = easingFunction(0, 0.5f, t / duration);

			for (int i = 0; i < subdivisions; i++) {
				Quaternion rot = Quaternion.Euler(0, transform.rotation.eulerAngles.y, (i/(float)subdivisions) * 360 );
				vertices[i*e] = rot * new Vector3(0, radius - side, 0);
				vertices[i*e + 1] = rot * new Vector3(0, radius - side, side*2);
				vertices[i*e + 2] = rot * new Vector3(0, radius + side, 0);
				vertices[i*e + 3] = rot * new Vector3(0, radius + side, side*2);

				tris[i*tv + 0] = (i * e + 0) % (subdivisions * 4);
				tris[i*tv + 1] = (i * e + 4) % (subdivisions * 4);
				tris[i*tv + 2] = (i * e + 2) % (subdivisions * 4);

				tris[i*tv + 3] = (i * e + 2) % (subdivisions * 4);
				tris[i*tv + 4] = (i * e + 4) % (subdivisions * 4);
				tris[i*tv + 5] = (i * e + 6) % (subdivisions * 4);

				tris[i*tv + 6] = (i * e + 2) % (subdivisions * 4);
				tris[i*tv + 7] = (i * e + 6) % (subdivisions * 4);
				tris[i*tv + 8] = (i * e + 3) % (subdivisions * 4);

				tris[i*tv + 9] = (i * e + 3) % (subdivisions * 4);
				tris[i*tv + 10] = (i * e + 6) % (subdivisions * 4);
				tris[i*tv + 11] = (i * e + 7) % (subdivisions * 4);

				tris[i*tv + 12] = (i * e + 3) % (subdivisions * 4);
				tris[i*tv + 13] = (i * e + 7) % (subdivisions * 4);
				tris[i*tv + 14] = (i * e + 1) % (subdivisions * 4);

				tris[i*tv + 15] = (i * e + 1) % (subdivisions * 4);
				tris[i*tv + 16] = (i * e + 7) % (subdivisions * 4);
				tris[i*tv + 17] = (i * e + 5) % (subdivisions * 4);

				tris[i*tv + 18] = (i * e + 1) % (subdivisions * 4);
				tris[i*tv + 19] = (i * e + 5) % (subdivisions * 4);
				tris[i*tv + 20] = (i * e + 0) % (subdivisions * 4);

				tris[i*tv + 21] = (i * e + 0) % (subdivisions * 4);
				tris[i*tv + 22] = (i * e + 5) % (subdivisions * 4);
				tris[i*tv + 23] = (i * e + 4) % (subdivisions * 4);
			}

			mesh.vertices = vertices;
			mesh.triangles = tris;
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			yield return new WaitForEndOfFrame();
		}
        if(radialMeshMode != RadialState.simple) {
            StartCoroutine(Twist(1f));
        }
    }

    IEnumerator Twist(float duration) {
        float radius = maxRadius;
        float side = 0.5f;
        for (float t = 0; t < duration; t += Time.deltaTime) {
            float cT = Mathf.Sin(t*PI / duration); //circular time
            float nT = t / duration; //normalized time

            glow.SetColor(emissionId, Color.Lerp(Color.white * 0.1f, emissionColor, nT));

            for (int i = 0; i < subdivisions; i++) {
                Quaternion rot = Quaternion.Euler(0, transform.rotation.eulerAngles.y, (i / (float)subdivisions) * 360);
                vertices[i * e] = rot * new Vector3(0, radius - side, 0);
                vertices[i * e + 1] = rot * new Vector3(0, radius - side, side * 2);
                vertices[i * e + 2] = rot * new Vector3(0, radius + side, 0);
                vertices[i * e + 3] = rot * new Vector3(0, radius + side, side * 2);

                float angle = Mathf.Lerp(0, Mathf.Abs(vertices[i*e].normalized.y) * 720, cT);
                rot = Quaternion.Euler(angle, 0, 0);
                vertices[i * e] = rot * vertices[i*e];
                vertices[i * e + 1] = rot * vertices[i * e+1];
                vertices[i * e + 2] = rot * vertices[i * e+2];
                vertices[i * e + 3] = rot * vertices[i * e+3];
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < subdivisions; i++) {
            Quaternion rot = Quaternion.Euler(0, transform.rotation.eulerAngles.y, (i / (float)subdivisions) * 360);
            vertices[i * e] = rot * new Vector3(0, radius - side, 0);
            vertices[i * e + 1] = rot * new Vector3(0, radius - side, side * 2);
            vertices[i * e + 2] = rot * new Vector3(0, radius + side, 0);
            vertices[i * e + 3] = rot * new Vector3(0, radius + side, side * 2);
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }


	// Use this for initialization
	void Start () {
        filter = GetComponent<MeshFilter>();
		mesh = new Mesh();
        mesh.MarkDynamic();
		filter.mesh = mesh;
		vertices = new Vector3[subdivisions * e];
		tris = new int[subdivisions * tv];

        switch(radialMeshMode) {
        case RadialState.random:
            transform.localRotation *= Quaternion.Euler(0, 0, Random.Range(0, 360));
            break;
        case RadialState.simple:
            break;
        case RadialState.orderly:
            rotationSpeed = 180;
            break;
        case RadialState.off:
            Destroy(gameObject);
            break;
        }

		startTime = Time.time;

        glow = GetComponent<MeshRenderer>().material;
        emissionId = Shader.PropertyToID("_EmissionColor");
        emissionColor = glow.GetColor(emissionId);
        glow.SetColor(emissionId, Color.white * 0.1f);

        easingFunction = Easing.GetEasingFunction(easing);
		StartCoroutine(Pop(popDuration));
	}

	// Update is called once per frame
	void Update () {
        float lifeTime = Time.time - startTime;
        float duration = m_duration / transform.parent.localScale.x;
        if (Time.time - startTime >= duration
            || radialMeshMode == RadialState.off) {
            Destroy(gameObject);
        }

        if(radialMeshMode == RadialState.orderly) {
            transform.localRotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);
        }

        if (bendTunnel) {
            const float angle = Mathf.PI / 2f;
            const float phase = Mathf.PI / 2.4f;
            float y = Mathf.Sin(angle * lifeTime / (duration) + phase) * 50f - 50f;
            float z = Mathf.Cos(angle * lifeTime / (duration) + phase) * 50f - 10f;
            transform.position = new Vector3(0, y, z);
        }
        else {
            transform.localPosition = (transform.localRotation * Vector3.forward) * speed * lifeTime;

        }
    }
}
