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
    string textToType = "";

    void Awake()
    {
        fadeManager.StartFadeIn();
        fadeIn = true;

        int timeInSeconds = (int)Globals.gameTime;
        int min = (int)(timeInSeconds / 60f);
        int sec = timeInSeconds - (min * 60);
        string secPadded = sec < 10 ? "0" + sec : sec.ToString();
        textToType = "SURVIVAL TIME: " + min + "m " + secPadded + "s\n\nEARTHLINGS ELIMINATED: " + Globals.killCount + "\n\nCANDIES COLLECTED: " + Globals.candyCount;
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
