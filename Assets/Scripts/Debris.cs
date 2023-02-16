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
        if (lifeTimer < 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Init()
    {
        lifeTimer = Random.Range(.1f, .5f);
        float randVal = Random.Range(0, 100f);
        Color newColor = Color.white;
        if (randVal < 25f)
            newColor = new Color(255f/255f, 233f/255f, 127f/255f); // light yello
        else if (randVal < 50f)
            newColor = Color.yellow;
        else if (randVal < 75f)
            newColor = new Color(255f/255f, 178f/255f, 127f/255f); // light orange
        this.GetComponent<SpriteRenderer>().color = newColor;
        float randomAngle = Random.Range(0f, 360f);
        Vector2 normalizedPos = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        Vector2 scaledNormalizedPos = normalizedPos * Random.Range (2.0f, 4.0f);
        float newScale = Random.Range(1f, 3f);
        this.transform.localScale = new Vector2(newScale, newScale);
        this.GetComponent<Rigidbody2D>().velocity = scaledNormalizedPos;

    }

}
