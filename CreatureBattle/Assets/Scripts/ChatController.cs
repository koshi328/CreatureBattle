using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour {

    [SerializeField]
    Text _messageQueue;

    public static ChatController Instance;

    private void Start()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
    }

    public void AddMessage(string message)
    {
        _messageQueue.text += message + "\n";
    }
}
