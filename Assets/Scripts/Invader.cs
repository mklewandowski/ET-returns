using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    [SerializeField]
    Sprite[] InvaderSprites;

    public void Init(Vector3 centerPos)
    {
        int index = (int)Globals.UpgradeTypes.Invader * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Invader] - 1;
        int enemyHitsAllowed = Globals.UpgradeLevelEnemyHits[index];
        this.GetComponent<Bullet>().SetEnemyHits(enemyHitsAllowed);
        this.GetComponent<SpriteRenderer>().sprite = InvaderSprites[Random.Range(0, InvaderSprites.Length)];
        this.transform.localPosition = new Vector2(Random.Range(centerPos.x - 3f, centerPos.x + 3f), centerPos.y + 6f);
    }

    void Update()
    {
        float newY = transform.localPosition.y + -5f * Time.deltaTime;
        Vector3 newPos = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        transform.localPosition = newPos;
    }
}
