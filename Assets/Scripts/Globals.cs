using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals
{
    public static bool DebugMode = false;

    // audio and music
    public static bool AudioOn = true;
    public static bool MusicOn = true;

    public static bool IsPaused = false;

    public enum PlayerTypes {
        Cinema,
        Atari2600,
        Ninja,
        Goth,
        Punk,
        Toxic,
        Super,
        RadStyle,
        Smurf
    }

    public static string[] PlayerNames = {
        "Movie E.T.",
        "2600 E.T.",
        "Ninja E.T.",
        "Goth E.T.",
        "Punk E.T.",
        "Toxic E.T.",
        "Super E.T.",
        "Rad Style E.T.",
        "Smurf E.T."
    };

    public static string[] AnimationSuffixes = {
        "",
        "-2600",
        "-ninja",
        "-goth",
        "-punk",
        "-toxic",
        "-super",
        "-rad",
        "-smurf"
    };

    public static PlayerTypes currentPlayerType = PlayerTypes.Cinema;

    public enum EnemyTypes {
        Yar,
        Pac,
        MsPac,
        Joust,
        Joust2,
        Frogger,
        Qbert,
        Kangaroo,
        Hero,
        Hero2,
        Dig,
        Plane,
        FBI,
        Scientist,
    }

    public static EnemyTypes[] FastEnemyTypes = {EnemyTypes.Yar, EnemyTypes.Pac, EnemyTypes.MsPac, EnemyTypes.Joust, EnemyTypes.Joust2};
    public static EnemyTypes[] StrongEnemyTypes = {EnemyTypes.Frogger, EnemyTypes.Qbert, EnemyTypes.Kangaroo, EnemyTypes.Hero, EnemyTypes.Hero2};

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
        Defense,
        Speed,
        SeekerMissile,
        Tornado,
        Bees,
        Boomerang,
        Pit,
        Attack,
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
        "Defense Boost",
        "Speed Boost",
        "Seeker Star",
        "Tornado",
        "Killer Bees",
        "Boomerang",
        "Pit Trap",
        "Attack Boost",
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
        "Increase player defense by 20%.", "Increase player defense by 20%.", "Increase player defense by 20%.", "Increase player defense by 20%.", "Increase player defense by 20%.",
        "Increase player speed by 10%.", "Increase player speed by 10%.", "Increase player speed by 10%.", "Increase player speed by 10%.", "Increase player speed by 10%.",
        "Launches seeker star that attack nearest enemy.", "Adds additional seeker star.", "Adds additional seeker star.", "Adds additional seeker star.", "Adds additional seeker star.",
        "Launch tornado that damages enemies.", "Increases tornado attack range by 10%.", "Increases tornado attack range by 10%.", "Increases tornado attack range by 10%.", "Increases tornado attack range by 10%.",
        "Summon killer bees from side of screen.", "Increase bee attack range by 20%.", "Increases number of enemies that bees pass through by 1.", "Adds second bee swarm.", "Increase bee attack range by 20%.",
        "Shoots boomerang to the side of player.", "Increases boomerang attack range by 20%.", "Increases boomerang attack range by 20%.", "Adds additional boomerang.", "Increases boomerang attack range by 20%.",
        "Place pit trap behind player.", "Increase pit trap attack time by 10%.", "Increase pit trap size by 25%.", "Increase pit trap attack time by 10%.", "Increase pit trap size by 25%.",
        "Increase player attack by 20%.", "Increase player attack by 20%.", "Increase player attack by 20%.", "Increase player attack by 20%.", "Increase player attack by 20%.",
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
        0,0,0,0,0,
        1,2,3,4,5,
        1,1,1,1,1,
        1,1,1,2,2,
        1,1,1,2,2,
        1,1,1,1,1,
        0,0,0,0,0,
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
        .5f, .55f, .6f, .6f, .7f,
        1.2f, 1.5f, 1.5f, 1.5f, 1.75f,
        .25f, .3f, .35f, .35f, .4f,
        1f, 1.1f, 1.1f, 1.2f, 1.2f,
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
        1f,1f,1.25f,1.25f,1.5f,
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
        0,0,0,0,0,
        0,0,0,0,0,
        1,1,1,1,1,
        6,6,6,6,6,
        3,3,4,4,4,
        1,1,1,1,1,
        1,1,1,1,1,
        0,0,0,0,0,
    };

    public static int[] CurrentUpgradeLevels;
    public static int MaxUpgradeLevel = 5;
    public static List<UpgradeTypes> CurrentUpgradeTypes = new List<Globals.UpgradeTypes>();

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
    public static int currrentNumEnemies = 0;
    public static int maxEnemies = 250;

    public static int candyCount;
    public static int killCount;
    public static float gameTime;

    public static void ResetGlobals()
    {
        Globals.maxExperiences = new int[] {100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1200, 1400, 1600, 1800, 2000};
        Globals.healthPerLevel  = new float[] {0, 2, 1, 1, 1, 2, 1, 1, 1, 2, 1, 1, 1, 2, 1, 1, 1 };
        Globals.attackPerLevel = new int[] {0, 10, 10, 10, 0, 10, 0, 10, 10, 10, 0, 10, 0, 10, 10, 10, 0, 10, 0, 10, 10, 10, 0, 10, 0, 10, 10, 10, 0, 10 };
        Globals.defensePerLevel = new int[] {0, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10 };
        Globals.shootTimerDecreasePerLevel = new float[] {0, 0, .1f, 0, 0, .1f, 0, 0, .1f, 0, 0, 0, .1f, 0, 0, 0, .1f };
        // the last upgrade slot is the HP refill, don't include that since it behaves uniquely
        int numUpgrades = System.Enum.GetValues(typeof(Globals.UpgradeTypes)).Length - 1;
        Globals.CurrentUpgradeLevels = new int[numUpgrades];
        for (int x = 0; x < Globals.CurrentUpgradeLevels.Length; x++)
        {
            Globals.CurrentUpgradeLevels[x] = 0;
        }
        // Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Tornado] = 5;
        // Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Bees] = 1;
        CurrentUpgradeTypes.Clear();

        currentExp = 0;
        currentLevel = 0;
        candyCount = 0;
        killCount = 0;
        gameTime = 0;
        startMaxHealth = 20f;
        currentAttack = 1f;
        currentMaxHealth = 20f;
        currentDefense = 1f;
        currentShootTimerMax = 2f;
        currrentNumEnemies = 0;
    }
}
