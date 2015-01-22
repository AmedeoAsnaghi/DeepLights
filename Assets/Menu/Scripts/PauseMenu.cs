using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

	private bool pauseGame;
	private GameObject pauseGUI;
	private Animator anGui;
	private int selection;
	private Animator anLeft, anMiddle, anRight;
	private GameManager gm;
	private bool canChange;
	private Camera mainCamera;
	private AudioSource audioGui;
	private AudioSource ambientAudio;
	// Use this for initialization
	void Start () {
		setManager ();
	}
	
	// Update is called once per frame
	void Update () {

		if (mainCamera == null) {
			this.setManager();		
		}

		if((Input.GetKeyDown("p"))||(Input.GetButtonDown("Pause")))
		{
			pauseGame = !pauseGame;
			
			if(pauseGame == true)
			{
				selection = 0;
				gm.setPause(true);
				canChange = true;
				anGui.SetTrigger("startPause");
				Time.timeScale = 0.001f;
				ambientAudio.audio.Stop ();
				pauseGame = true;
				updateSelection();
			}
			else if(!pauseGame){
				selection = 0;
				anMiddle.SetTrigger("press");
				pauseGame = false;
				gm.setPause(false);
				updateSelection();
				StartCoroutine(press ());
			}
		}
		if(pauseGame){
			if(canChange){
				if((Input.GetKeyDown(KeyCode.LeftArrow))||(Input.GetAxis("Horizontal")==-1)){
					if(selection == -1)
						selection = 1;
					else
						selection--;
					updateSelection();
					canChange=false;
					audioGui.audio.Play();
					StartCoroutine(waitAxis());

				}
				else if((Input.GetKeyDown(KeyCode.RightArrow))||(Input.GetAxis("Horizontal")==1)){
					if(selection == 1)
						selection = -1;
					else
						selection++;
					updateSelection();
					canChange=false;
					audioGui.audio.Play();
					StartCoroutine(waitAxis());
				}
			}
			if ((Input.GetKeyDown(KeyCode.Return))||(Input.GetButton("Dash"))){
				if(selection==0){
					anMiddle.SetTrigger("press");
					pauseGame = false;
				}
				else if (selection == 1){
					anRight.SetTrigger("press");
					pauseGame = false;
				}
				else {
					anLeft.SetTrigger("press");
					pauseGame = false;
				}
				StartCoroutine(press ());
			}
		}




	}

	private void updateSelection(){
		anLeft.SetInteger ("hover", selection);
		anRight.SetInteger ("hover", selection);
		anMiddle.SetInteger ("hover", selection);
	}

	IEnumerator waitAxis(){
		yield return new WaitForSeconds (0.0005f);
		canChange = true;
	}

	IEnumerator press() {
		yield return new WaitForSeconds (0.0005f);
		anGui.ResetTrigger("startPause");
		anGui.SetTrigger("endPause");
		if(selection==0){
			anMiddle.SetInteger("hover",1);
			Time.timeScale = 1;
		}
		else if (selection == 1){
			Time.timeScale = 1;
			gm.loadMenu();
		}
		else {
			Time.timeScale = 1;
			gm.restartLevel();
		}
		gm.setPause(false);
	}
	
	void setManager ()
	{
		mainCamera = Camera.main;
		pauseGame = false;
		pauseGUI = GameObject.Find ("PausedGUI");
		anGui = pauseGUI.GetComponent<Animator> () as Animator;
		audioGui = pauseGUI.GetComponent<AudioSource> () as AudioSource;
		anLeft = (GameObject.Find ("Left")).GetComponent<Animator> () as Animator;
		anMiddle = (GameObject.Find ("Middle")).GetComponent<Animator> () as Animator;
		anRight = (GameObject.Find ("Right")).GetComponent<Animator> () as Animator;
		gm = GameObject.Find ("Controller").GetComponent<GameManager> () as GameManager;
		ambientAudio = gm.GetComponent<AudioSource> () as AudioSource;
	}
}
