using UnityEngine;
using System.Collections;

public class lightPulse : MonoBehaviour {
	public float duration = 2.0f;
	public float lightRange;
	public float pulsingFactor;
	public float waveAmplitude;

	// Use this for initialization
	void Start () {
		light.range = lightRange;
	}
	
	// Update is called once per frame
	void Update () {
		var phi = (Time.time*pulsingFactor) / duration * 2 * Mathf.PI;
		// get cosine and transform from -1..1 to 0..1 range
		var amplitude = Mathf.Cos( phi ) * waveAmplitude ;

		light.range = lightRange + (float) amplitude;
	}
}
