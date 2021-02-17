using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueSystem : MonoBehaviour
{
    public AudioSource musicAudioSrc;
    public AudioClip gameOverMusic;
    public AudioClip GhostEncounterSfx;
    private Selectable m_Selectable;
    public Text dialogueText;
    public Text deadDialogueText;
    public DialogueMessages messages;
    public float letterTime;
    public GameObject DialogueBanner;
    public GameObject DeadOptions;
    public GameObject ConfirmedOptions;
    public Button ConfirmYes;
    public Button ConfirmNo;
    public Button button1;
    public GameObject button1I;
    public Button buttonMessage;
    public GameObject buttonPopUI;
    AudioSource audioSrc;
    public AudioClip scrollSound;
    public Button button2;
    public GameObject button2I;
    GameObject personObj;
    IEnumerator fadeAudio;
    IEnumerator routine = null;
    IEnumerator itemPreviewMsgRoutine = null;
    string storeMessage;
    float deathTime;
    public static bool isDialogueActive;
    [Header("Item Preview")]
    public GameObject itemPreviewWindow;
    public Sprite[] itemPreviewIcons;
    public Image previewIcon;
    public Text itemDiscriptionMsg;
    public Text itemTitle;
    string itemMessage;
    [Space]
    [Header("Scene Options")]
    public Button sceneConfirmYes;
    public GameObject sceneConfirmedOptions;
    bool mainMenuSet = false;
    bool dungeonOneSet = false;
    bool dungeonTwoSet = false;
    bool dungeonThreeSet = false;
    bool temple1Set = false;
    bool overWorldSet = false;
    bool ghostShipSet = false;
    bool dwellingTimberSet = false;
    [Space]
    [Header("Save Options")]
    public Button saveConfirmYes;
    public Button quitConfirmYes;
    public GameObject saveConfirmedOptions;
    public GameObject quitConfirmOptions;

    void Start()
    {
        PauseGame pauseGame = GetComponent<PauseGame>();
        pauseGame.EnableMouse(true);
    }
   
    public void FillMessage(bool active)
    {
        if (active)
            dialogueText.text = messages.popMessage;
        else
        {
            dialogueText.text = null;
            messages.popMessage = null;
        }
    }
    public IEnumerator AnimateMessage()
    {
        dialogueText.text = null;
        
        foreach(char letter in messages.popMessage)
        {
            isDialogueActive = true;
            audioSrc = GetComponent<AudioSource>();
            audioSrc.volume = 1f;
            audioSrc.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSrc.PlayOneShot(scrollSound);
            dialogueText.text += letter;


            if (!isDialogueActive)
            {
                dialogueText.text = null;
                messages.popMessage = null;
                break;
            }
            else if (PlayerSystem.isDead)
            {

                dialogueText.text = null;
                messages.popMessage = null;
                DialogueBanner.SetActive(false);
                break;
            }
         
            yield return new WaitForSecondsRealtime(0.0001f);
        }
    }
    IEnumerator AnimateWelcome()
    {
        foreach (char letter in messages.welcomeMsg.ToCharArray())
        {
            isDialogueActive = true;
            audioSrc = GetComponent<AudioSource>();
            audioSrc.volume = 1f;
            audioSrc.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSrc.PlayOneShot(scrollSound);
            dialogueText.text += letter;
            if (dialogueText.text.Length == messages.welcomeMsg.Length)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                button1I.SetActive(true);
                button1.interactable = true;
                m_Selectable = button1.GetComponent<Selectable>();
                m_Selectable.Select();
                break;
            }
            yield return new WaitForSecondsRealtime(.01f);
        }
    }
    IEnumerator AnimateTutorialB1()
    {
        foreach (char letter in messages.AskMsg.ToCharArray())
        {
            isDialogueActive = true;
            audioSrc = GetComponent<AudioSource>();
            audioSrc.volume = 1f;
            audioSrc.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSrc.PlayOneShot(scrollSound);
            dialogueText.text += letter;
            if (dialogueText.text.Length == messages.AskMsg.Length)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                button2I.SetActive(true);
                button2.interactable = true;
                m_Selectable = button2.GetComponent<Selectable>();
                m_Selectable.Select();
                break;
            }
            yield return new WaitForSecondsRealtime(.01f);
        }
    }
    IEnumerator AnimateTutorialB2()
    {
        foreach (char letter in messages.ContinueMsg.ToCharArray())
        {
            isDialogueActive = true;
            audioSrc = GetComponent<AudioSource>();
            audioSrc.volume = 1f;
            audioSrc.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSrc.PlayOneShot(scrollSound);
            dialogueText.text += letter;
            if (dialogueText.text.Length == messages.ContinueMsg.Length)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                ConfirmedOptions.SetActive(true);
                m_Selectable = ConfirmYes.GetComponent<Selectable>();
                m_Selectable.Select();
                break;
            }
            yield return new WaitForSecondsRealtime(.01f);
        }
    }
    IEnumerator AnimateDeadMessage()
    {
        if (!ItemSystem.lifeBerryEmpty)
        {
            deathTime = 0.01f;
            storeMessage = messages.DeadMsg;
        }
        else if (ItemSystem.lifeBerryEmpty)
        {
            deathTime = 0.05f;
            storeMessage = messages.gameOverMsg;
        }

        foreach (char letter in storeMessage.ToCharArray())
        {
            isDialogueActive = true;
            audioSrc = GetComponent<AudioSource>();
            audioSrc.volume = 1f;
            audioSrc.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSrc.PlayOneShot(scrollSound);
            PlayerSystem playerSys = GetComponent<PlayerSystem>();
            CharacterSystem player = GetComponent<CharacterSystem>();
            ItemSystem itemSys = GetComponent<ItemSystem>();
            SceneSystem sceneSys = GameObject.Find("Core/Player").GetComponent<SceneSystem>();

            deadDialogueText.text += letter;
            if (deadDialogueText.text.Length == storeMessage.Length)
            {
                ShutOffActive();
                if (!ItemSystem.lifeBerryEmpty)
                {
                    yield return new WaitForSecondsRealtime(2);
                    itemSys.UseItem(ItemSystem.Item.LifeBerry, 1);
                    DeadOptions.SetActive(false);
                    deadDialogueText.text = null;
                    playerSys.PlayerDied(false);
                    break;
                }
                else if (ItemSystem.lifeBerryEmpty)
                {
                    musicAudioSrc.clip = gameOverMusic;
                    if(fadeAudio != null)
                        StopCoroutine(fadeAudio);
                    fadeAudio = AudioFadeIn(musicAudioSrc);
                    StartCoroutine(fadeAudio);
                    GameObject enemyUIObjs = GameObject.Find("Core/Player/PopUpCanvas/EnemyUIStorage");
                    foreach (Transform child in enemyUIObjs.transform)
                        Destroy(child.gameObject);
                    yield return new WaitForSecondsRealtime(6);
                    if (fadeAudio != null)
                        StopCoroutine(fadeAudio);
                    fadeAudio = AudioFadeOut(musicAudioSrc);
                    StartCoroutine(fadeAudio);
                    player.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                    sceneSys.StartScene(SceneSystem.Level.Title);
                    DeadOptions.SetActive(false);
                    deadDialogueText.text = null;
                    playerSys.PlayerDied(false);
                    yield return new WaitUntil(() => SceneSystem.inTransition == false);
                    
                    break;
                }
               
            }
            yield return new WaitForSecondsRealtime(deathTime);
        }
    }
    IEnumerator AnimateScene()
    {
        foreach (char letter in messages.sceneMsg.ToCharArray())
        {
            isDialogueActive = true;
            audioSrc = GetComponent<AudioSource>();
            audioSrc.volume = 1f;
            audioSrc.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSrc.PlayOneShot(scrollSound);
            dialogueText.text += letter;
            if (dialogueText.text.Length == messages.sceneMsg.Length)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                sceneConfirmedOptions.SetActive(true);
                m_Selectable = sceneConfirmYes.GetComponent<Selectable>();
                m_Selectable.Select();
                break;
            }
            yield return new WaitForSecondsRealtime(.01f);
        }
    }
    IEnumerator AnimateSave()
    {
        foreach (char letter in messages.saveMsg.ToCharArray())
        {
            isDialogueActive = true;
            audioSrc = GetComponent<AudioSource>();
            audioSrc.volume = 1f;
            audioSrc.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSrc.PlayOneShot(scrollSound);
            dialogueText.text += letter;
            if (dialogueText.text.Length == messages.saveMsg.Length)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                saveConfirmedOptions.SetActive(true);
                m_Selectable = saveConfirmYes.GetComponent<Selectable>();
                m_Selectable.Select();
                break;
            }
            yield return new WaitForSecondsRealtime(.001f);
        }
    }
    IEnumerator AnimateQuit()
    {
        foreach (char letter in messages.quitMsg.ToCharArray())
        {
            isDialogueActive = true;
            audioSrc = GetComponent<AudioSource>();
            audioSrc.volume = 1f;
            audioSrc.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSrc.PlayOneShot(scrollSound);
            dialogueText.text += letter;
            if (dialogueText.text.Length == messages.quitMsg.Length)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                quitConfirmOptions.SetActive(true);
                m_Selectable = quitConfirmYes.GetComponent<Selectable>();
                m_Selectable.Select();
                break;
            }
            yield return new WaitForSecondsRealtime(.001f);
        }
    }


    public IEnumerator AudioFadeOut(AudioSource audio)
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
                break;
            }
        }
    }
    public IEnumerator AudioFadeIn(AudioSource audio)
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
                break;
            }
        }
    }
    public enum ButtonType {Welcome, TutorialB1, TutorialB2, Dead, CloseMessage, Scene, Save, quit}
    public void PressButton(ButtonType type)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        switch (type)
        {
            case ButtonType.Welcome:
                {
                    DialogueBanner.SetActive(true);
                    dialogueText.text = null;
                    if (routine != null)
                        StopCoroutine(routine);
                    routine = AnimateWelcome();
                    StartCoroutine(routine);
                    break;
                }
            case ButtonType.TutorialB1:
                {
                    button1I.SetActive(false);
                    button1.interactable = false;
                    dialogueText.text = null;
                    if (routine != null)
                        StopCoroutine(routine);
                    routine = AnimateTutorialB1();
                    StartCoroutine(routine);
                   
                    break;
                }
            case ButtonType.TutorialB2:
                {
                    button2I.SetActive(false);
                    button2.interactable = false;
                    dialogueText.text = null;
                    if (routine != null)
                        StopCoroutine(routine);
                    routine = AnimateTutorialB2();
                    StartCoroutine(routine);
                    break;
                }
            case ButtonType.Dead:
                {
                    DeadOptions.SetActive(true);
                    deadDialogueText.text = null;
                    if (routine != null)
                        StopCoroutine(routine);
                    routine = AnimateDeadMessage();
                    StartCoroutine(routine);
                    break;
                }
            case ButtonType.CloseMessage:
                {
                    ShutOffActive();
                    if (routine != null)
                        StopCoroutine(routine);
                    if (!musicAudioSrc.isPlaying)
                    {
                        if (fadeAudio != null)
                            StopCoroutine(fadeAudio);
                        fadeAudio = AudioFadeIn(musicAudioSrc);
                        StartCoroutine(fadeAudio);
                    }
                    DialogueBanner.SetActive(false);
                    OpenItemPreview(false, ItemPreviewType.none);
                    SceneSystem scene = GameObject.Find("Core/Player").GetComponent<SceneSystem>();
                    scene.EnablePlayer(true);
                    //CharacterSystem playChar = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterSystem>();
                    //playChar.PlayerInteraction(false);
                    break;
                }
            case ButtonType.Scene:
                {
                    CharacterSystem charSys = GetComponent<CharacterSystem>();
                    SceneSystem playerScene = GameObject.Find("Core/Player").GetComponent<SceneSystem>();
                    playerScene.EnablePlayer(false);
                    charSys.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                    playerScene.gameUI.SetActive(false);
                    IEnumerator blackScreen = playerScene.FadeIn(1.0f, 2.0f, playerScene.blackScreenGame, true);
                    StartCoroutine(blackScreen);
                 
                    messages.sceneMsg = null;
                    dialogueText.text = null;
                    
                    if (routine != null)
                        StopCoroutine(routine);
                   
                    if (musicAudioSrc.isPlaying)
                    {
                        if (fadeAudio != null)
                            StopCoroutine(fadeAudio);
                        fadeAudio = AudioFadeOut(musicAudioSrc);
                        StartCoroutine(fadeAudio);
                    }
                    sceneConfirmedOptions.SetActive(false);
                    DialogueBanner.SetActive(false);
                    
                    if (mainMenuSet)
                    {
                        playerScene.LoadSceneSystem(0);
                        mainMenuSet = false;
                    }
                    else if (dungeonOneSet)
                    {
                        playerScene.LoadSceneSystem(1);
                        dungeonOneSet = false;
                    }
                    else if (dungeonTwoSet)
                    {
                        playerScene.LoadSceneSystem(4);
                        dungeonTwoSet = false;
                    }
                    else if (temple1Set)
                    {
                        playerScene.LoadSceneSystem(5);
                        temple1Set = false;
                    }
                    else if (overWorldSet)
                    {
                        playerScene.LoadSceneSystem(2);
                        overWorldSet = false;
                    }
                    else if (ghostShipSet)
                    {
                        playerScene.LoadSceneSystem(3);
                        ghostShipSet = false;
                    }
                    else if (dwellingTimberSet)
                    {
                        playerScene.LoadSceneSystem(8);
                        dwellingTimberSet = false;
                    }
                    else if (dungeonThreeSet)
                    {
                        playerScene.LoadSceneSystem(9);
                        dungeonThreeSet = false;
                    }
                    break;
                }
            case ButtonType.Save:
                {
                    dialogueText.text = null;
                    DialogueBanner.SetActive(true);
                    if (routine != null)
                        StopCoroutine(routine);
                    routine = AnimateSave();
                    StartCoroutine(routine);
                    break;
                }
            case ButtonType.quit:
                {
                    dialogueText.text = null;
                    DialogueBanner.SetActive(true);
                    if (routine != null)
                        StopCoroutine(routine);
                    routine = AnimateQuit();
                    StartCoroutine(routine);
                    break;
                }

        }
    }
    public void SetButtonPress(int num)
    {
        ShutOffActive();
        if (!isDialogueActive)
        {
            if (num == 0)
                PressButton(ButtonType.Welcome);
            else if (num == 1)
                PressButton(ButtonType.TutorialB1);
            else if (num == 2)
                PressButton(ButtonType.TutorialB2);
            else if (num == 3)
                PressButton(ButtonType.Dead);
            else if (num == 4)
                PressButton(ButtonType.CloseMessage);
            else if (num == 5)
                PressButton(ButtonType.Scene);
         
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("IntroSceneOut"))
        {
            mainMenuSet = false;
            dungeonOneSet = false;
            dungeonTwoSet = false;
            dungeonThreeSet = false;
            temple1Set = false;
            overWorldSet = true;
            ghostShipSet = false;
            dwellingTimberSet = false;
            PressButton(ButtonType.Scene);
        }
        if (other.gameObject.CompareTag("Dungeon1In"))
        {
            mainMenuSet = false;
            dungeonOneSet = true;
            dungeonTwoSet = false;
            dungeonThreeSet = false;
            temple1Set = false;
            overWorldSet = false;
            ghostShipSet = false;
            dwellingTimberSet = false;
            PressButton(ButtonType.Scene);
        }
        if (other.gameObject.CompareTag("Dungeon1Out"))
        {
            mainMenuSet = false;
            dungeonOneSet = false;
            dungeonTwoSet = false;
            dungeonThreeSet = false;
            temple1Set = false;
            overWorldSet = true;
            ghostShipSet = false;
            dwellingTimberSet = false;
            PressButton(ButtonType.Scene);
        }
        if (other.gameObject.CompareTag("Dungeon2In"))
        {
            mainMenuSet = false;
            dungeonTwoSet = true;
            dungeonThreeSet = false;
            temple1Set = false;
            dungeonOneSet = false;
            overWorldSet = false;
            ghostShipSet = false;
            dwellingTimberSet = false;
            PressButton(ButtonType.Scene);
        }
        if (other.gameObject.CompareTag("Dungeon2Out"))
        {
            mainMenuSet = false;
            dungeonOneSet = false;
            dungeonTwoSet = false;
            dungeonThreeSet = false;
            temple1Set = false;
            overWorldSet = true;
            ghostShipSet = false;
            dwellingTimberSet = false;
            PressButton(ButtonType.Scene);
        }
        if (other.gameObject.CompareTag("Dungeon2Part2In"))
        {
            mainMenuSet = false;
            dungeonTwoSet = false;
            dungeonThreeSet = false;
            temple1Set = true;
            dungeonOneSet = false;
            overWorldSet = false;
            ghostShipSet = false;
            dwellingTimberSet = false;
            PressButton(ButtonType.Scene);
        }
        if (other.gameObject.CompareTag("Dungeon2Part2Out"))
        {
            mainMenuSet = false;
            dungeonOneSet = false;
            dungeonTwoSet = false;
            dungeonThreeSet = false;
            temple1Set = false;
            overWorldSet = true;
            ghostShipSet = false;
            dwellingTimberSet = false;
            PressButton(ButtonType.Scene);
        }
        if (other.gameObject.CompareTag("Dungeon2AltIn"))
        {
            mainMenuSet = false;
            dungeonTwoSet = true;
            temple1Set = false;
            dungeonOneSet = false;
            dungeonThreeSet = false;
            overWorldSet = false;
            ghostShipSet = false;
            dwellingTimberSet = false;
            GameObject player = GameObject.Find("Core/Player");
            SceneSystem sceneSys = player.GetComponent<SceneSystem>();
            sceneSys.SetAltEntrance(SceneSystem.Position.Dungeon2AltIn);
            PressButton(ButtonType.Scene);
        }
        if (other.gameObject.CompareTag("Dungeon2AltOut"))
        {
            mainMenuSet = false;
            dungeonOneSet = false;
            dungeonTwoSet = false;
            dungeonThreeSet = false;
            temple1Set = false;
            overWorldSet = true;
            ghostShipSet = false;
            dwellingTimberSet = false;
            GameObject player = GameObject.Find("Core/Player");
            SceneSystem sceneSys = player.GetComponent<SceneSystem>();
            sceneSys.SetAltEntrance(SceneSystem.Position.Dungeon2AltOut);
            PressButton(ButtonType.Scene);
        }
        if (other.gameObject.CompareTag("GhostShipIn"))
        {
            audioSrc = GetComponent<AudioSource>();
            audioSrc.PlayOneShot(GhostEncounterSfx);
            mainMenuSet = false;
            dungeonOneSet = false;
            dungeonTwoSet = false;
            dungeonThreeSet = false;
            temple1Set = false;
            overWorldSet = false;
            ghostShipSet = true;
            dwellingTimberSet = false;
            PressButton(ButtonType.Scene);
        }
        if (other.gameObject.CompareTag("GhostShipOut"))
        {
            mainMenuSet = false;
            dungeonOneSet = false;
            dungeonTwoSet = false;
            dungeonThreeSet = false;
            temple1Set = false;
            overWorldSet = true;
            ghostShipSet = false;
            dwellingTimberSet = false;
            PressButton(ButtonType.Scene);
        }
        if (other.gameObject.CompareTag("DwellingTimberIn"))
        {
            mainMenuSet = false;
            dungeonTwoSet = false;
            temple1Set = false;
            dungeonOneSet = false;
            dungeonThreeSet = false;
            overWorldSet = false;
            ghostShipSet = false;
            dwellingTimberSet = true;
            PressButton(ButtonType.Scene);
        }
        if (other.gameObject.CompareTag("DwellingTimberOut"))
        {
            mainMenuSet = false;
            dungeonTwoSet = false;
            temple1Set = false;
            dungeonOneSet = false;
            dungeonThreeSet = false;
            overWorldSet = true;
            ghostShipSet = false;
            dwellingTimberSet = false;
            PressButton(ButtonType.Scene);
        }
        if (other.gameObject.CompareTag("Dungeon3In"))
        {
            mainMenuSet = false;
            dungeonOneSet = false;
            dungeonTwoSet = false;
            dungeonThreeSet = true;
            temple1Set = false;
            overWorldSet = false;
            ghostShipSet = false;
            dwellingTimberSet = false;
            PressButton(ButtonType.Scene);
        }
        if (other.gameObject.CompareTag("Dungeon3Out"))
        {
            mainMenuSet = false;
            dungeonOneSet = false;
            dungeonTwoSet = false;
            dungeonThreeSet = false;
            temple1Set = false;
            overWorldSet = true;
            ghostShipSet = false;
            dwellingTimberSet = false;
            PressButton(ButtonType.Scene);
        }
        if (other.gameObject.CompareTag("Dungeon3AltIn"))
        {
            mainMenuSet = false;
            dungeonTwoSet = false;
            temple1Set = false;
            dungeonOneSet = false;
            dungeonThreeSet = true;
            overWorldSet = false;
            ghostShipSet = false;
            dwellingTimberSet = false;
            GameObject player = GameObject.Find("Core/Player");
            SceneSystem sceneSys = player.GetComponent<SceneSystem>();
            sceneSys.SetAltEntrance(SceneSystem.Position.Dungeon3AltIn);
            PressButton(ButtonType.Scene);
        }
        if (other.gameObject.CompareTag("Dungeon3AltOut"))
        {
            mainMenuSet = false;
            dungeonOneSet = false;
            dungeonTwoSet = false;
            dungeonThreeSet = false;
            temple1Set = false;
            overWorldSet = true;
            ghostShipSet = false;
            dwellingTimberSet = false;
            GameObject player = GameObject.Find("Core/Player");
            SceneSystem sceneSys = player.GetComponent<SceneSystem>();
            sceneSys.SetAltEntrance(SceneSystem.Position.Dungeon3AltOut);
            PressButton(ButtonType.Scene);
        }
        if (other.gameObject.CompareTag("Dungeon3Alt2In"))
        {
            mainMenuSet = false;
            dungeonTwoSet = false;
            temple1Set = false;
            dungeonOneSet = false;
            dungeonThreeSet = true;
            overWorldSet = false;
            ghostShipSet = false;
            dwellingTimberSet = false;
            GameObject player = GameObject.Find("Core/Player");
            SceneSystem sceneSys = player.GetComponent<SceneSystem>();
            sceneSys.SetAltEntrance(SceneSystem.Position.Dungeon3Alt2In);
            PressButton(ButtonType.Scene);
        }
        if (other.gameObject.CompareTag("Dungeon3Alt2Out"))
        {
            mainMenuSet = false;
            dungeonOneSet = false;
            dungeonTwoSet = false;
            dungeonThreeSet = false;
            temple1Set = false;
            overWorldSet = true;
            ghostShipSet = false;
            dwellingTimberSet = false;
            GameObject player = GameObject.Find("Core/Player");
            SceneSystem sceneSys = player.GetComponent<SceneSystem>();
            sceneSys.SetAltEntrance(SceneSystem.Position.Dungeon3Alt2Out);
            PressButton(ButtonType.Scene);
        }
       
        if (other.gameObject.CompareTag("Person"))
        {
            personObj = other.gameObject;
        }

    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MainMenu"))
        {
            ShutOffActive();
            mainMenuSet = false;
            dungeonOneSet = false;
            messages.sceneMsg = null;
            dialogueText.text = null;
            if (routine != null)
                StopCoroutine(routine);
            routine = null;
            DialogueBanner.SetActive(false);
            sceneConfirmedOptions.SetActive(false);
        }
     

    }
    public void ShutOffActive()
    {
        isDialogueActive = false;
    }
    public enum ItemPreviewType { vitalityEssence, lifeberry, rixile, nicirplant, mindremedy, petrishroom, reliefointment, terraidol, magicidol, key, demonmask, moonpearl, markofetymology,
        flaresword, lightningsword, frostsword, terrasword, helixarmor, apexarmor, mythicarmor, legendaryarmor, hovers, cinders, storms, medicals, powerjewel,
        spiritjewel, timejewel, lightjewel, darkjewel, gold5, gold10, gold20, gold50, none}
    public void OpenItemPreview(bool active, ItemPreviewType type)
    {
        if (active)
        {
            itemPreviewWindow.SetActive(true);
            switch (type)
            {
                case ItemPreviewType.vitalityEssence:
                    {
                        itemTitle.text = "- Vitality Essence -";
                        itemMessage = "A gifted charm that enhances ones vitality and strength." +
                                                  " When gathering a collection of these, it's said to almost grant someone immortality!";
                        previewIcon.sprite = itemPreviewIcons[0];
                        break;
                    }
                case ItemPreviewType.lifeberry:
                    {
                        itemTitle.text = "- Life Berry -";
                        itemMessage = "Magical berries that can be mixed together to make a life reviving potion." +
                                                  " When weary, drinking one potion brings you back into the fight, reviving and healing half of the players health!";
                        previewIcon.sprite = itemPreviewIcons[1];
                        break;
                    }
                case ItemPreviewType.rixile:
                    {
                        itemTitle.text = "- Rixile -";
                        itemMessage = "Brewed with the concetrated essence of life berries and medicine." +
                                                  " This strong potion is able to heal even the worst injuries, healing players health to full!";
                        previewIcon.sprite = itemPreviewIcons[2];
                        break;
                    }
                case ItemPreviewType.nicirplant:
                    {
                        itemTitle.text = "- Nicir Plant -";
                        itemMessage = "One of the most poisonous plants found in the wild." +
                                                  " It can be brewed into powerful antidote that would detox the player from the most deadly poison!";
                        previewIcon.sprite = itemPreviewIcons[3];
                        break;
                    }
                case ItemPreviewType.mindremedy:
                    {
                        itemTitle.text = "- Mind Remedy -";
                        itemMessage = "A mysterious potion that repairs a twixed mind." +
                                                  " Drinking this potion tastes terrible but will remove any hallucinogenic confusion!";
                        previewIcon.sprite = itemPreviewIcons[4];
                        break;
                    }
                case ItemPreviewType.petrishroom:
                    {
                        itemTitle.text = "- Petri-Shroom -";
                        itemMessage = "A rare mushroom picked in a damp withered forest and mashed into a medicine." +
                                                  " Consuming this medicine restores functions of nerve damage when paralyzed and unable to move!";
                        previewIcon.sprite = itemPreviewIcons[5];
                        break;
                    }
                case ItemPreviewType.reliefointment:
                    {
                        itemTitle.text = "- Relief Ointment -";
                        itemMessage = "A special medicinal ointment that relieves muscle stigma." +
                                                  " Rubbing this ointment on muscles that have locked up relives pain allowing to use your weapons again!";
                        previewIcon.sprite = itemPreviewIcons[6];
                        break;
                    }
                case ItemPreviewType.terraidol:
                    {
                        itemTitle.text = "- Terra Idol -";
                        itemMessage = "A spiritual tablet said to have been crafted by a terraformed goddess." +
                                                  " Summoning the spiritual magic is believed to cause catastrophic earth quakes!";
                        previewIcon.sprite = itemPreviewIcons[7];
                        break;
                    }
                case ItemPreviewType.magicidol:
                    {
                        itemTitle.text = "- Magic Idol -";
                        itemMessage = "A magical tablet crafted by a mystical black smith." +
                                                  " Summoning the mystical magic is believed to fully energize the wielders weapons for a short time!";
                        previewIcon.sprite = itemPreviewIcons[8];
                        break;
                    }
                case ItemPreviewType.key:
                    {
                        itemTitle.text = "- Key -";
                        itemMessage = "A rusty withered old age key." +
                                                  " This multipurposed key is used to open a majority of locked doors. To use this item, go up to the door and use the key in the item menu!";
                        previewIcon.sprite = itemPreviewIcons[9];
                        break;
                    }
                case ItemPreviewType.demonmask:
                    {
                        itemTitle.text = "- Demon Mask -";
                        itemMessage = "Undead spirits underlie deep inside this mask." +
                                                  " Wearing this mask taints the soul of the wearer to see things that once was or used to be!";
                        previewIcon.sprite = itemPreviewIcons[10];
                        break;
                    }
                case ItemPreviewType.moonpearl:
                    {
                        itemTitle.text = "- Moon Pearl -";
                        itemMessage = "A tiny mystical pearl wrapped in a magical blue velvet." +
                                                  " Other uses of this pearl are unknown but used only during night, opens the door to the abandoned ruins!";
                        previewIcon.sprite = itemPreviewIcons[11];
                        break;
                    }
                case ItemPreviewType.markofetymology:
                    {
                        itemTitle.text = "- Mark of Etymology -";
                        itemMessage = "A green emerald amulet with concealed magic contained within the crystalized gem." +
                                      " Used to decypher encripted script in decoded books.";
                                                  ;
                        previewIcon.sprite = itemPreviewIcons[11];
                        break;
                    }
                case ItemPreviewType.flaresword:
                    {
                        itemTitle.text = "- Flare Sword -";
                        itemMessage = "This firey blade was crafted from the core of a meterorite." +
                                                  " One swing when fully energized engulfs enemies on slice, inflicting burn damage over time!";
                        previewIcon.sprite = itemPreviewIcons[12];
                        break;
                    }
                case ItemPreviewType.lightningsword:
                    {
                        itemTitle.text = "- Lightning Sword -";
                        itemMessage = "This electrifying blade was crafted from the guardians of the sky." +
                                                  " One swing when fully energized shocks enemies on slice, weakens the toughest enemies inflicting double damage!";
                        previewIcon.sprite = itemPreviewIcons[13];
                        break;
                    }
                case ItemPreviewType.frostsword:
                    {
                        itemTitle.text = "- Frost Sword -";
                        itemMessage = "This ancient cryogenic magical sword with a blade made of pure ice that never thaws." +
                                                  " One swing when fully energized solidifies enemies on slice and creates a whirlwind of frost turning enemies completey frozen solid for a short time!";
                        previewIcon.sprite = itemPreviewIcons[14];
                        break;
                    }
                case ItemPreviewType.terrasword:
                    {
                        itemTitle.text = "- Terra Sword -";
                        itemMessage = "An unknown powerful sword only whispered by legend." +
                                                  " One swing when fully energized blasts catastrophic earth quake crashes debris around the player, damaging all enemies multiple times!";
                        previewIcon.sprite = itemPreviewIcons[15];
                        break;
                    }
                case ItemPreviewType.helixarmor:
                    {
                        itemTitle.text = "- Helix Armor -";
                        itemMessage = "Perplexed magical armor with condensed sapphire." +
                                                  " Wearing this type of armor resists half the damage recieved!";
                        previewIcon.sprite = itemPreviewIcons[16];
                        break;
                    }
                case ItemPreviewType.apexarmor:
                    {
                        itemTitle.text = "- Apex Armor -";
                        itemMessage = "Crafted only for soldiers of royal blood." +
                                                  " Wearing this elegant ruby armor withstands three times the damage recieved!";
                        previewIcon.sprite = itemPreviewIcons[17];
                        break;
                    }
                case ItemPreviewType.mythicarmor:
                    {
                        itemTitle.text = "- Mythic Armor -";
                        itemMessage = " Special mythical armor crafed with dragon scales." +
                                                  " Wearing this type of armor resists four times the damage recieved!";
                        previewIcon.sprite = itemPreviewIcons[18];
                        break;
                    }
                case ItemPreviewType.legendaryarmor:
                    {
                        itemTitle.text = "- Legendary Armor -";
                        itemMessage = "Unknown... This armor is molded by a dark power worn by King drake himself." +
                                                  " Wearing this type of armor dark magic aura that shields the wearer to withstand five times the damage recieved!";
                        previewIcon.sprite = itemPreviewIcons[19];
                        break;
                    }
                case ItemPreviewType.hovers:
                    {
                        itemTitle.text = "- Hovers -";
                        itemMessage = "Special boots crafted with powerful magic giving the ability to float." +
                                                  " When these boots are worn automatically gives the ability to float when walking across spike floors!";
                        previewIcon.sprite = itemPreviewIcons[20];
                        break;
                    }
                case ItemPreviewType.cinders:
                    {
                        itemTitle.text = "- Cinders -";
                        itemMessage = "Boots molded and chiseled from pure molten volcano rock." +
                                                  " Wearing these boots are heavy on the feet but make you impervious when walking across rivers of lava!";
                        previewIcon.sprite = itemPreviewIcons[21];
                        break;
                    }
                case ItemPreviewType.storms:
                    {
                        itemTitle.text = "- Storms -";
                        itemMessage = "Advanced storm gear with auto adaption." +
                                                  " Wearing these boots allow you to climb ice, walk through swamps and gives you the ability to swim!";
                        previewIcon.sprite = itemPreviewIcons[22];
                        break;
                    }
                case ItemPreviewType.medicals:
                    {
                        itemTitle.text = "- Medicals -";
                        itemMessage = "Super technological medical footware designed by a mad scientist" +
                                                  " Wearing this type of footwear while walking aids regenerative healing abilites!";
                        previewIcon.sprite = itemPreviewIcons[23];
                        break;
                    }
                case ItemPreviewType.powerjewel:
                    {
                        itemTitle.text = "- Jewel of Power -";
                        itemMessage = "A crystal constructed of magic from the guardian of power" +
                                                  " Harnessing its power, casts a shockwave force that damages all surrounded enemies and objects!";
                        previewIcon.sprite = itemPreviewIcons[24];
                        break;
                    }
                case ItemPreviewType.spiritjewel:
                    {
                        itemTitle.text = "- Jewel of Spirit -";
                        itemMessage = "A crystal constructed of magic from the guardian of spirit" +
                                                  " Harnessing its power, grants the ability to breath underwater and jump higher!";
                        previewIcon.sprite = itemPreviewIcons[25];
                        break;
                    }
                case ItemPreviewType.timejewel:
                    {
                        itemTitle.text = "- Jewel of Time -";
                        itemMessage = "A crystal constructed of magic from the guardian of time" +
                                                  " Harnessing its power of time, allows the user to decay time itself, slowing down fast moving obstacles!";
                        previewIcon.sprite = itemPreviewIcons[26];
                        break;
                    }
                case ItemPreviewType.lightjewel:
                    {
                        itemTitle.text = "- Jewel of Light -";
                        itemMessage = "A crystal constructed of magic from the guardian of light" +
                                                  " Harnessing its power of light, cycles the sun and moon ahead 6 hours forward in a day!";
                        previewIcon.sprite = itemPreviewIcons[27];
                        break;
                    }
                case ItemPreviewType.darkjewel:
                    {
                        itemTitle.text = "- Jewel of Dark -";
                        itemMessage = "A crystal constructed of magic from the guardian of dark" +
                                                  " Harnessing its power of darkness reveals the dark dimension, enemies are aethered with darkness!";
                        previewIcon.sprite = itemPreviewIcons[28];
                        break;
                    }
                case ItemPreviewType.gold5:
                    {
                        itemTitle.text = "- 5 Gold -";
                        itemMessage = "A small amount of change, not a lot but enough that adds up over time." +
                                                  " You feel content dropping a few gold in your wallet!";
                        previewIcon.sprite = itemPreviewIcons[29];
                        break;
                    }
                case ItemPreviewType.gold10:
                    {
                        itemTitle.text = "- 10 Gold -";
                        itemMessage = "A some what small amount of change, maybe it will allow you to buy that item you needed." +
                                                  " You feel more content dropping gold in your wallet!";
                        previewIcon.sprite = itemPreviewIcons[29];
                        break;
                    }
                case ItemPreviewType.gold20:
                    {
                        itemTitle.text = "- 20 Gold -";
                        itemMessage = "A nice chunk of change, enough to give you a nights rest at an inn." +
                                                  " You feel joyed dropping gold one by one in your wallet!";
                        previewIcon.sprite = itemPreviewIcons[29];
                        break;
                    }
                case ItemPreviewType.gold50:
                    {
                        itemTitle.text = "- 50 Gold -";
                        itemMessage = "A large chunk of change, like getting that nice pay check!" +
                                                  " You feel estatic loading up gold in your wallet!";
                        previewIcon.sprite = itemPreviewIcons[29];
                        break;
                    }
                case ItemPreviewType.none:
                    {
                        break;
                    }
            }
            itemDiscriptionMsg.text = itemMessage;
        }
        else
        {
            if (itemPreviewMsgRoutine != null)
                StopCoroutine(itemPreviewMsgRoutine);
            itemTitle.text = null;
            itemDiscriptionMsg.text = null;
            previewIcon.sprite = null;
            itemPreviewWindow.SetActive(false);
            
        }
    }
    IEnumerator AnimateItemDiscription()
    {
        foreach (char letter in itemMessage)
        {
            itemDiscriptionMsg.text += letter;
            if (itemDiscriptionMsg.text.Length == itemMessage.Length)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
[Serializable]
public struct DialogueMessages
{
    [Header("Tutorial")]
    public string welcomeMsg;
    public string AskMsg;
    public string ContinueMsg;
    public string DeadMsg;
    public string gameOverMsg;
    public string saveMsg;
    public string storyMsg;
    public string popMessage;
    public string sceneMsg;
    public string quitMsg;


}

