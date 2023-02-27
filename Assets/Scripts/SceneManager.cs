using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneManager : MonoBehaviour
{
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
    TextMeshProUGUI[] HUDUpgradeButtonTitleTexts;
    [SerializeField]
    TextMeshProUGUI[] HUDUpgradeButtonLvlTexts;
    [SerializeField]
    TextMeshProUGUI[] HUDUpgradeButtonDescTexts;
    List<Globals.UpgradeTypes> availableUpgrades = new List<Globals.UpgradeTypes>();

    float spawnTimer = 5f;
    float spawnTimerMax = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Globals.maxExperiences = new int[] {100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1200, 1400, 1600, 1800, 2000};
        Globals.healthPerLevel  = new float[] {0, 2, 1, 1, 1, 2, 1, 1, 1, 2, 1, 1, 1, 2, 1, 1, 1 };
        Globals.attackPerLevel = new int[] {0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10 };
        Globals.defensePerLevel = new int[] {0, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10 };
        Globals.shootTimerDecreasePerLevel = new float[] {0, 0, .1f, 0, 0, .1f, 0, 0, .1f, 0, 0, 0, .1f, 0, 0, 0, .1f };
        int numUpgrades = System.Enum.GetValues(typeof(Globals.UpgradeTypes)).Length;
        Globals.CurrentUpgradeLevels = new int[numUpgrades];
        for (int x = 0; x < Globals.CurrentUpgradeLevels.Length; x++)
        {
            Globals.CurrentUpgradeLevels[x] = 0;
        }
        SpawnEnemies(20);

        playerScript = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleEnemyTimer();
        HandleLevelUpTimer();
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
                LevelUpPanel.GetComponent<MoveNormal>().MoveDown();
            }
        }
    }

    void SpawnEnemies(int num)
    {
        for (int x = 0; x < num; x++)
        {
            float randomAngle = Random.Range(0f, 360f);
            Vector2 normalizedPos = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
            Vector2 scaledNormalizedPos = normalizedPos * Random.Range (7.0f, 9.0f);
            Vector2 playerPos = Player.transform.localPosition;
            Vector2 enemyPos = new Vector2(playerPos.x + scaledNormalizedPos.x, playerPos.y + scaledNormalizedPos.y);
            GameObject enemyGO = Instantiate(EnemyPrefab, enemyPos, Quaternion.identity, EnemyContainer.transform);
            Globals.EnemyTypes enemyType = Globals.EnemyTypes.Yar;
            float randVal = Random.Range(0, 100f);
            if (randVal > 95f)
                enemyType = Globals.EnemyTypes.FBI;
            else if (randVal > 90f)
                enemyType = Globals.EnemyTypes.Qbert;
            else if (randVal > 85f)
                enemyType = Globals.EnemyTypes.MsPac;
            else if (randVal > 75f)
                enemyType = Globals.EnemyTypes.Pac;
            enemyGO.GetComponent<Enemy>().ConfigureEnemy(enemyType);
        }
    }

    public void SelectUpgrade(int upgradeNum)
    {
        Globals.CurrentUpgradeLevels[(int)availableUpgrades[upgradeNum]]++;
        HUDUpgradePanel.GetComponent<MoveWhenPaused>().MoveDown();
        playerScript.ResetHUDPhone();
        playerScript.UpdateUpgrades();
        Time.timeScale = 1f;

    }
    public void ShowUpgradeSelection()
    {
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
            HUDUpgradeButtonLvlTexts[x].text = "Lvl " + (Globals.CurrentUpgradeLevels[(int)availableUpgrades[x]] + 1);
            HUDUpgradeButtonDescTexts[x].text = Globals.UpgradeDescriptionText[(int)availableUpgrades[x] * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)availableUpgrades[x]]];
        }

        HUDUpgradePanel.GetComponent<MoveWhenPaused>().MoveUp();
        Time.timeScale = 0f;
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

            LevelUpPanel.GetComponent<MoveNormal>().MoveUp();
            levelUpTimer = levelUpTimerMax;

            playerScript.RestoreMaxHealth();
        }
        float maxExpBarWidth = 400f;
        ExpBar.sizeDelta = new Vector2 ((float)(Globals.currentExp) / maxExp * maxExpBarWidth, ExpBar.sizeDelta.y);
    }
}
