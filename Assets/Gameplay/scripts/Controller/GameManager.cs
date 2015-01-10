
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
	private bool lightImpulseCamera;
	private Score sc;
	private int level;
	private bool canChangeLevel;
	
	private Animator anImpulse;
	private Animator anWarning;
	private Animator gameOverAnimator;

	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
	}

	// Use this for initialization
	void Start () {
		level = 1;
		this.setManager ();
	}
	
	// Update is called once per frame
	void Update () {
		if (level == 0) {
			DestroyImmediate(gameObject);		
		}
		if (mainCamera == null) {
			this.setManager();		
		}
		checkForLightImpulse ();


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
			if (isDead()) {
				Time.timeScale = 0;
				(GameObject.FindGameObjectWithTag ("buttonRestart")).GetComponent <Button> ().interactable = true;
				gameOverAnimator.SetTrigger("GameOver");

			}
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

	//-----------------CHECK POWER USAGE AND ACT------------------
	void checkForLightImpulse ()
	{	
		if (lightImpulseCamera) {
			if (mainCamera.orthographicSize < 10f) {
				mainCamera.orthographicSize += 0.1f;
				lightVisible [0].range += 0.1f;
			}
			if (lightVisible [0].range < lightRange + 10f) {
				lightVisible [0].range += 0.1f;
			}
		}
		else {
			if (mainCamera.orthographicSize > oldSizeCamera) {
				mainCamera.orthographicSize -= 0.1f;
			}
			if (lightVisible [0].range > lightRange) {
				lightVisible [0].range -= 0.01f;
			}
		}
	}

	//--------------------------------------- SUPER POWERS ---------------------------------------
	public void doLightImpulse(){
		if (!lightImpulse) {
			lightImpulseCamera = true;
			lightImpulse = true;
			anImpulse.SetBool ("impulsePower", true);
			StartCoroutine (WaitLightRestart(10f));
			StartCoroutine (WaitLightImpulse (3f, oldSizeCamera));

		}

	}

	IEnumerator WaitLightImpulse(float delay, float oldSizeCamera){
		yield return new WaitForSeconds(delay);
		anImpulse.SetBool ("impulsePower", false);
		lightImpulseCamera = false;
		//returnLightImpulse(oldSizeCamera);
	}

	IEnumerator WaitLightRestart(float delay){
		yield return new WaitForSeconds(delay);
		lightImpulse = false;
	}

	public void returnLightImpulse(float oldSizeCamera){
		while(mainCamera.orthographicSize > oldSizeCamera){
			mainCamera.orthographicSize -= Mathf.Log10(mainCamera.orthographicSize)*0.001f;	
		}
		lightVisible[0].range -= 10f;

	}
	public bool CanPulse(){
		return !lightImpulseCamera;
	}

	public bool CanDoLightImpulse(){
		return !lightImpulse;
		}
	//--------------------------------------- END SUPER POWERS ---------------------------------------


	//--------------------------------------- CHANGE LEVEL -------------------------------------------
	public void restartLevel() {
		resetStatus();
		Application.LoadLevel (level);
	}

	public void loadMenu() {
		level = 0;
		Application.LoadLevel (level);

	}

	public void changeLevel() {
		if (canChangeLevel) {
			mainCamera = null;
			canChangeLevel = false;
			level += level;
			Application.LoadLevel (level);
			StartCoroutine (WaitLevel(10f));
		}
	}

	IEnumerator WaitLevel(float delay){
		yield return new WaitForSeconds(delay);
		canChangeLevel = false;
	}

	public void resetStatus(){
		currentJellyFishLife = 100;
	}

	//--------------------------------------- END CHANGE LEVEL ---------------------------------------
	public void setManager() {
		if (level == 0) {
			DestroyImmediate(gameObject);
				}
		canChangeLevel = true;
		jellyFish = GameObject.FindWithTag ("Player");
		sc = (GameObject.FindGameObjectWithTag ("Score")).GetComponent<Score> ()as Score;
		lightVisible = jellyFish.GetComponentsInChildren<Light> (false) as Light[];
		lightRange = lightVisible[0].range;
		mainCamera = Camera.main;
		lightImpulse = false;
		lightImpulseCamera = false;
		oldSizeCamera = mainCamera.orthographicSize;
		anImpulse = (GameObject.FindGameObjectWithTag ("impulse")).GetComponent<Animator> () as Animator;
		anWarning = (GameObject.FindGameObjectWithTag ("warning")).GetComponent<Animator>() as Animator;
		gameOverAnimator = (GameObject.Find ("GUICanvas")).GetComponent<Animator>() as Animator;
	}
}
	