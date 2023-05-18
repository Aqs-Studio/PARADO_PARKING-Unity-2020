using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class open_gate : MonoBehaviour {
    public Animator anim;

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag=="Player")
        {

            anim.Play("MOVE_UP");
        }
    }
}
