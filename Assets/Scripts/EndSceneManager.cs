using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndSceneManager : MonoBehaviour
{
    [SerializeField]
    FadeManager fadeManager;
    [SerializeField]
    GameObject StatsText;

    bool fadeIn = false;
    bool fadeOut = false;
    string sceneToLoad = "TitleScene";
    string textToType = "SURVIVAL TIME: 3m 23s\n\nEARTHLINGS ELIMINATED: 39\n\nCANDY COLLECTED: 34";

    void Awake()
    {
        fadeManager.StartFadeIn();
        fadeIn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn && fadeManager.FadeComplete())
        {
            fadeIn = false;

            StatsText.GetComponent<TypewriterUI>().StartEffect(textToType);
        }
        if (fadeOut && fadeManager.FadeComplete())
        {
            fadeOut = false;
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    public void SelectStart()
    {
        fadeManager.StartFadeOut();
        fadeOut = true;
    }
}
