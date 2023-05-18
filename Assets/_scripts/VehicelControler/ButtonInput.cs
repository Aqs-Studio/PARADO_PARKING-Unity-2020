using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
	public static bool chk;
	public  bool pressing;
	private float gravity = 1;
	private float sensitivity = 1;
	[HideInInspector]
	public float input;
	public static ButtonInput instance;
	void start()
	{
		instance = this;
		chk = false;
	}
	public void OnPointerDown(PointerEventData eventData){

		pressing = true;
		chk = true;
        //	game_manager.instance.gas.GetComponent<Animation>().Stop ("gasBtnAnim");
        //	game_manager.instance.gas.GetComponent<Shadow> ().enabled = false;
        //	game_manager.instance.ImgHand.SetActive (false);
     
	
        
	}

	public void OnPointerUp(PointerEventData eventData){
		 
		pressing = false;
		chk = false;
	
	}
	void OnPress (bool isPressed){

        if (isPressed)
		{	
			pressing = true;
			//chk = true;	
		}
           
        else
            pressing = false;
	}
	void Update () {
		if(pressing)
			input += Time.deltaTime * sensitivity;
		else
			input -= Time.deltaTime * gravity;
            
		if(input < 0f)
			input = 0f;
		
		if(input > 1f)
			input = 1f;
	}





}
