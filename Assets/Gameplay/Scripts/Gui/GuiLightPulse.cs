using UnityEngine;
using System.Collections;

public class GuiLightPulse : MonoBehaviour {
	GameManager gameManager;
	GUITexture gt;
	Texture[] chargingTextures;
	Texture chargedTexture;
	private object[] objects;
	private bool lightPulsingChargingActive;
	private int currentLightImpulseFrame;

	public float x = 0;
	public float y = 0.9f;
	// Use this for initialization
	void Start () {
		GameObject controller = GameObject.Find("Controller");
		gameManager = controller.GetComponent<GameManager> () as GameManager;

		gt = gameObject.GetComponent<GUITexture> () as GUITexture;
		gt.transform.position = new Vector3(x,y,0);
		gt.transform.localScale = Vector3.zero;

		
		this.objects = Resources.LoadAll("LightPulseCharging", typeof(Texture)); 
		
		this.chargingTextures = new Texture[this.objects.Length];
		for(int i=0; i < objects.Length;i++)  
		{  
			this.chargingTextures[i] = (Texture)this.objects[i];  
		}   
		
		this.chargedTexture = this.chargingTextures[0];

		gt.texture = this.chargedTexture;
		gt.pixelInset = new Rect(-70, -200, 400,400);

	}
	
	// Update is called once per frame
	void Update () {
		if (!gameManager.CanDoLightImpulse()) {
			if (!lightPulsingChargingActive) {
				lightPulsingChargingActive = true;
				StartCoroutine (lightPulsing(0.01f));
			}
		}
	}

	IEnumerator lightPulsing(float delay){
		yield return new WaitForSeconds(delay);
		lightPulsingChargingActive = false;
		currentLightImpulseFrame = 0;
		if (currentLightImpulseFrame<this.chargingTextures.Length)
			gt.texture = this.chargingTextures[currentLightImpulseFrame];
		currentLightImpulseFrame++;
	}
}
