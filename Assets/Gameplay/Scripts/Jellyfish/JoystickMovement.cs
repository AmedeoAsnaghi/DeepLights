using UnityEngine;
using System.Collections;

public class JoystickMovement : MonoBehaviour {

	private GameManager gameManager;
	Animator an = null;
	bool i_am_moving = false;
	bool turning_right = false;
	bool turning_left = false;
	Rigidbody2D rigidBody;
	float velocity;
	float linearAccelleration;
	
	public float rotationSpeed = 240f;
	public float accellerationStep = 0.03f;
	public float speed = 0.01f;
	public float maxSpeed = 2f;
	
	private bool canMove;
	
	#region READ_KEYS
	float getTurn() {
		return Input.GetAxis("Horizontal");
		}
	
	bool getThrust(){
		return Input.GetButton("MovementImpulse");
	}

	bool getLightImpulse(){
		return Input.GetButtonDown("LightImpulse");
	}
	bool getBarrier(){
		return Input.GetButtonDown ("Barrier");
	}
	bool getFlash(){
		return Input.GetButtonDown ("Flash");
	}
	#endregion
	
	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> () as Rigidbody2D;
		an = GetComponent<Animator> () as Animator;
		linearAccelleration = 0;
		GameObject controller = GameObject.Find("Controller");
		gameManager = controller.GetComponent<GameManager> () as GameManager;
	}
	
	
	// Update is called once per frame
	void Update () {
		var currentState  = an.GetCurrentAnimatorStateInfo(0);	

		/*---GO STRAIGHT----*/
		if (getThrust ())// &&  !(an.GetBool("turning_right") || an.GetBool("turning_left"))) 
		{
			//			transform.position += transform.up * speed * Time.deltaTime;
			if ((currentState.nameHash == Animator.StringToHash ("Base Layer.Moving")) && rigidBody.velocity.sqrMagnitude < (Vector3.one * maxSpeed).sqrMagnitude)
				rigidBody.AddForce (transform.up * speed, ForceMode2D.Impulse);
			if ((currentState.nameHash == Animator.StringToHash ("Base Layer.Charging")) && rigidBody.velocity.sqrMagnitude < (Vector3.one * maxSpeed).sqrMagnitude)
				rigidBody.AddForce (-transform.up * speed);
			if (!i_am_moving) {
				i_am_moving = true;
				an.SetBool ("is_moving", true);
			}
		}
		else {
			if (i_am_moving) {
				i_am_moving = false;
				an.SetBool ("is_moving", false);
			}
		}

		/*---TURN_LEFT----*/
		if (getTurn () < 0) {
			//transform.Rotate(Vector3.forward * rotationSpeed * linearAccelleration * Time.deltaTime);
			rigidBody.angularVelocity = -getTurn()*rotationSpeed * linearAccelleration;
			if (linearAccelleration < 1)
				linearAccelleration += accellerationStep;
			if (!turning_left) {
				turning_left = true;
				an.SetBool ("turning_left", true);
			}
		}
		else {
			if (turning_left) {
				turning_left = false;
				an.SetBool ("turning_left", false);
				linearAccelleration = 0;
			}
		}

		/*---TURN_RIGHT----*/
		if (getTurn () > 0) {
			//transform.Rotate (Vector3.forward * (-rotationSpeed) * linearAccelleration * Time.deltaTime);
			rigidBody.angularVelocity = -getTurn()*rotationSpeed * linearAccelleration;
			if (linearAccelleration < 1)
				linearAccelleration += accellerationStep;
			if (!turning_right) {
				turning_right = true;
				an.SetBool ("turning_right", true);
			}
		}
		else {
			if (turning_right) {
				turning_right = false;
				an.SetBool ("turning_right", false);
				linearAccelleration = 0;
			}
		}



		//-------POWERS-------
		if (getLightImpulse ()) {
			gameManager.doLightImpulse ();
		}
		if (getBarrier ()) {
			gameManager.doBarrier();
		}
		if (getFlash ()) {
			gameManager.doFlash();		
		}
	}
}
