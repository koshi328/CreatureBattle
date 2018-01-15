using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SKILL_ID
{
    // ファイター
    FIGHTER_TEMPEST_BLOW,
    FIGHTER_EARTH_DESTRUCTION,
    FIGHTER_DEADLY_IMPACT,
    FIGHTER_RAGE_IMPACT,
    // タンク
    TANK_STAND_GUARD,
    TANK_ANGRY_SHOUT,
    TANK_SHIELD_BASH,
    TANK_AGGRESSIVE_SHOUT,

    // メイジのスキル
    MAGE_GROUND_FROST,
    MAGE_METEOR_IMPACT,
    MAGE_THUNDER_TRAP,
    MAGE_CHASE_FLAME,

    // モンスターのスキル
    MONSTER_DRAGON_STORM,
    MONSTER_ABNORMAL_COUNTER,
    MONSTER_STAN_BREATH,
    MONSTER_INITIALIZE_WAVE,
    MONSTER_EXPLOSION,
    MONSTER_DOUBLE_EDGE_RATE,
    MONSTER_LIMIT_BREAK,
    MONSTER_STADY_PROTECT,
    MONSTER_LATIS_THUNDER,

    ALL_NUM,
}

public class SkillGenerator
{
    static public SkillBase GetSkill(SKILL_ID skillID)
    {
        switch (skillID)
        {
    case SKILL_ID.FIGHTER_TEMPEST_BLOW:return new TempestBlow();
    case SKILL_ID.FIGHTER_EARTH_DESTRUCTION:return new EarthDestraction();
            case SKILL_ID.FIGHTER_DEADLY_IMPACT:return new DeadlyImpact();
            case SKILL_ID.FIGHTER_RAGE_IMPACT: return new RageImpact();
            case SKILL_ID.TANK_STAND_GUARD: return new StandGuard();
            case SKILL_ID.TANK_ANGRY_SHOUT: return new AngryShout();
            case SKILL_ID.TANK_SHIELD_BASH: return new ShieldBash();
            case SKILL_ID.TANK_AGGRESSIVE_SHOUT: return new AggressiveShout();
            case SKILL_ID.MAGE_GROUND_FROST: return new GroundFrost();
            case SKILL_ID.MAGE_METEOR_IMPACT: return new MeteoImpact();
            case SKILL_ID.MAGE_THUNDER_TRAP: return new ThunderTrap();
            case SKILL_ID.MAGE_CHASE_FLAME: return new ChaseFlame();
            case SKILL_ID.MONSTER_DRAGON_STORM: return new DragonStorm();
            case SKILL_ID.MONSTER_ABNORMAL_COUNTER: return new AbnormalCounter();
            case SKILL_ID.MONSTER_STAN_BREATH: return new StanBreath();
            case SKILL_ID.MONSTER_INITIALIZE_WAVE: return new InitializeWave();
            case SKILL_ID.MONSTER_EXPLOSION: return new Explosion();
            case SKILL_ID.MONSTER_DOUBLE_EDGE_RATE: return new DoubleEdgeRage();
            case SKILL_ID.MONSTER_LIMIT_BREAK: return new LimitBreak();
            case SKILL_ID.MONSTER_STADY_PROTECT: return new StuddiProtect();
            case SKILL_ID.MONSTER_LATIS_THUNDER: return new LatticeThunder();
        }
        return null;
    }
}
