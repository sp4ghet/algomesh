using UnityEngine;
using System.Collections;

public class SharkUserController : MonoBehaviour {
	SharkCharacter sharkCharacter;
	
	void Start () {
		sharkCharacter = GetComponent < SharkCharacter> ();
	}
	
	private void FixedUpdate()
	{
		
		if (Input.GetKeyDown (KeyCode.H)) {
			sharkCharacter.Hit();
		}
		
		if (Input.GetButtonDown ("Fire1")) {
			sharkCharacter.Bite();
		}	
		
		if (Input.GetKeyDown (KeyCode.F)) {
			sharkCharacter.BiteRight();
		}
		
		if (Input.GetKeyDown (KeyCode.G)) {
			sharkCharacter.BiteLeft();
		}	

		if (Input.GetKeyDown (KeyCode.V)) {
			sharkCharacter.BiteDown();
		}
		
		if (Input.GetKeyDown (KeyCode.T)) {
			sharkCharacter.BiteUp();
		}	

		if (Input.GetKeyDown (KeyCode.B)) {
			sharkCharacter.TwistBiteRight();
		}
		
		if (Input.GetKeyDown (KeyCode.C)) {
			sharkCharacter.TwistBiteLeft();
		}	

		if (Input.GetKeyDown (KeyCode.K)) {
			sharkCharacter.Death();
		}
		
		if (Input.GetKeyDown (KeyCode.L)) {
			sharkCharacter.Rebirth();
		}		


		if (Input.GetKey (KeyCode.U)) {
			sharkCharacter.upDownAccerelation = Mathf.Lerp (sharkCharacter.upDownAccerelation, 1f, Time.deltaTime);
		} else if (Input.GetKey (KeyCode.N)) {
			sharkCharacter.upDownAccerelation = Mathf.Lerp (sharkCharacter.upDownAccerelation, -1f, Time.deltaTime);
		} else {
			sharkCharacter.upDownAccerelation=Mathf.Lerp(sharkCharacter.upDownAccerelation,0f,Time.deltaTime);
		}
		
		if (Input.GetKey (KeyCode.Z)) {
			sharkCharacter.rollAccerelation = Mathf.Lerp (sharkCharacter.rollAccerelation, 1f, Time.deltaTime);
		} else if (Input.GetKey (KeyCode.X)) {
			sharkCharacter.rollAccerelation = Mathf.Lerp (sharkCharacter.rollAccerelation, -1f, Time.deltaTime);
		} else {
			sharkCharacter.rollAccerelation=Mathf.Lerp(sharkCharacter.rollAccerelation,0f,Time.deltaTime);
		}

		sharkCharacter.forwardAccerelation = Input.GetAxis ("Vertical")+1f;
		sharkCharacter.turnAccerelation = Input.GetAxis ("Horizontal");
	}
}
