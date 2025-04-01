using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals
{
    // turn on to unlock all characters and powerups
    public static bool DebugMode = false;
    public static bool UnlockCharacters = false;
    public static bool Unlock6Weapons = false;
    // turn on to speed up enemy spawning
    public static float SpawnSpeedMultiplier = 1f;
    public static float DifficultySpeedMultiplier = 1f;

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
        New,
        Bubblegum,
        Electro,
        Hulk,
        Super,
        Pac,
        Sailor,
        Mario,
        Luigi,
        Koolaid,
        Smurf,
        RadStyle,
        Ninja,
        Crush,
        Grape,
        Bomber,
        Croc,
        Toxic,
        Ghost,
        Invader,
        Rain,
        Bees,
        Commando,
        Tron,
        Flash,
        Missile,
        Karate,
        Hulk2,
        Snake,
        Rambo,
        Macho,
    }

    public static string[] PlayerNames = {
        "Movie E.T.",
        "2600 E.T.",
        "Goth E.T.",
        "Miami E.T.",
        "Punk E.T.",
        "New Wave E.T.",
        "Bubblegum E.T.",
        "Electro E.T.",
        "Hulk E.T.",
        "Super E.T.",
        "Pac E.T.",
        "Sailor E.T.",
        "Mario E.T.",
        "Luigi E.T.",
        "Kool E.T.",
        "Smurf E.T.",
        "Rad Style E.T.",
        "Ninja E.T.",
        "Crush E.T.",
        "Grape Crush E.T.",
        "Mad Bomber E.T.",
        "Crocodile E.T.",
        "Toxic E.T.",
        "Ghost E.T.",
        "Invader E.T.",
        "Stormy Weather E.T.",
        "Honey Farmer E.T.",
        "Commando E.T.",
        "Light Cycle E.T.",
        "Flashy E.T.",
        "Missile Command E.T.",
        "Karate E.T.",
        "Other Hulk E.T.",
        "Snake Commander E.T.",
        "Grizzled War Vet E.T.",
        "Macho E.T.",
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
        "Defeat Giant Pac-Man",
        "Defeat Popeye",
        "Defeat Mario",
        "Defeat Luigi",
        "Defeat Kool-aid Man",
        "Upgrade breakout ball to level 5",
        "Upgrade laser to level 5",
        "Upgrade seeker star to level 5",
        "Upgrade swirl to level 5",
        "Upgrade orbiting drone to level 5",
        "Upgrade bomb to level 5",
        "Upgrade boomerang to level 5",
        "Upgrade slime to level 5",
        "Upgrade ghost to level 5",
        "Upgrade invader to level 5",
        "Upgrade tornado to level 5",
        "Upgrade killer bees to level 5",
        "Upgrade spread shot to level 5",
        "Upgrade force field to level 5",
        "Upgrade speed to level 5",
        "Upgrade ICBM to level 5",
        "Upgrade pit trap to level 5",
        "Upgrade defense to level 5",
        "Upgrade rear shot to level 5",
        "Upgrade side shot to level 5",
        "Upgrade attack to level 5",
    };

    public static string[] AnimationSuffixes = {
        "",
        "-2600",
        "-goth",
        "-miami",
        "-punk",
        "-new",
        "-gum",
        "-electro",
        "-hulk",
        "-super",
        "-pac",
        "-sailor",
        "-mario",
        "-luigi",
        "-kool",
        "-smurf",
        "-rad",
        "-ninja",
        "-crush",
        "-grape",
        "-bomber",
        "-croc",
        "-toxic",
        "-ghost",
        "-invader",
        "-rain",
        "-bee",
        "-commando",
        "-tron",
        "-flash",
        "-missile",
        "-karate",
        "-hulk2",
        "-snake",
        "-rambo",
        "-macho",
    };

    public static PlayerTypes currentPlayerType = PlayerTypes.Cinema;
    public static int MaxPlayerTypes = 36;
    public static int[] CharacterUnlockStates = new int[MaxPlayerTypes];

    public enum EnemyTypes {
        Yar,
        Pac,
        MsPac,
        Yar2,
        JrPac,

        Qbert,
        Kangaroo,
        Hero,
        Pengo,
        Hero2,

        Frogger,
        Joust,
        Bear,
        Joust2,

        Indy,
        Jungle,
        Harry,

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

    public static EnemyTypes[] FastEnemyTypes = {EnemyTypes.Yar, EnemyTypes.Pac, EnemyTypes.MsPac, EnemyTypes.Yar2, EnemyTypes.JrPac};
    public static EnemyTypes[] StrongEnemyTypes = {EnemyTypes.Qbert, EnemyTypes.Kangaroo, EnemyTypes.Hero, EnemyTypes.Bear, EnemyTypes.Hero2};
    public static EnemyTypes[] SurroundEnemyTypes = {EnemyTypes.Frogger, EnemyTypes.Joust, EnemyTypes.Pengo, EnemyTypes.Joust2};
    public static EnemyTypes[] ChaoticEnemyTypes = {EnemyTypes.Indy, EnemyTypes.Jungle, EnemyTypes.Harry};

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
        ICBM,
        Breakout,
        Slime,
        RefillHP,
    }

    public enum BulletTypes {
        Standard,
        Swirl,
        Bomb,
        Slime,
        Breakout
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
        "ICBM",
        "Breakout Ball",
        "TNT Slime",
        "Refill HP"
    };

    public static int MaxLevelsPerUpgrade = 5;

    public static string[] UpgradeDescriptionText = {
        "Create an attack force field around the player.", "Increase force field by 10 percent.", "Increase force field by 10 percent.", "Increase force field by 10 percent.", "Increase force field by 10 percent.",
        "Convert normal gun into spread shot.", "Add additional round of spread shot.", "Add additional round of spread shot.", "Add additional round of spread shot.", "Add additional round of spread shot.",
        "Shoot bullets behind player.", "Add additional round of rear bullet.", "Add additional round of rear bullet.", "Add additional round of rear bullet.", "Add additional round of rear bullet.",
        "Shoot bullets to the sides of player.", "Add additional round of side bullet.", "Add additional round of side bullet.", "Add additional round of side bullet.", "Add additional round of side bullet.",
        "Launch bomb that create explosive shockwave.", "Add additional bomb.", "Add additional bomb.", "Add additional bomb.", "Add additional bomb.",
        "Launch swirl attack in random direction.", "Add additional swirl.", "Add additional swirl.", "Add additional swirl.", "Add additional swirl.",
        "Shoot laser beam from player's eye.", "Increase laser attack time by 25 percent.", "Increase laser attack time by 30 percent.", "Increase laser attack time by 30 percent.", "Increase laser attack time by 35 percent.",
        "Launch drone that orbits player.", "Increase drone orbit time by 10 percent.", "Add additional drone.", "Increase drone orbit time by 10 percent.", "Add additional drone.",
        "Drop attack invaders from top of screen.", "Increase number of enemies invader passes through by 1.", "Add additional invader.", "Increase number of enemies invader passes through by 1.", "Add additional invader.",
        "Emit ghosts that harm enemies in their path.", "Increase number of enemies ghost passes through by 1.", "Add additional ghost.", "Increase number of enemies ghost passes through by 1.", "Add additional ghost.",
        "Increase player speed by 5 percent.", "Increase player speed by 5 percent.", "Increase player speed by 5 percent.", "Increase player speed by 5 percent.", "Increase player speed by 5 percent.",
        "Launch seeker star that attacks nearest enemy.", "Add additional seeker star.", "Add additional seeker star.", "Add additional seeker star.", "Add additional seeker star.",
        "Summon tornado from right side of screen", "Increase number of enemies tornado passes through by 1.", "Add additional tornado.", "Increase number of enemies tornado passes through by 1.", "Add additional tornado.",
        "Summon killer bees from left side of screen.", "Increase number of enemies bees pass through by 1.", "Add additional bee swarm.", "Increase number of enemies bees pass through by 1.", "Add additional bee swarm.",
        "Shoot boomerang to the side of player.", "Increase boomerang attack range by 20 percent.", "Increase boomerang attack range by 20 percent.", "Add additional boomerang.", "Increase boomerang attack range by 20 percent.",
        "Place pit trap behind player.", "Increase pit trap attack time by 20 percent.", "Add additional pit.", "Increase pit trap attack time by 20 percent.", "Add additional pit.",
        "Increase player defense by 1 point.", "Increase player defense by 1 point.", "Increase player defense by 1 point.", "Increase player defense by 1 point.", "Increase player defense by 1 point.",
        "Increase player attack by 1 point.", "Increase player attack by 1 point.", "Increase player attack by 1 point.", "Increase player attack by 1 point.", "Increase player attack by 1 point.",
        "Launch ICBMs from bottom of screen.", "Increase number of enemies ICBMs pass through by 1.", "Add additional ICBM.", "Increase number of enemies ICBMs pass through by 1.", "Add additional ICBM.",
        "Shoot ricocheting breakout ball.", "Increase number of ricochets by 2", "Add additional breakout ball.", "Increase number of ricochets by 3", "Add additional breakout ball.",
        "Shoot exploding slimes in random direction.", "Increase slime shrapnel by 50 percent.", "Add additional exploding slime.", "Increase slime shrapnel by 50 percent.", "Add additional exploding slime.",
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
        1,1,2,2,3,
        1,1,2,2,3,
        1,1,1,2,2,
        1,1,2,2,3,
        0,0,0,0,0,
        0,0,0,0,0,
        1,1,2,2,3,
        1,1,2,2,3,
        1,1,2,2,3,
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
        .25f, .3f, .35f, .35f, .4f,
        1f, 1.2f, 1.2f, 1.4f, 1.4f,
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
        1.5f,1.5f,1.5f,1.5f,1.5f,
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
        0,0,0,0,0,
        1,1,1,1,1,
        3,4,4,5,5,
        3,4,4,5,5,
        1,1,1,1,1,
        1,1,1,1,1,
        0,0,0,0,0,
        0,0,0,0,0,
        3,4,4,5,5,
        5,7,7,10,10,
        1,1,1,1,1,
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
    public static int currentDefense = 0;
    public static float maxAttack = 10f;
    public static float maxDefense = 10f;
    public static int currrentNumEnemies = 0;
    public static int maxEnemies = 200;

    public static List<Vector2> surroundOffsets = new List<Vector2>();
    public static int surroundIndex = 0;

    public static int candyCount;
    public static int killCount;
    public static float gameTime;

    public static int GamesPlayed;
    public static int BestTime;
    public static List<PlayerTypes> UnlockedCharacters = new List<PlayerTypes>();

    public const string PlayerTypeUnlockPlayerPrefsKey = "PlayerType";
    public const string GamesPlayedUnlockPlayerPrefsKey = "GamedPlayed";
    public const string BestTimePlayerPrefsKey = "BestTime";

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
        BestTime = LoadIntFromPlayerPrefs(BestTimePlayerPrefsKey);
    }

    public static void UpdateGamesPlayed(int newVal)
    {
        GamesPlayed = newVal;
        SaveIntToPlayerPrefs(GamesPlayedUnlockPlayerPrefsKey, GamesPlayed);
    }

    public static void UpdateBestTime(int newVal)
    {
        if (BestTime < newVal)
            BestTime = newVal;
        SaveIntToPlayerPrefs(BestTimePlayerPrefsKey, BestTime);
    }

    public static void LoadCharacterUnlockStatesFromPlayerPrefs()
    {
        for (int x = 0; x < MaxPlayerTypes; x++)
        {
            int unlock = LoadIntFromPlayerPrefs(PlayerTypeUnlockPlayerPrefsKey + x.ToString());
            CharacterUnlockStates[x] = unlock;
        }
        CharacterUnlockStates[0] = 1;

        if (Globals.UnlockCharacters) {
            CharacterUnlockStates[(int)PlayerTypes.Atari2600] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Goth] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Miami] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Punk] = 1;
            CharacterUnlockStates[(int)PlayerTypes.New] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Bubblegum] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Electro] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Hulk] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Super] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Pac] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Sailor] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Mario] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Luigi] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Koolaid] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Smurf] = 1;
            CharacterUnlockStates[(int)PlayerTypes.RadStyle] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Ninja] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Crush] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Grape] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Bomber] = 1;

            CharacterUnlockStates[(int)PlayerTypes.Croc] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Toxic] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Ghost] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Invader] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Rain] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Bees] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Commando] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Tron] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Flash] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Missile] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Karate] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Hulk2] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Snake] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Rambo] = 1;
            CharacterUnlockStates[(int)PlayerTypes.Macho] = 1;
        }
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
        Globals.healthPerLevel  = new float[] {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        Globals.attackPerLevel = new int[] {0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 };
        Globals.defensePerLevel = new int[] {0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 };
        // the last upgrade slot is the HP refill, don't include that since it behaves uniquely
        int numUpgrades = System.Enum.GetValues(typeof(Globals.UpgradeTypes)).Length - 1;
        Globals.CurrentUpgradeLevels = new int[numUpgrades];
        for (int x = 0; x < Globals.CurrentUpgradeLevels.Length; x++)
        {
            Globals.CurrentUpgradeLevels[x] = 0;
        }

        if (Globals.Unlock6Weapons) {
            Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Surround] = 5;
            Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.ForceField] = 5;
            Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Tornado] = 5;
            Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Bees] = 5;
            Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Invader] = 5;
            Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.SeekerMissile] = 5;
        }

        CurrentUpgradeTypes.Clear();

        int numOffsets = 15;
        surroundOffsets.Clear();
        surroundIndex = 0;
        for (int x = 0; x < numOffsets; x++)
        {
            Vector2 radialVector = Quaternion.Euler(0, 0, x * 24f) * new Vector2(4f, 4f);
            surroundOffsets.Add(radialVector);
        }
        for (int x = 0; x < numOffsets; x++)
        {
            int index = Random.Range(0, surroundOffsets.Count);
            Vector2 swapVector = surroundOffsets[x];
            surroundOffsets[x] = surroundOffsets[index];
            surroundOffsets[index] = swapVector;
        }

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
