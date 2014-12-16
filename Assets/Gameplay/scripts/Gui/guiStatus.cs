using UnityEngine;
using System.Collections;

public class guiStatus : MonoBehaviour {

	private GameManager gameManager;
	private int count;
	private float health;
	private float oldHealth;
	private bool decreaseLife;
	private bool increaseLife;
	private int currentHealthFrame;
	private object[] objects;
	public float x = 0;
	public float y = 0.9f;
	GUITexture gt;
	Texture[] decreaseLifeTextures;
	Texture[] increaseLifeTextures;

	void Start () {
		GameObject controller = GameObject.Find("Controller");
		gameManager = controller.GetComponent<GameManager> () as GameManager;
		health = gameManager.showCurrentLife();
		oldHealth = health;
		gt = gameObject.GetComponent<GUITexture> () as GUITexture;
		gt.transform.position = new Vector3(x,y,0);
		gt.transform.localScale = Vector3.zero;

		this.objects = Resources.LoadAll("LifeDecrease", typeof(Texture)); 

		this.decreaseLifeTextures = new Texture[this.objects.Length];
		for(int i=0; i < objects.Length;i++)  
		{  
			this.decreaseLifeTextures[i] = (Texture)this.objects[i];  
		}   

		this.objects = Resources.LoadAll("LifeIncrease", typeof(Texture)); 
		
		this.increaseLifeTextures = new Texture[this.objects.Length];
		for(int i=0; i < objects.Length;i++)  
		{  
			this.increaseLifeTextures[i] = (Texture)this.objects[i];  
		}   

		gt.texture = this.decreaseLifeTextures[0];
		gt.pixelInset = new Rect(-70, -200, 400,400);
		currentHealthFrame = 0;

		decreaseLife = false;
		increaseLife = false;
	}


	void Update () {
		float deltaHealth = 0;
		health = gameManager.showCurrentLife();

		if (health != oldHealth) {
			deltaHealth = oldHealth-health;
			if (deltaHealth > 0) {
				decreaseLife = true;
				count = 0;
			}
			else {
				increaseLife = true;
				count = 0;
			}
			oldHealth = health;
		}

		if (decreaseLife) {
			decreaseLife = false;
			StartCoroutine(DecreaseLife(0.05f,deltaHealth));
		}

		if (increaseLife){
			increaseLife = false;
			StartCoroutine(IncreaseLife(0.05f,deltaHealth));
		}


		gt.pixelInset = new Rect(-70, -200, 400,400);
	}

	IEnumerator DecreaseLife(float delay, float deltaHealth){
		yield return new WaitForSeconds(delay);
		int i = currentHealthFrame;
		gt.texture = this.decreaseLifeTextures[i];
		i++;
		count ++;
		currentHealthFrame = i;
		decreaseLife = true;
		if (48 == count) {
			decreaseLife = false;	
			i = 0;
		}
	}

	IEnumerator IncreaseLife(float delay, float deltaHealth){
		yield return new WaitForSeconds(delay);
		int i = 193 - currentHealthFrame;
		gt.texture = this.increaseLifeTextures[i];
		i++;
		count++;
		currentHealthFrame = 193 - i;
		increaseLife = true;
		if (48 == count) {
			increaseLife = false;	
			i = 0;
		}
	}
}
