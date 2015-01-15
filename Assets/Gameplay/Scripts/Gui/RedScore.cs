using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RedScore : MonoBehaviour {
	private Text gt;
	private int totalRedSpheres;
	private GameObject[] redSpheres;
	private int currentSpheres;

	// Use this for initialization
	void Start () {
		gt = gameObject.GetComponent<Text> () as Text;
		redSpheres = GameObject.FindGameObjectsWithTag ("RedSphere");
		totalRedSpheres = redSpheres.Length;
		currentSpheres = 0;
		gt.text = currentSpheres.ToString() + "/" + totalRedSpheres.ToString();

	}
	
	// Update is called once per frame
	public void showUpdatedScore() {
		GameObject[] remSpheres = GameObject.FindGameObjectsWithTag ("RedSphere");
		int remainingSpheres = remSpheres.Length;
		currentSpheres = totalRedSpheres - remainingSpheres;
		gt.text = currentSpheres.ToString() + "/" + totalRedSpheres.ToString();
	}

	public int getCollectedEnergy(){
		return currentSpheres;
	}
}
