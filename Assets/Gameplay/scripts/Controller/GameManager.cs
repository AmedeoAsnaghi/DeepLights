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

	private bool unlockImpulse, unlockBarrier, unlockFlash;
	private bool mustYellow, mustBlue, mustRed;
	private int tryRed,tryBlue,tryYellow;
	
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

	private AudioSource barrierAudio;
	private AudioSource impulseAudio;
	private AudioSource flashAudio;


	private CircleCollider2D barrierCollider;

	private GameOverMenu gameOver;
	private bool pause;

	Text tutorialText;
	Animator anTutorial;


	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
	}

	// Use this for initialization
	void Start () {
		level = 2;
		totalBlueEnergyCollected = 0;
		totalRedEnergyCollected = 0;
		totalYellowEnergyCollected = 0;
		pause = false;

		//set unlockPower 
		unlockFlash = false;
		unlockImpulse = false;
		unlockBarrier = false;
		mustRed = false;
		mustBlue = false;
		mustYellow = false;

		//inizialize the colors of the jellyfish
		currentColor = grey;
		currentLightColor = lightGrey;

		this.setManager ();
	}
	
	// Update is called once per frame
	void Update () {
		if (level == 0 || level == 1) {
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
		totalBlueEnergyCollected += 1;
	}

	public void yellowSphereCatched() {
		totalYellowEnergyCollected += 1;
	}

	public void redSphereCatched(){
		totalRedEnergyCollected += 1;
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

	//-----------------UNLOCK POWER-------------------------------
	public bool getMustYellow(){
		return mustYellow;
	}

	public bool getMustRed(){
		return mustRed;
	}

	public bool getMustBlue(){
		return mustBlue;
	}


	public void unlockImpulsePower(){
		if(!unlockImpulse){
			tutorialText.text = "You unlocked the Radar Power! \n Press the red button to Light the Dark";
			anTutorial.SetTrigger("showText");
			anRedTimer.SetTrigger ("impulseUnlocked");
			lightImpulseCamera = false;
			lightImpulse = false;
			tryRed++;
			if (tryRed == 1) {
				StartCoroutine (stopTime (0));
				StartCoroutine (waitText ());
				mustRed = true;
			}
		}
	}

	public void unlockFlashPower(){
		if(!unlockFlash){
			tutorialText.text = "You unlocked the Flash Power! \n Press the yellow button to Stun your enemies";
			anTutorial.SetTrigger("showText");
			anYellowTimer.SetTrigger ("flashUnlocked");
			tryYellow++;
			if (tryYellow == 1) {
				StartCoroutine (waitText ());
				StartCoroutine (stopTime (2));
				mustYellow = true;
			}
		}
	}

	public void unlockBarrierPower(){
		if(!unlockBarrier){
			tutorialText.text = "You unlocked the Barrier Power! \n Press the blue button to Protect yourself";
			anTutorial.SetTrigger("showText");
			anBlueTimer.SetTrigger ("barrierUnlocked");
			tryBlue++;
			if (tryBlue == 1) {
				StartCoroutine (waitText ());
				StartCoroutine (stopTime (1));
				mustBlue = true;
			}
		}
	}

	IEnumerator stopTime(int selection){
		yield return new WaitForSeconds(1f);
		if (selection == 0){
			unlockImpulse = true;
		}
		else if (selection == 1){
			unlockBarrier = true;
		}
		else if (selection == 2){
			unlockFlash = true;
		}
		Time.timeScale = 0.001f;
	}

	//------------------------------------------------------------

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
		if (tryRed>0) {
			Time.timeScale = 1;
			tryRed = -100;
			mustRed = false;
		}
		if (!lightImpulse && unlockImpulse && !pause) {
			lightImpulseCamera = true;
			lightImpulse = true;

			//update color
			jellyfishLight.updateJellyfishLightColor(lightRed);
			jellyfishColorManager.updateColorJellyfish(red);
			currentColor = red;
			currentLightColor = lightRed;

			anImpulse.SetBool ("impulsePower", true);
			anRedTimer.SetTrigger("greyTimer");
			impulseAudio.Play();

			StartCoroutine (WaitLightRestart(15.2f));
			StartCoroutine (WaitLightImpulse (3f, oldSizeCamera));
		}
	}

	public void doBarrier(){
		if (tryBlue>0) {
			Time.timeScale = 1;
			tryBlue = -100;
			mustBlue = false;
		}
		if (!barrier && unlockBarrier && !pause) {
			barrier = true;
			invincible = true;

			//update color
			jellyfishLight.updateJellyfishLightColor(lightBlue);
			jellyfishColorManager.updateColorJellyfish(blue);
			currentColor = blue;
			currentLightColor = lightBlue;

			anBarrier.SetBool("barrierPower", true);
			anBlueTimer.SetTrigger("greyTimer");
			barrierAudio.Play();

			barrierCollider.radius = barrierRadius;

			StartCoroutine(WaitBarrierRestart (15.2f));
			StartCoroutine(WaitBarrierColliderReset(5f));
		}
	}

	public void doFlash(){
		if (tryYellow>0) {
			Time.timeScale = 1;
			tryYellow = -100;
			mustYellow = false;
		}
		if (!flash && unlockFlash && !pause) {
			flash = true;

			//update color
			jellyfishLight.updateJellyfishLightColor(lightYellow);
			jellyfishColorManager.updateColorJellyfish(yellow);
			currentColor = yellow;
			currentLightColor = lightYellow;

			anFlash.SetBool("flashPower",true);
			anYellowTimer.SetTrigger("greyTimer");
			flashAudio.Play ();

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
		level = 1;
		Application.LoadLevel (level);

	}

	public void changeLevel() {
		if (canChangeLevel) {
			anLoading.SetTrigger("startLoading");
			canChangeLevel = false;
			(jellyFish.GetComponent<Animator>()as Animator).SetBool("initLoading",true);
			StartCoroutine(nextLevel(1f));
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

	IEnumerator waitText() {
		yield return new WaitForSeconds (5f);
		anTutorial.ResetTrigger("showText");
		anTutorial.SetTrigger("hideText");
	}

	IEnumerator initTutorial() {
		yield return new WaitForSeconds (6.5f);
		tutorialText.text = "Move the Jelly with the left analog stick";
	}

	public void resetStatus(){
		currentJellyFishLife = 100;
	}

	public void endGame(){
		GameObject.Find ("Energy").SetActive (false);
		(GameObject.Find ("TheEnd").GetComponent<Animator> () as Animator).SetTrigger ("TheEnd");
		StartCoroutine (restartGame ());
	}

	IEnumerator restartGame(){
		yield return new WaitForSeconds (20f);
		loadMenu ();
	}

	//--------------------------------------- END CHANGE LEVEL ---------------------------------------
	public void setManager() {
		if (level == 0 || level == 1) {
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

		tutorialText = GameObject.Find ("TutorialText").GetComponent<Text> () as Text;
		anTutorial = GameObject.Find ("TutorialText").GetComponent<Animator> () as Animator;

		gameOver = (GameObject.Find ("Controller")).GetComponent<GameOverMenu> () as GameOverMenu;

		barrierAudio = GameObject.FindGameObjectWithTag ("barrier").GetComponent<AudioSource> () as AudioSource;
		impulseAudio = GameObject.FindGameObjectWithTag ("impulse").GetComponent<AudioSource> () as AudioSource;
		flashAudio = GameObject.FindGameObjectWithTag ("flash").GetComponent<AudioSource> () as AudioSource;

		if (unlockBarrier) {
			anBlueTimer.SetTrigger("barrierUnlocked");		
		}
		if (unlockFlash) {
			anYellowTimer.SetTrigger("flashUnlocked");		
		}
		if (unlockImpulse) {
			anRedTimer.SetTrigger("impulseUnlocked");		
		}

		tryBlue = 0;
		tryRed = 0;
		tryYellow = 0;
		if (level != 0)
			anLoading.SetTrigger ("stopLoading");

		if (level == 2) {
			StartCoroutine(initTutorial());
		}
	}
}
	