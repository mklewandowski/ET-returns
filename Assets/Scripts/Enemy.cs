using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    bool isActive = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        if (bullet != null && isActive)
        {
            KillEnemy(collider);
        }
    }

    public void KillEnemy(Collider2D collider)
    {
        this.GetComponent<Collider2D>().enabled = false;
        isActive = false;
        Destroy(this.gameObject);
    }
}
