using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class level_selection_manager : MonoBehaviour {
    public static level_selection_manager instance;
    public GameObject enviroment,loadingPanel;
    public GameObject car_level;
    public GameObject cone_level;
    public GameObject block_level;
    public GameObject back_to_main;
    public GameObject adsNotAvailable;
    public GameObject back_to_enviorment;
    public GameObject watch_vedio;
    // car mode
    [Range(1,5)]public int totalCarPanels = 3; // total panels in car mode
    public LevelButton [] carLevels;
	private int currentCarPanel= 1;
	private int currentCarLevel= 0;  //change in levels til last level shows on panel
	public int availableCarLevels = 20;
    public GameObject nextCarLevelButton, previousCarLevelButton;
  //  public CanvasGroup moreLevelsAre;


// cone mode
    [Range(1,5)]public int totalConePanels = 3; // total panels in car mode
    public LevelButton [] coneLevels;
	private int currentConePanel= 1;
	private int currentConeLevel= 0;  //change in levels til last level shows on panel
	public int availableConeLevels = 15;
    //public GameObject nextConeLevelButton, previousConeLevelButton;
    public GameObject lockCone;
    // block mode
    [Range(1,5)]public int totalBlockPanels = 1; // total panels in car mode
    public LevelButton [] blockLevels;
	private int currentBlockPanel= 1;
	private int currentBlockLevel= 0;  //change in levels til last level shows on panel
	public int availableBlockLevels = 15;
   // public Button[] levelBtnsM1, levelBtnsM2, levelBtnsM3;
   // private int levelBtnsM1_Length, levelBtnsM2_Length, levelBtnsM3_Length;
   public GameObject lockBlock;
    public GameObject the_limit_is_full;
    int counter_vedio;
    int cash = 0;
    public Text wallet;
	public static bool cashmain;
	// Use this for initialization
	void Start ()
    {
        instance = this;
		cashmain = false;
        back_to_enviorment.SetActive(false);
        PlayerPrefs.SetInt("M1 0", 1); // mode 1 level1
        PlayerPrefs.SetInt("M2 0", 1); // mode 2 level1
        PlayerPrefs.SetInt("M3 0", 1); // mode 3 level1
        PlayerPrefs.SetInt("availableCarLevels", availableCarLevels);
        PlayerPrefs.SetInt("availableConeLevels", availableConeLevels);
		//Debug.Log(availableConeLevels  +"cones");
        PlayerPrefs.SetInt("availableBlockLevels", availableBlockLevels);

        if(!PlayerPrefs.HasKey("funnelLevelNumber"))  // level numbering for analytics
        PlayerPrefs.SetInt("funnelLevelNumber",0);

		if (PlayerPrefs.GetInt ("startReward") == 0) 
		{
			PlayerPrefs.SetInt ("cash", 100);
			cash = PlayerPrefs.GetInt("cash");
			wallet.text = cash.ToString();
			PlayerPrefs.SetInt ("startReward", 1);
		}
        cash = PlayerPrefs.GetInt("cash");
        if(!PlayerPrefs.HasKey("lockBlock"))
        PlayerPrefs.SetInt("lockBlock",0);
        if(!PlayerPrefs.HasKey("lockCone"))
        PlayerPrefs.SetInt("lockCone",0);
        //InvokeRepeating("ShowCash",0.1f,0.1f);

        ShowCash ();
        if(PlayerPrefs.GetInt("lockBlock").Equals(0))
        lockBlock.SetActive(true);
        else
        lockBlock.SetActive(false);
        if(PlayerPrefs.GetInt("lockCone").Equals(0))
        lockCone.SetActive(true);
        else
        lockCone.SetActive(false);





    }
	void SwitchCarPanel ()
	{
		for(int i = 0; i<10; i++)
		{
			carLevels[i].SetLevel(currentCarLevel);
			currentCarLevel +=1;
		}
		if(currentCarPanel <2)
		{
			previousCarLevelButton.SetActive(false);
			nextCarLevelButton.SetActive(true);
		}
		else
		if(currentCarPanel >=totalCarPanels)
		{
			previousCarLevelButton.SetActive(true);
			nextCarLevelButton.SetActive(false);
		}
		else
		{
			previousCarLevelButton.SetActive(true);
			nextCarLevelButton.SetActive(true);	
		}
	}
    void SwitchConePanels ()
    {
        currentConeLevel = 0;
		for(int i = 0; i<15; i++)
		{
			coneLevels[i].SetLevel(currentConeLevel);
			currentConeLevel +=1;
		}        
    }
    void SwitchBlockPanels ()
    {
        currentBlockLevel = 0;
		for(int i = 0; i<15; i++)
		{
			blockLevels[i].SetLevel(currentBlockLevel);
			currentBlockLevel +=1;
		}        
    }    
//	public void CloseComingSoonPanel ()
//	{
//        if(moreLevelsAre)
//        {        
//            moreLevelsAre.alpha = 0;
//            moreLevelsAre.blocksRaycasts = false;
//        }
//	}
//	public void EnableComingSoonPanel ()
//	{
//        if(moreLevelsAre)
//        {
//            moreLevelsAre.alpha = 1;
//            moreLevelsAre.blocksRaycasts = true;
//        }
//
//		Invoke("CloseComingSoonPanel",1);
//	}    
    public void ShowCash ()
    {
        cash = PlayerPrefs.GetInt("cash");
        wallet.text = cash+System.String.Empty;
        Store_Manager.instance.ShowTotalCoins();
    }
	public void NextPanel ()
	{
		currentCarPanel +=1;
		SwitchCarPanel ();
		//AudioManager.instance.PlayButtonClick();
	}
	public void PreviousPanel ()
	{
		currentCarPanel -=1;
		currentCarLevel -=20;
		SwitchCarPanel ();
		//AudioManager.instance.PlayButtonClick();
	}	
    public void ModeSelection ()
    {
        back_to_enviorment.SetActive(false);
        back_to_main.SetActive(true);
    }
   public void LevelSelection ()
    {
        back_to_enviorment.SetActive(true);
        back_to_main.SetActive(false);
    }
    public void loadcar_level()
    {
        enviroment.SetActive(false);
        car_level.SetActive(true);
        LevelSelection ();
        SwitchCarPanel ();
		GameAnalytics.instance.ModeSelectionEvent ("CAR_FirstMode");
    }
    public void loadcone_level()
    {
		//Debug.Log ("cone");
        if(PlayerPrefs.GetInt("lockCone").Equals(1))
        {
            ConeLevel ();

        }
        else
        if(PlayerPrefs.GetInt("cash") >= 10000)
        {
            PlayerPrefs.SetInt("lockCone" ,1);
            PlayerPrefs.SetInt("cash", PlayerPrefs.GetInt("cash")-10000);
            PlayerPrefs.Save(); 
            ShowCash ();
            ConeLevel ();
            lockCone.SetActive(false);

        }
		GameAnalytics.instance.ModeSelectionEvent ("Cone_SecondMode");
    }
    void ConeLevel ()
    {
        enviroment.SetActive(false);
        cone_level.SetActive(true);
        LevelSelection ();
        SwitchConePanels ();
		//Debug.Log ("cone");
    }
    public void loadblock_level()
    {
        if(PlayerPrefs.GetInt("lockBlock").Equals(1))
        {
            BlockLevel ();
        }
        else
        if(PlayerPrefs.GetInt("cash") >= 8000)
        {
            PlayerPrefs.SetInt("lockBlock" ,1);
            PlayerPrefs.SetInt("cash", PlayerPrefs.GetInt("cash")-8000);
            PlayerPrefs.Save();  
            ShowCash ();
            BlockLevel ();
            lockBlock.SetActive(false);
            
        }
		GameAnalytics.instance.ModeSelectionEvent ("Blocks_thirdMode");
    }
    void BlockLevel ()
    {
        enviroment.SetActive(false);
        block_level.SetActive(true);
        LevelSelection ();
        SwitchBlockPanels ();
    }
    public void ShowLoading ()
    {
        loadingPanel.SetActive(true);
    }
 
    public void OpenSocialDialogue ()
    {
        
    }
    public void back_envorment()
    {
        currentCarPanel = 1;
        currentCarLevel = 0;
        enviroment.SetActive(true);
        car_level.SetActive(false);
        cone_level.SetActive(false);
        block_level.SetActive(false);
        back_to_main.SetActive(true);
        back_to_enviorment.SetActive(false);
    }
    public void back_main_menu()
	{  main_menu.instance.LevelSelection_ToMenu();
		//Ads_Manager.Instance.HideSmallAdmobBanner ();
        watch_vedio.SetActive(false);
      
        
        
    }
    public void back_to_level_selection()
    {
        watch_vedio.SetActive(false);
        
    }
    public void privecy_policy()
    {
        Application.OpenURL("https://gamershivepro.wordpress.com/");
    }
 
    public void StoreReward()
    {
        PlayerPrefs.SetInt("cash", PlayerPrefs.GetInt("cash") + 100);
		main_menu.instance.wallet.text = PlayerPrefs.GetInt ("cash").ToString ();
        PlayerPrefs.Save();
        ShowCash ();
        watch_vedio.SetActive(false);
        
    }
    IEnumerator sabar1()
    {
        yield return new WaitForSeconds(1f);
        adsNotAvailable.SetActive(false);
    }
    IEnumerator sabar2()
    {
        yield return new WaitForSeconds(1f);
        the_limit_is_full.SetActive(false);
    }
    public void show_watch_vedio()
    {
        watch_vedio.SetActive(true);

    }
    public void show_vedio_only_three_times()
    {
        if (counter_vedio < 3)
        {
           // ViewAd();
        }
        else
        {
            the_limit_is_full.SetActive(true);
            StartCoroutine(sabar2());
        }
    }
    public void WatchVideo ()
    {
      //  GoogleMobileAdsManager.Instance.Show_RewardedVideo_Unity_Admob();
	//	Ads_Manager.Instance.ShowUnityRewardedVideoAd ();
    }
}

