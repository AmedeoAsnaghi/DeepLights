using UnityEngine;
using System.Collections;

public class AttackCollider : MonoBehaviour {

	Animator an = null;

	public float step;
	BoxCollider2D[] colliders;
	
	float startY,endY;
	float centerX, centerY, sizeX, sizeY;

	// Use this for initialization
	void Start () {
		an = GetComponent<Animator> () as Animator;
		colliders = GetComponents<BoxCollider2D> () as BoxCollider2D[];
		startY = colliders [1].size.y;
		endY = startY + 2f;
		step = step * Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
		var hash = (an.GetCurrentAnimatorStateInfo (0)).nameHash;

		centerX = colliders [1].center.x;
		centerY = colliders [1].center.y;
		sizeX = colliders [1].size.x;
		sizeY = colliders [1].size.y;

		if(hash == Animator.StringToHash("Base Layer.Attack")){
			if(sizeY < endY){
				sizeY+=step;
				centerY-=step/2f;
			}
		}
		else if (hash == Animator.StringToHash("Base Layer.EndAttack")) {
			if(sizeY > startY){
				sizeY-=step;
				centerY+=step/2f;
			}
		}

		Destroy (colliders [1]);
		colliders[1] = (BoxCollider2D)gameObject.AddComponent ("BoxCollider2D");
		colliders [1].center = new Vector2 (centerX, centerY);
		colliders [1].size = new Vector2 (sizeX, sizeY);

	}
}
