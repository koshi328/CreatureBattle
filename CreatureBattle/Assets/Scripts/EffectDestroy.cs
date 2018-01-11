using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroy : MonoBehaviour {
    [SerializeField]
    ParticleSystem _myParticleSystem;

    private void Start()
    {
        Destroy(this.gameObject,_myParticleSystem.duration);
    }
}
