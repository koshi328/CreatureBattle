using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YesNoWindow : MonoBehaviour {

    [SerializeField]
    Button _yesButton;
    [SerializeField]
    Button _noButton;
    [SerializeField]
    Text _messageText;

    public string roomName;
    // Use this for initialization
    void Start () {
        NoButtonAddEvent(()=> { this.gameObject.SetActive(false); });
    }
    
    public void SetMessage(string message)
    {
        if (_messageText == null) return;
        _messageText.text = message;
    }

    public void YesButtonAddEvent(UnityEngine.Events.UnityAction action, bool reset = false)
    {
        if(reset)
        {
            _yesButton.onClick.RemoveAllListeners();
        }
        _yesButton.onClick.AddListener(action);
    }

    public void NoButtonAddEvent(UnityEngine.Events.UnityAction action, bool reset = false)
    {
        if (reset)
        {
            _noButton.onClick.RemoveAllListeners();
        }
        _noButton.onClick.AddListener(action);
    }
}
