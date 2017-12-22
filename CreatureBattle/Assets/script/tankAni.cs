using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankAni : MonoBehaviour {
    public Animator tanAni;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.R))
        {
            tanAni.SetTrigger("Run");
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            tanAni.SetTrigger("Dodg");
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            tanAni.SetTrigger("Attack01");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            tanAni.SetTrigger("Attack02");
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            tanAni.SetTrigger("Attack03");
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            tanAni.SetTrigger("React");
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            tanAni.SetTrigger("Cry");
        }
    }
}
