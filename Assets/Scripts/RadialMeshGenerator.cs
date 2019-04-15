using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMeshGenerator : MonoBehaviour {

	[SerializeField]
	GameObject radialMesh;

	float recentPop = 0;

    float meshThreshold;
    int subdivisions = 4;
    bool subdivisionsRandom;
    RadialMesh.RadialState radialMode;
    private float bpm;

    public float Bpm { get => bpm; set => bpm = value; }
    public float MeshThreshold { get => meshThreshold; set => meshThreshold = value; }

    // Update is called once per frame
    void Update () {
		float peak = AudioReactive.I.PeakLow;

		if(peak > meshThreshold
            && recentPop > 60f/bpm 
            && radialMode != RadialMesh.RadialState.off
        ) {
            var radialMeshInstance = Instantiate(radialMesh, transform);
            var mesh = radialMeshInstance.GetComponent<RadialMesh>();
            if (subdivisionsRandom) {
                mesh.Subdivisions = Random.Range(3, subdivisions);
            }
            else {
                mesh.Subdivisions = subdivisions;
            }

            recentPop = 0;
		}
		recentPop += Time.deltaTime;
	}
}
