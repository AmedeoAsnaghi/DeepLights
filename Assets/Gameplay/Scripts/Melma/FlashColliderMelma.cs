using UnityEngine;
using System.Collections;

public class FlashColliderMelma : MonoBehaviour {
	private Animator anMelma;
	private Animator anStars;
	private bool touch;

	void Start () {
		anMelma = gameObject.GetComponent<Animator> () as Animator;
		anStars = (gameObject.GetComponentsInChildren<Animator>() as Animator[])[1];
		touch = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter2D(Collider2D other){
		if ((other.tag == "flash") && !touch) {
			touch = true;
			
			StartCoroutine(WaitRestart(4f,anMelma.speed));
			anMelma.speed = 0;
			anStars.SetBool("startStars",true);
		}
	}
	
	IEnumerator WaitRestart(float delay, float speed){
		yield return new WaitForSeconds(delay);
		anMelma.speed = speed;
		anStars.SetBool ("startStars", false);
		touch = false;
	}
}
