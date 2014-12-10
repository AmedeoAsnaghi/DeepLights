using UnityEngine;
using System.Collections;

public class guiStatus : MonoBehaviour {

	private GameManager gameManager;
	float health;
	public Texture2D currentLife;
	public float x = 0;
	public float y = 0.9f;
	GUITexture gt;

	void Start () {
		GameObject controller = GameObject.Find("Controller");
		gameManager = controller.GetComponent<GameManager> () as GameManager;
		health = gameManager.showCurrentLife();
		gt = gameObject.GetComponent<GUITexture> () as GUITexture;
		gt.transform.position = new Vector3(x,y,0);
		gt.transform.localScale = Vector3.zero;
		gt.texture = currentLife;
		gt.pixelInset = new Rect(0, 0, Screen.width/2,Screen.width/20);
	}


	void Update () {
		health = gameManager.showCurrentLife();
		gt.pixelInset = new Rect(0, 0, health*5,Screen.width/20);
	}
}
