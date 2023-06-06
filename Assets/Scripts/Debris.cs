using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    bool isActive = false;

    float lifeTimer = 1f;

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0)
            {
                DeActivate();
            }
        }
    }

    public void Activate(Vector3 pos)
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
        float randomAngle = Random.Range(0f, 2 * Mathf.PI);
        Vector2 normalizedPos = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        Vector2 scaledNormalizedPos = normalizedPos * Random.Range (2.0f, 4.0f);
        float newScale = Random.Range(2f, 4f);
        this.transform.localScale = new Vector2(newScale, newScale);

        this.transform.localPosition = pos;
        this.gameObject.SetActive(true);
        this.GetComponent<Rigidbody2D>().velocity = scaledNormalizedPos;
        isActive = true;
    }

    public void BossInit(Color debrisColor)
    {
        lifeTimer = Random.Range(.2f, .5f);
        this.GetComponent<SpriteRenderer>().color = debrisColor;
        float randomAngle = Random.Range(0f, Mathf.PI);
        Vector2 normalizedPos = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        Vector2 scaledNormalizedPos = normalizedPos * Random.Range (3.0f, 5.0f);
        float newScale = Random.Range(4f, 6f);
        this.transform.localScale = new Vector2(newScale, newScale);
        this.GetComponent<Rigidbody2D>().velocity = scaledNormalizedPos;
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
