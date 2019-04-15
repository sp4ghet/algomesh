using UnityEngine;
using System.Collections;

public class MarineFishRandomGoalScript : MonoBehaviour {
	public float nextChangeTime=0f;
	public float maxNextChangeTime=15f;
	public float changeTime=0f;
	public float maxGoalDistance=20f;

	void Start () {
		nextChangeTime = Random.Range (0f,maxNextChangeTime);
	}

	void FixedUpdate () {
		changeTime += Time.deltaTime;
		if(changeTime>nextChangeTime){
			changeTime=0f;
			nextChangeTime = Random.Range (0f,maxNextChangeTime);
			GetComponent<MarineFishGotoTheGoalScript>().goalGameObject.transform.position=Random.insideUnitSphere*maxGoalDistance;
		}
	}
}
