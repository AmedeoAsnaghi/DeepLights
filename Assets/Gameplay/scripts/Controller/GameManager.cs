﻿
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public int currentJellyFishLife = 100;
	private bool invincible = false;
	private bool canGetLife = true;
	private bool canIncreaseScore = false;
	private GameObject jellyFish;
	private Light[] lightVisible;
	private Camera mainCamera;
	private float oldSizeCamera;
	private float lightRange;
	private bool lightImpulse;
	private Score sc;
	
	private Animator anImpulse;
	private Animator anWarning;


	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
	}

	// Use this for initialization
	void Start () {
		jellyFish = GameObject.FindWithTag ("Player");
		sc = (GameObject.FindGameObjectWithTag ("Score")).GetComponent<Score> ()as Score;
		lightVisible = jellyFish.GetComponentsInChildren<Light> (false) as Light[];
		lightRange = lightVisible[0].range;
		mainCamera = Camera.main;
		lightImpulse = false;
		oldSizeCamera = mainCamera.orthographicSize;
		anImpulse = (GameObject.FindGameObjectWithTag ("impulse")).GetComponent<Animator> () as Animator;
		anWarning = (GameObject.FindGameObjectWithTag ("warning")).GetComponent<Animator>() as Animator;
	}
	
	// Update is called once per frame
	void Update () {
		if (lightImpulse) {
			if(mainCamera.orthographicSize < 10f){
				mainCamera.orthographicSize+=0.1f;
				lightVisible[0].range += 0.1f;
			}
			if (lightVisible[0].range < lightRange + 10f){
				lightVisible[0].range+=0.1f;
			}
		}
		else{
			if (mainCamera.orthographicSize > oldSizeCamera){
				mainCamera.orthographicSize-=0.1f;
			}
			if (lightVisible[0].range > lightRange){
				lightVisible[0].range -= 0.01f;
			}
		}
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
			anWarning.SetBool ("warning", true);
			invincible = true;
			StartCoroutine(WaitInvulnerability(2));
		}

		return currentJellyFishLife;
	}
	
	public int increaseLife(int value) {
		if (canGetLife) {
			currentJellyFishLife = currentJellyFishLife + value;
			if (currentJellyFishLife > 100) {
					currentJellyFishLife = 100;
			}
			canGetLife = false;
			StartCoroutine (WaitForLife (0.5f));
		}
		return currentJellyFishLife;
	}
	
	public int showCurrentLife() {
		return currentJellyFishLife;
	}

	public void showUpdatedScore() {
		if (canIncreaseScore) {
			sc.showUpdatedScore ();
			canIncreaseScore = false;
		}
		StartCoroutine (WaitToUpdateScore (0.05f));
	}
	
	public bool isDead() {
		return currentJellyFishLife <= 0;
	}

	IEnumerator WaitInvulnerability(float delay){
		yield return new WaitForSeconds(delay);
		anWarning.SetBool ("warning", false);
		invincible = false;
	}

	IEnumerator WaitForLife(float delay){
		yield return new WaitForSeconds(delay);
		canGetLife = true;
	}

	IEnumerator WaitToUpdateScore(float delay){
		yield return new WaitForSeconds(delay);
		canIncreaseScore = true;
		showUpdatedScore ();
	}

	//--------------------------------------- SUPER POWERS ---------------------------------------
	public void doLightImpulse(){
		if (!lightImpulse) {
			lightImpulse = true;
			anImpulse.SetBool ("impulsePower", true);
			StartCoroutine (WaitLightImpulse (3f, oldSizeCamera));
		}

	}

	IEnumerator WaitLightImpulse(float delay, float oldSizeCamera){
		yield return new WaitForSeconds(delay);
		lightImpulse = false;
		anImpulse.SetBool ("impulsePower", false);
		//returnLightImpulse(oldSizeCamera);
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
	