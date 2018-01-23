using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class NetworkManager : MonoBehaviour {
    
    public static NetworkManager Instance
    {
        get;
        private set;
    }

	// Use this for initialization
	void Start () {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        PhotonNetwork.ConnectUsingSettings("0.0");
	}
	
    void OnJoinedLobby()
    {
        Debug.Log("Lobby");
    }

    void OnJoinedRoom()
    {
        Debug.Log("Join成功");
        //PhotonNetwork.isMessageQueueRunning = false;
        SceneController.Instance.LoadScene("Select", 2.0f, true);
    }

    void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("Join失敗");
    }
}
