using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSceneManager : MonoBehaviour
{
    AudioManager audioManager;

    [SerializeField]
    GameObject AudioManagerPrefab;

    [SerializeField]
    FadeManager fadeManager;

    [SerializeField]
    GameObject Player;
    Player playerScript;

    [SerializeField]
    RectTransform ExpBar;
    [SerializeField]
    TextMeshProUGUI ExpLevel;
    [SerializeField]
    GameObject LevelUpPanel;
    [SerializeField]
    TextMeshProUGUI LevelUpStats;
    float levelUpTimer = 0;
    float levelUpTimerMax = 3f;
    [SerializeField]
    GameObject[] HUDUpgradeDisplays;
    [SerializeField]
    Sprite[] UpgradeSprites;

    [SerializeField]
    GameObject HUDUpgradePanel;
    [SerializeField]
    GameObject[] HUDUpgradeButtonHighlights;
    [SerializeField]
    TextMeshProUGUI[] HUDUpgradeButtonTitleTexts;
    [SerializeField]
    TextMeshProUGUI[] HUDUpgradeButtonLvlTexts;
    [SerializeField]
    TextMeshProUGUI[] HUDUpgradeButtonDescTexts;
    [SerializeField]
    Image[] HUDUpgradeButtonIcons;
    List<Globals.UpgradeTypes> availableUpgrades = new List<Globals.UpgradeTypes>();
    int upgradeHighlightIndex = 0;
    bool stickDown = false;
    bool controllerAttached = false;

    int difficultyLevel = 0;
    float difficultyTimer = 60f;
    float difficultyTimerMax = 60f;
    float specialAttackTimer = 90f;
    float specialAttackTimerMax = 60f;

    // CANDY
    [SerializeField]
    GameObject ItemContainer;
    Candy[] candyPool = new Candy[100];
    [SerializeField]
    GameObject CandyPrefab;
    Phone[] phonePool = new Phone[10];
    [SerializeField]
    GameObject PhonePrefab;

    // ENEMIES
    [SerializeField]
    GameObject  HitNoticeContainer;
    [SerializeField]
    GameObject HitNoticePrefab;
    HitNotice[] hitNotice = new HitNotice[40];
    Enemy[] enemyPool = new Enemy[200];
    Enemy[] enemyDigPool = new Enemy[45];
    Enemy[] enemyFBIPool = new Enemy[3];
    Enemy[] enemyScientistPool = new Enemy[1];
    Enemy[] enemyVehiclePool = new Enemy[60];
    Enemy[] enemyBossPool = new Enemy[1];
    [SerializeField]
    GameObject EnemyPrefab;
    [SerializeField]
    GameObject EnemyContainer;

    [SerializeField]
    Transform topTanksTransform;
    [SerializeField]
    Transform bottomTanksTransform;
    [SerializeField]
    Transform leftTanksTransform;
    [SerializeField]
    Transform rightTanksTransform;

    enum EnemySpecialAttackPatterns {
        VerticalMove,
        HorizontalMove,
        Digs,
        Planes,
        Rovers,
        None
    }
    EnemySpecialAttackPatterns currentEnemySpecialAttack = EnemySpecialAttackPatterns.None;

    // D: 00, 01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12
    // T:  0,  1,  2,  3,  4,  5,  6,  7,  8,  9, 10, 11, 12

    //1R: 80, 60, 40, 20, 10, 10, 10, 10, 10, 10, 10, 10, 10
    //2R: 20, 40, 60, 80, 60, 40, 20, 10, 10, 10, 10, 10, 10
    //3R   0,  0,  0, 10, 20, 40, 60, 80, 60, 40, 20, 20, 20
    //4R   0,  0,  0,  0,  0,  0, 10, 20, 40, 60, 80, 80, 80,

    int[] enemyFastOneSpawnRates = {100, 80, 60, 40, 20, 10, 10, 10, 10, 10, 10, 10, 10, 0};
    int[] enemyFastTwoSpawnRates =   {0, 20, 40, 60, 80, 60, 40, 20, 10, 10, 10, 10, 10, 10, 10, 10, 0};
    int[] enemyFastThreeSpawnRates = {0,  0,  0,  0, 10, 20, 40, 60, 80, 60, 40, 20, 20, 20, 20, 20, 20};
    int[] enemyFastFourSpawnRates =  {0,  0,  0,  0,  0,  0,  0, 10, 20, 40, 60, 80, 60, 40, 30};
    int[] enemyFastFiveSpawnRates =  {0,  0,  0,  0,  0,  0,  0,  0,  0,  0, 10, 20, 40, 60, 80, 60, 40};
    int[] enemyFastSixSpawnRates =   {0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, 10, 20, 40, 60, 80, 60};
    int[] enemyFastSevenSpawnRates = {0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, 10, 20, 40, 60, 80};

    int[] enemyStrongOneSpawnRates = {100, 100, 80, 60, 40, 20, 20, 10, 10, 10, 10, 10, 10, 10, 10, 0};
    int[] enemyStrongTwoSpawnRates =   {0,  0,  20, 40, 60, 80, 80, 60, 40, 20, 10, 10, 10, 10, 10, 10, 10, 10, 0};
    int[] enemyStrongThreeSpawnRates = {0,  0,  0,  0,  0,  10, 20, 40, 60, 80, 80, 60, 40, 20, 10};
    int[] enemyStrongFourSpawnRates =  {0,  0,  0,  0,  0,  0,  0,  0,  10, 20, 40, 60, 80, 80, 60, 40};
    int[] enemyStrongFiveSpawnRates =  {0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  10, 20, 40, 60, 80};

    int[] enemySurroundOneSpawnRates = {100, 100, 100, 80, 60, 40, 20, 20, 10};
    int[] enemySurroundTwoSpawnRates =   {0,   0,   0, 20, 40, 60, 80, 60, 60, 40, 30};
    int[] enemySurroundThreeSpawnRates = {0,   0,   0,  0,  0,  0,  0, 0,  10, 20, 40, 60, 80};

    int[] fastEnemySpawnRates = { 100, 100, 100, 100, 100, 100, 100 };
    int[] strongEnemySpawnRates = { 100, 100, 100, 100, 100 };
    int[] surroundEnemySpawnRates = { 100, 100, 100 };
    int currentFastEnemyMaxSpawn = 100;
    int currentStrongEnemyMaxSpawn = 100;
    int currentSurroundEnemyMaxSpawn = 100;
    int numSpawns = 0;

    float spawnTimer = 5f;
    float spawnTimerMax = 7f;
    int digSpawnsRemaining = 0;
    int planeSpawnsRemaining = 0;
    int roverSpawnsRemaining = 0;
    float tankReturnTimer = 0;

    [SerializeField]
    GameObject HUDBossPanel;
    [SerializeField]
    Image HUDBossImage;
    [SerializeField]
    TextMeshProUGUI HUDBossNameText;
    [SerializeField]
    TextMeshProUGUI HUDBossText;
    [SerializeField]
    TypewriterUI HUDBossTextType;
    [SerializeField]
    Sprite[] BossSprites;
    Globals.EnemyTypes currentBossType = Globals.EnemyTypes.PacBoss;
    enum BossStates {
        None,
        ShowHUD,
        ShowText,
        ShowBoss,
    }
    BossStates currentBossState = BossStates.None;
    float bossTimer = 0;
    bool gameHasBoss = false;
    bool bossDefeated = false;

    float deadTimer = 0f;
    float deadTimerMax = 4f;
    bool fadeIn = false;
    bool fadeOut = false;

    void Awake()
    {
        Application.targetFrameRate = 60;
        GameObject am = GameObject.Find("AudioManager");
        if (am)
            audioManager = am.GetComponent<AudioManager>();
        else
        {
            GameObject ami = Instantiate(AudioManagerPrefab);
            ami.name = "AudioManager";
            audioManager = ami.GetComponent<AudioManager>();
        }
        CreateCandyPool();
        CreatePhonePool();
        CreateEnemyPool();
        CreateHitNoticePool();

        fadeManager.StartFadeIn();
        fadeIn = true;
    }

    void CreateCandyPool()
    {
        for (int x = 0; x < candyPool.Length; x++)
        {
            GameObject candyGO = Instantiate(CandyPrefab, this.transform.localPosition, Quaternion.identity, ItemContainer.transform);
            candyPool[x] = candyGO.GetComponent<Candy>();
        }
    }
    public void ActivateCandyFromPool(Vector3 candyPos)
    {
        for (int x = 0; x < candyPool.Length; x++)
        {
            if (!candyPool[x].IsActive())
            {
                candyPool[x].Activate(candyPos);
                break;
            }
        }
    }
    void CreatePhonePool()
    {
        for (int x = 0; x < phonePool.Length; x++)
        {
            GameObject phoneGO = Instantiate(PhonePrefab, this.transform.localPosition, Quaternion.identity, ItemContainer.transform);
            phonePool[x] = phoneGO.GetComponent<Phone>();
        }
    }
    public void ActivatePhoneFromPool(Vector3 phonePos)
    {
        for (int x = 0; x < phonePool.Length; x++)
        {
            if (!phonePool[x].IsActive())
            {
                phonePool[x].Activate(phonePos);
                break;
            }
        }
    }
    void CreateHitNoticePool()
    {
        for (int x = 0; x < hitNotice.Length; x++)
        {
            GameObject go = Instantiate(HitNoticePrefab, this.transform.localPosition, Quaternion.identity, HitNoticeContainer.transform);
            hitNotice[x] = go.GetComponent<HitNotice>();
        }
    }
    public void ActivateHitNoticeFromPool(Vector3 pos, int damage)
    {
        for (int x = 0; x < hitNotice.Length; x++)
        {
            if (!hitNotice[x].IsActive())
            {
                hitNotice[x].Activate(pos, damage);
                break;
            }
        }
    }
    void CreateEnemyPool()
    {
        for (int x = 0; x < enemyPool.Length; x++)
        {
            GameObject enemyGO = Instantiate(EnemyPrefab, this.transform.localPosition, Quaternion.identity, EnemyContainer.transform);
            enemyPool[x] = enemyGO.GetComponent<Enemy>();
            enemyPool[x].Init();
        }
        for (int x = 0; x < enemyDigPool.Length; x++)
        {
            GameObject enemyGO = Instantiate(EnemyPrefab, this.transform.localPosition, Quaternion.identity, EnemyContainer.transform);
            enemyDigPool[x] = enemyGO.GetComponent<Enemy>();
            enemyDigPool[x].Init();
        }
        for (int x = 0; x < enemyFBIPool.Length; x++)
        {
            GameObject enemyGO = Instantiate(EnemyPrefab, this.transform.localPosition, Quaternion.identity, EnemyContainer.transform);
            enemyFBIPool[x] = enemyGO.GetComponent<Enemy>();
            enemyFBIPool[x].Init();
        }
        for (int x = 0; x < enemyScientistPool.Length; x++)
        {
            GameObject enemyGO = Instantiate(EnemyPrefab, this.transform.localPosition, Quaternion.identity, EnemyContainer.transform);
            enemyScientistPool[x] = enemyGO.GetComponent<Enemy>();
            enemyScientistPool[x].Init();
        }
        for (int x = 0; x < enemyVehiclePool.Length; x++)
        {
            GameObject enemyGO = Instantiate(EnemyPrefab, this.transform.localPosition, Quaternion.identity, EnemyContainer.transform);
            enemyVehiclePool[x] = enemyGO.GetComponent<Enemy>();
            enemyVehiclePool[x].Init();
        }
        for (int x = 0; x < enemyBossPool.Length; x++)
        {
            GameObject enemyGO = Instantiate(EnemyPrefab, this.transform.localPosition, Quaternion.identity, EnemyContainer.transform);
            enemyBossPool[x] = enemyGO.GetComponent<Enemy>();
            enemyBossPool[x].Init();
        }
    }
    public void ActivateEnemyFromPool(Vector3 enemyPos, Globals.EnemyTypes enemyType, int extraLife, bool flip)
    {
        Enemy[] pool = enemyPool;
        if (enemyType == Globals.EnemyTypes.FBI)
            pool = enemyFBIPool;
        else if (enemyType == Globals.EnemyTypes.Scientist)
            pool = enemyScientistPool;
        else if (enemyType == Globals.EnemyTypes.Dig)
            pool = enemyDigPool;
        else if (enemyType == Globals.EnemyTypes.Moon || enemyType == Globals.EnemyTypes.Plane)
            pool = enemyVehiclePool;
        else if (enemyType == Globals.EnemyTypes.PacBoss || enemyType == Globals.EnemyTypes.KoolBoss || enemyType == Globals.EnemyTypes.PopeyeBoss ||
                 enemyType == Globals.EnemyTypes.MarioBoss || enemyType == Globals.EnemyTypes.LuigiBoss || enemyType == Globals.EnemyTypes.HarryBoss)
            pool = enemyBossPool;

        for (int x = 0; x < pool.Length; x++)
        {
            if (!pool[x].IsActive())
            {
                pool[x].ConfigureEnemy(enemyPos, enemyType, extraLife, flip);
                break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        string[] controllers = Input.GetJoystickNames();
        for (int x = 0; x < controllers.Length; x++)
        {
            if (controllers[x] != "")
                controllerAttached = true;
        }
        Globals.ResetGlobals();

        currentBossType = (Globals.EnemyTypes)Random.Range((int)Globals.EnemyTypes.PacBoss, (int)Globals.EnemyTypes.KoolBoss + 1);
        gameHasBoss = Random.Range(0, 2) == 1 ? true: false;

        // WTD WTD WTD remove
        currentBossType = Globals.EnemyTypes.PacBoss;
        gameHasBoss= true;

        SpawnEnemies(10);
        SpawnFBI();

        playerScript = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleFadeOut();
        HandleDifficultyTimer();
        HandleSpecialAttackTimer();
        HandleTankTimer();
        HandleEnemySpawnTimer();
        HandleLevelUpTimer();
        HandleBoss();
    }

    void HandleInput()
    {
        if (Globals.IsPaused && controllerAttached)
        {
            if (Input.GetButton("Fire1"))
                SelectUpgrade(upgradeHighlightIndex);

            float controllerLeftStickX;
            controllerLeftStickX = Input.GetAxis("Horizontal");
            bool moveLeft = false;
            bool moveRight = false;
            if (controllerLeftStickX > .5f)
            {
                if (!stickDown) moveRight = true;
                stickDown = true;
            }
            else if (controllerLeftStickX < -.5f)
            {
                if (!stickDown) moveLeft = true;
                stickDown = true;
            }
            else
            {
                stickDown = false;
            }
            int previousUpgradeHighlightIndex = upgradeHighlightIndex;
            if (moveLeft)
                upgradeHighlightIndex = Mathf.Max(0, upgradeHighlightIndex - 1);
            else if (moveRight)
                upgradeHighlightIndex = Mathf.Min(2, upgradeHighlightIndex + 1);
            if (previousUpgradeHighlightIndex != upgradeHighlightIndex)
                audioManager.PlayMenuSound();
            HighlightUpgradeButton();
        }
    }

    void HandleFadeOut()
    {
        if (deadTimer > 0)
        {
            deadTimer -= Time.deltaTime;
            if (deadTimer <= 0)
            {
                fadeManager.StartFadeOut();
                fadeOut = true;
            }
        }
        if (fadeOut && fadeManager.FadeComplete())
        {
            fadeOut = false;
            SceneManager.LoadScene("EndScene");
        }
    }

    void HandleDifficultyTimer()
    {
        difficultyTimer -= Time.deltaTime;
        if (difficultyTimer <= 0)
        {
            difficultyTimer = difficultyTimerMax;
            difficultyLevel++;
            spawnTimerMax = Mathf.Max(5f, spawnTimerMax - .5f);

            fastEnemySpawnRates[0] = difficultyLevel < enemyFastOneSpawnRates.Length ? enemyFastOneSpawnRates[difficultyLevel] : enemyFastOneSpawnRates[enemyFastOneSpawnRates.Length - 1];
            fastEnemySpawnRates[1] = fastEnemySpawnRates[0] + (difficultyLevel < enemyFastTwoSpawnRates.Length ? enemyFastTwoSpawnRates[difficultyLevel] : enemyFastTwoSpawnRates[enemyFastTwoSpawnRates.Length - 1]);
            fastEnemySpawnRates[2] = fastEnemySpawnRates[1] + (difficultyLevel < enemyFastThreeSpawnRates.Length ? enemyFastThreeSpawnRates[difficultyLevel] : enemyFastThreeSpawnRates[enemyFastThreeSpawnRates.Length - 1]);
            fastEnemySpawnRates[3] = fastEnemySpawnRates[2] + (difficultyLevel < enemyFastFourSpawnRates.Length ? enemyFastFourSpawnRates[difficultyLevel] : enemyFastFourSpawnRates[enemyFastFourSpawnRates.Length - 1]);
            fastEnemySpawnRates[4] = fastEnemySpawnRates[3] + (difficultyLevel < enemyFastFiveSpawnRates.Length ? enemyFastFiveSpawnRates[difficultyLevel] : enemyFastFiveSpawnRates[enemyFastFiveSpawnRates.Length - 1]);
            fastEnemySpawnRates[5] = fastEnemySpawnRates[4] + (difficultyLevel < enemyFastSixSpawnRates.Length ? enemyFastSixSpawnRates[difficultyLevel] : enemyFastSixSpawnRates[enemyFastSixSpawnRates.Length - 1]);
            fastEnemySpawnRates[6] = fastEnemySpawnRates[5] + (difficultyLevel < enemyFastSevenSpawnRates.Length ? enemyFastSevenSpawnRates[difficultyLevel] : enemyFastSevenSpawnRates[enemyFastSevenSpawnRates.Length - 1]);

            strongEnemySpawnRates[0] = difficultyLevel < enemyStrongOneSpawnRates.Length ? enemyStrongOneSpawnRates[difficultyLevel] : enemyStrongOneSpawnRates[enemyStrongOneSpawnRates.Length - 1];
            strongEnemySpawnRates[1] = strongEnemySpawnRates[0] + (difficultyLevel < enemyStrongTwoSpawnRates.Length ? enemyStrongTwoSpawnRates[difficultyLevel] : enemyStrongTwoSpawnRates[enemyStrongTwoSpawnRates.Length - 1]);
            strongEnemySpawnRates[2] = strongEnemySpawnRates[1] + (difficultyLevel < enemyStrongThreeSpawnRates.Length ? enemyStrongThreeSpawnRates[difficultyLevel] : enemyStrongThreeSpawnRates[enemyStrongThreeSpawnRates.Length - 1]);
            strongEnemySpawnRates[3] = strongEnemySpawnRates[2] + (difficultyLevel < enemyStrongFourSpawnRates.Length ? enemyStrongFourSpawnRates[difficultyLevel] : enemyStrongFourSpawnRates[enemyStrongFourSpawnRates.Length - 1]);
            strongEnemySpawnRates[4] = strongEnemySpawnRates[3] + (difficultyLevel < enemyStrongFiveSpawnRates.Length ? enemyStrongFiveSpawnRates[difficultyLevel] : enemyStrongFiveSpawnRates[enemyStrongFiveSpawnRates.Length - 1]);

            surroundEnemySpawnRates[0] = difficultyLevel < enemySurroundOneSpawnRates.Length ? enemySurroundOneSpawnRates[difficultyLevel] : enemySurroundOneSpawnRates[enemySurroundOneSpawnRates.Length - 1];
            surroundEnemySpawnRates[1] = surroundEnemySpawnRates[0] + (difficultyLevel < enemySurroundTwoSpawnRates.Length ? enemySurroundTwoSpawnRates[difficultyLevel] : enemySurroundTwoSpawnRates[enemySurroundTwoSpawnRates.Length - 1]);
            surroundEnemySpawnRates[2] = surroundEnemySpawnRates[1] + (difficultyLevel < enemySurroundThreeSpawnRates.Length ? enemySurroundThreeSpawnRates[difficultyLevel] : enemySurroundThreeSpawnRates[enemySurroundThreeSpawnRates.Length - 1]);

            currentSurroundEnemyMaxSpawn = surroundEnemySpawnRates[surroundEnemySpawnRates.Length - 1];
            currentStrongEnemyMaxSpawn = strongEnemySpawnRates[strongEnemySpawnRates.Length - 1];
            currentFastEnemyMaxSpawn = fastEnemySpawnRates[fastEnemySpawnRates.Length - 1];
        }
    }

    void HandleSpecialAttackTimer()
    {
        specialAttackTimer -= Time.deltaTime;
        if (specialAttackTimer <= 0)
        {
            specialAttackTimer = specialAttackTimerMax;

            if (gameHasBoss && !bossDefeated && (difficultyLevel == 6 || difficultyLevel == 10 || difficultyLevel == 14))
            {
                StartBoss();
                return;
            }

            EnemySpecialAttackPatterns specialNum = (EnemySpecialAttackPatterns)Random.Range(0, (int)EnemySpecialAttackPatterns.Digs);
            if (difficultyLevel > 3 && difficultyLevel <= 5)
                specialNum = (EnemySpecialAttackPatterns)Random.Range(0, (int)EnemySpecialAttackPatterns.Planes);
            else if (difficultyLevel > 5)
                specialNum = (EnemySpecialAttackPatterns)Random.Range(0, (int)EnemySpecialAttackPatterns.Rovers);
            else if (difficultyLevel > 7)
                specialNum = (EnemySpecialAttackPatterns)Random.Range(0, (int)EnemySpecialAttackPatterns.None);

            if (specialNum == EnemySpecialAttackPatterns.VerticalMove)
            {
                bottomTanksTransform.gameObject.GetComponent<MoveNormal>().MoveUp();
                topTanksTransform.gameObject.GetComponent<MoveNormal>().MoveDown();
                tankReturnTimer = Random.Range(20f, 40f);
            }
            else if (specialNum == EnemySpecialAttackPatterns.HorizontalMove)
            {
                leftTanksTransform.gameObject.GetComponent<MoveNormal>().MoveRight();
                rightTanksTransform.gameObject.GetComponent<MoveNormal>().MoveLeft();
                tankReturnTimer = Random.Range(20f, 40f);
            }
            else if (specialNum == EnemySpecialAttackPatterns.Digs)
            {
                digSpawnsRemaining = Random.Range(Mathf.Min(difficultyLevel, 2), Mathf.Min(difficultyLevel, 4));
            }
            else if (specialNum == EnemySpecialAttackPatterns.Planes)
            {
                planeSpawnsRemaining = Random.Range(Mathf.Min(difficultyLevel, 2), Mathf.Min(difficultyLevel, 5));
            }
            else if (specialNum == EnemySpecialAttackPatterns.Rovers)
            {
                roverSpawnsRemaining = Random.Range(Mathf.Min(difficultyLevel, 2), Mathf.Min(difficultyLevel, 5));
            }
        }
    }

    void HandleTankTimer()
    {
        if (tankReturnTimer > 0)
        {
            tankReturnTimer -= Time.deltaTime;
            if (tankReturnTimer <= 0)
            {
                bottomTanksTransform.gameObject.GetComponent<MoveNormal>().MoveDown();
                topTanksTransform.gameObject.GetComponent<MoveNormal>().MoveUp();
                leftTanksTransform.gameObject.GetComponent<MoveNormal>().MoveLeft();
                rightTanksTransform.gameObject.GetComponent<MoveNormal>().MoveRight();
            }
        }
    }

    void HandleEnemySpawnTimer()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            spawnTimer = spawnTimerMax;
            SpawnEnemies(10 + (int)((float)difficultyLevel * 1.5f));
            SpawnFBI();
        }
    }

    void HandleLevelUpTimer()
    {
        if (levelUpTimer > 0)
        {
            levelUpTimer -= Time.deltaTime;
            if (levelUpTimer <= 0)
            {
                LevelUpPanel.GetComponent<MoveWhenPaused>().MoveDown();
            }
        }
    }

    void HandleBoss()
    {
        if (currentBossState == BossStates.ShowHUD)
        {
            bossTimer -= Time.deltaTime;
            if (bossTimer <= 0)
            {
                int firstBossIndex = (int)Globals.EnemyTypes.PacBoss;
                HUDBossTextType.StartEffect(Globals.BossText[(int)currentBossType - firstBossIndex]);
                bossTimer = 1f;
                currentBossState = BossStates.ShowText;
            }
        }
        else if (currentBossState == BossStates.ShowText)
        {
            bossTimer -= Time.deltaTime;
            if (bossTimer <= 0)
            {
                SpawnBoss();
                bossTimer = 1f;
                currentBossState = BossStates.ShowBoss;
            }
        }
        else if (currentBossState == BossStates.ShowBoss)
        {
            bossTimer -= Time.deltaTime;
            if (bossTimer <= 0)
            {
                HUDBossPanel.GetComponent<MoveNormal>().MoveLeft();
                currentBossState = BossStates.None;
            }
        }
    }

    void SpawnFBI()
    {
        int extraLife = (int)(difficultyLevel * .5f);
        SpawnEnemy(Globals.EnemyTypes.FBI, extraLife);
    }

    void SpawnEnemies(int num)
    {
        numSpawns++;
        int extraLife = 0;
        for (int x = 0; x < num; x++)
        {
            Globals.EnemyTypes enemyType = Globals.EnemyTypes.Yar;
            float randVal = Random.Range(0, 100f);
            if (randVal > 98f && difficultyLevel > 3)
            {
                enemyType = Globals.EnemyTypes.Scientist;
                extraLife = (int)(difficultyLevel * .5f);
            }
            else if (randVal > 84f)
                enemyType = GetEnemyType(currentStrongEnemyMaxSpawn, strongEnemySpawnRates, Globals.StrongEnemyTypes);
            else if (randVal > 74f)
                enemyType = GetEnemyType(currentSurroundEnemyMaxSpawn, surroundEnemySpawnRates, Globals.SurroundEnemyTypes);
            else
                enemyType = GetEnemyType(currentFastEnemyMaxSpawn, fastEnemySpawnRates, Globals.FastEnemyTypes);
            SpawnEnemy(enemyType, extraLife);
        }
        if (digSpawnsRemaining > 0)
        {
            digSpawnsRemaining--;
            SpawnDigs();
        }
        if (planeSpawnsRemaining > 0)
        {
            planeSpawnsRemaining--;
            SpawnPlanes();
        }
        if (roverSpawnsRemaining > 0)
        {
            roverSpawnsRemaining--;
            SpawnRovers();
        }
    }

    void SpawnEnemy(Globals.EnemyTypes enemyType, int extraLife)
    {
        // Debug.Log(Globals.currrentNumEnemies);
        if (Globals.currrentNumEnemies >= Globals.maxEnemies && enemyType != Globals.EnemyTypes.FBI)
            return;
        bool verticalPos = (Random.Range(0, 2) == 0);
        float xRangeMax = Mathf.Min((rightTanksTransform.position.x - leftTanksTransform.position.x - 2f) * .5f, 13f);
        float yRangeMax = Mathf.Min((topTanksTransform.position.y - bottomTanksTransform.position.y - 2f) * .5f, 9f);
        float xOffset = verticalPos
            ? Random.Range(-10.5f, 10.5f)
            : Random.Range(10f, xRangeMax) * (Random.Range(0, 2) == 0 ? -1f : 1f);
        float yOffset = verticalPos
            ? Random.Range(6f, yRangeMax) * (Random.Range(0, 2) == 0 ? -1f : 1f)
            : Random.Range(-6.5f, 6.5f);
        Vector2 playerPos = Player.transform.localPosition;
        float minX = leftTanksTransform.position.x + 1f;
        float maxX = rightTanksTransform.position.x - 1f;
        float minY = bottomTanksTransform.position.y + 1f;
        float maxY = topTanksTransform.position.y - 1f;
        if ((playerPos.x + xOffset) < minX || (playerPos.x + xOffset) > maxX)
        {
            xOffset = xOffset * -1f;
        }
        if ((playerPos.y + yOffset) < minY || (playerPos.y + yOffset) > maxY)
        {
            yOffset = yOffset * -1f;
        }
        Vector2 enemyPos = new Vector2(playerPos.x + xOffset, playerPos.y + yOffset);
        ActivateEnemyFromPool(enemyPos, enemyType, extraLife, false);
        Globals.currrentNumEnemies++;
    }

    void StartBoss()
    {
        int firstBossIndex = (int)Globals.EnemyTypes.PacBoss;
        HUDBossImage.sprite = BossSprites[(int)currentBossType - firstBossIndex];
        HUDBossNameText.text = Globals.BossNames[(int)currentBossType - firstBossIndex];
        HUDBossText.text = "";
        HUDBossPanel.GetComponent<MoveNormal>().MoveRight();
        currentBossState = BossStates.ShowHUD;
        bossTimer = .75f;
    }

    void SpawnBoss()
    {
        Vector2 enemyPos = new Vector3(Player.transform.localPosition.x - 4f, Player.transform.localPosition.y + 10f, Player.transform.localPosition.z);
        ActivateEnemyFromPool(enemyPos, currentBossType, 0, false);
        // WTD
        // destroy nearby enemies
        // shake camera
        // make noise
        // make shoot
        // handle invalid spawn position
    }

    void KillBoss()
    {
        bossDefeated = true;
    }

    void SpawnDigs()
    {
        int extraLife = (int)(difficultyLevel * .5f);
        int numDigs = 15;
        Vector2 playerPos = Player.transform.localPosition;
        float minX = leftTanksTransform.position.x + 1f;
        float maxX = rightTanksTransform.position.x - 1f;
        float minY = bottomTanksTransform.position.y + 1f;
        float maxY = topTanksTransform.position.y - 1f;
        for (int x = 0; x < numDigs; x++)
        {
            Vector2 enemyRadialVector = Quaternion.Euler(0, 0, x * 24f) * new Vector2(3f, 3f);
            Vector2 enemyPos = playerPos + enemyRadialVector;
            if (enemyPos.x < maxX && enemyPos.x > minX && enemyPos.y < maxY && enemyPos.y > minY)
            {
                ActivateEnemyFromPool(enemyPos, Globals.EnemyTypes.Dig, extraLife, false);
            }
        }
    }

    void SpawnPlanes()
    {
        int extraLife = (int)(difficultyLevel * .5f);
        int numRows = 5;
        int numCols = 3;
        Vector2 playerPos = Player.transform.localPosition;
        float startX = playerPos.x - 6f;
        float startY = playerPos.y - 4f;
        for (int x = 0; x < numCols; x++)
        {
            for (int y = 0; y < numRows; y++)
            {
                Vector2 enemyPos = new Vector2(startX + x * -2.5f, startY + y * 2f);
                ActivateEnemyFromPool(enemyPos, Globals.EnemyTypes.Plane, extraLife, false);
            }
        }
    }

    void SpawnRovers()
    {
        int extraLife = (int)(difficultyLevel * .5f);
        int numRows = 5;
        int numCols = 2;
        Vector2 playerPos = Player.transform.localPosition;
        float startX = playerPos.x - 6f;
        float startY = playerPos.y - 4f;
        for (int x = 0; x < numCols; x++)
        {
            for (int y = 0; y < numRows; y++)
            {
                Vector2 enemyPos = new Vector2(startX + x * -4.5f, startY + y * 2f);
                ActivateEnemyFromPool(enemyPos, Globals.EnemyTypes.Moon, extraLife, false);
            }
        }
        startX = playerPos.x + 6f;
        startY = playerPos.y - 5f;
        for (int x = 0; x < numCols; x++)
        {
            for (int y = 0; y < numRows; y++)
            {
                Vector2 enemyPos = new Vector2(startX + x * 4.5f, startY + y * 2f);
                ActivateEnemyFromPool(enemyPos, Globals.EnemyTypes.Moon, extraLife, true);
            }
        }
    }

    Globals.EnemyTypes GetEnemyType(int currentEnemyMaxSpawn, int[] enemySpawnRates, Globals.EnemyTypes[] enemyTypes)
    {
        int randVal = Random.Range(0, currentEnemyMaxSpawn);
        int index = 0;
        int x = 0;
        bool valid = false;
        do {
            index = x;
            if (randVal < enemySpawnRates[x])
                valid = true;
            x++;
        } while (!valid && x < enemySpawnRates.Length);
        Globals.EnemyTypes enemyType = enemyTypes[index];
        return enemyType;
    }

    public void SelectUpgrade(int upgradeNum)
    {
        audioManager.PlayButtonSound();
        if (availableUpgrades[upgradeNum] == Globals.UpgradeTypes.RefillHP)
        {
            playerScript.RestoreMaxHealth();
        }
        else
        {
            if (Globals.CurrentUpgradeLevels[(int)availableUpgrades[upgradeNum]] == 0)
            {
                int numUpgrades = Globals.CurrentUpgradeTypes.Count;
                HUDUpgradeDisplays[numUpgrades].SetActive(true);
                HUDUpgradeDisplays[numUpgrades].GetComponent<Image>().sprite =  UpgradeSprites[(int)availableUpgrades[upgradeNum]];
                Globals.CurrentUpgradeTypes.Add(availableUpgrades[upgradeNum]);

            }
            Globals.CurrentUpgradeLevels[(int)availableUpgrades[upgradeNum]]++;
        }
        HUDUpgradePanel.GetComponent<MoveWhenPaused>().MoveDown();
        playerScript.ResetHUDPhone();
        playerScript.UpdateUpgrades(availableUpgrades[upgradeNum]);
        Time.timeScale = 1f;
        Globals.IsPaused = false;
    }

    public void ShowUpgradeSelection()
    {
        upgradeHighlightIndex = 0;
        if (controllerAttached)
            HighlightUpgradeButton();
        availableUpgrades.Clear();
        int numUpgrades = 0;
        int maxUpgrades = 6;
        for (int x = 0; x < Globals.CurrentUpgradeLevels.Length; x++)
        {
            if (Globals.CurrentUpgradeLevels[x] > 0)
                numUpgrades++;
        }
        for (int x = 0; x < Globals.CurrentUpgradeLevels.Length; x++)
        {
            if ((numUpgrades < maxUpgrades && Globals.CurrentUpgradeLevels[x] < Globals.MaxUpgradeLevel) ||
                (numUpgrades >= maxUpgrades && Globals.CurrentUpgradeLevels[x] < Globals.MaxUpgradeLevel && Globals.CurrentUpgradeLevels[x] > 0))
                availableUpgrades.Add((Globals.UpgradeTypes)x);
        }
        int maxSlots = 3;
        int numHPslots = maxSlots - availableUpgrades.Count;
        for (int x = 0; x < numHPslots; x++)
        {
            availableUpgrades.Add(Globals.UpgradeTypes.RefillHP);
        }

        for (int x = 0; x < availableUpgrades.Count; x++)
        {
            int swapPos = Random.Range(0, availableUpgrades.Count);
            Globals.UpgradeTypes temp = availableUpgrades[x];
            availableUpgrades[x] = availableUpgrades[swapPos];
            availableUpgrades[swapPos] = temp;
        }

        for (int x = 0; x < HUDUpgradeButtonTitleTexts.Length; x++)
        {
            HUDUpgradeButtonTitleTexts[x].text = Globals.UpgradeText[(int)availableUpgrades[x]];
            if (availableUpgrades[x] == Globals.UpgradeTypes.RefillHP)
            {
                HUDUpgradeButtonLvlTexts[x].text = "";
                HUDUpgradeButtonDescTexts[x].text = Globals.UpgradeDescriptionText[(int)availableUpgrades[x] * Globals.MaxLevelsPerUpgrade];
                HUDUpgradeButtonIcons[x].sprite = UpgradeSprites[UpgradeSprites.Length - 1];
            }
            else
            {
                HUDUpgradeButtonLvlTexts[x].text = "Lvl " + (Globals.CurrentUpgradeLevels[(int)availableUpgrades[x]] + 1);
                HUDUpgradeButtonDescTexts[x].text = Globals.UpgradeDescriptionText[(int)availableUpgrades[x] * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)availableUpgrades[x]]];
                HUDUpgradeButtonIcons[x].sprite = UpgradeSprites[(int)availableUpgrades[x]];
            }
        }

        audioManager.PlayPhoneDialSound();
        HUDUpgradePanel.GetComponent<MoveWhenPaused>().MoveUp();
        LevelUpPanel.GetComponent<MoveWhenPaused>().MoveDown();
        Time.timeScale = 0f;
        Globals.IsPaused = true;
    }

    private void HighlightUpgradeButton()
    {
        for (int x = 0; x < HUDUpgradeButtonHighlights.Length; x++)
        {
            HUDUpgradeButtonHighlights[x].SetActive(x == upgradeHighlightIndex);
        }
    }

    public void AddExperience(int expAmount)
    {
        Globals.currentExp += expAmount;
        int maxExp = Globals.currentLevel < Globals.maxExperiences.Length
            ? Globals.maxExperiences[Globals.currentLevel]
            : Globals.maxExperiences[Globals.maxExperiences.Length - 1];
        Globals.currentExp = Mathf.Min(maxExp, Globals.currentExp);
        if (Globals.currentExp == maxExp)
        {
            Globals.currentLevel++;
            Globals.currentExp = 0;
            ExpLevel.text = "LVL " + (Globals.currentLevel + 1);
            string statsText = "";
            if (Globals.healthPerLevel.Length > Globals.currentLevel && Globals.healthPerLevel[Globals.currentLevel] > 0)
            {
                statsText = statsText + ("HP+" + Globals.healthPerLevel[Globals.currentLevel] + " ");
                Globals.currentMaxHealth += Globals.healthPerLevel[Globals.currentLevel];
            }
            if (Globals.attackPerLevel.Length > Globals.currentLevel && Globals.attackPerLevel[Globals.currentLevel] > 0)
            {
                statsText = statsText + ("ATTACK+" + Globals.attackPerLevel[Globals.currentLevel] + " ");
                Globals.currentAttack += Globals.attackPerLevel[Globals.currentLevel];
            }
            if (Globals.defensePerLevel.Length > Globals.currentLevel && Globals.defensePerLevel[Globals.currentLevel] > 0)
            {
                statsText = statsText + ("DEFENSE+" + Globals.defensePerLevel[Globals.currentLevel] + " ");
                Globals.currentDefense += Globals.defensePerLevel[Globals.currentLevel];
            }
            LevelUpStats.text = statsText;

            LevelUpPanel.GetComponent<MoveWhenPaused>().MoveUp();
            levelUpTimer = levelUpTimerMax;

            playerScript.RestoreMaxHealth();

            audioManager.PlayLevelUpSound();
        }
        float maxExpBarWidth = 400f;
        ExpBar.sizeDelta = new Vector2 ((float)(Globals.currentExp) / maxExp * maxExpBarWidth, ExpBar.sizeDelta.y);
    }

    public void GameOver()
    {
        deadTimer = deadTimerMax;
    }
}
