using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Screen.SetResolution(1920, 1080, true);
        SceneController.Instance.LoadScene("Title", 2.0f, true);
	}
	
}
