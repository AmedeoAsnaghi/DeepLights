
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public int currentJellyFishLife = 100;
	private bool invincible = false;
	private GameObject jellyFish;
	private Light[] lightVisible;
	private Camera mainCamera;

	private bool lightImpulse;

	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
	}

	// Use this for initialization
	void Start () {
		jellyFish = GameObject.FindWithTag ("Player");
		lightVisible = jellyFish.GetComponentsInChildren<Light> (false) as Light[];
		mainCamera = Camera.main;
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
		float oldSizeCamera = mainCamera.orthographicSize;
		while (mainCamera.orthographicSize < 10f) {
			mainCamera.orthographicSize += Mathf.Log10(mainCamera.orthographicSize)*0.001f;		
		}
		StartCoroutine(WaitLightImpulse(3f,oldSizeCamera));
		//while (lightImpulse) {}

		lightVisible[0].range += 10f;
	}

	IEnumerator WaitLightImpulse(float delay, float oldSizeCamera){
		yield return new WaitForSeconds(delay);
		returnLightImpulse(oldSizeCamera);
	}

	public void returnLightImpulse(float oldSizeCamera){
		while(mainCamera.orthographicSize > oldSizeCamera){
			mainCamera.orthographicSize -= Mathf.Log10(mainCamera.orthographicSize)*0.001f;	
		}
		lightVisible[0].range -= 10f;
		lightImpulse = false;
	}
	public bool CanPulse(){
		return !lightImpulse;
	}
	//--------------------------------------- END SUPER POWERS ---------------------------------------

}
	