using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class YellowScore : MonoBehaviour {
	private Text gt;
	private int totalYellowSpheres;
	private GameObject[] yellowSpheres;
	private int currentSpheres;

	// Use this for initialization
	void Start () {
		gt = gameObject.GetComponent<Text> () as Text;
		yellowSpheres = GameObject.FindGameObjectsWithTag ("YellowSphere");
		totalYellowSpheres = yellowSpheres.Length;
		currentSpheres = 0;
		gt.text = currentSpheres.ToString() + "/" + totalYellowSpheres.ToString();
	}
	
	// Update is called once per frame
	public void showUpdatedScore() {
		GameObject[] remSpheres = GameObject.FindGameObjectsWithTag ("YellowSphere");
		int remainingSpheres = remSpheres.Length;
		currentSpheres = totalYellowSpheres - remainingSpheres;
		gt.text = currentSpheres.ToString() + "/" + totalYellowSpheres.ToString();
	}

	public int getCollectedEnergy(){
		return currentSpheres;
	}
}
