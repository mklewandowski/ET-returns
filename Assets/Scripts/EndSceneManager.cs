using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndSceneManager : MonoBehaviour
{
    AudioManager audioManager;

    [SerializeField]
    GameObject AudioManagerPrefab;

    [SerializeField]
    FadeManager fadeManager;
    [SerializeField]
    GameObject StatsText;

    bool fadeIn = false;
    bool fadeOut = false;
    string sceneToLoad = "TitleScene";
    string textToType = "";

    bool controllerAttached = false;

    void Awake()
    {
        Application.targetFrameRate = 60;
        GameObject am = GameObject.Find("AudioManager");
        if (am)
            audioManager = am.GetComponent<AudioManager>();
        else
        {
            GameObject ami = Instantiate(AudioManagerPrefab);
            ami.name = "AudioManager";
            audioManager = ami.GetComponent<AudioManager>();
        }

        string[] controllers = Input.GetJoystickNames();
        for (int x = 0; x < controllers.Length; x++)
        {
            if (controllers[x] != "")
                controllerAttached = true;
        }

        fadeManager.StartFadeIn();
        fadeIn = true;

        int timeInSeconds = (int)Globals.gameTime;
        int min = (int)(timeInSeconds / 60f);
        int sec = timeInSeconds - (min * 60);
        string secPadded = sec < 10 ? "0" + sec : sec.ToString();
        textToType = "SURVIVAL TIME: " + min + ":" + secPadded + "\n\nEARTHLINGS ELIMINATED: " + Globals.killCount + "\n\nCANDIES COLLECTED: " + Globals.candyCount;
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
        HandleInput();
    }

    void HandleInput()
    {
        if (controllerAttached)
        {
            if (Input.GetButton("Fire1"))
                SelectStart();
        }
        if (Input.GetKeyDown("space"))
            SelectStart();
    }

    public void SelectStart()
    {
        if (fadeOut) return;
        audioManager.PlayButtonSound();
        fadeManager.StartFadeOut();
        fadeOut = true;
    }
}
