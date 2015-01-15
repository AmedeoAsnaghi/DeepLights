using UnityEngine;
using System.Collections;

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
		increaseLife = new Queue ();

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

		currentHealthFrame = (100 - health) * 12 / 20;
		gt.texture = this.decreaseLifeTextures[currentHealthFrame];
		gt.pixelInset = new Rect(40, -30, 220,50);
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
		gt.pixelInset = new Rect(40, -30, 220,50);
	}

	IEnumerator DecreaseLife(float delay, float deltaHealth){
		yield return new WaitForSeconds(delay);
		int i = currentHealthFrame;
		if (currentHealthFrame<this.decreaseLifeTextures.Length)
			gt.texture = this.decreaseLifeTextures[i+1];
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
		if (currentHealthFrame<this.decreaseLifeTextures.Length)
			gt.texture = this.increaseLifeTextures[i];
		i++;
		count++;
		currentHealthFrame = 49 - i;

		if (11 == count) {
			running = false;	
			i = 0;
		}
	}
}
