using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    bool isActive = true;
    float life = 1f;
    float damage = 1f;

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
    bool flipWithMovement = false;

    Transform playerTransform;
    float positionTimer = .1f;
    float positionTimerMax = .5f;

    float flashTimer = 0f;
    float flashTimerMax = .15f;

    [SerializeField]
    GameObject DebrisPrefab;
    GameObject debrisContainer;

    // Start is called before the first frame update
    void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
        debrisContainer = GameObject.Find("DebrisContainer");
        enemyAnimator = GetComponent<Animator>();
        enemyCollider = GetComponent<BoxCollider2D>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyRenderer = GetComponent<SpriteRenderer>();
    }

    public void ConfigureEnemy(Globals.EnemyTypes newType)
    {
        type = newType;
        enemyRenderer.sprite = EnemySprites[(int)type];
        if (type == Globals.EnemyTypes.Yar)
        {
            moveSpeed = Random.Range(.65f, .75f);
            positionTimerMax = 1f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("yar");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.08f, 0.08f);
            flipWithMovement = true;
            life = 1f;
            damage = 1f;
        }
        else if (type == Globals.EnemyTypes.Robot)
        {
            moveSpeed = Random.Range(.45f, .55f);
            positionTimerMax = .5f;
            enemyAnimator.enabled = false;
            this.transform.localScale = new Vector3(3f, 3f, 1f);
            enemyCollider.size = new Vector2(0.15f, 0.15f);
            flipWithMovement = false;
            life = 1f;
            damage = 1f;
        }
        else if (type == Globals.EnemyTypes.Qbert)
        {
            moveSpeed = Random.Range(.35f, .45f);
            positionTimerMax = .5f;
            enemyAnimator.enabled = false;
            this.transform.localScale = new Vector3(3f, 3f, 1f);
            enemyCollider.size = new Vector2(0.15f, 0.15f);
            flipWithMovement = true;
            life = 2f;
            damage = 2f;
        }
        else if (type == Globals.EnemyTypes.Pac)
        {
            moveSpeed = Random.Range(.55f, .65f);
            positionTimerMax = .5f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("pac");
            this.transform.localScale = new Vector3(5f, 5f, 1f);
            enemyCollider.size = new Vector2(0.07f, 0.07f);
            flipWithMovement = true;
            life = 1f;
            damage = 1f;
        }
        else if (type == Globals.EnemyTypes.MsPac)
        {
            moveSpeed = Random.Range(.55f, .65f);
            positionTimerMax = .5f;
            enemyAnimator.enabled = true;
            enemyAnimator.Play("mspac");
            this.transform.localScale = new Vector3(3f, 3f, 1f);
            enemyCollider.size = new Vector2(0.09f, 0.09f);
            flipWithMovement = true;
            life = 2f;
            damage = 1.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();
        handleFlash();
    }

    private void handleMovement()
    {
        if (positionTimer > 0)
        {
            positionTimer -= Time.deltaTime;
            if (positionTimer < 0)
            {
                movementVector = (playerTransform.position - this.transform.localPosition).normalized * moveSpeed;
                positionTimer = Random.Range(positionTimerMax - .25f, positionTimerMax + .25f);
            }
        }
        if (flipWithMovement)
        {
            if ((movementVector.x >= 0 && this.transform.localScale.x < 0) || (movementVector.x < 0 && this.transform.localScale.x > 0))
                this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
        }
        enemyRigidbody.velocity = movementVector;
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

    void OnTriggerEnter2D(Collider2D collider)
    {
        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        if (bullet != null && isActive)
        {
            life = life - 1f;
            if (life <= 0)
                KillEnemy();
            else
                DamageEnemy();
            Destroy(bullet.gameObject);
        }
        Player player = collider.gameObject.GetComponent<Player>();
        if (player != null && isActive)
        {
            player.HitPlayer(damage);
        }
    }

    public void KillEnemy()
    {
        int numDebris = Random.Range(8, 10);
        for (int x = 0; x < numDebris; x++)
        {
            GameObject debrisGO = Instantiate(DebrisPrefab, this.transform.localPosition, Quaternion.identity, debrisContainer.transform);
            debrisGO.GetComponent<Debris>().Init();
        }

        this.GetComponent<Collider2D>().enabled = false;
        isActive = false;
        Destroy(this.gameObject);
    }

    void DamageEnemy()
    {
        enemyRenderer.color = new Color(87f/255f, 87f/255f, 87f/255f);
        flashTimer = flashTimerMax;
    }
}
