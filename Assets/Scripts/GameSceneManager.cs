using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField]
    FadeManager fadeManager;
    [SerializeField]
    GameObject EnemyPrefab;
    [SerializeField]
    GameObject EnemyContainer;

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
    GameObject HUDUpgradePanel;
    [SerializeField]
    GameObject[] HUDUpgradeButtonHighlights;
    [SerializeField]
    TextMeshProUGUI[] HUDUpgradeButtonTitleTexts;
    [SerializeField]
    TextMeshProUGUI[] HUDUpgradeButtonLvlTexts;
    [SerializeField]
    TextMeshProUGUI[] HUDUpgradeButtonDescTexts;
    List<Globals.UpgradeTypes> availableUpgrades = new List<Globals.UpgradeTypes>();
    int upgradeHighlightIndex = 0;
    bool stickDown = false;
    bool controllerAttached = false;

    float spawnTimer = 5f;
    float spawnTimerMax = 5f;

    float deadTimer = 0f;
    float deadTimerMax = 4f;
    bool fadeIn = false;
    bool fadeOut = false;

    void Awake()
    {
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

        SpawnEnemies(20);

        playerScript = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleEnemyTimer();
        HandleLevelUpTimer();
        HandleInput();
        HandleFadeOut();
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
            if (moveLeft)
                upgradeHighlightIndex = Mathf.Max(0, upgradeHighlightIndex - 1);
            else if (moveRight)
                upgradeHighlightIndex = Mathf.Min(2, upgradeHighlightIndex + 1);
            HighlightUpgradeButton();
        }
    }

    void HandleEnemyTimer()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            spawnTimer = spawnTimerMax;
            SpawnEnemies(10);
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

    void SpawnEnemies(int num)
    {
        for (int x = 0; x < num; x++)
        {
            float randomAngle = Random.Range(0f, 360f);
            Vector2 normalizedPos = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
            Vector2 scaledNormalizedPos = normalizedPos * Random.Range (11.0f, 13.0f);
            Vector2 playerPos = Player.transform.localPosition;
            Vector2 enemyPos = new Vector2(playerPos.x + scaledNormalizedPos.x, playerPos.y + scaledNormalizedPos.y);
            GameObject enemyGO = Instantiate(EnemyPrefab, enemyPos, Quaternion.identity, EnemyContainer.transform);
            Globals.EnemyTypes enemyType = Globals.EnemyTypes.Yar;
            float randVal = Random.Range(0, 100f);
            if (randVal > 95f)
                enemyType = Globals.EnemyTypes.FBI;
            else if (randVal > 90f)
                enemyType = Globals.EnemyTypes.Joust;
            else if (randVal > 85f)
                enemyType = Globals.EnemyTypes.MsPac;
            else if (randVal > 75f)
                enemyType = Globals.EnemyTypes.Pac;
            enemyGO.GetComponent<Enemy>().ConfigureEnemy(enemyType);
        }
    }

    public void SelectUpgrade(int upgradeNum)
    {
        if (availableUpgrades[upgradeNum] == Globals.UpgradeTypes.RefillHP)
        {
            playerScript.RestoreMaxHealth();
        }
        else
        {
            Globals.CurrentUpgradeLevels[(int)availableUpgrades[upgradeNum]]++;
        }
        HUDUpgradePanel.GetComponent<MoveWhenPaused>().MoveDown();
        playerScript.ResetHUDPhone();
        playerScript.UpdateUpgrades();
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
        int maxUpgrades = 5;
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
            }
            else
            {
                HUDUpgradeButtonLvlTexts[x].text = "Lvl " + (Globals.CurrentUpgradeLevels[(int)availableUpgrades[x]] + 1);
                HUDUpgradeButtonDescTexts[x].text = Globals.UpgradeDescriptionText[(int)availableUpgrades[x] * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)availableUpgrades[x]]];
            }
        }

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
                Globals.currentAttack = Globals.currentAttack + (Globals.currentAttack * Globals.attackPerLevel[Globals.currentLevel] * .01f);
            }
            if (Globals.defensePerLevel.Length > Globals.currentLevel && Globals.defensePerLevel[Globals.currentLevel] > 0)
            {
                statsText = statsText + ("DEFENSE+" + Globals.defensePerLevel[Globals.currentLevel] + "% ");
                Globals.currentDefense = Globals.currentDefense + (Globals.currentDefense * Globals.defensePerLevel[Globals.currentLevel] * .01f);
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
        }
        float maxExpBarWidth = 400f;
        ExpBar.sizeDelta = new Vector2 ((float)(Globals.currentExp) / maxExp * maxExpBarWidth, ExpBar.sizeDelta.y);
    }

    public void GameOver()
    {
        deadTimer = deadTimerMax;
    }
}
