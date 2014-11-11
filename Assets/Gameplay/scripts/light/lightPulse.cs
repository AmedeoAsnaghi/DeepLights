using UnityEngine;
using System.Collections;

public class lightPulse : MonoBehaviour {
	public float duration = 2.0f;


	// Use this for initialization
	void Start () {
		light.range = 15.0f;
	}
	
	// Update is called once per frame
	void Update () {
		var phi = Time.time / duration * 2 * Mathf.PI;
		// get cosine and transform from -1..1 to 0..1 range
		var amplitude = Mathf.Cos( phi ) * 0.5 + 0.5;
		light.range = (float) amplitude;
	}
}
