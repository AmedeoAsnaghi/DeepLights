using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Credits : MonoBehaviour {
	public int speed;
	public GameObject list;

	private bool shown;
	private Vector3 startingPosition; 
	private Image backgroundImage;
	private Animator anCredits;
	private bool canPress;
	private bool first;

	void Start () {
		shown = false;
		startingPosition = list.transform.position;
		backgroundImage = GetComponentInChildren<Image> () as Image;
		anCredits = GetComponent<Animator> ()as Animator;
		canPress = true;
		first = true;
	}

	// Update is called once per frame
	void Update () {
		if (first) {
			list.transform.position = startingPosition;
			first = false;
		}
	
		if (shown) {
			if (Input.GetButton("Submit") && canPress) {
				anCredits.SetTrigger("return");

				canPress = false;
				shown = false;
				first = true;
				StartCoroutine(press());
			}
			list.transform.Translate (Vector3.up * speed * Time.deltaTime);	
		}



		if (backgroundImage.color.a == 1) {
			shown = true;		
		}


	}

	IEnumerator press() {
		yield return new WaitForSeconds (2f);
		canPress = true;
	}
}
