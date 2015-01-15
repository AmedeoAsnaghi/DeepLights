using UnityEngine;
using System.Collections;

public class UpdateCollider : MonoBehaviour {
	Animator an = null;
	Rigidbody2D body;
	BoxCollider2D[] colliders;
	bool isFirst = true;
	float[] size0 = {2.060746f, 2.626462f};
	float[] center0 = {2.601087f, 1.070721f};

	private int go;

	float[] size1 = {2.060746f, 2.626462f};
	float[] center1 = {-2.601087f, 1.070721f};

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> () as Rigidbody2D;
		colliders = GetComponents<BoxCollider2D> () as BoxCollider2D[];
		an = GetComponent<Animator> () as Animator;
		go = 1;
	}
	
	// Update is called once per frame
	void Update () {

		var currentState = an.GetCurrentAnimatorStateInfo (0);
		var parentPosition = GetComponentInParent<Transform> () as Transform;

		if (go==1) {
			Destroy (colliders [0]);
			Destroy (colliders [1]);
		
			colliders [0] = (BoxCollider2D)gameObject.AddComponent ("BoxCollider2D");
			colliders [0].size = new Vector2 (size0 [0], size0 [1]);
			colliders [0].center = new Vector2 (center0 [0], center0 [1]);

			colliders [1] = (BoxCollider2D)gameObject.AddComponent ("BoxCollider2D");
			colliders [1].size = new Vector2 (size1 [0], size1 [1]);
			colliders [1].center = new Vector2 (center1 [0], center1 [1]);

			if (currentState.nameHash == Animator.StringToHash ("Base Layer.Loop")) {
				if (isFirst) {
					center0[0] = parentPosition.localPosition.x + 2.601087f;
					center0[1] = parentPosition.localPosition.y + 2.556885f;
					center1[0] = parentPosition.localPosition.x - 2.601087f;
					center1[1] = parentPosition.localPosition.y + 2.556885f;

					isFirst = false;
					
				}
				center0 [0] = center0 [0] - 0.045f;
				center0 [1] = center0 [1] + 0.045f;

				center1 [0] = center1 [0] + 0.045f;
				center1 [1] = center1 [1] + 0.045f;

			}

			if (currentState.nameHash == Animator.StringToHash ("Base Layer.pincerLoopEnd")) {
				center0 [0] = center0 [0] + 0.025f;
				center0 [1] = center0 [1] - 0.025f;

				center1 [0] = center1 [0] - 0.025f;
				center1 [1] = center1 [1] - 0.025f;

				isFirst = true;
				
			}
		}
	}

	public void restart(){
		go = 1;
	}

	public void stop(){
		go = 0;
	}
}
