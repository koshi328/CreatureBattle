using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor {

	// Use this for initialization
	void Start () {
        base.Initialize();
    }

    private void Update()
    {
        base.ActionTest();
    }
}
