using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class park_back : MonoBehaviour {

    void OnTriggerStay(Collider front)
    {
        // if (front.gameObject.tag == "back")
        // {
        //     PlayerPrefs.SetInt("back", 1);
        // }
        // if (front.gameObject.tag == "front")
        // {
        //     PlayerPrefs.SetInt("wrong_front", 1);
        // }
        if (front.gameObject.name.Equals("front") || front.gameObject.name.Equals("back"))
        {
            PlayerPrefs.SetInt("back", 1);
        }

        PlayerPrefs.Save();
        game_manager.instance.ChangeRingColors();
    }
    void OnTriggerExit(Collider front)
    {
        // PlayerPrefs.SetInt("back", 0);
        // PlayerPrefs.SetInt("wrong_front", 0);
        PlayerPrefs.SetInt("back", 0);
        PlayerPrefs.Save();
        game_manager.instance.ChangeRingColors();
    }

}
