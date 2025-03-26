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
    CameraController cameraController;

    [SerializeField]
    FadeManager fadeManager;

    [SerializeField]
    GameObject Player;
    Player playerScript;
    [SerializeField]
    GameObject DustPrefab;
    Dust[] dustPool = new Dust[10];

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
    RectTransform[] HUDUpgradeButtons;
    [SerializeField]
    TextMeshProUGUI[] HUDUpgradeButtonTitleTexts;
    [SerializeField]
    TextMeshProUGUI[] HUDUpgradeButtonLvlTexts;
    [SerializeField]
    TextMeshProUGUI[] HUDUpgradeButtonDescTexts;
    [SerializeField]
    Image[] HUDUpgradeButtonIcons;
    [SerializeField]
    RectTransform[] HUDUpgradeCompletionBoxes;
    List<Globals.UpgradeTypes> availableUpgrades = new List<Globals.UpgradeTypes>();
    int upgradeHighlightIndex = 0;
    bool stickDown = false;
    bool controllerAttached = false;

    int difficultyLevel = 0;
    float difficultyTimer = 60f;
    float difficultyTimerMax = 60f;
    float specialAttackTimer = 90f;
    float specialAttackTimerMax = 60f;
    float specialAttackTimerMin = 40f;

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
    GameObject DebrisPrefab;
    [SerializeField]
    GameObject DebrisContainer;
    Debris[] debrisPool = new Debris[100];
    [SerializeField]
    GameObject HitNoticeContainer;
    [SerializeField]
    GameObject HitNoticePrefab;
    HitNotice[] hitNoticePool = new HitNotice[40];
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
    Transform LeftWall;
    Transform TopWall;
    Transform RightWall;
    Transform BottomWall;

    enum EnemySpecialAttackPatterns {
        VerticalMove,
        HorizontalMove,
        Digs,
        Planes,
        Rovers,
        None
    }

    int[] fastEnemySpawnValues =     { 6,  0, -3, -6,  -9 };
    int[] strongEnemySpawnValues =   { 4, -1, -4, -7, -11 };
    int[] surroundEnemySpawnValues = { 2, -3, -7, -11 };
    int[] chaoticEnemySpawnValues =  {-3, -8, -12};

    int[] fastEnemySpawnThresholds = { 2, 0, 0, 0, 0 };
    int[] strongEnemySpawnThresholds = { 2, 0, 0, 0, 0 };
    int[] surroundEnemySpawnThresholds = { 2, 0, 0, 0 };
    int[] chaoticEnemySpawnThresholds = {2, 0, 0};

    int currentFastEnemyMaxSpawn = 2;
    int currentStrongEnemyMaxSpawn = 2;
    int currentSurroundEnemyMaxSpawn = 2;
    int currentChaoticEnemyMaxSpawn = 2;
    int numSpawns = 0;

    float spawnTimer = 5f;
    float spawnTimerMax = 7f;
    float FBIspawnTimer = 5f;
    float FBIspawnTimerMax = 7f;
    int digSpawnsRemaining = 0;
    int planeSpawnsRemaining = 0;
    int roverSpawnsRemaining = 0;
    float tankReturnTimer = 0;
    int totalUpgradesThisGame = 0;

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
    Globals.EnemyTypes currentBossType = Globals.EnemyTypes.PacBoss;
    bool gameHasBoss = false;
    int bossSpawnDifficulty = 0;
    bool bossDefeated = false;

    float deadTimer = 0f;
    float deadTimerMax = 4f;
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
        LeftWall = GameObject.Find("EnemySolidLeft").transform;
        TopWall = GameObject.Find("EnemySolidTop").transform;
        RightWall = GameObject.Find("EnemySolidRight").transform;
        BottomWall = GameObject.Find("EnemySolidBottom").transform;
        CreateCandyPool();
        CreatePhonePool();
        CreateEnemyPool();
        CreateHitNoticePool();
        CreateDebrisPool();
        CreateDustPool();

        fadeManager.StartFadeIn();
    }

    void CreateCandyPool()
    {
        for (int x = 0; x < candyPool.Length; x++)
        {
            GameObject candyGO = Instantiate(CandyPrefab, this.transform.localPosition, Quaternion.identity, ItemContainer.transform);
            candyPool[x] = candyGO.GetComponent<Candy>();
        }
    }
    public void ActivateCandyFromPool(Vector3 candyPos, bool move, Vector3 moveDir)
    {
        for (int x = 0; x < candyPool.Length; x++)
        {
            if (!candyPool[x].IsActive())
            {
                candyPool[x].Activate(candyPos);
                if (move)
                    candyPool[x].StartMove(moveDir);
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
    void CreateDustPool()
    {
        for (int x = 0; x < dustPool.Length; x++)
        {
            GameObject go = Instantiate(DustPrefab, this.transform.localPosition, Quaternion.identity, DebrisContainer.transform);
            dustPool[x] = go.GetComponent<Dust>();
        }
    }
    public void ActivateDustFromPool(Vector3 pos)
    {
        for (int x = 0; x < dustPool.Length; x++)
        {
            if (!dustPool[x].IsActive())
            {
                dustPool[x].Activate(pos);
                break;
            }
        }
    }
    void CreateDebrisPool()
    {
        for (int x = 0; x < debrisPool.Length; x++)
        {
            GameObject go = Instantiate(DebrisPrefab, this.transform.localPosition, Quaternion.identity, DebrisContainer.transform);
            debrisPool[x] = go.GetComponent<Debris>();
        }
    }
    public void ActivateDebrisFromPool(Vector3 pos, bool isPlayer)
    {
        for (int x = 0; x < debrisPool.Length; x++)
        {
            if (!debrisPool[x].IsActive())
            {
                debrisPool[x].Activate(pos, isPlayer);
                break;
            }
        }
    }
    void CreateHitNoticePool()
    {
        for (int x = 0; x < hitNoticePool.Length; x++)
        {
            GameObject go = Instantiate(HitNoticePrefab, this.transform.localPosition, Quaternion.identity, HitNoticeContainer.transform);
            hitNoticePool[x] = go.GetComponent<HitNotice>();
        }
    }
    public void ActivateHitNoticeFromPool(Vector3 pos, int damage)
    {
        for (int x = 0; x < hitNoticePool.Length; x++)
        {
            if (!hitNoticePool[x].IsActive())
            {
                hitNoticePool[x].Activate(pos, damage);
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
    public void ActivateEnemyFromPool(Vector3 enemyPos, Globals.EnemyTypes enemyType, bool flip)
    {
        Enemy[] pool = enemyPool;
        int extraLife = 0;
        int extraStrength = 0;
        float extraSpeed = 0;
        if (enemyType == Globals.EnemyTypes.FBI)
        {
            extraLife = (int)(difficultyLevel * 1.5f);
            pool = enemyFBIPool;
        }
        else if (enemyType == Globals.EnemyTypes.Scientist)
        {
            extraLife = (int)(difficultyLevel * .5f);
            pool = enemyScientistPool;
        }
        else if (enemyType == Globals.EnemyTypes.Dig)
        {
            extraLife = (int)(difficultyLevel * .5f);
            pool = enemyDigPool;
        }
        else if (enemyType == Globals.EnemyTypes.Moon || enemyType == Globals.EnemyTypes.Plane)
        {
            extraLife = (int)(difficultyLevel * .5f);
            pool = enemyVehiclePool;
        }
        else if (enemyType == Globals.EnemyTypes.PacBoss || enemyType == Globals.EnemyTypes.KoolBoss || enemyType == Globals.EnemyTypes.PopeyeBoss ||
                 enemyType == Globals.EnemyTypes.MarioBoss || enemyType == Globals.EnemyTypes.LuigiBoss)
        {
            pool = enemyBossPool;
        }
        else
        {
            if (difficultyLevel > 40)
            {
                extraLife = 10;
                extraStrength = 6;
                extraSpeed = .3f;
            }
            if (difficultyLevel > 30)
            {
                extraLife = 6;
                extraStrength = 6;
                extraSpeed = .2f;
            }
            else if (difficultyLevel > 25)
            {
                extraLife = 4;
                extraStrength = 4;
                extraSpeed = .1f;
            }
            else if (difficultyLevel > 20)
            {
                extraLife = 2;
                extraStrength = 2;
            }
        }

        for (int x = 0; x < pool.Length; x++)
        {
            if (!pool[x].IsActive())
            {
                pool[x].ConfigureEnemy(enemyPos, enemyType, extraLife, extraStrength, extraSpeed, flip);
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
        bossSpawnDifficulty = Random.Range(6, 10);

        SpawnEnemies(10, false);
        SpawnFBI(false);

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
    }

    void HandleInput()
    {
        if (Globals.IsPaused)
        {
            bool moveLeft = false;
            bool moveRight = false;

            if (controllerAttached)
            {
                if (Input.GetButtonDown("Fire1"))
                    SelectUpgrade(upgradeHighlightIndex);

                float controllerLeftStickX;
                controllerLeftStickX = Input.GetAxis("Horizontal");
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
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown("a"))
                moveLeft = true;
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("d"))
                moveRight = true;
            if (Input.GetKeyDown("space"))
                SelectUpgrade(upgradeHighlightIndex);

            if (moveLeft)
            {
                if (upgradeHighlightIndex == 0)
                    return;
                upgradeHighlightIndex--;
                audioManager.PlayMenuSound();
                HighlightUpgradeButton();
            }
            else if (moveRight)
            {
                if (upgradeHighlightIndex == HUDUpgradeButtons.Length - 1)
                    return;
                upgradeHighlightIndex++;
                audioManager.PlayMenuSound();
                HighlightUpgradeButton();
            }
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

            // -2, -1, 0, 2, 4, 6, 8, 7, 5, 3, 1,
            UpdateSpawnValuesAndThresholds(fastEnemySpawnValues, fastEnemySpawnThresholds);
            UpdateSpawnValuesAndThresholds(strongEnemySpawnValues, strongEnemySpawnThresholds);
            UpdateSpawnValuesAndThresholds(surroundEnemySpawnValues, surroundEnemySpawnThresholds);
            UpdateSpawnValuesAndThresholds(chaoticEnemySpawnValues, chaoticEnemySpawnThresholds);

            currentFastEnemyMaxSpawn = fastEnemySpawnThresholds[fastEnemySpawnThresholds.Length - 1];
            currentStrongEnemyMaxSpawn = strongEnemySpawnThresholds[strongEnemySpawnThresholds.Length - 1];
            currentSurroundEnemyMaxSpawn = surroundEnemySpawnThresholds[surroundEnemySpawnThresholds.Length - 1];
            currentChaoticEnemyMaxSpawn = chaoticEnemySpawnThresholds[chaoticEnemySpawnThresholds.Length - 1];
        }
    }

    void UpdateSpawnValuesAndThresholds(int[] spawnValues, int[] spawnThresholds)
    {
        for (int x = 0; x < spawnValues.Length; x++)
        {
            if (spawnValues[x] < 0)
                spawnValues[x]++;
            else if (spawnValues[x] == 8)
                spawnValues[x]--;
            else if (spawnValues[x] % 2 == 0)
                spawnValues[x] = spawnValues[x] + 2;
            else if (spawnValues[x] % 2 == 1 && spawnValues[x] > 1)
                spawnValues[x] = spawnValues[x] - 2;
        }
        for (int x = 0; x < spawnValues.Length; x++)
        {
            if (x == 0)
                spawnThresholds[x] = spawnValues[x];
            else
                spawnThresholds[x] = spawnThresholds[x - 1] + (spawnValues[x] > 0 ? spawnValues[x] : 0);
        }
    }

    void HandleSpecialAttackTimer()
    {
        specialAttackTimer -= Time.deltaTime;
        if (specialAttackTimer <= 0)
        {
            specialAttackTimer = Mathf.Max(specialAttackTimerMin, (specialAttackTimerMax - difficultyLevel));

            if (gameHasBoss && (difficultyLevel == bossSpawnDifficulty || difficultyLevel == (bossSpawnDifficulty + 5)))
            {
                SpawnBoss();
                return;
            }

            EnemySpecialAttackPatterns specialNum = (EnemySpecialAttackPatterns)Random.Range(0, (int)EnemySpecialAttackPatterns.Digs);
            if (difficultyLevel > 3 && difficultyLevel <= 5)
                specialNum = (EnemySpecialAttackPatterns)Random.Range(0, (int)EnemySpecialAttackPatterns.Planes);
            else if (difficultyLevel > 5 && difficultyLevel <= 7)
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
                digSpawnsRemaining = 2;
            }
            else if (specialNum == EnemySpecialAttackPatterns.Planes)
            {
                planeSpawnsRemaining = Random.Range(2, 4);
            }
            else if (specialNum == EnemySpecialAttackPatterns.Rovers)
            {
                roverSpawnsRemaining = Random.Range(2, 4);
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
            SpawnEnemies(10 + (int)((float)difficultyLevel * 1.5f), true);
        }
        FBIspawnTimer -= Time.deltaTime;
        if (FBIspawnTimer <= 0)
        {
            FBIspawnTimer = FBIspawnTimerMax;
            SpawnFBI(true);
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

    void SpawnFBI(bool fullArea)
    {
        SpawnEnemy(Globals.EnemyTypes.FBI, fullArea);
    }

    void SpawnEnemies(int num, bool fullArea)
    {
        numSpawns++;
        for (int x = 0; x < num; x++)
        {
            Globals.EnemyTypes enemyType = Globals.EnemyTypes.Yar;
            float randVal = Random.Range(0, 100f);
            if (randVal > 98f && difficultyLevel > 3)
                enemyType = Globals.EnemyTypes.Scientist;
            else if (randVal > 90f && difficultyLevel > 4)
                enemyType = GetEnemyType(currentChaoticEnemyMaxSpawn, chaoticEnemySpawnThresholds, Globals.ChaoticEnemyTypes);
            else if (randVal > 25)
                enemyType = GetEnemyType(currentFastEnemyMaxSpawn, fastEnemySpawnThresholds, Globals.FastEnemyTypes);
            else if (randVal > 10f)
                enemyType = GetEnemyType(currentStrongEnemyMaxSpawn, strongEnemySpawnThresholds, Globals.StrongEnemyTypes);
            else
                enemyType = GetEnemyType(currentSurroundEnemyMaxSpawn, surroundEnemySpawnThresholds, Globals.SurroundEnemyTypes);

            SpawnEnemy(enemyType, fullArea);
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

    void SpawnEnemy(Globals.EnemyTypes enemyType, bool fullArea)
    {
        // Debug.Log(Globals.currrentNumEnemies);
        if (Globals.currrentNumEnemies >= Globals.maxEnemies && enemyType != Globals.EnemyTypes.FBI)
            return;

        Vector2 enemyPos;
        if (fullArea)
        {
            float horizontalPlayerSpace = 19f;
            float verticalPlayerSpace = 11f;
            float xMin = LeftWall.position.x + 1.5f;
            float xMax = RightWall.position.x - 1.5f;
            float yMin = BottomWall.position.y + 1.5f;
            float yMax = TopWall.position.y - 1.5f;
            float rawX = Random.Range(0, xMax - xMin - horizontalPlayerSpace);
            float rawY = Random.Range(0, yMax - yMin - verticalPlayerSpace);
            Vector2 playerPos = Player.transform.localPosition;
            float xPos = xMin + rawX;
            float yPos = yMin + rawY;
            if ((xMin + rawX) > (playerPos.x - 9.5f) && (yMin + rawY) > (playerPos.y - 5.5f))
            {
                xPos = xPos + horizontalPlayerSpace;
                yPos = yPos + verticalPlayerSpace;
            }
            enemyPos = new Vector2(xPos, yPos);
        }
        else
        {
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
            enemyPos = new Vector2(playerPos.x + xOffset, playerPos.y + yOffset);
        }
        ActivateEnemyFromPool(enemyPos, enemyType, false);
        Globals.currrentNumEnemies++;
    }

    void SpawnBoss()
    {
        Vector2 enemyPos = new Vector3(Player.transform.localPosition.x - 4f, Player.transform.localPosition.y + 10f, Player.transform.localPosition.z);
        ActivateEnemyFromPool(enemyPos, currentBossType, false);
    }

    public void KillBoss()
    {
        bossDefeated = true;
    }

    void SpawnDigs()
    {
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
                ActivateEnemyFromPool(enemyPos, Globals.EnemyTypes.Dig, (x <= 1 || x >= 10));
            }
        }
    }

    void SpawnPlanes()
    {
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
                ActivateEnemyFromPool(enemyPos, Globals.EnemyTypes.Plane, false);
            }
        }
    }

    void SpawnRovers()
    {
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
                ActivateEnemyFromPool(enemyPos, Globals.EnemyTypes.Moon, false);
            }
        }
        startX = playerPos.x + 6f;
        startY = playerPos.y - 5f;
        for (int x = 0; x < numCols; x++)
        {
            for (int y = 0; y < numRows; y++)
            {
                Vector2 enemyPos = new Vector2(startX + x * 4.5f, startY + y * 2f);
                ActivateEnemyFromPool(enemyPos, Globals.EnemyTypes.Moon, true);
            }
        }
    }

    Globals.EnemyTypes GetEnemyType(int currentEnemyMaxSpawn, int[] enemySpawnThresholds, Globals.EnemyTypes[] enemyTypes)
    {
        int randVal = Random.Range(0, currentEnemyMaxSpawn);
        int index = 0;
        int x = 0;
        bool valid = false;
        do {
            index = x;
            if (randVal < enemySpawnThresholds[x])
                valid = true;
            x++;
        } while (!valid && x < enemySpawnThresholds.Length);
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

            float upgradeCompletionBarWidth = 54f;
            for (int x = 0; x < Globals.CurrentUpgradeTypes.Count; x++)
            {
                if (Globals.CurrentUpgradeTypes[x] == availableUpgrades[upgradeNum])
                {
                    HUDUpgradeCompletionBoxes[x].sizeDelta = new Vector2 (
                        (float)(Globals.CurrentUpgradeLevels[(int)availableUpgrades[upgradeNum]]) / (float)Globals.MaxLevelsPerUpgrade * upgradeCompletionBarWidth,
                        HUDUpgradeCompletionBoxes[x].sizeDelta.y);
                }
            }
        }
        HUDUpgradePanel.GetComponent<MoveWhenPaused>().MoveDown();
        playerScript.ResetHUDPhone();
        playerScript.UpdateUpgrades(availableUpgrades[upgradeNum]);
        totalUpgradesThisGame++;
        if (totalUpgradesThisGame % 5 == 0)
            FBIspawnTimerMax = FBIspawnTimerMax + 1f;
        if (totalUpgradesThisGame == 30)
            FBIspawnTimerMax = FBIspawnTimerMax + 5f; // make FBI much less frequent once they max out upgrades
        Time.timeScale = 1f;
        Globals.IsPaused = false;
    }

    public void ShowUpgradeSelection()
    {
        upgradeHighlightIndex = 1;
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
        for (int x = 0; x < HUDUpgradeButtons.Length; x++)
        {
            if (x == upgradeHighlightIndex)
            {
                HUDUpgradeButtons[x].sizeDelta = new Vector2 (410f, 350f);
            }
            else
            {
                HUDUpgradeButtons[x].sizeDelta = new Vector2 (380f, 320f);
            }
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
        Globals.UpdateGamesPlayed(Globals.GamesPlayed + 1);
        Globals.UpdateBestTime((int)Globals.gameTime);

        // check for unlocked characters
        Globals.ResetUnlockedCharacterList();
        if (Globals.GamesPlayed >= 1 && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Atari2600] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Atari2600);
        if (Globals.GamesPlayed >= 2 && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Goth] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Goth);
        if (Globals.GamesPlayed >= 5 && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Miami] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Miami);
        if (Globals.GamesPlayed >= 10 && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Punk] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Punk);
        if (Globals.GamesPlayed >= 20 && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.New] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.New);
        if (Globals.GamesPlayed >= 30 && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Bubblegum] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Bubblegum);
        if (Globals.GamesPlayed >= 50 && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Electro] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Electro);
        if (Globals.gameTime >= 600 && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Hulk] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Hulk);
        if (Globals.gameTime >= 1200 && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Super] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Super);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Laser] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.RadStyle] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.RadStyle);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.SeekerMissile] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Ninja] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Ninja);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Swirl] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Crush] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Crush);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Surround] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Grape] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Grape);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Bomb] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Bomber] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Bomber);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Slime] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Toxic] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Toxic);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Invader] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Invader] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Invader);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Ghost] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Ghost] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Ghost);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Boomerang] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Croc] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Croc);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Breakout] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Smurf] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Smurf);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Tornado] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Rain] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Rain);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Bees] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Bees] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Bees);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.SpreadShot] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Commando] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Commando);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.ForceField] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Tron] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Tron);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Speed] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Flash] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Flash);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.ICBM] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Missile] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Missile);

        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Pit] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Karate] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Karate);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Attack] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Macho] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Macho);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Defense] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Hulk2] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Hulk2);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.RearShot] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Snake] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Snake);
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.SideShot] >= Globals.MaxUpgradeLevel && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Rambo] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Rambo);

        if (bossDefeated && currentBossType == Globals.EnemyTypes.PacBoss && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Pac] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Pac);
        if (bossDefeated && currentBossType == Globals.EnemyTypes.PopeyeBoss && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Sailor] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Sailor);
        if (bossDefeated && currentBossType == Globals.EnemyTypes.MarioBoss && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Mario] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Mario);
        if (bossDefeated && currentBossType == Globals.EnemyTypes.LuigiBoss && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Luigi] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Luigi);
        if (bossDefeated && currentBossType == Globals.EnemyTypes.KoolBoss && Globals.CharacterUnlockStates[(int)Globals.PlayerTypes.Koolaid] == 0)
            Globals.AddUnlockedCharacterToList(Globals.PlayerTypes.Koolaid);

        Globals.UpdateUnlockedCharactersFromList();

    }
}
