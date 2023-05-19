using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
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
        if (lifeTimer <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Init()
    {
        lifeTimer = Random.Range(.1f, .5f);
        Color[] debrisColors = new Color[]
        {
            new Color(255f/255f, 0/255f, 0/255f),
            new Color(251f/255f, 35f/255f, 35f/255f),
            new Color(251f/255f, 70f/255f, 70f/255f),
        };
        Color newColor = debrisColors[Random.Range(0, debrisColors.Length)];
        this.GetComponent<SpriteRenderer>().color = newColor;
        float randomAngle = Random.Range(0f, 360f);
        Vector2 normalizedPos = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        Vector2 scaledNormalizedPos = normalizedPos * Random.Range (2.0f, 4.0f);
        float newScale = Random.Range(2f, 4f);
        this.transform.localScale = new Vector2(newScale, newScale);
        this.GetComponent<Rigidbody2D>().velocity = scaledNormalizedPos;

    }

}
