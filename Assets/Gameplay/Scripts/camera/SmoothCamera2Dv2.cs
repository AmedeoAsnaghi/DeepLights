using UnityEngine;
using System.Collections;

public class SmoothCamera2Dv2 : MonoBehaviour {

	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;
	public bool init;
	private bool firstTime = true;
	// Update is called once per frame
	void Update () 
	{
		if (target && !init)
		{
			if(firstTime){
				Animator anCamera = gameObject.GetComponent<Animator>() as Animator;
				if(anCamera){
					anCamera.enabled = false;
				}
				firstTime = false;
			}
			Vector3 point = camera.WorldToViewportPoint(target.position);
			Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = gameObject.transform.position + delta;
			gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, destination, ref velocity, dampTime);
		}
		
	}
}
