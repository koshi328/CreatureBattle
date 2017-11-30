using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour {

    [SerializeField]
    YesNoWindow _decideWindow;
    [SerializeField]
    CreateRoomMenu _roomMenuWindow;
    [SerializeField]
    GameObject _roomListViewContent;
    

    private GameObject _roomElement;
    
	// Use this for initialization
	void Start () {
        // ポップアップウィンドウを非アクティブにしておく
        _decideWindow.gameObject.SetActive(false);
        _roomMenuWindow.gameObject.SetActive(false);
        _roomElement = Resources.Load("Prefabs/RoomElement") as GameObject;
    }

    void OnReceivedRoomListUpdate()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        for (int i = 0; i < _roomListViewContent.transform.childCount; i++)
        {
            Destroy(_roomListViewContent.transform.GetChild(i).gameObject);
        }
        if (rooms.Length <= 0) return;

        for (int i = 0; i < rooms.Length; i++)
        {
            GameObject element = Instantiate(_roomElement, _roomListViewContent.transform);
            string info = "";
            info += "RoomName:" + rooms[i].Name + "\n";
            info += "PlayNum :" + rooms[i].PlayerCount + "/" + rooms[i].MaxPlayers + "\n";
            element.transform.GetComponentInChildren<Text>().text = info;
            element.GetComponent<Button>().onClick.AddListener(() =>
            {
                _decideWindow.SetMessage(element.name + "に入室します。\nよろしいですか？");
                _decideWindow.gameObject.SetActive(true);
                _decideWindow.YesButtonAddEvent(() => { JoinRoom(element.name);_decideWindow.gameObject.SetActive(false); }, true);
            });
            element.name = rooms[i].Name;
        }
    }

    public void JoinRoom(string roomName)
    {
        Debug.Log("Joinしたいです");
        PhotonNetwork.JoinRoom(roomName);        
    }
}
