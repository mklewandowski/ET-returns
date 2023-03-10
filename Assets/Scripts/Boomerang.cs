using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    Transform playerTransform;
    Rigidbody2D boomerangRigidbody;

    Vector2 movementVector;
    bool movingTowardPlayer = false;
    float movementTimer = 1f;
    float movementTimerMax = .5f;

    // Start is called before the first frame update
    void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
        boomerangRigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        int index = (int)Globals.UpgradeTypes.Boomerang * Globals.MaxLevelsPerUpgrade + Globals.CurrentUpgradeLevels[(int)Globals.UpgradeTypes.Boomerang] - 1;
        float movementTimerMax = Globals.UpgradeLevelAttackTimes[index];
        movementTimer = movementTimerMax;
    }

    // Update is called once per frame
    void Update()
    {
        movementTimer -= Time.deltaTime;
        if (movementTimer <= 0)
        {
            if (!movingTowardPlayer)
            {
                boomerangRigidbody.velocity = Vector2.zero;
                movementTimer = movementTimerMax;
                movingTowardPlayer = true;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        if (movingTowardPlayer)
        {
            float dist = Vector3.Distance(playerTransform.position, this.transform.position);
            if (Mathf.Abs(dist) < .5f)
                Destroy(this.gameObject);
            movementVector = (playerTransform.position - this.transform.localPosition).normalized * 8f;
            boomerangRigidbody.velocity = movementVector;
        }
    }
}
