using UnityEngine;
using System.Collections;

public class parallax : MonoBehaviour {

	GameObject camera = null;
	Vector3 cameraOldPos;
	public float attenuation = 1f;

	// Use this for initialization
	void Start () {
		camera = GameObject.Find ("Main Camera");
		cameraOldPos = camera.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 cameraNewPos = camera.transform.position;

		if (cameraNewPos.x != cameraOldPos.x) {
			float step = -(cameraNewPos.x - cameraOldPos.x)/attenuation;
			gameObject.transform.position = new Vector3 (transform.position.x + step, transform.position.y);		
		}

		if (cameraNewPos.y != cameraOldPos.y) {
			float step = -(cameraNewPos.y - cameraOldPos.y)/attenuation;
			gameObject.transform.position = new Vector3 (transform.position.x, transform.position.y + step);		
		}


		cameraOldPos = cameraNewPos;
	
	}
}
