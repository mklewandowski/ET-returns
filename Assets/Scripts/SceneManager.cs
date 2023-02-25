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
        Globals.maxExperiences = new int[] {100, 200, 300, 400, 500, 600, 700, 800, 900, 1000};
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
        HUDUpgradePanel.SetActive(false);
        playerScript.ResetHUDPhone();
        playerScript.UpdateUpgrades();
        Time.timeScale = 1f;

    }
    public void ShowUpgradeSelection()
    {
        availableUpgrades.Clear();
        for (int x = 0; x < Globals.CurrentUpgradeLevels.Length; x++)
        {
            if (Globals.CurrentUpgradeLevels[x] <= Globals.MaxUpgradeLevel)
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
            HUDUpgradeButtonDescTexts[x].text = Globals.UpgradeDescriptionText[(int)availableUpgrades[x]];
        }

        HUDUpgradePanel.SetActive(true);
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
            LevelUpPanel.GetComponent<MoveNormal>().MoveUp();
            levelUpTimer = levelUpTimerMax;

            Globals.currentAttack += 1;
            Globals.currentMaxHealth += 1f;
            Globals.currentDefense += 1;
            Globals.currentShootTimerMax -= .1f;

            playerScript.RestoreMaxHealth();
        }
        float maxExpBarWidth = 400f;
        ExpBar.sizeDelta = new Vector2 ((float)(Globals.currentExp) / maxExp * maxExpBarWidth, ExpBar.sizeDelta.y);
    }
}
