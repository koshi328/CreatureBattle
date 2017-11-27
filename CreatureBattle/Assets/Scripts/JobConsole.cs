using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobConsole : MonoBehaviour {
    [SerializeField]
    PlayerInfoCanvas _playerInfo;
    private void OnMouseDown()
    {
        _playerInfo.gameObject.SetActive(true);   
    }
}
