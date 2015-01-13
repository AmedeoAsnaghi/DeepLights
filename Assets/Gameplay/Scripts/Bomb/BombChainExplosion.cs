using UnityEngine;
using System.Collections;

public class BombChainExplosion : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log ("SONO LA BOMBA"  + other.gameObject.tag);

		if (other.gameObject.tag == "chainCollider") {
			Debug.Log ("BOMBAAAA!");
			Animator an = other.gameObject.GetComponent<Animator> () as Animator;
			an.SetTrigger("bombTouched");
		} 

	}
}
