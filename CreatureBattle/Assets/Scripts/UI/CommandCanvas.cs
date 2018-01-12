using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandCanvas : MonoBehaviour
{
    [SerializeField]
    Button[] _commandButton;
    [SerializeField]
    ScriptableAllSkills _skillData;

    [SerializeField]
    Image[] _commandRecastImage;
    [SerializeField]
    Image[] _commandImage;

    /// <summary>
    /// ボタンをクリックした時のイベントを設定する
    /// </summary>
    /// <param name="elem"></param>
    /// <param name="action"></param>
    public void SetCommand(int elem, UnityEngine.Events.UnityAction action)
    {
        if (elem >= _commandButton.Length) return;
        _commandButton[elem].onClick.RemoveAllListeners();
        _commandButton[elem].onClick.AddListener(action);
    }

    /// <summary>
    /// ボタンの押下可能か否かを切り替える
    /// </summary>
    /// <param name="elem"></param>
    /// <param name="flag"></param>
    public void SetInteractable(int elem, bool flag)
    {
        _commandButton[elem].interactable = flag;
    }

    /// <summary>
    /// ボタンの画像の表示割合を変える
    /// </summary>
    /// <param name="elem"></param>
    /// <param name="amount"></param>
    public void SetFillAmount(int elem, float amount)
    {
        _commandRecastImage[elem].fillAmount = amount;
    }

    public void SetImage(int elem, int skillID)
    {
        _commandImage[elem].sprite = _skillData.data[skillID].sprite;
    }
}
