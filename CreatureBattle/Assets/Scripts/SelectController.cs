using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectController : MonoBehaviour
{
    [SerializeField]
    SkillSelectWindow skillWindow;
    [SerializeField]
    Image[] _playerImage = new Image[4];
    [SerializeField]
    Outline[] _playerImageOutLine = new Outline[4];
    [SerializeField]
    Image[] _skillImage = new Image[3];
    [SerializeField]
    GameObject _characterSelectList;
    [SerializeField]
    GameObject _characterButtonPrefab;
    [SerializeField]
    GameObject _decideFilter;

    [SerializeField]
    ScriptableActor actorData;
    [SerializeField]
    ScriptableAllSkills skillsData;


    int selectActorID;
    int[] selectSkillID = new int[4];

    int _myElem;
    SelectActorConnector[] _connector = new SelectActorConnector[4];
    // Use this for initialization
    void Start()
    {
        PhotonNetwork.isMessageQueueRunning = true;
        selectActorID = 0;
        for (int i = 0; i < selectSkillID.Length; i++)
        {
            selectSkillID[i] = -1;
        }

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
        if ((int)player != 0)
        {
            PhotonNetwork.LeaveRoom();
            SceneController.Instance.LoadScene("Lobby", 2.0f, true);
            return;
        }
        var roomProperties = new ExitGames.Client.Photon.Hashtable();
        roomProperties.Add(PhotonNetwork.playerName, 1);
        PhotonNetwork.room.SetCustomProperties(roomProperties);

        if (PhotonNetwork.playerName == "monster")
            actorData = Resources.Load("MonsterData") as ScriptableActor;
        else
            actorData = Resources.Load("ActorData") as ScriptableActor;
        CreateCharacterList();

        
        string name = PhotonNetwork.playerName;
             if (name == "player1") _myElem = 0;
        else if (name == "player2") _myElem = 1;
        else if (name == "player3") _myElem = 2;
        else if (name == "monster") _myElem = 3;
        SelectActorConnector temp = PhotonNetwork.Instantiate("SelectActorConnector", Vector3.zero, Quaternion.identity, 0).GetComponent<SelectActorConnector>();
        temp.Initialize(_myElem, PhotonNetwork.playerName);
    }

    private void Update()
    {
        // デバッグ用
        if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.Z))
        {
            PhotonNetwork.isMessageQueueRunning = false;
            SceneController.Instance.LoadScene("Game", 2.0f, true);
            PhotonNetwork.room.IsVisible = false;
        }
        if (SceneController.Instance.NowFade()) return;
        string name = PhotonNetwork.playerName;
             if (name == "player1") _myElem = 0;
        else if (name == "player2") _myElem = 1;
        else if (name == "player3") _myElem = 2;
        else if (name == "monster") _myElem = 3;
        for (int i = 0; i < 4; i++)
        {
            if (!_connector[i])
            {
                _playerImageOutLine[i].effectColor = new Color(0.3f, 0.3f, 0.3f, 0.7f);
                continue;
            }
            if (_myElem == i)
                _playerImageOutLine[i].effectColor = new Color(0, 0, 1, 0.7f);
            else
                _playerImageOutLine[i].effectColor = new Color(0, 1, 0, 0.7f);
            if (_connector[i].isDone())
            {
                _playerImageOutLine[i].effectColor = new Color(1, 1, 0, 0.7f);
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (!_connector[i]) return;
            if (!_connector[i].isDone()) return;
            if(i == 3)
            {
                PhotonNetwork.isMessageQueueRunning = false;
                SceneController.Instance.LoadScene("Game", 2.0f, true);
                PhotonNetwork.room.IsOpen = false;
                _decideFilter.SetActive(false);
            }
        }

    }

    void SetPlayerData(int actorID)
    {
        var playerTable = new ExitGames.Client.Photon.Hashtable();
        playerTable.Add("ActorID", actorID);
        PhotonNetwork.SetPlayerCustomProperties(playerTable);
    }
    void OnPhotonPlayerConnected()
    {
        SetPlayerData(selectActorID);
    }

    void CreateCharacterList()
    {
        while(_characterSelectList.transform.childCount != 0)
        {
            Destroy(_characterSelectList.transform.GetChild(0));
        }
        for (int i = 0; i < actorData.data.Length; i++)
        {
            GameObject button = Instantiate(_characterButtonPrefab, _characterSelectList.transform);
            button.transform.localPosition = new Vector3(30 + 150 * i, 0, 0);
            button.name = i.ToString();
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                selectActorID = int.Parse(button.gameObject.name);
                // これを消せば、準備完了ボタンを押すまで相手の画面に表示されない
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
        else if (name == "player2") elem = 1;
        else if (name == "player3") elem = 2;
        else if (name == "monster") elem = 3;

        if (PhotonNetwork.playerName == "monster" && name != "monster")
        {
            return;
        }
        if (PhotonNetwork.playerName != "monster" && name == "monster")
        {
            return;
        }
        object value = null;
        if (table.TryGetValue("ActorID", out value))
        {
            _playerImage[elem].sprite = actorData.data[(int)value].sprite;
        }
    }
    
    public void Decide()
    {
        if (selectActorID == -1) return;
        if (selectSkillID[2] == -1) return;
        var playerTable = new ExitGames.Client.Photon.Hashtable();
        playerTable.Add("ActorID", selectActorID);
        for (int i = 0; i <= 3; i++)
            playerTable.Add("skill" + (i + 1).ToString(), selectSkillID[i]);
        PhotonNetwork.SetPlayerCustomProperties(playerTable);
        _connector[_myElem].Done(true);
        _decideFilter.SetActive(true);
    }

    public void Cancel()
    {
        skillWindow.SkillReset();
        _connector[_myElem].Done(false);
        _decideFilter.SetActive(false);
    }

    public void SetSkill(int elem, int id)
    {
        if (id < 0)
        {
            selectSkillID[elem] = id;
            _skillImage[elem].sprite = null;
            return;
        }
        _skillImage[elem].sprite = skillsData.data[id].sprite;
        selectSkillID[elem] = id;
    }

    public void SetConnector(int elem, SelectActorConnector connector)
    {
        if (connector == null)
        {
            _connector[elem] = connector;
            return;
        }
        if (!PhotonNetwork.isMasterClient)
        {
            _connector[elem] = connector;
            return;
        }
        if (_connector[elem] != null)
        {
            if (_connector[elem] == connector) return;
            string[] nameList = { "player1", "player2", "player3", "monster" };
            object value = null;
            for (int i = 0; i < nameList.Length; i++)
            {
                PhotonNetwork.room.CustomProperties.TryGetValue(nameList[i], out value);
                if ((int)value == 0)
                {
                    var roomProperties = new ExitGames.Client.Photon.Hashtable();
                    roomProperties.Add(nameList[i], 1);
                    PhotonNetwork.room.SetCustomProperties(roomProperties);
                    _connector[elem].Initialize(elem, _connector[elem]._playerName);
                    connector.Initialize(i, nameList[i]);
                    connector.ChangePhotonPlayerName(nameList[i]);
                    return;
                }
            }
            connector.LeaveRoom();
        }
        else
        {
            _connector[elem] = connector;
        }
    }
    void OnMasterClientSwitched()
    {
        PhotonNetwork.LeaveRoom();
        SceneController.Instance.LoadScene("Lobby", 2.0f, true);
    }
    public int GetDoneCount()
    {
        int n = 0;
        for (int i = 0; i < _connector.Length; i++)
        {
            if (_connector == null) continue;
            if (_connector[i].isDone() == false) continue;
            n++;
        }
        return n;
    }
}
