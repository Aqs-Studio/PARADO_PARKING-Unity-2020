//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class arrow_color_show : MonoBehaviour {
//    public GameObject arrow;
//    public GameObject parado;
//    public Color tmpcolor;
//	// Use this for initialization
//	void Start ()
//    {
//       // tmpcolor = arrow.GetComponent<SpriteRenderer>().color;
//       tmpcolor.a = 1;
//       InvokeRepeating("_Update",0.1f,0.1f);
//	}
//
//    void _Update()
//    {
//
//	
//
//        var distance = Vector3.Distance(arrow.transform.position, parado.transform.position);
//        if (distance <= 40)
//        {
//            arrow.GetComponent<SpriteRenderer>().color = Color.green;
//
//        }
//        else 
//        {
//            arrow.GetComponent<SpriteRenderer>().color = tmpcolor;
//        }
//    }
//
//}
