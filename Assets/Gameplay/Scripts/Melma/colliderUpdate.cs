using UnityEngine;
using System.Collections;

public class colliderUpdate : MonoBehaviour {
	BoxCollider2D[] colliders;

	// Use this for initialization
	void Start () {
		colliders = (GameObject.FindGameObjectWithTag("melma")).GetComponents<BoxCollider2D>() as BoxCollider2D[];
	}
	
	// Update is called once per frame
	void Update () {
		colliders [1].size = colliders [0].size;
	}
}
