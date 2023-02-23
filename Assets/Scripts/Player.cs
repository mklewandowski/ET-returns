using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    enum PlayerOrientation {
        Up,
        UpRight,
        UpLeft,
        Down,
        DownRight,
        DownLeft,
        Left,
        Right
    };
    PlayerOrientation currentPlayerOrientation = PlayerOrientation.Right;

    [SerializeField]
    GameObject PlayerGO;
    [SerializeField]
    GameObject GunGO;
    [SerializeField]
    GameObject MuzzleGO;
    [SerializeField]
    GameObject BulletPrefab;
    [SerializeField]
    GameObject BulletContainer;

    float moveSpeed = 2f;
    Vector2 movementVector = new Vector2(0, 0);
    bool moveLeft;
    bool moveRight;
    bool moveUp;
    bool moveDown;
    private Rigidbody2D playerRigidbody;
    bool isMoving = false;
    Animator playerAnimator;

    float shootTimer = 2f;
    float shootTimerBurstMax = .25f;
    float shootTimerMax = 1.5f;
    int burstNum = 4;
    int burstNumMax = 4;
    float muzzleFlashTimer = 0f;
    float muzzleFlashTimerMax = .05f;

    bool isAlive = true;
    float life = 10f;
    float lifeMax = 10f;
    float invincibleTimer = 0f;
    float invincibleTimerMax = 1f;
    private SpriteRenderer playerRenderer;
    [SerializeField]
    GameObject LifeBar;

    [SerializeField]
    RectTransform ExpBar;
    [SerializeField]
    TextMeshProUGUI ExpLevel;
    float currentExp = 0;
    int currentLevel = 0;
    float maxExpBarWidth = 400f;
    float[] maxExperiences;
    int currentPhonePieces = 0;
    int maxPhonePieces = 3;
    [SerializeField]
    GameObject[] PhonePieces;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = PlayerGO.GetComponent<Animator>();
        playerRenderer = PlayerGO.GetComponent<SpriteRenderer>();
        maxExperiences = new float[] {100f, 200f, 300f, 400f, 500f, 600f, 700f, 800f, 900, 1000f};
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleShoot();
        HandleInvincible();
    }

    private void HandleMovement()
    {
        if (!isAlive) return;
        moveLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        moveRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        moveUp = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        moveDown = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
        movementVector = new Vector2(0, 0);

        if (moveRight)
        {
            movementVector.x = moveSpeed;
            PlayerGO.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (moveLeft)
        {
            movementVector.x = moveSpeed * -1f;
            PlayerGO.transform.localEulerAngles = new Vector3(0, 180f, 0);
        }
        if (moveUp)
            movementVector.y = moveSpeed;
        else if (moveDown)
            movementVector.y = moveSpeed * -1f;

        playerRigidbody.velocity = movementVector;

        if ((movementVector.x != 0 || movementVector.y != 0) && !isMoving)
        {
            isMoving = true;
            playerAnimator.Play("et-walk");
        }
        else if (movementVector.x == 0 && movementVector.y == 0 && isMoving)
        {
            isMoving = false;
            playerAnimator.Play("et-idle");
        }

        if (moveRight && !moveUp && !moveDown)
            currentPlayerOrientation = PlayerOrientation.Right;
        else if (moveLeft && !moveUp && !moveDown)
            currentPlayerOrientation = PlayerOrientation.Left;
        else if (moveUp && !moveRight && !moveLeft)
            currentPlayerOrientation = PlayerOrientation.Up;
        else if (moveDown && !moveRight && !moveLeft)
            currentPlayerOrientation = PlayerOrientation.Down;
        else if (moveRight && moveUp)
            currentPlayerOrientation = PlayerOrientation.UpRight;
        else if (moveLeft && moveUp)
            currentPlayerOrientation = PlayerOrientation.UpLeft;
        else if (moveRight && moveDown)
            currentPlayerOrientation = PlayerOrientation.DownRight;
        else if (moveLeft && moveDown)
            currentPlayerOrientation = PlayerOrientation.DownLeft;

        if (currentPlayerOrientation == PlayerOrientation.Right || currentPlayerOrientation == PlayerOrientation.Left)
            GunGO.transform.localEulerAngles = new Vector3(0, 0, 0);
        else if (currentPlayerOrientation == PlayerOrientation.UpLeft || currentPlayerOrientation == PlayerOrientation.UpRight)
            GunGO.transform.localEulerAngles = new Vector3(0, 0, 45f);
        else if (currentPlayerOrientation == PlayerOrientation.Up)
            GunGO.transform.localEulerAngles = new Vector3(0, 0, 90f);
        else if (currentPlayerOrientation == PlayerOrientation.Down)
            GunGO.transform.localEulerAngles = new Vector3(0, 0, 270f);
        else if (currentPlayerOrientation == PlayerOrientation.DownLeft || currentPlayerOrientation == PlayerOrientation.DownRight)
            GunGO.transform.localEulerAngles = new Vector3(0, 0, 315f);
    }

    private void HandleShoot()
    {
        if (!isAlive) return;
        shootTimer -= Time.deltaTime;
        if (shootTimer < 0)
        {
            GameObject bulletGO = Instantiate(BulletPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
            Rigidbody2D bulletRigidbody = bulletGO.GetComponent<Rigidbody2D>();
            float xMovement = currentPlayerOrientation == PlayerOrientation.Right || currentPlayerOrientation == PlayerOrientation.UpRight || currentPlayerOrientation == PlayerOrientation.DownRight
                ? 10f
                : currentPlayerOrientation == PlayerOrientation.Left || currentPlayerOrientation == PlayerOrientation.DownLeft || currentPlayerOrientation == PlayerOrientation.UpLeft
                    ? -10f
                    : 0;
            float yMovement = currentPlayerOrientation == PlayerOrientation.Up || currentPlayerOrientation == PlayerOrientation.UpRight || currentPlayerOrientation == PlayerOrientation.UpLeft
                ? 10f
                : currentPlayerOrientation == PlayerOrientation.Down || currentPlayerOrientation == PlayerOrientation.DownLeft  || currentPlayerOrientation == PlayerOrientation.DownRight
                    ? -10f
                    : 0;
            Vector2 bulletMovement = new Vector2(xMovement, yMovement);
            bulletRigidbody.velocity = bulletMovement;

            if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.TripleShot] >= 0)
                HandleShootTriple(bulletMovement);
            if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.RearShot] >= 0)
                HandleShootRear(bulletMovement);
            if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.SideShot] >= 0)
                HandleShootSide(bulletMovement);

            burstNum = burstNum - 1;
            shootTimer = burstNum == 0 ? shootTimerMax : shootTimerBurstMax;
            if (burstNum == 0) burstNum = burstNumMax;

            MuzzleGO.SetActive(true);
            muzzleFlashTimer = muzzleFlashTimerMax;
        }

        if (muzzleFlashTimer > 0)
        {
            muzzleFlashTimer -= Time.deltaTime;
            if (muzzleFlashTimer < 0)
                MuzzleGO.SetActive(false);
        }
    }

    private void HandleShootTriple(Vector2 bulletMovement)
    {
        Vector2 bullet1Movement = new Vector2(bulletMovement.x, bulletMovement.y);
        Vector2 bullet2Movement = new Vector2(bulletMovement.x, bulletMovement.y);
        if (bulletMovement.x == 0)
        {
            bullet1Movement.x = bulletMovement.x + 2f;
            bullet2Movement.x = bulletMovement.x - 2f;
        }
        else if (bulletMovement.y == 0)
        {
            bullet1Movement.y = bulletMovement.y + 2f;
            bullet2Movement.y = bulletMovement.y - 2f;
        }
        else if ((bulletMovement.x < 0 && bulletMovement.y > 0) || bulletMovement.x > 0 && bulletMovement.y < 0)
        {
            bullet1Movement.x = bulletMovement.x + 2f;
            bullet2Movement.x = bulletMovement.x - 2f;
            bullet1Movement.y = bulletMovement.y + 2f;
            bullet2Movement.y = bulletMovement.y - 2f;
        }
        else
        {
            bullet1Movement.x = bulletMovement.x + 2f;
            bullet2Movement.x = bulletMovement.x - 2f;
            bullet1Movement.y = bulletMovement.y - 2f;
            bullet2Movement.y = bulletMovement.y + 2f;
        }
        GameObject bullet1GO = Instantiate(BulletPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
        Rigidbody2D bullet1Rigidbody = bullet1GO.GetComponent<Rigidbody2D>();
        GameObject bullet2GO = Instantiate(BulletPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
        Rigidbody2D bullet2Rigidbody = bullet2GO.GetComponent<Rigidbody2D>();
        bullet1Rigidbody.velocity = bullet1Movement;
        bullet2Rigidbody.velocity = bullet2Movement;
    }

    private void HandleShootRear(Vector2 bulletMovement)
    {
        if (burstNum < burstNumMax)
            return;
        Vector2 bulletRearMovement = new Vector2(bulletMovement.x * -1f, bulletMovement.y * -1f);
        GameObject bulletRearGO = Instantiate(BulletPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
        Rigidbody2D bulletRearRigidbody = bulletRearGO.GetComponent<Rigidbody2D>();
        bulletRearRigidbody.velocity = bulletRearMovement;
    }

    private void HandleShootSide(Vector2 bulletMovement)
    {
        if (burstNum < burstNumMax)
            return;
        Vector2 bullet1Movement = new Vector2(bulletMovement.x, bulletMovement.y);
        Vector2 bullet2Movement = new Vector2(bulletMovement.x, bulletMovement.y);
        if (bulletMovement.x == 0)
        {
            bullet1Movement.x = 10f;
            bullet2Movement.x = -10f;
            bullet1Movement.y = 0;
            bullet2Movement.y = 0;
        }
        else if (bulletMovement.y == 0)
        {
            bullet1Movement.x = 0;
            bullet2Movement.x = 0;
            bullet1Movement.y = 10f;
            bullet2Movement.y = -10f;
        }
        else if ((bulletMovement.x < 0 && bulletMovement.y > 0) || bulletMovement.x > 0 && bulletMovement.y < 0)
        {
            bullet1Movement.x = 10f;
            bullet2Movement.x = -10f;
            bullet1Movement.y = 10f;
            bullet2Movement.y = -10f;
        }
        else
        {
            bullet1Movement.x = 10f;
            bullet2Movement.x = -10f;
            bullet1Movement.y = -10f;
            bullet2Movement.y = 10f;
        }
        GameObject bullet1GO = Instantiate(BulletPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
        Rigidbody2D bullet1Rigidbody = bullet1GO.GetComponent<Rigidbody2D>();
        GameObject bullet2GO = Instantiate(BulletPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
        Rigidbody2D bullet2Rigidbody = bullet2GO.GetComponent<Rigidbody2D>();
        bullet1Rigidbody.velocity = bullet1Movement;
        bullet2Rigidbody.velocity = bullet2Movement;
    }

    private void HandleInvincible()
    {
        if (invincibleTimer > 0)
        {
            invincibleTimer -= Time.deltaTime;
            bool flashOn = (int)Mathf.Floor(invincibleTimer / .1f) % 2 == 1;
            if (invincibleTimer < 0)
            {
                flashOn = false;
            }
            playerRenderer.color = flashOn ? new Color(240f/255f, 165f/255f, 0) : Color.white;
        }
    }

    public void HitPlayer(float damage)
    {
        if (invincibleTimer <= 0)
        {
            life -= damage;
            UpdateLifeBar();
            if (life <= 0)
                KillPlayer();
            else
                invincibleTimer = invincibleTimerMax;
        }
    }

    void UpdateLifeBar()
    {
        float lifePercent = life / lifeMax;
        float maxWidth = 40f;
        float currentWidth = maxWidth * lifePercent;
        LifeBar.transform.localScale = new Vector3(currentWidth, LifeBar.transform.localScale.y, LifeBar.transform.localScale.z);
        float startPos = -.2f;
        float extent = .2f;
        float currentPos = startPos + extent * lifePercent;
        LifeBar.transform.localPosition = new Vector3(currentPos, LifeBar.transform.localPosition.y, LifeBar.transform.localPosition.z);
    }

    public void KillPlayer()
    {
        this.GetComponent<Collider2D>().enabled = false;
        playerAnimator.Play("et-dead");
        isAlive = false;
        GunGO.SetActive(false);
        MuzzleGO.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Candy candy = collider.gameObject.GetComponent<Candy>();
        if (candy != null && isAlive)
        {
            CollectCandy();
            Destroy(candy.gameObject);
        }
        Phone phone = collider.gameObject.GetComponent<Phone>();
        if (phone != null && isAlive)
        {
            CollectPhone();
            Destroy(phone.gameObject);
        }
    }

    void CollectCandy()
    {
        currentExp+=20f;
        float maxExp = currentLevel < maxExperiences.Length ? maxExperiences[currentLevel] : maxExperiences[maxExperiences.Length - 1];
        currentExp = Mathf.Min(maxExp, currentExp);
        if (currentExp == maxExp)
        {
            currentLevel++;
            currentExp = 0;
            ExpLevel.text = "LVL " + (currentLevel + 1);
        }
        ExpBar.sizeDelta = new Vector2 ((currentExp / maxExp) * maxExpBarWidth, ExpBar.sizeDelta.y);
    }

    void CollectPhone()
    {
        currentPhonePieces++;
        if (currentPhonePieces == maxPhonePieces)
        {
            currentPhonePieces = 0;
        }
        for (int x = 0; x < PhonePieces.Length; x++)
        {
            PhonePieces[x].SetActive(x < currentPhonePieces);
        }
    }
}
