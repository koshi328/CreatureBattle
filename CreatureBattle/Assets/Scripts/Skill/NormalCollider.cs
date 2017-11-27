using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCollider : MonoBehaviour {

    Actor _owner;
	// Use this for initialization
	void Awake () {

	}

    public void Initialize(Actor owner,Vector3 pos, Vector3 scale)
    {
        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        transform.position = pos;
        transform.localScale = scale;
        _owner = owner;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OntriggerEnter");
        if (_owner.transform == other.transform) return;
        Actor hitActor = other.GetComponent<Actor>();
        if (!hitActor) return;
        hitActor.CallTakeDamage(10);
        // なぜか何度もヒットするので強制終了（直せたら直す）
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log("当たり判定を破棄");
    }
}
