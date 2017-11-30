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
        option.MaxPlayers = (byte)maxPlayer;
        option.IsOpen = true;
        option.IsVisible = true;
        // シーンを読み込むまでルームのメッセージを受け取らない
        if (PhotonNetwork.CreateRoom(roomName, option, TypedLobby.Default)) 
        {
            SceneController.Instance.LoadScene("Select", 2.0f, true);
            Debug.Log("部屋を生成");
        }
    }
}
