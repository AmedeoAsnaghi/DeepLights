using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {
	public int speed;
	public GameObject list;

	// Update is called once per frame
	void Update () {
		list.transform.Translate (Vector3.up * speed * Time.deltaTime);
	}
}
