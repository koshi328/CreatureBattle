using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Example/Create Instance")]
public class ScriptableActor : ScriptableObject {
    [System.Serializable]
    public struct ActorData
    {
        public string path;
        public Sprite sprite;// あとで画像にする
    }

    public ActorData[] data;
    
}
