using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    AudioManager audioManager;

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
    Sprite[] GunSprites;
    [SerializeField]
    GameObject MuzzleGO;
    [SerializeField]
    GameObject BulletPrefab;
    [SerializeField]
    GameObject BulletArrowPrefab;
    [SerializeField]
    GameObject BulletDartPrefab;
    [SerializeField]
    GameObject BulletSwirlPrefab;
    [SerializeField]
    GameObject BulletTornadoPrefab;
    [SerializeField]
    GameObject BulletBombPrefab;
    [SerializeField]
    GameObject BoomerangPrefab;
    [SerializeField]
    GameObject BulletSeekerPrefab;
    [SerializeField]
    GameObject InvaderPrefab;
    [SerializeField]
    GameObject GhostPrefab;
    [SerializeField]
    GameObject BeesPrefab;
    [SerializeField]
    GameObject PitPrefab;
    [SerializeField]
    GameObject ICBMPrefab;
    [SerializeField]
    GameObject SlimePrefab;
    [SerializeField]
    GameObject BreakoutPrefab;
    [SerializeField]
    GameObject BulletContainer;
    [SerializeField]
    GameObject ForceField;
    [SerializeField]
    GameObject Laser;
    [SerializeField]
    GameObject[] SurroundObjects;

    float moveSpeed = 2.4f;
    float moveSpeedInitial = 2.4f;
    Vector2 movementVector = new Vector2(0, 0);
    bool moveLeft;
    bool moveRight;
    bool moveUp;
    bool moveDown;
    private Rigidbody2D playerRigidbody;
    bool isMoving = false;
    Animator playerAnimator;
    float dustTimer = 0f;
    float dustTimerMax = .1f;

    public enum ShootType {
        Gun,
        Bomb,
        Swirl,
        Laser,
        Invader,
        Ghost,
        SeekerMissile,
        Tornado,
        Bees,
        Boomerang,
        Pit,
        ICBM,
        Breakout,
        Slime,
        None
    }

    ShootType[] shootTypes = {
        ShootType.Gun, ShootType.Gun, ShootType.Gun, ShootType.Gun, ShootType.Gun,
        ShootType.Bomb, ShootType.Swirl, ShootType.Laser, ShootType.Invader, ShootType.Ghost, ShootType.Breakout,
        ShootType.Gun, ShootType.Gun, ShootType.Gun, ShootType.Gun, ShootType.Gun,
        ShootType.ICBM, ShootType.Tornado, ShootType.Bees, ShootType.Boomerang, ShootType.Pit, ShootType.Slime
    };
    int shootIndex = 0;

    float shootTimer = 1f;
    float shootTimerMax = .2f;
    int burstNum = 5;
    int burstNumMax = 5;
    float muzzleFlashTimer = 0f;
    float muzzleFlashTimerMax = .075f;
    float laserTimer = 0f;
    float surroundTimer = 0f;
    float surroundTimerOffMax = 4f;
    bool surroundOn = false;

    bool isAlive = true;
    float health = 20f;
    float invincibleTimer = 0f;
    float invincibleTimerMax = .75f;
    private SpriteRenderer playerRenderer;
    private BoxCollider2D playerCollider;
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
        GameObject am = GameObject.Find("AudioManager");
        if (am)
            audioManager = am.GetComponent<AudioManager>();

        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerAnimator = PlayerGO.GetComponent<Animator>();
        playerRenderer = PlayerGO.GetComponent<SpriteRenderer>();
        if (Globals.DebugMode)
            ForceField.SetActive(true);

        playerAnimator.Play("et-idle" + Globals.AnimationSuffixes[(int)Globals.currentPlayerType]);
        SpriteRenderer gunSpriteRenderer = GunGO.GetComponent<SpriteRenderer>();
        gunSpriteRenderer.sprite = GunSprites[(int)Globals.currentPlayerType];
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleShoot();
        HandleInvincible();
        HandleLaser();
        HandleSurround();
        HandleDust();
        if (isAlive)
            Globals.gameTime += Time.deltaTime;
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
            playerAnimator.Play("et-walk" + Globals.AnimationSuffixes[(int)Globals.currentPlayerType]);
        }
        else if (movementVector.x == 0 && movementVector.y == 0 && isMoving)
        {
            isMoving = false;
            playerAnimator.Play("et-idle" + Globals.AnimationSuffixes[(int)Globals.currentPlayerType]);
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

    private void HandleDust()
    {
        dustTimer -= Time.deltaTime;
        if (dustTimer <= 0)
        {
            dustTimer = dustTimerMax;
            if (isMoving)
                GameSceneManagerScript.ActivateDustFromPool(this.transform.localPosition);
        }
    }

    private void HandleShoot()
    {
        if (!isAlive) return;
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            ShootType shootType = shootTypes[shootIndex];
            Vector2 bulletMovement = GetBulletMovement();
            if (shootType == ShootType.Gun)
            {
                audioManager.PlayPlayerShootSound();
                GameObject bulletGO = Instantiate(BulletPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
                Rigidbody2D bulletRigidbody = bulletGO.GetComponent<Rigidbody2D>();
                bulletRigidbody.velocity = bulletMovement;
                if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.SpreadShot] > 0 || Globals.DebugMode)
                    HandleShootSpread(bulletMovement, (burstNumMax - burstNum));
                if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.RearShot] > 0 || Globals.DebugMode)
                    HandleShootRear(bulletMovement, (burstNumMax - burstNum));
                if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.SideShot] > 0 || Globals.DebugMode)
                    HandleShootSide(bulletMovement, (burstNumMax - burstNum));
                if ((shootIndex > 10 && Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.SeekerMissile] > 0) || Globals.DebugMode)
                    HandleShootSeekerMissile((burstNumMax - burstNum));
                MuzzleGO.SetActive(true);
                muzzleFlashTimer = muzzleFlashTimerMax;

                burstNum = burstNum - 1;
                if (burstNum == 0) burstNum = burstNumMax;
            }
            else if (shootType == ShootType.Bomb && (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Bomb] > 0 || Globals.DebugMode))
                HandleShootBomb();
            else if (shootType == ShootType.Swirl && (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Swirl] > 0 || Globals.DebugMode))
                HandleShootSwirl();
            else if (shootType == ShootType.Laser && (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Laser] > 0 || Globals.DebugMode))
                HandleShootLaser();
            else if (shootType == ShootType.Invader && (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Invader] > 0 || Globals.DebugMode))
                HandleLaunchInvader();
            else if (shootType == ShootType.Ghost && (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Ghost] > 0 || Globals.DebugMode))
                HandleLaunchGhost();
            else if (shootType == ShootType.Tornado && (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Tornado] > 0 || Globals.DebugMode))
                HandleShootTornado();
            else if (shootType == ShootType.Bees && (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Bees] > 0 || Globals.DebugMode))
                HandleLaunchBees();
            else if (shootType == ShootType.Boomerang && (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Boomerang] > 0 || Globals.DebugMode))
                HandleShootBoomerang(bulletMovement);
            else if (shootType == ShootType.Pit && (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Pit] > 0 || Globals.DebugMode))
                HandleCreatePit(bulletMovement);
            else if (shootType == ShootType.ICBM && (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.ICBM] > 0 || Globals.DebugMode))
                HandleLaunchICBM();
            else if (shootType == ShootType.Slime && (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Slime] > 0 || Globals.DebugMode))
                HandleShootSlime();
            else if (shootType == ShootType.Breakout && (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Breakout] > 0 || Globals.DebugMode))
                HandleShootBreakout();

            shootTimer = shootTimerMax;
            shootIndex++;
            if (shootIndex >= shootTypes.Length)
                shootIndex = 0;
        }

        if (muzzleFlashTimer > 0)
        {
            muzzleFlashTimer -= Time.deltaTime;
            if (muzzleFlashTimer <= 0)
                MuzzleGO.SetActive(false);
        }
    }

    Vector2 GetBulletMovement()
    {
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
        return bulletMovement;
    }

    private void HandleShootSpread(Vector2 bulletMovement, int burstInterval)
    {
        int index = (int)Globals.UpgradeTypes.SpreadShot * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.SpreadShot] - 1;
        if (burstInterval >= Globals.UpgradeLevelBullets[index])
            return;
        MakeBullet(Quaternion.Euler(0, 0, -10f) * bulletMovement);
        MakeBullet(Quaternion.Euler(0, 0, 10f) * bulletMovement);
    }

    private void MakeBullet(Vector2 bulletMovement)
    {
        GameObject bulletGO = Instantiate(BulletPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
        Rigidbody2D bulletRigidbody = bulletGO.GetComponent<Rigidbody2D>();
        bulletRigidbody.velocity = bulletMovement;
    }

    private void HandleShootRear(Vector2 bulletMovement, int burstInterval)
    {
        int index = (int)Globals.UpgradeTypes.RearShot * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.RearShot] - 1;
        if (burstInterval >= Globals.UpgradeLevelBullets[index])
            return;
        Vector2 bulletRearMovement = new Vector2(bulletMovement.x * -1f, bulletMovement.y * -1f);
        GameObject bulletRearGO = Instantiate(BulletDartPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
        Rigidbody2D bulletRearRigidbody = bulletRearGO.GetComponent<Rigidbody2D>();
        bulletRearRigidbody.velocity = bulletRearMovement;
        float bulletAngle = Vector2.SignedAngle(new Vector2(1f, 0), bulletRearMovement);
        bulletRearGO.transform.localEulerAngles = new Vector3(0, 0, bulletAngle);
    }

    private void HandleShootSide(Vector2 bulletMovement, int burstInterval)
    {
        int index = (int)Globals.UpgradeTypes.SideShot * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.SideShot] - 1;
        if (burstInterval >= Globals.UpgradeLevelBullets[index])
            return;
        Vector2 bullet1Movement = Quaternion.Euler(0, 0, -90f) * bulletMovement;
        Vector2 bullet2Movement = Quaternion.Euler(0, 0, 90f) * bulletMovement;
        GameObject bullet1GO = Instantiate(BulletArrowPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
        Rigidbody2D bullet1Rigidbody = bullet1GO.GetComponent<Rigidbody2D>();
        GameObject bullet2GO = Instantiate(BulletArrowPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
        Rigidbody2D bullet2Rigidbody = bullet2GO.GetComponent<Rigidbody2D>();
        bullet1Rigidbody.velocity = bullet1Movement;
        bullet2Rigidbody.velocity = bullet2Movement;
        float bullet1Angle = Vector2.SignedAngle(new Vector2(1f, 0), bullet1Movement);
        float bullet2Angle = Vector2.SignedAngle(new Vector2(1f, 0), bullet2Movement);
        bullet1GO.transform.localEulerAngles = new Vector3(0, 0, bullet1Angle);
        bullet2GO.transform.localEulerAngles = new Vector3(0, 0, bullet2Angle);
    }

    private void HandleShootBoomerang(Vector2 bulletMovement)
    {
        audioManager.PlayPlayerShoot2Sound();
        int index = (int)Globals.UpgradeTypes.Boomerang * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Boomerang] - 1;
        int numShots = Globals.UpgradeLevelBullets[index];
        for (int x = 0; x < numShots; x++)
        {
            Vector2 boomerangMovement = Quaternion.Euler(0, 0, x == 0 ? 80f : -80f) * new Vector3(bulletMovement.x * .75f, bulletMovement.y * .75f);
            GameObject boomerangGO = Instantiate(BoomerangPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
            Rigidbody2D boomerangRigidbody = boomerangGO.GetComponent<Rigidbody2D>();
            boomerangRigidbody.velocity = boomerangMovement;
        }
    }

    private void HandleShootSlime()
    {
        audioManager.PlayPlayerShoot2Sound();
        int index = (int)Globals.UpgradeTypes.Slime * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Slime] - 1;
        int numDebris = 4;
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Slime] >= 4)
            numDebris = 8;
        else if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Slime] >= 2)
            numDebris = 6;
        int numShots = Globals.UpgradeLevelBullets[index];
        for (int x = 0; x < numShots; x++)
        {
            Vector2 bulletMovement = Quaternion.Euler(0, 0, Random.Range(0, 360f)) * new Vector2(3f, 3f);
            GameObject bulletGO = Instantiate(SlimePrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
            bulletGO.GetComponent<Bullet>().SetDebrisAmount(numDebris);
            Rigidbody2D bulletRigidbody = bulletGO.GetComponent<Rigidbody2D>();
            bulletRigidbody.velocity = bulletMovement;
        }
    }

    private void HandleShootBreakout()
    {
        audioManager.PlayPlayerShoot2Sound();
        int index = (int)Globals.UpgradeTypes.Breakout * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Breakout] - 1;
        int numShots = Globals.UpgradeLevelBullets[index];
        for (int x = 0; x < numShots; x++)
        {
            Vector2 bulletMovement = Quaternion.Euler(0, 0, Random.Range(0, 360f)) * new Vector2(3f, 3f);
            GameObject bulletGO = Instantiate(BreakoutPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
            Rigidbody2D bulletRigidbody = bulletGO.GetComponent<Rigidbody2D>();
            bulletRigidbody.velocity = bulletMovement;
        }
    }

    private void HandleShootSwirl()
    {
        audioManager.PlayPlayerShoot2Sound();
        int index = (int)Globals.UpgradeTypes.Swirl * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Swirl] - 1;
        int numShots = Globals.UpgradeLevelBullets[index];
        for (int x = 0; x < numShots; x++)
        {
            Vector2 bulletMovement = Quaternion.Euler(0, 0, Random.Range(0, 360f)) * new Vector2(3f, 3f);
            GameObject bulletGO = Instantiate(BulletSwirlPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
            Rigidbody2D bulletRigidbody = bulletGO.GetComponent<Rigidbody2D>();
            bulletRigidbody.velocity = bulletMovement;
        }
    }

    private void HandleShootBomb()
    {
        audioManager.PlayPlayerShoot2Sound();
        int index = (int)Globals.UpgradeTypes.Bomb * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Bomb] - 1;
        int numShots = Globals.UpgradeLevelBullets[index];
        for (int x = 0; x < numShots; x++)
        {
            Vector2 bulletMovement = Quaternion.Euler(0, 0, Random.Range(-25f, 25f)) * new Vector2(0, 5f);
            GameObject bulletGO = Instantiate(BulletBombPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
            Rigidbody2D bulletRigidbody = bulletGO.GetComponent<Rigidbody2D>();
            bulletRigidbody.velocity = bulletMovement;
        }
    }

    private void HandleShootLaser()
    {
        audioManager.PlayPlayerLaserSound();
        Laser.SetActive(true);
        int index = (int)Globals.UpgradeTypes.Laser * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Laser] - 1;
        float laserTimerMax = Globals.UpgradeLevelAttackTimes[index];
        laserTimer = laserTimerMax;
    }

    private void HandleLaunchInvader()
    {
        audioManager.PlayPlayerShoot2Sound();
        int index = (int)Globals.UpgradeTypes.Invader * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Invader] - 1;
        int numShots = Globals.UpgradeLevelBullets[index];
        for (int x = 0; x < numShots; x++)
        {
            GameObject invaderGO = Instantiate(InvaderPrefab, new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + 100f, 0), Quaternion.identity, BulletContainer.transform);
            invaderGO.GetComponent<Invader>().Init(this.transform.localPosition);
        }
    }

    private void HandleLaunchICBM()
    {
        audioManager.PlayPlayerShoot2Sound();
        int index = (int)Globals.UpgradeTypes.ICBM * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.ICBM] - 1;
        int numShots = Globals.UpgradeLevelBullets[index];
        for (int x = 0; x < numShots; x++)
        {
            GameObject icbmGO = Instantiate(ICBMPrefab, new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + 100f, 0), Quaternion.identity, BulletContainer.transform);
            icbmGO.GetComponent<ICBM>().Init(this.transform.localPosition);
        }
    }

    private void HandleLaunchGhost()
    {
        audioManager.PlayPlayerShoot2Sound();
        int index = (int)Globals.UpgradeTypes.Ghost * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Ghost] - 1;
        int numShots = Globals.UpgradeLevelBullets[index];
        for (int x = 0; x < numShots; x++)
        {
            GameObject ghostGO = Instantiate(GhostPrefab, new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, 0), Quaternion.identity, BulletContainer.transform);
            ghostGO.GetComponent<Ghost>().Init(this.transform.localPosition);
        }
    }

    private void HandleLaunchBees()
    {
        audioManager.PlayPlayerBeeSound();
        int index = (int)Globals.UpgradeTypes.Bees * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Bees] - 1;
        int numShots = Globals.UpgradeLevelBullets[index];
        for (int x = 0; x < numShots; x++)
        {
            GameObject beesGO = Instantiate(BeesPrefab, new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + 100f, 0), Quaternion.identity, BulletContainer.transform);
            beesGO.GetComponent<Bees>().Init(this.transform.localPosition);
        }
    }

    private void HandleCreatePit(Vector2 bulletMovement)
    {
        audioManager.PlayPlayerShoot2Sound();
        int index = (int)Globals.UpgradeTypes.Pit * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Pit] - 1;
        int numShots = Globals.UpgradeLevelBullets[index];
        for (int x = 0; x < numShots; x++)
        {
            Vector3 rotatedBulletMovement = new Vector3(bulletMovement.x, bulletMovement.y);
            if (x == 1)
                rotatedBulletMovement = Quaternion.Euler(0, 0, -60f) * bulletMovement;
            else if (x == 2)
                rotatedBulletMovement = Quaternion.Euler(0, 0, 60f) * bulletMovement;

            Vector3 behindVector = new Vector3(this.transform.localPosition.x + rotatedBulletMovement.x * -.1f, this.transform.localPosition.y + rotatedBulletMovement.y * -.1f, 0);

            GameObject pitGO = Instantiate(PitPrefab,
                behindVector,
                Quaternion.identity,
                BulletContainer.transform);
        }
    }

    private void HandleShootTornado()
    {
        audioManager.PlayPlayerShoot2Sound();
        int index = (int)Globals.UpgradeTypes.Tornado * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Tornado] - 1;
        int numShots = Globals.UpgradeLevelBullets[index];
        for (int x = 0; x < numShots; x++)
        {
            GameObject tornadoGO = Instantiate(BulletTornadoPrefab, new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + 100f, 0), Quaternion.identity, BulletContainer.transform);
            tornadoGO.GetComponent<Tornado>().Init(this.transform.localPosition);
        }
    }

    private void HandleShootSeekerMissile(int burstInterval)
    {
        int index = (int)Globals.UpgradeTypes.SeekerMissile * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.SeekerMissile] - 1;
        if (burstInterval >= Globals.UpgradeLevelBullets[index])
            return;
        GameObject bulletGO = Instantiate(BulletSeekerPrefab, MuzzleGO.transform.position, Quaternion.identity, BulletContainer.transform);
    }

    private void HandleLaser()
    {
        if (!isAlive) return;
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
            if (invincibleTimer <= 0)
            {
                flashOn = false;
                playerCollider.enabled = false;
                playerCollider.enabled = true;
            }
            playerRenderer.color = flashOn ? new Color(240f/255f, 165f/255f, 0) : Color.white;
        }
    }

    private void HandleSurround()
    {
        if (!isAlive) return;
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

    public void HitPlayer(int damage)
    {
        if (invincibleTimer <= 0)
        {
            audioManager.PlayPlayerHitSound();
            int defenseAdjustment = (int)(Mathf.Round((float)(Globals.currentDefense) / 2f));
            float defenseAdjustedDamage = Mathf.Max(1, damage - defenseAdjustment);
            health -= defenseAdjustedDamage;
            if (health < 0 )
                health = 0;
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
        audioManager.PlayPlayerDieSound();

        for (int x = 0; x < SurroundObjects.Length; x++)
        {
            SurroundObjects[x].SetActive(false);
        }
        ForceField.SetActive(false);

        // create debris
        int numDebris = Random.Range(10, 12);
        for (int x = 0; x < numDebris; x++)
        {
            GameSceneManagerScript.ActivateDebrisFromPool(this.transform.localPosition, true);
        }

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
            candy.DeActivate();
        }
        Phone phone = collider.gameObject.GetComponent<Phone>();
        if (phone != null && isAlive)
        {
            CollectPhone();
            phone.DeActivate();
        }
    }

    void CollectCandy()
    {
        audioManager.PlayCollectCandySound();
        GameSceneManagerScript.AddExperience(20);
        Globals.candyCount++;
    }

    void CollectPhone()
    {
        audioManager.PlayCollectPhoneSound();
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

    public void UpdateUpgrades(Globals.UpgradeTypes upgradeType)
    {
        if (Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.ForceField] > 0)
        {
            ForceField.SetActive(true);
            int index = (int)Globals.UpgradeTypes.ForceField * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.ForceField] - 1;
            float scale = Globals.UpgradeLevelAttackSizes[index];
            ForceField.transform.localScale = new Vector3(scale, scale, 1f);
        }
        if (upgradeType == Globals.UpgradeTypes.Speed && Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Speed] > 0)
        {
            int speedLevel = Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Speed];
            moveSpeed = moveSpeedInitial + (speedLevel * .12f);
        }
        else if (upgradeType == Globals.UpgradeTypes.Defense && Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Defense] > 0)
        {
            Globals.currentDefense = Globals.currentDefense + 1;
        }
        else if (upgradeType == Globals.UpgradeTypes.Attack && Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Attack] > 0)
        {
            Globals.currentAttack = Globals.currentAttack + 1;
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

    public void AdjustMoveSpeed(float multiplier)
    {
        moveSpeed = moveSpeed * multiplier;
    }
}
