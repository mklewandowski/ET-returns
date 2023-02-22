using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    [SerializeField]
    Sprite[] PhoneSprites;

    private SpriteRenderer phoneRenderer;

    void Awake()
    {
        phoneRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init()
    {
        int randVal = Random.Range(0, PhoneSprites.Length);
        phoneRenderer.sprite = PhoneSprites[randVal];
    }
}
