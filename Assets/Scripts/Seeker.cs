using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : MonoBehaviour
{
    Rigidbody2D bulletRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        bulletRigidbody = this.GetComponent<Rigidbody2D>();
        UpdateDirection();
    }

    void UpdateDirection()
    {
        Enemy[] enemyObjects = GameObject.FindObjectsOfType<Enemy>(false);
        float closestDist = 999f;
        int attackIndex = -1;
        for (int i = 0; i < enemyObjects.Length; i++)
        {
            float dist = Vector2.Distance(this.transform.localPosition, enemyObjects[i].gameObject.transform.localPosition);
            if (dist < closestDist)
            {
                closestDist = dist;
                attackIndex = i;
            }
        }
        if (attackIndex >= 0)
        {
             Vector3 bulletMovement = enemyObjects[attackIndex].gameObject.transform.localPosition - this.transform.localPosition;
             bulletMovement = Vector3.Normalize(bulletMovement) * 8f;
             bulletRigidbody.velocity = bulletMovement;
        }
    }
}
