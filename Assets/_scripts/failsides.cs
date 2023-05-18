using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class failsides : MonoBehaviour {
    public GameObject fail_panal;
    public GameObject rcc_canvas;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="sides")
        {
            fail_panal.GetComponent<Canvas>().enabled = true;
            rcc_canvas.GetComponent<Canvas>().enabled = false;
            rcc_canvas.GetComponent<GraphicRaycaster>().enabled = true;
            fail_panal.GetComponent<GraphicRaycaster>().enabled = false;
            game_manager.instance.EngineSoundOff();
            game_manager.instance.SetMusic(0.6f);
            Time.timeScale = 0;

        }
    }
}
