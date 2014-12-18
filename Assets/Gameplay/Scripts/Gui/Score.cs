using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
	private GUIText gt;
	private GameObject[] spheres;
	private int totalSpheres;
	private int currentSpheres;

	// Use this for initialization
	void Start () {
		gt = gameObject.GetComponent<GUIText> () as GUIText;
		spheres = GameObject.FindGameObjectsWithTag ("sphere");
		totalSpheres = spheres.Length;
		currentSpheres = 0;
		gt.text = currentSpheres.ToString() + " / " + totalSpheres.ToString();
	}

	public void showUpdatedScore() {
		GameObject[] remSpheres = GameObject.FindGameObjectsWithTag ("sphere");
		int remainingSpheres = remSpheres.Length;
		currentSpheres = totalSpheres - remainingSpheres;

		gt.text = currentSpheres.ToString() + " / " + totalSpheres.ToString();
	}
}
