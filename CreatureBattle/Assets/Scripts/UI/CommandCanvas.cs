using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandCanvas : MonoBehaviour {
    [SerializeField]
    Button[] _commandButton;

    public void SetCommand(int elem, UnityEngine.Events.UnityAction action)
    {
        if (elem >= _commandButton.Length) return;
        _commandButton[elem].onClick.RemoveAllListeners();
        _commandButton[elem].onClick.AddListener(action);
    }
}
