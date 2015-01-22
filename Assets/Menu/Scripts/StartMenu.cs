using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {
	private GameObject pauseGUI;
	private Animator anGui;
	private Camera mainCamera;
	private bool canPress;

	// Use this for initialization
	void Start () {
		this.setManager();
	}
	
	// Update is called once per frame
	void Update () {
		if (mainCamera == null) {
			this.setManager();		
		}

		if (((Input.GetKeyDown (KeyCode.Return)) || (Input.GetButton ("Dash"))) && canPress) {
			anGui.SetTrigger ("startGame");
			canPress = false;
			StartCoroutine(LoadLevelCoroutine());
		}
		
			StartCoroutine(press ());
		}


	void setManager() {
		mainCamera = Camera.main;
		pauseGUI = GameObject.Find ("MenuGUI");
		anGui = pauseGUI.GetComponent<Animator> () as Animator;
		canPress = true;
	}

	IEnumerator press() {
		yield return new WaitForSeconds (0.5f);
		canPress = true;
	}

	IEnumerator LoadLevelCoroutine() {
		yield return new WaitForSeconds (1f);
		Application.LoadLevel (2);
	}

}
