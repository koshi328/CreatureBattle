using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorStatus {

    float _attack;
    float _attackInterval;
    float _hp;
    float _maxHP;
    float _speed;
    ActorCondition _condition;

    public void Initialize(ActorCondition condition, float attack, float attackInterval, float maxHP)
    {
        _hp = maxHP;
        _maxHP = maxHP;
        _attack = attack;
        _attackInterval = attackInterval;
        _condition = condition;
        _speed = 3.0f;
    }

    public float GetAttack()
    {
        return _attack * _condition.GiveDamageRate;
    }
    public float GetHP()
    {
        return _hp;
    }
    public float GetMaxHP()
    {
        return _maxHP;
    }
    public float GetAttackInterval()
    {
        return _attackInterval;
    }
    public float GetSpeed()
    {
        return _speed * _condition.SpeedDownRate;
    }

    public void TakeDamage(float damage)
    {
        damage = damage * _condition.ReciveDamageRate;
        _hp = Mathf.Clamp(_hp - damage, 0, _maxHP);
    }

    public void ReceiveRecovery(float recovery)
    {
        _hp = Mathf.Clamp(_hp + recovery, 0, _maxHP);
    }
}
