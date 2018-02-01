using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour {

    public void LoadScene(string sceneName)
    {
        SceneController.Instance.LoadScene(sceneName, 2.0f, true);
    }
}
