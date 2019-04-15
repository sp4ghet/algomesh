using UnityEngine;
using System.Collections;

public class SharkCharacter : MonoBehaviour {
	Animator sharkAnimator;
	Rigidbody sharkRigid;
	
	public bool isLived=true;
	public float forwardAccerelation=0f;
	public float turnAccerelation=0f;
	public float upDownAccerelation=0f;
	public float rollAccerelation = 0f;
	public GameObject sharkPrefab;
	//public GameObject sharkBody;

	void Start () {
		sharkAnimator = GetComponent<Animator> ();
		sharkRigid=GetComponent<Rigidbody>();	
	}
	
	void FixedUpdate(){
		Move ();
	}	
	
	public void Hit(){
		sharkAnimator.SetTrigger("Hit");
	}
	
	public void Bite(){
		sharkAnimator.SetTrigger("Bite");
	}		

	public void BiteRight(){
		sharkAnimator.SetTrigger("BiteRight");
	}	

	public void BiteLeft(){
		sharkAnimator.SetTrigger("BiteLeft");
	}	
	public void BiteDown(){
		sharkAnimator.SetTrigger("BiteDown");
	}	
	
	public void BiteUp(){
		sharkAnimator.SetTrigger("BiteUp");
	}	

	public void TwistBiteRight(){
		sharkAnimator.SetTrigger("TwistBiteRight");
	}	
	
	public void TwistBiteLeft(){
		sharkAnimator.SetTrigger("TwistBiteLeft");
	}	

	public void Death(){
		if (isLived) {
			sharkAnimator.SetTrigger ("Death");
			isLived = false;
			//sharkBody= (GameObject)GameObject.Instantiate (sharkPrefab, transform.position, transform.rotation);
			//SkinnedMeshRenderer[] skins = GetComponentsInChildren<SkinnedMeshRenderer> ();
			//foreach (SkinnedMeshRenderer skin in skins) {
			//	skin.enabled = false;
			//}

			//CapsuleCollider[] capsels=GetComponentsInParent<CapsuleCollider>();
			//foreach(CapsuleCollider capsel in capsels){
			//	capsel.enabled=false;
			//}
			//sharkRigid.constraints=RigidbodyConstraints.FreezeAll;

		}

	}
	
	public void Rebirth(){
		if(!isLived){
			sharkAnimator.SetTrigger("Rebirth");
			isLived = true;

			//SkinnedMeshRenderer[] skins = GetComponentsInChildren<SkinnedMeshRenderer> ();
			//foreach (SkinnedMeshRenderer skin in skins) {
			//	skin.enabled = true;
			//}
			
			//CapsuleCollider[] capsels=GetComponentsInParent<CapsuleCollider>();
			//foreach(CapsuleCollider capsel in capsels){
			//	capsel.enabled=true;
			//}
			//Destroy(sharkBody);
			//sharkRigid.constraints=RigidbodyConstraints.None;
		}
	}
	
	public void Move(){
		if (isLived) {
			sharkRigid.AddForce(transform.forward*forwardAccerelation*6000f);
			sharkRigid.AddTorque(-transform.right*upDownAccerelation *5000f);
			sharkRigid.AddTorque(transform.up*turnAccerelation*3000f);
			sharkRigid.AddTorque(transform.forward*rollAccerelation*1000f);
			sharkAnimator.SetFloat ("Forward", forwardAccerelation);
			sharkAnimator.SetFloat ("Turn", turnAccerelation);
			sharkAnimator.SetFloat ("UpDown", upDownAccerelation);
			sharkAnimator.SetFloat ("Roll", rollAccerelation);
		}
	}
}
