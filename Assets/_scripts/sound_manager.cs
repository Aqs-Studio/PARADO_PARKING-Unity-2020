using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sound_manager : MonoBehaviour
{

    public static sound_manager instance;
    public AudioSource menuMusic, gamePlayMusic;
    //public AudioClip buttonClick;
    public string currentLevel;
    public bool first_one;
    public int fail;
    public int complete;
    public int pause;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
// public void PlayButtonClick ()
// {
//     //if(menuMusic)
//    menuMusic.PlayOneShot(buttonClick,1);
// }
    
}
