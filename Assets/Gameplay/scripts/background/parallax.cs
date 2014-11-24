using UnityEngine;
using System.Collections;

public class parallax : MonoBehaviour {

	public float duration = 2.0f;
	public float pulsingFactor;
	public float waveAmplitude;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		var phi = (Time.time*pulsingFactor) / duration * 2 * Mathf.PI;
		// get cosine and transform from -1..1 to 0..1 range
		var amplitude = Mathf.Cos( phi ) * waveAmplitude ;
		
		transform.position = new Vector3 (transform.position.x+ (float)amplitude , transform.position.y);
	
	}
}
