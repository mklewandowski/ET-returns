using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    TextMeshPro DebugText;

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
    float behaviorTimer = .1f;
    float behaviorTimerMax = .5f;

    [SerializeField]
    Material FlashMaterial;
    Material enemyMaterial;
    float flashTimer = 0f;
    float flashTimerMax = .10f;

    float lifeTimer = 0f;
    bool useLifeTimer = false;

    GameObject debrisContainer;
    [SerializeField]
    GameObject ToxicDebrisPrefab;

    enum AttackType {
        Seek,
        Surround,
        Chaotic,
        Avoid,
        StaticLine
    }
    AttackType attackType = AttackType.Seek;

    enum BehaviorType {
        Seek,
        Surround,
        RandomAngle,
        SetAngle,
        Spread, // used to unclump
        Wait,
        Avoid, // used by FBI and scientist
        StaticLine, // used by plane and rover
    }
    BehaviorType currentBehavior = BehaviorType.Seek;
    float sightDistance = 2f;

    float surroundOffsetX;
    float surroundOffsetY;

    bool isBoss = false;

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
        type = newType;

        // reset timers
        flashTimer = 0;
        impactTimer = 0f;
        behaviorTimer = .1f;

        // visual
        enemyRenderer.material = enemyMaterial;
        enemyRenderer.enabled = false;
        enemyAnimator.enabled = false;
        enemyRenderer.sprite = EnemySprites[(int)type];

        // position
        this.transform.localPosition = pos;

        // movement and behavior
        flipWithMovement = true;
        allowImpactVelocity = true;
        attackType = AttackType.Seek;
        currentBehavior = BehaviorType.Seek;

        enemyRigidbody.mass = 1f;
        useLifeTimer = false;
        this.gameObject.SetActive(true);
        int defaultCollisionLayer = LayerMask.NameToLayer("Enemy");
        gameObject.layer = defaultCollisionLayer;

        // FAST SEEK
        if (type == Globals.EnemyTypes.Yar)
        {
            moveSpeed = Random.Range(.9f, 1.2f);
            behaviorTimerMax = .75f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("yar");
            this.transform.localScale = new Vector3(6f, 6f, 1f);
            enemyCollider.size = new Vector2(0.08f, 0.08f);
            life = 2;
            hitStrength = 2;
        }
        else if (type == Globals.EnemyTypes.Pac)
        {
            moveSpeed = Random.Range(1.2f, 1.5f);
            behaviorTimerMax = .75f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("pac");
            this.transform.localScale = new Vector3(4f, 4f, 1f);
            enemyCollider.size = new Vector2(0.14f, 0.1f);
            life = 4;
            hitStrength = 4;
        }
        else if (type == Globals.EnemyTypes.MsPac)
        {
            moveSpeed = Random.Range(1.5f, 1.8f);
            behaviorTimerMax = .5f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("mspac");
            this.transform.localScale = new Vector3(4f, 4f, 1f);
            enemyCollider.size = new Vector2(0.14f, 0.1f);
            life = 6;
            hitStrength = 5;
        }
        else if (type == Globals.EnemyTypes.Yar2)
        {
            moveSpeed = Random.Range(1.8f, 2.1f);
            behaviorTimerMax = .5f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("yar2");
            this.transform.localScale = new Vector3(6f, 6f, 1f);
            enemyCollider.size = new Vector2(0.08f, 0.08f);
            life = 10;
            hitStrength = 6;
        }
        else if (type == Globals.EnemyTypes.JrPac)
        {
            moveSpeed = Random.Range(2.1f, 2.3f);
            behaviorTimerMax = .6f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("jrpac");
            this.transform.localScale = new Vector3(4f, 4f, 1f);
            enemyCollider.size = new Vector2(0.14f, 0.1f);
            life = 14;
            hitStrength = 7;
        }

        // STRONG SEEK
        else if (type == Globals.EnemyTypes.Qbert)
        {
            moveSpeed = Random.Range(.8f, 1f);
            behaviorTimerMax = 1f;
            enemyAnimator.enabled = false;
            this.transform.localScale = new Vector3(4f, 4f, 1f);
            enemyCollider.size = new Vector2(0.15f, 0.15f);
            life = 10;
            hitStrength = 8;
        }
        else if (type == Globals.EnemyTypes.Kangaroo)
        {
            moveSpeed = Random.Range(1f, 1.2f);
            behaviorTimerMax = .9f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("kangaroo");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.12f, 0.2f);
            life = 20;
            hitStrength = 12;
        }
        else if (type == Globals.EnemyTypes.Hero)
        {
            moveSpeed = Random.Range(1.2f, 1.4f);
            behaviorTimerMax = .6f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("hero");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.12f, 0.2f);
            life = 28;
            hitStrength = 16;
        }
        else if (type == Globals.EnemyTypes.Pengo)
        {
            moveSpeed = Random.Range(1.4f, 1.6f);
            behaviorTimerMax = 2f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("pengo");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.3f, 0.07f);
            life = 34;
            hitStrength = 18;
        }
        else if (type == Globals.EnemyTypes.Hero2)
        {
            moveSpeed = Random.Range(1.6f, 1.8f);
            behaviorTimerMax = .4f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("hero2");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.12f, 0.2f);
            life = 40;
            hitStrength = 20;
        }


        // SURROUND
        else if (type == Globals.EnemyTypes.Frogger)
        {
            moveSpeed = Random.Range(.6f, .8f);
            behaviorTimerMax = 2f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("frog");
            this.transform.localScale = new Vector3(6f, 6f, 1f);
            enemyCollider.size = new Vector2(0.1f, 0.1f);
            life = 5;
            hitStrength = 5;
            attackType = AttackType.Surround;
            currentBehavior = BehaviorType.Surround;
            PickSurroundOffsets();
        }
        else if (type == Globals.EnemyTypes.Joust)
        {
            moveSpeed = Random.Range(1.5f, 1.2f);
            behaviorTimerMax = 1f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("joust");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.15f, 0.15f);
            life = 8;
            hitStrength = 7;
            attackType = AttackType.Surround;
            currentBehavior = BehaviorType.Surround;
            PickSurroundOffsets();
        }
        else if (type == Globals.EnemyTypes.Bear)
        {
            moveSpeed = Random.Range(1.4f, 1.6f);
            behaviorTimerMax = 1f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("bear");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.12f, 0.12f);
            life = 12;
            hitStrength = 10;
            attackType = AttackType.Surround;
            currentBehavior = BehaviorType.Surround;
            PickSurroundOffsets();
        }
        else if (type == Globals.EnemyTypes.Joust2)
        {
            moveSpeed = Random.Range(1.6f, 1.8f);
            behaviorTimerMax = 1f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("joust2");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.15f, 0.15f);
            life = 18;
            hitStrength = 12;
            attackType = AttackType.Surround;
            currentBehavior = BehaviorType.Surround;
            PickSurroundOffsets();
        }

        // CHAOS
        else if (type == Globals.EnemyTypes.Indy)
        {
            moveSpeed = Random.Range(1.6f, 1.8f);
            behaviorTimerMax = 2f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("indy");
            this.transform.localScale = new Vector3(8f, 8f, 1f);
            enemyCollider.size = new Vector2(0.05f, 0.1f);
            life = 10;
            hitStrength = 8;
            attackType = AttackType.Chaotic;
            currentBehavior = Random.Range(0, 4) > 0 ? BehaviorType.SetAngle : BehaviorType.RandomAngle;
        }
        else if (type == Globals.EnemyTypes.Jungle)
        {
            moveSpeed = Random.Range(1.8f, 2f);
            behaviorTimerMax = 1.5f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("jungle");
            this.transform.localScale = new Vector3(4f, 4f, 1f);
            enemyCollider.size = new Vector2(0.08f, 0.16f);
            life = 20;
            hitStrength = 10;
            attackType = AttackType.Chaotic;
            currentBehavior = Random.Range(0, 4) > 0 ? BehaviorType.SetAngle : BehaviorType.RandomAngle;
        }
        else if (type == Globals.EnemyTypes.Harry)
        {
            moveSpeed = Random.Range(2f, 2.2f);
            behaviorTimerMax = 1.5f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("harry");
            this.transform.localScale = new Vector3(4f, 4f, 1f);
            enemyCollider.size = new Vector2(0.08f, 0.16f);
            life = 30;
            hitStrength = 12;
            attackType = AttackType.Chaotic;
            currentBehavior = Random.Range(0, 4) > 0 ? BehaviorType.SetAngle : BehaviorType.RandomAngle;
        }

        // SPECIAL
        else if (type == Globals.EnemyTypes.Dig)
        {
            moveSpeed = Random.Range(.6f, .8f);
            behaviorTimerMax = .9f;
            enemyAnimator.enabled = false;
            this.transform.localScale = new Vector3(.1f, .1f, 1f);
            enemyCollider.size = new Vector2(0.1f, 0.1f);
            life = 6;
            hitStrength = 6;
            this.transform.localEulerAngles = new Vector3(0, flip ? 180f: 0f, 0);
            currentBehavior = BehaviorType.Wait;
            behaviorTimer = 2f;
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

            attackType = AttackType.StaticLine;
            currentBehavior = BehaviorType.Wait;
            behaviorTimer = 2f;
            allowImpactVelocity = false;
            flipWithMovement = false;
            movementVector = new Vector3(5f, 0, 0);
            this.transform.localEulerAngles = new Vector3(0, 0f, 0);
            useLifeTimer = true;
            lifeTimer = 15f;

            int collisionLayer = LayerMask.NameToLayer("EnemyPlane");
            gameObject.layer = collisionLayer;

            this.GetComponent<GrowAndShrink>().StartEffect();
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

            attackType = AttackType.StaticLine;
            currentBehavior = BehaviorType.Wait;
            behaviorTimer = 2f;
            allowImpactVelocity = false;
            flipWithMovement = false;
            movementVector = new Vector3(flip ? -5.5f : 5.5f, 0, 0);
            this.transform.localEulerAngles = new Vector3(0, flip ? 180f: 0f, 0);
            useLifeTimer = true;
            lifeTimer = 15f;

            int collisionLayer = LayerMask.NameToLayer("EnemyPlane");
            gameObject.layer = collisionLayer;

            this.GetComponent<GrowAndShrink>().StartEffect();
        }
        else if (type == Globals.EnemyTypes.FBI)
        {
            moveSpeed = Random.Range(1.75f, 2.25f);
            behaviorTimerMax = 4f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("fbi");
            this.transform.localScale = new Vector3(4f, 4f, 1f);
            enemyCollider.size = new Vector2(0.15f, 0.3f);
            life = 2;
            hitStrength = 5;

            attackType = AttackType.Avoid;
            currentBehavior = BehaviorType.Avoid;

            int collisionLayer = LayerMask.NameToLayer("EnemySpecial");
            gameObject.layer = collisionLayer;
        }
        else if (type == Globals.EnemyTypes.Scientist)
        {
            moveSpeed = Random.Range(2f, 2.5f);
            behaviorTimerMax = 4f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("scientist");
            this.transform.localScale = new Vector3(4f, 4f, 1f);
            enemyCollider.size = new Vector2(0.15f, 0.3f);
            life = 5;
            hitStrength = 10;

            attackType = AttackType.Avoid;
            currentBehavior = BehaviorType.Avoid;

            int collisionLayer = LayerMask.NameToLayer("EnemySpecial");
            gameObject.layer = collisionLayer;
        }

        // BOSS
        else if (type == Globals.EnemyTypes.PacBoss)
        {
            moveSpeed = 1.4f;
            behaviorTimerMax = 1.5f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("pac");
            this.transform.localScale = new Vector3(12f, 12f, 1f);
            enemyCollider.size = new Vector2(0.12f, 0.1f);
            enemyRigidbody.mass = 99f;
            life = 200;
            hitStrength = 10;

            attackType = AttackType.Seek;
            behaviorTimer = 2f;
            isBoss = true;
        }
        else if (type == Globals.EnemyTypes.PopeyeBoss)
        {
            moveSpeed = 1.8f;
            behaviorTimerMax = 1f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("sailor");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.14f, 0.24f);
            enemyRigidbody.mass = 99f;
            life = 200;
            hitStrength = 10;

            attackType = AttackType.Seek;
            behaviorTimer = 2f;
            isBoss = true;
        }
        else if (type == Globals.EnemyTypes.MarioBoss)
        {
            moveSpeed = 1.8f;
            behaviorTimerMax = 1f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("mario");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.1f, 0.2f);
            enemyRigidbody.mass = 99f;
            life = 200;
            hitStrength = 10;

            attackType = AttackType.Seek;
            behaviorTimer = 2f;
            isBoss = true;
        }
        else if (type == Globals.EnemyTypes.LuigiBoss)
        {
            moveSpeed = 1.8f;
            behaviorTimerMax = 1f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("luigi");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.1f, 0.2f);
            enemyRigidbody.mass = 99f;
            life = 200;
            hitStrength = 10;

            attackType = AttackType.Seek;
            behaviorTimer = 2f;
            isBoss = true;
        }
        else if (type == Globals.EnemyTypes.KoolBoss)
        {
            moveSpeed = 1.4f;
            behaviorTimerMax = 1.5f;
            enemyAnimator.enabled = false;
            this.transform.localScale = new Vector3(6f, 6f, 1f);
            enemyCollider.size = new Vector2(0.16f, 0.11f);
            enemyRigidbody.mass = 99f;
            life = 250;
            hitStrength = 10;

            attackType = AttackType.Seek;
            behaviorTimer = .5f;
            isBoss = true;
        }

        life = life + extraLife;
        enemyCollider.enabled = true;
        enemyRenderer.enabled = true;
        isActive = true;
    }

    void PickSurroundOffsets()
    {
        surroundOffsetY = Random.Range(0, 2) == 0
            ? Random.Range(2f, 4f)
            : Random.Range(-2f, -4f);
        surroundOffsetX = Random.Range(0, 2) == 0
            ? Random.Range(3f, 6f)
            : Random.Range(-3f, -6f);
    }

    // Update is called once per frame
    void Update()
    {
        HandleBehavior();
        HandleFlash();
        HandleImpact();
        HandleLifeTimer();
    }

    private void HandleBehavior()
    {
        if (behaviorTimer > 0)
        {
            behaviorTimer -= Time.deltaTime;
            if (behaviorTimer <= 0)
            {
                behaviorTimer = Random.Range(behaviorTimerMax - .25f, behaviorTimerMax + .25f);
                // do something with current behavior
                if (currentBehavior == BehaviorType.Wait)
                {
                    if (attackType == AttackType.StaticLine)
                        currentBehavior = BehaviorType.StaticLine;
                    else
                    {
                        currentBehavior = BehaviorType.Seek;
                        UpdateSeekPosition();
                    }
                }
                else if (currentBehavior == BehaviorType.StaticLine)
                {
                    // nothing?
                }
                else if (currentBehavior == BehaviorType.Avoid)
                {
                    UpdateAvoidPosition();
                }
                else if (currentBehavior == BehaviorType.RandomAngle)
                {
                    UpdateRandomAnglePosition();
                }
                else if (currentBehavior == BehaviorType.SetAngle)
                {
                    UpdateSetAnglePosition();
                }
                else if (currentBehavior == BehaviorType.Seek)
                {
                    if (CheckForClumping())
                    {
                        currentBehavior = BehaviorType.Spread;
                        behaviorTimer = Mathf.Max(4f, Random.Range(behaviorTimerMax - .25f, behaviorTimerMax + .25f) * 5f);
                    }
                    else
                    {
                        if (attackType == AttackType.Surround)
                        {
                            float distanceFromPlayer = Mathf.Abs(Vector3.Distance(playerTransform.position, this.transform.localPosition));
                            if (distanceFromPlayer > (sightDistance + 1f))
                            {
                                currentBehavior = BehaviorType.Surround;
                                UpdateSurroundPosition();
                            }
                        }
                        else
                        {
                            UpdateSeekPosition();
                        }
                    }
                }
                else if (currentBehavior == BehaviorType.Surround)
                {
                    if (CheckForClumping())
                    {
                        currentBehavior = BehaviorType.Spread;
                        behaviorTimer = Mathf.Max(4f, Random.Range(behaviorTimerMax - .25f, behaviorTimerMax + .25f) * 5f);
                    }
                    else
                    {
                        float distanceFromPlayer = Mathf.Abs(Vector3.Distance(playerTransform.position, this.transform.localPosition));
                        if (distanceFromPlayer <= sightDistance)
                        {
                            currentBehavior = BehaviorType.Seek;
                            UpdateSeekPosition();
                        }
                        else
                        {
                            UpdateSurroundPosition();
                        }
                    }
                }
                else if (currentBehavior == BehaviorType.Spread)
                {
                    if (attackType == AttackType.Surround)
                    {
                        currentBehavior = BehaviorType.Surround;
                        UpdateSurroundPosition();
                    }
                    else if (attackType == AttackType.Seek)
                    {
                        currentBehavior = BehaviorType.Seek;
                        UpdateSeekPosition();
                    }
                }

            }
        }
        if (currentBehavior == BehaviorType.Seek || currentBehavior == BehaviorType.Surround || currentBehavior == BehaviorType.RandomAngle || currentBehavior == BehaviorType.SetAngle ||
            currentBehavior == BehaviorType.StaticLine || currentBehavior == BehaviorType.Avoid || currentBehavior == BehaviorType.Spread)
            enemyRigidbody.velocity = impactTimer > 0 ? impactVector : movementVector;

        if (flipWithMovement && currentBehavior != BehaviorType.Wait)
        {
            if (movementVector.x >= 0 && this.transform.localEulerAngles.y > 1f)
                this.transform.localEulerAngles = new Vector3(0, 0, 0);
            else if (movementVector.x < 0 && this.transform.localEulerAngles.x < 1f)
                this.transform.localEulerAngles = new Vector3(0, 180f, 0);
        }
        if (currentBehavior == BehaviorType.Avoid)
        {
            CheckAvoidPosition();
        }
    }

    private bool CheckForClumping()
    {
        Vector2 enemyVectorSum = Vector2.zero;
        float enemiesInGroup = 0f; // the number of enemies in the collision circle
        float groupCircleRadius = .75f;
        int minEnemiesInGroupBeforeSpread = 2;
        float spreadMagnitudeThreshold = 3f;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, groupCircleRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Enemy>() != null && hitCollider.transform != transform)
            {
                // get the difference so we know which way to go
                Vector2 difference = transform.position - hitCollider.transform.position;

                // weight by distance so being closer means moving more
                difference = difference.normalized / Mathf.Abs(difference.magnitude);

                // add together to get average of the group
                // this allows those at the edges of a group to move out while
                // the enemies in the center of a group to not move much
                enemyVectorSum += difference;
                enemiesInGroup++;
            }
        }
        if (enemiesInGroup >= minEnemiesInGroupBeforeSpread && enemyVectorSum.magnitude > spreadMagnitudeThreshold)
        {
            // normalize the vector and multiply by movespeed to move away from group
            movementVector = enemyVectorSum.normalized * moveSpeed;
            return true;
        }
        return false;
    }

    private void UpdateSeekPosition()
    {
        desiredPosition = playerTransform.position;
        movementVector = (desiredPosition - this.transform.localPosition).normalized * moveSpeed;
    }
    private void UpdateSurroundPosition()
    {
        desiredPosition = new Vector3(playerTransform.position.x + surroundOffsetX, playerTransform.position.y + surroundOffsetY, playerTransform.position.z);
        movementVector = (desiredPosition - this.transform.localPosition).normalized * moveSpeed;
    }
    private void UpdateRandomAnglePosition()
    {
        desiredPosition = playerTransform.position;
        movementVector = (desiredPosition - this.transform.localPosition).normalized * moveSpeed;
        movementVector = Quaternion.Euler(0, 0, Random.Range (-50f, 50f)) * movementVector;
    }
    private void UpdateSetAnglePosition()
    {
        desiredPosition = playerTransform.position;
        movementVector = (desiredPosition - this.transform.localPosition).normalized * moveSpeed;
        movementVector = Quaternion.Euler(0, 0, 50f) * movementVector;
    }
    private void UpdateAvoidPosition()
    {
        desiredPosition = new Vector3(
            playerTransform.position.x + 6f * (Random.Range(0, 2) == 0 ? -1f : 1f),
            playerTransform.position.y + 4f * (Random.Range(0, 2) == 0 ? -1f : 1f),
            0
        );
        movementVector = (desiredPosition - this.transform.localPosition).normalized * moveSpeed;
    }
    private void CheckAvoidPosition()
    {
        float xBuffer = 2f;
        float yBuffer = 2.2f;
        if ((this.transform.localPosition.x + xBuffer >= RightWall.position.x && movementVector.x > 0) ||
            (this.transform.localPosition.x - xBuffer <= LeftWall.position.x && movementVector.x < 0) ||
            (this.transform.localPosition.y + yBuffer >= TopWall.position.y && movementVector.y > 0) ||
            (this.transform.localPosition.y - yBuffer <= BottomWall.position.y && movementVector.y < 0))
        {
            UpdateAvoidPosition();
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
        Boomerang boomerang = collider.gameObject.GetComponent<Boomerang>();
        ToxicDebris toxicDebris = collider.gameObject.GetComponent<ToxicDebris>();
        SlimeDebris slimeDebris = collider.gameObject.GetComponent<SlimeDebris>();
        AttackObject attackObject = collider.gameObject.GetComponent<AttackObject>();
        int damage = 0;
        Vector2 damageVelocity = new Vector2(0, 0);
        if (bullet != null)
        {
            Rigidbody2D bulletRB = collider.gameObject.GetComponent<Rigidbody2D>();
            if (bulletRB != null && currentBehavior != BehaviorType.Wait)
                damageVelocity = bulletRB.velocity;
            bullet.HitEnemy();
        }
        else if (boomerang != null || toxicDebris != null || !slimeDebris)
        {
            Rigidbody2D objectRB = collider.gameObject.GetComponent<Rigidbody2D>();
            if (objectRB != null && currentBehavior != BehaviorType.Wait)
                damageVelocity = objectRB.velocity;
            if (slimeDebris)
                slimeDebris.HitEnemy();
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

            if (attackObject.CausePushBackDamageVelocity && currentBehavior != BehaviorType.Wait)
            {
                damageVelocity = enemyRigidbody.velocity.normalized * (-1f * attackObject.PushBackDamageVelocityMultiplier);
            }
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
            GameSceneManagerScript.ActivateDebrisFromPool(this.transform.localPosition, false);
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
        else if (isBoss)
        {
            audioManager.PlayBossKillSound();
            GameSceneManagerScript.KillBoss();
            for (int i = 0; i < 10; i++)
            {
                float randX = Random.Range(-.5f, .5f);
                float randY = Random.Range(-.5f, .5f);
                GameSceneManagerScript.ActivateCandyFromPool(this.transform.localPosition, true, new Vector3(randX, randY, this.transform.localPosition.z));
            }
        }
        else if (Random.Range(0, 100f) < 50f)
        {
            GameSceneManagerScript.ActivateCandyFromPool(this.transform.localPosition, false, Vector3.zero);
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
