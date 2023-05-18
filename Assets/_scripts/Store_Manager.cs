using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Store_Manager : MonoBehaviour {
	public static Store_Manager instance;
	public GameObject [] vehicle;
	public Transform [] transformPoints;
	public int [] vPrice;
	public GameObject playButton,buyButton,previousButton, nextButton;
	private int currentVehicle = 0;
	private int totalVehicles = 0;
	public Text total_Coins, vehiclePrice;
	public GameObject PriceImage;
	public GameObject notEnoughCash;
    public GameObject buy_button, play_btn;
    public Text text_1, text_2;
    void Start () {
		instance = this;
		Time.timeScale = 1;
		totalVehicles = vehicle.Length;
		if(!PlayerPrefs.HasKey("currentVehicle"))
		PlayerPrefs.SetInt("currentVehicle",0);

		if(!PlayerPrefs.HasKey("purchasedVehicle0"))
		   PlayerPrefs.SetInt("purchasedVehicle0",1);		


		currentVehicle = PlayerPrefs.GetInt("currentVehicle");
		Invoke("SwitchVehicle",0.1f);
		Invoke("ShowTotalCoins",0.1f);
        PlayerPrefs.SetInt("purchased_all_parado", 0);
	}
void SwitchVehicle ()
{
        
        for (int i = 0; i<totalVehicles;i++)
	{
		if(i.Equals(currentVehicle))
		{
			if(i<1)
			{
				previousButton.SetActive(false);
				nextButton.SetActive(true);
			}
			else
			if(i>=totalVehicles-1)
			{
				previousButton.SetActive(true);
				nextButton.SetActive(false);
			}
			else
			{
				previousButton.SetActive(true);
				nextButton.SetActive(true);				
			}
                if (PlayerPrefs.GetInt("purchased_all_parado") == 0)
                {

                    if (PlayerPrefs.GetInt("purchasedVehicle" + i.ToString()).Equals(1))
                    {
                        ShowPriceCoins(false);
                        playButton.SetActive(true);
                        buyButton.SetActive(false);
                    }
                    else
                    {
                        ShowPriceCoins(true);
                        playButton.SetActive(false);
                        buyButton.SetActive(true);
                    }

                }
                else
                {
                    playButton.SetActive(true);
                    buyButton.SetActive(false);
                }
			vehicle[i].SetActive(true);
			SmoothFollow1.instance.target = transformPoints[i];
		}
		else
		vehicle[i].SetActive(false);
	}
}
public void ShowTotalCoins ()
{
	if(total_Coins)
	total_Coins.text = PlayerPrefs.GetInt("cash")+System.String.Empty;
}

    void Update()
    {
        if(currentVehicle == 0)
        {
            text_1.text = "Cadillac Escalade 2020";
            text_2.text = "Full size Luxury SUV ";

        }
        if (currentVehicle == 1)
        {
            text_1.text = "RANGE ROVER AUTOBIOGRAPHY 2020";
            text_2.text = "Luxury SUV ";

        }
        if (currentVehicle == 2)
        {
            text_1.text = "RANGE Evoque 2020";
            text_2.text = "Black ";

        }
    }
void ShowPriceCoins (bool status)
{
	if(vehiclePrice)
	{
		if(status)
		{
		vehiclePrice.text = "Price :"+vPrice[currentVehicle];
		PriceImage.SetActive(true);

		}

		else
		{
			PriceImage.SetActive(false);
			vehiclePrice.text = System.String.Empty;
		}
		
	}
}
public void NextVehicle ()
{
        
        currentVehicle +=1;
	SwitchVehicle ();
}	
public void PreviousVehicle ()
{
        vehicle[currentVehicle].SetActive(false);
        currentVehicle -=1;
	    SwitchVehicle ();
        
    }
public void PlayLevel ()
{
	PlayerPrefs.SetInt("currentVehicle",currentVehicle);
	PlayerPrefs.Save();
	level_selection_manager.instance.ShowLoading();

		int Vehicalnum = currentVehicle;
		Vehicalnum += 1;
		if(Vehicalnum==1)
		{
			GameAnalytics.instance.SelectedItemEvent ("Vehicle1");
		}
		if(Vehicalnum==2)
		{
			GameAnalytics.instance.SelectedItemEvent ("Vehicle2");
		}

		if(Vehicalnum==3)
		{
			GameAnalytics.instance.SelectedItemEvent ("Vehicle3");
		}

		if(Vehicalnum==4)
		{
			GameAnalytics.instance.SelectedItemEvent ("Vehicle4");
		}

		if(Vehicalnum==5)
		{
			GameAnalytics.instance.SelectedItemEvent ("Vehicle5");
		}





}
public void BuyVehicle ()
	{	
		if(PlayerPrefs.GetInt("cash") < vPrice[currentVehicle])
		{ 
			notEnoughCash.SetActive (true);
			Invoke ("setFalseNoCashimg",2f);

		}



	else if(PlayerPrefs.GetInt("cash") >= vPrice[currentVehicle])
	{

		
		PlayerPrefs.SetInt("purchasedVehicle"+currentVehicle.ToString(),1);
		PlayerPrefs.SetInt("cash", PlayerPrefs.GetInt("cash")-vPrice[currentVehicle]);
		PlayerPrefs.Save();
		SwitchVehicle ();
		ShowTotalCoins();
		level_selection_manager.instance.ShowCash();
		
	}
}
public void StoreToLevel ()
{
	main_menu.instance.Store_To_Level();
}
	public void setFalseNoCashimg()
	{
		notEnoughCash.SetActive (false);
	}
}
