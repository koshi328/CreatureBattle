using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour {

    enum ROOMLIST_TAG{
        HUMAN,
        MONSTER,
    }

    [SerializeField]
    YesNoWindow _decideWindow;
    [SerializeField]
    CreateRoomMenu _roomMenuWindow;
    [SerializeField]
    GameObject[] _roomListViewContent;
    [SerializeField]
    Button _createRoomButton;

    bool isWantToMonster;
    
    private GameObject _roomElement;
	// Use this for initialization
	void Start () {
        isWantToMonster = false;
        // ポップアップウィンドウを非アクティブにしておく
        _decideWindow.gameObject.SetActive(false);
        _roomMenuWindow.gameObject.SetActive(false);
        _roomElement = Resources.Load("Prefabs/RoomElement") as GameObject;
        _createRoomButton.Select();
    }

    void OnReceivedRoomListUpdate()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        for (int i = 0; i < _roomListViewContent.Length; i++)
        {
            for (int j = 0; j < _roomListViewContent[i].transform.childCount; j++)
            {
                Destroy(_roomListViewContent[i].transform.GetChild(j).gameObject);
            }
        }
        if (rooms.Length <= 0) return;

        for (int i = 0; i < _roomListViewContent.Length; i++)
        {
            for (int j = 0; j < rooms.Length; j++)
            {
                object existMonster = null;
                rooms[j].CustomProperties.TryGetValue("monster", out existMonster);
                if (i == (int)ROOMLIST_TAG.MONSTER && (int)existMonster != 0) continue;
                if(rooms[j].MaxPlayers <= rooms[j].PlayerCount)
                {
                    continue;
                }

                string str = "";
                if (i == (int)ROOMLIST_TAG.MONSTER)
                {
                    str = "このルームに\nモンスターで入室します。\nよろしいですか？";
                    if ((int)existMonster != 0) continue;
                }
                else
                {
                    str = "このルームに\nヒューマンで入室します。\nよろしいですか？";
                    if ((int)existMonster == 0 && rooms[j].PlayerCount >= rooms[j].MaxPlayers - 1) continue;
                }
                GameObject element = Instantiate(_roomElement, _roomListViewContent[i].transform);
                string info = "";
                info += "RoomName:" + rooms[j].Name + "\n";
                info += "PlayNum :" + rooms[j].PlayerCount + "/" + rooms[j].MaxPlayers + "\n";
                element.transform.GetComponentInChildren<Text>().text = info;
                if (i == (int)ROOMLIST_TAG.MONSTER)
                {
                    element.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        isWantToMonster = true;
                    });
                }
                else
                {
                    element.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        isWantToMonster = false;
                    });
                }
                element.GetComponent<Button>().onClick.AddListener(() =>
                {
                    _decideWindow.roomName = element.name;
                    _decideWindow.SetMessage(str);
                    _decideWindow.gameObject.SetActive(true);
                    _decideWindow.YesButtonAddEvent(() => { JoinRoom(_decideWindow.roomName); _decideWindow.gameObject.SetActive(false); }, true);
                });
                element.name = rooms[j].Name;
            }
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

    public void SetActiveCreateRoomWindow(bool flag)
    {
        _roomMenuWindow.gameObject.SetActive(flag);

        if(flag)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            _createRoomButton.Select();
        }
    }
}
