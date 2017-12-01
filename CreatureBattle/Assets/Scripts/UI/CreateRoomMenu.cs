using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomMenu : MonoBehaviour {
    [SerializeField]
    InputField _roomNameInput;
    [SerializeField]
    Slider _playerNumSlider;
    [SerializeField]
    Text _playerNum;
    [SerializeField]
    Button _decideButton;
    [SerializeField]
    Button _cancelButton;
    [SerializeField]
    Toggle _monsterCheckBox;
	// Use this for initialization
	void Start () {
        // sliderを変えると表示されている人数も変更する
        _playerNumSlider.onValueChanged.AddListener(value => { _playerNum.text = value.ToString() + "人"; });

        // ButtonEventの設定
        _decideButton.onClick.AddListener(CreateRoom);
        _cancelButton.onClick.AddListener(() => { gameObject.SetActive(false); });
	}

    void CreateRoom()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        string roomName = _roomNameInput.text;
        int maxPlayer = (int)_playerNumSlider.value;
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
        option.MaxPlayers = (byte)maxPlayer;
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
