//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using UnityEngine.SceneManagement;
//using UnityEngine.Advertisements;
//public class Ads_Manager : MonoBehaviour
//{
//	public static Ads_Manager Instance;
//	private string appID = "ca-app-pub-8220336299264563~8269741729";
//	private string bannerID = "ca-app-pub-8220336299264563/2634271665";
//	private AdPosition bannerSmallPosition = AdPosition.Bottom;
//	private AdPosition bannerLargePosition = AdPosition.BottomRight;
//	// public bool showBannerOnStart;
//	private string interstitialID = "ca-app-pub-8220336299264563/4877291624";
//	private string unityAdID = "2671146"; // unity id

//	[SerializeField]
//	private bool enableTestMode;
//	//	[Header("Rewarded Video")]
//	//	public string rewardedVideoID;
//	private BannerView smallbannerView;
//	private BannerView largebannerView;
//	private InterstitialAd interstitial;
//	//private RewardBasedVideoAd rewardBasedVideo;
//	private bool SmallBannerOnceLoaded;
//	private bool LargeBannerOnceLoaded;
//	private bool isInternet = false;
//	private bool isAdInitialized = false;
//    public static bool small_bann_bool = false;
//    private void Awake()
//	{

//		//PlayerPrefs.DeleteAll ();
//		Screen.sleepTimeout = SleepTimeout.NeverSleep;
//		if (Instance != null)
//		{
//			Destroy(this.gameObject);
//		}
//		else
//		{
//			Instance = this;
//			DontDestroyOnLoad(this.gameObject);
//		}
//	}
//	private bool CheckInitialization()
//	{
//		if (isAdInitialized)
//		{
//			isAdInitialized = true;
//			return isAdInitialized;
//		}
//		else
//		{
//			isAdInitialized = false;
//			InitializeAds();
//			return false;
//		}

//	}
//	public bool IsInternetConnection()
//	{
//		if (Application.internetReachability != NetworkReachability.NotReachable)
//		{
//			isInternet = true;
//		}
//		else
//			isInternet = false;

//		return isInternet;
//	}
//	private void Start()
//	{

//		if (enableTestMode)
//		{
//			//test ids
//			bannerID = "ca-app-pub-3940256099942544/6300978111";
//			interstitialID = "ca-app-pub-3940256099942544/1033173712";
//		}

//		SmallBannerOnceLoaded = false;
//		LargeBannerOnceLoaded = false;
//		if (IsInternetConnection())
//		{
//			InitializeAds();
//		}
//		else
//			isAdInitialized = false;
//		// Invoke("SetFalse",1);
//	}
//	void InitializeAds()
//	{
//		isAdInitialized = true;

//		MobileAds.Initialize(appID);
//		InitUnityAds();
//		//if (PlayerPrefs.GetInt("ADSUNLOCK").Equals(0))
//		//{

//			RequestBanner();
//			RequestLargeBanner();
//			RequestInterstitial();
//	//	}
//	}
//	void SetFalse()
//	{
//		// showBannerOnStart = false;
//	}
//	private AdRequest CreateAdRequest()
//	{
//		return new AdRequest.Builder().Build();
//	}
//	private void RequestBanner()
//	{
//		if (smallbannerView == null)
//		{
//			this.smallbannerView = new BannerView(bannerID, AdSize.Banner, bannerSmallPosition);
//			// Register for ad events.
//			this.smallbannerView.OnAdLoaded += this.HandleAdLoaded;
//			//this.smallbannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
//			//this.smallbannerView.OnAdOpening += this.HandleAdOpened;
//			//this.smallbannerView.OnAdClosed += this.HandleAdClosed;
//			this.smallbannerView.OnAdLeavingApplication += this.HandleAdLeftApplication;
//			// Load a banner ad.
//			this.smallbannerView.LoadAd(this.CreateAdRequest());
//			// if(!showBannerOnStart)
//			// {
//		this.smallbannerView.Hide();
//			// }
//			// else
//			// {
//			//    showBannerOnStart = false;
//			//}            
//		}
//	}
//	private void RequestLargeBanner()
//	{
//		if (largebannerView == null)
//		{
//			this.largebannerView = new BannerView(bannerID, AdSize.MediumRectangle, bannerLargePosition);
//			// Register for ad events.
//			this.largebannerView.OnAdLoaded += this.HandleLargeBannerAdLoaded;
//			//this.largebannerView.OnAdFailedToLoad += this.HandleLargeBannerAdFailedToLoad;
//			//this.largebannerView.OnAdOpening += this.HandleLargeBannerAdOpened;
//			//this.largebannerView.OnAdClosed += this.HandleLargeBannerAdClosed;
//			this.largebannerView.OnAdLeavingApplication += this.HandleLargeBannerAdLeftApplication;
//			// Load a banner ad.
//			this.largebannerView.LoadAd(this.CreateAdRequest());
//			this.largebannerView.Hide();
//		}

//	}
//	public void ShowSmallAdmobBanner()
//	{
//        //if (PlayerPrefs.GetInt("ADSUNLOCK").Equals(0))
//        //	{

//        if (IsInternetConnection())
//        {

//            if (CheckInitialization())
//            {



//                if (SmallBannerOnceLoaded)
//                {
//                    smallbannerView.Show();
//                    small_bann_bool = true;
//                }
//                else
//                {
//                    small_bann_bool = false;
//                }
//            }

//        }
//		//}
//	}

//	public void HideSmallAdmobBanner()
//	{
//		//if (PlayerPrefs.GetInt("ADSUNLOCK").Equals(0))
//		//{
//			if (SmallBannerOnceLoaded && CheckInitialization())
//				smallbannerView.Hide();
//		//}
//	}
//	public void ShowLargeAdmobBanner()
//	{
//		//if (PlayerPrefs.GetInt("ADSUNLOCK").Equals(0))
//		//{
//			if (IsInternetConnection())
//			{
//				if (CheckInitialization())
//				{
//					if (LargeBannerOnceLoaded)
//						largebannerView.Show();
//				}
//			}
//		//}
//	}

//	public void HideLargeAdmobBanner()
//	{
//		//if (PlayerPrefs.GetInt("ADSUNLOCK").Equals(0))
//		//{
//			if (LargeBannerOnceLoaded && CheckInitialization())
//				largebannerView.Hide();
//		//}
//	}
//	public void HideLargeOnRemoveAd()
//	{

//		if (LargeBannerOnceLoaded && CheckInitialization())
//			largebannerView.Hide();
//	}
//	public void DestroySmallBanner()
//	{
//		smallbannerView.Destroy();
//	}

//	public void DestroyLargeBanner()
//	{
//		largebannerView.Destroy();
//	}

//	private void RequestInterstitial()
//	{
//		// Create an interstitial.
//		this.interstitial = new InterstitialAd(interstitialID);
//		// Register for ad events.
//		this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
//		//this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
//		//	this.interstitial.OnAdOpening += this.HandleInterstitialOpened;
//		this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
//		this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;
//		// Load an interstitial ad.
//		this.interstitial.LoadAd(this.CreateAdRequest());
//	}

//	public void ShowInterstitial()
//	{
//		//if (PlayerPrefs.GetInt("ADSUNLOCK").Equals(0))
//		//{
//			if (IsInternetConnection())
//			{
//				if (CheckInitialization())
//				{
//					if (interstitial.IsLoaded())
//					{
//						interstitial.Show();
//					}
//				}
//			}
//		//}
//	}


//	/*     public void ShowRewardBasedVideo()
//        {
//                    if (rewardBasedVideo.IsLoaded())
//                    {
//                        rewardBasedVideo.Show();
//                    }
//        } */

//	#region Small Banner callback handlers

//	public void HandleAdLoaded(object sender, EventArgs args)
//	{
//		SmallBannerOnceLoaded = true;
//	}

//	/*     public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
//        {

//        } */


//	public void HandleAdLeftApplication(object sender, EventArgs args)
//	{
//		this.smallbannerView.OnAdLoaded -= this.HandleAdLoaded;
//		//this.smallbannerView.OnAdFailedToLoad -= this.HandleAdFailedToLoad;
//		//this.smallbannerView.OnAdOpening -= this.HandleAdOpened;
//		//this.smallbannerView.OnAdClosed -= this.HandleAdClosed;
//		this.smallbannerView.OnAdLeavingApplication -= this.HandleAdLeftApplication;
//	}

//	#endregion

//	#region LargeBanner callback handlers

//	public void HandleLargeBannerAdLoaded(object sender, EventArgs args)
//	{
//		LargeBannerOnceLoaded = true;
//	}

//	/*     public void HandleLargeBannerAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
//        {

//        } */
//	/*
//	public void HandleLargeBannerAdOpened(object sender, EventArgs args)
//	{

//	}

//	public void HandleLargeBannerAdClosed(object sender, EventArgs args)
//	{

//	} */

//	public void HandleLargeBannerAdLeftApplication(object sender, EventArgs args)
//	{
//		this.largebannerView.OnAdLoaded -= this.HandleLargeBannerAdLoaded;
//		//this.largebannerView.OnAdFailedToLoad -= this.HandleLargeBannerAdFailedToLoad;
//		//this.largebannerView.OnAdOpening -= this.HandleLargeBannerAdOpened;
//		//this.largebannerView.OnAdClosed -= this.HandleLargeBannerAdClosed;
//		this.largebannerView.OnAdLeavingApplication -= this.HandleLargeBannerAdLeftApplication;
//	}

//	#endregion

//	#region Interstitial callback handlers

//	public void HandleInterstitialLoaded(object sender, EventArgs args)
//	{

//	}

//	/*     public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
//        {

//        } */

//	/* 	public void HandleInterstitialOpened(object sender, EventArgs args)
//	{

//	} */

//	public void HandleInterstitialClosed(object sender, EventArgs args)
//	{
//		RequestInterstitial();
//	}

//	public void HandleInterstitialLeftApplication(object sender, EventArgs args)
//	{

//		this.interstitial.OnAdLoaded -= this.HandleInterstitialLoaded;
//		//this.interstitial.OnAdFailedToLoad -= this.HandleInterstitialFailedToLoad;
//		//this.interstitial.OnAdOpening -= this.HandleInterstitialOpened;
//		this.interstitial.OnAdClosed -= this.HandleInterstitialClosed;
//		this.interstitial.OnAdLeavingApplication -= this.HandleInterstitialLeftApplication;
//	}

//	#endregion



//	//==================================================================================================================//

//	//=============================================  Unity Ads  ========================================================//

//	public void InitUnityAds()
//	{
//		string gameId = null;

//		#if UNITY_ANDROID
//		gameId = unityAdID;
//		#elif UNITY_IOS
//		gameId = iOSGameID;
//		#endif

//		if (string.IsNullOrEmpty(gameId))
//		{
//			// Make sure the Game ID is set.
//			Debug.LogError("Failed to initialize Unity Ads. Game ID is null or empty.");
//		}
//		else if (!Advertisement.isSupported)
//		{
//			//Debug.LogWarning("Unable to initialize Unity Ads. Platform not supported.");
//		}
//		else if (Advertisement.isInitialized)
//		{
//			Debug.Log("Unity Ads is already initialized.");
//		}
//		else
//		{
//			#if UNITY_EDITOR
//			Debug.Log(string.Format("Initialize Unity Ads using Game ID {0} with Test Mode {1}.",
//				gameId, enableTestMode ? "enabled" : "disabled"));
//			#endif
//			Advertisement.Initialize(gameId, enableTestMode);
//		}
//	}


//	public void ShowUnityVideoAd()
//	{
//		/*         if (Advertisement.IsReady("video"))
//                {
//                    Advertisement.Show("video");
//                } */
//	}
//	public bool isRewardedReady()
//	{
//		bool isCheck = false;
//		if (IsInternetConnection())
//		{
//			if (CheckInitialization())
//			{
//				if (Advertisement.IsReady("rewardedVideo"))
//					isCheck = true;
//			}

//		}
//		return isCheck;
//	}
//	public void ShowUnityRewardedVideoAd()
//	{
//		ShowOptions options = new ShowOptions();
//		options.resultCallback = HandleShowResult;

//		if (Advertisement.IsReady("rewardedVideo"))
//		{
//			Advertisement.Show("rewardedVideo", options);
//		}
//	}

//	private void HandleShowResult(ShowResult result)
//	{
//		switch (result)
//		{
//		case ShowResult.Finished:
//			{
//				GiveReward ();
			
//			}
//			break;
//		case ShowResult.Skipped:
//			//Debug.LogWarning("Video was skipped.");
//			break;
//		case ShowResult.Failed:
//			Debug.LogError("Video failed to show.");
//			break;
//		}
//	}

//	//==================================================================================================================//

//	//=============================================  Priority Ads  =====================================================//


//	public void Show_Unity()
//	{
//		ShowUnityVideoAd();
//	}
//	public void Show_Unity_Admob()
//	{
//		//if (PlayerPrefs.GetInt("ADSUNLOCK").Equals(0))
//		//{
//			if (IsInternetConnection())
//			{
//				if (CheckInitialization())
//				{
//					if (Advertisement.IsReady("video"))
//					{
//						Advertisement.Show("video");
//					}
//					else
//					{
//						ShowInterstitial();
//					}
//				}
//			}
//		//}
//	}
//	public void GiveReward()
//	{
//		if (SceneManager.GetActiveScene().name.Equals("complete"))
//		{
//			complete_manager.instance.DoubleReward();
//		}
//		else
//			if (SceneManager.GetActiveScene().name.Equals("main_menu"))
//			{
//				level_selection_manager.instance.StoreReward();
//			}
//	}

//}

