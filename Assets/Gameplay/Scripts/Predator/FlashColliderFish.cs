using UnityEngine;
using System.Collections;

public class FlashColliderFish : MonoBehaviour {

	private Animator anFish;
	private Animator anStars;
	private bool touch;

	private Attack attack;
	private AttackCollider attackCollider;

	// Use this for initialization
	void Start () {
		anFish = gameObject.GetComponent<Animator> () as Animator;
		anStars = (gameObject.GetComponentsInChildren<Animator>() as Animator[])[1];
		attackCollider = gameObject.GetComponent<AttackCollider> () as AttackCollider;
		attack = gameObject.GetComponent<Attack> () as Attack;
		touch = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if ((other.tag == "flash") && !touch) {
			touch = true;

			attack.stop();
			attackCollider.stop ();

			StartCoroutine(WaitRestart(4f,anFish.speed));
			anFish.speed = 0;
			anStars.SetBool("startStars",true);
		}
	}

	IEnumerator WaitRestart(float delay, float speed){
		yield return new WaitForSeconds(delay);
		anFish.speed = speed;
		attack.restart ();
		attackCollider.restart ();
		anStars.SetBool ("startStars", false);
		touch = false;
	}
}
