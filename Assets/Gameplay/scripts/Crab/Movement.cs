using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	public float timeChangeDirection;
	public float step;

	bool goRight;
	bool startTimer;

	// Use this for initialization
	void Start () {
		goRight = true;
		startTimer = true;
		step = step*Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (startTimer) {
			StartCoroutine (WaitToChange ());
			startTimer = false;
		}
		if (goRight)
			transform.position = new Vector3 (transform.position.x + step, transform.position.y );
		else
			transform.position = new Vector3 (transform.position.x - step, transform.position.y);
	}

	IEnumerator WaitToChange()
	{
		yield return new WaitForSeconds(timeChangeDirection);
		goRight = !goRight;
		startTimer = true;
	}
}
