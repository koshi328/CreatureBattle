using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour {

    [SerializeField]
    Button _startButton;
    private void Start()
    {
        _startButton.Select();
    }
    public void ChangeScene(string sceneName)
    {
        SceneController.Instance.LoadScene(sceneName, 2.0f, true);
    }
}
