using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField]
    Sprite[] GhostSprites;
    Vector2 direction;
    float directionChangeTimer;

    public void Init(Vector3 centerPos)
    {
        int index = (int)Globals.UpgradeTypes.Ghost * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Ghost] - 1;
        int enemyHitsAllowed = Globals.UpgradeLevelEnemyHits[index];
        this.GetComponent<Bullet>().SetEnemyHits(enemyHitsAllowed);
        this.GetComponent<SpriteRenderer>().sprite = GhostSprites[Random.Range(0, GhostSprites.Length)];
        ChangeDirection();
    }

    void Update()
    {
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0)
        {
            ChangeDirection();
        }
        float newX = transform.localPosition.x + direction.x * Time.deltaTime;
        float newY = transform.localPosition.y + direction.y * Time.deltaTime;
        Vector3 newPos = new Vector3(newX, newY, transform.localPosition.z);
        transform.localPosition = newPos;
    }

    void ChangeDirection()
    {
        int randVal = Random.Range(0, 4);
        if (randVal == 0)
            direction = new Vector2(3f, 0);
        else if(randVal == 1)
            direction = new Vector2(-3f, 0);
        else if(randVal == 2)
            direction = new Vector2(0, 3f);
        else if(randVal == 3)
            direction = new Vector2(0, -3f);

        directionChangeTimer = Random.Range(.33f, .66f);

    }
}
