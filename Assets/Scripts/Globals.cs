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
        SeekerMissile,
        Swirl,
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

    public static int[] CurrentUpgradeLevels;
}
