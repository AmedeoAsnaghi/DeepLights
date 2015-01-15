using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	//public float timeChangeDirection;
	public float step = 0.5f;
	public float range = 3f;

	private float initPosition;
	private bool goRight;
	//bool startTimer;
	int walk;

	// Use this for initialization
	void Start () {
		goRight = true;
		//startTimer = true;
		step = step*Time.deltaTime;
		walk = 1;
		initPosition = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		/*if (startTimer) {
			StartCoroutine (WaitToChange ());
			startTimer = false;
		}*/
		if (transform.position.x > initPosition + range) {
			goRight = false;		
		} 
		else if (transform.position.x < initPosition - range) {
			goRight = true;		
		}
		if (goRight)
			transform.position = new Vector3 (transform.position.x + step*walk, transform.position.y );
		else
			transform.position = new Vector3 (transform.position.x - step*walk, transform.position.y);
	}

	/*IEnumerator WaitToChange()
	{
		yield return new WaitForSeconds(timeChangeDirection);
		goRight = !goRight;
		startTimer = true;
	}*/

	public void restart(){
		walk = 1;
	}

	public void stop(){
		walk = 0;
	}
}
