using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    bool isActive = false;

    AudioManager audioManager;
    GameSceneManager GameSceneManagerScript;

    Transform LeftWall;
    Transform TopWall;
    Transform RightWall;
    Transform BottomWall;

    int life = 1;
    int hitStrength = 1;

    [SerializeField]
    Globals.EnemyTypes type = Globals.EnemyTypes.Yar;

    [SerializeField]
    Sprite[] EnemySprites;
    private SpriteRenderer enemyRenderer;
    private Animator enemyAnimator;

    private BoxCollider2D enemyCollider;
    private Rigidbody2D enemyRigidbody;
    float moveSpeed = .5f;
    Vector3 movementVector = new Vector3(0, 0, 0);
    Vector3 desiredPosition = new Vector3(0, 0, 0);
    bool allowImpactVelocity = true;
    Vector3 impactVector = new Vector3(0, 0, 0);
    float impactTimer = 0f;
    float impactTimerMax = .1f;
    bool flipWithMovement = false;

    Transform playerTransform;
    float positionTimer = .1f;
    float positionTimerMax = .5f;

    [SerializeField]
    Material FlashMaterial;
    Material enemyMaterial;
    float flashTimer = 0f;
    float flashTimerMax = .10f;

    float lifeTimer = 0f;
    bool useLifeTimer = false;

    [SerializeField]
    GameObject DebrisPrefab;
    GameObject debrisContainer;
    [SerializeField]
    GameObject ToxicDebrisPrefab;

    float pauseBeforeAction = 2f;

    enum AttackPattern {
        Direct,
        Angle,
        Alternate
    }
    AttackPattern attackPattern = AttackPattern.Direct;
    bool target = false;

    public void Init()
    {
        GameObject am = GameObject.Find("AudioManager");
        if (am)
            audioManager = am.GetComponent<AudioManager>();
        GameSceneManagerScript = GameObject.Find("SceneManager").GetComponent<GameSceneManager>();
        playerTransform = GameObject.Find("Player").transform;
        debrisContainer = GameObject.Find("DebrisContainer");
        LeftWall = GameObject.Find("EnemySolidLeft").transform;
        TopWall = GameObject.Find("EnemySolidTop").transform;
        RightWall = GameObject.Find("EnemySolidRight").transform;
        BottomWall = GameObject.Find("EnemySolidBottom").transform;
        enemyAnimator = GetComponent<Animator>();
        enemyCollider = GetComponent<BoxCollider2D>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        enemyMaterial = enemyRenderer.material;
    }

    public void DeActivate()
    {
        isActive = false;
        this.gameObject.SetActive(false);
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void ConfigureEnemy(Vector3 pos, Globals.EnemyTypes newType, int extraLife, bool flip)
    {
        flashTimer = 0;
        impactTimer = 0f;
        positionTimer = .1f;
        enemyRenderer.material = enemyMaterial;

        this.transform.localPosition = pos;
        enemyRenderer.enabled = false;
        enemyAnimator.enabled = false;

        type = newType;
        enemyRenderer.sprite = EnemySprites[(int)type];
        enemyRigidbody.mass = 1f;
        pauseBeforeAction = 0f;
        useLifeTimer = false;
        allowImpactVelocity = true;
        attackPattern = AttackPattern.Direct;
        this.gameObject.SetActive(true);

        int defaultCollisionLayer = LayerMask.NameToLayer("Enemy");
        gameObject.layer = defaultCollisionLayer;

        // FAST
        if (type == Globals.EnemyTypes.Yar)
        {
            moveSpeed = Random.Range(.9f, 1.2f);
            positionTimerMax = .75f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("yar");
            this.transform.localScale = new Vector3(6f, 6f, 1f);
            enemyCollider.size = new Vector2(0.08f, 0.08f);
            flipWithMovement = true;
            life = 2;
            hitStrength = 2;
        }
        else if (type == Globals.EnemyTypes.Pac)
        {
            moveSpeed = Random.Range(1.2f, 1.5f);
            positionTimerMax = .75f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("pac");
            this.transform.localScale = new Vector3(4f, 4f, 1f);
            enemyCollider.size = new Vector2(0.14f, 0.1f);
            flipWithMovement = true;
            life = 4;
            hitStrength = 4;
        }
        else if (type == Globals.EnemyTypes.MsPac)
        {
            moveSpeed = Random.Range(1.5f, 1.8f);
            positionTimerMax = .5f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("mspac");
            this.transform.localScale = new Vector3(4f, 4f, 1f);
            enemyCollider.size = new Vector2(0.14f, 0.1f);
            flipWithMovement = true;
            life = 6;
            hitStrength = 5;
        }
        else if (type == Globals.EnemyTypes.Joust)
        {
            moveSpeed = Random.Range(1.8f, 2.1f);
            positionTimerMax = .4f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("joust");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.15f, 0.15f);
            flipWithMovement = true;
            life = 8;
            hitStrength = 7;
        }
        else if (type == Globals.EnemyTypes.Joust2)
        {
            moveSpeed = Random.Range(1.9f, 2.2f);
            positionTimerMax = .3f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("joust2");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.15f, 0.15f);
            flipWithMovement = true;
            life = 10;
            hitStrength = 10;
        }

        else if (type == Globals.EnemyTypes.Yar2)
        {
            moveSpeed = Random.Range(2.3f, 2.5f);
            positionTimerMax = .75f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("yar2");
            this.transform.localScale = new Vector3(6f, 6f, 1f);
            enemyCollider.size = new Vector2(0.08f, 0.08f);
            flipWithMovement = true;
            life = 10;
            hitStrength = 12;
        }

        // SURROUND
        else if (type == Globals.EnemyTypes.Frogger)
        {
            moveSpeed = Random.Range(.6f, .8f);
            positionTimerMax = 2f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("frog");
            this.transform.localScale = new Vector3(6f, 6f, 1f);
            enemyCollider.size = new Vector2(0.1f, 0.1f);
            flipWithMovement = true;
            life = 5;
            hitStrength = 5;
            attackPattern = AttackPattern.Angle;
        }
        else if (type == Globals.EnemyTypes.Indy)
        {
            moveSpeed = Random.Range(1.6f, 1.8f);
            positionTimerMax = 2f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("indy");
            this.transform.localScale = new Vector3(8f, 8f, 1f);
            enemyCollider.size = new Vector2(0.05f, 0.1f);
            flipWithMovement = true;
            life = 10;
            hitStrength = 8;
            attackPattern = AttackPattern.Angle;
        }
        else if (type == Globals.EnemyTypes.Pengo)
        {
            moveSpeed = Random.Range(1.3f, 1.6f);
            positionTimerMax = 2f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("pengo");
            this.transform.localScale = new Vector3(4f, 4f, 1f);
            enemyCollider.size = new Vector2(0.3f, 0.07f);
            flipWithMovement = true;
            life = 20;
            hitStrength = 12;
            attackPattern = AttackPattern.Angle;
        }

        // STRONG
        else if (type == Globals.EnemyTypes.Qbert)
        {
            moveSpeed = Random.Range(.8f, 1f);
            positionTimerMax = 1f;
            attackPattern = AttackPattern.Alternate;
            enemyAnimator.enabled = false;
            this.transform.localScale = new Vector3(4f, 4f, 1f);
            enemyCollider.size = new Vector2(0.15f, 0.15f);
            flipWithMovement = true;
            life = 10;
            hitStrength = 8;
        }
        else if (type == Globals.EnemyTypes.Kangaroo)
        {
            moveSpeed = Random.Range(1f, 1.2f);
            positionTimerMax = .9f;
            attackPattern = AttackPattern.Alternate;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("kangaroo");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.12f, 0.2f);
            flipWithMovement = true;
            life = 20;
            hitStrength = 12;
        }
        else if (type == Globals.EnemyTypes.Bear)
        {
            moveSpeed = Random.Range(1.8f, 2.1f);
            positionTimerMax = .7f;
            attackPattern = AttackPattern.Alternate;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("bear");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.12f, 0.12f);
            flipWithMovement = true;
            life = 28;
            hitStrength = 14;
        }
        else if (type == Globals.EnemyTypes.Hero)
        {
            moveSpeed = Random.Range(1.2f, 1.4f);
            positionTimerMax = .6f;
            attackPattern = AttackPattern.Alternate;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("hero");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.12f, 0.2f);
            flipWithMovement = true;
            life = 36;
            hitStrength = 16;
        }
        else if (type == Globals.EnemyTypes.Hero2)
        {
            moveSpeed = Random.Range(1.4f, 1.6f);
            positionTimerMax = .4f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("hero2");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.12f, 0.2f);
            flipWithMovement = true;
            life = 40;
            hitStrength = 18;
        }

        // SPECIAL
        else if (type == Globals.EnemyTypes.Dig)
        {
            moveSpeed = Random.Range(.6f, .8f);
            positionTimerMax = .9f;
            enemyAnimator.enabled = false;
            this.transform.localScale = new Vector3(.1f, .1f, 1f);
            enemyCollider.size = new Vector2(0.1f, 0.1f);
            flipWithMovement = true;
            life = 6;
            hitStrength = 6;
            pauseBeforeAction = 2f;
            this.GetComponent<GrowAndShrink>().StartEffect();
        }
        else if (type == Globals.EnemyTypes.Plane)
        {
            moveSpeed = Random.Range(.6f, .8f);
            enemyAnimator.enabled = false;
            this.transform.localScale = new Vector3(.1f, .1f, 1f);
            enemyCollider.size = new Vector2(0.1f, 0.1f);
            life = 8;
            hitStrength = 10;
            pauseBeforeAction = 2f;
            this.GetComponent<GrowAndShrink>().StartEffect();
            int collisionLayer = LayerMask.NameToLayer("EnemyPlane");
            gameObject.layer = collisionLayer;
            useLifeTimer = true;
            lifeTimer = 15f;
            allowImpactVelocity = false;
            movementVector = new Vector3(5f, 0, 0);
        }
        else if (type == Globals.EnemyTypes.Moon)
        {
            moveSpeed = Random.Range(1.3f, 1.6f);
            enemyAnimator.enabled = true;
            enemyAnimator.Play("moon");
            this.transform.localScale = new Vector3(.1f, .1f, 1f);
            enemyCollider.size = new Vector2(0.3f, 0.07f);
            life = 10;
            hitStrength = 15;
            pauseBeforeAction = 2f;
            this.GetComponent<GrowAndShrink>().StartEffect();
            int collisionLayer = LayerMask.NameToLayer("EnemyPlane");
            gameObject.layer = collisionLayer;
            useLifeTimer = true;
            lifeTimer = 15f;
            allowImpactVelocity = false;
            movementVector = new Vector3(flip ? -5.5f : 5.5f, 0, 0);
            if (flip)
            {
                this.transform.localEulerAngles = new Vector3(0, 180f, 0);
            }
        }
        else if (type == Globals.EnemyTypes.FBI)
        {
            moveSpeed = Random.Range(1.75f, 2.25f);
            positionTimerMax = 4f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("fbi");
            this.transform.localScale = new Vector3(4f, 4f, 1f);
            enemyCollider.size = new Vector2(0.15f, 0.3f);
            flipWithMovement = true;
            life = 2;
            hitStrength = 5;
            enemyRigidbody.mass = 999f;
            int collisionLayer = LayerMask.NameToLayer("EnemySpecial");
            gameObject.layer = collisionLayer;
        }
        else if (type == Globals.EnemyTypes.Scientist)
        {
            moveSpeed = Random.Range(2f, 2.5f);
            positionTimerMax = 4f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("scientist");
            this.transform.localScale = new Vector3(4f, 4f, 1f);
            enemyCollider.size = new Vector2(0.15f, 0.3f);
            flipWithMovement = true;
            life = 5;
            hitStrength = 10;
            enemyRigidbody.mass = 999f;
            int collisionLayer = LayerMask.NameToLayer("EnemySpecial");
            gameObject.layer = collisionLayer;
        }

        // BOSS
        else if (type == Globals.EnemyTypes.PacBoss)
        {
            moveSpeed = .5f;
            positionTimerMax = 4f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("pac");
            this.transform.localScale = new Vector3(15f, 15f, 1f);
            enemyCollider.size = new Vector2(0.14f, 0.1f);
            flipWithMovement = true;
            life = 100;
            hitStrength = 10;
        }
        life = life + extraLife;
        enemyCollider.enabled = true;
        enemyRenderer.enabled = true;
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleFlash();
        HandleImpact();
        HandleLifeTimer();
    }

    private void HandleMovement()
    {
        if (pauseBeforeAction > 0)
        {
            pauseBeforeAction -= Time.deltaTime;
            return;
        }
        if (positionTimer > 0)
        {
            positionTimer -= Time.deltaTime;
            if (positionTimer <= 0)
            {
                if (type != Globals.EnemyTypes.Moon && type != Globals.EnemyTypes.Plane)
                {
                    if (type == Globals.EnemyTypes.FBI || type == Globals.EnemyTypes.Scientist)
                    {
                        UpdateFBIOrScientistMovement();
                    }
                    else
                    {
                        // float distanceFromDesiredPosition = Mathf.Abs(Vector3.Distance(desiredPosition, this.transform.localPosition));
                        // int distanceUnitsAway = (int)(distanceFromDesiredPosition / 2f);
                        desiredPosition = playerTransform.position;
                        // movementVector = Quaternion.Euler(0, 0, Random.Range (-20f * distanceUnitsAway, 20f * distanceUnitsAway)) * movementVector;
                        movementVector = (desiredPosition - this.transform.localPosition).normalized * moveSpeed;
                    }
                    if (attackPattern == AttackPattern.Angle)
                    {
                        movementVector = Quaternion.Euler(0, 0, Random.Range (50f, 50f)) * movementVector;
                    }
                    else if (attackPattern == AttackPattern.Alternate)
                    {
                        target = !target;
                        if (!target)
                            movementVector = Quaternion.Euler(0, 0, Random.Range (50f, 50f)) * movementVector;
                    }
                }
                positionTimer = Random.Range(positionTimerMax - .25f, positionTimerMax + .25f);
            }
        }
        if (flipWithMovement)
        {
            if ((movementVector.x >= 0 && this.transform.localScale.x < 0) || (movementVector.x < 0 && this.transform.localScale.x > 0))
                this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
        }
        if (type == Globals.EnemyTypes.FBI || type == Globals.EnemyTypes.Scientist)
        {
            CheckFBIOrScientistPosition();
        }
        enemyRigidbody.velocity = impactTimer > 0 ? impactVector : movementVector;
    }

    private void UpdateFBIOrScientistMovement()
    {
        desiredPosition = new Vector3(
            playerTransform.position.x + 6f * (Random.Range(0, 2) == 0 ? -1f : 1f),
            playerTransform.position.y + 4f * (Random.Range(0, 2) == 0 ? -1f : 1f),
            0
        );
        movementVector = (desiredPosition - this.transform.localPosition).normalized * moveSpeed;
    }
    private void CheckFBIOrScientistPosition()
    {
        float xBuffer = 2f;
        float yBuffer = 2.2f;
        if ((this.transform.localPosition.x + xBuffer >= RightWall.position.x && movementVector.x > 0) ||
            (this.transform.localPosition.x - xBuffer <= LeftWall.position.x && movementVector.x < 0) ||
            (this.transform.localPosition.y + yBuffer >= TopWall.position.y && movementVector.y > 0) ||
            (this.transform.localPosition.y - yBuffer <= BottomWall.position.y && movementVector.y < 0))
        {
            UpdateFBIOrScientistMovement();
        }
    }

    private void HandleFlash()
    {
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0)
            {
                enemyRenderer.material = enemyMaterial;
            }
        }
    }

    private void HandleImpact()
    {
        if (impactTimer > 0)
        {
            impactTimer -= Time.deltaTime;
        }
    }

    private void HandleLifeTimer()
    {
        if (!useLifeTimer)
            return;
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            DeActivate();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isActive) return;

        Player player = collider.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.HitPlayer(hitStrength);
        }

        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        AttackObject attackObject = collider.gameObject.GetComponent<AttackObject>();
        int damage = 0;
        Vector2 damageVelocity = new Vector2(0, 0);
        if (bullet != null)
        {
            Rigidbody2D bulletRB = collider.gameObject.GetComponent<Rigidbody2D>();
            if (bulletRB != null)
                damageVelocity = collider.gameObject.GetComponent<Rigidbody2D>().velocity;
            bullet.HitEnemy();
        }
        if (attackObject)
        {
            audioManager.PlayEnemyHitSound();
            float currentAttackPercent = (float)Globals.currentAttack / (float)Globals.maxAttack;
            float randVal = Random.Range(0, 100f);
            if (randVal < 75f)
                damage = (int)Mathf.Round(attackObject.NormalDamageMin + (attackObject.NormalDamageMax - attackObject.NormalDamageMin) * currentAttackPercent);
            else if (randVal < 95f)
                damage = (int)Mathf.Round(attackObject.StrongDamageMin + (attackObject.StrongDamageMax - attackObject.StrongDamageMin) * currentAttackPercent);
            else
                damage = (int)Mathf.Round(attackObject.CriticalDamageMin + (attackObject.CriticalDamageMax - attackObject.CriticalDamageMin) * currentAttackPercent);
        }
        life = life - damage;
        if (damage > 0)
            GameSceneManagerScript.ActivateHitNoticeFromPool(this.gameObject.transform.position, damage);

        if (life <= 0)
            KillEnemy();
        else if (damage > 0)
            DamageEnemy(damageVelocity);
    }

    public void KillEnemy()
    {
        Globals.killCount++;
        Globals.currrentNumEnemies--;

        // create debris
        int numDebris = Random.Range(8, 10);
        for (int x = 0; x < numDebris; x++)
        {
            GameObject debrisGO = Instantiate(DebrisPrefab, this.transform.localPosition, Quaternion.identity, debrisContainer.transform);
            debrisGO.GetComponent<Debris>().Init();
        }

        // spawn phone or candy or toxic debris
        if (type == Globals.EnemyTypes.FBI)
        {
            GameSceneManagerScript.ActivatePhoneFromPool(this.transform.localPosition);
        }
        else if (type == Globals.EnemyTypes.Scientist)
        {
            // create toxic debris
            int numToxicDebris = Random.Range(8, 15);
            for (int x = 0; x < numToxicDebris; x++)
            {
                GameObject toxicDebrisGO = Instantiate(ToxicDebrisPrefab, this.transform.localPosition, Quaternion.identity, debrisContainer.transform);
                toxicDebrisGO.GetComponent<ToxicDebris>().Init();
            }
        }
        else if (Random.Range(0, 100f) < 50f)
        {
            GameSceneManagerScript.ActivateCandyFromPool(this.transform.localPosition);
        }

        enemyCollider.enabled = false;
        DeActivate();
    }

    void DamageEnemy(Vector2 impactVelocity)
    {
        enemyRenderer.material = FlashMaterial;
        flashTimer = flashTimerMax;
        if (allowImpactVelocity)
        {
            impactVector = new Vector3(impactVelocity.x * .5f, impactVelocity.y * .5f, 0);
            impactTimer = impactTimerMax;
        }
    }
}
