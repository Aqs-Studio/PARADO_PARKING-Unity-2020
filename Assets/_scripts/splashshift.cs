using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class splashshift : MonoBehaviour {
    public GameObject environment;

	void Start () {

	
        StartCoroutine(splashwait());

        //Screen.sleepTimeout =30;
        //Screen.SetResolution(1280,720, true);
        Application.targetFrameRate = 30;
	}
	
    public IEnumerator splashwait()
    {
       yield return new WaitForSeconds(1f);
        if(environment)
        {
            environment.SetActive(true);
            DontDestroyOnLoad(environment);
        }
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("main_menu");

    }
}
