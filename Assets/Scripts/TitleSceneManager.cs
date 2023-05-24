using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    AudioManager audioManager;

    [SerializeField]
    GameObject AudioManagerPrefab;

    [SerializeField]
    FadeManager fadeManager;
    [SerializeField]
    GameObject animationPanel;
    [SerializeField]
    GameObject[] tutorialPanels;
    int currTutorial = 0;
    float tutorialGapTimer = 0f;
    float tutorialGapTimerMax = 2f;
    float showTutorialTimer = 0f;
    float showTutorialTimerMax = 4f;

    bool fadeIn = false;
    bool fadeOut = false;
    string sceneToLoad = "SelectScene";

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

        Globals.LoadGameStateFromPlayerPrefs();

        fadeManager.StartFadeIn();
        fadeIn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn && fadeManager.FadeComplete())
        {
            fadeIn = false;
            animationPanel.GetComponent<MoveNormal>().MoveRight();
        }
        if (fadeOut && fadeManager.FadeComplete())
        {
            fadeOut = false;
            SceneManager.LoadScene(sceneToLoad);
        }
        if (animationPanel.transform.localPosition.x >= 1600f)
        {
            animationPanel.transform.localPosition = new Vector2(-1500f, animationPanel.transform.localPosition.y);
            animationPanel.GetComponent<MoveNormal>().StopMove();
            currTutorial = 0;
            tutorialPanels[currTutorial].GetComponent<MoveNormal>().MoveUp();
            tutorialGapTimer = tutorialGapTimerMax;
        }
        if (tutorialGapTimer > 0)
        {
            tutorialGapTimer -= Time.deltaTime;
            if (tutorialGapTimer <= 0)
            {
                currTutorial++;
                if (currTutorial >= tutorialPanels.Length)
                {
                    showTutorialTimer = showTutorialTimerMax;
                }
                else
                {
                    tutorialPanels[currTutorial].GetComponent<MoveNormal>().MoveUp();
                    tutorialGapTimer = tutorialGapTimerMax;
                }
            }
        }
        if (showTutorialTimer > 0)
        {
            showTutorialTimer -= Time.deltaTime;
            if (showTutorialTimer <= 0)
            {
                animationPanel.GetComponent<MoveNormal>().MoveRight();
                foreach (GameObject tutorial in tutorialPanels)
                {
                    tutorial.GetComponent<MoveNormal>().MoveDown();
                }
            }
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
