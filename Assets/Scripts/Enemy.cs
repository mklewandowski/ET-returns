using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameSceneManager GameSceneManagerScript;

    bool isActive = true;
    float life = 1f;
    float hitStrength = 1f;

    [SerializeField]
    Globals.EnemyTypes type = Globals.EnemyTypes.Yar;

    [SerializeField]
    Sprite[] EnemySprites;
    private SpriteRenderer enemyRenderer;
    private Animator enemyAnimator;

    private BoxCollider2D enemyCollider;
    private Rigidbody2D enemyRigidbody;
    float moveSpeed = .5f;
    Vector2 movementVector = new Vector2(0, 0);
    Vector2 impactVector = new Vector2(0, 0);
    float impactTimer = 0f;
    float impactTimerMax = .1f;
    bool flipWithMovement = false;

    Transform playerTransform;
    float positionTimer = .1f;
    float positionTimerMax = .5f;

    float flashTimer = 0f;
    float flashTimerMax = .15f;

    [SerializeField]
    GameObject DebrisPrefab;
    GameObject debrisContainer;

    [SerializeField]
    GameObject CandyPrefab;
    [SerializeField]
    GameObject PhonePrefab;
    GameObject itemContainer;

    void Awake()
    {
        GameSceneManagerScript = GameObject.Find("SceneManager").GetComponent<GameSceneManager>();
        playerTransform = GameObject.Find("Player").transform;
        debrisContainer = GameObject.Find("DebrisContainer");
        itemContainer = GameObject.Find("ItemContainer");
        enemyAnimator = GetComponent<Animator>();
        enemyCollider = GetComponent<BoxCollider2D>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyRenderer = GetComponent<SpriteRenderer>();
    }

    public void ConfigureEnemy(Globals.EnemyTypes newType)
    {
        type = newType;
        enemyRenderer.sprite = EnemySprites[(int)type];
        enemyRigidbody.mass = 1f;
        if (type == Globals.EnemyTypes.Yar)
        {
            moveSpeed = Random.Range(.8f, 1f);
            positionTimerMax = 1f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("yar");
            this.transform.localScale = new Vector3(6f, 6f, 1f);
            enemyCollider.size = new Vector2(0.08f, 0.08f);
            flipWithMovement = true;
            life = 1.5f;
            hitStrength = 2f;
        }
        else if (type == Globals.EnemyTypes.Pac)
        {
            moveSpeed = Random.Range(1f, 1.2f);
            positionTimerMax = .75f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("pac");
            this.transform.localScale = new Vector3(4f, 4f, 1f);
            enemyCollider.size = new Vector2(0.14f, 0.1f);
            flipWithMovement = true;
            life = 2f;
            hitStrength = 3f;
        }
        else if (type == Globals.EnemyTypes.MsPac)
        {
            moveSpeed = Random.Range(1.2f, 1.4f);
            positionTimerMax = .5f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("mspac");
            this.transform.localScale = new Vector3(4f, 4f, 1f);
            enemyCollider.size = new Vector2(0.14f, 0.1f);
            flipWithMovement = true;
            life = 3f;
            hitStrength = 4f;
        }
        else if (type == Globals.EnemyTypes.Joust)
        {
            moveSpeed = Random.Range(1.4f, 1.6f);
            positionTimerMax = .5f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("joust");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.15f, 0.15f);
            flipWithMovement = true;
            life = 4f;
            hitStrength = 5f;
        }
        else if (type == Globals.EnemyTypes.Joust2)
        {
            moveSpeed = Random.Range(1.6f, 1.8f);
            positionTimerMax = .4f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("joust2");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.15f, 0.15f);
            flipWithMovement = true;
            life = 6f;
            hitStrength = 6f;
        }
        else if (type == Globals.EnemyTypes.Frogger)
        {
            moveSpeed = Random.Range(.6f, .8f);
            positionTimerMax = .9f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("frog");
            this.transform.localScale = new Vector3(6f, 6f, 1f);
            enemyCollider.size = new Vector2(0.1f, 0.1f);
            flipWithMovement = true;
            life = 4f;
            hitStrength = 4f;
        }
        else if (type == Globals.EnemyTypes.Qbert)
        {
            moveSpeed = Random.Range(.8f, 1f);
            positionTimerMax = .8f;
            enemyAnimator.enabled = false;
            this.transform.localScale = new Vector3(4f, 4f, 1f);
            enemyCollider.size = new Vector2(0.15f, 0.15f);
            flipWithMovement = true;
            life = 8f;
            hitStrength = 7f;
        }
        else if (type == Globals.EnemyTypes.Kangaroo)
        {
            moveSpeed = Random.Range(1f, 1.2f);
            positionTimerMax = .7f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("kangaroo");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.12f, 0.2f);
            flipWithMovement = true;
            life = 12f;
            hitStrength = 10f;
        }
        else if (type == Globals.EnemyTypes.Hero)
        {
            moveSpeed = Random.Range(1.2f, 1.4f);
            positionTimerMax = .6f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("hero");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.12f, 0.2f);
            flipWithMovement = true;
            life = 16f;
            hitStrength = 12f;
        }
        else if (type == Globals.EnemyTypes.FBI)
        {
            moveSpeed = Random.Range(1.4f, 1.8f);
            positionTimerMax = 4f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("fbi");
            this.transform.localScale = new Vector3(4f, 4f, 1f);
            enemyCollider.size = new Vector2(0.15f, 0.3f);
            flipWithMovement = true;
            life = 2f;
            hitStrength = 4f;
            enemyRigidbody.mass = 999f;
            int layerIgnoreRaycast = LayerMask.NameToLayer("EnemySpecial");
            gameObject.layer = layerIgnoreRaycast;
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();
        handleFlash();
        handleImpact();
    }

    private void handleMovement()
    {
        if (positionTimer > 0)
        {
            positionTimer -= Time.deltaTime;
            if (positionTimer < 0)
            {
                Vector3 desiredPosition = type == Globals.EnemyTypes.FBI
                    ? new Vector3(playerTransform.position.x + 6f * (Random.Range(0, 2) == 0 ? -1f : 1f), playerTransform.position.y + 4f * (Random.Range(0, 2) == 0 ? -1f : 1f), 0)
                    : playerTransform.position;
                movementVector = (desiredPosition - this.transform.localPosition).normalized * moveSpeed;
                positionTimer = Random.Range(positionTimerMax - .25f, positionTimerMax + .25f);
            }
        }
        if (flipWithMovement)
        {
            if ((movementVector.x >= 0 && this.transform.localScale.x < 0) || (movementVector.x < 0 && this.transform.localScale.x > 0))
                this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
        }
        enemyRigidbody.velocity = impactTimer > 0 ? impactVector : movementVector;
    }

    private void handleFlash()
    {
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer < 0)
            {
                enemyRenderer.color = Color.white;
            }
        }
    }

    private void handleImpact()
    {
        if (impactTimer > 0)
        {
            impactTimer -= Time.deltaTime;
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
        float damage = 0;
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
            damage = attackObject.Damage * Globals.currentAttack;
        }
        life = life - damage;
        if (life <= 0)
            KillEnemy();
        else if (damage > 0)
            DamageEnemy(damageVelocity);
    }

    public void KillEnemy()
    {
        Globals.killCount++;

        // create debris
        int numDebris = Random.Range(8, 10);
        for (int x = 0; x < numDebris; x++)
        {
            GameObject debrisGO = Instantiate(DebrisPrefab, this.transform.localPosition, Quaternion.identity, debrisContainer.transform);
            debrisGO.GetComponent<Debris>().Init();
        }

        // spawn phone or candy
        if (type == Globals.EnemyTypes.FBI)
        {
            GameObject phoneGO = Instantiate(PhonePrefab, this.transform.localPosition, Quaternion.identity, itemContainer.transform);
            phoneGO.GetComponent<Phone>().Init();
            GameSceneManagerScript.KillFBI();
        }
        else if (Random.Range(0, 100f) < 50f)
        {
            GameObject candyGO = Instantiate(CandyPrefab, this.transform.localPosition, Quaternion.identity, itemContainer.transform);
            candyGO.GetComponent<Candy>().Init();
        }

        this.GetComponent<Collider2D>().enabled = false;
        isActive = false;
        Destroy(this.gameObject);
    }

    void DamageEnemy(Vector2 impactVelocity)
    {
        enemyRenderer.color = new Color(87f/255f, 87f/255f, 87f/255f);
        flashTimer = flashTimerMax;
        impactVector = new Vector2(impactVelocity.x * .5f, impactVelocity.y * .5f);
        impactTimer = impactTimerMax;
    }
}
