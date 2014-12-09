
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public int currentJellyFishLife = 100;
	private bool invincible = false;
	private GameObject jellyFish;
	private Light[] lightVisible;

	private bool lightImpulse;

	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
	}

	// Use this for initialization
	void Start () {
		jellyFish = GameObject.FindWithTag ("Player");
		lightVisible = jellyFish.GetComponentsInChildren<Light> (false) as Light[];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void usePower(int type){
		switch(type)
		{
			case utils.LIGHT_IMPULSE:
				doLightImpulse();
				break;


		}

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

	//--------------------------------------- SUPER POWERS ---------------------------------------
	public void doLightImpulse(){
		lightImpulse = true;
		float oldSizeCamera = camera.orthographicSize;
		while (camera.orthographicSize > 2f) {
			camera.orthographicSize -= 0.2f;		
		}
		StartCoroutine(WaitLightImpulse());
		while (lightImpulse) {
				
		}
		lightVisible[0].range += 10f;
		while(camera.orthographicSize < oldSizeCamera){
			camera.orthographicSize += 0.2f;	
		}
	}

	IEnumerator WaitLightImpulse(){
		yield return new WaitForSeconds(3);
		lightImpulse = false;
	}
	//--------------------------------------- END SUPER POWERS ---------------------------------------

}
	