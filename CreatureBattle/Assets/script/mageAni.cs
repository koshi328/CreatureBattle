using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mageAni : MonoBehaviour {
    public Animator magAni;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.R))
        {
            magAni.SetTrigger("Walk");
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            magAni.SetTrigger("Cast01");
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            magAni.SetTrigger("Cast02");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            magAni.SetTrigger("Cast03");
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            magAni.SetTrigger("Cast04");
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            magAni.SetTrigger("React");
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            magAni.SetTrigger("Trap");
        }
    }
}
