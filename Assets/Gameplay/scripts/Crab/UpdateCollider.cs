using UnityEngine;
using System.Collections;

public class UpdateCollider : MonoBehaviour {

	Rigidbody2D body;
	BoxCollider2D collider;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> () as Rigidbody2D;
		collider = GetComponent<BoxCollider2D> () as BoxCollider2D;
	}
	
	// Update is called once per frame
	void Update () {
		Destroy (collider);
		collider = (BoxCollider2D) gameObject.AddComponent ("BoxCollider2D");
	}
}
