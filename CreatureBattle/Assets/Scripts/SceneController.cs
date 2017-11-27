using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class SceneController :MonoBehaviour {
    [SerializeField]
    Image _fadeFilter;

    public delegate void nonParamDelegate();

    bool _nowFade;
    
    public static SceneController Instance
    {
        get;
        private set;
    }

    private void Start()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        _nowFade = false;
    }

    public void LoadScene(string name, float time, bool useFade, nonParamDelegate finalizeDelegate = null)
    {
        if (_nowFade) return;
        if (useFade)
        {
            StartCoroutine(_Fade(time, () => { SceneManager.LoadScene(name); }, finalizeDelegate));
            return;
        }
        SceneManager.LoadScene(name);
        if (finalizeDelegate != null)
            finalizeDelegate();
    }

    public void Fade(float time, nonParamDelegate middleDelegate = null, nonParamDelegate finalizeDelegate = null)
    {
        if (_nowFade) return;
        StartCoroutine(_Fade(time, middleDelegate, finalizeDelegate));
    }

    IEnumerator _Fade(float time, nonParamDelegate middleDelegate = null, nonParamDelegate finalizeDelegate = null)
    {
        _fadeFilter.gameObject.SetActive(true);
        _nowFade = true;
        for (int i = 0; i < 60 * time / 2; i++)
        {
            _fadeFilter.color += new Color(0.0f, 0.0f, 0.0f, Time.deltaTime);
            yield return null;
        }
        if(middleDelegate != null)
        {
            middleDelegate();
        }
        for (int i = 0; i < 60 * time / 2; i++)
        {
            _fadeFilter.color -= new Color(0.0f, 0.0f, 0.0f, Time.deltaTime);
            yield return null;
        }
        if (finalizeDelegate != null)
        {
            finalizeDelegate();
        }
        _nowFade = false;
        _fadeFilter.gameObject.SetActive(false);
    }
}
