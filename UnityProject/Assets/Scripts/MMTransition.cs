using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MMTransition : MonoBehaviour
{
    OptSystem optSystem = new OptSystem();
    public Intro introMsg;
    public Material orgSky;
    string selectedMessage;
    string curMessage;
    int textCounter = 1;

    public Animator tmAnim;

    public AudioSource audioSrc;
    public AudioSource soundAudioSrc;
    public AudioClip titleMusic;
    public AudioClip menuMusic;
    public AudioClip introMusic;
    public AudioClip startEffect;

    public GameObject titleMenuEffects;
    public GameObject fileMenuEffects;
    public GameObject introMenuEffects;
    public GameObject titleMode;

    public GameObject fileMenu;
    public Button startButton;
    public Button introButton;

    public Image blackScreen;
    public Image titleTop;
    public Image titleBottom;
    public Image subTitle;
    public Image flashScreen;

    IEnumerator fadeAudio = null;
    IEnumerator screenBlack = null;
    IEnumerator scrolling = null;
    IEnumerator animateRoutine = null;

    public Text introText;

    public GameObject titleMain;
    public GameObject fileMain;
    public GameObject introMenu;
    public GameObject pressStart;
    public GameObject Player;
    
    public GameObject loadingGame;

    GameObject MainMenuLevel;
    public GameObject animationText;
 
    public float titleTopFillAmount = 0.0f;
    public float flashScreenTime = 0.01f;
    public float flashScreenTimer;
    public bool flash;
    bool isIntro;
    CoreObject core;
    SceneSystem sceneSystem;

    bool isStarted = false;

    public static bool[] fileOpen = new bool[4] { false, false, false, false };
    private Selectable m_Selectable;
    private void OnEnable()
    {
        core = GameObject.Find("Core").GetComponent<CoreObject>();
    }

    private void Start()
    {
        StartTitleIntro();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        introText.text = null;
    }
    public void SetTitleModeActive(int num)
    {
        if (num == 1)
            titleMode.SetActive(true);
        else
            titleMode.SetActive(false);
    }
    void Update()
    {
        handler();
        if (RenderSettings.skybox != orgSky)
        {
            RenderSettings.skybox = orgSky;
            DynamicGI.UpdateEnvironment();
        }

            if (optSystem.Input.GetButtonDown("Pause") && !isStarted && pressStart.activeInHierarchy 
            || optSystem.Input.GetButtonDown("Submit") && !isStarted && pressStart.activeInHierarchy 
            || Input.GetMouseButtonDown(0) && !isStarted && pressStart.activeInHierarchy
            || Input.GetMouseButtonDown(1) && !isStarted && pressStart.activeInHierarchy
            || Input.GetKeyDown(KeyCode.Space) && !isStarted && pressStart.activeInHierarchy)
        {
            flash = true;
            tmAnim.SetTrigger("PressStart");
            StopCoroutine(screenBlack);
            screenBlack = FadeIn(1.0f, 4.0f, blackScreen, true, 3);
            StartCoroutine(screenBlack);
            fadeAudio = AudioFadeOut(audioSrc);
            StartCoroutine(fadeAudio);
            pressStart.SetActive(false);
            soundAudioSrc.PlayOneShot(startEffect);
            isStarted = true;
        }
        else if (optSystem.Input.GetButtonDown("Pause") && isIntro
            || optSystem.Input.GetButtonDown("Submit") && isIntro
            || Input.GetMouseButtonDown(0) && isIntro
            || Input.GetMouseButtonDown(1) && isIntro
            || Input.GetKeyDown(KeyCode.Space) && isIntro)
        {
            isIntro = false;
            if(screenBlack != null)
                StopCoroutine(screenBlack);
            screenBlack = FadeIn(1.0f, 4.0f, blackScreen, true, 6);
            StartCoroutine(screenBlack);
            if(fadeAudio != null)
                StopCoroutine(fadeAudio);
            fadeAudio = AudioFadeOut(audioSrc);
            StartCoroutine(fadeAudio);
        }
        if (flash)
        {
            if (flashScreenTimer > 0)
            {
                flashScreenTimer -= Time.unscaledDeltaTime;
                flashScreen.enabled = true;
            }
            else if (flashScreenTimer <= 0)
            {
                flashScreen.enabled = false;
                flash = false;
                flashScreenTimer = flashScreenTime;
            }
        }

        for(int fo = 0; fo < 4; fo++)
        {
            if (fileOpen[fo] && optSystem.Input.GetButtonDown("Cancel"))
                DeselectFileAnimation(fo);
        }
    }
    public IEnumerator ScrollTitle(float fillTime)
    {
        titleTopFillAmount = 0;
        audioSrc.clip = titleMusic;
        audioSrc.loop = true;
        fadeAudio = AudioFadeIn(audioSrc);
        StartCoroutine(fadeAudio);
        for (float t = 0.0f; t < 1.0f; t += Time.unscaledDeltaTime / fillTime)
        {
            titleTopFillAmount += 0.0065f;
        
            if (t > 0.8)
            {
                titleTopFillAmount = 1;
                StopCoroutine(scrolling);
                screenBlack = FadeIn(1.0f, 0.5f, titleBottom, true, 0); 
                StartCoroutine(screenBlack);
                break;
            }
            yield return null;
        }
    }
    public IEnumerator FadeImage(Image image, bool fadeIn)
    {
        float alpha = image.color.a;
        float inFactor = fadeIn ? 0 : alpha;
        float outFactor = fadeIn ? alpha : 0;
        for (float a = 0; a < 1; a += 0.01f)
        {
            image.color = optSystem.Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(inFactor, outFactor, a));
            if (a > 0.99)
                image.color = optSystem.Color(image.color.r, image.color.g, image.color.b, outFactor);
            yield return optSystem.WaitRealtime(0.001f);
        }
        yield break;
    }
    public IEnumerator FadeIn(float aValue, float aTime, Image image, bool imgProperty, int num)
    {
        float alpha = image.GetComponent<Image>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.fixedUnscaledDeltaTime / aTime)
        {
            Color newColor = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(alpha, aValue, t));
            image.GetComponent<Image>().color = newColor;
            if (t >= 0.9)
            {
                if (imgProperty)
                    newColor = new Color(0, 0, 0, 1);
                else
                    newColor = new Color(1, 1, 1, 1);
                image.GetComponent<Image>().color = newColor;
                
                if (num == 0)
                {d
                    StopCoroutine(screenBlack);
                    screenBlack = FadeIn(1.0f, 0.5f, titleBottom, false, 1);
                    StartCoroutine(screenBlack);
                }
                else if (num == 1)
                {
                    StopCoroutine(screenBlack);
                    screenBlack = FadeIn(1.0f, 0.5f, subTitle, false, 2);
                    StartCoroutine(screenBlack);
                }
                else if (num == 2)
                {
                    pressStart.SetActive(true);
                    flash = true;
                }
                else if (num == 3)
                {
                    if (titleMode.activeInHierarchy)
                        SetTitleModeActive(0);
                    CoreObject core = GameObject.Find("Core").GetComponent<CoreObject>();
                    core.LoadFileExisting();
                    audioSrc.clip = menuMusic;
                    audioSrc.loop = true;

                    StopCoroutine(fadeAudio);
                    fadeAudio = AudioFadeIn(audioSrc);
                    StartCoroutine(fadeAudio);
                    StopCoroutine(screenBlack);
                    screenBlack = FadeOut(0.0f, 2.5f, blackScreen, true);
                    StartCoroutine(screenBlack);
               
                    MainMenuLevel = GameObject.Find("MainMenuLevel");
                    if (MainMenuLevel != null)
                        MainMenuLevel.SetActive(true);
                    titleMain.SetActive(false);
                    titleMenuEffects.SetActive(false);
                    fileMain.SetActive(true);
                    fileMenu.SetActive(true);
                    fileMenuEffects.SetActive(true);
                    m_Selectable = startButton.GetComponent<Selectable>();
                    m_Selectable.Select();
                    
                }
                else if (num == 4)
                {
                    fileMain.SetActive(false);
                    fileMenuEffects.SetActive(false);
                    for(int fa = 0; fa < 4; fa++)
                    {
                        if (CoreObject.fileActive[fa])
                        {
                            if (CoreObject.newGame[fa])
                            {
                                StopCoroutine(screenBlack);
                                screenBlack = FadeIn(1.0f, 4.0f, blackScreen, true, 5);
                                StartCoroutine(screenBlack);
                            }
                            else if (!CoreObject.newGame[fa])
                            {
                                StopCoroutine(screenBlack);
                                Player.SetActive(true);
                                core.Loading(true);
                                core.LoadScene();
                            }
                        }
                    }
                    Color returnColor2 = new Color(0, 0, 0, 0);
                    blackScreen.GetComponent<Image>().color = returnColor2;
                }
                else if (num == 5)
                {
                    audioSrc.clip = introMusic;
                    audioSrc.loop = true;
                    StopCoroutine(fadeAudio);
                    fadeAudio = AudioFadeIn(audioSrc);
                    StartCoroutine(fadeAudio);
                    StopCoroutine(screenBlack);
                    screenBlack = FadeOut(0.0f, 2.5f, blackScreen, true);
                    StartCoroutine(screenBlack);
                    startIntroTransition();
                    titleMain.SetActive(false);
                    titleMenuEffects.SetActive(false);
                    introMenu.SetActive(true);
                    introMenuEffects.SetActive(true);
                    isIntro = true;
                }
                else if (num == 6)
                {
                    pressStart.SetActive(true);

                    titleMain.SetActive(true);
                    titleMenuEffects.SetActive(true);

                    fileMain.SetActive(false);
                    fileMenuEffects.SetActive(false);

                    introMenu.SetActive(false);
                    introMenuEffects.SetActive(false);

                    Player.SetActive(true);
                    sceneSystem = Player.GetComponent<SceneSystem>();
                    yield return new WaitForSeconds(2);
                    sceneSystem.StartScene(SceneSystem.Level.Intro);
                    break;
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
        for (float t = 0.0f; t < 1.0f; t += Time.fixedUnscaledDeltaTime / aTime)
        {
            Color newColor = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(alpha, aValue, t));
            image.GetComponent<Image>().color = newColor;
            if (t >= 0.9)
            {
              
                if (imgProperty)
                    newColor = new Color(0, 0, 0, 0);
                else
                    newColor = new Color(1, 1, 1, 0);
                image.GetComponent<Image>().color = newColor;
                if (SceneSystem.isQuit)
                    Player.SetActive(false);
            }
            else
            {
                image.GetComponent<Image>().enabled = true;
            }
            yield return null;
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
    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
    void handler()
    {
        titleTop.fillAmount = Map(titleTopFillAmount, 0, 1, 0, 1);
    }
    public void StartTitleIntro()
    {
        titleMain.SetActive(true);
        fileMenu.SetActive(true);
        introMenu.SetActive(false);
        titleMenuEffects.SetActive(true);
       
        isStarted = false;
        Color returnColor = new Color(1, 1, 1, 0);
        if (fadeAudio != null)
        {
            StopCoroutine(fadeAudio);
        }
       
        titleBottom.color = returnColor;
        subTitle.color = returnColor;
        pressStart.SetActive(false);
        flashScreenTimer = flashScreenTime;
        scrolling = ScrollTitle(2.8f);
        StartCoroutine(scrolling);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }
    public void StartIntroSceneTransition()
    {
        MainMenuLevel = GameObject.Find("MainMenuLevel");
        if (MainMenuLevel != null)
            MainMenuLevel.SetActive(false);
        if (!SelectComponents.FileDeletion)
        {
           
            fileMenu.SetActive(false);
            AudioSystem.FadeMusic(false);

            screenBlack = FadeIn(1.0f, 4.0f, blackScreen, true, 4);
            StartCoroutine(screenBlack);
            
        }
    }
    public IEnumerator AnimateIntroText()
    {
        curMessage = null;
        introText.text = null;
        if (textCounter == 1)
            curMessage = introMsg.message1;
        else if (textCounter == 2)
            curMessage = introMsg.message2;
        else if (textCounter == 3)
            curMessage = introMsg.message3;
        else if (textCounter == 4)
            curMessage = introMsg.message4;
        else if (textCounter == 5)
            curMessage = introMsg.message5;
        foreach (char letter in curMessage.ToCharArray())
        {
           introText.text += letter;
            if (introText.text.Length == curMessage.Length)
            {
                textCounter++;
                if (textCounter > 5)
                {
                    yield return new WaitForSecondsRealtime(5);
                    StopCoroutine(screenBlack);
                    screenBlack = FadeIn(1.0f, 0.5f, blackScreen, true, 6);
                    StartCoroutine(screenBlack);
                    if (fadeAudio != null)
                    {
                        StopCoroutine(fadeAudio);
                    }
                    fadeAudio = AudioFadeOut(audioSrc);
                    StartCoroutine(fadeAudio);
                }
                else
                {
                    yield return new WaitForSecondsRealtime(5);
                    startIntroTransition();
                }
                break;
            }
            yield return 0;
            yield return new WaitForSecondsRealtime(.05f);
        }
    }
    public void startIntroTransition()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (animateRoutine != null)
            StopCoroutine(animateRoutine);
        animateRoutine = AnimateIntroText();
        StartCoroutine(animateRoutine);
        
    }
    public void DeselectFileAnimation(int num)
    {
        Animator anim = fileMenu.GetComponent<Animator>();
        string animID = "OpenFile" + (num + 1).ToString();
        if (fileOpen[num])
        {
            anim.SetBool(animID, false);
            fileOpen[num] = false;
        }
    }
    public void SelectFileAnimation(int num)
    {
        Animator anim = fileMenu.GetComponent<Animator>();
        for (int fo = 0; fo < 4; fo++)
        {
            if (num == fo)
            {
                string animID = "OpenFile" + (fo + 1).ToString();
                if (!fileOpen[fo])
                {
                    anim.SetBool(animID, true);
                    fileOpen[fo] = true;
                }
                else if(fileOpen[fo])
                {
                    if (SelectComponents.FileDeletion)
                    {
                        AnimateText animText = animationText.GetComponent<AnimateText>();
                        animText.DeletedFileOnClick(fo);
                        core.DeleteExistingFile(fo);
                        DeselectFileAnimation(fo);
                    }
                    else
                    {
                        anim.Rebind();
                        StartIntroSceneTransition();
                        fileOpen[fo] = false;
                    }
                }
            }
        }
    }
}
[Serializable]
public struct Intro
{
    public string message1;
    public string message2;
    public string message3;
    public string message4;
    public string message5;
}
