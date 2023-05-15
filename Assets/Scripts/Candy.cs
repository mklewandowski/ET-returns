using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    bool isActive = false;

    public void Activate(Vector3 pos)
    {
        Color[] candyColors = new Color[]
        {
            new Color(255f/255f, 56f/255f, 1f/255f), // orange FF3801
            new Color(251f/255f, 238f/255f, 1f/255f), // yellow FBEE01
            new Color(82f/255f, 24f/255f, 0f/255f) // brown 521800
        };
        Color newColor = candyColors[Random.Range(0, candyColors.Length)];
        this.GetComponent<SpriteRenderer>().color = newColor;
        this.transform.localPosition = pos;
        isActive = true;
        this.gameObject.SetActive(true);
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
