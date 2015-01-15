using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {
	public float step;
	public float range;

	Animator an;
	bool isAttacking;
	bool moving;
	Vector3 dir;
	float countRange;
	bool back;
	GameObject target;
	float savedX;
	float savedY;

	private bool go;

	// Use this for initialization
	void Start () {
		//rigidBody = GetComponent<Rigidbody2D> () as Rigidbody2D;
		an = GetComponent<Animator> () as Animator;
		isAttacking = false;
		moving = false;
		back = false;
		countRange = 0;
		target = GameObject.FindGameObjectWithTag ("Player");
		go = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(go){
			if (isAttacking) {
				if(!moving){
					dir = target.transform.position - transform.position;
					float angle = Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg;
					//transform.rotation = Quaternion.AngleAxis(angle,new Vector3(0,0,1));
					//transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
					transform.Rotate(dir* Time.deltaTime);
					moving = true;
				}
				else{
					if(countRange < range && !back){
						transform.position += (dir/dir.magnitude) * step;
						countRange+=step;
					}
					else{
						back = true;
						countRange-=step;
						transform.position -= (dir/dir.magnitude) * step;
						if(countRange <= 0)
						{
							back = false;
							isAttacking = false;
							moving = false;
							countRange = 0;
							an.SetBool("preyFound",false);

						}
					}
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		preyFound (other);
	}

	public void preyFound(Collider2D other){
		if (other.gameObject.tag == "Player" && !isAttacking) {
			an.SetBool("preyFound",true);
			isAttacking = true;
		} 	
	}

	public void restart(){
		go = true;	
	}
	
	public void stop(){
		go = false;
	}
}
