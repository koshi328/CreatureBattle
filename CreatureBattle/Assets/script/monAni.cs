using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monAni : MonoBehaviour {
    public Animator monsAni;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.R))
        {
            monsAni.SetTrigger("Run");
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            monsAni.SetTrigger("Punch");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            monsAni.SetTrigger("Breath");
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            monsAni.SetTrigger("Cast");
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            monsAni.SetTrigger("React");
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            monsAni.SetTrigger("Scream");
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            monsAni.SetTrigger("Swap");
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            monsAni.SetTrigger("SwapR");
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            monsAni.SetTrigger("PunchR");
        }
    }
}
