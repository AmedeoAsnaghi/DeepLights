using UnityEngine;
using System.Collections;

public class BombChainExplosion : MonoBehaviour {
	private bool updateCollider;
	private CircleCollider2D[] bomb;
	public float updateStep = 0.05f;
	private Animator an;

	void Start(){
		updateCollider = false;
		bomb = gameObject.GetComponents<CircleCollider2D> () as CircleCollider2D[];
		an = gameObject.GetComponent<Animator> () as Animator;
		}
	void Update(){
		if (updateCollider) {
			bomb[0].radius += updateStep;
			bomb[1].radius+=updateStep;
		}
	}

	void OnTriggerEnter2D(Collider2D other){

		//Debug.Log ("SONO LA BOMBA"  + other.gameObject.tag);
		if (other.tag != "flash") {
			an.SetTrigger ("bombTouched");
			gameObject.GetComponentsInParent<AudioSource> () [0].audio.Play ();
			StartCoroutine (BombExplosionEffect ());
	    	updateCollider = true;
		}

	}

	IEnumerator DestroyBomb() {
		yield return new WaitForSeconds (0.5f);
		Destroy (gameObject);
	}
	
	IEnumerator BombExplosionEffect() {
		yield return new WaitForSeconds (0.5f);
		StartCoroutine (DestroyBomb ());	
	}
}
