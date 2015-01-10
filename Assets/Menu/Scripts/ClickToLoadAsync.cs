using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClickToLoadAsync : MonoBehaviour {
	
	public GameObject loadingImage;

	public void ClickAsync(int level)
	{
		loadingImage.SetActive(true);
		Application.LoadLevel (level);
	}
	
	
}