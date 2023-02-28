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
    }

    public void SelectStart()
    {
        fadeManager.StartFadeOut();
        fadeOut = true;
    }
}
