using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectController : MonoBehaviour
{
    [SerializeField]
    Button _decideButton;
    [SerializeField]
    Image[] _playerImage = new Image[4];
    [SerializeField]
    GameObject _characterSelectList;
    [SerializeField]
    GameObject _characterButtonPrefab;

    [SerializeField]
    ScriptableActor actorData;
    

    int selectActorID;
    int[] selectSkillID = new int[4];
    int decideNum;
    // Use this for initialization
    void Start()
    {
        PhotonNetwork.isMessageQueueRunning = true;
        decideNum = 0;
        selectActorID = 0;
        selectSkillID[0] = 0;
        selectSkillID[1] = 0;
        selectSkillID[2] = 0;
        selectSkillID[3] = 0;
        CreateCharacterList();
        _decideButton.onClick.AddListener(() =>
        {
            var table = new ExitGames.Client.Photon.Hashtable();
            table.Add(PhotonNetwork.player.NickName, 2);
            PhotonNetwork.room.SetCustomProperties(table);
            // 仮
            _decideButton.onClick.RemoveAllListeners();
        });

        // モンスター側のplayerか人間側のplayerか判断して処理する
        object isMonster = null;
        object player = null;
        PhotonNetwork.player.CustomProperties.TryGetValue("monster", out isMonster);
        if ((bool)isMonster == true)
        {
            PhotonNetwork.playerName = "monster";
        }
        else
        {
            for (int i = 1; i <= 3; i++)
            {
                PhotonNetwork.room.CustomProperties.TryGetValue("player" + i.ToString(), out player);
                if ((int)player == 0)
                {
                    PhotonNetwork.playerName = "player" + i.ToString();
                    break;
                }
            }
        }
        PhotonNetwork.room.CustomProperties.TryGetValue(PhotonNetwork.playerName, out player);
        if((int)player != 0)
        {
            PhotonNetwork.LeaveRoom();
            return;
        }
        var roomProperties = new ExitGames.Client.Photon.Hashtable();
        roomProperties.Add(PhotonNetwork.playerName, 1);
        PhotonNetwork.room.SetCustomProperties(roomProperties);
    }

    void OnPhotonPlayerDisconnected()
    {
        Debug.Log("Disconnected");
        var properties = new ExitGames.Client.Photon.Hashtable();
        properties.Add(PhotonNetwork.playerName, 0);
        PhotonNetwork.room.SetCustomProperties(properties);
    }

    private void OnApplicationQuit()
    {
        OnPhotonPlayerDisconnected();
    }

    void SetPlayerData(int actorID, int[] skillID)
    {
        var playerTable = new ExitGames.Client.Photon.Hashtable();
        playerTable.Add("ActorID", actorID);
        playerTable.Add("skill1", skillID[0]);
        playerTable.Add("skill2", skillID[1]);
        playerTable.Add("skill3", skillID[2]);
        playerTable.Add("skill4", skillID[3]);
        PhotonNetwork.SetPlayerCustomProperties(playerTable);
    }



    void CreateCharacterList()
    {
        for (int i = 0; i < actorData.data.Length; i++)
        {
            GameObject button = Instantiate(_characterButtonPrefab, _characterSelectList.transform);
            button.transform.localPosition = new Vector3(30 + 150 * i, 0, 0);
            button.name = i.ToString();
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                selectActorID = int.Parse(button.gameObject.name);
                SetPlayerData(selectActorID, selectSkillID);
            });
        }
    }

    void OnPhotonPlayerPropertiesChanged(object[] data)
    {
        PhotonPlayer player = data[0] as PhotonPlayer;
        ExitGames.Client.Photon.Hashtable table = data[1] as ExitGames.Client.Photon.Hashtable;
        object value = null;
        if (table.TryGetValue("ActorID", out value))
        {
            int elem = 0;
            string name = player.NickName;
            if (name == "player1") elem = 0;
            if (name == "player2") elem = 1;
            if (name == "player3") elem = 2;
            if (name == "monster") elem = 3;
            _playerImage[elem].sprite = actorData.data[(int)value].sprite;
        }
    }

    void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable table)
    {
        {
            object value = null;
            if (table.TryGetValue("player1", out value))
            {
                if ((int)value == 2) decideNum++;
            }
        }
        {
            object value = null;
            if (table.TryGetValue("player2", out value))
            {
                if ((int)value == 2) decideNum++;
            }
        }
        {
            object value = null;
            if (table.TryGetValue("player3", out value))
            {
                if ((int)value == 2) decideNum++;
            }
        }
        {
            object value = null;
            if (table.TryGetValue("monster", out value))
            {
                if ((int)value == 2) decideNum++;
            }
        }
        
        if (decideNum >= PhotonNetwork.room.PlayerCount)
        {
            PhotonNetwork.isMessageQueueRunning = false;
            SceneController.Instance.LoadScene("Game", 2.0f, true);
        }
    }
}