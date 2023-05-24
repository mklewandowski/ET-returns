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

    [SerializeField]
    GameObject[] UnlockPanels;
    [SerializeField]
    TextMeshProUGUI[] UnlockPanelNames;
    [SerializeField]
    Image[] UnlockPanelETimages;
    [SerializeField]
    Image[] UnlockPanelETguns;
    [SerializeField]
    Sprite[] ETsprites;
    [SerializeField]
    Sprite[] ETgunSprites;

    [SerializeField]
    GameObject NextButton;

    float unlockTimer = 0;
    float unlockTimerMax = 3f;

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

    void DisplayUnlockedCharacters()
    {
        if (Globals.UnlockedCharacters.Count > 0)
        {
            audioManager.PlayFanfareSound();
        }
        for (int x = 0; x < Globals.UnlockedCharacters.Count; x++)
        {
            Globals.PlayerTypes playerType = Globals.UnlockedCharacters[x];
            Debug.Log(playerType);
            int characterIndex = (int)playerType;
            UnlockPanelETimages[x].sprite = ETsprites[characterIndex];
            UnlockPanelETguns[x].sprite = ETgunSprites[characterIndex];
            UnlockPanelNames[x].text = Globals.PlayerNames[characterIndex];
            UnlockPanels[x].transform.localScale = new Vector3(.1f, .1f, .1f);
            UnlockPanels[x].SetActive(true);
            UnlockPanels[x].GetComponent<GrowAndShrink>().StartEffect();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn && fadeManager.FadeComplete())
        {
            fadeIn = false;

            StatsText.GetComponent<TypewriterUI>().StartEffect(textToType);
            unlockTimer = unlockTimerMax;
        }
        if (fadeOut && fadeManager.FadeComplete())
        {
            fadeOut = false;
            SceneManager.LoadScene(sceneToLoad);
        }
        if (unlockTimer > 0)
        {
            unlockTimer -= Time.deltaTime;
            if (unlockTimer <= 0)
            {
                DisplayUnlockedCharacters();
                NextButton.SetActive(true);
                NextButton.GetComponent<MoveNormal>().MoveUp();
            }
        }
        HandleInput();
    }

    void HandleInput()
    {
        if (!NextButton.activeSelf)
            return;
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
