using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject playerGO;

    float moveSpeed = 2f;
    Vector2 movementVector = new Vector2(0, 0);
    bool moveLeft;
    bool moveRight;
    bool moveUp;
    bool moveDown;
    private Rigidbody2D playerRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        moveRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        moveUp = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        moveDown = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
        movementVector = new Vector2(0, 0);

        if (moveRight)
        {
            movementVector.x = moveSpeed;
            playerGO.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (moveLeft)
        {
            movementVector.x = moveSpeed * -1f;
            playerGO.transform.localEulerAngles = new Vector3(0, 180f, 0);
        }

        if (moveUp)
            movementVector.y = moveSpeed;
        else if (moveDown)
            movementVector.y = moveSpeed * -1f;

        playerRigidbody.velocity = movementVector;
    }
}
