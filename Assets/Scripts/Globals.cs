using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals
{
    public static bool DebugMode = false;

    public enum EnemyTypes {
        Robot,
        Qbert,
        Yar,
        Pac,
        MsPac,
        FBI
    }

    public enum UpgradeTypes {
        ForceField,
        SpreadShot,
        RearShot,
        SideShot,
        Bomb,
        Swirl,
        Laser,
        Surround,
        Invader,
        Ghost,
        // MechSuit,
        // SeekerMissile,
        // DonkeyKong,
        // Coily,
        // Robot,
        // Frygar
        RefillHP,
    }

    public enum BulletTypes {
        Standard,
        Swirl,
        Bomb
    }

    public static string[] UpgradeText = {
        "Force Field",
        "Spread Shot",
        "Rear Shot",
        "Side Shot",
        "Bomb",
        "Swirl",
        "Laser Beam",
        "Orbiting Drone",
        "Invader",
        "Ghost",
        // "Mech Suit",
        // "Seeker Missile",
        // "Donkey Kong",
        // "Coily",
        // "Berzerk Bot",
        // "Frygar"
        "Refill HP"
    };

    public static int MaxLevelsPerUpgrade = 5;

    public static string[] UpgradeDescriptionText = {
        "Creates an attack force field around the player.", "Increases force field by 10%.", "Increases force field by 10%.", "Increases force field by 10%.", "Increases force field by 10%.",
        "Converts normal gun into spread shot.", "Adds additional spread bullet.", "Adds additional spread bullet.", "Adds additional spread bullet.", "Adds additional spread bullet.",
        "Shoots bullets behind player.", "Adds additional rear bullet.", "Adds additional rear bullet.", "Adds additional rear bullet.", "Adds additional rear bullet.",
        "Shoots bullets to the sides of player.", "Adds additional side bullet.", "Adds additional side bullet.", "Adds additional side bullet.", "Adds additional side bullet.",
        "Launches bombs that create explosive shockwave.", "Adds additional bomb.", "Adds additional bomb.", "Adds additional bomb.", "Adds additional bomb.",
        "Launches powerful swirl attacks in random direction.", "Adds additional swirl.", "Adds additional swirl.", "Adds additional swirl.", "Adds additional swirl.",
        "Shoots laser beam from player's eye.", "Increases laser attack time by 25%.", "Increases laser attack time by 30%.", "Increases laser attack time by 30%.", "Increases laser attack time by 35%.",
        "Launches drone that orbits player.", "Increases drone orbit time by 10%.", "Adds additional drone.", "Increases drone orbit time by 10%.", "Adds additional drone.",
        "Drops attack invaders from top of screen.", "Increases number of enemies that invader passes through by 1.", "Adds additional invader.", "Increases number of enemies that invader passes through by 1.", "Adds additional invader.",
        "Emits ghosts that harm enemies in their path.", "Increases number of enemies that ghost passes through by 1.", "Adds additional ghost.", "Increases number of enemies that ghost passes through by 1.", "Adds additional ghost.",
        // "Equips mech suit that reduces enemy attack strength.",
        // "Launches missiles that attack nearest enemy.",
        // "Summons Donkey Kong to attack enemies.",
        // "Summons Coily to attack enemies.",
        // "Summons Berzerk Bot to attack enemies.",
        // "Summons Frygar to attack enemies."
        "Refill HP meter to maximum amount."
    };

    public static int[] UpgradeLevelBullets = {
        0,0,0,0,0,
        3,4,5,6,7,
        1,2,3,4,5,
        1,2,3,4,5,
        1,2,3,4,5,
        1,2,3,4,5,
        0,0,0,0,0,
        1,1,2,2,3,
        1,1,2,2,3,
        1,1,2,2,3,
        0,0,0,0,0,
        1,2,3,4,5,
        1,2,3,4,5,
        1,2,3,4,5,
        1,2,3,4,5,
        1,2,3,4,5,
    };
    public static float[] UpgradeLevelAttackTimes = {
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        .2f,.25f,.325f,.4225f,.57f,
        3f,3.3f,3.3f,3.63f,3.63f,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
    };
    public static float[] UpgradeLevelAttackSizes = {
        1f,1.1f,1.2f,1.33f,1.46f,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
    };
    public static int[] UpgradeLevelEnemyHits = {
        1,1,1,1,1,
        1,1,1,1,1,
        1,1,1,1,1,
        1,1,1,1,1,
        1,1,1,1,1,
        3,3,3,3,3,
        1,1,1,1,1,
        1,1,1,1,1,
        3,4,4,5,5,
        3,4,4,5,5,
        1,1,1,1,1,
        1,1,1,1,1,
        1,1,1,1,1,
        1,1,1,1,1,
        1,1,1,1,1,
        1,1,1,1,1,
    };

    public static int[] CurrentUpgradeLevels;
    public static int MaxUpgradeLevel = 5;

    public static int currentExp = 0;
    public static int currentLevel = 0;
    public static int[] maxExperiences;
    public static float[] healthPerLevel;
    public static int[] attackPerLevel;
    public static int[] defensePerLevel;
    public static float[] shootTimerDecreasePerLevel;

    public static float startMaxHealth = 20f;
    public static float currentAttack = 1f;
    public static float currentMaxHealth = 20f;
    public static float currentDefense = 1f;
    public static float currentShootTimerMax = 2f;
}
