using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class parado_manger : MonoBehaviour
{
    public static parado_manger GM;
    public enum GameControl
    {
        Paddle = 0,
        Steering = 1,
        Tilt = 2
    };


    [Header("UI Related")]
    public Button For;
    public Button Rev;

    [Space]
    public GameObject PausePanel;
    public GameObject CompletePanel;
    public GameObject FailedPanel;
    public GameObject ComingSoonPanel;

    [Space]
    public GameObject BrakeLights;
    public GameObject ReverseLights;

    [Space]
    public GameObject ControlsParent;
    public GameObject[] ControlUI;
    public GameObject[] ControlImages;
    public Text currentLevel_Label;

    public GameControl AssignedControl;

    [Space]
    public Transform handImage;

    [Space]
    [Space]

    [Header("Essential Game Objects of Game")]
    public GameObject MainCamera;
    public GameObject PlayerCars;
    public GameObject environment;
    public GameObject RevivePanel;
    public AudioSource aud;

    [Space]
    public GameObject[] Levels;
    public Transform[] SpawnPoints;

    [Space]
    [Space]

    [Header("Variables")]
    public Material DummyMaterial;
    public AudioSource Music;
    public Text levelWinLabel;
    public Text gameScoreLabel;
    int scoreCounter;



    [HideInInspector]
    public bool Accelerate, Brake, Left, Right;


    [HideInInspector]
    public int Direction;
  public  int Control_Index;
    private Transform spawn;

    private int camIndex;
    private int color_index;
    private int LevelNo;

    private bool IsTutorialLevel;

    public Transform RespawnPoint;
    public Vector3 Prev_CamOffset;
    private bool RevivedOnce;

    public GameObject counter_image;

    public  bool Reverse_bool;
    public  bool break_bool;
    void Awake()
    {
        GM = this;
        PlayerCars = GameObject.FindGameObjectWithTag("Player");
        RevivedOnce = false;
//        AudioManager.instance.SlowDownMusic();
        LevelNo = PlayerPrefs.GetInt("CurrentLevel", 1);
        PlayerPrefs.SetInt("TotalLevels", Levels.Length);
        AnalyticsEvent.LevelStart(LevelNo);
       // Levels[LevelNo - 1].SetActive(true);
       

    }
    void Start()
    {

      //  counter_image.SetActive(false);

      //  PlayerPrefs.SetInt("CurrentLevel",15);
        scoreCounter = 0;
        Debug.Log(PlayerPrefs.GetInt("ControlIndex"));
        //     Control_Index = PlayerPrefs.GetInt("ControlIndex");
      //  Control_Index = 1;
        Direction = 1;
        SwitchControl();
      //  currentLevel_Label.text = "Level " + LevelNo.ToString();
        camIndex = 0;
        EngineStart();
       


        if(PlayerPrefs.GetInt("CurrentLevel") >= 15)
        {
            counter_image.SetActive(true);
        }

        Reverse_bool = false;
        break_bool = false;


    }
    void SpawnCar()
    {
    }
    void SpawnEnvironment()
    {

    }


    public void ChangeDirection(int v)
    {
        Direction = v;
        if (v == 1)
        {

            For.gameObject.SetActive(false);
            Rev.gameObject.SetActive(true);
            Reverse_bool = false;
            ReverseLights.SetActive(false);
            FollowCam.Instance.offset.z *= -1;
        }
        else
        {
            ReverseLights.SetActive(true);
            Reverse_bool = true;
            For.gameObject.SetActive(true);
            Rev.gameObject.SetActive(false);

            FollowCam.Instance.offset.z *= -1;
        }
    }
    public void ChangeControls()
    {
        Control_Index++;

        if (Control_Index == ControlUI.Length)
            Control_Index = 0;
        SwitchControl();
    }
   
  public  void SwitchControl()
    {
        if (Control_Index == 0)
        {
            PlayerPrefs.SetInt("ControlIndex", 0);
            ControlUI[0].SetActive(true);
            ControlUI[1].SetActive(false);
            //ControlUI[2].SetActive(false);
          //  ControlImages[0].SetActive(false);
         //   ControlImages[2].SetActive(false);
        //   ControlImages[1].SetActive(true);
            AssignedControl = GameControl.Paddle;

        }
        else if (Control_Index == 1)
        {
            PlayerPrefs.SetInt("ControlIndex", 1);
            ControlUI[0].SetActive(false);
            ControlUI[1].SetActive(true);
         //   ControlUI[2].SetActive(false);
        //    ControlImages[2].SetActive(true);
           // ControlImages[1].SetActive(false);
          //  ControlImages[0].SetActive(false);
            AssignedControl = GameControl.Steering;
        }
        else
        {
            PlayerPrefs.SetInt("ControlIndex", 2);
            ControlUI[0].SetActive(false);
            ControlUI[1].SetActive(false);
            ControlUI[2].SetActive(true);

          //  ControlImages[1].SetActive(false);
          //  ControlImages[0].SetActive(true);
          //  ControlImages[2].SetActive(false);
            AssignedControl = GameControl.Tilt;
        }
        Debug.Log(PlayerPrefs.GetInt("ControlIndex"));
    }
    public void ToogleAccelerate(bool flag)
    {
        Accelerate = flag;


    }

  
    public void ToogleBrake(bool flag)
    {
        BrakeLights.SetActive(flag);
        Brake = flag;
        if (flag)
        {
            //            AudioManager.instance.PlayBrakeSound();
           
        }
        // BrakeLights.SetActive(flag);
        break_bool = flag;
        
        

    }

    public void Toogleft(bool flag)
    {
        Left = flag;
       
    }

    public void ToogleRight(bool flag)
    {
        Right = flag;
        
    }


    

   

   
    public void ToggleCiren()
    {
        if (aud.enabled)
        {
            aud.volume = 0f;
            aud.enabled = false;
            PlayerPrefs.SetInt("Ciren", 1);
        }
        else
        {
            aud.enabled = true;
            aud.volume = 0.5f;
            PlayerPrefs.GetInt("Ciren", 0);
        }
    }

    public void EngineStart()
    {
     //   AudioManager.instance.PlayEngineStart();

     //  AudioManager.instance.Engine(true);
        StartCoroutine(ShowControls());
    }

    IEnumerator ShowControls()
    {
        yield return new WaitForSeconds(1.5f);
      //  ControlsParent.SetActive(true);
    }
    public void Play_ButtonClickSound()
    {
       //AudioManager.instance.PlayButtonClick();
    }

    

    
    public void RespawnPlayer()
    {

        RevivePanel.SetActive(false);
        RevivedOnce = true;
      
        FailedPanel.SetActive(false);

        PlayerCars.transform.position = RespawnPoint.transform.position;
        PlayerCars.transform.rotation = RespawnPoint.transform.rotation;

        Accelerate = false;
        Brake = false;
        Left = false;
        Right = false;

        FollowCam_1.Instance.offset = Prev_CamOffset;

        SimpleCarController.Instance.FreeTheCar();

    }

    
}
