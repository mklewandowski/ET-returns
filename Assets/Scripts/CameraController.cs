using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform Player;

    float shakeTimer = 0f;
    float shakeTimerMax = .5f;

    // Update is called once per frame
    void Update()
    {
        float shakeX = 0;
        float shakeY = 0;
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            shakeX = Random.Range(shakeTimer * -.25f, shakeTimer *.25f);
            shakeY = Random.Range(shakeTimer * -.25f, shakeTimer *.25f);
        }
        this.transform.position = new Vector3(Player.position.x + shakeX, Player.position.y + shakeY, this.transform.position.z);
    }

    public void ShakeCamera()
    {
        shakeTimer = shakeTimerMax;
    }
}
