using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectActorConnector : MonoBehaviour
{

    PhotonView _myPhotonView;
    SelectController selectCon;
    bool _done;
    

    int _element;
    public string _playerName;
    private void Awake()
    {
        _done = false;
        _myPhotonView = GetComponent<PhotonView>();
    }

    [PunRPC]
    void RPCInitialize(int elem, string playerName)
    {
        _element = elem;
        _playerName = playerName;
        selectCon = GameObject.Find("SelectController").GetComponent<SelectController>();
        selectCon.SetConnector(elem, this);
    }
    public void Initialize(int elem, string playerName)
    {
        _myPhotonView.RPC("RPCInitialize", PhotonTargets.AllBufferedViaServer, elem, playerName);
    }

    [PunRPC]
    void RPCDone(bool isDone)
    {
        if (selectCon.GetDoneCount() == 4) return;
        _done = isDone;
    }
    public void Done(bool isDone)
    {
        _myPhotonView.RPC("RPCDone", PhotonTargets.AllBufferedViaServer, isDone);
    }
    public bool isDone()
    {
        return _done;
    }


    public void LeaveRoom()
    {
        _myPhotonView.RPC("LeaveRoomRPC",PhotonTargets.AllViaServer);
    }
    [PunRPC]
    void LeaveRoomRPC()
    {
        if (!_myPhotonView.isMine) return;
        PhotonNetwork.LeaveRoom();
        SceneController.Instance.LoadScene("Lobby", 2.0f, true);
    }

    public void ChangePhotonPlayerName(string playerName)
    {
        _myPhotonView.RPC("ChangePhotonPlayerNameRPC", PhotonTargets.AllBufferedViaServer, playerName);
    }
    [PunRPC]
    void ChangePhotonPlayerNameRPC(string playerName)
    {
        if (!_myPhotonView.isMine) return;
        PhotonNetwork.playerName = playerName;
    }

    private void OnDestroy()
    {
        GameObject.Find("SelectController").GetComponent<SelectController>().SetConnector(_element, null);
        if (!PhotonNetwork.isMasterClient) return;
        var roomProperties = new ExitGames.Client.Photon.Hashtable();
        roomProperties.Add(_playerName, 0);
        PhotonNetwork.room.SetCustomProperties(roomProperties);
    }
}
