using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    GameObject _player;
    [SerializeField]
    TrackCamera _cameraScript;
    [SerializeField]
    YesNoWindow _decideWindow;
    [SerializeField]
    ScriptableActor actorData;

    void Start () {
        PhotonNetwork.isMessageQueueRunning = true;
        CreatePlayer("Yuko", Vector3.zero, Quaternion.identity);
    }
	
	void Update () {
        if(Input.GetKeyDown("space"))
        {
            _player = PhotonNetwork.Instantiate(actorData.data[0].path, Vector3.zero, Quaternion.identity, 0);
        }
    }

    public void LeaveRoom()
    {
        if (_decideWindow.gameObject.GetActive() == true) return;
        _decideWindow.gameObject.SetActive(true);
        _decideWindow.YesButtonAddEvent(() => {
            PhotonNetwork.LeaveRoom();
            SceneController.Instance.LoadScene("Lobby", 3.0f, true);
            _decideWindow.transform.parent.gameObject.SetActive(false);
        }, true);
        _decideWindow.NoButtonAddEvent(() => { _decideWindow.gameObject.SetActive(false); }, true);
        _decideWindow.SetMessage("ログアウトします。\nよろしいですか？");
    }

    public void CreatePlayer(string path, Vector3 pos, Quaternion rot)
    {
        //Photonにつながっていない時のデバッグ用
        if (!PhotonNetwork.connected)
        {
            _player = Instantiate(Resources.Load(path), pos, rot) as GameObject;
            _player.AddComponent<ActorController>();
            _cameraScript.SetTarget(_player.transform);
        }
        else
        {
            object value;
            PhotonNetwork.player.CustomProperties.TryGetValue("ActorID", out value);
            _player = PhotonNetwork.Instantiate(actorData.data[(int)value].path, pos, rot, 0);
            _player.AddComponent<ActorController>();
            _cameraScript.SetTarget(_player.transform);
        }
    }
    public GameObject GetPlayer()
    {
        return _player;
    }
}
