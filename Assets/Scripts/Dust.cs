using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    bool isActive = false;

    [SerializeField]
    SpriteRenderer dustRenderer;

    float timer = 0f;
    float timerMax = .6f;

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            timer -= Time.deltaTime;
            dustRenderer.color = new Color(1f, 1f ,1f, .6f * timer / timerMax);
            if (timer <= 0)
                DeActivate();
        }
    }

    public void Activate(Vector3 pos)
    {
        this.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360f));
        this.transform.localPosition = new Vector3(pos.x + Random.Range(-.1f, .1f), pos.y - .37f, pos.z);
        float newScale = Random.Range(2f, 4f);
        this.transform.localScale = new Vector3(newScale, newScale, 1f);
        this.gameObject.SetActive(true);
        timer = timerMax + Random.Range(-.1f, .1f);
        isActive = true;
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
