using UnityEngine;
using System.Collections;

public class FlashCollider : MonoBehaviour {

	Animator anCrab;
	Animator[] anChildren;
	private bool touch;
	Movement mv;
	UpdateCollider uc;
	// Use this for initialization
	void Start () {
		anCrab = gameObject.GetComponent<Animator> () as Animator;
		anChildren = gameObject.GetComponentsInChildren<Animator> () as Animator[];
		mv = gameObject.GetComponent<Movement> () as Movement;
		uc = gameObject.GetComponentInChildren<UpdateCollider> () as UpdateCollider;
		touch = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if ((other.tag == "flash") && !touch) {
			touch = true;
			mv.stop();
			uc.stop();
			StartCoroutine(WaitRestart(4f,anCrab.speed,anChildren[1].speed));
			anCrab.speed = 0;
			anChildren[1].speed = 0;
			anChildren[2].SetBool("startStars",true);
		}
	}

	IEnumerator WaitRestart(float delay, float speedCrab, float speedPincer){
		yield return new WaitForSeconds(delay);
		anCrab.speed = speedCrab;
		mv.restart ();
		uc.restart ();
		anChildren[1].speed = speedPincer ;
		anChildren [2].SetBool ("startStars", false);
		touch = false;
	}
}
