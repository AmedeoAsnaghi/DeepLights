using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlueScore : MonoBehaviour {
	private Text gt;
	private GameObject[] blueSpheres;
	private int totalBlueSpheres;
	private int currentSpheres;

	// Use this for initialization
	void Start () {
		gt = gameObject.GetComponent<Text> () as Text;
		blueSpheres = GameObject.FindGameObjectsWithTag ("BlueSphere");
		totalBlueSpheres = blueSpheres.Length;
		currentSpheres = 0;
		gt.text = currentSpheres.ToString() + "/" + totalBlueSpheres.ToString();
		Debug.Log (gt.text);
	}

	public void showUpdatedScore() {
		GameObject[] remSpheres = GameObject.FindGameObjectsWithTag ("BlueSphere");
		int remainingSpheres = remSpheres.Length;
		currentSpheres = totalBlueSpheres - remainingSpheres;
		gt.text = currentSpheres.ToString() + "/" + totalBlueSpheres.ToString();
	}
}
