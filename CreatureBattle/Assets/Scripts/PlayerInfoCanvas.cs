using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoCanvas : MonoBehaviour {

    [SerializeField]
    Text _infoText;
    [SerializeField]
    Button _nextButton;
    [SerializeField]
    Button _prevButton;
    [SerializeField]
    Button _decideButton;
    [SerializeField]
    Button _cancelButton;
    

    struct INFO
    {
        public string job;
        public string path;
    }
    INFO[] infoList;
    int _current;

    private void Start()
    {
        _current = 0;
        infoList = new INFO[3];
        infoList[0].job = "剣士"; infoList[0].path = "Yuko";
        infoList[1].job = "魔法"; infoList[1].path = "Misaki";
        infoList[2].job = "盾";   infoList[2].path = "UnityChan";

        _decideButton.onClick.AddListener(()=> { ChangeActor(); gameObject.SetActive(false); });
        _nextButton.onClick.AddListener(() => { _current++; ShowInfo(); });
        _prevButton.onClick.AddListener(() => { _current--; ShowInfo(); });
        _cancelButton.onClick.AddListener(()=> { gameObject.SetActive(false); });
        ShowInfo();
    }

    void ShowInfo()
    {
        if (_current >= infoList.Length) _current = 0;
        if (_current < 0) _current = infoList.Length - 1;
        _infoText.text = infoList[_current].job;
    }

    void ChangeActor()
    {
        GameController gameCon = GameObject.Find("GameController").GetComponent<GameController>();
        GameObject player = gameCon.GetPlayer();
        Destroy(player);
        gameCon.CreatePlayer(infoList[_current].path, player.transform.position, player.transform.rotation);
    }
}
