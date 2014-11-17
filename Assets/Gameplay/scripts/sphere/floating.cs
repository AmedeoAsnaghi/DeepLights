using UnityEngine;
using System.Collections;

public class floating : MonoBehaviour {

	float step ;
	float duration = 2.0f;

	// Use this for initialization
	void Start () {

	}

	
	// Update is called once per frame
	void Update () {
		var phi = Time.time / duration * 2 * Mathf.PI;
		// get cosine and transform from -1..1 to 0..1 range
		step = Mathf.Cos( phi ) * 0.5f ;

		transform.position = new Vector3(transform.position.x,transform.position.y + step * Time.deltaTime, transform.position.z);


	}
	
}
