using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatticeThunder : SkillBase {

    GameObject[] _rangeObj = new GameObject[5];
    static Vector2[] pos_memory = new Vector2[5];
    ParticleSystem[] _effect = new ParticleSystem[5];
    float one_range_size = 15.0f;
    public LatticeThunder()
    {
        CAST_TIME = 1.0f;
        RECAST_TIME = 18.0f;
        ACTION_TIME = 0.0f;
        int elem = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if ((i + j) % 2 == 1) continue;
                pos_memory[elem] = new Vector2(i - 1, j - 1);
                elem++;
            }
        }
        GameObject prefab = Resources.Load("Effect/KY_effects/AMFX02/P_AMFX02_thunderVolt") as GameObject;
        for (int i = 0; i < 5; i++)
        {
            _effect[i] = GameObject.Instantiate(prefab).GetComponent<ParticleSystem>();
            _effect[i].Stop();
        }
    }

    protected override void EntryCast(Actor actor)
    {
        for (int i = 0; i < pos_memory.Length; i++)
        {
            Vector3 pos = actor.transform.forward * pos_memory[i].y * one_range_size;
            pos += actor.transform.right * pos_memory[i].x * one_range_size;
            _rangeObj[i] = EffectManager.Instance.QuadRange(actor.transform.position + pos, actor.transform.eulerAngles.y, new Vector3(one_range_size, one_range_size, one_range_size), new Color(1.0f, 0.2f, 0.0f, 1));
        }
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 pos = actor.transform.forward * pos_memory[i].y * one_range_size;
            pos += actor.transform.right * pos_memory[i].x * one_range_size;
            _effect[i].transform.position = actor.transform.position + pos;
            _effect[i].Play();
        }
        if (!actor.GetPhotonView().isMine) return;
        for (int i = 0; i < pos_memory.Length; i++)
        {
            SkillCollider col = ColliderManager.Instance.GetCollider();
            col.Initialize(actor, SkillCollider.HitTarget.Player, 2.0f, 99.0f, (argActor) =>
            {
                argActor.TakeDamage(374.0f);
                argActor.AddCondition(ActorCondition.KIND.SILENCE, 3.0f, 0.0f);
            });
            Vector3 pos = actor.transform.forward * pos_memory[i].y * one_range_size;
            pos += actor.transform.right * pos_memory[i].x * one_range_size;
            col.SetQubeCollider(actor.transform.position + pos, actor.transform.rotation, new Vector3(one_range_size, one_range_size, one_range_size));
        }
    }

    protected override void Action(Actor actor)
    {

    }

    protected override void EndAction(Actor actor)
    {
        for (int i = 0; i < 5; i++)
            GameObject.Destroy(_rangeObj[i]);
    }

    protected override void Cancel(Actor actor)
    {
        for (int i = 0; i < 5; i++)
            GameObject.Destroy(_rangeObj[i]);
    }
}
