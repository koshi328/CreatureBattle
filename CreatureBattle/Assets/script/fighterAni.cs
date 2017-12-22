using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fighterAni : MonoBehaviour {

    //[SerializeField]
    public Animator fightAni;
    public GameObject go;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //go.transform.localScale = new Vector3(100f, 100f, 100f);

		if(Input.GetKey(KeyCode.R))
        {
            fightAni.SetTrigger("Run");
        }
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            fightAni.SetTrigger("Slash1");
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            fightAni.SetTrigger("Slash2");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            fightAni.SetTrigger("Slash3");
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            fightAni.SetTrigger("React");
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            fightAni.SetTrigger("Cast");
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            fightAni.SetTrigger("Stan");
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            fightAni.SetTrigger("Power");
        }
    }
}
