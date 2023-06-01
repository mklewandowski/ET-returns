using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    bool isActive = false;
    float moveTimer = 0f;
    float moveTimerMax = .25f;
    Vector3 moveDir;
    float speed = .2f;
    float startSpeed = .2f;

    void Update()
    {
        if (moveTimer > 0)
        {
            moveTimer -= Time.deltaTime;
            this.transform.localPosition = this.transform.localPosition + moveDir * speed;
        }
    }

    public void Activate(Vector3 pos)
    {
        moveTimer = 0f;
        Color[] candyColors = new Color[]
        {
            new Color(255f/255f, 56f/255f, 1f/255f), // orange FF3801
            new Color(251f/255f, 238f/255f, 1f/255f), // yellow FBEE01
            new Color(82f/255f, 24f/255f, 0f/255f) // brown 521800
        };
        Color newColor = candyColors[Random.Range(0, candyColors.Length)];
        this.GetComponent<SpriteRenderer>().color = newColor;
        this.transform.localPosition = pos;
        isActive = true;
        this.gameObject.SetActive(true);
    }

    public void StartMove(Vector3 dir)
    {
        moveTimer = moveTimerMax + Random.Range(-.1f, .1f);
        speed = startSpeed + Random.Range(-.02f, .02f);
        moveDir = dir;
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
}
