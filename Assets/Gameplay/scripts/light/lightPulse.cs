using UnityEngine;
using System.Collections;

public class lightPulse : MonoBehaviour {
	public float lightRange = 0;

	private GameManager gameManager;
	public float duration = 2.0f;
	public float pulsingFactor;
	public float waveAmplitude;

	private float targetLightRange;
	private float currentLightRange;
	private bool update;
	public float updateStep = 0.1f;

	public float updateColorStep = 1f;
	private Color targetColor;
	private bool updateColor;
	private float red,green,blue,a;

	// Use this for initialization
	void Start () {
		GameObject controller = GameObject.Find("Controller");
		gameManager = controller.GetComponent<GameManager> () as GameManager;
		lightRange = light.range;
		currentLightRange = lightRange  * gameManager.showCurrentLife()/100;
		update = false;
		updateColor = false;
		updateColorStep = updateColorStep * Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameManager.CanPulse ()) {
						var phi = (Time.time * pulsingFactor) / duration * 2 * Mathf.PI;
						// get cosine and transform from -1..1 to 0..1 range
						var amplitude = Mathf.Cos (phi) * waveAmplitude;
						light.range = (currentLightRange + (float)amplitude);

						if (update) {
								if (currentLightRange < targetLightRange) {
										currentLightRange += updateStep;
								} else if (currentLightRange > targetLightRange) {
										currentLightRange -= updateStep;
								} else {
										update = false;
								}
						}
			}
			if(updateColor)
			{

				red = light.color.r;
				green = light.color.g;
				blue = light.color.b;
				a = light.color.a;

				//RED
				if(red < targetColor.r){
					red += updateColorStep;
				}
				else if(red > targetColor.r){
					red -= updateColorStep;
				}

				//GREEN
				if(green < targetColor.g){
					green += updateColorStep;
				}
				else if(green > targetColor.g){
					green -= updateColorStep;
				}

				//BLUE
				if(blue < targetColor.b){
					blue += updateColorStep;
				}
				else if(blue > targetColor.b){
					blue -= updateColorStep;
				}

				light.color = new Color(red,green,blue,a);

				if(light.color.Equals(targetColor)){
					updateColor = false;
				}

			}

	}

	//use this method AFTER the life update
	public void updateJellyfishLight(){
		targetLightRange = lightRange * gameManager.showCurrentLife () / 100;
		update = true;
	}

	public void updateJellyfishLightColor(Color c){
		targetColor = c;
		updateColor = true;
	}


}
