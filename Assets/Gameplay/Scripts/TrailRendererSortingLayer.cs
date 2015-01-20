using UnityEngine;
using System.Collections;

public class TrailRendererSortingLayer : MonoBehaviour {

	public int sortingOrder = 0;
	public string layerName = "";
	void Start ()
	{
		TrailRenderer tr = gameObject.GetComponent<TrailRenderer>() as TrailRenderer;
		// Set the sorting layer of the particle system.
		tr.sortingLayerName = layerName;
		tr.sortingOrder = sortingOrder;
	}
}
