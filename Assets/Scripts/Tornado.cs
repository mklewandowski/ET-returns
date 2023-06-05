using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    public void Init(Vector3 centerPos)
    {
        int index = (int)Globals.UpgradeTypes.Bees * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Bees] - 1;
        int enemyHitsAllowed = Globals.UpgradeLevelEnemyHits[index];
        this.GetComponent<Bullet>().SetEnemyHits(enemyHitsAllowed);
        this.transform.localPosition = new Vector2(centerPos.x + Random.Range(9.2f, 9.7f), Random.Range(centerPos.y - 3f, centerPos.y + 3f));
    }

    void Update()
    {
        float newX = transform.localPosition.x + -5f * Time.deltaTime;
        Vector3 newPos = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
        transform.localPosition = newPos;
    }
}
