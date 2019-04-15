using UnityEngine;
using System.Collections;

public class MarineFishUserController : MonoBehaviour {
	MarineFishCharacter marineFishCharacter;
	
	void Start () {
		marineFishCharacter = GetComponent < MarineFishCharacter> ();
	}
	
	private void FixedUpdate()
	{
		

		
		if (Input.GetKeyDown (KeyCode.B)) {
			marineFishCharacter.Bite();
		}

		if (Input.GetKeyDown (KeyCode.O)) {
			marineFishCharacter.MouthOpen();
		}		
		
		if (Input.GetKeyUp (KeyCode.O)) {
			marineFishCharacter.MouthClose();
		}		
		
		if (Input.GetKey(KeyCode.U)) {
			marineFishCharacter.upDownAcceleration=Mathf.Lerp(marineFishCharacter.upDownAcceleration,1f,Time.deltaTime);
		}		
		
		if (Input.GetKey(KeyCode.N)) {
			marineFishCharacter.upDownAcceleration=Mathf.Lerp(marineFishCharacter.upDownAcceleration,-1f,Time.deltaTime);
		}		
		
		
		marineFishCharacter.upDownAcceleration=Mathf.Lerp(marineFishCharacter.upDownAcceleration,0f,Time.deltaTime);
		
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");
		

		marineFishCharacter.forwardAcceleration = v+1f+marineFishCharacter.minForwardAcceleration;
		marineFishCharacter.turnAcceleration = h;
		
	}
}
