using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField]
    GameObject EnemyPrefab;
    [SerializeField]
    GameObject EnemyContainer;

    [SerializeField]
    GameObject Player;

    float spawnTimer = 5f;
    float spawnTimerMax = 5f;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemies(20);
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            spawnTimer = spawnTimerMax;
            SpawnEnemies(10);
        }
    }

    void SpawnEnemies(int num)
    {
        for (int x = 0; x < num; x++)
        {
            float randomAngle = Random.Range(0f, 360f);
            Vector2 normalizedPos = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
            Vector2 playerPos = Player.transform.localPosition;
            Vector2 enemyPos = new Vector2(playerPos.x + normalizedPos.x * Random.Range(4.5f, 7.5f), playerPos.y + normalizedPos.y * Random.Range(5.5f, 7.5f));
            GameObject enemyGO = Instantiate(EnemyPrefab, enemyPos, Quaternion.identity, EnemyContainer.transform);
        }
    }
}
