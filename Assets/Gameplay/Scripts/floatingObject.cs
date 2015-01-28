using UnityEngine;
using System.Collections;

public class floatingObject : MonoBehaviour {

	float step;
	public float stepFactor = 0.2f;
	public float duration = 1f;

	// Update is called once per frame
	void Update () {
		var phi = Time.time / duration * Mathf.PI;
		// get cosine and transform from -1..1 to 0..1 range
		step = Mathf.Cos( phi ) * stepFactor;
		
		transform.position = new Vector3(transform.position.x,transform.position.y + step * Time.deltaTime, transform.position.z);
		
		
	}

}
