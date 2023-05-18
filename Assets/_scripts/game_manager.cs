using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class game_manager : MonoBehaviour {
   public static game_manager instance;
   private GameObject spawnpoint;
    public GameObject[] mode1SpawnPoints,mode2SpawnPoints,mode3SpawnPoints;
    public GameObject [] playerVehicles;
    private int currentVehicle = 0;

    //public GameObject animation;

    public GameObject pause_Canvas,gamePlay_Canvas,failed_Canavas,setting_canvas; // //
    public GameObject instruct_park_straigth; //
    public GameObject instruct_park_reverse;  //
    public GameObject levelskipped;
    public GameObject already_skipped;
    public GameObject notenoughcash;
    public GameObject skip_panal;
    public GameObject fail_panal,startingPanel;
    public int cash;
    public Image parking_area;
    public Image sign;
    int current_level = 0;
    public GameObject cars_enviorment;//,parkingEnvironment;
    private static GameObject CarsEnvironment;
    public GameObject cones_and_block_level;
    private static GameObject conesBlock_Level;
    public Material rings;
    //public Scrollbar gear;
    //float value_gear;
    public ButtonInput gas, brake,brakeTilt,left,right;
    private float gasInput,brakeInput, leftInput,rightInput,accelInput,steerInput;
  [HideInInspector]
    public int controlNumber = 5;
    public GameObject gearForward,grearReverse,steerWheel;
    private bool isForward = true; // forward , backward
    public Camera mainCamera, topCamera;
    private int cameraNumber = 0;
  // Dictionary<string, object> levelDict = new Dictionary<string, object>();

	public GameObject ImgHand;
	public Text levelNumberText;
	public GameObject frontmirror;

    void Awake ()
    {
        instance = this;
	
	
		frontmirror.SetActive (false);
		ImgHand.SetActive (false);
        currentVehicle = PlayerPrefs.GetInt("currentVehicle");
       // currentVehicle = 5;
        StartingPanel_Status(true);
        Invoke("show_instruct",0.4f);
        //show_instruct();
        RingColor ();
        Invoke("ChangeRingColors",1);
		int levelnum;
		levelnum = PlayerPrefs.GetInt ("funnelLevelNumber");
		levelNumberText.text = levelnum.ToString ();

		gas.GetComponent<Animation>().Play ("gasBtnAnim");
       // LevelStart_Events ();
        if( main_menu.instance)
        main_menu.isLevelScreen = false;
	//	Ads_Manager.Instance.HideSmallAdmobBanner ();
	
		if(PlayerPrefs.GetInt("funnelLevelNumber")==1)
			{

			gas.GetComponent<Animation> ().Play ("gasBtnAnim");
			gas.GetComponent<Shadow>().enabled = true;
		}else
		{
			gas.GetComponent<Animation>().Stop ("gasBtnAnim");
			gas.GetComponent<Shadow>().enabled = false;
		}
		if(PlayerPrefs.GetInt("funnelLevelNumber")==6)
		{

			Invoke ("imgAct",0.5f);

		}

		GameAnalytics.instance.LevelStartEvent (PlayerPrefs.GetInt("funnelLevelNumber"));
	//	Ads_Manager.Instance.HideLargeAdmobBanner ();
    }

	void imgAct()
	{
		ImgHand.SetActive (true);
	}
    public void ControlSelection (int cNumber)
    {
        if(cNumber.Equals(0))
        {
            brake.transform.gameObject.SetActive(false);
          //  brakeTilt.transform.gameObject.SetActive(true); // tilt
            left.transform.gameObject.SetActive(true);
            right.transform.gameObject.SetActive(true);
            steerWheel.SetActive(false);
        }
        else
        if(cNumber.Equals(1))
        {           
            brake.transform.gameObject.SetActive(true);
            brakeTilt.transform.gameObject.SetActive(false);
            left.transform.gameObject.SetActive(true);
            right.transform.gameObject.SetActive(true);            
            steerWheel.SetActive(false);
        }
        else
        {      
            brake.transform.gameObject.SetActive(true);
            brakeTilt.transform.gameObject.SetActive(false);
            left.transform.gameObject.SetActive(false);
            right.transform.gameObject.SetActive(false);            
            steerWheel.SetActive(true);
        }

        controlNumber = cNumber;
    }

    float GetInput(ButtonInput button){

		if(button == null)
			return 0f;

		return(button.input);

	}
    public void StartingPanel_Status (bool status)
    {
        if(startingPanel)
        startingPanel.SetActive(status);
    }
//    public void LevelStart_Events ()
//	{
//        //levelDict.Add("level_mode", PlayerPrefs.GetInt("currentMode") );
//        levelDict.Add("level_index", PlayerPrefs.GetInt("funnelLevelNumber") );
//        Analytics.CustomEvent("vc30_Start"+PlayerPrefs.GetInt("funnelLevelNumber").ToString(), levelDict);
//        levelDict.Clear();
//		//AnalyticsEvent.LevelStart ()
//		// nalytics.CustomEvent("vc20_start", new Dictionary <string, object> {
//		// 	{ "level_index", PlayerPrefs.GetInt("level") }
//		// } );
//	}
    void RingColor ()
    {
        PlayerPrefs.SetInt("back",0);
        PlayerPrefs.SetInt("front",0);
        PlayerPrefs.SetInt("wrong_back",0);
        PlayerPrefs.SetInt("wrong_front",0);
    }
    public void ChangeRingColors ()
    {
        // Debug.LogError("Resolve it");
        Debug.Log("out");
        int back = PlayerPrefs.GetInt("back");
        int front = PlayerPrefs.GetInt("front");
        // int wrong_back = PlayerPrefs.GetInt("wrong_back");
        // int wrong_front = PlayerPrefs.GetInt("wrong_front");
        if (back == 1 && front == 1)
        {
            Debug.Log("innn");
            rings.color = Color.green;
           // rings.GetComponent<MeshRenderer>().sharedMaterial.color = Color.green;
            parking_area.fillAmount += 0.5f * Time.deltaTime;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().isKinematic = true;
            
            
        }
        // else if (wrong_back==1 && wrong_front==1)
        // {
        //     rings.color = Color.red;
        //     //rings.GetComponent<MeshRenderer>().sharedMaterial.color = Color.red;
        // }
        else
        {
            parking_area.fillAmount -= 0.5f * Time.deltaTime;
            rings.color = Color.yellow;
            //rings.GetComponent<MeshRenderer>().sharedMaterial.color = Color.yellow;
        }
    }
    public void ShiftGear (bool R)
    {
        isForward = R;
        gearForward.SetActive(R);
        grearReverse.SetActive(!R);
		VehicleController.instance.ToggleReverse(!R);
		if(R)
		{
			frontmirror.SetActive (false);
		
			VehicleController.instance.cameraRev.SetActive (false);
		}
		else if(!R)
		{
			frontmirror.SetActive (true);

		

			VehicleController.instance.cameraRev.SetActive (true);


		}
  

	



    }
    void Show_ParkingEnvironment ()
    {
        //parkingEnvironment.SetActive(true);

        //Instantiate(parkingEnvironment , parkingEnvironment.transform.position, parkingEnvironment.transform.rotation);
    }
    // Use this for initialization
    void Start () {
        //Show_ParkingEnvironment ();
      //
      
        playerVehicles[currentVehicle].SetActive(true);
		sign.enabled = false;
        if(sound_manager.instance)
        {
            sound_manager.instance.gamePlayMusic = mainCamera.transform.GetComponent<AudioSource>();
        }

        cash = PlayerPrefs.GetInt("cash");
        PlayerPrefs.SetInt("back",0);
        PlayerPrefs.SetInt("front",0);
        Time.timeScale = 1;
        LoadSceneObjects ();
        Invoke("Find_TopCamera",0.2f);
		//Ads_Manager.Instance.HideLargeAdmobBanner ();
		//Ads_Manager.Instance.HideSmallAdmobBanner ();

    }


	public void GasPress()
	{
		VehicleController.instance.rb.drag = 0.65f;
		VehicleController.instance.CancelInvoke ();
	//	ButtonInput.instance.pressing = true;
//		if (ButtonInput.instance.pressing==null) {
//			ButtonInput.instance.pressing = true;
//		}
	
	}




    void Find_TopCamera ()
    {
        topCamera = GameObject.Find("TopCamera").GetComponent<Camera>();
    }
    void LoadSceneObjects ()
    {
        current_level = PlayerPrefs.GetInt("level");
       
        switch(PlayerPrefs.GetInt("currentMode"))
        {
            case 1:
                if(mode1SpawnPoints[current_level])
                {
                    //Debug.Log("current_level:"+current_level);
                    playerVehicles[currentVehicle].transform.position = mode1SpawnPoints[current_level].transform.position;
                    playerVehicles[currentVehicle].transform.localRotation = mode1SpawnPoints[current_level].transform.localRotation;
                    Show_CarEnvironment ();
                }
            break;
            case 2:
                if(mode2SpawnPoints[current_level])
                {
                    playerVehicles[currentVehicle].transform.position = mode2SpawnPoints[current_level].transform.position;
                    playerVehicles[currentVehicle].transform.localRotation = mode2SpawnPoints[current_level].transform.localRotation;
                    Show_ConesAndBlock ();
                }
            break;
            case 3:
                if(mode3SpawnPoints[current_level])
                {
                    playerVehicles[currentVehicle].transform.position = mode3SpawnPoints[current_level].transform.position;
                    playerVehicles[currentVehicle].transform.localRotation = mode3SpawnPoints[current_level].transform.localRotation;
                    Show_ConesAndBlock ();
                }
            break;
            default :                
            break;                         
        }
        EngineSound ();
        SetMusic(1.2f);
    }


    void Update()
    {
//#if !UNITY_EDITOR
        if(controlNumber.Equals(0))
        {
            accelInput = Input.acceleration.x;
            brakeInput = GetInput(brakeTilt);
         //   VehicleController.instance.steerVal = accelInput;
        }
        else
        if(controlNumber.Equals(1))
        {
//button
            leftInput = GetInput(left);
            rightInput = GetInput(right);
            brakeInput = GetInput(brake);
         //   VehicleController.instance.steerVal = -leftInput+rightInput;
        }
        else
       
        gasInput = GetInput(gas);
        
     //   VehicleController.instance.handBrakeValue = brakeInput;

		//if (isForward) 
		//	VehicleController.instance.torqueVal = gasInput;
		
		
		//gas.gameObjec
   //     else

			//VehicleController.instance.torqueVal = -gasInput;
		
       
        if(gasInput>0.5f)

        AccelerateSound();
		
        else
        IdleEngineSound ();


//#endif

        if(parking_area.fillAmount>0)

        {
            parking_area.fillAmount += 0.5f * Time.deltaTime;
        }
        if (parking_area.fillAmount == 1)
        {
            sign.enabled = true;
            if( main_menu.instance)
            main_menu.isLevelScreen = true;
            SceneManager.LoadScene("complete");
           


        }
	
    }
    public void EngineSound ()
    {
        Debug.Log("from_setting : "+PlayerPrefs.GetInt("from_setting"));
        if (PlayerPrefs.GetInt("from_setting") == 0)
        {
            if(sound_manager.instance)
            sound_manager.instance.gamePlayMusic.Pause();
        }
        else if (PlayerPrefs.GetInt("from_setting") == 1)
        {
            if(sound_manager.instance)
            sound_manager.instance.gamePlayMusic.Play();
        }
    }
    void AccelerateSound ()
    {
        if(sound_manager.instance)
        sound_manager.instance.gamePlayMusic.pitch = 1.5f;
    }
    void IdleEngineSound ()
    {
        if(sound_manager.instance)
        sound_manager.instance.gamePlayMusic.pitch = 1.7f;        
    }
    public void EngineSoundOff ()
    {
        if(sound_manager.instance)
        sound_manager.instance.gamePlayMusic.Pause();
    }
    public void level_selection()
    {
        SceneManager.LoadScene("main_menu");
    }
    public void SetMusic (float volume)
    {
       if (PlayerPrefs.GetInt("from_setting_music") == 1)
        {
            if(sound_manager.instance)
            sound_manager.instance.menuMusic.volume = volume;
        }
        else
        {
            if(sound_manager.instance)
            sound_manager.instance.menuMusic.Pause();
        }
    }
   // public void Level_Events (string value)
	//{
        // Pause , Resume
        // levelDict.Add("level_mode", PlayerPrefs.GetInt("currentMode") );
        // levelDict.Add("level_index", PlayerPrefs.GetInt("level") );
        // Analytics.CustomEvent("vc20_"+value, levelDict);
        // levelDict.Clear();
	//}
    public void pause()
    {
        SetMusic(0.4F);
        EngineSoundOff ();
        PauseComponent(true);        
        PlayComponent(false);
        if(sound_manager.instance)
        sound_manager.instance.pause++;
		//LevelPaused_Events();
        Time.timeScale = 0;
		//Ads_Manager.Instance.HideSmallAdmobBanner ();
		//Ads_Manager.Instance.ShowLargeAdmobBanner ();
        PlayerPrefs.SetInt("AdCount", PlayerPrefs.GetInt("AdCount") + 1);
        if (PlayerPrefs.GetInt("AdCount") == 2)
        {
          
		//	Ads_Manager.Instance.Show_Unity_Admob ();
			PlayerPrefs.SetInt("AdCount", 0);
        }
    }




    public void setting()
    {
        PauseComponent(false);  
        SettingComponent(true);
    }
    public void return_from_setting()
    {
        PauseComponent(true);  
        SettingComponent(false);
    }

    public void resume()
    {
        Time.timeScale = 1;
        SetMusic(0.05F);
        EngineSound ();
        PlayComponent(true);
        PauseComponent(false);
       // Level_Events("Resume");
	//	Ads_Manager.Instance.HideLargeAdmobBanner ();
    }
    public void restart()
    {
        SetMusic(0.4F);
        SceneManager.LoadScene("loading");
	//	Ads_Manager.Instance.HideLargeAdmobBanner ();
    }
    public void menu()
    {
        SetMusic(0.4F);
        SceneManager.LoadScene("main_menu");
        Time.timeScale = 1;
	//	Ads_Manager.Instance.HideLargeAdmobBanner ();
    }
    public void show_straight()
    {
        ParkStraightComponent(true);
        Time.timeScale = 0;
    }
    public void show_reverse()
    { 
        ParkReverseComponent(true); 
        Time.timeScale = 0;
    }
    public void ok_instruct()
    {
        Time.timeScale = 1;
        PlayComponent(true);        
        ParkReverseComponent(false);
        ParkStraightComponent(false);
        FailedComponent(false);
        PauseComponent(false);
        SettingComponent(false);
        EngineSound ();
    }

    void PauseComponent (bool status)
    {
        if(status)
        {
            pause_Canvas.GetComponent<CanvasScaler>().enabled = true;            
            pause_Canvas.GetComponent<Canvas>().enabled = true;
            pause_Canvas.GetComponent<GraphicRaycaster>().enabled = true;
        }
        else
        {
            pause_Canvas.GetComponent<Canvas>().enabled = false;
            pause_Canvas.GetComponent<GraphicRaycaster>().enabled = false;
            pause_Canvas.GetComponent<CanvasScaler>().enabled = false;            
        }  
    }
    void PlayComponent (bool status)
    {
        if(status)
        {
            gamePlay_Canvas.GetComponent<CanvasScaler>().enabled = true;
            gamePlay_Canvas.GetComponent<Canvas>().enabled = true;
            gamePlay_Canvas.GetComponent<GraphicRaycaster>().enabled = true;
        }
        else
        {
            gamePlay_Canvas.GetComponent<Canvas>().enabled = false;
            gamePlay_Canvas.GetComponent<GraphicRaycaster>().enabled = false;
            gamePlay_Canvas.GetComponent<CanvasScaler>().enabled = false;            
        }  
    }
    void FailedComponent (bool status)
    {
        if(status)
        {
            failed_Canavas.GetComponent<CanvasScaler>().enabled = true;            
            failed_Canavas.GetComponent<Canvas>().enabled = true;
            failed_Canavas.GetComponent<GraphicRaycaster>().enabled = true;
          //  LevelFailed_Events ();
			GameAnalytics.instance.LevelFailedEvent (PlayerPrefs.GetInt("funnelLevelNumber"));
        }
        else
        {
            failed_Canavas.GetComponent<Canvas>().enabled = false;
            failed_Canavas.GetComponent<GraphicRaycaster>().enabled = false;
            failed_Canavas.GetComponent<CanvasScaler>().enabled = false;           
        }  
    }
//    public void LevelFailed_Events ()
//	{
//        levelDict.Add("level_mode", PlayerPrefs.GetInt("currentMode") );
//        levelDict.Add("level_index", PlayerPrefs.GetInt("level") );
//		Analytics.CustomEvent ("vc30_Failed"+PlayerPrefs.GetInt("funnelLevelNumber").ToString(), levelDict);
//	
//       // Analytics.CustomEvent("vc22_Failed", levelDict);
//        levelDict.Clear();
//	}
//	public void LevelPaused_Events ()
//	{
//		levelDict.Add("level_mode", PlayerPrefs.GetInt("currentMode") );
//		levelDict.Add("level_index", PlayerPrefs.GetInt("level") );
//		Analytics.CustomEvent ("vc31_Paused"+PlayerPrefs.GetInt("funnelLevelNumber").ToString(), levelDict);
//		// Analytics.CustomEvent("vc22_Failed", levelDict);
//		levelDict.Clear();
//	}
    void ParkStraightComponent (bool status)
    {
        if(status)
        {
            instruct_park_straigth.GetComponent<CanvasScaler>().enabled = true;             
            instruct_park_straigth.GetComponent<Canvas>().enabled = true;
            instruct_park_straigth.GetComponent<GraphicRaycaster>().enabled = true;
        }
        else
        {
            instruct_park_straigth.GetComponent<Canvas>().enabled = false;
            instruct_park_straigth.GetComponent<GraphicRaycaster>().enabled = false;
            instruct_park_straigth.GetComponent<CanvasScaler>().enabled = false;            
        }  
    }
    void ParkReverseComponent (bool status)
    {
        if(status)
        {
            instruct_park_reverse.GetComponent<CanvasScaler>().enabled = true;            
            instruct_park_reverse.GetComponent<Canvas>().enabled = true;
            instruct_park_reverse.GetComponent<GraphicRaycaster>().enabled = true;
        }
        else
        {
            instruct_park_reverse.GetComponent<Canvas>().enabled = false;
            instruct_park_reverse.GetComponent<GraphicRaycaster>().enabled = false;
            instruct_park_reverse.GetComponent<CanvasScaler>().enabled = false;           
        }  
    }
    void SettingComponent (bool status)
    {
        if(status)
        {
            setting_canvas.GetComponent<CanvasScaler>().enabled = true;            
            setting_canvas.GetComponent<Canvas>().enabled = true;
            setting_canvas.GetComponent<GraphicRaycaster>().enabled = true;
        }
        else
        {
            setting_canvas.GetComponent<Canvas>().enabled = false;
            setting_canvas.GetComponent<GraphicRaycaster>().enabled = false;
            setting_canvas.GetComponent<CanvasScaler>().enabled = false;        
        }  
    }                          
    public void open_skip()
    {
        Time.timeScale = 1;
        Failed_Canvas ();
        skip_panal.SetActive(true);
        fail_panal.SetActive(false);
    }
    public void close_skip()
    {
        Failed_Canvas ();
        skip_panal.SetActive(false);
        fail_panal.SetActive(true);
    }
    public void SwitchCamera ()
    {
        cameraNumber +=1;
        if(cameraNumber == 1)
        {
            mainCamera.enabled = false;
            topCamera.enabled = true;
        }
        else
        {
            mainCamera.enabled = true;
            topCamera.enabled = false;  
            cameraNumber = 0;
        }
    }
    public void skip_level()
    {
        Time.timeScale = 1;
        if (cash >= 800)
        {
        current_level = PlayerPrefs.GetInt("level");
        switch(PlayerPrefs.GetInt("currentMode"))
        {
            case 1:

                if (PlayerPrefs.GetInt("M1 "+(current_level+1).ToString()) == 1)
                {
                    Failed_Canvas ();
                    already_skipped.SetActive(true);
                    skip_panal.SetActive(false);
                    StartCoroutine(sabar3());
                }
                else
                {
                    PlayerPrefs.SetInt("M1 "+(current_level+1).ToString(), 1);
                    Failed_Canvas ();
                    levelskipped.SetActive(true);
                    skip_panal.SetActive(false);
                    cash = cash - 800;
                    PlayerPrefs.SetInt("cash", cash);
                    StartCoroutine(sabar1());
                    StartCoroutine(change_scene());
                }

            break;
            case 2:

                    if (PlayerPrefs.GetInt("M2 "+(current_level+1).ToString()) == 1)
                    {
                        Failed_Canvas ();
                        already_skipped.SetActive(true);
                        skip_panal.SetActive(false);
                        StartCoroutine(sabar3());
                    }
                    else
                    {
                        Failed_Canvas ();
                        PlayerPrefs.SetInt("M2 "+(current_level+1).ToString(), 1);
                        levelskipped.SetActive(true);
                        skip_panal.SetActive(false);
                        cash = cash - 800;
                        PlayerPrefs.SetInt("cash", cash);
                        StartCoroutine(sabar1());
                        StartCoroutine(change_scene());
                    }
            break;
            case 3:
               if (PlayerPrefs.GetInt("M3 "+(current_level+1).ToString()) == 1)
                {
                    Failed_Canvas ();
                    already_skipped.SetActive(true);
                    skip_panal.SetActive(false);
                    StartCoroutine(sabar3());
                }
                else
                {
                    Failed_Canvas ();
                    PlayerPrefs.SetInt("M3 "+(current_level+1).ToString(), 1);
                    levelskipped.SetActive(true);
                    skip_panal.SetActive(false);
                    cash = cash - 800;
                    PlayerPrefs.SetInt("cash", cash);
                    StartCoroutine(sabar1());
                    StartCoroutine(change_scene());
                }

            break;
            default :
            Failed_Canvas ();
            notenoughcash.SetActive(true);
            skip_panal.SetActive(false);
            StartCoroutine(sabar2());
            break;                         
        } 
        }           
    }
    public void LevelFailed ()
    {
        EngineSoundOff();
        SetMusic(0.6f);
        gamePlay_Canvas.GetComponent<Canvas>().enabled = false;
        failed_Canavas.GetComponent<Canvas>().enabled = true;
        failed_Canavas.GetComponent<GraphicRaycaster>().enabled = true;

    }
    void Failed_Canvas ()
    {
        FailedComponent(true);
    }
    void Show_CarEnvironment ()
    {
        if(CarsEnvironment == null)
        {
            CarsEnvironment = Instantiate(cars_enviorment , cars_enviorment.transform.position, cars_enviorment.transform.rotation);
            DontDestroyOnLoad(CarsEnvironment);
            if(conesBlock_Level)
            DestroyImmediate(conesBlock_Level);
        }
    }
    public void show_instruct()
    {
        StartingPanel_Status(false);
        //current_level = PlayerPrefs.GetInt("level");
        // switch(PlayerPrefs.GetInt("currentMode"))
        // {
        //     case 1:
        //     switch(current_level)
        //     {
        //         case 0: case 1: case 2: case 5: case 7: case 12:  case 13: case 19: case 22: case 23: 
        //         show_straight();
        //         break;
        //         case 3: case 4: case 6: case 8:  case 9: case 10:case 11: case 14: case 15: case 16: case 17: case 18: case 20: case 21:case 24:  case 25: case 26: case 27:   case 28: case 29:
        //         show_reverse();
        //         break;                
        //     }

        //     break;
        //     case 2:
        //     switch(current_level)
        //     {
        //         case 1: case 3: case 4: case 5: case 6: case 7:
        //         show_straight();
        //         break;
        //         case 0: case 2: case 8:
        //         show_reverse();
        //         break;                
        //     }
        //     break;
        //     case 3:
        //     switch(current_level)
        //     {
        //         case 0: case 1: case 2: case 3: case 4: 
        //         show_straight();
        //         break;
        //          case 8:
        //         //show_reverse();
        //         break;                
        //     }
        //     break;
        //     default :
                
        //     break;                         
        // }

    }
    void Show_ConesAndBlock ()
    {
        if(conesBlock_Level == null)
        {
            conesBlock_Level = Instantiate(cones_and_block_level , cones_and_block_level.transform.position, cones_and_block_level.transform.rotation);
            DontDestroyOnLoad(conesBlock_Level);
            if(CarsEnvironment)
            DestroyImmediate(CarsEnvironment);
        }

    }
    public void more_by_us()
    {
         Application.OpenURL("https://play.google.com/store/apps/developer?id=Knock-Solutions+(Gamtech)+Inc.");
    }

    IEnumerator sabar1()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(1f);
        levelskipped.SetActive(false);
    }
    IEnumerator sabar2()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(1f);
        notenoughcash.SetActive(false);
        Failed_Canvas ();
        fail_panal.SetActive(true);
    }
    IEnumerator sabar3()
    {
         Time.timeScale = 1;
        yield return new WaitForSeconds(1f);
        already_skipped.SetActive(false);
        Failed_Canvas ();
        fail_panal.SetActive(true);
    }
    IEnumerator turn_on_time()
    {
        yield return new WaitForSeconds(0.000005f);
        Time.timeScale = 0;
    }
    IEnumerator change_scene()
    {
        Time.timeScale = 1;

        yield return new WaitForSeconds(1f);
        LevelSkipped_ToMenu ();
    }
    public void AlreadySkipped_ToFail ()
    {
        already_skipped.SetActive(false);
        fail_panal.SetActive(true);        
    }
    public void NotEnough_ToFail ()
    {
        notenoughcash.SetActive(false);
        fail_panal.SetActive(true);        
    }
    public void LevelSkipped_ToMenu ()
    {
        Time.timeScale = 1;
        if( main_menu.instance)
        main_menu.isLevelScreen = true;
        SetMusic(0.4F);
        SceneManager.LoadScene("main_menu");
	//	Ads_Manager.Instance.HideLargeAdmobBanner ();  
    }

    public void BrakesApplied(bool flag)
    {
      //  VehicleController.instance.ToggleBrakes(flag);
    }


}
