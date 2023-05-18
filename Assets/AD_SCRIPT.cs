//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AD_SCRIPT : MonoBehaviour
//{
//    public static AD_SCRIPT Instance;
//    public GameObject ad1, ad2, ad3;
//    private bool isInternet = false;

    

//    // Start is called before the first frame update
//    void Start()
//    {
//        if (IsInternetConnection())
//        {
//            Debug.Log("Internet is Connected");
//            Invoke("small_bann_check", 2.5f);

//        }
//        else
//        {
//            ad1.SetActive(true);
//            Invoke("Ad2_show", 2f);
            
//        }
//    }

//   void small_bann_check()
//    {
//        if(Ads_Manager.small_bann_bool == true)
//        {
//            Debug.Log("small banner is showing");
          

//        }
//        else
//        {
//            ad1.SetActive(true);
//            Invoke("Ad2_show", 2f);
//        }

//        if (PlayerPrefs.GetInt("_scene")==0)
//            {
//            Debug.Log("Scene_1");

//        }else if(PlayerPrefs.GetInt("level")>=3)
//        {
//            ad1.SetActive(true);
//            Invoke("Ad2_show", 2f);
//        }
//    }

//    public bool IsInternetConnection()
//    {
//        if (Application.internetReachability != NetworkReachability.NotReachable)
//        {
//            isInternet = true;
//        }
//        else
//            isInternet = false;

//        return isInternet;
//    }

//    void Ad2_show()
//    {
//        ad2.SetActive(true);
//        ad3.SetActive(false);
//        Invoke("Ad3_show", 2f);
//    }
   

//    public void Ad3_show()
//    {
//        ad2.SetActive(false);
//        ad3.SetActive(true);
//        Invoke("Ad1_show", 2f);

//    }
//    public void Ad1_show()
//    {
//        ad2.SetActive(false);
//        ad3.SetActive(false);
//        ad1.SetActive(true);
//        Invoke("Ad2_show", 2f);
//    }
//        public void ad1_link()
//    {
//        Application.OpenURL("https://play.google.com/store/apps/details?id=com.zact.bicycle.endless.rider.game.free&referrer=utm_source=prado_parking");
//    }
//    public void ad2_link()
//    {
//        Application.OpenURL("https://play.google.com/store/apps/details?id=com.zact.water.park.stuntman.game.free&referrer=utm_source=prado_parking");
//    }

//    public void ad3_link()
//    {
//        Application.OpenURL("https://play.google.com/store/apps/details?id=com.door.modren.car.traffic.apps&referrer=utm_source=prado_parking");
//    }
//}
