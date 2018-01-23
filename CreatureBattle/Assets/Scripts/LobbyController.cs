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
    [SerializeField]
    Button _humanButton;
    [SerializeField]
    Button _monsterButton;

    bool isWantToMonster;
    
    private GameObject _roomElement;
	// Use this for initialization
	void Start () {
        isWantToMonster = false;
        _humanButton.GetComponent<Image>().color = new Color(1, 0, 0, 1);
        _monsterButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        // ポップアップウィンドウを非アクティブにしておく
        _decideWindow.gameObject.SetActive(false);
        _roomMenuWindow.gameObject.SetActive(false);
        _roomElement = Resources.Load("Prefabs/RoomElement") as GameObject;
        _humanButton.onClick.AddListener(()=> 
        {
            isWantToMonster = false;
            OnReceivedRoomListUpdate();
            _humanButton.GetComponent<Image>().color = new Color(1, 0, 0, 1);
            _monsterButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        });
        _monsterButton.onClick.AddListener(() => 
        {
            isWantToMonster = true;
            OnReceivedRoomListUpdate();
            _humanButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            _monsterButton.GetComponent<Image>().color = new Color(1, 0, 0, 1);
        });
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
            object existMonster = null;
            rooms[i].CustomProperties.TryGetValue("monster", out existMonster);
            if (isWantToMonster)
            {
                if ((int)existMonster != 0) continue;
            }
            else
            {
                if ((int)existMonster == 0 && rooms[i].PlayerCount >= rooms[i].MaxPlayers - 1) continue;
            }
            GameObject element = Instantiate(_roomElement, _roomListViewContent.transform);
            string info = "";
            info += "RoomName:" + rooms[i].Name + "\n";
            info += "PlayNum :" + rooms[i].PlayerCount + "/" + rooms[i].MaxPlayers + "\n";
            element.transform.GetComponentInChildren<Text>().text = info;
            element.GetComponent<Button>().onClick.AddListener(() =>
            {
                _decideWindow.roomName = element.name;
                _decideWindow.SetMessage(element.name + "に入室します。\nよろしいですか？");
                _decideWindow.gameObject.SetActive(true);
                _decideWindow.YesButtonAddEvent(() => { JoinRoom(_decideWindow.roomName);_decideWindow.gameObject.SetActive(false); }, true);
            });
            element.name = rooms[i].Name;
        }
    }

    public void JoinRoom(string roomName)
    {
        var properties = new ExitGames.Client.Photon.Hashtable();
        properties.Add("monster", (isWantToMonster) ? true : false);
        PhotonNetwork.SetPlayerCustomProperties(properties);
        PhotonNetwork.JoinRoom(roomName);
    }

    void OnJoinedLobby()
    { 
}
}
