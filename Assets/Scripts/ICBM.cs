using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICBM : MonoBehaviour
{
    public void Init(Vector3 centerPos)
    {
        int index = (int)Globals.UpgradeTypes.ICBM * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.ICBM] - 1;
        int enemyHitsAllowed = Globals.UpgradeLevelEnemyHits[index];
        this.GetComponent<Bullet>().SetEnemyHits(enemyHitsAllowed);
        this.transform.localPosition = new Vector2(Random.Range(centerPos.x - 3f, centerPos.x + 3f), centerPos.y - Random.Range(6f, 6.5f));
    }

    void Update()
    {
        float newY = transform.localPosition.y + 5f * Time.deltaTime;
        Vector3 newPos = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        transform.localPosition = newPos;
    }
}
