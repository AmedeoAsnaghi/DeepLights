using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverButtonsScript : MonoBehaviour {

	public void restartLevel () {
		Debug.Log ("RESTART LEVEL!!!!");
		GameObject controller = GameObject.Find("Controller");
		GameManager gameManager = controller.GetComponent<GameManager> () as GameManager;
		gameManager.restartLevel ();
	}
	public void menu(){
		GameObject controller = GameObject.Find("Controller");
		GameManager gameManager = controller.GetComponent<GameManager> () as GameManager;
		gameManager.loadMenu ();
	}
}
