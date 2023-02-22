using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public void Init()
    {
        Color[] candyColors = new Color[]
        {
            new Color(255f/255f, 56f/255f, 1f/255f), // orange
            new Color(251f/255f, 238f/255f, 1f/255f), // yellow
            new Color(82f/255f, 24f/255f, 0f/255f) // brown
        };
        Color newColor = candyColors[Random.Range(0, candyColors.Length)];
        this.GetComponent<SpriteRenderer>().color = newColor;
    }
}
