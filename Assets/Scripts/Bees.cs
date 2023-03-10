using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bees : MonoBehaviour
{
    bool startPosIsLeft = false;

    public void Init(Vector3 centerPos, bool isLeft)
    {
        startPosIsLeft = isLeft;
        int index = (int)Globals.UpgradeTypes.Bees * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Bees] - 1;
        int enemyHitsAllowed = Globals.UpgradeLevelEnemyHits[index];
        this.GetComponent<Bullet>().SetEnemyHits(enemyHitsAllowed);
        float enemyLifeTimerAllowed = Globals.UpgradeLevelAttackTimes[index];
        this.GetComponent<Bullet>().SetLifeTimer(enemyLifeTimerAllowed);
        this.transform.localPosition = new Vector2(centerPos.x + (startPosIsLeft ? -9.5f : 9.5f), Random.Range(centerPos.y - 3f, centerPos.y + 3f));
    }

    void Update()
    {
        float newX = transform.localPosition.x + (startPosIsLeft ? 5f : -5f) * Time.deltaTime;
        Vector3 newPos = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
        transform.localPosition = newPos;
    }
}
