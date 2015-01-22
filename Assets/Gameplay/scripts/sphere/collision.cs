using UnityEngine;
using System.Collections;

public class collision : MonoBehaviour {
	public float stepExplosion;
	public float secondToExplode;
	public int maxParticles;

	ParticleSystem[] ps;
	bool explode;
	AudioSource sound;

	// Use this for initialization
	void Start () {
		ps = GetComponentsInChildren<ParticleSystem> (false) as ParticleSystem[];
		explode = false;
		sound = GetComponent<AudioSource> () as AudioSource;
	}
	
	// Update is called once per frame
	void Update () {
		if (explode) {
			ps[0].startLifetime-=stepExplosion;
			ps[1].startLifetime-=stepExplosion;
		}
	}

	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.tag == "Player") {
			explode = true;
			if (gameObject.tag != "yellowPower" && gameObject.tag != "bluePower" && gameObject.tag != "redPower"){
				ps[2].maxParticles = maxParticles;
			}
			sound.Play ();
			StartCoroutine(WaitToExplode());
		} 
	}

	IEnumerator WaitToExplode()
	{
		yield return new WaitForSeconds(secondToExplode);
		Destroy (gameObject);
	}
}
