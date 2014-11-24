using UnityEngine;
using System.Collections;

public class collisions : MonoBehaviour {

	Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> () as Rigidbody2D;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "sphere") {
						Destroy (other.gameObject);

						//TODO: increase resource
				} 
		//TODO: manage other collision
	}
}
