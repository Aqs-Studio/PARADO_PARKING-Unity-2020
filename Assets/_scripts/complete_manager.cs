using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class complete_manager : MonoBehaviour 
{
    public static complete_manager instance;
    public GameObject complete, doubleRewardText, doubleRewardButton;
      Dictionary<string, object> levelDict = new Dictionary<string, object>();
//	public static bool addAnalyticcomplete;

    //int cash = 0;
	// Use this for initialization
	void Start ()
    {
        instance = this;
        Time.timeScale = 1;
	

        PlayerPrefs.SetInt("cash", PlayerPrefs.GetInt("cash") + 500);
        if (PlayerPrefs.GetInt("from_setting_music") == 1)
        {
            if(sound_manager.instance)
            sound_manager.instance.menuMusic.volume = 0.4f;
        }
        next_stat();
 
		GameAnalytics.instance.LevelCompleteEvent (PlayerPrefs.GetInt("funnelLevelNumber"));


       
		///Ads_Manager.Instance.ShowLargeAdmobBanner ();
    }
//	public void Level_Events ()
//	{
//		// status =   start ,complete
//        //levelDict.Add("level_mode", PlayerPrefs.GetInt("currentMode") );
//        levelDict.Add("level_index", PlayerPrefs.GetInt("funnelLevelNumber") );
//
//        Analytics.CustomEvent("vc30_Complete"+PlayerPrefs.GetInt("funnelLevelNumber").ToString(), levelDict);
//        levelDict.Clear();
//	
//	
//		// Analytics.CustomEvent("vc20_complete", new Dictionary <string, object> {
//		// 	{ "level_index", PlayerPrefs.GetInt("level") }
//		// } );
//	}
	
    public void next_stat()
    {
        if(sound_manager.instance)
        sound_manager.instance.complete++;
        complete.SetActive(true);

		//Ads_Manager.Instance.HideSmallAdmobBanner ();
        PlayerPrefs.SetInt("AdCount", PlayerPrefs.GetInt("AdCount") + 1);
        if (PlayerPrefs.GetInt("AdCount") == 2)
        {
          
			//Ads_Manager.Instance.Show_Unity_Admob ();
			//Ads_Manager.Instance.Show_Unity_Admob ();
			PlayerPrefs.SetInt("AdCount", 0);
		
		
        }
        if (sound_manager.instance.complete == 2)
        {
            sound_manager.instance.complete = 0;
            
              
        }
		//Ads_Manager.Instance.ShowLargeAdmobBanner ();
    }



    public void next()
    {
       // GoogleMobileAdsManager.Instance.HideLargeAdmobBanner();
	
        int level_played = PlayerPrefs.GetInt("level");
        level_played++;
        PlayerPrefs.SetInt("funnelLevelNumber", PlayerPrefs.GetInt("funnelLevelNumber")+1);
        PlayerPrefs.SetInt("level", level_played);
        UnlockNextLevel_next(level_played);
        PlayerPrefs.Save();
        switch(PlayerPrefs.GetInt("currentMode"))
        {
            case 1:
                if(level_played >= PlayerPrefs.GetInt("availableCarLevels"))
                {
                    SceneManager.LoadScene("main_menu");

                    
                }
                else
                {
                    SceneManager.LoadScene("loading");
                }
            break;
            case 2:
                if(level_played >= PlayerPrefs.GetInt("availableConeLevels"))
                {
                    SceneManager.LoadScene("main_menu");
                    

                }
                else
                {
                    SceneManager.LoadScene("loading");
                }
            break;
            case 3:
                if(level_played >= PlayerPrefs.GetInt("availableBlockLevels"))
                {
                    SceneManager.LoadScene("main_menu");
                    
                }
                else
                {
                    SceneManager.LoadScene("loading");
                }
            break;                         
        }  
	//	Ads_Manager.Instance.HideLargeAdmobBanner ();
		//Ads_Manager.Instance.HideSmallAdmobBanner ();
    
    }
    public void restart()
    {
        //GoogleMobileAdsManager.Instance.HideLargeAdmobBanner();
	
        SceneManager.LoadScene("loading");
	//	Ads_Manager.Instance.HideLargeAdmobBanner ();
	
    }
    public void main_menu()
    {
		
        int level_played = PlayerPrefs.GetInt("level");
        level_played++;
        PlayerPrefs.SetInt("level", level_played);
        UnlockNextLevel_next(level_played);

        SceneManager.LoadScene("main_menu");
	//	Ads_Manager.Instance.HideSmallAdmobBanner ();
	//	Ads_Manager.Instance.HideLargeAdmobBanner ();
    }

    void UnlockNextLevel_next(int level )
    {
        switch(PlayerPrefs.GetInt("currentMode"))
        {
            case 1:
            PlayerPrefs.SetInt("M1 "+level.ToString(),1);
            sound_manager.instance.currentLevel = "M1 "+level.ToString();
            break;
            case 2:
            PlayerPrefs.SetInt("M2 "+level.ToString(),1);
            sound_manager.instance.currentLevel = "M2 "+level.ToString();
            break;
            case 3:
            PlayerPrefs.SetInt("M3 "+level.ToString(),1);
            sound_manager.instance.currentLevel = "M3 "+level.ToString();
            break;                         
        }  


    }
    public void ShowRewardedVideo ()
    {
       // GoogleMobileAdsManager.Instance.Show_RewardedVideo_Unity_Admob();
	//	Ads_Manager.Instance.ShowUnityRewardedVideoAd ();
    }
    public void DoubleReward ()
    {
        PlayerPrefs.SetInt("cash", PlayerPrefs.GetInt("cash") + 500);
        PlayerPrefs.Save();
        doubleRewardText.SetActive(true);
        doubleRewardButton.SetActive(false);
    }

}

