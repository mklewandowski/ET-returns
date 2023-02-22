using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public void Init()
    {
        float randVal = Random.Range(0, 100f);
        Color newColor = Color.white;
        if (randVal < 33f)
            newColor = new Color(255f/255f, 56f/255f, 1f/255f); // orange
        else if (randVal < 66f)
            newColor = new Color(251f/255f, 238f/255f, 1f/255f); // yellow
        else
            newColor = new Color(82f/255f, 24f/255f, 0f/255f); // brown
        this.GetComponent<SpriteRenderer>().color = newColor;
    }
}
