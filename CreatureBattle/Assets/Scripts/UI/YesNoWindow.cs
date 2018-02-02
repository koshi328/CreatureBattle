using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using antilunchbox;

public class YesNoWindow : MonoBehaviour {

    [SerializeField]
    Button _yesButton;
    [SerializeField]
    Button _noButton;
    [SerializeField]
    Text _messageText;
    [SerializeField]
    Button _createRoomButton;

    public string roomName;
    // Use this for initialization
    void Start () {
        NoButtonAddEvent(()=> { this.gameObject.SetActive(false); });
    }

    private void Update()
    {
        _yesButton.Select();

        if (Input.GetButtonDown("Submit"))
        {
            ClickedYes();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            ClickedNo();
        }
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

    public void ClickedYes()
    {
        SoundManager.PlaySFX("se_050");
    }

    public void ClickedNo()
    {
        SoundManager.PlaySFX("se_051");
        gameObject.SetActive(false);
        _createRoomButton.Select();
    }
}
