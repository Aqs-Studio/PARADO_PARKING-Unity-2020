using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class main_menu : MonoBehaviour {
    public static main_menu instance;
    public GameObject exit_panal,menu_Panel;
    public GameObject menuCanvas,settingsCanvas,levelselectionCanvas,storeCanvas;

    public Text wallet;
	public  int cash, cashhh;
    public static bool isLevelScreen = false;
    public GameObject MenuEnvironment;
    public GameObject social_Panel,facebookButton,instagramButton,youtubeButton;
	public GameObject watch_vedio;

    
	// Use this for initialization
	void Start ()
    {
		//PlayerPrefs.DeleteAll ();
        instance = this;
        LoadMenuEnvironment ();
        PlayerPrefs.SetInt("cash", 20000);




     watch_vedio.SetActive (false);
		if(menuCanvas.activeInHierarchy)
		{
			HideSmallBanner ();
		}
//		if(level_selection_manager.cashmain)
//		{
//			Debug.Log ("videooooooooooooooooooooooooooooooo");
//		}
        PlayerPrefs.SetInt("AdCount",0);
		cash =(PlayerPrefs.GetInt("cash"));
        if(!PlayerPrefs.HasKey("facebook"))
            PlayerPrefs.SetInt("facebook",0);
        if(!PlayerPrefs.HasKey("instagram"))
            PlayerPrefs.SetInt("instagram",0);
        if(!PlayerPrefs.HasKey("youtube"))
            PlayerPrefs.SetInt("youtube",0);  




		if (PlayerPrefs.GetInt ("startReward") == 0) 
		{
			PlayerPrefs.SetInt ("cash", 100);
			cash = PlayerPrefs.GetInt("cash");
			wallet.text = cash.ToString();
			PlayerPrefs.SetInt ("startReward", 1);
		}




	
       // PlayerPrefs.SetInt("cash", cash);
        //PlayerPrefs.SetInt("cash", 50000);
//        if (SystemInfo.graphicsMemorySize >= 0 && SystemInfo.graphicsMemorySize <= 256 || SystemInfo.systemMemorySize >= 0 && SystemInfo.systemMemorySize <= 1024)
//        {
//            QualitySettings.SetQualityLevel(0);
//        }
//        else if (SystemInfo.graphicsMemorySize > 256 && SystemInfo.graphicsMemorySize <= 512 || SystemInfo.systemMemorySize > 1024 && SystemInfo.systemMemorySize <= 2048)
//        {
//            QualitySettings.SetQualityLevel(1);
//
//        }
//        else if (SystemInfo.graphicsMemorySize > 512 || SystemInfo.systemMemorySize > 2048)
//        {
//            QualitySettings.SetQualityLevel(2);
//        }
        if(isLevelScreen)
        {
            play();
            isLevelScreen = false;
        }
        ShowCash ();
        MenuMusic ();

     

        
    }
    public void ShowSmallBanner()
    {
      
	//	Ads_Manager.Instance.ShowSmallAdmobBanner ();
    }
    public void HideSmallBanner()
    {
      
		//Ads_Manager.Instance.HideSmallAdmobBanner ();
    }
    public void ShowLargeBanner()
    {
       
	//	Ads_Manager.Instance.ShowLargeAdmobBanner ();
    }
    public void HideLargeBanner()
    {
    
		//Ads_Manager.Instance.HideLargeAdmobBanner ();
    }


    void LoadMenuEnvironment ()
    {
        MenuEnvironment.SetActive(true);
        //Instantiate(MenuEnvironment, MenuEnvironment.transform.position, MenuEnvironment.transform.rotation);
    }
    public void ShowCash ()
    {
		
		
		//PlayerPrefs.SetInt ("adamount", 0);

        cash = PlayerPrefs.GetInt("cash");
        wallet.text = cash.ToString();
	
	
    }
  
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
			
            if(social_Panel.activeInHierarchy)
            {
                social_Panel.SetActive(false);
            }
            else
            if (exit_panal.activeInHierarchy)
            {
                exit_panal.SetActive(false);

                menu_Panel.SetActive(true);
				HideSmallBanner ();
            }
            else
            if(settingsCanvas.GetComponent<Canvas>().enabled)
            {
                menuCanvas.GetComponent<Canvas>().enabled = true;
                menuCanvas.GetComponent<GraphicRaycaster>().enabled = true;
                menu_Panel.SetActive(true);
                back_to_main();
						HideLargeBanner();
						HideSmallBanner ();
            }
            else
            if(levelselectionCanvas.GetComponent<Canvas>().enabled)
            {
                levelselectionCanvas.GetComponent<Canvas>().enabled = false;
                levelselectionCanvas.GetComponent<GraphicRaycaster>().enabled = false;
                menuCanvas.GetComponent<Canvas>().enabled = true;
                menuCanvas.GetComponent<GraphicRaycaster>().enabled = true;
                menu_Panel.SetActive(true);
							HideLargeBanner();
            }
            else
            if(menuCanvas.GetComponent<Canvas>().enabled && menu_Panel.activeInHierarchy)
            {
                exit_panal.SetActive(true);
                menu_Panel.SetActive(false);
            }
		
        }
	 if(menuCanvas.GetComponent<Canvas>().enabled && menu_Panel.activeInHierarchy)
		{

			HideLargeBanner ();

		}


     
    }
    void MenuMusic ()
    {
        if (PlayerPrefs.GetInt("from_setting_music")==1)
        {
            sound_manager.instance.menuMusic.Play();
            sound_manager.instance.menuMusic.volume = 0.5f;
        }
    }
    public void play()
    {
       levelselectionCanvas.GetComponent<Canvas>().enabled = true;
       levelselectionCanvas.GetComponent<GraphicRaycaster>().enabled = true;
       menuCanvas.GetComponent<Canvas>().enabled = false;
       menuCanvas.GetComponent<GraphicRaycaster>().enabled = false;
       level_selection_manager.instance.back_envorment();
       level_selection_manager.instance.ModeSelection();
      // ClickSound ();
		GameAnalytics.instance.PlayGameEvent ();
       ShowSmallBanner();

	//	Analytics.CustomEvent ("PlayBtn");
    }
//  public  void ClickSound ()
//    {
//
//    }
    public void setting()
    {
        ShowSmallBanner();
        settingsCanvas.GetComponent<Canvas>().enabled = true;
        settingsCanvas.GetComponent<GraphicRaycaster>().enabled = true;
        menuCanvas.GetComponent<Canvas>().enabled = false;
        menuCanvas.GetComponent<GraphicRaycaster>().enabled = false;
 
    }
    public void more_by_us()
    {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Knock-Solutions%20(Gamtech)%20Inc.");
		Analytics.CustomEvent ("MoreGames");
    }
    public void ratethisgame()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.ghive.jeep.parking.car.free.game.master.apps");
		Analytics.CustomEvent ("rateUs");
    }
    public void privecy_policy()
    {
        Application.OpenURL("https://gamershivepro.wordpress.com/");

    }
    public void exit()
    {
        ShowLargeBanner();
       // GoogleMobileAdsManager.Instance.ShowUnityVideoAd();
	//	Ads_Manager.Instance.ShowUnityVideoAd ();
        menu_Panel.SetActive(false);
        exit_panal.SetActive(true);
		Analytics.CustomEvent ("exitgame");
        
    }
    public void exit_panal_yes()
    {
        Application.Quit();
		HideLargeBanner ();
    }
    public void LevelSelection_ToMenu ()
    {
        levelselectionCanvas.GetComponent<Canvas>().enabled = false;
        levelselectionCanvas.GetComponent<GraphicRaycaster>().enabled = false;
        menuCanvas.GetComponent<Canvas>().enabled = true;
        menuCanvas.GetComponent<GraphicRaycaster>().enabled = true;
        menu_Panel.SetActive(true);
    }
    public void exit_panal_no()
    {
  
        menu_Panel.SetActive(true);
		HideLargeBanner();
        exit_panal.SetActive(false);
    }
    public void back_to_main()
    {
        HideSmallBanner();
        settingsCanvas.GetComponent<Canvas>().enabled = false;
        settingsCanvas.GetComponent<GraphicRaycaster>().enabled = false;
        menuCanvas.GetComponent<Canvas>().enabled = true;
        menuCanvas.GetComponent<GraphicRaycaster>().enabled = true;
        menu_Panel.SetActive(true);
       


    }
    public void Store_To_Level()
    {
        storeCanvas.GetComponent<Canvas>().enabled = false;
        storeCanvas.GetComponent<GraphicRaycaster>().enabled = false;
        levelselectionCanvas.GetComponent<Canvas>().enabled = true;
        levelselectionCanvas.GetComponent<GraphicRaycaster>().enabled = true;

    }  

    IEnumerator sabar1()
    {
        yield return new WaitForSeconds(1f);
        //adsNotAvailable.SetActive(false);
    }
    public void EnableStore ()
    {
     
        storeCanvas.GetComponent<Canvas>().enabled = true;
        storeCanvas.GetComponent<GraphicRaycaster>().enabled = true;
        levelselectionCanvas.GetComponent<Canvas>().enabled = false;
        levelselectionCanvas.GetComponent<GraphicRaycaster>().enabled = false;

    }
    public void Social_Dialogue(bool status)
    {
      
      
            HideSmallBanner();
       
        social_Panel.SetActive(status);
        if(PlayerPrefs.GetInt("facebook").Equals(1))
          facebookButton.GetComponent<Button>().interactable = false;
        if(PlayerPrefs.GetInt("instagram").Equals(1))
          instagramButton.GetComponent<Button>().interactable = false;
        if(PlayerPrefs.GetInt("youtube").Equals(1))
          youtubeButton.GetComponent<Button>().interactable = false;  
		Analytics.CustomEvent ("Ad_mainMenu_Social_Dialogue");
    }    
    public void Facebook()
    {
        Application.OpenURL("https://www.facebook.com/KSG-Inc-340340530109538/");
        PlayerPrefs.SetInt("cash",PlayerPrefs.GetInt("cash")+150);
        PlayerPrefs.SetInt("facebook",1);
        facebookButton.GetComponent<Button>().interactable = false;
        PlayerPrefs.Save();
        ShowCash();
        level_selection_manager.instance.ShowCash();
        Store_Manager.instance.ShowTotalCoins();
    }
    public void Instagram()
    {
        Application.OpenURL("https://www.instagram.com/knocksolutions_gamtech_inc/");
        PlayerPrefs.SetInt("cash",PlayerPrefs.GetInt("cash")+150);
        PlayerPrefs.SetInt("instagram",1);
        PlayerPrefs.Save();
        instagramButton.GetComponent<Button>().interactable = false;
        ShowCash();
        level_selection_manager.instance.ShowCash();
        Store_Manager.instance.ShowTotalCoins();
    }
    public void Youtube ()
    {
        Application.OpenURL("https://www.youtube.com/channel/UCasMmkFwEJ3TYNooB44x5-w?view_as=subscriber");
        PlayerPrefs.SetInt("cash",PlayerPrefs.GetInt("cash")+150);
        PlayerPrefs.SetInt("youtube",1);  
        PlayerPrefs.Save();
        youtubeButton.GetComponent<Button>().interactable = false;
        ShowCash();
        level_selection_manager.instance.ShowCash();
        Store_Manager.instance.ShowTotalCoins();     
    }
	public void watchADD()
	{
		watch_vedio.SetActive (true);
	}
	public void WatchVideo ()
	{
		//GoogleMobileAdsManager.Instance.Show_RewardedVideo_Unity_Admob();
	//	Ads_Manager.Instance.ShowUnityRewardedVideoAd ();
		watch_vedio.SetActive (false);

		//PlayerPrefs.SetInt("cash", PlayerPrefs.GetInt("cash") + 100);
		//wallet.text = cash.ToString();

//		cash = PlayerPrefs.GetInt("cash")+ 100;
//		wallet.text = cash.ToString();
	}
	public void watchADDBACK()
	{
		watch_vedio.SetActive (false);
	}

	public void impossibleTracklink()
	{
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.impossible.tracks.real.race.games&referrer=utm_source=prado_parking");
		Analytics.CustomEvent ("impossibleTrackAdd");
	}
}
