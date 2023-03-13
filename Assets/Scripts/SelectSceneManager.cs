using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SelectSceneManager : MonoBehaviour
{
    AudioManager audioManager;

    [SerializeField]
    GameObject AudioManagerPrefab;

    [SerializeField]
    FadeManager fadeManager;

    [SerializeField]
    Image ETimage;
    [SerializeField]
    Image ETgun;
    [SerializeField]
    TextMeshProUGUI ETname;

    [SerializeField]
    Sprite[] ETsprites;
    [SerializeField]
    Sprite[] ETgunSprites;

    bool fadeIn = false;
    bool fadeOut = false;
    string sceneToLoad = "GameScene";

    bool controllerAttached = false;

	bool stickDown = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn && fadeManager.FadeComplete())
        {
            fadeIn = false;
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

        bool moveLeft = false;
        bool moveRight = false;
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
        if (moveLeft)
            SelectPrevious();
        else if (moveRight)
            SelectNext();
    }

    public void SelectStart()
    {
        if (fadeOut) return;
        audioManager.PlayButtonSound();
        fadeManager.StartFadeOut();
        fadeOut = true;
    }

    public void SelectNext()
    {
        if (fadeOut) return;
        audioManager.PlayButtonSound();
        int index = (int)Globals.currentPlayerType;
        index++;
        int numPlayerTypes = System.Enum.GetValues(typeof(Globals.PlayerTypes)).Length;
        if (index >= numPlayerTypes)
            index = 0;
        Globals.currentPlayerType = (Globals.PlayerTypes)index;
        UpdateET();
    }

    public void SelectPrevious()
    {
        if (fadeOut) return;
        audioManager.PlayButtonSound();
        int index = (int)Globals.currentPlayerType;
        index--;
        int numPlayerTypes = System.Enum.GetValues(typeof(Globals.PlayerTypes)).Length;
        if (index < 0)
            index = numPlayerTypes - 1;
        Globals.currentPlayerType = (Globals.PlayerTypes)index;
        UpdateET();
    }

    void UpdateET()
    {
        int index = (int)Globals.currentPlayerType;
        ETimage.sprite = ETsprites[index];
        ETgun.sprite = ETgunSprites[index];
        ETname.text = Globals.PlayerNames[index];
    }
}
