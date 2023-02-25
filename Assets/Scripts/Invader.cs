using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    [SerializeField]
    Sprite[] InvaderSprites;

    public void Init(Vector3 centerPos)
    {
        this.GetComponent<SpriteRenderer>().sprite = InvaderSprites[Random.Range(0, InvaderSprites.Length)];
        this.transform.localPosition = new Vector2(Random.Range(centerPos.x - 3f, centerPos.x + 3f), centerPos.y + 6f);
    }

    void Update()
    {
        float newY = transform.localPosition.y + -5f * Time.deltaTime;
        Vector3 newPos = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        transform.localPosition = newPos;
    }
}
