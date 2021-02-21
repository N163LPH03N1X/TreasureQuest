using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{

    public GameObject rainSystem;
    ParticleSystem rainPart;
    LoadingSystem loadingSystem;
    public GameObject loadingSysObj;
    AudioSource rainAmbience;
    public Vector3 setNewPosition;
    public Quaternion setNewRotation;
    public ScenePositions scenePosition;
    public SceneMusic sceneMusic;
    public LoadSceneMode loadMode;
    public GameObject MainMenu;
    public GameObject titleMode;
    public GameObject titleMenu;
    public GameObject playerController;
    public GameObject Player;
    public GameObject loadingGame;
    public GameObject gameMenu;
    public GameObject gameUI;
    public GameObject dialogueBanner;
    public GameObject timeSysObj;
    TimeSystem timeSys;
    public Text buttonText;
    public Image aButton;
    Image loadFillAmount;
    Transform newPosition;
    CharacterSystem playChar;
    public Image blackScreenGame;
    Vector3 textOrgPos;
    IEnumerator fadeAudio = null;
    IEnumerator screenBlack = null;
    IEnumerator sceneLoad = null;
    IEnumerator townPlaceRoutine;

    Vector3 freezePlayer = Vector3.zero;
    Scene game_Scene;

    bool alreadyNamed = false;
    public static int scene;

    public static bool isIntroScene;
    public static bool isOverWorld;
    public static bool isDungeon1;
    public static bool isDungeon2;
    public static bool isDungeon3;
    public static bool isTemple1;
    public static bool isGhostShip;
    public static bool isDwellingTimber;
    public static bool isPrototype;
   

    public bool titleActive = false;
    public bool introSceneActive = false;
    public bool dungeonOneActive = false;
    public bool dungeonTwoActive = false;
    public bool dungeonThreeActive = false;
    public bool TempleOneActive = false;
    public bool overWorldActive = false;
    public bool ghostShipActive = false;
    public bool dwellingTimberActive = false;
    public bool prototypeActive = false;

    bool dungeon1Entered = false;
    bool dungeon2Entered = false;
    bool dungeon3Entered = false;
    bool dungeon2AltEntrance = false;
    bool dungeon3AltEntrance = false;
    bool dungeon3Alt2Entrance = false;
    bool Temple1Entered = false;
    bool ghostShipEntered = false;
    bool introSceneEntered = true;
    bool dwellingTimberEntered = false;
    //bool 

    public static bool overWorldEntered = false;
    public static bool isDisabled = false;
    public static bool inTransition = true;
    public static bool isNewGame;
    public bool enablePlayer;
    public static bool isQuit = false;
    bool isLoad = false;
    public Text townPlaceName;

    public enum SceneActive { title, intro, overworld, dungeon1, dungeon2, Temple1, ghostShip, dwellingTimber, dungeon3, prototype }
    public void SetSceneActive(SceneActive sceneType)
    {
        switch (sceneType)
        {
            case SceneActive.title:
                {
                    isIntroScene = false;
                    isOverWorld = false;
                    isDungeon1 = false;
                    isDungeon2 = false;
                    isTemple1 = false;
                    isGhostShip = false;
                    isPrototype = false;
                    isDwellingTimber = false;
                    isDungeon3 = false;
                    break;
                }
            case SceneActive.intro:
                {
                    isOverWorld = false;
                    isIntroScene = true;
                    isDungeon1 = false;
                    isDungeon2 = false;
                    isTemple1 = false;
                    isGhostShip = false;
                    isPrototype = false;
                    isDwellingTimber = false;
                    isDungeon3 = false;
                    break;
                }
            case SceneActive.overworld:
                {
                    isIntroScene = false;
                    isOverWorld = true;
                    isDungeon1 = false;
                    isDungeon2 = false;
                    isTemple1 = false;
                    isGhostShip = false;
                    isPrototype = false;
                    isDwellingTimber = false;
                    isDungeon3 = false;
                    break;
                }
            case SceneActive.dungeon1:
                {
                    isIntroScene = false;
                    isOverWorld = false;
                    isDungeon1 = true;
                    isDungeon2 = false;
                    isTemple1 = false;
                    isGhostShip = false;
                    isPrototype = false;
                    isDwellingTimber = false;
                    isDungeon3 = false;
                    break;
                }
            case SceneActive.dungeon2:
                {
                    isIntroScene = false;
                    isOverWorld = false;
                    isDungeon1 = false;
                    isDungeon2 = true;
                    isTemple1 = false;
                    isGhostShip = false;
                    isPrototype = false;
                    isDwellingTimber = false;
                    isDungeon3 = false;
                    break;
                }
            case SceneActive.Temple1:
                {
                    isIntroScene = false;
                    isOverWorld = false;
                    isDungeon1 = false;
                    isDungeon2 = false;
                    isTemple1 = true;
                    isGhostShip = false;
                    isPrototype = false;
                    isDwellingTimber = false;
                    isDungeon3 = false;
                    break;
                }
            case SceneActive.ghostShip:
                {
                    isIntroScene = false;
                    isOverWorld = false;
                    isDungeon1 = false;
                    isDungeon2 = false;
                    isTemple1 = false;
                    isGhostShip = true;
                    isPrototype = false;
                    isDwellingTimber = false;
                    isDungeon3 = false;
                    break;
                }
            case SceneActive.prototype:
                {
                    isIntroScene = false;
                    isOverWorld = false;
                    isDungeon1 = false;
                    isDungeon2 = false;
                    isTemple1 = false;
                    isGhostShip = false;
                    isPrototype = true;
                    isDwellingTimber = false;
                    isDungeon3 = false;
                    break;
                }
            case SceneActive.dwellingTimber:
                {
                    isIntroScene = false;
                    isOverWorld = false;
                    isDungeon1 = false;
                    isDungeon2 = false;
                    isTemple1 = false;
                    isGhostShip = false;
                    isPrototype = false;
                    isDwellingTimber = true;
                    isDungeon3 = false;
                    break;
                }
            case SceneActive.dungeon3:
                {
                    isIntroScene = false;
                    isOverWorld = false;
                    isDungeon1 = false;
                    isDungeon2 = false;
                    isTemple1 = false;
                    isGhostShip = false;
                    isPrototype = false;
                    isDwellingTimber = false;
                    isDungeon3 = true;
                    break;
                }
        }
    }
    public bool cameraMode = false;
    private void Start()
    {
        if (enablePlayer)
        {
            SetPlaceName(Place.dungeon3, sceneMusic.dungeonThreeMusic);
            EnablePlayer(enablePlayer);
            inTransition = false;
        }
        textOrgPos = townPlaceName.GetComponent<RectTransform>().anchoredPosition;

    }

    public IEnumerator FadeTextOut(float timeFactor, Text currentText)
    {
        currentText.color =  new Color(currentText.color.r, currentText.color.g, currentText.color.b, 1);
        currentText.GetComponent<RectTransform>().anchoredPosition  = textOrgPos;
        while (currentText.color.a > 0.0f)
        {
            currentText.enabled = true;
            RectTransform move = currentText.GetComponent<RectTransform>();
           
            move.transform.Translate(Vector3.up * -Time.fixedDeltaTime * 10);
            currentText.color =  new Color(currentText.color.r, currentText.color.g, currentText.color.b, currentText.color.a - (Time.fixedDeltaTime / timeFactor));
            yield return null;
        }
        if (currentText.color.a <= 0.0f)
        {
            currentText.GetComponent<Text>().enabled = false;
            currentText.GetComponent<RectTransform>().anchoredPosition = textOrgPos;
        }
    }
 
    IEnumerator LoadNewScene()
    {
        loadingGame.SetActive(true);
        loadFillAmount = GameObject.Find("Core/Player/PlayerCanvas/LoadingGame/LoadBanner/Title/Top/").GetComponent<Image>();
        blackScreenGame.enabled = true;
        buttonText.text = "Loading...";
        aButton.enabled = false;
        loadFillAmount.fillAmount = 0;
        yield return new WaitForSecondsRealtime(5);
        AsyncOperation async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);

        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            loadFillAmount.fillAmount = Map(Mathf.RoundToInt(async.progress * 100), 0, 100, 0, 1);

            if (async.progress >= 0.9f)
            {
                loadFillAmount.fillAmount = 100;
                async.allowSceneActivation = true;
               
                //buttonText.text = "Continue";
                //aButton.enabled = true;
              
                //if (Input.GetButtonDown("Pause") || Input.GetButtonDown("Submit") || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
                //{
                    
                //    buttonText.text = null;
                //    aButton.enabled = false;
                //}
            }

            yield return null;
        }
        if (async.isDone)
            StartLevel(true);
    }
    public enum Level { Title, Intro, OverWorldStart, OverWorld, DungeonOne, DungeonTwo, TempleOne, GhostShip, Prototype, DungeonThree, DwellingTimber }
    public void StartScene(Level level)
    {
        if (CharacterSystem.isCarrying)
        {
            CharacterSystem character = playerController.GetComponent<CharacterSystem>();
            character.PlayerPickUpCarryObj(0);
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        inTransition = true;
        loadingSystem = loadingSysObj.GetComponent<LoadingSystem>();
        timeSys = timeSysObj.GetComponent<TimeSystem>();
        timeSys.isGameTimeRunning(false);
        EnablePlayer(false);
        GameObject enemyUIObjs = GameObject.Find("Core/Player/PopUpCanvas/EnemyUIStorage");
        foreach (Transform child in enemyUIObjs.transform)
            Destroy(child.gameObject);
        GameObject playerUIObjs = GameObject.Find("Core/Player/PopUpCanvas/PlayerUIStorage");
        foreach (Transform child in playerUIObjs.transform)
            Destroy(child.gameObject);
        switch (level)
        {
            case Level.Title:
                {
                    SetSceneActive(SceneActive.title);
                    isLoad = true;
                    gameUI.SetActive(false);
                    titleActive = true;
                    dungeonOneActive = false;
                    dungeonTwoActive = false;
                    TempleOneActive = false;
                    overWorldActive = false;
                    introSceneActive = false;
                    ghostShipActive = false;
                    prototypeActive = false;
                    dwellingTimberActive = false;
                    dungeonThreeActive = false;
                    if (screenBlack != null)
                        StopCoroutine(screenBlack);
                    screenBlack = FadeIn(1.0f, 4.0f, blackScreenGame, true);
                    StartCoroutine(screenBlack);
                    scene = 0;
       
                    break;
                }
            case Level.Intro:
                {
                    SetSceneActive(SceneActive.intro);
                    titleActive = false;
                    dungeonOneActive = false;
                    dungeonTwoActive = false;
                    TempleOneActive = false;
                    overWorldActive = false;
                    introSceneActive = true;
                    ghostShipActive = false;
                    prototypeActive = false;
                    dwellingTimberActive = false;
                    dungeonThreeActive = false;
                    scene = 6;

                    sceneLoad = LoadNewScene();
                    StartCoroutine(sceneLoad);
                    break;
                }
            case Level.OverWorld:
                {
                    SetSceneActive(SceneActive.overworld);
                    titleActive = false;
                    dungeonOneActive = false;
                    dungeonTwoActive = false;
                    TempleOneActive = false;
                    overWorldActive = true;
                    introSceneActive = false;
                    ghostShipActive = false;
                    prototypeActive = false;
                    dwellingTimberActive = false;
                    dungeonThreeActive = false;
                    scene = 2;

                    sceneLoad = LoadNewScene();
                    StartCoroutine(sceneLoad);
                    break;
                }
            case Level.DungeonOne:
                {
                    SetSceneActive(SceneActive.dungeon1);
                    rainPart = rainSystem.GetComponent<ParticleSystem>();
                    rainPart.Stop();
                    rainAmbience = rainSystem.GetComponent<AudioSource>();
                    rainAmbience.Stop();
                    sceneMusic.gameAmbience.Stop();
                    sceneMusic.gameAmbience.clip = null;
                    titleActive = false;
                    dungeonOneActive = true;
                    dungeonTwoActive = false;
                    TempleOneActive = false;
                    overWorldActive = false;
                    introSceneActive = false;
                    ghostShipActive = false;
                    prototypeActive = false;
                    dwellingTimberActive = false;
                    dungeonThreeActive = false;
                    scene = 1;

                    sceneLoad = LoadNewScene();
                    StartCoroutine(sceneLoad);
                    break;
                }
            case Level.DungeonTwo:
                {
                    SetSceneActive(SceneActive.dungeon2);
                    rainPart = rainSystem.GetComponent<ParticleSystem>();
                    rainPart.Stop();
                    rainAmbience = rainSystem.GetComponent<AudioSource>();
                    rainAmbience.Stop();
                    sceneMusic.gameAmbience.Stop();
                    sceneMusic.gameAmbience.clip = null;
                    titleActive = false;
                    dungeonTwoActive = true;
                    TempleOneActive = false;
                    dungeonOneActive = false;
                    overWorldActive = false;
                    introSceneActive = false;
                    ghostShipActive = false;
                    prototypeActive = false;
                    dwellingTimberActive = false;
                    dungeonThreeActive = false;
                    scene = 4;

                    sceneLoad = LoadNewScene();
                    StartCoroutine(sceneLoad);
                    break;
                }
            case Level.DungeonThree:
                {
                    SetSceneActive(SceneActive.dungeon3);
                    rainPart = rainSystem.GetComponent<ParticleSystem>();
                    rainPart.Stop();
                    rainAmbience = rainSystem.GetComponent<AudioSource>();
                    rainAmbience.Stop();
                    sceneMusic.gameAmbience.Stop();
                    sceneMusic.gameAmbience.clip = null;
                    titleActive = false;
                    dungeonOneActive = false;
                    dungeonTwoActive = false;
                    TempleOneActive = false;
                    overWorldActive = false;
                    introSceneActive = false;
                    ghostShipActive = false;
                    prototypeActive = false;
                    dwellingTimberActive = false;
                    dungeonThreeActive = true;
                    scene = 9;

                    sceneLoad = LoadNewScene();
                    StartCoroutine(sceneLoad);
                    break;
                }
            case Level.TempleOne:
                {
                    SetSceneActive(SceneActive.Temple1);
                    rainPart = rainSystem.GetComponent<ParticleSystem>();
                    rainPart.Stop();
                    rainAmbience = rainSystem.GetComponent<AudioSource>();
                    rainAmbience.Stop();
                    sceneMusic.gameAmbience.Stop();
                    sceneMusic.gameAmbience.clip = null;
                    titleActive = false;
                    dungeonTwoActive = false;
                    TempleOneActive = true;
                    dungeonOneActive = false;
                    overWorldActive = false;
                    introSceneActive = false;
                    ghostShipActive = false;
                    prototypeActive = false;
                    dwellingTimberActive = false;
                    dungeonThreeActive = false;
                    scene = 5;

                    sceneLoad = LoadNewScene();
                    StartCoroutine(sceneLoad);
                    break;
                }
            case Level.GhostShip:
                {
                    SetSceneActive(SceneActive.ghostShip);
                    rainPart = rainSystem.GetComponent<ParticleSystem>();
                    rainPart.Stop();
                    rainAmbience = rainSystem.GetComponent<AudioSource>();
                    rainAmbience.Stop();
                    sceneMusic.gameAmbience.Stop();
                    sceneMusic.gameAmbience.clip = null;
                    titleActive = false;
                    dungeonOneActive = false;
                    dungeonTwoActive = false;
                    TempleOneActive = false;
                    overWorldActive = false;
                    introSceneActive = false;
                    ghostShipActive = true;
                    prototypeActive = false;
                    dwellingTimberActive = false;
                    dungeonThreeActive = false;
                    scene = 3;

                    sceneLoad = LoadNewScene();
                    StartCoroutine(sceneLoad);
                    break;
                }
            case Level.Prototype:
                {
                    SetSceneActive(SceneActive.prototype);
                    rainPart = rainSystem.GetComponent<ParticleSystem>();
                    rainPart.Stop();
                    rainAmbience = rainSystem.GetComponent<AudioSource>();
                    rainAmbience.Stop();
                    sceneMusic.gameAmbience.Stop();
                    sceneMusic.gameAmbience.clip = null;
                    titleActive = false;
                    dungeonOneActive = false;
                    dungeonTwoActive = false;
                    TempleOneActive = false;
                    overWorldActive = false;
                    introSceneActive = false;
                    ghostShipActive = false;
                    prototypeActive = true;
                    dwellingTimberActive = false;
                    dungeonThreeActive = false;
                    scene = 7;

                    sceneLoad = LoadNewScene();
                    StartCoroutine(sceneLoad);
                    break;
                }
            case Level.DwellingTimber:
                {
                    SetSceneActive(SceneActive.dwellingTimber);
                    rainPart = rainSystem.GetComponent<ParticleSystem>();
                    rainPart.Stop();
                    rainAmbience = rainSystem.GetComponent<AudioSource>();
                    rainAmbience.Stop();
                    sceneMusic.gameAmbience.Stop();
                    sceneMusic.gameAmbience.clip = null;

                    titleActive = false;
                    dungeonOneActive = false;
                    dungeonTwoActive = false;
                    TempleOneActive = false;
                    overWorldActive = false;
                    introSceneActive = false;
                    ghostShipActive = false;
                    prototypeActive = false;
                    dwellingTimberActive = true;
                    dungeonThreeActive = false;
                    scene = 8;

                    sceneLoad = LoadNewScene();
                    StartCoroutine(sceneLoad);
                    break;
                }
              
        }
        if(scene != 0)
            loadingSystem.RandomizeSystem();
       
    }
    public void StartLevel(bool True)
    {
        isQuit = false;
        for(int fa = 0; fa < 4; fa++)
        {
            if (CoreObject.fileActive[fa])
            {
                if (CoreObject.newGame[fa])
                    isNewGame = true;
                else if (!CoreObject.newGame[fa])
                    isNewGame = false;
            }
        }

        if (True)
        {
            if (titleActive)
                LoadScene(Level.Title);
            else if (dungeonOneActive)
                LoadScene(Level.DungeonOne);
            else if (dungeonTwoActive)
                LoadScene(Level.DungeonTwo);
            else if (TempleOneActive)
                LoadScene(Level.TempleOne);
            else if (introSceneActive)
                LoadScene(Level.Intro);
            else if (overWorldActive)
                LoadScene(Level.OverWorld);
            else if (ghostShipActive)
                LoadScene(Level.GhostShip);
            else if (prototypeActive)
                LoadScene(Level.Prototype);
            else if (dwellingTimberActive)
                LoadScene(Level.DwellingTimber);
            else if (dungeonThreeActive)
                LoadScene(Level.DungeonThree);

           
            if (sceneLoad != null)
                StopCoroutine(sceneLoad);
        }
    }
    public void LoadScene(Level level)
    {
        inTransition = false;
        PlayerSystem playerSystem = playerController.GetComponent<PlayerSystem>();
        AudioListener PlayerCameraListener = GameObject.Find("Core/Player/PlayerController/Head/PlayerCamera").GetComponent<AudioListener>();
        playChar = playerController.GetComponent<CharacterSystem>();
        CoreObject core = GameObject.Find("Core").GetComponent<CoreObject>();
        playerSystem.TurnOffSpikeDamage();
        playChar.PlayerInteraction(false);
        switch (level)
        {
            case Level.Title:
                {
                    DebugSystem debugSystem = playerController.GetComponent<DebugSystem>();
                    if(DebugSystem.isDebugMode)
                        debugSystem.ShutOffDebugMode();
                    isQuit = true;
                    townPlaceName.text = null;
                    overWorldEntered = false;
                    dungeon1Entered = false;
                    dungeon2Entered = false;
                    Temple1Entered = false;
                    ghostShipEntered = false;
                    
                    dwellingTimberEntered = false;
                    dungeon3Entered = false;
                    PlayerSystem playSys = playerController.GetComponent<PlayerSystem>();
                    rainPart = rainSystem.GetComponent<ParticleSystem>();
                    rainPart.Stop();
                    rainAmbience = rainSystem.GetComponent<AudioSource>();
                    rainAmbience.Stop();
                    sceneMusic.gameAmbience.Stop();
                    sceneMusic.gameAmbience.clip = null;
                    playSys.ResetPlayer();
                    timeSys = timeSysObj.GetComponent<TimeSystem>();
                    timeSysObj.SetActive(false);
                    core.ResetActiveFiles();
                    PlayerCameraListener.enabled = false;
                    loadingGame.SetActive(false);
                    gameUI.SetActive(false);
                    gameMenu.SetActive(false);
                    playerController.SetActive(false);
                    MainMenu.SetActive(true);
                    titleMode.SetActive(true);
                    setNewRotation = new Quaternion(0, 0, 0, 0);
                    setNewPosition = Vector3.zero;
                    playerController.transform.position = setNewPosition;
                    playerController.transform.rotation = setNewRotation;
                    sceneMusic.gameMusic.clip = sceneMusic.introMusic;
                    fadeAudio = AudioFadeIn(sceneMusic.gameMusic);
                    StartCoroutine(fadeAudio);
                    SetPlayerPosition(Position.Title);
                   
                    MMTransition mainMenutrans = GameObject.Find("Core/MainMenu").GetComponent<MMTransition>();
                   
                    if (screenBlack != null)
                        StopCoroutine(screenBlack);
                    screenBlack = mainMenutrans.FadeOut(0.0f, 1f, mainMenutrans.blackScreen, true);
                    StartCoroutine(screenBlack);
            
                    mainMenutrans.StartTitleIntro();
                    titleMode.SetActive(true);
                    GameObject animObj = GameObject.Find("Core/MainMenu/Center/MainMenuCamera");
                    Animator anim = animObj.GetComponent<Animator>();
                    anim.Rebind();
                    break;
                }
            case Level.Intro:
                {
                    timeSysObj.SetActive(true);
                    timeSys = timeSysObj.GetComponent<TimeSystem>();
                    timeSys.PermaRain(true);
                    timeSys.isGameTimeRunning(true);
                    timeSys.GamePlayTimeActive(true);
                    timeSys.SetSkybox(1);
                    GameObject activeSunMoon = timeSysObj.transform.GetChild(0).gameObject;
                    activeSunMoon.SetActive(false);
                    if (CoreObject.isLoaded)
                    {
                        introSceneEntered = true;
                        overWorldEntered = false;
                        dungeon1Entered = false;
                        dungeon2Entered = false;
                        Temple1Entered = false;
                        ghostShipEntered = false;
                        dwellingTimberEntered = false;
                        dungeon3Entered = false;
                        MainMenu.SetActive(false);
                        loadingGame.SetActive(false);
                        if (screenBlack != null)
                            StopCoroutine(screenBlack);
                        screenBlack = FadeOut(0.0f, 4.0f, blackScreenGame, true);
                        StartCoroutine(screenBlack);
                        playerController.SetActive(true);
                       
                        gameUI.SetActive(true);
                        PlayerCameraListener.enabled = true;
                        playChar.SetPosition();
                        core.LoadGame();
                        core.Loading(false);
                        SetPlaceName(Place.intro, sceneMusic.introSceneMusic);

                    }
                    else
                    {
                        introSceneEntered = true;
                        overWorldEntered = false;
                        dungeon1Entered = false;
                        dungeon2Entered = false;
                        Temple1Entered = false;
                        ghostShipEntered = false;
                        
                        dwellingTimberEntered = false;
                        dungeon3Entered = false;
                        MainMenu.SetActive(false);
                        loadingGame.SetActive(false);
                        if (screenBlack != null)
                            StopCoroutine(screenBlack);
                        screenBlack = FadeOut(0.0f, 4.0f, blackScreenGame, true);
                        StartCoroutine(screenBlack);
                        playerController.SetActive(true);
                   
                        gameUI.SetActive(true);
                        PlayerCameraListener.enabled = true;
                        SetPlayerPosition(Position.Intro);
                        sceneMusic.gameAmbience.Play();
                    
                        SetPlaceName(Place.intro, sceneMusic.introSceneMusic);
                    }
                 
                    break;
                }
            case Level.OverWorld:
                {
                    timeSysObj.SetActive(true);
                    timeSys = timeSysObj.GetComponent<TimeSystem>();
                    timeSys.PermaRain(false);
                    timeSys.isGameTimeRunning(true);
                    timeSys.GamePlayTimeActive(true);
                    GameObject activeSunMoon = timeSysObj.transform.GetChild(0).gameObject;
                    activeSunMoon.SetActive(true);
                    if (CoreObject.isLoaded)
                    {
                        introSceneEntered = false;
                        overWorldEntered = true;
                        dungeon1Entered = false;
                        dungeon2Entered = false;
                        Temple1Entered = false;
                        ghostShipEntered = false;
                        
                        dwellingTimberEntered = false;
                        dungeon3Entered = false;
                        MainMenu.SetActive(false);
                        loadingGame.SetActive(false);
                        if (screenBlack != null)
                            StopCoroutine(screenBlack);
                        screenBlack = FadeOut(0.0f, 4.0f, blackScreenGame, true);
                        StartCoroutine(screenBlack);
                        playerController.SetActive(true);
             
                        gameUI.SetActive(true);
                        PlayerCameraListener.enabled = true;
                        playChar.SetPosition();
                        core.LoadGame();
                        core.Loading(false);
                        SetPlaceName(Place.overworld, sceneMusic.overWorldMusic);
                    }
                    else
                    {
                        loadingGame.SetActive(false);
                        if (screenBlack != null)
                            StopCoroutine(screenBlack);
                        screenBlack = FadeOut(0.0f, 4.0f, blackScreenGame, true);
                        StartCoroutine(screenBlack);
               
                        gameUI.SetActive(true);
                        sceneMusic.gameMusic.clip = sceneMusic.overWorldMusic;
                        fadeAudio = AudioFadeIn(sceneMusic.gameMusic);
                        StartCoroutine(fadeAudio);
                        playChar.SetPosition();

                        if (introSceneEntered)
                        {
                            SetPlayerPosition(Position.OverWorld);
                            introSceneEntered = false;
                            dungeon1Entered = false;
                            dungeon2Entered = false;
                            Temple1Entered = false;
                            overWorldEntered = true;
                            ghostShipEntered = false;
                            
                            dwellingTimberEntered = false;
                            dungeon3Entered = false;
                        }
                        else if (dungeon1Entered)
                        {
                            SetPlayerPosition(Position.Dungeon1Out);
                            introSceneEntered = false;
                            dungeon1Entered = false;
                            dungeon2Entered = false;
                            Temple1Entered = false;
                            overWorldEntered = true;
                            ghostShipEntered = false;
                            
                            dwellingTimberEntered = false;
                            dungeon3Entered = false;
                        }
                        else if (dungeon2Entered)
                        {
                            if (dungeon2AltEntrance)
                            {
                                SetPlayerPosition(Position.Dungeon2AltOut);
                                dungeon2AltEntrance = false;
                            }
                            else
                                SetPlayerPosition(Position.Dungeon2Out);
                            introSceneEntered = false;
                            dungeon1Entered = false;
                            dungeon2Entered = false;
                            Temple1Entered = false;
                            overWorldEntered = true;
                            ghostShipEntered = false;
                            
                            dwellingTimberEntered = false;
                            dungeon3Entered = false;
                        }
                        else if (dungeon3Entered)
                        {
                            if (dungeon3AltEntrance)
                            {
                                SetPlayerPosition(Position.Dungeon3AltOut);
                                dungeon3AltEntrance = false;
                            }
                            else if (dungeon3Alt2Entrance)
                            {
                                SetPlayerPosition(Position.Dungeon3Alt2Out);
                                dungeon3Alt2Entrance = false;
                            }
                            else
                                SetPlayerPosition(Position.Dungeon3Out);
                            introSceneEntered = false;
                            dungeon1Entered = false;
                            dungeon2Entered = false;
                            Temple1Entered = false;
                            overWorldEntered = true;
                            ghostShipEntered = false;
                            
                            dwellingTimberEntered = false;
                            dungeon3Entered = false;
                        }
                        else if (Temple1Entered)
                        {
                            SetPlayerPosition(Position.Temple1Out);
                            introSceneEntered = false;
                            dungeon1Entered = false;
                            dungeon2Entered = false;
                            Temple1Entered = false;
                            overWorldEntered = true;
                            ghostShipEntered = false;
                            
                            dwellingTimberEntered = false;
                            dungeon3Entered = false;
                        }
                       
                        else if (ghostShipEntered)
                        {
                            SetPlayerPosition(Position.GhostShipOut);
                            introSceneEntered = false;
                            dungeon1Entered = false;
                            dungeon2Entered = false;
                            Temple1Entered = false;
                            ghostShipEntered = false;
                            overWorldEntered = true;
                            
                            dwellingTimberEntered = false;
                            dungeon3Entered = false;
                        }
                        else if (dwellingTimberEntered)
                        {
                            SetPlayerPosition(Position.DwellingTimberOut);
                            introSceneEntered = false;
                            dungeon1Entered = false;
                            dungeon2Entered = false;
                            Temple1Entered = false;
                            ghostShipEntered = false;
                            overWorldEntered = true;
                            
                            dwellingTimberEntered = false;
                            dungeon3Entered = false;
                        }
                    }
                    SetPlaceName(Place.overworld, sceneMusic.overWorldMusic);
                    if (CharacterSystem.isClimbing)
                    {
                        CharacterSystem characterSystem = playerController.GetComponent<CharacterSystem>();
                        characterSystem.setClimbing(false);
                    }
                    if (UnderWaterSystem.isSwimming)
                    {
                        UnderWaterSystem underWaterSystem = GameObject.FindGameObjectWithTag("Head").GetComponent<UnderWaterSystem>();
                        underWaterSystem.ResetAir();
                    }
                    break;
                }
            case Level.DungeonOne:
                {
                    timeSysObj.SetActive(true);
                    timeSys = timeSysObj.GetComponent<TimeSystem>();
                    timeSys.isGameTimeRunning(true);
                    timeSys.GamePlayTimeActive(true);
                    GameObject activeSunMoon = timeSysObj.transform.GetChild(0).gameObject;
                    activeSunMoon.SetActive(false);
                    PlayerSystem playSys = playerController.GetComponent<PlayerSystem>();
                    rainPart = rainSystem.GetComponent<ParticleSystem>();
                    rainPart.Stop();
                    rainAmbience = rainSystem.GetComponent<AudioSource>();
                    rainAmbience.Stop();
                    sceneMusic.gameAmbience.Stop();
                    sceneMusic.gameAmbience.clip = null;
                    if (CoreObject.isLoaded)
                    {
                        overWorldEntered = false;
                        introSceneEntered = false;
                        ghostShipEntered = false;
                        dungeon1Entered = true;
                        dungeon2Entered = false;
                        Temple1Entered = false;
                        
                        dwellingTimberEntered = false;
                        dungeon3Entered = false;
                        MainMenu.SetActive(false);
                        loadingGame.SetActive(false);
                        if (screenBlack != null)
                            StopCoroutine(screenBlack);
                        screenBlack = FadeOut(0.0f, 4.0f, blackScreenGame, true);
                        StartCoroutine(screenBlack);
                        playerController.SetActive(true);
               
                        gameUI.SetActive(true);
                        PlayerCameraListener.enabled = true;
                        playChar.SetPosition();
                        core.LoadGame();
                        core.Loading(false);
                    }
                    else
                    {
                        introSceneEntered = false;
                        overWorldEntered = false;
                        ghostShipEntered = false;
                        dungeon1Entered = true;
                        dungeon2Entered = false;
                        Temple1Entered = false;
                        
                        dwellingTimberEntered = false;
                        dungeon3Entered = false;
                        loadingGame.SetActive(false);
                        if (screenBlack != null)
                            StopCoroutine(screenBlack);
                        screenBlack = FadeOut(0.0f, 4.0f, blackScreenGame, true);
                        StartCoroutine(screenBlack);
                
                        gameUI.SetActive(true);
                        playChar.SetPosition();
                        SetPlayerPosition(Position.Dungeon1In);
                    }
                    SetPlaceName(Place.dungeon1, sceneMusic.dungeonOneMusic);
                    break;
                }
            case Level.DungeonTwo:
                {
                    timeSysObj.SetActive(true);
                    timeSys = timeSysObj.GetComponent<TimeSystem>();
                    timeSys.isGameTimeRunning(true);
                    timeSys.GamePlayTimeActive(true);
                    GameObject activeSunMoon = timeSysObj.transform.GetChild(0).gameObject;
                    activeSunMoon.SetActive(false);
                    PlayerSystem playSys = playerController.GetComponent<PlayerSystem>();
                    rainPart = rainSystem.GetComponent<ParticleSystem>();
                    rainPart.Stop();
                    rainAmbience = rainSystem.GetComponent<AudioSource>();
                    rainAmbience.Stop();
                    sceneMusic.gameAmbience.Stop();
                    sceneMusic.gameAmbience.clip = null;
                    if (CoreObject.isLoaded)
                    {
                        introSceneEntered = false;
                        overWorldEntered = false;
                        ghostShipEntered = false;
                        dungeon1Entered = false;
                        dungeon2Entered = true;
                        Temple1Entered = false;
                        
                        dwellingTimberEntered = false;
                        dungeon3Entered = false;
                        MainMenu.SetActive(false);
                        loadingGame.SetActive(false);
                        if (screenBlack != null)
                            StopCoroutine(screenBlack);
                        screenBlack = FadeOut(0.0f, 4.0f, blackScreenGame, true);
                        StartCoroutine(screenBlack);
                        playerController.SetActive(true);
              
                        gameUI.SetActive(true);
                       
                        PlayerCameraListener.enabled = true;
                        playChar.SetPosition();
                        core.LoadGame();
                        core.Loading(false);
                    }
                    else
                    {
                        introSceneEntered = false;
                        overWorldEntered = false;
                        ghostShipEntered = false;
                        dungeon1Entered = false;
                        Temple1Entered = false;
                        
                        dwellingTimberEntered = false;
                        dungeon3Entered = false;
                        loadingGame.SetActive(false);
                        if (screenBlack != null)
                            StopCoroutine(screenBlack);
                        screenBlack = FadeOut(0.0f, 4.0f, blackScreenGame, true);
                        StartCoroutine(screenBlack);
              
                        gameUI.SetActive(true);
                        playChar.SetPosition();
                        if (dungeon2AltEntrance)
                        {
                            SetPlayerPosition(Position.Dungeon2AltIn);
                            dungeon2AltEntrance = false;
                        }
                        else
                            SetPlayerPosition(Position.Dungeon2In);
                        dungeon2Entered = true;
                    }
                    SetPlaceName(Place.dungeon2, sceneMusic.dungeonTwoMusic);
                    break;
                }
            case Level.DungeonThree:
                {
                    timeSysObj.SetActive(true);
                    timeSys = timeSysObj.GetComponent<TimeSystem>();
                    timeSys.isGameTimeRunning(true);
                    timeSys.GamePlayTimeActive(true);
                    GameObject activeSunMoon = timeSysObj.transform.GetChild(0).gameObject;
                    activeSunMoon.SetActive(false);
                    PlayerSystem playSys = playerController.GetComponent<PlayerSystem>();
                    rainPart = rainSystem.GetComponent<ParticleSystem>();
                    rainPart.Stop();
                    rainAmbience = rainSystem.GetComponent<AudioSource>();
                    rainAmbience.Stop();
                    sceneMusic.gameAmbience.Stop();
                    sceneMusic.gameAmbience.clip = null;
                    if (CoreObject.isLoaded)
                    {
                        introSceneEntered = false;
                        overWorldEntered = false;
                        ghostShipEntered = false;
                        dungeon1Entered = false;
                        dungeon2Entered = false;
                        Temple1Entered = false;
                        
                        dwellingTimberEntered = false;
                        dungeon3Entered = true;
                        MainMenu.SetActive(false);
                        loadingGame.SetActive(false);
                        if (screenBlack != null)
                            StopCoroutine(screenBlack);
                        screenBlack = FadeOut(0.0f, 4.0f, blackScreenGame, true);
                        StartCoroutine(screenBlack);
                        playerController.SetActive(true);
              
                        gameUI.SetActive(true);

                        PlayerCameraListener.enabled = true;
                        playChar.SetPosition();
                        core.LoadGame();
                        core.Loading(false);
                    }
                    else
                    {
                        introSceneEntered = false;
                        overWorldEntered = false;
                        ghostShipEntered = false;
                        dungeon1Entered = false;
                        dungeon2Entered = false;
                        Temple1Entered = false;
                        
                        dwellingTimberEntered = false;
                        loadingGame.SetActive(false);
                        if (screenBlack != null)
                            StopCoroutine(screenBlack);
                        screenBlack = FadeOut(0.0f, 4.0f, blackScreenGame, true);
                        StartCoroutine(screenBlack);
                   
                        gameUI.SetActive(true);
                        playChar.SetPosition();
                        if (dungeon3AltEntrance)
                        {
                            SetPlayerPosition(Position.Dungeon3AltIn);
                            dungeon3AltEntrance = false;
                        }
                        if (dungeon3Alt2Entrance)
                        {
                            SetPlayerPosition(Position.Dungeon3Alt2In);
                            dungeon3AltEntrance = false;
                        }
                        else
                            SetPlayerPosition(Position.Dungeon3In);
                        dungeon3Entered = true;
                    }
                    SetPlaceName(Place.dungeon3, sceneMusic.dungeonThreeMusic);
                    break;
                }
            case Level.TempleOne:
                {
                    timeSysObj.SetActive(true);
                    timeSys = timeSysObj.GetComponent<TimeSystem>();
                    timeSys.isGameTimeRunning(true);
                    timeSys.GamePlayTimeActive(true);
                    GameObject activeSunMoon = timeSysObj.transform.GetChild(0).gameObject;
                    activeSunMoon.SetActive(false);
                    PlayerSystem playSys = playerController.GetComponent<PlayerSystem>();
                    rainPart = rainSystem.GetComponent<ParticleSystem>();
                    rainPart.Stop();
                    rainAmbience = rainSystem.GetComponent<AudioSource>();
                    rainAmbience.Stop();
                    sceneMusic.gameAmbience.Stop();
                    sceneMusic.gameAmbience.clip = null;
                    if (CoreObject.isLoaded)
                    {
                        introSceneEntered = false;
                        overWorldEntered = false;
                        ghostShipEntered = false;
                        dungeon1Entered = false;
                        dungeon2Entered = false;
                        
                        dwellingTimberEntered = false;
                        dungeon3Entered = false;
                        MainMenu.SetActive(false);
                        loadingGame.SetActive(false);
                        if (screenBlack != null)
                            StopCoroutine(screenBlack);
                        screenBlack = FadeOut(0.0f, 4.0f, blackScreenGame, true);
                        StartCoroutine(screenBlack);
                        playerController.SetActive(true);
                 
                        gameUI.SetActive(true);
                        Temple1Entered = true;
                        PlayerCameraListener.enabled = true;
                        playChar.SetPosition();
                        core.LoadGame();
                        core.Loading(false);
                    }
                    else
                    {
                        introSceneEntered = false;
                        overWorldEntered = false;
                        ghostShipEntered = false;
                        dungeon1Entered = false;
                        dungeon2Entered = false;
                        
                        dwellingTimberEntered = false;
                        dungeon3Entered = false;
                        loadingGame.SetActive(false);
                        if (screenBlack != null)
                            StopCoroutine(screenBlack);
                        screenBlack = FadeOut(0.0f, 4.0f, blackScreenGame, true);
                        StartCoroutine(screenBlack);
                   
                        gameUI.SetActive(true);
                        playChar.SetPosition();
                        SetPlayerPosition(Position.Temple1In);
                        Temple1Entered = true;
                    }
                    SetPlaceName(Place.Temple1, sceneMusic.templeOneMusic);
                    break;
                }
            case Level.GhostShip:
                {
                    timeSysObj.SetActive(true);
                    timeSys = timeSysObj.GetComponent<TimeSystem>();
                    timeSys.isGameTimeRunning(true);
                    timeSys.GamePlayTimeActive(true);
                    timeSys.SetSkybox(1);
                    GameObject activeSunMoon = timeSysObj.transform.GetChild(0).gameObject;
                    activeSunMoon.SetActive(false);
                    PlayerSystem playSys = playerController.GetComponent<PlayerSystem>();
                    rainPart = rainSystem.GetComponent<ParticleSystem>();
                    rainPart.Stop();
                    rainAmbience = rainSystem.GetComponent<AudioSource>();
                    rainAmbience.Stop();
                    sceneMusic.gameAmbience.Stop();
                    sceneMusic.gameAmbience.clip = null;
                    if (CoreObject.isLoaded)
                    {
                        introSceneEntered = false;
                        overWorldEntered = false;
                        dungeon1Entered = false;
                        dungeon2Entered = false;
                        Temple1Entered = false;
                        
                        dwellingTimberEntered = false;
                        dungeon3Entered = false;
                        MainMenu.SetActive(false);
                        loadingGame.SetActive(false);
                        if (screenBlack != null)
                            StopCoroutine(screenBlack);
                        screenBlack = FadeOut(0.0f, 4.0f, blackScreenGame, true);
                        StartCoroutine(screenBlack);
                        playerController.SetActive(true);
         
                        gameUI.SetActive(true);
                        ghostShipEntered = true;
                        PlayerCameraListener.enabled = true;
                        playChar.SetPosition();
                        core.LoadGame();
                        core.Loading(false);
                    }
                    else
                    {
                        introSceneEntered = false;
                        overWorldEntered = false;
                        dungeon1Entered = false;
                        dungeon2Entered = false;
                        Temple1Entered = false;
                        
                        dwellingTimberEntered = false;
                        dungeon3Entered = false;
                        loadingGame.SetActive(false);
                        if (screenBlack != null)
                            StopCoroutine(screenBlack);
                        screenBlack = FadeOut(0.0f, 4.0f, blackScreenGame, true);
                        StartCoroutine(screenBlack);
             
                        gameUI.SetActive(true);
                        playChar.SetPosition();
                        SetPlayerPosition(Position.GhostShipIn);
                        ghostShipEntered = true;
                    }
                    SetPlaceName(Place.ghostShip, sceneMusic.ghostShipMusic);
                    break;
                }
            case Level.Prototype:
                {
                    timeSysObj.SetActive(true);
                    timeSys = timeSysObj.GetComponent<TimeSystem>();
                    timeSys.PermaRain(true);
                    timeSys.isGameTimeRunning(true);
                    timeSys.GamePlayTimeActive(true);
                    timeSys.SetSkybox(1);
                    GameObject activeSunMoon = timeSysObj.transform.GetChild(0).gameObject;
                    activeSunMoon.SetActive(false);
                    PlayerSystem playSys = playerController.GetComponent<PlayerSystem>();
                    rainPart = rainSystem.GetComponent<ParticleSystem>();
                    rainPart.Stop();
                    rainAmbience = rainSystem.GetComponent<AudioSource>();
                    rainAmbience.Stop();
                    sceneMusic.gameAmbience.Stop();
                    sceneMusic.gameAmbience.clip = null;
                    if (CoreObject.isLoaded)
                    {
                        introSceneEntered = false;
                        overWorldEntered = false;
                        dungeon1Entered = false;
                        dungeon2Entered = false;
                        Temple1Entered = false;
                        ghostShipEntered = false;
                        dwellingTimberEntered = false;
                        dungeon3Entered = false;
                        MainMenu.SetActive(false);
                        loadingGame.SetActive(false);
                        if (screenBlack != null)
                            StopCoroutine(screenBlack);
                        screenBlack = FadeOut(0.0f, 4.0f, blackScreenGame, true);
                        StartCoroutine(screenBlack);
                        playerController.SetActive(true);
               
                        gameUI.SetActive(true);
                        //protoTypeEntered = true;
                        PlayerCameraListener.enabled = true;
                        playChar.SetPosition();
                        core.LoadGame();
                        core.Loading(false);
                    }
                    else
                    {
                        introSceneEntered = false;
                        overWorldEntered = false;
                        dungeon1Entered = false;
                        dungeon2Entered = false;
                        Temple1Entered = false;
                        ghostShipEntered = false;
                        dwellingTimberEntered = false;
                        dungeon3Entered = false;
                        loadingGame.SetActive(false);
                        if (screenBlack != null)
                            StopCoroutine(screenBlack);
                        screenBlack = FadeOut(0.0f, 4.0f, blackScreenGame, true);
                        StartCoroutine(screenBlack);
               
                        gameUI.SetActive(true);
                        playChar.SetPosition();
                        SetPlayerPosition(Position.Prototype);
                        //protoTypeEntered = true;
                    }
                    SetPlaceName(Place.prototype, sceneMusic.debugMusic);
                    break;
                }
            case Level.DwellingTimber:
                {
                    timeSysObj.SetActive(true);
                    timeSys = timeSysObj.GetComponent<TimeSystem>();
                    timeSys.isGameTimeRunning(true);
                    timeSys.GamePlayTimeActive(true);
                    timeSys.SetSkybox(1);
                    GameObject activeSunMoon = timeSysObj.transform.GetChild(0).gameObject;
                    activeSunMoon.SetActive(false);
                    PlayerSystem playSys = playerController.GetComponent<PlayerSystem>();
                    rainPart = rainSystem.GetComponent<ParticleSystem>();
                    rainPart.Stop();
                    rainAmbience = rainSystem.GetComponent<AudioSource>();
                    rainAmbience.Stop();
                    sceneMusic.gameAmbience.Stop();
                    sceneMusic.gameAmbience.clip = null;
                    if (CoreObject.isLoaded)
                    {
                        introSceneEntered = false;
                        overWorldEntered = false;
                        dungeon1Entered = false;
                        dungeon2Entered = false;
                        Temple1Entered = false;
                        ghostShipEntered = false;
                        
                        dungeon3Entered = false;

                        MainMenu.SetActive(false);
                        loadingGame.SetActive(false);
                        if (screenBlack != null)
                            StopCoroutine(screenBlack);
                        screenBlack = FadeOut(0.0f, 4.0f, blackScreenGame, true);
                        StartCoroutine(screenBlack);
                        playerController.SetActive(true);
               
                        gameUI.SetActive(true);
                        dwellingTimberEntered = true;
                        PlayerCameraListener.enabled = true;
                        playChar.SetPosition();
                        core.LoadGame();
                        core.Loading(false);
                    }
                    else
                    {
                        introSceneEntered = false;
                        overWorldEntered = false;
                        dungeon1Entered = false;
                        dungeon2Entered = false;
                        Temple1Entered = false;
                        ghostShipEntered = false;
                        
                        dungeon3Entered = false;
                        loadingGame.SetActive(false);
                        if (screenBlack != null)
                            StopCoroutine(screenBlack);
                        screenBlack = FadeOut(0.0f, 4.0f, blackScreenGame, true);
                        StartCoroutine(screenBlack);
           
                        gameUI.SetActive(true);
                        playChar.SetPosition();
                        SetPlayerPosition(Position.DwellingTimberIn);
                        dwellingTimberEntered = true;
                    }
                    SetPlaceName(Place.dwellingTimber, sceneMusic.dwellingMusic);
                    break;
                }
        }
      
    }
    public void SetAltEntrance(Position Pos)
    {
        switch (Pos)
        {
            case Position.Dungeon2AltIn:
                {
                    dungeon2AltEntrance = true;
                    break;
                }
            case Position.Dungeon2AltOut:
                {
                    dungeon2AltEntrance = true;
                    break;
                }
            case Position.Dungeon3AltIn:
                {
                    dungeon3AltEntrance = true;
                    break;
                }
            case Position.Dungeon3AltOut:
                {
                    dungeon3AltEntrance = true;
                    break;
                }
            case Position.Dungeon3Alt2In:
                {
                    dungeon3Alt2Entrance = true;
                    break;
                }
            case Position.Dungeon3Alt2Out:
                {
                    dungeon3Alt2Entrance = true;
                    break;
                }
        }
    }
    public IEnumerator FadeIn(float aValue, float aTime, Image image, bool imgProperty)
    {
        float alpha = image.GetComponent<Image>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.fixedDeltaTime / aTime)
        {
            Color newColor =  new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(alpha, aValue, t));
            image.GetComponent<Image>().color = newColor;
            if (t >= 0.9)
            {
                if (imgProperty)
                    newColor =  new Color(0, 0, 0, 1);
                else
                    newColor =  new Color(1, 1, 1, 1);
                image.GetComponent<Image>().color = newColor;
                if (isLoad)
                {
                    if (sceneLoad != null)
                    {
                        StopCoroutine(sceneLoad);
                    }
                    sceneLoad = LoadNewScene();
                    StartCoroutine(sceneLoad);
                    loadingSystem.RandomizeSystem();
                    isLoad = false;
                }
            }
            else
                image.GetComponent<Image>().enabled = true;

            yield return null;
        }

    }
    public IEnumerator FadeOut(float aValue, float aTime, Image image, bool imgProperty)
    {
        float alpha = image.GetComponent<Image>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.fixedDeltaTime / aTime)
        {
            Color newColor =  new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(alpha, aValue, t));
            image.GetComponent<Image>().color = newColor;
            if (t >= 0.9)
            {
                if(!DialogueSystem.isDialogueActive)
                    EnablePlayer(true);
                if (imgProperty)
                    newColor =  new Color(0, 0, 0, 0);
                else
                    newColor =  new Color(1, 1, 1, 0);
                image.GetComponent<Image>().color = newColor;
            }
            else
            {
                image.GetComponent<Image>().enabled = true;
            }
            yield return null;
        }

    }
    IEnumerator AudioFadeOut(AudioSource audio)
    {
        audio.volume = 1.0f;
        float MinVol = 0;
        for (float f = 1f; f > MinVol; f -= 0.05f)
        {
            audio.volume = f;
            yield return new WaitForSecondsRealtime(.1f);
            if (f <= 0.1)
            {
                audio.Stop();
                audio.volume = MinVol;
            }
        }
    }
    IEnumerator AudioFadeIn(AudioSource audio)
    {
        audio.volume = 0.0f;
        float MaxVol = 1;
        audio.Play();
        for (float f = 0f; f < MaxVol; f += 0.05f)
        {
            audio.volume = f;
            yield return new WaitForSecondsRealtime(.1f);
            if (f >= 0.9)
            {
                audio.volume = MaxVol;
            }
        }
    }
    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
    public void EnablePlayer(bool active)
    {
        isDisabled = !active;
    }
    public void LoadSceneSystem(int sceneNum)
    {
        scene = sceneNum;
        if (scene == 0)
            StartScene(Level.Title);
        else if (scene == 1)
            StartScene(Level.DungeonOne);
        else if (scene == 4)
            StartScene(Level.DungeonTwo);
        else if (scene == 5)
            StartScene(Level.TempleOne);
        else if (scene == 6)
            StartScene(Level.Intro);
        else if (scene == 2)
            StartScene(Level.OverWorld);
        else if (scene == 3)
            StartScene(Level.GhostShip);
        else if(scene == 7)
            StartScene(Level.Prototype);
        else if (scene == 8)
            StartScene(Level.DwellingTimber);
        else if (scene == 9)
            StartScene(Level.DungeonThree);

    }
    public enum Position { Title, Intro, OverWorld, Dungeon1In, Dungeon1Out, Dungeon2In, Dungeon2Out, Temple1In, Temple1Out,
                           Dungeon2AltIn, Dungeon2AltOut, GhostShipIn, GhostShipOut, Dungeon3In, Dungeon3Out, Dungeon3AltIn,
                           Dungeon3AltOut, Dungeon3Alt2In, Dungeon3Alt2Out, DwellingTimberIn, DwellingTimberOut, Prototype }
    public void SetPlayerPosition(Position pos)
    {
        switch (pos)
        {
            case Position.Title:
                {
                    setNewRotation = new Quaternion(0, 0, 0, 0);
                    setNewPosition = Vector3.zero;
                    break;
                }
            case Position.Intro:
                {
                    setNewRotation = scenePosition.introScenePosition.rotation;
                    setNewPosition = scenePosition.introScenePosition.position;
                    break;
                }
            case Position.OverWorld:
                {
                    setNewRotation = scenePosition.overWorldPosition.rotation;
                    setNewPosition = scenePosition.overWorldPosition.position;
                    break;
                }
            case Position.Dungeon1In:
                {
                    setNewRotation = scenePosition.dungeon1InPosition.rotation;
                    setNewPosition = scenePosition.dungeon1InPosition.position;
                    break;
                }
            case Position.Dungeon1Out:
                {
                    setNewRotation = scenePosition.dungeon1OutPosition.rotation;
                    setNewPosition = scenePosition.dungeon1OutPosition.position;
                    break;
                }
            case Position.Dungeon2In:
                {
                    setNewRotation = scenePosition.dungeon2InPosition.rotation;
                    setNewPosition = scenePosition.dungeon2InPosition.position;
                    break;
                }
            case Position.Dungeon2Out:
                {
                    setNewRotation = scenePosition.dungeon2OutPosition.rotation;
                    setNewPosition = scenePosition.dungeon2OutPosition.position;
                    break;
                }
            case Position.Temple1In:
                {
                    setNewRotation = scenePosition.temple1InPosition.rotation;
                    setNewPosition = scenePosition.temple1InPosition.position;
                    break;
                }
            case Position.Temple1Out:
                {
                    setNewRotation = scenePosition.temple1OutPosition.rotation;
                    setNewPosition = scenePosition.temple1OutPosition.position;
                    break;
                }
            case Position.Dungeon2AltIn:
                {
                    setNewRotation = scenePosition.dungeon2AltInPosition.rotation;
                    setNewPosition = scenePosition.dungeon2AltInPosition.position;
                    break;
                }
            case Position.Dungeon2AltOut:
                {
                    setNewRotation = scenePosition.dungeon2AltOutPosition.rotation;
                    setNewPosition = scenePosition.dungeon2AltOutPosition.position;
                    break;
                }
            case Position.GhostShipIn:
                {
                    setNewRotation = scenePosition.ghostShipInPosition.rotation;
                    setNewPosition = scenePosition.ghostShipInPosition.position;
                    break;
                }
            case Position.GhostShipOut:
                {
                    setNewRotation = scenePosition.ghostShipOutPosition.rotation;
                    setNewPosition = scenePosition.ghostShipOutPosition.position;
                    break;
                }
            case Position.Prototype:
                {
                    setNewRotation = scenePosition.prototypePosition.rotation;
                    setNewPosition = scenePosition.prototypePosition.position;
                    break;
                }
            case Position.DwellingTimberIn:
                {
                    setNewRotation = scenePosition.dwellingTimberInPosition.rotation;
                    setNewPosition = scenePosition.dwellingTimberInPosition.position;
                    break;
                }
            case Position.DwellingTimberOut:
                {
                    setNewRotation = scenePosition.dwellingTimberOutPosition.rotation;
                    setNewPosition = scenePosition.dwellingTimberOutPosition.position;
                    break;
                }
            case Position.Dungeon3In:
                {
                    setNewRotation = scenePosition.dungeon3InPosition.rotation;
                    setNewPosition = scenePosition.dungeon3InPosition.position;
                    break;
                }
            case Position.Dungeon3Out:
                {
                    setNewRotation = scenePosition.dungeon3OutPosition.rotation;
                    setNewPosition = scenePosition.dungeon3OutPosition.position;
                    break;
                }
            case Position.Dungeon3AltIn:
                {
                    setNewRotation = scenePosition.dungeon3AltInPosition.rotation;
                    setNewPosition = scenePosition.dungeon3AltInPosition.position;
                    break;
                }
            case Position.Dungeon3AltOut:
                {
                    setNewRotation = scenePosition.dungeon3AltOutPosition.rotation;
                    setNewPosition = scenePosition.dungeon3AltOutPosition.position;
                    break;
                }
            case Position.Dungeon3Alt2In:
                {
                    setNewRotation = scenePosition.dungeon3Alt2InPosition.rotation;
                    setNewPosition = scenePosition.dungeon3Alt2InPosition.position;
                    break;
                }
            case Position.Dungeon3Alt2Out:
                {
                    setNewRotation = scenePosition.dungeon3Alt2OutPosition.rotation;
                    setNewPosition = scenePosition.dungeon3Alt2OutPosition.position;
                    break;
                }
        }

        playerController.transform.position = setNewPosition;
        playerController.transform.rotation = setNewRotation;
    }
    public enum Place { debug, prototype, intro, overworld, dungeon1, dungeon2, Temple1, ghostShip, duskCliff, duskCliffShop, windAcre, windacreShop, windAcreDock, fishersTrawl, dwellingTimber, dungeon3, abandonedBog, skulkCove, skulkCoveShop, shadowedBog }
    public void SetPlaceName(Place name, AudioClip clip)
    {
        switch (name)
        {
            case Place.debug:
                {
                    townPlaceName.text = "Debug Mode";
                    break;
                }
            case Place.prototype:
                {
                    townPlaceName.text = "NIGELPHOENIX's Army";
                    if (fadeAudio != null)
                        StopCoroutine(fadeAudio);
                    sceneMusic.gameMusic.clip = clip;
                    fadeAudio = AudioFadeIn(sceneMusic.gameMusic);
                    StartCoroutine(fadeAudio);
                    break;
                }
            case Place.overworld:
                {
                    townPlaceName.text = "Conquest Island";
                    if (fadeAudio != null)
                        StopCoroutine(fadeAudio);
                    if (sceneMusic.gameMusic.clip != sceneMusic.overWorldMusic)
                    {
                        sceneMusic.gameMusic.clip = clip;
                        fadeAudio = AudioFadeIn(sceneMusic.gameMusic);
                        StartCoroutine(fadeAudio);
                    }
                    else if(sceneMusic.gameMusic.clip == sceneMusic.overWorldMusic && sceneMusic.gameMusic.volume < 1)
                    {
                        fadeAudio = AudioFadeIn(sceneMusic.gameMusic);
                        StartCoroutine(fadeAudio);
                    }
                    break;
                }
            case Place.intro:
                {
                    townPlaceName.text = "Rafter's Bargain";
                    if (fadeAudio != null)
                        StopCoroutine(fadeAudio);
                    sceneMusic.gameMusic.clip = clip;
                    fadeAudio = AudioFadeIn(sceneMusic.gameMusic);
                    StartCoroutine(fadeAudio);
                    break;
                }
            case Place.dungeon1:
                {
                    townPlaceName.text = "Riverfall Shrine";
                    if (fadeAudio != null)
                        StopCoroutine(fadeAudio);
                    sceneMusic.gameMusic.clip = clip;
                    fadeAudio = AudioFadeIn(sceneMusic.gameMusic);
                    StartCoroutine(fadeAudio);
                    break;
                }
            case Place.dungeon2:
                {
                    townPlaceName.text = "Abandoned Ruins";
                    if (fadeAudio != null)
                        StopCoroutine(fadeAudio);
                    sceneMusic.gameMusic.clip = clip;
                    fadeAudio = AudioFadeIn(sceneMusic.gameMusic);
                    StartCoroutine(fadeAudio);
                    break;
                }
            case Place.Temple1:
                {
                    townPlaceName.text = "Temple of Reign";
                    if (fadeAudio != null)
                        StopCoroutine(fadeAudio);
                    sceneMusic.gameMusic.clip = clip;
                    fadeAudio = AudioFadeIn(sceneMusic.gameMusic);
                    StartCoroutine(fadeAudio);
                    break;
                }
            case Place.ghostShip:
                {
                    townPlaceName.text = "The Cursed Grail";
                    if (fadeAudio != null)
                        StopCoroutine(fadeAudio);
                    sceneMusic.gameMusic.clip = clip;
                    fadeAudio = AudioFadeIn(sceneMusic.gameMusic);
                    StartCoroutine(fadeAudio);
                    break;
                }
            case Place.duskCliff:
                {
                    townPlaceName.text = "Duskcliff";
                    if (fadeAudio != null)
                        StopCoroutine(fadeAudio);
                    sceneMusic.gameMusic.clip = clip;
                    fadeAudio = AudioFadeIn(sceneMusic.gameMusic);
                    StartCoroutine(fadeAudio);
                    break;
                }
            case Place.windAcre:
                {
                    townPlaceName.text = "Windacre";
                    if (fadeAudio != null)
                        StopCoroutine(fadeAudio);
                    sceneMusic.gameMusic.clip = clip;
                    fadeAudio = AudioFadeIn(sceneMusic.gameMusic);
                    StartCoroutine(fadeAudio);
                    break;
                }
            case Place.skulkCove:
                {
                    townPlaceName.text = "Skulk Cove";
                    if (fadeAudio != null)
                        StopCoroutine(fadeAudio);
                    sceneMusic.gameMusic.clip = clip;
                    fadeAudio = AudioFadeIn(sceneMusic.gameMusic);
                    StartCoroutine(fadeAudio);
                    break;
                }
            case Place.windAcreDock:
                {
                    townPlaceName.text = "Windacre Dock";
                    break;
                }
            case Place.windacreShop:
                {
                    townPlaceName.text = "Windacre Shop";
                    break;
                }
            case Place.duskCliffShop:
                {
                    townPlaceName.text = "Duskcliff Shop";
                    break;
                }
            case Place.skulkCoveShop:
                {
                    townPlaceName.text = "Skulkcove Shop";
                    break;
                }
            case Place.abandonedBog:
                {
                    townPlaceName.text = "Abandoned Bog";
                    break;
                }
            case Place.shadowedBog:
                {
                    townPlaceName.text = "Shadowed Bog";
                    break;
                }
            case Place.fishersTrawl:
                {
                    townPlaceName.text = "Fishers Trawl";
                    break;
                }
            case Place.dwellingTimber:
                {
                    townPlaceName.text = "Dwelling Timber";
                    if (fadeAudio != null)
                        StopCoroutine(fadeAudio);
                    sceneMusic.gameMusic.clip = clip;
                    fadeAudio = AudioFadeIn(sceneMusic.gameMusic);
                    StartCoroutine(fadeAudio);
                    break;
                }
            case Place.dungeon3:
                {
                    townPlaceName.text = "Well Pathway";
                    if (fadeAudio != null)
                        StopCoroutine(fadeAudio);
                    sceneMusic.gameMusic.clip = clip;
                    fadeAudio = AudioFadeIn(sceneMusic.gameMusic);
                    StartCoroutine(fadeAudio);
                    break;
                }
        }
        if (townPlaceName.text != "Conquest Island")
        {
            if (townPlaceRoutine != null)
                StopCoroutine(townPlaceRoutine);
            townPlaceRoutine = FadeTextOut(3, townPlaceName);
            StartCoroutine(townPlaceRoutine);
            alreadyNamed = false;
        }
        else if(townPlaceName.text == "Conquest Island" && !alreadyNamed)
        {
            if (townPlaceRoutine != null)
                StopCoroutine(townPlaceRoutine);
            townPlaceRoutine = FadeTextOut(3, townPlaceName);
            StartCoroutine(townPlaceRoutine);
            alreadyNamed = true;
        }
    }
    public void QuitTheGame()
    {
        CharacterSystem characterSystem = playerController.GetComponent<CharacterSystem>();
        characterSystem.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
        PauseGame pause = playerController.GetComponent<PauseGame>();
        if(PauseGame.isPaused)
            pause.PauseTheGame();
        isDisabled = true;
        Debug.Log(isDisabled);
        GameObject enemyUIObjs = GameObject.Find("Core/Player/PopUpCanvas/EnemyUIStorage");
        foreach (Transform child in enemyUIObjs.transform)
            Destroy(child.gameObject);
        GameObject playerUIObjs = GameObject.Find("Core/Player/PopUpCanvas/PlayerUIStorage");
        foreach (Transform child in playerUIObjs.transform)
            Destroy(child.gameObject);
        AudioSource musicAudioSrc = GameObject.Find("Core/GameMusic&Sound/GameMusic").GetComponent<AudioSource>();
        if (fadeAudio != null)
            StopCoroutine(fadeAudio);
        fadeAudio = AudioFadeOut(musicAudioSrc);
        StartCoroutine(fadeAudio);
        CharacterSystem player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterSystem>();
        player.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
        StartScene(Level.Title);

    }
}
[Serializable]
public struct ScenePositions
{
    public Transform Player;
    public Transform introScenePosition;
    public Transform dungeon1InPosition;
    public Transform dungeon1OutPosition;
    public Transform dungeon2InPosition;
    public Transform dungeon2OutPosition;
    public Transform temple1InPosition;
    public Transform temple1OutPosition;
    public Transform dungeon2AltInPosition;
    public Transform dungeon2AltOutPosition;
    public Transform overWorldPosition;
    public Transform ghostShipInPosition;
    public Transform ghostShipOutPosition;
    public Transform dungeon3InPosition;
    public Transform dungeon3OutPosition;
    public Transform dungeon3AltInPosition;
    public Transform dungeon3AltOutPosition;
    public Transform dungeon3Alt2InPosition;
    public Transform dungeon3Alt2OutPosition;
    public Transform dwellingTimberInPosition;
    public Transform dwellingTimberOutPosition;
    public Transform prototypePosition;
}
[Serializable]
public class SceneMusic
{
    public AudioSource gameMusic;
    public AudioSource gameAmbience;
    public AudioClip introMusic;
    public AudioClip introSceneMusic;
    public AudioClip overWorldMusic;
    public AudioClip townOneMusic;
    public AudioClip townTwoMusic;
    public AudioClip dungeonOneMusic;
    public AudioClip dungeonTwoMusic;
    public AudioClip dungeonThreeMusic;
    public AudioClip templeOneMusic;
    public AudioClip ghostShipMusic;
    public AudioClip dwellingMusic;
    public AudioClip debugMusic;
}
