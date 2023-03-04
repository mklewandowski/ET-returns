using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    FadeManager fadeManager;
    [SerializeField]
    GameObject animationPanel;

    bool fadeIn = false;
    bool fadeOut = false;
    string sceneToLoad = "GameScene";

    bool controllerAttached = false;

    void Awake()
    {
        string[] controllers = Input.GetJoystickNames();
        for (int x = 0; x < controllers.Length; x++)
        {
            if (controllers[x] != "")
                controllerAttached = true;
        }

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
        if (animationPanel.transform.localPosition.x >= 2000f)
        {
            animationPanel.transform.localPosition = new Vector2(-1500f, animationPanel.transform.localPosition.y);
            animationPanel.GetComponent<MoveNormal>().MoveRight();
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
    }

    public void SelectStart()
    {
        fadeManager.StartFadeOut();
        fadeOut = true;
    }
}