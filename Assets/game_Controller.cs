using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class game_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] prado;
    public GameObject[] prado_pos;
    int counter;
    
    void Start()
    {
        
        Instantiate(prado[0], prado_pos[0].transform.position, prado_pos[0].transform.rotation);
        GameObject.Find("pvoit").GetComponent<garageOrbit>().enabled = true;
       

    }

    // Update is called once per frame
    

  public void inst()
    {

       
      
       
        
    }
}
