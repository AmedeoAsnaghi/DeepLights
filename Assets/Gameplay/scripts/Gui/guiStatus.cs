using UnityEngine;
using System.Collections;

public class guiStatus : MonoBehaviour {

	private GameManager gameManager;
	float health;
	public Texture2D currentLife;
	public Texture2D maxLife;
	public Material mat;
	float x = 0;
	float y = 0;
	public float w;
	public float h;
	void Start () {
		GameObject controller = GameObject.Find("Controller");
		gameManager = controller.GetComponent<GameManager> () as GameManager;
		health = gameManager.showCurrentLife();
	}
	void OnGUI(){
		Rect box = new Rect (x, y, 100 * 3f, h);
		Graphics.DrawTexture(box, maxLife, mat);

		Rect box1 = new Rect(x, y, health * 3f, h);
		Graphics.DrawTexture(box1, currentLife, mat);
	}
	void Update () {
		health = gameManager.showCurrentLife();
	}
}
