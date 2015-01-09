using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClickToLoadAsync : MonoBehaviour {
	
	public GameObject loadingImage;
	
	
	private AsyncOperation async;

	public void ClickAsync(int level)
	{
		loadingImage.SetActive(true);
		Application.LoadLevel (level);
	}
	
	
}