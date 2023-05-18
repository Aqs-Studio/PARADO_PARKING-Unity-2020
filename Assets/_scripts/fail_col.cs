using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class fail_col : MonoBehaviour {
    public GameObject rcc_canvas;
    public GameObject fail_panal;
    public int fail_counter;
    public Text failCount;
    public bool once= false;
	public GameObject imgCounter;

    void OnCollisionEnter(Collision other)
    {



			if (other.gameObject.tag == "obsticle") {
				
				fail_counter++;
				failCount.text = fail_counter + System.String.Empty;

				if (fail_counter >= 3) {
					if (PlayerPrefs.GetInt ("from_setting_music") == 1) {
						if (sound_manager.instance)
							sound_manager.instance.menuMusic.volume = 0.6f;
					} else {
						AudioListener.volume = 0;
					}
					fail_counter = 0;
					if (!once) {
						once = true;
					//	Ads_Manager.Instance.HideSmallAdmobBanner ();
					//	Ads_Manager.Instance.ShowLargeAdmobBanner ();
						PlayerPrefs.SetInt ("AdCount", PlayerPrefs.GetInt ("AdCount") + 1);
						if (PlayerPrefs.GetInt ("AdCount") == 2) {
							//Ads_Manager.Instance.Show_Unity_Admob ();
							PlayerPrefs.SetInt ("AdCount", 0);
				
					
						}
					}
					game_manager.instance.LevelFailed ();
					GameAnalytics.instance.LevelFailedEvent (PlayerPrefs.GetInt ("funnelLevelNumber"));
					if (sound_manager.instance)
						sound_manager.instance.fail++;
					Time.timeScale = 0;
					if (sound_manager.instance.fail == 2) {
						sound_manager.instance.fail = 0;
                    
					}
			
				}
		

				imgCounter.GetComponent<Animation> ().Play ("imgCounterAnim");
		

	

			 
		}
	

    }
	

}
