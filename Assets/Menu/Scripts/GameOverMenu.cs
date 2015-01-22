using UnityEngine;
using System.Collections;

public class GameOverMenu : MonoBehaviour {

	private bool gameOver;
	private Animator anGui;
	private int selection;
	private Animator anLeft, anRight;
	private GameManager gm;
	private bool canChange;
	private Camera mainCamera;

	// Use this for initialization
	void Start () {
		setParameters ();
	}
	
	// Update is called once per frame
	void Update () {
		if (mainCamera == null) {
			setParameters();		
		}

		if (gameOver) {
			if(canChange){
				if((Input.GetKeyDown(KeyCode.LeftArrow))||(Input.GetAxis("Horizontal")==-1)){
					selection = selection*-1;
					updateSelection();
					canChange=false;
					StartCoroutine(waitAxis());
				}
				else if((Input.GetKeyDown(KeyCode.RightArrow))||(Input.GetAxis("Horizontal")==1)){
					selection = selection*-1;
					updateSelection();
					canChange=false;
					StartCoroutine(waitAxis());
				}
			}
			if ((Input.GetKeyDown(KeyCode.Return))||(Input.GetButton("Dash"))){
				if (selection == 1){
					anRight.SetTrigger("press");
				}
				else {
					anLeft.SetTrigger("press");
				}
				StartCoroutine(press ());
			}
		}
	}

	private void updateSelection(){
		anLeft.SetInteger ("hover", selection);
		anRight.SetInteger ("hover", selection);
	}

	IEnumerator waitAxis(){
		yield return new WaitForSeconds (0.5f);
		canChange = true;
	}

	IEnumerator press() {
		yield return new WaitForSeconds (0.5f);
		anGui.ResetTrigger("startGameOver");
		anGui.SetTrigger("endGameOver");
		if (selection == 1){
			gm.loadMenu();
		}
		else {
			gm.restartLevel();
		}
	}

	public void gameover(){
		gameOver = true;
		canChange = true;
		selection = -1;
		anGui.SetTrigger ("startGameOver");
		updateSelection ();
	}

	void setParameters ()
	{
		mainCamera = Camera.main;
		gameOver = false;
		anGui = (GameObject.Find ("GameOverGUI")).GetComponent<Animator> () as Animator;
		anLeft = (GameObject.Find ("LeftGO")).GetComponent<Animator> () as Animator;
		anRight = (GameObject.Find ("RightGO")).GetComponent<Animator> () as Animator;
		gm = GameObject.Find ("Controller").GetComponent<GameManager> () as GameManager;
	}
}
