using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakoutBall : MonoBehaviour
{
    [SerializeField]
    GameObject FadePrefab;

    float timer = .1f;
    float timerMax = .075f;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = timerMax;
            Instantiate(FadePrefab, this.transform.localPosition, Quaternion.identity);
        }

    }
}
