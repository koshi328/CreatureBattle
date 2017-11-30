using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Example/Create Instance")]
public class ScriptableActor : ScriptableObject {
    [System.Serializable]
    public struct ActorData
    {
        public string path;
        public Color color;// あとで画像にする
        public ActorData(string path, Color color)
        {
            this.path = path;
            this.color = color;
        }
    }

    public ActorData[] data;
    
}
