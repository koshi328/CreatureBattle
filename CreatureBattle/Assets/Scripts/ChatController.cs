using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour {

    [SerializeField]
    Text _messageQueue;
    [SerializeField]
    Scrollbar _bar;

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
        return;
        _messageQueue.text += message + "\n";
        //_bar.value = 0;
    }
}
