using UnityEngine;
using System.Collections;

public class MarineFishSelectScript : MonoBehaviour {
	public GameObject[] fishes;
	public GameObject fishCamera;

	public void SelectFish(int fishNum){
		fishCamera.GetComponent<MarineFishCameraScript> ().TargetSet (fishes[fishNum]);
		foreach (GameObject fish in fishes) {
			fish.GetComponent<MarineFishUserController>().enabled=false;
			fish.GetComponent<MarineFishGotoTheGoalScript>().enabled=true;
		}
		fishes[fishNum].GetComponent<MarineFishUserController>().enabled=true;
		fishes[fishNum].GetComponent<MarineFishGotoTheGoalScript>().enabled=false;

	}
}
