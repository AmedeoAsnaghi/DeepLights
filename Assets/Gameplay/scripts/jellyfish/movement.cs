using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {

	Animator an = null;
	float speed = 2f;
	float rotationSpeed = 120f;
	bool i_am_moving = false;
	Rigidbody2D rigidBody;

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
	#endregion

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> () as Rigidbody2D;
		an = GetComponent<Animator> () as Animator;
	}

	
	// Update is called once per frame
	void Update () {

		if (getLeft ()) {
			transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
		}

		if (getRight ()) {
			transform.Rotate(Vector3.forward * (-rotationSpeed) * Time.deltaTime);
		}

		if (getThrust ()) {
			//			transform.position += transform.up * speed * Time.deltaTime;
			rigidBody.AddForce(transform.up*speed);
			if (!i_am_moving){
				i_am_moving = true;
				an.SetBool("is_moving",true);
			}
		}
		else {
			if (i_am_moving){
				i_am_moving = false;
				an.SetBool("is_moving",false);
			}
		}

		if (/*canShoot &&*/ getFire ()) {
			//GameObject missile = Instantiate(prefabMissile, transform.position + transform.up * 0.45f, transform.rotation) as GameObject;
			
			//canShoot = false;
			//StartCoroutine(WaitToShoot());
		}
	
	}
}
