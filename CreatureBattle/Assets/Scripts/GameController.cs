using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    GameObject _player;
    [SerializeField]
    TrackCamera _cameraScript;

    void Start () {
        SkillController.Initialize();
        PhotonNetwork.isMessageQueueRunning = true;
        //Photonにつながっていない時のデバッグ用
        if (!PhotonNetwork.connected)
        {
            _player = Instantiate(Resources.Load("TestModel"), Vector3.zero, Quaternion.identity) as GameObject;
            _player.AddComponent<ActorController>();
            _cameraScript.SetTarget(_player.transform);
        }
        else
        {
            _player = PhotonNetwork.Instantiate("TestModel", Vector3.zero, Quaternion.identity, 0);
            _player.AddComponent<ActorController>();
            _cameraScript.SetTarget(_player.transform);
        }
    }
	
	void Update () {
        
    }
}
