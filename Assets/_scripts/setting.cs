using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class setting : MonoBehaviour {
    [Header ("sound elements")]
    public Sprite music_on;
    public Sprite music_off;
    public Sprite sound_on;
    public Sprite sound_off;
    public Button sound;
    public Button music;
    [Header("controls elements")]
    public Sprite arrow_on;
    public Sprite arrow_off;
    public Sprite steering_on;
    public Sprite steering_off;
    public Sprite accel_on;
    public Sprite accel_off;
    public Button accel;
    public Button steering;
    public Button arrow;
	public Sprite high_on;
	public Sprite high_off;
	public Sprite medium_on;
	public Sprite medium_off;
	public Sprite low_on;
	public Sprite low_off;
	public Button high;
	public Button medium;
	public Button low;
    public bool isGamePlay = false;
    public GameObject adImage;
   // [Header("gear")]
    //public Scrollbar gear;

    void Awake ()
    {
        //PlayerPrefs.DeleteKey("from_setting");
        if(!PlayerPrefs.HasKey("from_setting_music"))
        {
            PlayerPrefs.SetInt("from_setting_music",1);  //music
        }
        if(!PlayerPrefs.HasKey("from_setting"))
        {
            PlayerPrefs.SetInt("from_setting",1);  //sound
        }
    }
    void Start()
    { 
        if(!PlayerPrefs.HasKey("controlNumber"))
        {
            PlayerPrefs.SetInt("controlNumber",1);
            PlayerPrefs.SetInt("ControlIndex", 1);
        }
        Invoke("controller_change",0.2f);

        _Update();
    }
    
    public void EnableAdImage ()
    {
        adImage.SetActive(true);
    }
    void _Update()
    {
        //high_on_off();

        if (PlayerPrefs.GetInt("from_setting_music").Equals(1))
        {
            if(music)
            music.image.sprite = music_on;
        }
        else
        {
            if(music)
            music.image.sprite = music_off;
        }

        if (PlayerPrefs.GetInt("from_setting") == 1)
        {
            if(sound)
            sound.image.sprite = sound_on;
        }
        else
        {
            if(sound)
            sound.image.sprite = sound_off;
        }
    }
    //---------------------------------------------------------------------------------------------sound stuff in here --------------------------------------------------------------------------------------------
    public void SoundVolon()
    {
        if (PlayerPrefs.GetInt("from_setting")==0)
        {
            if(sound)
            sound.image.sprite = sound_on;
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                StartCoroutine(turn_on_time());
            }
            PlayerPrefs.SetInt("from_setting", 1);
            PlayerPrefs.Save();
            game_manager.instance.EngineSound();
        }
        else
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                StartCoroutine(turn_on_time());
            }
            PlayerPrefs.SetInt("from_setting", 0);
            PlayerPrefs.Save();

            if(sound)
            sound.image.sprite = sound_off;
            game_manager.instance.EngineSound();
        }
    }
    public void MusicVolon()
    {
        if (sound_manager.instance.menuMusic.volume == 0.0f )
        {
            PlayerPrefs.SetInt("from_setting_music", 1);
            sound_manager.instance.menuMusic.volume = 0.5f;
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                StartCoroutine(turn_on_time());
            }
            if(music)
            music.image.sprite = music_on;
        }
        else
        {
            PlayerPrefs.SetInt("from_setting_music", 0);
            sound_manager.instance.menuMusic.volume = 0.0f;
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                StartCoroutine(turn_on_time());
            }
            if(music)
            music.image.sprite = music_off;

        }
        _Update();
    }
    public void controller_change()
    {
        switch(PlayerPrefs.GetInt("controlNumber"))
        {
            case 0:
                PlayerPrefs.SetInt("controlNumber",0);
                if(isGamePlay)
                game_manager.instance.ControlSelection(0);  
                accel.image.sprite = accel_on;
                steering.image.sprite = steering_off;
                arrow.image.sprite = arrow_off;

            break;
            case 1:
                PlayerPrefs.SetInt("ControlIndex", 1);
                PlayerPrefs.SetInt("controlNumber",1);
                if(isGamePlay)
                game_manager.instance.ControlSelection(1);
             
                accel.image.sprite = accel_off;
                steering.image.sprite = steering_off;
                arrow.image.sprite = arrow_on;
            break;
            case 2:
                PlayerPrefs.SetInt("controlNumber",2);
                if(isGamePlay)
                game_manager.instance.ControlSelection(2);

                accel.image.sprite = accel_off;
                steering.image.sprite = steering_on;
                arrow.image.sprite = arrow_off;
            break;
            default :
                PlayerPrefs.SetInt("controlNumber",2);

                if(isGamePlay)
                game_manager.instance.ControlSelection(2);
                accel.image.sprite = accel_off;
                steering.image.sprite = steering_on;
                arrow.image.sprite = arrow_off;
            break;
        }
    }
    void Save_Pref ()
    {
        PlayerPrefs.Save();
    }
    public void accel_control()
    {
        PlayerPrefs.SetInt("controlNumber",0);
        Save_Pref ();
        controller_change();
        parado_manger.GM.Control_Index = 2;
        parado_manger.GM.SwitchControl();

    }
    public void steering_control()
    {
        PlayerPrefs.SetInt("controlNumber",2);
        PlayerPrefs.SetInt("ControlIndex", 1);
        Save_Pref ();
        controller_change();
        parado_manger.GM.Control_Index = 1;
        parado_manger.GM.SwitchControl();
    }
    public void arrow_control()
    {
        PlayerPrefs.SetInt("controlNumber",1);
        PlayerPrefs.SetInt("ControlIndex", 0);

        Save_Pref ();
        controller_change();
        parado_manger.GM.Control_Index = 0;
        parado_manger.GM.SwitchControl();

    }
    IEnumerator turn_on_time()
    {
        yield return new WaitForSeconds(0.0005f);
        //Debug.Log("on and off");
        Time.timeScale = 0;
    }
	public void high_setting()
	{
		//QualitySettings.SetQualityLevel(3);
		high.image.sprite = high_on;
		medium.image.sprite = medium_off;
		low.image.sprite = low_off;
	}
	public void medium_setting()
	{
		high.image.sprite = high_off;
		medium.image.sprite = medium_on;
		low.image.sprite = low_off;
	}
	public void low_setting()
	{
		high.image.sprite = high_off;
		medium.image.sprite = medium_off;
		low.image.sprite = low_on;
	}
}


