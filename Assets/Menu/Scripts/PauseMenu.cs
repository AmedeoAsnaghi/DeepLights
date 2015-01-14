using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

	private bool pauseGame;
	private bool showGUI;
	private GameObject pauseGUI;
	private Animator anGui;
	private int selection;
	private Animator anLeft, anMiddle, anRight;
	private GameManager gm;
	// Use this for initialization
	void Start () {
		pauseGame = false;
		showGUI = false;
		pauseGUI = GameObject.Find("PausedGUI");
		anGui = pauseGUI.GetComponent<Animator> ()as Animator;
		anLeft = (GameObject.Find ("Left")).GetComponent<Animator> () as Animator;
		anMiddle = (GameObject.Find ("Middle")).GetComponent<Animator> () as Animator;
		anRight = (GameObject.Find ("Right")).GetComponent<Animator> () as Animator;
		gm = GameObject.Find ("Controller").GetComponent<GameManager> () as GameManager;

		//pauseGUI.guiTexture.transform.position = new Vector3(0.35f,0.5f,0);
		//pauseGUI.guiTexture.transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		if((Input.GetKeyDown("p"))||(Input.GetButtonDown("Pause")))
		{
			pauseGame = !pauseGame;
			
			if(pauseGame == true)
			{
				selection = 0;
				anGui.SetTrigger("startPause");
				Time.timeScale = 0.001f;
				pauseGame = true;
				updateSelection();
			}
		}
		if(pauseGame){
			if(Input.GetKeyDown(KeyCode.LeftArrow)){
				if(selection == -1)
					selection = 1;
				else
					selection--;
				updateSelection();

			}
			else if(Input.GetKeyDown(KeyCode.RightArrow)){
				if(selection == 1)
					selection = -1;
				else
					selection++;
				updateSelection();
			}
			else if (Input.GetKeyDown(KeyCode.Return)){
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

	IEnumerator press() {
		yield return new WaitForSeconds (0.0005f);
		if(selection==0){
			anMiddle.SetInteger("hover",1);
			anGui.ResetTrigger("startPause");
			anGui.SetTrigger("endPause");
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
	}
	
}
