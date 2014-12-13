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
			Destroy (other.gameObject);
			gameManager.increaseLife(10);	
		} 
		//TODO: manage other collision
		if (other.gameObject.tag == "Pincers") {
			gameManager.decreaseLife(10);
		}

		if (other.gameObject.tag == "Predator") {
			if (other.GetType() == typeof(BoxCollider2D))
				gameManager.decreaseLife(20);
		}
	}


}
