using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorCondition : MonoBehaviour {
    public enum KIND
    {
        STAN,              
        SILENCE,
        // ファイター
        TEMPEST_BLOW,    
        EARTH_DESTRACTION,
        DEADLY_IMPACT,   
        // タンク
        AGGRESSIVE_SHOUT,
        STAND_GUARD,     
        ANGRY_SHOUT,
        // メイジ
        GROUND_FROST,
        METEO_IMPACT,
        // モンスター
        STAN_BLESS,
        INITIALIZE_WAVE,
        EXPLOSION,
        DOUBLE_EDGE,
        LIMIT_BREAK,
        STUDII_PROTECT,
        ABNORMAL_COUNTER,

        MAX_NUM
    }
    Condition[] _conditions;
    public float ReciveDamageRate { get; set; }
    public float GiveDamageRate { get; set; }
    public float SpeedDownRate { get; set; }
    public float RecoveryRate { get; set; }

    ParticleSystem _giveDamageUp;
    ParticleSystem _reciveDamageDown;
    ParticleSystem _speedDown;

    public void Initialize()
    {
        _conditions = new Condition[(int)KIND.MAX_NUM];
        _conditions[(int)KIND.STAN]              = new StanCondition(1, 60);
        _conditions[(int)KIND.SILENCE]           = new SilenceCondition(1, 60);
        // ファイター
        _conditions[(int)KIND.TEMPEST_BLOW]      = new TempestBlowCondition(99, 10);
        _conditions[(int)KIND.EARTH_DESTRACTION] = new EarthDestractionCondition(1, 10);
        _conditions[(int)KIND.DEADLY_IMPACT]     = new DeadlyImpactCondition(99, 3);
        // タンク
        _conditions[(int)KIND.AGGRESSIVE_SHOUT]  = new AggressiveShoutCondition(1, 60);
        _conditions[(int)KIND.STAND_GUARD]       = new StandGuardCondition(1, 60);
        _conditions[(int)KIND.ANGRY_SHOUT]       = new AngryShoutCondition(1, 60);
        // メイジ
        _conditions[(int)KIND.GROUND_FROST]      = new GroundFrostCondition(1, 60);
        _conditions[(int)KIND.METEO_IMPACT]      = new MeteoImpactCondition(1, 60);
        // モンスター
        _conditions[(int)KIND.STAN_BLESS]        = new StanBlessCondition(1, 60);
        _conditions[(int)KIND.INITIALIZE_WAVE]   = new InitializeWaveCondition(1, 60);
        _conditions[(int)KIND.EXPLOSION]         = new ExplosionCondition(1, 60);
        _conditions[(int)KIND.DOUBLE_EDGE]       = new DoubleEdgeCondition(99, 60);
        _conditions[(int)KIND.LIMIT_BREAK]       = new LimitBreakCondition(1, 60);
        _conditions[(int)KIND.STUDII_PROTECT]    = new StudiiProtectCondition(1, 60);
        _conditions[(int)KIND.ABNORMAL_COUNTER]  = new AbnormalCounterCondition(1, 60);

        _giveDamageUp = Instantiate(Resources.Load("Effect/ItoEffects/DamageUp") as GameObject).GetComponent<ParticleSystem>();
        _reciveDamageDown = Instantiate(Resources.Load("Effect/ItoEffects/DefenseUp") as GameObject).GetComponent<ParticleSystem>();
        _speedDown = Instantiate(Resources.Load("Effect/ItoEffects/SpeedDown") as GameObject).GetComponent<ParticleSystem>();
    }

    public void Execute(Actor actor)
    {
        ReciveDamageRate = 1.0f;
        GiveDamageRate = 1.0f;
        SpeedDownRate = 1.0f;
        RecoveryRate = 1.0f;
        for (int i = 0; i < (int)KIND.MAX_NUM; i++)
        {
            _conditions[i].Update(actor);
        }

        EffectUpdate(actor);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            actor.AddCondition(KIND.AGGRESSIVE_SHOUT, 10.0f, 0.0f);
        }
    }

    public bool DontMove()
    {
        bool stan = _conditions[(int)KIND.STAN].GetStack() > 0;
        return stan;
    }
    public bool DontUseSkill()
    {
        bool silence = _conditions[(int)KIND.SILENCE].GetStack() > 0;
        bool stan = _conditions[(int)KIND.STAN].GetStack() > 0;
        return silence || stan;
    }

    public Condition GetCondition(KIND kind)
    {
        return _conditions[(int)kind];
    }

    public void ClearDebuff()
    {
        _conditions[(int)KIND.STAN].SetTime(0.0f);
        _conditions[(int)KIND.SILENCE].SetTime(0.0f);
        _conditions[(int)KIND.TEMPEST_BLOW].SetTime(0.0f);
        _conditions[(int)KIND.EARTH_DESTRACTION].SetTime(0.0f);
        _conditions[(int)KIND.DEADLY_IMPACT].SetTime(0.0f);
        _conditions[(int)KIND.GROUND_FROST].SetTime(0.0f);
        _conditions[(int)KIND.METEO_IMPACT].SetTime(0.0f);
        _conditions[(int)KIND.STAN_BLESS].SetTime(0.0f);
        _conditions[(int)KIND.EXPLOSION].SetTime(0.0f);
        _conditions[(int)KIND.ANGRY_SHOUT].SetTime(0.0f);

        //_conditions[(int)KIND.AGGRESSIVE_SHOUT].SetTime(0.0f);
        //_conditions[(int)KIND.STAND_GUARD].SetTime(0.0f);
        //_conditions[(int)KIND.INITIALIZE_WAVE].SetTime(0.0f);
        //_conditions[(int)KIND.DOUBLE_EDGE].SetTime(0.0f);
        //_conditions[(int)KIND.LIMIT_BREAK].SetTime(0.0f);
        //_conditions[(int)KIND.STUDII_PROTECT].SetTime(0.0f);
    }

    private void EffectUpdate(Actor actor)
    {
        if (GiveDamageRate > 1.0f)
        {
            _giveDamageUp.transform.position = actor.transform.position;
            _giveDamageUp.transform.localScale = actor.transform.localScale;
            if (!_giveDamageUp.isPlaying)
                _giveDamageUp.Play();
        }
        else
            _giveDamageUp.Stop();

        if (ReciveDamageRate < 1.0f)
        {
            _reciveDamageDown.transform.position = actor.transform.position;
            _giveDamageUp.transform.localScale = actor.transform.localScale;
            if (!_reciveDamageDown.isPlaying)
                _reciveDamageDown.Play();
        }
        else
            _reciveDamageDown.Stop();

        if (SpeedDownRate < 1.0f)
        {
            _speedDown.transform.position = actor.transform.position;
            _giveDamageUp.transform.localScale = actor.transform.localScale;
            if (!_speedDown.isPlaying)
                _speedDown.Play();
        }
        else
            _speedDown.Stop();
    }
}

public class Condition
{
    protected int _stack;
    protected float _time;
    protected int _maxStack;
    protected float _maxTime;
    protected float _rate;

    public Condition(int maxStack, float maxTime)
    {
        _maxStack = maxStack;
        _maxTime = maxTime;
        _stack = 0;
        _time = _maxTime;
    }
    public int GetStack()
    {
        return _stack;
    }
    public virtual void AddStack(float time, float rate, Actor actor, bool isTimeUpdate = true)
    {
        if(isTimeUpdate)
        {
            _time = Mathf.Clamp(time, 0, _maxTime);
        }
        else
        {
            if(_stack == 0)
            {
                _time = time;
            }
        }
        if (_stack >= _maxStack) return;
        _rate = rate;
        _stack++;
        Entry(actor);
    }
    public void SetTime(float time)
    {
        _time = time;
    }
    public float GetRate()
    {
        return _rate;
    }
    public float GetTime()
    {
        return _time;
    }
    protected virtual void Entry(Actor actor) { }
    protected virtual void Execute(Actor actor) { }
    protected virtual void Exit(Actor actor)
    {
        _stack = 0;
    }

    public void Update(Actor actor)
    {
        if (_stack <= 0) return;
        if (_time <= 0.0f)
        {
            Exit(actor);
            return;
        }
        _time -= Time.deltaTime;
        Execute(actor);
    }
}

public class StanCondition : Condition
{
    public StanCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }

    protected override void Execute(Actor actor)
    {
        actor.GetAnimator().SetTrigger("React");
    }
}

public class SilenceCondition : Condition
{
    public SilenceCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
}
