using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    AudioManager audioManager;
    [SerializeField]
    Globals.BulletTypes type = Globals.BulletTypes.Standard;

    [SerializeField]
    float lifeTimer = 3f;
    [SerializeField]
    int enemyHits = 1;

    [SerializeField]
    GameObject BombShockWavePrefab;
    [SerializeField]
    GameObject SlimeDebrisPrefab;
    int numDebris = 4;
    GameObject bulletContainer;

    void Awake()
    {
        GameObject am = GameObject.Find("AudioManager");
        if (am)
            audioManager = am.GetComponent<AudioManager>();
        bulletContainer = GameObject.Find("BulletContainer");
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Init(Globals.BulletTypes newType)
    {
        type = newType;
    }

    public void SetEnemyHits(int newEnemyHits)
    {
        enemyHits = newEnemyHits;
    }

    public void SetLifeTimer(float newLifeTimer)
    {
        lifeTimer = newLifeTimer;
    }

    public void SetDebrisAmount(int newNumDebris)
    {
        numDebris = newNumDebris;
    }

    public void HitEnemy()
    {
        enemyHits--;

        if (enemyHits <= 0)
        {
            if (type == Globals.BulletTypes.Bomb)
            {
                // create shockwave
                audioManager.PlayExplodeSound();
                GameObject shockwaveGO = Instantiate(BombShockWavePrefab, this.transform.position, Quaternion.identity, bulletContainer.transform);
                shockwaveGO.GetComponent<BombShockWave>().StartEffect();
            }
            else if (type == Globals.BulletTypes.Slime)
            {
                for (int x = 0; x < numDebris; x++)
                {
                    GameObject slimeDebrisGO = Instantiate(SlimeDebrisPrefab, this.transform.localPosition, Quaternion.identity, bulletContainer.transform);
                    slimeDebrisGO.GetComponent<SlimeDebris>().Init();
                }
            }
            Destroy(this.gameObject);
        }
    }
}
