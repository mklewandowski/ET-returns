using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitNotice : MonoBehaviour
{
    bool isActive = false;
    [SerializeField]
    TextMeshPro HitText;
    float displayTime = 0f;
    float displayTimeMax = 1f;
    float movingUpVelocity = .5f;

    // Update is called once per frame
    void Update()
    {
        float newY = transform.localPosition.y + movingUpVelocity * Time.deltaTime;
        Vector3 newPos = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        transform.localPosition = newPos;

        displayTime -=Time.deltaTime;
        if (displayTime <= 0)
        {
           DeActivate();
        }
    }

    public void Activate(Vector3 newPos, int damage)
    {
        isActive = true;
        this.transform.localPosition = newPos;
        HitText.text = damage.ToString();
        displayTime = displayTimeMax;
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
