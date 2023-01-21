using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Transform playerTransform;

    bool isActive = true;

    private Rigidbody2D enemyRigidbody;
    float moveSpeed = 1f;
    Vector2 movementVector = new Vector2(0, 0);

    float positionTimer = .1f;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (positionTimer > 0)
        {
            positionTimer -= Time.deltaTime;
            if (positionTimer < 0)
            {
                movementVector = playerTransform.position - this.transform.localPosition;
                movementVector = movementVector * moveSpeed;
                positionTimer = Random.Range(.2f, .5f);
            }
        }
        enemyRigidbody.velocity = movementVector;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        if (bullet != null && isActive)
        {
            KillEnemy(collider);
        }
    }

    public void KillEnemy(Collider2D collider)
    {
        this.GetComponent<Collider2D>().enabled = false;
        isActive = false;
        Destroy(this.gameObject);
    }
}
