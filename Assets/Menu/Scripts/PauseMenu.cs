using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	private bool pauseGame;
	private bool showGUI;
	private GameObject pauseGUI;
	// Use this for initialization
	void Start () {
		pauseGame = false;
		showGUI = false;
		pauseGUI = GameObject.Find("PausedGUI");
		pauseGUI.guiTexture.transform.position = new Vector3(0.35f,0.5f,0);
		pauseGUI.guiTexture.transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("p"))
		{
			pauseGame = !pauseGame;
			
			if(pauseGame == true)
			{
				Time.timeScale = 0;
				pauseGame = true;
				showGUI = true;
			}
		}
		
		if(pauseGame == false)
		{
			Time.timeScale = 1;
			pauseGame = false;
			showGUI = false;
		}
		
		if(showGUI == true)
		{

			pauseGUI.guiTexture.enabled = true;  

		}
		
		else
		{
			pauseGUI.guiTexture.enabled = false;  
		}
	}
}
