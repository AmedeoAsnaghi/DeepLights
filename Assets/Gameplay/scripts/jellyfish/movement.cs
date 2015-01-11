	using UnityEngine;
	using System.Collections;

	public class movement : MonoBehaviour {

	private GameManager gameManager;
	Animator an = null;
	bool i_am_moving = false;
	bool turning_right = false;
	bool turning_left = false;
	Rigidbody2D rigidBody;
	float velocity;
	float linearAccelleration;

	public float rotationSpeed;
	public float accellerationStep;
	public float speed = 1f;
	public float maxSpeed;

	private bool canMove;

	#region READ_KEYS
	bool getLeft(){
		return Input.GetKey (KeyCode.LeftArrow);
	}
	
	bool getRight(){
		return Input.GetKey (KeyCode.RightArrow);
	}
	
	bool getThrust(){
		return Input.GetKey (KeyCode.UpArrow);
	}
	
	bool getFire(){
		return Input.GetKeyDown (KeyCode.LeftControl);
	}

	bool getLightImpulse(){
		return (Input.GetKeyDown(KeyCode.Q));
	}

	bool getBarrier(){
		return (Input.GetKeyDown (KeyCode.E));
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
		else{
			if (i_am_moving) {
				i_am_moving = false;
				an.SetBool ("is_moving", false);
			}
			if(getLeft() && getRight())
			{
				linearAccelleration = 0;
				an.SetBool("turning_left",false);
				an.SetBool("turning_right",false);
				rigidBody.angularVelocity = 0;
			}
			else{
			
				if (getLeft () && !getRight() /*&& (currentState.nameHash != Animator.StringToHash ("Base Layer.Charging"))*/) {
					//transform.Rotate(Vector3.forward * rotationSpeed * linearAccelleration * Time.deltaTime);
					rigidBody.angularVelocity = rotationSpeed * linearAccelleration;
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
					if (getRight () && !getLeft()) {
						//transform.Rotate (Vector3.forward * (-rotationSpeed) * linearAccelleration * Time.deltaTime);
						rigidBody.angularVelocity = -rotationSpeed * linearAccelleration;
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
				}
			}
		}

	


		//-------POWERS-------
		if (getLightImpulse ()) {
			gameManager.doLightImpulse ();
		}
		if (getBarrier ()) {
			gameManager.doBarrier();		
		}
	}
}
