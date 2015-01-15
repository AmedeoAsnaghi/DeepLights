using UnityEngine;
using System.Collections;

public class LineOfSight : MonoBehaviour {

	private Attack attack;
	// Use this for initialization
	void Start () {
		attack = gameObject.GetComponentInChildren<Attack> () as Attack;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			attack.preyFound(other);		
		}
	}
}
