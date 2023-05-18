using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Loading : MonoBehaviour {
public static bool showAdmob = true;

	void OnEnable () {
        Time.timeScale = 1;
        SceneManager.LoadScene("testing");
	}
    IEnumerator loadingWait()
    {
        float i=0;
        while (i<1)
        {
            i+=Time.deltaTime*1;
            yield return new WaitForSeconds(0);
        }
        showads();
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("testing");
    }
    public void showads()
    {
        if(showAdmob)
        {
            showAdmob = false;
           // GoogleMobileAdsManager.Instance.ShowInterstitial();
			//Ads_Manager.Instance.ShowInterstitial ();
        }

    }
}
