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
    [SerializeField]
    Transform PlayerStartPos;
    [SerializeField]
    Transform MonsterStartPos;

    void Start () {
        PhotonNetwork.isMessageQueueRunning = true;
        if (PhotonNetwork.playerName == "monster")
            actorData = Resources.Load("MonsterData") as ScriptableActor;
        else
            actorData = Resources.Load("ActorData") as ScriptableActor;
        CreatePlayer("Yuko", Vector3.zero, Quaternion.identity);

        PhotonNetwork.InstantiateSceneObject("EffectManager", Vector3.zero, Quaternion.identity, 0, null);
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
            _player.transform.position = (PhotonNetwork.playerName == "monster") ? MonsterStartPos.position : PlayerStartPos.position;
            _cameraScript.SetTarget(_player.transform);

        }
    }
    public GameObject GetPlayer()
    {
        return _player;
    }

    private void OnDestroy()
    {
        ColliderManager.Instance.AllColliderDispose();
    }
}
