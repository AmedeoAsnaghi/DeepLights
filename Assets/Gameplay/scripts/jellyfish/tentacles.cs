using UnityEngine;
using System.Collections;

public class tentacles : MonoBehaviour {
	private LineRenderer lineRenderer;
	// Use this for initialization
	void Start () {
		lineRenderer = gameObject.GetComponent<LineRenderer> () as LineRenderer;

		lineRenderer.SetVertexCount (30);



	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i<30; i++) {
			Vector3 pos = Vector3.zero;
			pos.x = Mathf.Sin(Time.time*4 + i*Mathf.PI/5)*0.2f;
			pos.y = i*0.2f;
			lineRenderer.SetPosition(i,pos);
		}
	}
}
