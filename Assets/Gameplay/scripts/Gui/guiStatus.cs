using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class guiStatus : MonoBehaviour {

	private GameManager gameManager;

	private int count;
	private int health;
	private int oldHealth;
	private Queue increaseLife;	
	private bool increase;
	private bool running;
	private int currentHealthFrame;
	private object[] objects;


	Image image;
	Texture2D[] decreaseLifeTextures;
	Texture2D[] increaseLifeTextures;

	void Start () {
		GameObject controller = GameObject.Find("Controller");
		gameManager = controller.GetComponent<GameManager> () as GameManager;
		health = gameManager.showCurrentLife();
		oldHealth = health;
		image = gameObject.GetComponent<Image> () as Image;
		increaseLife = new Queue ();

		this.objects = Resources.LoadAll("LifeDecrease", typeof(Texture2D)); 

		this.decreaseLifeTextures = new Texture2D[this.objects.Length];
		for(int i=0; i < objects.Length;i++)  
		{  
			this.decreaseLifeTextures[i] = (Texture2D)this.objects[i];  
		}   

		this.objects = Resources.LoadAll("LifeIncrease", typeof(Texture2D)); 
		
		this.increaseLifeTextures = new Texture2D[this.objects.Length];
		for(int i=0; i < objects.Length;i++)  
		{  
			this.increaseLifeTextures[i] = (Texture2D)this.objects[i];  
		}   

		currentHealthFrame = (100 - health) * 12 / 20;
		Texture2D texture = this.decreaseLifeTextures[currentHealthFrame];
		image.overrideSprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));

		running = false;
	}


	void Update () {
		float deltaHealth = 0;

		health = gameManager.showCurrentLife();

		if (health != oldHealth) {
			deltaHealth = oldHealth-health;
			if (deltaHealth > 0) {
				increaseLife.Enqueue(false);
			}
			else {
				increaseLife.Enqueue(true);
			}
			oldHealth = health;
		}
		if (increaseLife.Count != 0 && running == false) {
			increase = (bool)increaseLife.Dequeue ();
			running = true;
			count = 0;
		}

		if (running) {
			if (increase) {
				StartCoroutine (IncreaseLife (0.01f, deltaHealth));
			} else {
				StartCoroutine (DecreaseLife (0.01f, deltaHealth));
			}
		}
	}

	IEnumerator DecreaseLife(float delay, float deltaHealth){
		yield return new WaitForSeconds(delay);
		int i = currentHealthFrame;
		Debug.Log (currentHealthFrame);
		if (currentHealthFrame < this.decreaseLifeTextures.Length - 1) {
			Texture2D texture = this.decreaseLifeTextures [i + 1];
			image.overrideSprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));
		}
		i++;
		count ++;
		currentHealthFrame = i;

		if (11 == count) {
			running = false;	
			i = 0;
		}
	}

	IEnumerator IncreaseLife(float delay, float deltaHealth){
		yield return new WaitForSeconds(delay);
		int i = 49 - currentHealthFrame;
		if (currentHealthFrame < this.decreaseLifeTextures.Length) {
			Texture2D texture = this.increaseLifeTextures[i];
			image.overrideSprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));		
		}
		i++;
		count++;
		currentHealthFrame = 49 - i;

		if (11 == count) {
			running = false;	
			i = 0;
		}
	}
}
