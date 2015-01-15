using UnityEngine;
using System.Collections;

public class ColorManager : MonoBehaviour {

	GameObject heart;
	GameObject particellare_cuore;
	ParticleSystem particle;
	SpriteRenderer heartSprite;

	public float updateColorStep = 1f;
	private Color targetColor;
	private bool updateColor;
	private float red,green,blue,a;

	// Use this for initialization
	void Start () {
		heart = (GameObject.Find("heart")) as GameObject;
		heartSprite = (heart.GetComponent<SpriteRenderer> ()) as SpriteRenderer; 
		particellare_cuore = (GameObject.Find ("particellare_cuore")) as GameObject;
		particle = particellare_cuore.GetComponent<ParticleSystem>() as ParticleSystem;
		updateColorStep = updateColorStep * Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(updateColor)
		{	
			red = particle.startColor.r;
			green = particle.startColor.g;
			blue = particle.startColor.b;
			a = particle.startColor.a;
			
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
			
			particle.startColor = new Color(red,green,blue,a);
			heartSprite.color = new Color(red,green,blue,a);
			if(particle.startColor.Equals(targetColor)){
				updateColor = false;
			}
			
		}	
	}

	public void updateColorJellyfish(Color c){
		targetColor = c;
		updateColor = true;
	}
}
