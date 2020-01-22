using UnityEngine;
using System.Collections;

public class MarineFishGotoTheGoalScript : MonoBehaviour {
	public GameObject goalGameObject;

	MarineFishCharacter marineFishCharacter;
	Vector3 goalRelPos;
	float goalDistance=0f;

	void Start () {
		marineFishCharacter = GetComponent < MarineFishCharacter> ();
		goalGameObject=new GameObject();

		goalGameObject.transform.position = Random.insideUnitSphere*5f;
		goalRelPos = goalGameObject.transform.position - transform.position;


	}



	void FixedUpdate(){
		goalDistance=(goalGameObject.transform.position - transform.position).sqrMagnitude;

		marineFishCharacter.forwardAcceleration = Mathf.Clamp (goalDistance,marineFishCharacter.minForwardAcceleration,marineFishCharacter.minForwardAcceleration+2f);

		goalRelPos = transform.InverseTransformPoint (goalGameObject.transform.position).normalized;
		if(goalRelPos.z>0f){
			marineFishCharacter.turnAcceleration=goalRelPos.x;
		}else{
			if(goalRelPos.x>0f){
				marineFishCharacter.turnAcceleration=1f;
			}else{
				marineFishCharacter.turnAcceleration=-1f;
			}
		}
		marineFishCharacter.upDownAcceleration = goalRelPos.y;
        
	}

}
