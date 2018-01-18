using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomMenu : MonoBehaviour {
    [SerializeField]
    InputField _roomNameInput;
    [SerializeField]
    Button _decideButton;
    [SerializeField]
    Button _cancelButton;
    [SerializeField]
    Toggle _monsterCheckBox;
	// Use this for initialization
	void Start () {
        // ButtonEventの設定
        _decideButton.onClick.AddListener(CreateRoom);
        _cancelButton.onClick.AddListener(() => { gameObject.SetActive(false); });
	}

    void CreateRoom()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        string roomName = _roomNameInput.text;
        // 同じ名前のルームがあったら生成しない
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].Name == roomName)
            {
                Debug.Log("すでに存在する名前です");
                return;
            }
        }

        RoomOptions option = new RoomOptions();
        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
        option.MaxPlayers = (byte)4;
        option.IsOpen = true;
        option.IsVisible = true;
        table.Add("player1", 0);
        table.Add("player2", 0);
        table.Add("player3", 0);
        table.Add("monster", 0);
        option.CustomRoomProperties = table;
        option.CustomRoomPropertiesForLobby = new string[] { "player1", "player2", "player3", "monster" };
        // シーンを読み込むまでルームのメッセージを受け取らない
        if (PhotonNetwork.CreateRoom(roomName, option, TypedLobby.Default)) 
        {
            var properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add("monster", (_monsterCheckBox.isOn) ? true : false);
            PhotonNetwork.SetPlayerCustomProperties(properties);
            SceneController.Instance.LoadScene("Select", 2.0f, true);
            Debug.Log("部屋を生成");
        }
    }
}
