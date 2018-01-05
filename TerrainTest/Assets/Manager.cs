using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    [SerializeField]
    private GameObject _terrainPrefab;

	// Use this for initialization
	void Start () {
        Instantiate<GameObject>(_terrainPrefab, this.transform);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
