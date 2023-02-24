using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals
{
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
        RearShot,
        SideShot,
        TripleShot,
        Bomb,
        Swirl,
        SeekerMissile,
        Invader,
        Ghost,
        Laser,
        RobotSuit,
        DonkeyKong,
        Coily,
        Robot,
        Centipede
    }

    public enum BulletTypes {
        Standard,
        Swirl,
        Bomb
    }

    public static string[] UpgradeText = {
        "Force Field",
        "Rear Shot",
        "Side Shot",
        "Spread Shot",
        "Kabomb",
        "Swirl",
        "Seeker Missile",
        "Invader",
        "Ghost",
        "Laser Beam",
        "Mech Suit",
        "Stubborn Gorilla",
        "Snake",
        "Attack Bot",
        "Bug"
    };

    public static int[] CurrentUpgradeLevels;
    public static int MaxUpgradeLevel = 5;
}
