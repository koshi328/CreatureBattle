using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    static public AudioController Instance
    {
        get;
        private set;
    }

    Dictionary<string, AudioClip> seList = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> bgmList = new Dictionary<string, AudioClip>();

    AudioSource _bgmAudioSource;
    AudioSource _seAudioSource;

    void SoundLoad()
    {

    }

	void Start () {
		if(Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        DontDestroyOnLoad(this);
        SoundLoad();
    }

    public void SetBGM()
    {

    }

    void Register(int n, string name)
    {

    }
}
