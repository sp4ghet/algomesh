using UnityEngine;
using System.Collections.Generic;

public class SpectrumTest : MonoBehaviour {

	[SerializeField] Material _lineMaterial;
	float[] samples;
	new AudioSource audio;
	Mesh _mesh;
	List<Vector3> _vertices;

	// Use this for initialization
	void Start() {
		samples = new float[1024];
		_vertices = new List<Vector3>(samples.Length);
		for (var i = 0; i < samples.Length; i++) _vertices.Add(Vector3.zero);

		CreateMesh();

		audio = GetComponent<AudioSource>();
		audio.clip = Microphone.Start(null, true, 100, 44100);
		audio.loop = true;
		while (!(Microphone.GetPosition(null) > 0)) { }
		Debug.Log("start playing... position is " + Microphone.GetPosition(null));
		audio.Play();
	}

	// Update is called once per frame
	void Update() {
		audio.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);

		for (int i = 1; i < samples.Length - 1; i++) {
			//Debug.DrawLine(new Vector3((i - 1f)*10 / samples.Length, samples[i]*200 + 10, 0), new Vector3((float)i*10/samples.Length, samples[i + 1]*200 + 10, 0), Color.red);
			//Debug.DrawLine(new Vector3((i - 1f)*10 / samples.Length, Mathf.Log(samples[i - 1]) + 10, 2), new Vector3((float)i*10/samples.Length, Mathf.Log(samples[i]) + 10, 2), Color.cyan);
			Debug.DrawLine(new Vector3(Mathf.Log(i - 1), samples[i - 1] * 200 - 10, 1), new Vector3(Mathf.Log(i), samples[i] * 200 - 10, 1), Color.cyan);
			//Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(samples[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(samples[i]), 3), Color.blue);
		}
		UpdateMeshWithWaveform();
		Graphics.DrawMesh(_mesh, transform.localToWorldMatrix, _lineMaterial, gameObject.layer);
	}

	void CreateMesh() {
		var indices = new int[2 * (samples.Length - 1)];

		for (var i = 0; i < samples.Length - 1; i++) {
			indices[2 * i + 0] = i;
			indices[2 * i + 1] = i + 1;
		}

		_mesh = new Mesh();
		_mesh.MarkDynamic();
		_mesh.SetVertices(_vertices);
		_mesh.SetIndices(indices, MeshTopology.Lines, 0);
	}

	void UpdateMeshWithWaveform() {
		var scale = Mathf.Log(samples.Length) / 2f;

		for (var i = 0; i < samples.Length; i++)
			_vertices[i] = new Vector3(Mathf.Log(i)-scale, samples[i]*200, 0);

		_mesh.SetVertices(_vertices);
	}
}
