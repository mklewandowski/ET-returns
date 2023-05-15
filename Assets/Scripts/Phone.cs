using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    bool isActive = false;

    [SerializeField]
    Sprite[] PhoneSprites;

    [SerializeField]
    SpriteRenderer phoneRenderer;

    public void Activate(Vector3 pos)
    {
        int randVal = Random.Range(0, PhoneSprites.Length);
        phoneRenderer.sprite = PhoneSprites[randVal];
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
