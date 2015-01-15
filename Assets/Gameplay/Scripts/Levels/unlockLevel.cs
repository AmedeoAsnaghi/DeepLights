using UnityEngine;
using System.Collections;

public class unlockLevel : MonoBehaviour {
	public int totalLevelEnergy = 0;
	GameManager gameManager;

	// Use this for initialization
	void Start () {
		GameObject controller = GameObject.Find("Controller");
		gameManager = controller.GetComponent<GameManager> () as GameManager;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameManager.getCollectedEnergy () == totalLevelEnergy) {
			Destroy (gameObject);		
		}
	
	}
}
