using UnityEngine;
using System.Collections;

public class ParticleSortingLayer : MonoBehaviour {
		
	public int sortingOrder = 0;
	public string layerName = "";
		void Start ()
		{
			// Set the sorting layer of the particle system.
			particleSystem.renderer.sortingLayerName = layerName;
			particleSystem.renderer.sortingOrder = sortingOrder;
		}
	}