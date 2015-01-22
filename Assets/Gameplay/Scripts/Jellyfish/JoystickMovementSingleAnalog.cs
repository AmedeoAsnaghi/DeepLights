using UnityEngine;
using System.Collections;

public class JoystickMovementSingleAnalog : MonoBehaviour {
		
	private GameManager gameManager;
	Animator an = null;
	bool i_am_moving = false;
	Rigidbody2D rigidBody;
	private Vector3 newAngle;
	
	public float rotationSpeed = 2f;
	public float accellerationStep = 0.03f;
	public float speed = 0.01f;
	public float maxSpeed = 2f;

	public float dashSpeed = 200f;
	
	private bool canMove;


	#region READ_KEYS
	float getTurn() {
		return Input.GetAxis("Horizontal");
	}
	
	float getThrust(){
		return Input.GetAxis("Vertical");
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

	bool getDash(){
		return Input.GetButton ("Dash");
	}
	#endregion
	
	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> () as Rigidbody2D;
		an = GetComponent<Animator> () as Animator;
		GameObject controller = GameObject.Find("Controller");
		gameManager = controller.GetComponent<GameManager> () as GameManager;
		newAngle = rigidBody.transform.eulerAngles;
	}
	

	// Update is called once per frame
	void Update () {
		var currentState  = an.GetCurrentAnimatorStateInfo(0);	

		/*----GO STRAIGHT AND TURN----*/
		if (getThrust ()!=0 || getTurn() != 0)
		{
			if (getDash ()) 
				dashSpeed = 0.2f;
			else
				dashSpeed=0f;
			if ((currentState.nameHash == Animator.StringToHash ("Base Layer.Moving")) && rigidBody.velocity.sqrMagnitude < (Vector3.one * maxSpeed).sqrMagnitude)
				{
				rigidBody.AddForce (new Vector2(getTurn() * (speed + dashSpeed) , getThrust() * (speed+ dashSpeed))*Time.deltaTime, ForceMode2D.Impulse);

			}
			if ((currentState.nameHash == Animator.StringToHash ("Base Layer.Charging")) && rigidBody.velocity.sqrMagnitude < (Vector3.one * maxSpeed).sqrMagnitude)
			{
				rigidBody.AddForce (-transform.up * speed * Time.deltaTime);

			}
			if (!i_am_moving) {
				i_am_moving = true;
				an.SetBool ("is_moving", true);


			}

			var angleRadians=Mathf.Atan2(getThrust(), getTurn());
			var angleDegrees = angleRadians * Mathf.Rad2Deg - 90;

			if (angleDegrees<0) {
				angleDegrees = 360 + angleDegrees;
			}

			newAngle = new Vector3(0,0,angleDegrees);

			transform.rotation = Quaternion.Lerp(rigidBody.transform.rotation, Quaternion.Euler(newAngle),Time.deltaTime * rotationSpeed);	

		}
		else {
			if (i_am_moving) {
				i_am_moving = false;
				an.SetBool ("is_moving", false);	

			}
		}




		
		//-------POWERS-------
		if (getLightImpulse () && !gameManager.getMustYellow() && !gameManager.getMustBlue()) {
			gameManager.doLightImpulse ();
		}
		else if (getBarrier () && !gameManager.getMustYellow() && !gameManager.getMustRed()) {
			gameManager.doBarrier();
		}
		else if (getFlash () && !gameManager.getMustRed() && !gameManager.getMustBlue()) {
			gameManager.doFlash();		
		}
	}
}
