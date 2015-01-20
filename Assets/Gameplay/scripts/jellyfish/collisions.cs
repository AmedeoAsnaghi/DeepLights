using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class collisions : MonoBehaviour {

	GameManager gameManager;
	Text tutorialText;
	Animator anTutorial;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find ("Controller").GetComponent<GameManager> () as GameManager;
		tutorialText = GameObject.Find ("TutorialText").GetComponent<Text> () as Text;
		anTutorial = GameObject.Find ("TutorialText").GetComponent<Animator> () as Animator;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log (other.gameObject.tag);
		if (other.gameObject.tag == "RedSphere") {
			gameManager.increaseLife (20);
			gameManager.redSphereCatched();
			gameManager.showUpdatedScore ();
		} 
		else if (other.gameObject.tag == "BlueSphere") {
			gameManager.increaseLife (20);
			gameManager.blueSphereCatched();
			gameManager.showUpdatedScore ();
		} 
		else if (other.gameObject.tag == "YellowSphere") {
			gameManager.increaseLife (20);
			gameManager.yellowSphereCatched();
			gameManager.showUpdatedScore ();
		} 
		//TODO: manage other collision
		else if (other.gameObject.tag == "Pincers") {
			gameManager.decreaseLife (20);
		} 
		else if (other.gameObject.tag == "Predator") {
			if (other.GetType () == typeof(BoxCollider2D)) {
				gameManager.decreaseLife (20);
			}
		} 
		else if (other.gameObject.tag == "assassinAlga") {
			gameManager.decreaseLife (20);
		} 
		else if (other.gameObject.tag == "melma") {
			gameManager.decreaseLife(20);
		} 
		else if (other.gameObject.tag == "bomb") {
			StartCoroutine(BombExplosionEffect(other.gameObject));
		}
		else if (other.gameObject.tag == "nextLevel") {
			gameManager.changeLevel();
		} 
		else if (other.gameObject.tag == "tutorial"){
			//(tutorialText.GetComponent<Text>as Text).text = (other.gameObject.GetComponent<Text>()as Text).text;
			tutorialText.text = (other.gameObject.GetComponent<Text>()as Text).text;
			Destroy(other.gameObject);
			anTutorial.SetTrigger("showText");
			StartCoroutine(waitText());
		}
		else if(other.gameObject.tag == "redPower"){
			gameManager.unlockImpulsePower();
		}
		else if (other.gameObject.tag == "bluePower"){
			gameManager.unlockBarrierPower();
		}
		else if (other.gameObject.tag == "yellowPower"){
			gameManager.unlockFlashPower();
		}

	}

	IEnumerator waitText() {
		yield return new WaitForSeconds (5f);
		anTutorial.ResetTrigger("showText");
		anTutorial.SetTrigger("hideText");
	}

	IEnumerator DestroyBomb(GameObject go) {
		yield return new WaitForSeconds (0.5f);
		Destroy (go);
	}

	IEnumerator BombExplosionEffect(GameObject go) {
		yield return new WaitForSeconds (0.5f);
		if (go != null) {
			gameManager.decreaseLife(20);
		}

	}






}
