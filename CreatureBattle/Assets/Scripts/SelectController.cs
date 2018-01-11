using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectController : MonoBehaviour
{
    [SerializeField]
    SkillSelectWindow skillWindow;
    [SerializeField]
    Button _decideButton;
    [SerializeField]
    Image[] _playerImage = new Image[4];
    [SerializeField]
    Image[] _skillImage = new Image[12];
    [SerializeField]
    GameObject _characterSelectList;
    [SerializeField]
    GameObject _characterButtonPrefab;

    [SerializeField]
    ScriptableActor actorData;
    [SerializeField]
    ScriptableAllSkills skillsData;
    

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

        if(PhotonNetwork.playerName == "monster")
            actorData = Resources.Load("MonsterData") as ScriptableActor;
        else
            actorData = Resources.Load("ActorData") as ScriptableActor;
        CreateCharacterList();
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

    void SetPlayerData(int actorID)
    {
        var playerTable = new ExitGames.Client.Photon.Hashtable();
        playerTable.Add("ActorID", actorID);
        PhotonNetwork.SetPlayerCustomProperties(playerTable);
    }

    public void SetSkillData(int skillElem,int skillID)
    {
        skillElem = Mathf.Clamp(skillElem, 0, 3);
        var playerTable = new ExitGames.Client.Photon.Hashtable();
        playerTable.Add("skill" + (skillElem + 1).ToString(), skillID);
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
                SetPlayerData(selectActorID);
                skillWindow.ChangeActor(actorData, selectActorID);
            });
            button.GetComponent<Button>().image.sprite = actorData.data[i].sprite;
        }
    }

    void OnPhotonPlayerPropertiesChanged(object[] data)
    {
        // RPCでやればよかった暇があったら直す
        PhotonPlayer player = data[0] as PhotonPlayer;
        ExitGames.Client.Photon.Hashtable table = data[1] as ExitGames.Client.Photon.Hashtable;
        int elem = 0;
        string name = player.NickName;
        if (name == "player1") elem = 0;
        if (name == "player2") elem = 1;
        if (name == "player3") elem = 2;
        if (name == "monster") elem = 3;

        if (PhotonNetwork.playerName == "monster" && name != "monster")
        {
            return;
        }
        if (PhotonNetwork.playerName != "monster" && name == "monster")
        {
            return;
        }
        object value = null;
        if(table.TryGetValue("ActorID", out value))
        {
            _playerImage[elem].sprite = actorData.data[(int)value].sprite;
        }

        player.CustomProperties.TryGetValue("ActorID", out value);
        int actorNum = (int)value;

        if (table.TryGetValue("skill1",out value))
        {
            _skillImage[elem * 3].sprite = ((int)value < 0) ? null : skillsData.data[(int)value].sprite;
        }
        if (table.TryGetValue("skill2", out value))
        {
            _skillImage[elem * 3 + 1].sprite = ((int)value < 0) ? null : skillsData.data[(int)value].sprite;
        }
        if (table.TryGetValue("skill3", out value))
        {
            _skillImage[elem * 3 + 2].sprite = ((int)value < 0) ? null : skillsData.data[(int)value].sprite;
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
            PhotonNetwork.room.IsVisible = false;
        }
    }
}
