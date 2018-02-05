using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using antilunchbox;

public class CreateRoomMenu : MonoBehaviour {
    [SerializeField]
    Button _decideButton;
    [SerializeField]
    Button _cancelButton;
    [SerializeField]
    Toggle _monsterCheckBox;
    [SerializeField]
    Button _createRoomButton;

    string _message;
    float _fade;
	// Use this for initialization
	void Start () {
        // ButtonEventの設定
        _decideButton.onClick.AddListener(CreateRoom);
        _cancelButton.onClick.AddListener(() => { gameObject.SetActive(false); });
	}

    void Update()
    {
        if (Input.GetButtonDown("Skill1"))
        {
            SoundManager.PlaySFX("se_051");
            _monsterCheckBox.isOn = !_monsterCheckBox.isOn;
        }

        if(Input.GetButtonDown("Submit"))
        {
            SoundManager.PlaySFX("se_050");
            CreateRoom();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            SoundManager.PlaySFX("se_051");
            gameObject.SetActive(false);
            _createRoomButton.Select();
        }
    }

    void CreateRoom()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        string roomName = (rooms.Length + 1).ToString();
        roomName = "afrstyggd";
        if (roomName == "")
        {
            _message = "部屋の名前を決めてください";
            _fade = 3.0f;
            return;
        }
        // 同じ名前のルームがあったら生成しない
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].Name == roomName)
            {
                _message = "すでに存在する名前です";
                _fade = 3.0f;
                return;
            }
        }

        RoomOptions option = new RoomOptions();
        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
        option.MaxPlayers = (byte)4;
        option.IsOpen = true;
        option.IsVisible = false;
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

    private void OnGUI()
    {
        if (_fade <= 0.0f) return;
        _fade -= Time.deltaTime;
        float x = Screen.width / 2.5f;
        float y = Screen.height / 3 * 2;
        float w = 1000;
        float h = 60;
        GUIStyle style = new GUIStyle();
        GUIStyleState state = new GUIStyleState();
        state.textColor = new Color(1.0f, 0.0f, 0.0f, _fade);
        style.normal = state;

        GUI.Label(new Rect(x, y, w, h), _message, style);
    }
}
