using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ACTOR_TYPE
{
    PLAYER,
    MONSTER,
}

[CreateAssetMenu(menuName = "Example/Create Instance")]
public class ScriptableActor : ScriptableObject {
    [System.Serializable]
    public struct ActorData
    {
        public string path;
        public Sprite sprite;// あとで画像にする
        public int hp;              // 最大HP
        public int attackDamage;    // 通常攻撃のダメージ
        public float attackInterval;// 通常攻撃の間隔
        public ACTOR_TYPE actorType;// 人間かモンスターか
        public ScriptableSkill skillData;
    }

    public ActorData[] data;
    
}
