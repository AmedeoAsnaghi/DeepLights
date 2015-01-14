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
	private lightPulse jellyfishLight;
	private ColorManager jellyfishColorManager;
	private Camera mainCamera;
	private float oldSizeCamera;
	private float lightRange;
	private bool lightImpulse;
	private bool lightImpulseCamera;
	private bool barrier;
	private BlueScore bScore;
	private RedScore rScore;
	private YellowScore yScore;
	private int level;
	private bool canChangeLevel;
	private bool canUpdateImage;

	private int totalBlueEnergyCollected;
	private int totalYellowEnergyCollected;
	private int totalRedEnergyCollected;

	private Animator anImpulse;
	private Animator anWarning;
	private Animator anBarrier;
	private Animator gameOverAnimator;
	private Animator anRedTimer;
	private Animator anBlueTimer;
	private Animator anYellowTimer;

	Texture2D[] activationRedPower;
	Texture2D[] activationBluePower;
	Texture2D[] activationYellowPower;

	public Color red;
	public  Color lightRed;
	public  Color blue;
	public  Color lightBlue;

	private CircleCollider2D barrierCollider;

	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
	}

	// Use this for initialization
	void Start () {
		level = 1;
		totalBlueEnergyCollected = 0;
		totalRedEnergyCollected = 0;
		totalYellowEnergyCollected = 0;

		object[] objects = Resources.LoadAll("BluePowerActivation",typeof(Texture2D));
		Debug.Log (objects[0]);
		this.activationBluePower = new Texture2D[objects.Length];
		for(int i=0; i < objects.Length;i++)  
		{  
			this.activationBluePower[i] = (Texture2D)objects[i];
		}

		objects = Resources.LoadAll("RedPowerActivation", typeof(Texture2D));
		Debug.Log (objects[0]);
		this.activationRedPower = new Texture2D[objects.Length];
		for(int i=0; i < objects.Length;i++)  
		{  
			this.activationRedPower[i] = (Texture2D)objects[i];  
		}

		objects = Resources.LoadAll("YellowPowerActivation", typeof(Texture2D));
		Debug.Log (objects[0]);
		this.activationYellowPower = new Texture2D[objects.Length];
		for(int i=0; i < objects.Length;i++)  
		{  
			this.activationYellowPower[i] = (Texture2D)objects[i];  
		}

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
				(GameObject.FindGameObjectWithTag ("buttonMenu2")).GetComponent <Button> ().interactable = true;
				jellyfishColorManager.updateColorJellyfish(Color.black);
				(jellyFish.GetComponent<movement>() as movement).enabled = false;
				gameOverAnimator.SetTrigger("GameOver");

			}
			invincible = true;
			jellyfishLight.updateJellyfishLight();
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
			jellyfishLight.updateJellyfishLight();
			StartCoroutine (WaitForLife (1f));
		}
		return currentJellyFishLife;
	}
	
	public int showCurrentLife() {
		return currentJellyFishLife;
	}

	public void blueSphereCatched(){
		if (canUpdateImage) {
			Image blueImage = GameObject.FindGameObjectWithTag ("blueTimer").GetComponent<Image> ();
			Texture2D texture = activationBluePower [totalBlueEnergyCollected];
			blueImage.overrideSprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));
			totalBlueEnergyCollected += 1;
			canUpdateImage = false;
			StartCoroutine(WaitUpdateImage(1f));
		}
		Debug.Log (totalBlueEnergyCollected);
	}

	public void yellowSphereCatched() {
		if (canUpdateImage) {
			Image yellowImage = GameObject.FindGameObjectWithTag ("yellowTimer").GetComponent<Image> ();
			Texture2D texture = activationYellowPower [totalYellowEnergyCollected];
			yellowImage.overrideSprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));
			totalYellowEnergyCollected += 1;
			canUpdateImage = false;
			StartCoroutine(WaitUpdateImage(1f));
		}
	}

	public void redSphereCatched(){
		if (canUpdateImage) {
			Image redImage = GameObject.FindGameObjectWithTag ("redTimer").GetComponent<Image> ();
			Texture2D texture = activationRedPower [totalRedEnergyCollected];
			redImage.overrideSprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));
			totalRedEnergyCollected += 1;
			canUpdateImage = false;
			StartCoroutine(WaitUpdateImage(1f));
		}
	}

	public void showUpdatedScore() {
		if (canIncreaseScore) {
			bScore.showUpdatedScore ();
			rScore.showUpdatedScore ();
			yScore.showUpdatedScore ();
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
		if (!lightImpulse && totalRedEnergyCollected>=15) {
			lightImpulseCamera = true;
			lightImpulse = true;

			//update color
			jellyfishLight.updateJellyfishLightColor(lightRed);
			jellyfishColorManager.updateColorJellyfish(red);

			anImpulse.SetBool ("impulsePower", true);

			StartCoroutine (WaitLightRestart(15.2f));
			StartCoroutine (WaitLightImpulse (3f, oldSizeCamera));
		}
	}

	public void doBarrier(){
		if (!barrier && totalBlueEnergyCollected>=15) {
			barrier = true;
			invincible = true;

			//update color
			jellyfishLight.updateJellyfishLightColor(lightBlue);
			jellyfishColorManager.updateColorJellyfish(blue);

			anBarrier.SetBool("barrierPower", true);

			barrierCollider.radius = 2f;

			StartCoroutine(WaitBarrierRestart (15.2f));
			StartCoroutine(WaitBarrierColliderReset(3f));
		}
	}
	
	IEnumerator WaitBarrierColliderReset(float delay){
		yield return new WaitForSeconds (delay);
		invincible = false;
		barrierCollider.radius = 0;
		anBarrier.SetBool ("barrierPower", false);
		anBlueTimer.SetTrigger("barrierUsed");
	}
	IEnumerator WaitBarrierRestart(float delay){
		yield return new WaitForSeconds (delay);
		barrier = false;
	}

	IEnumerator WaitLightImpulse(float delay, float oldSizeCamera){
		yield return new WaitForSeconds(delay);
		anImpulse.SetBool ("impulsePower", false);
		lightImpulseCamera = false;
		anRedTimer.SetTrigger ("lightImpulseUsed");
		//returnLightImpulse(oldSizeCamera);
	}

	IEnumerator WaitLightRestart(float delay){
		yield return new WaitForSeconds(delay);
		lightImpulse = false;
	}

	IEnumerator WaitUpdateImage(float delay){
		yield return new WaitForSeconds(delay);
		canUpdateImage = true;
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
			level += 1;
			Application.LoadLevel (level);
			StartCoroutine (WaitLevel(10f));
		}
	}

	IEnumerator WaitLevel(float delay){
		yield return new WaitForSeconds(delay);
		canChangeLevel = true;
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
		rScore = (GameObject.FindGameObjectWithTag ("RedScore")).GetComponent<RedScore> ()as RedScore;
		bScore = (GameObject.FindGameObjectWithTag ("BlueScore")).GetComponent<BlueScore> ()as BlueScore;
		yScore = (GameObject.FindGameObjectWithTag ("YellowScore")).GetComponent<YellowScore> ()as YellowScore;
		lightVisible = jellyFish.GetComponentsInChildren<Light> (false) as Light[];
		lightRange = lightVisible[0].range;
		jellyfishLight = jellyFish.GetComponentInChildren<lightPulse> () as lightPulse;
		jellyfishColorManager = jellyFish.GetComponent<ColorManager> () as ColorManager;
		mainCamera = Camera.main;
		lightImpulse = false;
		barrier = false;
		lightImpulseCamera = false;
		oldSizeCamera = mainCamera.orthographicSize;
		anImpulse = (GameObject.FindGameObjectWithTag ("impulse")).GetComponent<Animator> () as Animator;
		anWarning = (GameObject.FindGameObjectWithTag ("warning")).GetComponent<Animator>() as Animator;
		anBarrier = (GameObject.FindGameObjectWithTag ("barrier")).GetComponent<Animator> () as Animator;
		anRedTimer = (GameObject.FindGameObjectWithTag ("redTimer")).GetComponent<Animator> () as Animator;
		anBlueTimer = (GameObject.FindGameObjectWithTag ("blueTimer")).GetComponent<Animator> () as Animator;
		anYellowTimer = (GameObject.FindGameObjectWithTag ("yellowTimer")).GetComponent<Animator> () as Animator;
		gameOverAnimator = (GameObject.Find ("GUICanvas")).GetComponent<Animator>() as Animator;
		barrierCollider = (GameObject.FindGameObjectWithTag ("barrier")).GetComponent<CircleCollider2D> () as CircleCollider2D;
		canUpdateImage = true;

		Image blueImage = GameObject.FindGameObjectWithTag ("blueTimer").GetComponent<Image> ();
		Texture2D texture = activationBluePower [totalBlueEnergyCollected];
		blueImage.overrideSprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));

		Image yellowImage = GameObject.FindGameObjectWithTag ("yellowTimer").GetComponent<Image> ();
		texture = activationYellowPower [totalYellowEnergyCollected];
		yellowImage.overrideSprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));

		Image redImage = GameObject.FindGameObjectWithTag ("redTimer").GetComponent<Image> ();
		texture = activationRedPower [totalRedEnergyCollected];
		redImage.overrideSprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));


	}
}
	