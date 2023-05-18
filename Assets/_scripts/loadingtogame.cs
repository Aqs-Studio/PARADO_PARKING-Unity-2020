using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class loadingtogame : MonoBehaviour {
    public Text percentagedone;
    public Image loadingbar;
	// Use this for initialization
	void Start () {
        /* 
        Admob.Instance().removeAllBanner();
        Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.TOP_CENTER, 0);
        if (Admob.Instance().isInterstitialReady())
        {
            Admob.Instance().showInterstitial();
            Admob.Instance().loadInterstitial();
        }
*/
        Time.timeScale = 1;
        //SceneManager.LoadScene("testing");
        StartCoroutine(LoadScene());
	}

    // Update is called once per frame
 
    IEnumerator LoadScene()
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("testing");
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
       // Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            loadingbar.fillAmount = (asyncOperation.progress);
            int a = Mathf.RoundToInt( asyncOperation.progress*100) ;
            percentagedone.text = a.ToString();

            // Check if the load has finished
            if (asyncOperation.progress>= 0.9f)
            {
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}   

