using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour {

    [SerializeField]
    Text _messageQueue;
    [SerializeField]
    InputField _input;
    [SerializeField]
    Button _sendButton;

    private void Start()
    {
        _sendButton.onClick.AddListener(()=> { SendMessage(); });
        _input.onEndEdit.AddListener(value => { SendMessage(); });
    }

    public void SendMessage()
    {
        if (_input.text == "") return;
        SendHashTable(PhotonNetwork.playerName + ":" + _input.text + "\n");
        _input.text = "";
    }
    void SendHashTable(string message)
    {
        var properties = new ExitGames.Client.Photon.Hashtable();
        properties.Add("Message", message);
        PhotonNetwork.room.SetCustomProperties(properties);
    }

    public void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable hash)
    {
        object value = null;
        if(hash.TryGetValue("Message" ,out value))
        {
            _messageQueue.text += (string)value;
            Debug.Log((string)value);
        }
    }
}
