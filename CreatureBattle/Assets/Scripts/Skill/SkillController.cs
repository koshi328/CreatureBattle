using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SKILL_ID
{
    // 通常攻撃
    SWORD_ATTACK_NORMAL,

    // ファイターのスキル
    FIGHTER_CONTINUOUS_ATTACK,  // 連撃
    FIGHTER_EMERGENCY_AVOID,    // 緊急回避
    FIGHTER_TRIP_STAN,          // トリップスタン

    // タンクのスキル
    TANK_CONTINUOUS_ATTACK,  // 連撃
    TANK_EMERGENCY_AVOID,    // 緊急回避
    TANK_SHIELD_BASH,

    // メイジのスキル
    MAGE_FROST_ENERGY,
    MAGE_MAGIC_IMPULSE,
    MAGE_GROUND_FROST,
    MAGE_METEOR_IMPACT,
    MAGE_ICE_BRUST,

    // モンスターのスキル
    MONSTER_CAPACITY_RISE,
    MONSTER_DRAGON_STORM,
    MONSTER_ABNORMAL_COUNTER,
    MONSTER_STAN_BREATH,

    MAGE_THUNDER_FALL,
    MAGE_FLAME_PILLAR,
    MAGE_CHASE_FLAME,

    MONSTER_EXPLOSION,
    MONSTER_DOUBLE_EDGE_RATE,
    MONSTER_CLEANSE_SYSTEM,
    MONSTER_DARK_IMPACT,
    MONSTER_LIMIT_BREAK,
    MONSTER_STADY_PROTECT,
    MONSTER_LATIS_THUNDER,

    FIGHTER_TEMPEST_BLOW,
    FIGHTER_EARTH_DESTRUCTION,
    FIGHTER_CROSS_COUNTER,
    FIGHTER_DEADLY_IMPACT,
    FIGHTER_FLASH_ASSAULT,
    FIGHTER_LAST_ATTEMPT,
    FIGHTER_RAGE_IMPACT,
    FIGHTER_INVISIBLE_DODGE,
    FIGHTER_SUMMER_SALT_RAVE,

    TANK_RESRECTIION,
    TANK_BATTKLE_INSPIRE,
    TANK_ANGRY_SHOUT,
    TANK_AGGRESSIVE_SHOUT,
    TANK_ARMOR_BREAK,
    TANK_DELAY_WAVE,
    TANK_GUARD_AGREEMENT,

    ALL_NUM,
}

public class SkillController
{
	static public SkillBase GetSkill(SKILL_ID skillID)
    {
        switch (skillID)
        {
            case SKILL_ID.SWORD_ATTACK_NORMAL: return new Skill.NormalAttack();

            case SKILL_ID.FIGHTER_CONTINUOUS_ATTACK: return new FighterSkill.ContinuousAttack();
            case SKILL_ID.FIGHTER_EMERGENCY_AVOID: return new FighterSkill.EmergencyAvoid();
            case SKILL_ID.FIGHTER_TRIP_STAN: return new FighterSkill.TripStan();
            case SKILL_ID.FIGHTER_TEMPEST_BLOW: return new FighterSkill.TempestBlow();
            case SKILL_ID.FIGHTER_EARTH_DESTRUCTION: return new FighterSkill.EarthDestruction();
            case SKILL_ID.FIGHTER_CROSS_COUNTER: return new FighterSkill.CrossCounter();
            case SKILL_ID.FIGHTER_DEADLY_IMPACT: return new FighterSkill.CrossCounter();
            case SKILL_ID.FIGHTER_FLASH_ASSAULT: return new FighterSkill.FlashAssault();
            case SKILL_ID.FIGHTER_LAST_ATTEMPT: return new FighterSkill.LastAttempt();
            case SKILL_ID.FIGHTER_RAGE_IMPACT: return new FighterSkill.RageImpact();
            case SKILL_ID.FIGHTER_INVISIBLE_DODGE: return new FighterSkill.InvisibleDodge();
            case SKILL_ID.FIGHTER_SUMMER_SALT_RAVE: return new FighterSkill.SummerSaltRave();

            case SKILL_ID.TANK_CONTINUOUS_ATTACK: return new TankSkill.ContinuousAttack();
            case SKILL_ID.TANK_EMERGENCY_AVOID: return new TankSkill.EmergencyAvoid();
            case SKILL_ID.TANK_SHIELD_BASH: return new TankSkill.ShieldBash();

            case SKILL_ID.MAGE_FROST_ENERGY: return new MageSkill.FrostEnergy();
            case SKILL_ID.MAGE_MAGIC_IMPULSE: return new MageSkill.MagicImpulse();
            case SKILL_ID.MAGE_GROUND_FROST: return new MageSkill.GroundFrost();
            case SKILL_ID.MAGE_METEOR_IMPACT: return new MageSkill.MeteorImpact();
            case SKILL_ID.MAGE_ICE_BRUST: return new MageSkill.IceBrust();
            case SKILL_ID.MAGE_THUNDER_FALL: return new MageSkill.ThunderFall();
            case SKILL_ID.MAGE_FLAME_PILLAR: return new MageSkill.FlamePillar();
            case SKILL_ID.MAGE_CHASE_FLAME: return new MageSkill.ChaseFlame();

            case SKILL_ID.MONSTER_CAPACITY_RISE: return new MonsterSkill.CapacityRise();
            case SKILL_ID.MONSTER_DRAGON_STORM: return new MonsterSkill.DragonStorm();
            case SKILL_ID.MONSTER_ABNORMAL_COUNTER: return new MonsterSkill.AbnormalCounter();
            case SKILL_ID.MONSTER_STAN_BREATH: return new MonsterSkill.StanBreath();
            case SKILL_ID.MONSTER_EXPLOSION: return new MonsterSkill.Explosion();
            case SKILL_ID.MONSTER_DOUBLE_EDGE_RATE: return new MonsterSkill.DoubleEdgeRate();
            case SKILL_ID.MONSTER_CLEANSE_SYSTEM: return new MonsterSkill.CleanseSystem();
            case SKILL_ID.MONSTER_DARK_IMPACT: return new MonsterSkill.DarkImpact();
            case SKILL_ID.MONSTER_LIMIT_BREAK: return new MonsterSkill.LimitBreak();
            case SKILL_ID.MONSTER_STADY_PROTECT: return new MonsterSkill.StadyProtect();
            case SKILL_ID.MONSTER_LATIS_THUNDER: return new MonsterSkill.LatisThunder();

            case SKILL_ID.TANK_RESRECTIION: return new TankSkill.Resurrection();
            case SKILL_ID.TANK_BATTKLE_INSPIRE: return new TankSkill.BattleInspire();
            case SKILL_ID.TANK_ANGRY_SHOUT: return new TankSkill.AngryShout();
            case SKILL_ID.TANK_AGGRESSIVE_SHOUT: return new TankSkill.AggressiveShout();
            case SKILL_ID.TANK_ARMOR_BREAK: return new TankSkill.ArmorBreak();
            case SKILL_ID.TANK_DELAY_WAVE: return new TankSkill.DelayWave();
            case SKILL_ID.TANK_GUARD_AGREEMENT: return new TankSkill.GuardAgreement();

            default: return new Skill.NormalAttack();
        }
    }
}
