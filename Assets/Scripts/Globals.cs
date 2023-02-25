using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals
{
    public static bool DebugMode = true;

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
        // SeekerMissile,
        // Invader,
        // Ghost,
        // RobotSuit,
        // DonkeyKong,
        // Coily,
        // Robot,
        // Centipede
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
        "Seeker Missile",
        "Invader",
        "Ghost",
        "Mech Suit",
        "Stubborn Gorilla",
        "Snake",
        "Attack Bot",
        "Bug"
    };

    public static int[] CurrentUpgradeLevels;
    public static int MaxUpgradeLevel = 5;

    public static int currentExp = 0;
    public static int currentLevel = 0;
    public static int[] maxExperiences;
}
