using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MMTransition : MonoBehaviour
{

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

    SceneSystem sceneSystem;

    bool isStarted = false;

    public static bool file1Open;
    public static bool file2Open;
    public static bool file3Open;
    public static bool file4Open;

   

    private Selectable m_Selectable;

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

            if (Input.GetButtonDown("Pause") && !isStarted && pressStart.activeInHierarchy 
            || Input.GetButtonDown("Submit") && !isStarted && pressStart.activeInHierarchy 
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
        else if (Input.GetButtonDown("Pause") && isIntro
            || Input.GetButtonDown("Submit") && isIntro
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


        if (file1Open && Input.GetButtonDown("Cancel"))
            DeselectFileAnimation(0);
        else if (file2Open && Input.GetButtonDown("Cancel"))
            DeselectFileAnimation(1);
        else if (file3Open && Input.GetButtonDown("Cancel"))
            DeselectFileAnimation(2);
        else if (file4Open && Input.GetButtonDown("Cancel"))
            DeselectFileAnimation(3);
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
                {
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
                    if (CoreObject.file1Active)
                    {
                        if (CoreObject.newGameOne)
                        {
                            StopCoroutine(screenBlack);
                            screenBlack = FadeIn(1.0f, 4.0f, blackScreen, true, 5);
                            StartCoroutine(screenBlack);
                        }
                        else if (!CoreObject.newGameOne)
                        {
                            StopCoroutine(screenBlack);
                            Player.SetActive(true);
                            CoreObject core = GameObject.Find("Core").GetComponent<CoreObject>();
                            core.Loading(true);
                            core.LoadScene();
                        }
                    }
                    if (CoreObject.file2Active)
                    {
                        if (CoreObject.newGameTwo)
                        {
                            StopCoroutine(screenBlack);
                            screenBlack = FadeIn(1.0f, 4.0f, blackScreen, true, 5);
                            StartCoroutine(screenBlack);
                        }
                        else if (!CoreObject.newGameTwo)
                        {
                            StopCoroutine(screenBlack);
                            Player.SetActive(true);
                            CoreObject core = GameObject.Find("Core").GetComponent<CoreObject>();
                            core.Loading(true);
                            core.LoadScene();
                        }
                    }
                    if (CoreObject.file3Active)
                    {
                        if (CoreObject.newGameThree)
                        {
                            StopCoroutine(screenBlack);
                            screenBlack = FadeIn(1.0f, 4.0f, blackScreen, true, 5);
                            StartCoroutine(screenBlack);
                        }
                        else if (!CoreObject.newGameThree)
                        {
                            StopCoroutine(screenBlack);
                            Player.SetActive(true);
                            CoreObject core = GameObject.Find("Core").GetComponent<CoreObject>();
                            core.Loading(true);
                            core.LoadScene();
                        }
                    }
                    if (CoreObject.file4Active)
                    {
                        if (CoreObject.newGameFour)
                        {
                            StopCoroutine(screenBlack);
                            screenBlack = FadeIn(1.0f, 4.0f, blackScreen, true, 5);
                            StartCoroutine(screenBlack);
                        }
                        else if (!CoreObject.newGameFour)
                        {
                            StopCoroutine(screenBlack);
                            Player.SetActive(true);
                            CoreObject core = GameObject.Find("Core").GetComponent<CoreObject>();
                            core.Loading(true);
                            core.LoadScene();
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
       
        titleBottom.GetComponent<Image>().color = returnColor;
        subTitle.GetComponent<Image>().color = returnColor;
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
            if (fadeAudio != null)
            {
                StopCoroutine(fadeAudio);
            }
            fadeAudio = AudioFadeOut(audioSrc);
            StartCoroutine(fadeAudio);

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
        if (file1Open)
        {
            anim.SetBool("OpenFile1", false);
            file1Open = false;
        }
        if (file2Open)
        {
            anim.SetBool("OpenFile2", false);
            file2Open = false;
        }
        if (file3Open)
        {
            anim.SetBool("OpenFile3", false);
            file3Open = false;
        }
        if (file4Open)
        {
            anim.SetBool("OpenFile4", false);
            file4Open = false;
        }
    }
    public void SelectFileAnimation(int num)
    {
        Animator anim = fileMenu.GetComponent<Animator>();
        if (num == 0)
        {
            if (!file1Open)
            {
                anim.SetBool("OpenFile1", true);
                file1Open = true;
            }
            else if (file1Open && !SelectComponents.FileDeletion)
            {
                anim.Rebind();
                StartIntroSceneTransition();
                file1Open = false;
            }
            else if(file1Open && SelectComponents.FileDeletion)
            {
                AnimateText animText = animationText.GetComponent<AnimateText>();
                animText.DeletedFileOnClick(0);
                CoreObject core = GameObject.Find("Core").GetComponent<CoreObject>();
                core.DeleteExistingFile(0);
                DeselectFileAnimation(0);
            }
        }
        else if(num == 1)
        {
            if (!file2Open)
            {
                anim.SetBool("OpenFile2", true);
                file2Open = true;
            }
            else if (file2Open && !SelectComponents.FileDeletion)
            {
                anim.Rebind();
                StartIntroSceneTransition();
                file2Open = false;
            }
            else if (file2Open && SelectComponents.FileDeletion)
            {
                AnimateText animText = animationText.GetComponent<AnimateText>();
                animText.DeletedFileOnClick(1);
                CoreObject core = GameObject.Find("Core").GetComponent<CoreObject>();
                core.DeleteExistingFile(1);
                DeselectFileAnimation(1);
            }
        }
        else if (num == 2)
        {
            if (!file3Open)
            {
                anim.SetBool("OpenFile3", true);
                file3Open = true;
            }
            else if (file3Open && !SelectComponents.FileDeletion)
            {
                anim.Rebind();
                StartIntroSceneTransition();
                file3Open = false;
            }
            else if (file3Open && SelectComponents.FileDeletion)
            {
                AnimateText animText = animationText.GetComponent<AnimateText>();
                animText.DeletedFileOnClick(2);
                CoreObject core = GameObject.Find("Core").GetComponent<CoreObject>();
                core.DeleteExistingFile(2);
                DeselectFileAnimation(2);
            }
        }
        else if (num == 3)
        {
            if (!file4Open)
            {
                anim.SetBool("OpenFile4", true);
                file4Open = true;
            }
            else if (file4Open && !SelectComponents.FileDeletion)
            {
                anim.Rebind();
                StartIntroSceneTransition();
                file4Open = false;
            }
            else if (file4Open && SelectComponents.FileDeletion)
            {
                AnimateText animText = animationText.GetComponent<AnimateText>();
                animText.DeletedFileOnClick(3);
                CoreObject core = GameObject.Find("Core").GetComponent<CoreObject>();
                core.DeleteExistingFile(3);
                DeselectFileAnimation(3);
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
