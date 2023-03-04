using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicDebris : MonoBehaviour
{
    float lifeTimer = 1f;

    void Awake()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer < 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Init()
    {
        lifeTimer = Random.Range(.75f, 1f);
        float randomAngle = Random.Range(0f, 360f);
        Vector2 normalizedPos = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        Vector2 scaledNormalizedPos = normalizedPos * Random.Range (2.0f, 4.0f);
        float newScale = Random.Range(4f, 6f);
        this.transform.localScale = new Vector2(newScale, newScale);
        this.GetComponent<Rigidbody2D>().velocity = scaledNormalizedPos;
    }
}
