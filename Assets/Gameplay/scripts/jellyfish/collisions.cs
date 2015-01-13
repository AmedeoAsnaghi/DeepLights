using UnityEngine;
using System.Collections;

public class collisions : MonoBehaviour {

	Rigidbody2D rigidBody;
	GameManager gameManager;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> () as Rigidbody2D;
		gameManager = GameObject.Find ("Controller").GetComponent<GameManager> () as GameManager;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log (other.gameObject.tag);
		if (other.gameObject.tag == "sphere") {
			gameManager.increaseLife (20);
			gameManager.showUpdatedScore ();
		} 
		//TODO: manage other collision
		else if (other.gameObject.tag == "Pincers") {
			gameManager.decreaseLife (20);
		} 
		else if (other.gameObject.tag == "Predator") {
			if (other.GetType () == typeof(BoxCollider2D)) {
				gameManager.decreaseLife (20);
			}
		} 
		else if (other.gameObject.tag == "assassinAlga") {
			gameManager.decreaseLife (20);
		} 
		else if (other.gameObject.tag == "melma") {
			gameManager.decreaseLife(20);
		} 
		else if (other.gameObject.tag == "bomb") {
			Animator an = other.GetComponent<Animator> () as Animator;
			an.SetTrigger("bombTouched");
			StartCoroutine(BombExplosionEffect(other.gameObject));
		}
		else if (other.gameObject.tag == "nextLevel") {
			gameManager.changeLevel();
		} 

	}

	IEnumerator DestroyBomb(GameObject go) {
		yield return new WaitForSeconds (0.5f);
		Destroy (go);
	}

	IEnumerator BombExplosionEffect(GameObject go) {
		yield return new WaitForSeconds (0.5f);
		if (go != null) {
			CircleCollider2D cc = go.GetComponent<CircleCollider2D>() as CircleCollider2D;
			cc.radius += 1;
			gameManager.decreaseLife(20);
			StartCoroutine (DestroyBomb (go));
		}

	}






}
