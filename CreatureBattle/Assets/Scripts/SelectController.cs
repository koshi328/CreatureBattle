using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectController : MonoBehaviour
{
    [SerializeField]
    Button _decideButton;
    [SerializeField]
    GameObject _characterSelectList;
    [SerializeField]
    GameObject _characterButtonPrefab;

    string[] actorData;

    string selectActorName;
    int[] selectSkillID = new int[4];
    int decideNum;
    // Use this for initialization
    void Start()
    {
        PhotonNetwork.isMessageQueueRunning = true;
        actorData = new string[3];
        actorData[0] = "Yuki";
        actorData[1] = "Misaki";
        actorData[2] = "UnityChan";

        decideNum = 0;
        selectActorName = "";
        selectSkillID[0] = 0;
        selectSkillID[1] = 0;
        selectSkillID[2] = 0;
        selectSkillID[3] = 0;
        CreateCharacterList();
        _decideButton.onClick.AddListener(() =>
        {
            SetPlayerData(selectActorName, selectSkillID);
            var table = new ExitGames.Client.Photon.Hashtable();
            table.Add(PhotonNetwork.player.ID.ToString(), 1);
            PhotonNetwork.room.SetCustomProperties(table);
            // 仮
            _decideButton.onClick.RemoveAllListeners();
        });
    }

    void SetPlayerData(string actorName, int[] skillID)
    {
        var playerTable = new ExitGames.Client.Photon.Hashtable();
        playerTable.Add("ActorName", actorName);
        playerTable.Add("skill1", skillID[0]);
        playerTable.Add("skill2", skillID[1]);
        playerTable.Add("skill3", skillID[2]);
        playerTable.Add("skill4", skillID[3]);
        PhotonNetwork.player.SetCustomProperties(playerTable);
    }



    void CreateCharacterList()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject button = Instantiate(_characterButtonPrefab, _characterSelectList.transform);
            button.transform.localPosition = new Vector3(30 + 150 * i, 0, 0);
            button.name = i.ToString();
            button.GetComponent<Button>().onClick.AddListener(() => { selectActorName = actorData[int.Parse(button.gameObject.name)]; });
        }
    }

    void OnPhotonPlayerPropertiesChenged(object[] data)
    {
        PhotonPlayer player = data[0] as PhotonPlayer;
        ExitGames.Client.Photon.Hashtable table = data[1] as ExitGames.Client.Photon.Hashtable;
    }

    void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable table)
    {
        {
            object value = null;
            if (table.TryGetValue("1", out value))
            {
                if ((int)value == 1) decideNum++;
            }
        }
        {
            object value = null;
            if (table.TryGetValue("2", out value))
            {
                if ((int)value == 1) decideNum++;
            }
        }
        {
            object value = null;
            if (table.TryGetValue("3", out value))
            {
                if ((int)value == 1) decideNum++;
            }
        }
        {
            object value = null;
            if (table.TryGetValue("4", out value))
            {
                if ((int)value == 1) decideNum++;
            }
        }
        
        if (decideNum >= PhotonNetwork.room.PlayerCount)
        {
            PhotonNetwork.isMessageQueueRunning = false;
            SceneController.Instance.LoadScene("Game", 2.0f, true);
        }
    }
}
