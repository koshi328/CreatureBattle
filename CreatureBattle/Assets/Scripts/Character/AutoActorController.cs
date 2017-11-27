using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoActorController : MonoBehaviour {
    [SerializeField]
    float _speed;
    [SerializeField]
    Actor _target;
    Actor _myActor;
    // Use this for initialization
    void Start () {
        _myActor = GetComponent<Actor>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!_target) return;
        Vector3 dir = _target.transform.position - transform.position;
        _myActor.Movement(dir.x, dir.z, _speed);
    }
}
