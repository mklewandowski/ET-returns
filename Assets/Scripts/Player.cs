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
    GameSceneManager GameSceneManagerScript;
    [SerializeField]
    GameObject PlayerGO;
    [SerializeField]
    GameObject GunGO;
    [SerializeField]
    GameObject MuzzleGO;
    [SerializeField]
    GameObject BulletPrefab;
    [SerializeField]
    GameObject BulletSwirlPrefab;
    [SerializeField]
    GameObject BulletBombPrefab;
    [SerializeField]
    GameObject InvaderPrefab;
    [SerializeField]
    GameObject GhostPrefab;
    [SerializeField]
    GameObject BulletContainer;
    [SerializeField]
    GameObject ForceField;
    [SerializeField]
    GameObject Laser;
    [SerializeField]
    GameObject[] SurroundObjects;

    float moveSpeed = 2.2f;
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
    int burstNum = 5;
    int burstNumMax = 5;
    float muzzleFlashTimer = 0f;
    float muzzleFlashTimerMax = .05f;
    float laserTimer = 0f;
    float surroundTimer = 0f;
    float surroundTimerOffMax = 4f;
    bool surroundOn = false;

    bool isAlive = true;
    float health = 20f;
    float invincibleTimer = 0f;
    float invincibleTimerMax = .75f;
    private SpriteRenderer playerRenderer;
    [SerializeField]
    GameObject HealthBar;
    [SerializeField]
    GameObject HealthBarBack;

    int currentPhonePieces = 0;
    int maxPhonePieces = 3;
    [SerializeField]
    GameObject[] PhonePieces;

	float controllerLeftStickX;
	float controllerLeftStickY;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = PlayerGO.GetComponent<Animator>();
        playerRenderer = PlayerGO.GetComponent<SpriteRenderer>();
        if (Globals.DebugMode)
            ForceField.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleShoot();
        HandleInvincible();
        HandleLaser();
        HandleSurround();
    }

    private void HandleMovement()
    {
        if (!isAlive || Globals.IsPaused) return;
        moveLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        moveRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        moveUp = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        moveDown = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);

        controllerLeftStickX = Input.GetAxis("Horizontal");
        controllerLeftStickY = Input.GetAxis("Vertical");
        if (controllerLeftStickX > .5f)
            moveRight = true;
        else if (controllerLeftStickX < -.5f)
            moveLeft = true;
        if (controllerLeftStickY > .5f)
            moveUp = true;
        else if (controllerLeftStickY < -.5f)
            moveDown = true;

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

            if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.SpreadShot] > 0 || Globals.DebugMode)
                HandleShootSpread(bulletMovement);
            if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.RearShot] > 0 || Globals.DebugMode)
                HandleShootRear(bulletMovement);
            if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.SideShot] > 0 || Globals.DebugMode)
                HandleShootSide(bulletMovement);
            if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Swirl] > 0 || Globals.DebugMode)
                HandleShootSwirl();
            if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Bomb] > 0 || Globals.DebugMode)
                HandleShootBomb();
            if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Laser] > 0 || Globals.DebugMode)
                HandleShootLaser();
            if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Invader] > 0 || Globals.DebugMode)
                HandleLaunchInvader();
            if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Ghost] > 0 || Globals.DebugMode)
                HandleLaunchGhost();

            burstNum = burstNum - 1;
            shootTimer = burstNum == 0 ? Globals.currentShootTimerMax : shootTimerBurstMax;
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

    private void HandleShootSpread(Vector2 bulletMovement)
    {
        int index = (int)Globals.UpgradeTypes.SpreadShot * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.SpreadShot] - 1;
        int spreadShots = Globals.UpgradeLevelBullets[index];
        MakeBullet(Quaternion.Euler(0, 0, -10f) * bulletMovement);
        MakeBullet(Quaternion.Euler(0, 0, 10f) * bulletMovement);
        if (spreadShots >= 4)
            MakeBullet(Quaternion.Euler(0, 0, 15f) * bulletMovement);
        if (spreadShots >= 5)
            MakeBullet(Quaternion.Euler(0, 0, -15f) * bulletMovement);
        if (spreadShots >= 6)
            MakeBullet(Quaternion.Euler(0, 0, 20f) * bulletMovement);
        if (spreadShots >= 7)
            MakeBullet(Quaternion.Euler(0, 0, -20f) * bulletMovement);
    }

    private void MakeBullet(Vector2 bulletMovement)
    {
        GameObject bulletGO = Instantiate(BulletPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
        Rigidbody2D bulletRigidbody = bulletGO.GetComponent<Rigidbody2D>();
        bulletRigidbody.velocity = bulletMovement;
    }

    private void HandleShootRear(Vector2 bulletMovement)
    {
        int index = (int)Globals.UpgradeTypes.RearShot * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.RearShot] - 1;
        if ((burstNumMax - burstNum) >= Globals.UpgradeLevelBullets[index])
            return;
        Vector2 bulletRearMovement = new Vector2(bulletMovement.x * -1f, bulletMovement.y * -1f);
        GameObject bulletRearGO = Instantiate(BulletPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
        Rigidbody2D bulletRearRigidbody = bulletRearGO.GetComponent<Rigidbody2D>();
        bulletRearRigidbody.velocity = bulletRearMovement;
    }

    private void HandleShootSide(Vector2 bulletMovement)
    {
        int index = (int)Globals.UpgradeTypes.SideShot * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.SideShot] - 1;
        if ((burstNumMax - burstNum) >= Globals.UpgradeLevelBullets[index])
            return;
        Vector2 bullet1Movement = Quaternion.Euler(0, 0, -90f) * bulletMovement;
        Vector2 bullet2Movement = Quaternion.Euler(0, 0, 90f) * bulletMovement;
        GameObject bullet1GO = Instantiate(BulletPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
        Rigidbody2D bullet1Rigidbody = bullet1GO.GetComponent<Rigidbody2D>();
        GameObject bullet2GO = Instantiate(BulletPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
        Rigidbody2D bullet2Rigidbody = bullet2GO.GetComponent<Rigidbody2D>();
        bullet1Rigidbody.velocity = bullet1Movement;
        bullet2Rigidbody.velocity = bullet2Movement;
    }

    private void HandleShootSwirl()
    {
        int index = (int)Globals.UpgradeTypes.Swirl * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Swirl] - 1;
        if ((burstNumMax - burstNum) >= Globals.UpgradeLevelBullets[index])
            return;
        Vector2 bulletMovement = Quaternion.Euler(0, 0, Random.Range(0, 360f)) * new Vector2(3f, 3f);
        GameObject bulletGO = Instantiate(BulletSwirlPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
        Rigidbody2D bulletRigidbody = bulletGO.GetComponent<Rigidbody2D>();
        bulletRigidbody.velocity = bulletMovement;
    }

    private void HandleShootBomb()
    {
        int index = (int)Globals.UpgradeTypes.Bomb * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Bomb] - 1;
        if ((burstNumMax - burstNum) >= Globals.UpgradeLevelBullets[index])
            return;
        Vector2 bulletMovement = Quaternion.Euler(0, 0, Random.Range(-25f, 25f)) * new Vector2(0, 5f);
        GameObject bulletGO = Instantiate(BulletBombPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
        Rigidbody2D bulletRigidbody = bulletGO.GetComponent<Rigidbody2D>();
        bulletRigidbody.velocity = bulletMovement;
    }

    private void HandleShootLaser()
    {
        if (burstNum < burstNumMax)
            return;
        Laser.SetActive(true);
        int index = (int)Globals.UpgradeTypes.Laser * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Laser] - 1;
        float laserTimerMax = Globals.UpgradeLevelAttackTimes[index];
        laserTimer = laserTimerMax;
    }

    private void HandleLaunchInvader()
    {
        int index = (int)Globals.UpgradeTypes.Invader * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Invader] - 1;
        if ((burstNumMax - burstNum) >= Globals.UpgradeLevelBullets[index])
            return;
        GameObject invaderGO = Instantiate(InvaderPrefab, new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + 100f, 0), Quaternion.identity, BulletContainer.transform);
        invaderGO.GetComponent<Invader>().Init(this.transform.localPosition);
    }

    private void HandleLaunchGhost()
    {
        int index = (int)Globals.UpgradeTypes.Ghost * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Ghost] - 1;
        if ((burstNumMax - burstNum) >= Globals.UpgradeLevelBullets[index])
            return;
        GameObject ghostGO = Instantiate(GhostPrefab, new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, 0), Quaternion.identity, BulletContainer.transform);
        ghostGO.GetComponent<Ghost>().Init(this.transform.localPosition);
    }

    private void HandleLaser()
    {
        if (laserTimer > 0)
        {
            laserTimer -= Time.deltaTime;
            if (laserTimer <= 0)
            {
                Laser.SetActive(false);
            }
        }
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

    private void HandleSurround()
    {
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Surround] > 0 || Globals.DebugMode)
        {
            surroundTimer -= Time.deltaTime;
            if (surroundTimer <= 0)
            {
                int index = (int)Globals.UpgradeTypes.Surround * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Surround] - 1;
                int numSurroundObjects = Globals.UpgradeLevelBullets[index];
                if (surroundOn)
                {
                    for (int x = 0; x < SurroundObjects.Length; x++)
                    {
                        SurroundObjects[x].SetActive(false);
                    }
                    surroundTimer = surroundTimerOffMax;
                    surroundOn = false;
                }
                else
                {
                    SurroundObjects[0].transform.localPosition = new Vector2(0, 1f);
                    SurroundObjects[1].transform.localPosition = numSurroundObjects < 3 ? new Vector2(0, -1f) : new Vector2(-.87f, -.5f);
                    SurroundObjects[2].transform.localPosition = new Vector2(.87f, -.5f);
                    for (int x = 0; x < SurroundObjects.Length; x++)
                    {
                        SurroundObjects[x].SetActive(numSurroundObjects > x);
                    }
                    float surroundTimerOnMax = Globals.UpgradeLevelAttackTimes[index];
                    surroundTimer = surroundTimerOnMax;
                    surroundOn = true;
                }
            }
        }
    }

    public void HitPlayer(float damage)
    {
        if (invincibleTimer <= 0)
        {
            float defenseAdjustedDamage = damage / Globals.currentDefense;
            health -= defenseAdjustedDamage;
            UpdateHealthBar();
            if (health <= 0)
                KillPlayer();
            else
                invincibleTimer = invincibleTimerMax;
        }
    }

    void UpdateHealthBar()
    {
        float healthPercent = health / Globals.currentMaxHealth;
        float maxWidth = Globals.currentMaxHealth * 2f;
        float currentWidth = maxWidth * healthPercent;
        HealthBar.transform.localScale = new Vector3(currentWidth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
        HealthBarBack.transform.localScale = new Vector3(maxWidth, HealthBarBack.transform.localScale.y, HealthBarBack.transform.localScale.z);
        float healthScale = Globals.currentMaxHealth / Globals.startMaxHealth;
        float startPos = -.2f * healthScale;
        float extent = .2f * healthScale;
        float currentPos = startPos + extent * healthPercent;
        HealthBar.transform.localPosition = new Vector3(currentPos, HealthBar.transform.localPosition.y, HealthBar.transform.localPosition.z);
    }

    public void KillPlayer()
    {
        playerRigidbody.velocity = new Vector2(0, 0);
        this.GetComponent<Collider2D>().enabled = false;
        playerAnimator.Play("et-dead");
        isAlive = false;
        GunGO.SetActive(false);
        MuzzleGO.SetActive(false);
        GameSceneManagerScript.GameOver();
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
        GameSceneManagerScript.AddExperience(20);
        Globals.candyCount++;
    }

    void CollectPhone()
    {
        currentPhonePieces++;
        if (currentPhonePieces == maxPhonePieces)
        {
            GameSceneManagerScript.ShowUpgradeSelection();
        }
        UpdateHUDPhone();
    }

    void UpdateHUDPhone()
    {
        for (int x = 0; x < PhonePieces.Length; x++)
        {
            PhonePieces[x].SetActive(x < currentPhonePieces);
        }
    }

    public void UpdateUpgrades()
    {
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.ForceField] > 0)
        {
            ForceField.SetActive(true);
            int index = (int)Globals.UpgradeTypes.ForceField * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.ForceField] - 1;
            float scale = Globals.UpgradeLevelAttackSizes[index];
            ForceField.transform.localScale = new Vector3(scale, scale, 1f);
        }
    }

    public void ResetHUDPhone()
    {
        currentPhonePieces = 0;
        UpdateHUDPhone();
    }

    public void RestoreMaxHealth()
    {
        health = Globals.currentMaxHealth;
        UpdateHealthBar();
    }
}
