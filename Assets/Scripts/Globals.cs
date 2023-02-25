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
        TripleShot,
        RearShot,
        SideShot,
        Bomb,
        Swirl,
        Laser,
        Surround,
        Invader,
        Ghost,
        MechSuit,
        // SeekerMissile,
        // DonkeyKong,
        // Coily,
        // Robot,
        // Frygar
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
        "Surround",
        "Invader",
        "Ghost",
        "Mech Suit",
        "Seeker Missile",
        "Donkey Kong",
        "Coily",
        "Berzerk Bot",
        "Frygar"
    };

    public static string[] UpgradeDescriptionText = {
        "Creates an attack force field around the player.",
        "Converts normal gun into spread shot.",
        "Shoots bullets behind player.",
        "Shoots bullets to the sides of player.",
        "Launches bombs that create explosive shockwave.",
        "Launch powerful swirl attacks in random direction.",
        "Shoot laser beam from player's eye.",
        "Drop attack invaders from top of screen.",
        "Emit ghosts that harm enemies in their path",
        "Equips mech suit that reduces enemy attack strength",
        "Launch missiles that attack nearest enemy.",
        "Summons Donkey Kong to attack enemies.",
        "Summons Coily to attack enemies.",
        "Summons Berzerk Bot to attack enemies.",
        "Summons Frygar to attack enemies."
    };

    public static int[] CurrentUpgradeLevels;
    public static int MaxUpgradeLevel = 5;

    public static int currentExp = 0;
    public static int currentLevel = 0;
    public static int[] maxExperiences;

    public static float startMaxHealth = 10f;
    public static int currentAttack = 1;
    public static float currentMaxHealth = 10f;
    public static int currentDefense = 0;
    public static float currentShootTimerMax = 2f;
}
