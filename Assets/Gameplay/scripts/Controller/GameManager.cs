using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public int currentJellyFishLife = 100;
	private bool invincible = false;

	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int decreaseLife(int value) {
		if (!invincible) { 
			currentJellyFishLife = currentJellyFishLife - value;
			invincible = true;
			StartCoroutine(WaitInvulnerability());
		}

		return currentJellyFishLife;
	}
	
	public int increaseLife(int value) {
		currentJellyFishLife = currentJellyFishLife + value;
		if (currentJellyFishLife > 100) {
			currentJellyFishLife = 100;
		}
		return currentJellyFishLife;
	}
	
	public int showCurrentLife() {
		return currentJellyFishLife;
	}
	
	public bool isDead() {
		return currentJellyFishLife <= 0;
	}

	IEnumerator WaitInvulnerability(){
		yield return new WaitForSeconds(2);
		invincible = false;
	}
}
	