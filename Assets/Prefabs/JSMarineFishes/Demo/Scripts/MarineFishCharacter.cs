using UnityEngine;
using System.Collections;

public class MarineFishCharacter : MonoBehaviour {
	Animator marineFishAnimator;
	Rigidbody marineFishRigid;
	
	public bool isLived=true;
	
	public float forwardSpeed=0f;
	public float turnAcceleration=.3f;
	public float upDownAcceleration=0f;
	
	public float forwardAccelerationMultiplier=50f;
	public float minForwardAcceleration=1f;

	public float turnAccelerationMultiplier=5f;
	public float upDownAccelerationMultiplier=100f;
	
	public float forwardAcceleration=0f;
	public float minAnimatorSpeed=.3f;
	
	void Start () {
		marineFishAnimator = GetComponent<Animator> ();
		marineFishRigid=GetComponent<Rigidbody>();
		
	}
	
	void FixedUpdate(){
		Move ();
	}
	

	public void Bite(){
		marineFishAnimator.SetTrigger("Bite");
	}

	public void MouthOpen(){
		marineFishAnimator.SetBool ("MouthOpen", true);
	}

	public void MouthClose(){
		marineFishAnimator.SetBool ("MouthOpen", false);
	}	
	
	
	
	public void Move(){
		if (isLived) {
			//marineFishRigid.velocity = transform.forward * forwardAcceleration * forwardAccelerationMultiplier + transform.up * upDownAcceleration * upDownAccelerationMultiplier;
			//transform.RotateAround (transform.position, Vector3.up, turnAcceleration * Time.deltaTime * turnAccelerationMultiplier);
			marineFishRigid.AddForce(transform.forward*forwardAcceleration*forwardAccelerationMultiplier+transform.up*upDownAcceleration*upDownAccelerationMultiplier);
			marineFishRigid.AddTorque(transform.up*turnAcceleration*turnAccelerationMultiplier);

			forwardSpeed=marineFishRigid.velocity.magnitude;
			marineFishAnimator.speed=forwardAcceleration+minAnimatorSpeed;
			marineFishAnimator.SetFloat ("ForwardSpeed", forwardSpeed);
			marineFishAnimator.SetFloat ("ForwardAcceleration", forwardAcceleration);
			marineFishAnimator.SetFloat ("TurnAcceleration", turnAcceleration);
			marineFishAnimator.SetFloat ("UpDownAcceleration", upDownAcceleration);
		}
	}
}
