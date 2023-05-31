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
    RectTransform[] Buttons;
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

    bool stickDown = false;
    bool controllerAttached = false;
    int highlightIndex = 1;

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
        bool moveLeft = false;
        bool moveRight = false;
        bool selectButton = false;
        if (controllerAttached)
        {
            if (Input.GetButton("Fire1"))
            {
                selectButton = true;
            }

            float controllerLeftStickX;
            controllerLeftStickX = Input.GetAxis("Horizontal");
            if (controllerLeftStickX > .5f)
            {
                if (!stickDown) moveRight = true;
                stickDown = true;
            }
            else if (controllerLeftStickX < -.5f)
            {
                if (!stickDown) moveLeft = true;
                stickDown = true;
            }
            else
            {
                stickDown = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown("a"))
            moveLeft = true;
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("d"))
            moveRight = true;
        if (Input.GetKeyDown("space"))
            selectButton = true;

        if (selectButton)
        {
            if (highlightIndex == 0)
                SelectStart();
            else if (highlightIndex == 1)
                SelectStart();
            else if (highlightIndex == 2)
                SelectStart();
        }
        if (moveLeft)
        {
            highlightIndex--;
            if (highlightIndex < 0)
                highlightIndex = Buttons.Length - 1;

            audioManager.PlayMenuSound();
            HighlightButton();
        }
        else if (moveRight)
        {
            highlightIndex++;
            if (highlightIndex >= Buttons.Length)
                highlightIndex = 0;

            audioManager.PlayMenuSound();
            HighlightButton();
        }
    }

    private void HighlightButton()
    {
        for (int x = 0; x < Buttons.Length; x++)
        {
            if (x == highlightIndex)
            {
                Buttons[x].sizeDelta = new Vector2 (420f, 120f);
            }
            else
            {
                Buttons[x].sizeDelta = new Vector2 (400f, 100f);
            }
        }
    }

    public void SelectTutorial()
    {

    }

    public void SelectStats()
    {

    }

    public void SelectStart()
    {
        if (fadeOut) return;
        audioManager.PlayButtonSound();
        fadeManager.StartFadeOut();
        fadeOut = true;
    }
}
