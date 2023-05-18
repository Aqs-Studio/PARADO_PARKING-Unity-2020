using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjects : MonoBehaviour {
	public GameObject [] objects;
	private int objLength;

	

	// Use this for initialization
	void OnEnable () {

		objLength = objects.Length;
		StartCoroutine(EnableGameObjects());

	}
	
	IEnumerator EnableGameObjects ()
	{
		int i=0;
		while(i<objLength)
		{
			yield return new WaitForSeconds(0.001f);
			objects[i].SetActive(true);
			i+=1;
			yield return new WaitForSeconds(0);
		}
	}
	
		
}
