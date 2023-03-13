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

    enum EnemySpecialAttackPatterns {
        VerticalMove,
        HorizontalMove,
        Digs,
        Planes,
        None
    }
    EnemySpecialAttackPatterns currentEnemySpecialAttack = EnemySpecialAttackPatterns.None;

    int[] fastEnemySpawnRates = { 80, 100, 999, 999, 999 };
    int[] strongEnemySpawnRates = { 95, 100, 999, 999, 999 };
    int currentNumFBI = 0;
    int currentNumScientist = 0;
    int maxFBI = 3;
    int maxScientist = 1;

    float spawnTimer = 5f;
    float spawnTimerMax = 7f;
    int digSpawnsRemaining = 0;
    int planeSpawnsRemaining = 0;
    float tankReturnTimer = 0;

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

        fadeManager.StartFadeIn();
        fadeIn = true;
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

        SpawnEnemies(10, true);

        playerScript = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleFadeOut();
        HandleDifficultyTimer();
        HandleEnemyTimer();
        HandleLevelUpTimer();
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

            if (difficultyLevel == 1)
            {
                fastEnemySpawnRates = new int[] { 50, 100, 999, 999, 999 };
                strongEnemySpawnRates = new int[] { 80, 100, 999, 999, 999 };
            }
            else if (difficultyLevel == 2)
            {
                fastEnemySpawnRates = new int[] { 20, 100, 999, 999, 999 };
                strongEnemySpawnRates = new int[] { 60, 100, 999, 999, 999 };
            }
            else if (difficultyLevel == 3)
            {
                fastEnemySpawnRates = new int[] { 10, 80, 100, 999, 999 };
                strongEnemySpawnRates = new int[] { 40, 95, 100, 999, 999 };
            }
            else if (difficultyLevel == 4)
            {
                fastEnemySpawnRates = new int[] { 5, 55, 100, 999, 999 };
                strongEnemySpawnRates = new int[] { 20, 95, 100, 999, 999 };
            }
            else if (difficultyLevel == 5)
            {
                fastEnemySpawnRates = new int[] { 5, 25, 100, 999, 999 };
                strongEnemySpawnRates = new int[] { 20, 80, 100, 999, 999 };
            }
            else if (difficultyLevel == 6)
            {
                fastEnemySpawnRates = new int[] { 5, 10, 80, 100, 999 };
                strongEnemySpawnRates = new int[] { 20, 60, 100, 999, 999 };
            }
            else if (difficultyLevel == 7)
            {
                fastEnemySpawnRates = new int[] { 5, 10, 60, 100, 999 };
                strongEnemySpawnRates = new int[] { 20, 40, 95, 100, 999 };
            }
            else if (difficultyLevel == 8)
            {
                fastEnemySpawnRates = new int[] { 5, 10, 30, 100, 999 };
                strongEnemySpawnRates = new int[] { 20, 40, 80, 100, 999 };
            }
            else if (difficultyLevel == 9)
            {
                fastEnemySpawnRates = new int[] { 5, 10, 20, 90, 100 };
                strongEnemySpawnRates = new int[] { 20, 40, 60, 100, 999 };
            }
            else if (difficultyLevel == 10)
            {
                fastEnemySpawnRates = new int[] { 5, 10, 20, 70, 100 };
                strongEnemySpawnRates = new int[] { 15, 30, 45, 95, 100 };
            }
            else if (difficultyLevel == 11)
            {
                fastEnemySpawnRates = new int[] { 5, 10, 20, 50, 100 };
                strongEnemySpawnRates = new int[] { 10, 20, 30, 90, 100 };
            }
            else if (difficultyLevel == 12)
            {
                fastEnemySpawnRates = new int[] { 5, 10, 20, 40, 100 };
                strongEnemySpawnRates = new int[] { 5, 15, 25, 80, 100 };
            }
            else if (difficultyLevel == 13)
            {
                fastEnemySpawnRates = new int[] { 5, 10, 20, 30, 100 };
                strongEnemySpawnRates = new int[] { 5, 10, 20, 60, 100 };
            }
            else if (difficultyLevel == 14)
            {
                fastEnemySpawnRates = new int[] { 5, 10, 20, 30, 100 };
                strongEnemySpawnRates = new int[] { 5, 10, 20, 40, 100 };
            }
            // check if it is time for an enemy special attack
            if (difficultyLevel % 3 == 0)
            {
                EnemySpecialAttackPatterns specialNum = (EnemySpecialAttackPatterns)Random.Range(0, (int)EnemySpecialAttackPatterns.Planes);
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
                }
                else if (specialNum == EnemySpecialAttackPatterns.Digs)
                {
                    digSpawnsRemaining = Random.Range(2, 5);
                }
                else if (specialNum == EnemySpecialAttackPatterns.Planes)
                {
                    planeSpawnsRemaining = Random.Range(2, 5);
                }
            }
        }
    }

    void HandleEnemyTimer()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            spawnTimer = spawnTimerMax;
            SpawnEnemies(10 + difficultyLevel * 2, false);
        }

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

    void HandleLevelUpTimer()
    {
        if (levelUpTimer > 0)
        {
            levelUpTimer -= Time.deltaTime;
            if (levelUpTimer < 0)
            {
                LevelUpPanel.GetComponent<MoveWhenPaused>().MoveDown();
            }
        }
    }

    void SpawnEnemies(int num, bool FBIrequired)
    {
        int numFBIthisSpawn = 0;
        int maxSpecialPerSpawn = difficultyLevel >= 10 ? 1 : 2;
        int specialThisSpawn = 0;
        float extraLife = 0;
        for (int x = 0; x < num; x++)
        {
            Globals.EnemyTypes enemyType = Globals.EnemyTypes.Yar;
            float randVal = Random.Range(0, 100f);
            if (randVal > 95f && currentNumFBI < maxFBI && specialThisSpawn < maxSpecialPerSpawn)
            {
                enemyType = Globals.EnemyTypes.FBI;
                currentNumFBI++;
                numFBIthisSpawn++;
                specialThisSpawn++;
                extraLife = difficultyLevel * .5f;
            }
            else if (randVal > 92f && currentNumScientist < maxScientist && specialThisSpawn < maxSpecialPerSpawn && difficultyLevel > 3)
            {
                enemyType = Globals.EnemyTypes.Scientist;
                currentNumScientist++;
                specialThisSpawn++;
                extraLife = difficultyLevel * .4f;
            }
            else if (randVal > 82f)
                enemyType = GetStrongEnemyType();
            else
                enemyType = GetFastEnemyType();
            SpawnEnemy(enemyType, extraLife);
        }

        if (FBIrequired && numFBIthisSpawn == 0)
        {
            SpawnEnemy(Globals.EnemyTypes.FBI, difficultyLevel * .5f);
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
    }

    void SpawnEnemy(Globals.EnemyTypes enemyType, float extraLife)
    {
        // Debug.Log(Globals.currrentNumEnemies);
        if (Globals.currrentNumEnemies >= Globals.maxEnemies)
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
        GameObject enemyGO = Instantiate(EnemyPrefab, enemyPos, Quaternion.identity, EnemyContainer.transform);
        enemyGO.GetComponent<Enemy>().ConfigureEnemy(enemyType, extraLife);
        Globals.currrentNumEnemies++;
    }

    void SpawnDigs()
    {
        float extraLife = difficultyLevel * .5f;
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
                GameObject enemyGO = Instantiate(EnemyPrefab, enemyPos, Quaternion.identity, EnemyContainer.transform);
                enemyGO.GetComponent<Enemy>().ConfigureEnemy(Globals.EnemyTypes.Dig, extraLife);
            }
        }
    }

    void SpawnPlanes()
    {
        float extraLife = difficultyLevel * .5f;
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
                GameObject enemyGO = Instantiate(EnemyPrefab, enemyPos, Quaternion.identity, EnemyContainer.transform);
                enemyGO.GetComponent<Enemy>().ConfigureEnemy(Globals.EnemyTypes.Plane, extraLife);
            }
        }
    }

    Globals.EnemyTypes GetFastEnemyType()
    {
        int randVal = Random.Range(0, 100);
        int index = 0;
        int x = 0;
        bool valid = false;
        do {
            index = x;
            if (randVal < fastEnemySpawnRates[x] && fastEnemySpawnRates[x] <= 100)
                valid = true;
            x++;
        } while (!valid && x < fastEnemySpawnRates.Length);
        Globals.EnemyTypes fastEnemyType = Globals.FastEnemyTypes[index];
        return fastEnemyType;
    }

    Globals.EnemyTypes GetStrongEnemyType()
    {
        int randVal = Random.Range(0, 100);
        int index = 0;
        int x = 0;
        bool valid = false;
        do {
            index = x;
            if (randVal < strongEnemySpawnRates[x] && strongEnemySpawnRates[x] <= 100)
                valid = true;
            x++;
        } while (!valid && x < strongEnemySpawnRates.Length);
        Globals.EnemyTypes strongEnemyType = Globals.StrongEnemyTypes[index];
        return strongEnemyType;
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
                Globals.currentMaxHealth += 1f;
            }
            if (Globals.attackPerLevel.Length > Globals.currentLevel && Globals.attackPerLevel[Globals.currentLevel] > 0)
            {
                statsText = statsText + ("ATTACK+" + Globals.attackPerLevel[Globals.currentLevel] + "% ");
                Globals.currentAttack = Globals.currentAttack + (Globals.attackPerLevel[Globals.currentLevel] * .01f);
            }
            if (Globals.defensePerLevel.Length > Globals.currentLevel && Globals.defensePerLevel[Globals.currentLevel] > 0)
            {
                statsText = statsText + ("DEFENSE+" + Globals.defensePerLevel[Globals.currentLevel] + "% ");
                Globals.currentDefense = Globals.currentDefense + (Globals.defensePerLevel[Globals.currentLevel] * .01f);
            }
            if (Globals.shootTimerDecreasePerLevel.Length > Globals.currentLevel && Globals.shootTimerDecreasePerLevel[Globals.currentLevel] > 0)
            {
                statsText = statsText + ("SHOT INTERVAL-" + Globals.shootTimerDecreasePerLevel[Globals.currentLevel]);
                Globals.currentShootTimerMax -= Globals.shootTimerDecreasePerLevel[Globals.currentLevel];
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

    public void KillFBI()
    {
        currentNumFBI--;
    }
    public void KillScientist()
    {
        currentNumScientist--;
    }
}
