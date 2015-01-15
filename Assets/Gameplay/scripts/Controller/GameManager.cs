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
	public float barrierRadius = 2f;
	private bool flash;
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
	private Animator anFlash;
	private Animator anLoading;
	private Animator anRedTimer;
	private Animator anBlueTimer;
	private Animator anYellowTimer;

	Texture2D[] activationRedPower;
	Texture2D[] activationBluePower;
	Texture2D[] activationYellowPower;

	//colors
	private Color currentColor;
	private Color currentLightColor;
	public Color red;
	public  Color lightRed;
	public  Color blue;
	public  Color lightBlue;
	public Color yellow;
	public Color lightYellow;
	public Color grey;
	public Color lightGrey;


	private CircleCollider2D barrierCollider;

	private GameOverMenu gameOver;
	private bool pause;

	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
	}

	// Use this for initialization
	void Start () {
		level = 1;
		totalBlueEnergyCollected = 0;
		totalRedEnergyCollected = 0;
		totalYellowEnergyCollected = 0;
		pause = false;

		//inizialize the colors of the jellyfish
		currentColor = grey;
		currentLightColor = lightGrey;

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

	public void setPause (bool p){
		pause = p;
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
		if ((!invincible)&&(!pause)) { 
			currentJellyFishLife = currentJellyFishLife - value;
			anWarning.SetBool ("warning", true);
			if (isDead()) {
				jellyfishColorManager.updateColorJellyfish(Color.black);
				(jellyFish.GetComponent<movement>() as movement).enabled = false;
				gameOver.gameover();

			}
			invincible = true;
			jellyfishLight.updateJellyfishLight();
			StartCoroutine(WaitInvulnerability(2));
		}

		return currentJellyFishLife;
	}
	
	public int increaseLife(int value) {
		if ((canGetLife)&&(!pause)) {
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
		if ((canUpdateImage)&&(!pause)) {
			Image blueImage = GameObject.FindGameObjectWithTag ("blueTimer").GetComponent<Image> ();
			Texture2D texture = activationBluePower [totalBlueEnergyCollected];
			blueImage.sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));
			totalBlueEnergyCollected += 1;
			canUpdateImage = false;
			StartCoroutine(WaitUpdateImage(1f));
		}
		Debug.Log (totalBlueEnergyCollected);
	}

	public void yellowSphereCatched() {
		if ((canUpdateImage)&&(!pause)) {
			Image yellowImage = GameObject.FindGameObjectWithTag ("yellowTimer").GetComponent<Image> ();
			Texture2D texture = activationYellowPower [totalYellowEnergyCollected];
			yellowImage.sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));
			totalYellowEnergyCollected += 1;
			canUpdateImage = false;
			StartCoroutine(WaitUpdateImage(1f));
		}
	}

	public void redSphereCatched(){
		if ((canUpdateImage)&&(!pause)) {
			Image redImage = GameObject.FindGameObjectWithTag ("redTimer").GetComponent<Image> ();
			Texture2D texture = activationRedPower [totalRedEnergyCollected];
			redImage.sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));
			totalRedEnergyCollected += 1;
			canUpdateImage = false;
			StartCoroutine(WaitUpdateImage(1f));
		}
	}

	public void showUpdatedScore() {
		if ((canIncreaseScore)&&(!pause)) {
			bScore.showUpdatedScore ();
			rScore.showUpdatedScore ();
			yScore.showUpdatedScore ();
			canIncreaseScore = false;
		}
		StartCoroutine (WaitToUpdateScore (0.05f));
	}

	public int getCollectedEnergy() {
		return rScore.getCollectedEnergy () + bScore.getCollectedEnergy () + yScore.getCollectedEnergy ();
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
		if (!lightImpulse /*&& totalRedEnergyCollected>=15*/&& !pause) {
			lightImpulseCamera = true;
			lightImpulse = true;

			//update color
			jellyfishLight.updateJellyfishLightColor(lightRed);
			jellyfishColorManager.updateColorJellyfish(red);
			currentColor = red;
			currentLightColor = lightRed;

			anImpulse.SetBool ("impulsePower", true);

			StartCoroutine (WaitLightRestart(15.2f));
			StartCoroutine (WaitLightImpulse (3f, oldSizeCamera));
		}
	}

	public void doBarrier(){
		if (!barrier /*&& totalBlueEnergyCollected>=15*/&& !pause) {
			barrier = true;
			invincible = true;

			//update color
			jellyfishLight.updateJellyfishLightColor(lightBlue);
			jellyfishColorManager.updateColorJellyfish(blue);
			currentColor = blue;
			currentLightColor = lightBlue;

			anBarrier.SetBool("barrierPower", true);

			barrierCollider.radius = barrierRadius;

			StartCoroutine(WaitBarrierRestart (15.2f));
			StartCoroutine(WaitBarrierColliderReset(3f));
		}
	}

	public void doFlash(){
		if (!flash /* && totalYellowEnergyCollected>=15*/&& !pause) {
			flash = true;

			//update color
			jellyfishLight.updateJellyfishLightColor(lightYellow);
			jellyfishColorManager.updateColorJellyfish(yellow);
			currentColor = yellow;
			currentLightColor = lightYellow;

			anFlash.SetBool("flashPower",true);

			StartCoroutine(WaitFlashRestart(15.2f));
			StartCoroutine(WaitFlashEnd(0.5f));
		}
	}

	IEnumerator WaitFlashRestart(float delay){
		yield return new WaitForSeconds (delay);
		flash = false;
	}

	IEnumerator WaitFlashEnd(float delay){
		yield return new WaitForSeconds (delay);
		anFlash.SetBool ("flashPower", false);
		anYellowTimer.SetTrigger ("stunUsed");
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
			anLoading.SetTrigger("startLoading");
			canChangeLevel = false;
			StartCoroutine(nextLevel(3f));
			StartCoroutine (WaitLevel(10f));
		}
	}

	IEnumerator nextLevel(float delay){
		yield return new WaitForSeconds(delay);
		mainCamera = null;
		level += 1;
		Application.LoadLevel (level);
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
		jellyfishLight.updateJellyfishLightColor (currentLightColor);
		jellyfishColorManager.updateColorJellyfish (currentColor);
		mainCamera = Camera.main;
		lightImpulse = false;
		barrier = false;
		flash = false;
		lightImpulseCamera = false;
		oldSizeCamera = mainCamera.orthographicSize;
		anImpulse = (GameObject.FindGameObjectWithTag ("impulse")).GetComponent<Animator> () as Animator;
		anWarning = (GameObject.FindGameObjectWithTag ("warning")).GetComponent<Animator>() as Animator;
		anBarrier = (GameObject.FindGameObjectWithTag ("barrier")).GetComponent<Animator> () as Animator;
		anFlash = (GameObject.FindGameObjectWithTag ("flash")).GetComponent<Animator> () as Animator;
		anRedTimer = (GameObject.FindGameObjectWithTag ("redTimer")).GetComponent<Animator> () as Animator;
		anBlueTimer = (GameObject.FindGameObjectWithTag ("blueTimer")).GetComponent<Animator> () as Animator;
		anYellowTimer = (GameObject.FindGameObjectWithTag ("yellowTimer")).GetComponent<Animator> () as Animator;
		anLoading = (GameObject.Find ("Loading")).GetComponent<Animator>() as Animator;
		barrierCollider = (GameObject.FindGameObjectWithTag ("barrier")).GetComponent<CircleCollider2D> () as CircleCollider2D;
		canUpdateImage = true;

		Image blueImage = GameObject.FindGameObjectWithTag ("blueTimer").GetComponent<Image> ();
		Texture2D texture = activationBluePower [totalBlueEnergyCollected];
		blueImage.sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));

		Image yellowImage = GameObject.FindGameObjectWithTag ("yellowTimer").GetComponent<Image> ();
		texture = activationYellowPower [totalYellowEnergyCollected];
		yellowImage.sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));

		Image redImage = GameObject.FindGameObjectWithTag ("redTimer").GetComponent<Image> ();
		texture = activationRedPower [totalRedEnergyCollected];
		redImage.sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));

		gameOver = (GameObject.Find ("Controller")).GetComponent<GameOverMenu> () as GameOverMenu;
		if (level != 0)
			anLoading.SetTrigger ("stopLoading");
	}
}
	