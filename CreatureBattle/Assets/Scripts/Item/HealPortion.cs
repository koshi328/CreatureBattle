using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPortion : MonoBehaviour {

    // 回復量
    [SerializeField]
    float _recoverPower;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        Actor actor = collision.gameObject.GetComponent<Actor>();
        if (actor == null) return;

        actor.ReceiveRecovery(_recoverPower);
        Destroy(this.gameObject);
    }
}
