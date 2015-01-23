using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {
	private GameObject pauseGUI;
	private Animator anGui;
	private Camera mainCamera;
	private bool canPress;
	private bool canChange;
	private int selection;
	private Image backgroundImage;
	private Animator anStart, anCredits, anQuit, anShowCredits;
	private AudioSource audioGui;
	// Use this for initialization

	void Start () {
		this.setManager();
	}
	
	// Update is called once per frame
	void Update () {
		if (mainCamera == null) {
				this.setManager ();		
		}

		if (canChange) {
				if ((Input.GetKeyDown (KeyCode.UpArrow)) || (Input.GetAxis ("Vertical") == 1)) {
						if (selection == -1)
								selection = 1;
						else
								selection--;
						updateSelection ();
						canChange = false;
						audioGui.audio.Play ();
						StartCoroutine (waitAxis ());
		
				} else if ((Input.GetKeyDown (KeyCode.DownArrow)) || (Input.GetAxis ("Vertical") == -1)) {
						if (selection == 1)
								selection = -1;
						else
								selection++;
						updateSelection ();
						canChange = false;
						audioGui.audio.Play ();
						StartCoroutine (waitAxis ());
				}
		}
		if (((Input.GetKeyDown (KeyCode.Return)) || (Input.GetButtonDown ("Dash"))) && canPress && backgroundImage.color.a == 0) {
			if (selection == 0) {
				anCredits.SetTrigger ("clicked");
				StartCoroutine (WaitToOpenCredits ());	
			} else if (selection == 1) {
				anQuit.SetTrigger ("clicked");
				StartCoroutine (WaitToQuit ());	

			} else {
				anStart.SetTrigger ("clicked");
				canPress = false;
				StartCoroutine (WaitToStart ());
			}
			StartCoroutine (press ());
		}
	}

	void setManager() {
		mainCamera = Camera.main;
		pauseGUI = GameObject.Find ("MenuGUI");
		anGui = pauseGUI.GetComponent<Animator> () as Animator;
		audioGui = pauseGUI.GetComponent<AudioSource> () as AudioSource;
		anStart = (GameObject.Find ("ButtonStart")).GetComponent<Animator> () as Animator;
		anCredits = (GameObject.Find ("ButtonCredits")).GetComponent<Animator> () as Animator;
		anQuit = (GameObject.Find ("ButtonQuit")).GetComponent<Animator> () as Animator;
		canPress = true;
		canChange = true;
		selection = -1;
		anShowCredits = (GameObject.Find ("Credits")).GetComponent<Animator> () as Animator;
		backgroundImage = (GameObject.Find ("Image")).GetComponent<Image> () as Image;
	}

	private void updateSelection(){
		anStart.SetInteger ("hover", selection);
		anCredits.SetInteger ("hover", selection);
		anQuit.SetInteger ("hover", selection);
	}

	IEnumerator waitAxis(){
		yield return new WaitForSeconds (0.4f);
		canChange = true;
	}

	IEnumerator press() {
		yield return new WaitForSeconds (1f);
		canPress = true;
	}

	IEnumerator LoadLevelCoroutine() {
		yield return new WaitForSeconds (1f);
		Application.LoadLevel (2);
	}

	IEnumerator WaitToStart() {
		yield return new WaitForSeconds (1f);
		anGui.SetTrigger ("startGame");
		StartCoroutine(LoadLevelCoroutine());
	}

	IEnumerator WaitToQuit() {
		yield return new WaitForSeconds (1f);
		Application.Quit ();
	}

	IEnumerator WaitToOpenCredits() {
		yield return new WaitForSeconds (1f);
		anShowCredits.SetTrigger ("showCredits");
	}

}
