using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [SerializeField]
    Image FadeOverlay;

    float fadeTimerMax = .5f;
    float fadeTimer = 0;
    bool fadeIn = true;

    // Update is called once per frame
    void Update()
    {
        if (fadeTimer > 0)
        {
            fadeTimer -= Time.deltaTime;
            if (fadeTimer <= 0)
            {
                fadeTimer = 0;
                if (fadeIn)
                    FadeOverlay.gameObject.SetActive(false);
            }

            if (fadeIn)
            {
                FadeOverlay.color = new Color (0, 0, 0, fadeTimer * 2f);
            }
            else
            {
                FadeOverlay.color = new Color (0, 0, 0, (.5f - fadeTimer) * 2f);
            }
        }
    }

    public bool FadeComplete()
    {
        return fadeTimer <= 0;
    }

    public void StartFadeIn()
    {
        fadeTimer = fadeTimerMax;
        fadeIn = true;
        FadeOverlay.gameObject.SetActive(true);
    }

    public void StartFadeOut()
    {
        fadeTimer = fadeTimerMax;
        fadeIn = false;
        FadeOverlay.gameObject.SetActive(true);
    }
}
