using UnityEngine;
using System.Collections;

public class parallax : MonoBehaviour {

	GameObject mainCamera = null;
	Vector3 cameraOldPos;
	public float attenuation = 1f;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.Find ("Main Camera");
		cameraOldPos = mainCamera.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 cameraNewPos = mainCamera.transform.position;

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
