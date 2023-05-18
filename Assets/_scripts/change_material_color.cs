//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class change_material_color : MonoBehaviour {
//    public bool towhite; // true
//    public bool tored; // false
//
//    public float speedToLerp;//17
//
//    public bool isCoroutineRunning; //  true
//
//    public int counter; //0
//
//	
//	// Update is called once per frame
//	void Update ()
//    {
//        if (PlayerPrefs.GetInt("x") == 1)
//        {
//            if (tored)
//            {
//                this.gameObject.GetComponent<Renderer>().material.color = Color.Lerp(this.gameObject.GetComponent<Renderer>().material.color, Color.red, Time.deltaTime * speedToLerp);
//            }
//            if (towhite)
//            {
//                this.gameObject.GetComponent<Renderer>().material.color = Color.Lerp(this.gameObject.GetComponent<Renderer>().material.color, Color.white, Time.deltaTime * speedToLerp);
//            }
//            if (!isCoroutineRunning && counter < 6)
//            {
//                counter++;
//                isCoroutineRunning = true;
//                StartCoroutine(color_change());
//            }
//        }	
//	}
//    void OnCollisionEnter(Collision other)
//    {
//        if(other.gameObject.tag=="Player")
//        {
//            isCoroutineRunning = false;
//           // Debug.Log("gg");
//        }
//    }
//
//    IEnumerator color_change()
//    {
//        yield return new WaitForSeconds(1f);
//        isCoroutineRunning = false;
//        tored = !tored;
//        towhite = !towhite;
//    }
//}
