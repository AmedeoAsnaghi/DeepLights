using UnityEngine;
using System.Collections;

public class guiStatus : MonoBehaviour {

	private GUIText guiStats;
	private GameManager gameManager;
	Color rgb_color = new Color(255, 0, 0);
	// Use this for initialization
	void Start () {
		guiStats = GetComponent<GUIText> () as GUIText;
		GameObject controller = GameObject.Find("Controller");
		gameManager = controller.GetComponent<GameManager> () as GameManager;
	}
	
	// Update is called once per frame
	void Update () {
		//rgb_texture.Apply();
		//GUIStyle generic_style = new GUIStyle();
		//GUI.skin.box = generic_style;
		guiStats.text = (gameManager.showCurrentLife()).ToString();
		//GUI.Box(new Rect(0, 0, 0.001f * Screen.width * gameManager.showCurrentLife(),  0.1f * Screen.height), "LIFE",generic_style);
	}
}
