using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class park : MonoBehaviour {

	// Use this for initialization
    void OnTriggerStay(Collider other)
    {
        // if (other.gameObject.tag == "front")
        // {
        //     PlayerPrefs.SetInt("front", 1);
        // }
        // if (other.gameObject.tag == "back")
        // {
        //     PlayerPrefs.SetInt("wrong_back", 1);
        // }
		if (other.gameObject.name.Equals("front") || other.gameObject.name.Equals("back"))
        {

            PlayerPrefs.SetInt("front", 1);
        }

        PlayerPrefs.Save();
        game_manager.instance.ChangeRingColors();

    }
    void OnTriggerExit(Collider front)
    {
        // PlayerPrefs.SetInt("front", 0);
        // PlayerPrefs.SetInt("wrong_back", 0);
        PlayerPrefs.SetInt("front", 0);

        PlayerPrefs.Save();
        game_manager.instance.ChangeRingColors();
    }
}
