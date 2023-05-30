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
        Goth,
        Miami,
        Punk,
        Toxic,
        Bubblegum,
        Smurf,
        Hulk,
        Super,
        RadStyle,
        Ninja,
        Crush,
        Grape,
        New,
        Pac,
        Sailor,
        Mario,
        Luigi,
        Koolaid
    }

    public static string[] PlayerNames = {
        "Movie E.T.",
        "2600 E.T.",
        "Goth E.T.",
        "Miami E.T.",
        "Punk E.T.",
        "Toxic E.T.",
        "Bubblegum E.T.",
        "Smurf E.T.",
        "Hulk E.T.",
        "Super E.T.",
        "Rad Style E.T.",
        "Ninja E.T.",
        "Crush E.T.",
        "Grape Crush E.T.",
        "New Wave E.T.",
        "Pac E.T.",
        "Sailor E.T.",
        "Mario E.T.",
        "Luigi E.T.",
        "Kool E.T.",
    };

    public static string[] PlayerUnlockTexts = {
        "",
        "Complete 1 mission",
        "Complete 2 missions",
        "Complete 5 missions",
        "Complete 10 missions",
        "Complete 20 missions",
        "Complete 30 missions",
        "Complete 50 missions",
        "Survive for 10 minutes",
        "Survive for 20 minutes",
        "Upgrade laser to level 5",
        "Upgrade seeker star to level 5",
        "Upgrade swirl to level 5",
        "Upgrade pit trap to level 5",
        "Upgrade bomb to level 5",
        "Defeat Giant Pac-Man",
        "Defeat Popeye",
        "Defeat Mario",
        "Defeat Luigi",
        "Defeat Kool-aid Man",
    };

    public static string[] AnimationSuffixes = {
        "",
        "-2600",
        "-goth",
        "-miami",
        "-punk",
        "-toxic",
        "-gum",
        "-smurf",
        "-hulk",
        "-super",
        "-rad",
        "-ninja",
        "-crush",
        "-grape",
        "-new",
        "-pac",
        "-sailor",
        "-mario",
        "-luigi",
        "-kool",
    };

    public static PlayerTypes currentPlayerType = PlayerTypes.Cinema;
    public static int MaxPlayerTypes = 20;
    public static int[] CharacterUnlockStates = new int[MaxPlayerTypes];

    public enum EnemyTypes {
        Yar,
        Pac,
        MsPac,
        Joust,
        Joust2,
        Yar2,

        Frogger,
        Indy,
        Pengo,

        Qbert,
        Kangaroo,
        Bear,
        Hero,
        Hero2,

        Dig,
        Plane,
        Moon,
        FBI,
        Scientist,

        PacBoss,
        PopeyeBoss,
        MarioBoss,
        LuigiBoss,
        KoolBoss,
        HarryBoss,
    }

    public static string[] BossText = {
        "Waka Waka Waka Waka!",
        "I yam what I yam!",
        "It's a me!",
        "Mamma Mia!",
        "Oh Yeah!",
        "Welcome to the jungle!",
    };
    public static string[] BossNames = {
        "Mr. Pac-Man",
        "Popeye",
        "Mario",
        "Luigi",
        "Koolaid Man",
        "Harry",
    };

    public static EnemyTypes[] FastEnemyTypes = {EnemyTypes.Yar, EnemyTypes.Pac, EnemyTypes.MsPac, EnemyTypes.Bear, EnemyTypes.Joust, EnemyTypes.Joust2, EnemyTypes.Yar2};
    public static EnemyTypes[] StrongEnemyTypes = {EnemyTypes.Qbert, EnemyTypes.Kangaroo, EnemyTypes.Bear, EnemyTypes.Hero, EnemyTypes.Hero2};
    public static EnemyTypes[] SurroundEnemyTypes = {EnemyTypes.Frogger, EnemyTypes.Indy, EnemyTypes.Pengo};

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
        Speed,
        SeekerMissile,
        Tornado,
        Bees,
        Boomerang,
        Pit,
        Defense,
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
        "Speed Boost",
        "Seeker Star",
        "Tornado",
        "Killer Bees",
        "Boomerang",
        "Pit Trap",
        "Defense Boost",
        "Attack Boost",
        // "Donkey Kong",
        // "Coily",
        // "Berzerk Bot",
        // "Frygar"
        "Refill HP"
    };

    public static int MaxLevelsPerUpgrade = 5;

    public static string[] UpgradeDescriptionText = {
        "Create an attack force field around the player.", "Increase force field by 10%.", "Increase force field by 10%.", "Increase force field by 10%.", "Increase force field by 10%.",
        "Convert normal gun into spread shot.", "Add additional round of spread shot.", "Add additional round of spread shot.", "Add additional round of spread shot.", "Add additional round of spread shot.",
        "Shoot bullets behind player.", "Add additional round of rear bullet.", "Add additional round of rear bullet.", "Add additional round of rear bullet.", "Add additional round of rear bullet.",
        "Shoot bullets to the sides of player.", "Add additional round of side bullet.", "Add additional round of side bullet.", "Add additional round of side bullet.", "Add additional round of side bullet.",
        "Launch bomb that create explosive shockwave.", "Add additional bomb.", "Add additional bomb.", "Add additional bomb.", "Add additional bomb.",
        "Launch powerful swirl attack in random direction.", "Add additional swirl.", "Add additional swirl.", "Add additional swirl.", "Add additional swirl.",
        "Shoot laser beam from player's eye.", "Increase laser attack time by 25%.", "Increase laser attack time by 30%.", "Increase laser attack time by 30%.", "Increase laser attack time by 35%.",
        "Launch drone that orbits player.", "Increase drone orbit time by 10%.", "Add additional drone.", "Increase drone orbit time by 10%.", "Add additional drone.",
        "Drop attack invaders from top of screen.", "Increase number of enemies that invader passes through by 1.", "Add additional invader.", "Increase number of enemies that invader passes through by 1.", "Add additional invader.",
        "Emit ghosts that harm enemies in their path.", "Increase number of enemies that ghost passes through by 1.", "Add additional ghost.", "Increase number of enemies that ghost passes through by 1.", "Add additional ghost.",
        "Increase player speed by 10%.", "Increase player speed by 10%.", "Increase player speed by 10%.", "Increase player speed by 10%.", "Increase player speed by 10%.",
        "Launch seeker star that attacks nearest enemy.", "Add additional seeker star.", "Add additional seeker star.", "Add additional seeker star.", "Add additional seeker star.",
        "Launch tornado that damages enemies.", "Increase tornado attack range by 10%.", "Increase tornado attack range by 10%.", "Increase tornado attack range by 10%.", "Increase tornado attack range by 10%.",
        "Summon killer bees from side of screen.", "Increase bee attack range by 20%.", "Increase number of enemies that bees pass through by 1.", "Add second bee swarm.", "Increase bee attack range by 20%.",
        "Shoot boomerang to the side of player.", "Increase boomerang attack range by 20%.", "Increase boomerang attack range by 20%.", "Add additional boomerang.", "Increase boomerang attack range by 20%.",
        "Place pit trap behind player.", "Increase pit trap attack time by 10%.", "Increase pit trap size by 25%.", "Increase pit trap attack time by 10%.", "Increase pit trap size by 25%.",
        "Increase player defense by 1 point.", "Increase player defense by 1 point.", "Increase player defense by 1 point.", "Increase player defense by 1 point.", "Increase player defense by 1 point.",
        "Increase player attack by 1 point.", "Increase player attack by 1 point.", "Increase player attack by 1 point.", "Increase player attack by 1 point.", "Increase player attack by 1 point.",
        // "Summons Donkey Kong to attack enemies.",
        // "Summons Coily to attack enemies.",
        // "Summons Berzerk Bot to attack enemies.",
        // "Summons Frygar to attack enemies."
        "Refill HP meter to maximum amount."
    };

    public static int[] UpgradeLevelBullets = {
        0,0,0,0,0,
        1,2,3,4,5,
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
        1,1,1,1,1,
        1,1,1,2,2,
        1,1,1,2,2,
        1,1,1,1,1,
        0,0,0,0,0,
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
        .5f, .55f, .6f, .6f, .7f,
        1.2f, 1.5f, 1.5f, 1.5f, 1.75f,
        .25f, .3f, .35f, .35f, .4f,
        1f, 1.1f, 1.1f, 1.2f, 1.2f,
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
        1f,1f,1.25f,1.25f,1.5f,
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
        0,0,0,0,0,
        1,1,1,1,1,
        6,6,6,6,6,
        3,3,4,4,4,
        1,1,1,1,1,
        1,1,1,1,1,
        0,0,0,0,0,
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

    public static float startMaxHealth = 20f;
    public static int currentAttack = 0;
    public static float currentMaxHealth = 20f;
    public static int currentDefense = 1;
    public static float maxAttack = 10f;
    public static float maxDefense = 10f;
    public static int currrentNumEnemies = 0;
    public static int maxEnemies = 200;

    public static int candyCount;
    public static int killCount;
    public static float gameTime;

    public static int GamesPlayed;
    public static List<PlayerTypes> UnlockedCharacters = new List<PlayerTypes>();

    public const string PlayerTypeUnlockPlayerPrefsKey = "PlayerType";
    public const string GamesPlayedUnlockPlayerPrefsKey = "GamedPlayed";

    public static void ResetUnlockedCharacterList()
    {
        UnlockedCharacters.Clear();
    }
    public static void AddUnlockedCharacterToList(PlayerTypes character)
    {
        UnlockedCharacters.Add(character);
    }
    public static void UpdateUnlockedCharactersFromList()
    {
        foreach(Globals.PlayerTypes playerType in Globals.UnlockedCharacters)
        {
            UnlockCharacter((int)playerType);
        }
    }

    public static void LoadGameStateFromPlayerPrefs()
    {
        LoadCharacterUnlockStatesFromPlayerPrefs();
        GamesPlayed = LoadIntFromPlayerPrefs(GamesPlayedUnlockPlayerPrefsKey);
    }

    public static void UpdateGamesPlayed(int newVal)
    {
        GamesPlayed = newVal;
        SaveIntToPlayerPrefs(GamesPlayedUnlockPlayerPrefsKey, GamesPlayed);
    }

    public static void LoadCharacterUnlockStatesFromPlayerPrefs()
    {
        for (int x = 0; x < MaxPlayerTypes; x++)
        {
            int unlock = LoadIntFromPlayerPrefs(PlayerTypeUnlockPlayerPrefsKey + x.ToString());
            CharacterUnlockStates[x] = unlock;
        }
        CharacterUnlockStates[0] = 1;
    }

    public static void UnlockCharacter(int playerTypeNum)
    {
        CharacterUnlockStates[playerTypeNum] = 1;
        SaveIntToPlayerPrefs(PlayerTypeUnlockPlayerPrefsKey + playerTypeNum.ToString(), 1);
    }

    public static void SaveIntToPlayerPrefs(string key, int val)
    {
        PlayerPrefs.SetInt(key, val);
    }
    public static int LoadIntFromPlayerPrefs(string key, int defaultVal = 0)
    {
        int val = PlayerPrefs.GetInt(key, defaultVal);
        return val;
    }

    public static void SaveFloatToPlayerPrefs(string key, float val)
    {
        PlayerPrefs.SetFloat(key, val);
    }
    public static float LoadFloatFromPlayerPrefs(string key)
    {
        float val = PlayerPrefs.GetFloat(key, 0f);
        return val;
    }

    public static void ResetGlobals()
    {
        Globals.maxExperiences = new int[] {100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1200, 1400, 1600, 1800, 2000};
        Globals.healthPerLevel  = new float[] {0, 2, 1, 1, 1, 2, 1, 1, 1, 2, 1, 1, 1, 2, 1, 1, 1 };
        Globals.attackPerLevel = new int[] {0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 };
        Globals.defensePerLevel = new int[] {0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 };
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
        currentAttack = 0;
        currentMaxHealth = 20f;
        currentDefense = 0;
        currrentNumEnemies = 0;
    }

}
