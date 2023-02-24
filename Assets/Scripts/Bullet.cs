using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    Globals.BulletTypes type = Globals.BulletTypes.Standard;

    float lifeTimer = 3f;
    int enemyHits = 1;

    [SerializeField]
    GameObject BombShockWavePrefab;
    GameObject bulletContainer;

    void Awake()
    {
        bulletContainer = GameObject.Find("BulletContainer");
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer < 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Init(Globals.BulletTypes newType)
    {
        type = newType;
    }

    public void HitEnemy()
    {
        enemyHits--;

        if (enemyHits <= 0)
        {
            if (type == Globals.BulletTypes.Bomb)
            {
                // create shockwave
                 GameObject shockwaveGO = Instantiate(BombShockWavePrefab, this.transform.position, Quaternion.identity, bulletContainer.transform);
                 shockwaveGO.GetComponent<BombShockWave>().StartEffect();
            }
            Destroy(this.gameObject);
        }
    }
}
