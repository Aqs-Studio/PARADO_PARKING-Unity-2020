using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelEnable : MonoBehaviour {

//public GameObject [] LevelsObj;
	// Use this for initialization
	void Start () {
		
		LoadSceneObjects ();
	}

    void LoadSceneObjects ()
    {



        int current_level = PlayerPrefs.GetInt("level");
	


	

        #if UNITY_EDITOR
      //  Debug.Log("currentMode "+PlayerPrefs.GetInt("currentMode")+" , current_level "+current_level);

        #endif
        switch(PlayerPrefs.GetInt("currentMode"))
        {
            case 1:
            GameObject levelCar = Resources.Load(("levelcars"+current_level.ToString())) as GameObject;
            Instantiate(levelCar, levelCar.transform.position, levelCar.transform.rotation);
            break;
            case 2:
            GameObject levelcones = Resources.Load(("levelcones"+current_level.ToString())) as GameObject;
            Instantiate(levelcones, levelcones.transform.position, levelcones.transform.rotation);
            break;
            case 3:
            GameObject levelblock = Resources.Load(("levelblock"+current_level.ToString())) as GameObject;
            Instantiate(levelblock, levelblock.transform.position, levelblock.transform.rotation);
            break;                         
        }
        // if(current_level <10)
        // {
        //     GameObject levelCar = Resources.Load(("levelcars"+current_level.ToString())) as GameObject;
        //     Instantiate(levelCar, levelCar.transform.position, levelCar.transform.rotation);
        // }
        // else
        // if(PlayerPrefs.GetInt("level") <15)
        // {
        //     GameObject levelcones = Resources.Load(("levelcones"+PlayerPrefs.GetInt("level").ToString())) as GameObject;
        //     Instantiate(levelcones, levelcones.transform.position, levelcones.transform.rotation);
        // }
		// else
		// if(PlayerPrefs.GetInt("level") <20)
        // {
        //     GameObject levelblock = Resources.Load(("levelblock"+PlayerPrefs.GetInt("level").ToString())) as GameObject;
        //     Instantiate(levelblock, levelblock.transform.position, levelblock.transform.rotation);
        // }

    }

}
