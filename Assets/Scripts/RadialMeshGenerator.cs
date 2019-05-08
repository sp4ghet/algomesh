using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMeshGenerator : MonoBehaviour {

	[SerializeField]
	GameObject radialMesh;

    [SerializeField]
    RadialMesh.RadialState radialMode = RadialMesh.RadialState.orderly;

    float recentPop = 0;

    float meshThreshold;
    private float bpm;
    float speed;

    public float Bpm { get => bpm; set => bpm = value; }
    public float MeshThreshold { get => meshThreshold; set => meshThreshold = value; }
    public float Speed { get => speed; set => speed = value; }

    // Update is called once per frame
    void Update () {
		float peak = AudioReactive.I.PeakLow;

		if(peak > meshThreshold
            && recentPop > 60f/bpm 
            && radialMode != RadialMesh.RadialState.off
        ) {
            var radialMeshInstance = Instantiate(radialMesh, transform);
            var mirrorInstance = Instantiate(radialMesh, Vector3.zero, Quaternion.Euler(0f, 180f, 0f), transform);
            var mesh = radialMeshInstance.GetComponent<RadialMesh>();
            var mirrorMesh = mirrorInstance.GetComponent<RadialMesh>();
            
            int subdivisions = 9;
            mirrorMesh.Subdivisions = subdivisions;
            mesh.Subdivisions = subdivisions;
            mirrorMesh.RadialMeshMode = RadialMesh.RadialState.simple;
            mesh.RadialMeshMode = radialMode;
            mesh.Speed = speed;
            mirrorMesh.Speed = speed;
            
            recentPop = 0;
		}
		recentPop += Time.deltaTime;
	}
}
