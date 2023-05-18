using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelButton : MonoBehaviour {
	public static LevelButton instance;
	public enum  LevelMode
	{
		Cars,
		Cones,
		Blocks
	}
	public LevelMode modeType;
	private Text levelNumberText; 
	//public Image _lock; 
	public CanvasGroup _lockObj;
	private int currentLevel = 0;
	//private int currentFunnelNumber = 0;
	void Start()
	{
		instance = this;	
	}
	void OnEnable ()
	{
		levelNumberText = GetComponentInChildren<Text>();
		if(!PlayerPrefs.HasKey("currentMode"))
		{
			PlayerPrefs.SetInt("currentMode",1);
		}

		//_lockObj = GetComponentInChildren<CanvasGroup>();
	}

	public void SetLevel (int number) {


		currentLevel = number;
		levelNumberText.text = (number+1).ToString();
	


		if(modeType == LevelMode.Cars)
		{
			if(PlayerPrefs.GetInt("M1 "+number.ToString()).Equals(1))
			{
				_lockObj.alpha = 0;
			}
			else
			{
				_lockObj.alpha = 1;
			}
		}
		else
		if(modeType == LevelMode.Cones)
		{
			if(PlayerPrefs.GetInt("M2 "+number.ToString()).Equals(1))
			{
				_lockObj.alpha = 0;
			}
			else
			{
				_lockObj.alpha = 1;
			}
		}
		else
		if(modeType == LevelMode.Blocks)
		{
			if(PlayerPrefs.GetInt("M3 "+number.ToString()).Equals(1))
			{
				_lockObj.alpha = 0;
			}
			else
			{
				_lockObj.alpha = 1;
			}
		}					
	}
	public void LoadLevel () {

		if(modeType == LevelMode.Cars)
		{
			if(PlayerPrefs.GetInt("M1 "+currentLevel.ToString()).Equals(1))
			{
				 if(currentLevel >= level_selection_manager.instance.availableCarLevels)
				{
					//level_selection_manager.instance.EnableComingSoonPanel();
				}
				 else
				 {
					PlayerPrefs.SetInt("level", currentLevel); 
					PlayerPrefs.SetInt("currentMode",1);
					PlayerPrefs.SetInt("funnelLevelNumber",currentLevel+1);   // available level are 30
					sound_manager.instance.currentLevel = "M1 "+currentLevel.ToString();
        			//level_selection_manager.instance.ShowLoading();
					main_menu.instance.EnableStore();
				 }
			}
		}
		else
		if(modeType == LevelMode.Cones)
		{
			if(PlayerPrefs.GetInt("M2 "+currentLevel.ToString()).Equals(1))
			{
				 if(currentLevel >= level_selection_manager.instance.availableConeLevels)
				{
					//level_selection_manager.instance.EnableComingSoonPanel();
				}
				 else
				 {
					PlayerPrefs.SetInt("level", currentLevel); 
					PlayerPrefs.SetInt("currentMode",2);
					PlayerPrefs.SetInt("funnelLevelNumber",level_selection_manager.instance.availableCarLevels+currentLevel+1);   // 30 +cone levels
					sound_manager.instance.currentLevel = "M2 "+currentLevel.ToString();
					main_menu.instance.EnableStore();
        			//level_selection_manager.instance.ShowLoading();
				 }
			}
		}
		else
		if(modeType == LevelMode.Blocks)
		{
			if(PlayerPrefs.GetInt("M3 "+currentLevel.ToString()).Equals(1))
			{

				 if(currentLevel >= level_selection_manager.instance.availableBlockLevels)
				{
					//level_selection_manager.instance.EnableComingSoonPanel();
				}
				 else
				 {
					PlayerPrefs.SetInt("level", currentLevel);
					PlayerPrefs.SetInt("currentMode",3);
					PlayerPrefs.SetInt("funnelLevelNumber",level_selection_manager.instance.availableCarLevels+level_selection_manager.instance.availableConeLevels+currentLevel+1);   // 30 +cone levels+block levels 
					sound_manager.instance.currentLevel = "M3 "+currentLevel.ToString();
					main_menu.instance.EnableStore();
        			//level_selection_manager.instance.ShowLoading();
				 }
			}
		}
		
	}
}
