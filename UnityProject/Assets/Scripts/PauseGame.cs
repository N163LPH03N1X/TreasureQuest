using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;
public class PauseGame : MonoBehaviour
{
    OptSystem optSystem = new OptSystem();
    private Selectable m_Selectable;
    public static bool isPaused = false;
    bool isSave;
    public Text yNavText;
    public bool mouseVisible = false;
    public static bool isStatus;
    public GameObject gameMenu;
    public GameObject Options;
    public GameObject Items;
    public GameObject Equipment;
    public GameObject Journal;
    public GameObject Status;
    public GameObject pauseBanner;
    public GameObject gameUI;
    public GameObject jewelUI;
    public EventSystem eventSystem;
    DialogueSystem diagSys;
    InteractionSystem interaction;
    public Button ItemButton;
    public Image backGround;
    AudioSource audioSrc;
    public AudioClip pauseSfx;
    IEnumerator fadeImage = null;
    UnderWaterSystem underWaterSys;
    bool interactionActive;

    void Update()
    {
        if (!SceneSystem.isDisabled && !PlayerSystem.isDead && !SceneSystem.inTransition && !CoreObject.isSaving && !DialogueSystem.isDialogueActive && !ShopSystem.isShop && !CharacterSystem.isCarrying)
        {
            if (optSystem.Input.GetButtonDown("Pause"))
                PauseTheGame();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
            mouseVisible = !mouseVisible;

        if (isPaused && !ShopSystem.isShop)
        {
            if (optSystem.Input.GetButtonDown("Select") && !isStatus)
            {
                isStatus = true;
                pauseBanner.SetActive(false);
                Status.SetActive(true);
                Equipment.SetActive(false);
                Journal.SetActive(false);
                Options.SetActive(false);
                Items.SetActive(false);
                gameUI.SetActive(false);
                yNavText.text = "Save";
                audioSrc = GetComponent<AudioSource>();
                audioSrc.volume = 1;
                audioSrc.pitch = 1;
                audioSrc.PlayOneShot(pauseSfx);
                PlayerSystem playSys = GetComponent<PlayerSystem>();
                playSys.ReEvaluateHearts();
                playSys.SetHealthHeartRatio();
                playSys.ApplyHealthOrVitality();

            }
            else if (optSystem.Input.GetButtonDown("Select") && isStatus && !isSave && !CoreObject.isSaving && !UnderWaterSystem.isSwimming)
            {
                diagSys = GetComponent<DialogueSystem>();
                diagSys.PressButton(DialogueSystem.ButtonType.Save);
                Saving(true);
                audioSrc = GetComponent<AudioSource>();
                audioSrc.volume = 1;
                audioSrc.pitch = 1;
                audioSrc.PlayOneShot(pauseSfx);
            }
            else if(optSystem.Input.GetButtonDown("Select") && isStatus && !isSave && !CoreObject.isSaving && UnderWaterSystem.isSwimming)
            {
                InteractionSystem interact = GetComponent<InteractionSystem>();
                interact.DialogueInteraction(true, "Cannot Save While Swimming.");
                interactionActive = true;
            }
        }
        if (optSystem.Input.GetButtonDown("Submit") && interactionActive)
        {
            InteractionSystem interact = GetComponent<InteractionSystem>();
            interact.DialogueInteraction(false, null);
            interactionActive = false;
        }
        if(!ShopSystem.isShop && !DebugSystem.toggleCodeList)
            EnableMouse(mouseVisible);
        

    }
    public void BackOut()
    {
        if (!isSave && !CoreObject.isSaving && !DialogueSystem.isDialogueActive && !ShopSystem.isShop)
        {
            audioSrc = GetComponent<AudioSource>();
            audioSrc.volume = 1;
            audioSrc.pitch = 1;
            audioSrc.PlayOneShot(pauseSfx);
            isStatus = false;
            Status.SetActive(false);
            pauseBanner.SetActive(true);
            gameUI.SetActive(true);
            m_Selectable = ItemButton.GetComponent<Selectable>();
            m_Selectable.Select();
            yNavText.text = "Status";

        }
    }
    public void Saving(bool save)
    {
        isSave = save;
    }
    public void EnableMouse(bool True)
    {
        if (True)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }
    public void PauseTheGame()
    {
        interaction = GetComponent<InteractionSystem>();

        isPaused = !isPaused;
        interaction.DialogueInteraction(false, null);
        GameObject PopupCanvas = GameObject.Find("Core/Player/PopUpCanvas");
        PopupCanvas.SetActive(!isPaused);
        Time.timeScale = isPaused ? 0 : 1;

        if (isPaused)
        {
            
            if (UnderWaterSystem.isSwimming)
            {
                underWaterSys = GameObject.FindGameObjectWithTag("Head").GetComponent<UnderWaterSystem>();
                underWaterSys.AirBannerActive(false);
            }
            gameMenu.SetActive(true);
            if (fadeImage != null)
                StopCoroutine(fadeImage);
            fadeImage = FadeIn(0.75f, 0.2f, backGround, false);
            StartCoroutine(fadeImage);
        }
        else
        {
            if (UnderWaterSystem.isSwimming)
            {
                underWaterSys = GameObject.FindGameObjectWithTag("Head").GetComponent<UnderWaterSystem>();
                underWaterSys.AirBannerActive(true);
            }
            else
            {
                underWaterSys = GameObject.FindGameObjectWithTag("Head").GetComponent<UnderWaterSystem>();
                underWaterSys.AirBannerActive(false);
            }
       
            if (fadeImage != null)
                StopCoroutine(fadeImage);
            fadeImage = FadeOut(0.0f, 0.2f, backGround, false);
            StartCoroutine(fadeImage);
          
        }
    }
    public IEnumerator FadeIn(float aValue, float aTime, Image image, bool imgProperty)
    {
        float alpha = image.GetComponent<Image>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.fixedDeltaTime / aTime)
        {
            Color newColor = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(alpha, aValue, t));
            image.GetComponent<Image>().color = newColor;
            if (t >= 0.9)
            {
                if (imgProperty)
                    newColor = new Color(0, 0, 0, 1);
                else
                    newColor = new Color(1, 1, 1, 0.75f);
                image.GetComponent<Image>().color = newColor;
                mouseVisible = true;
                pauseBanner.SetActive(true);
                m_Selectable = ItemButton.GetComponent<Selectable>();
                m_Selectable.Select();
                audioSrc = GetComponent<AudioSource>();
                audioSrc.volume = 1;
                audioSrc.pitch = 1;
                audioSrc.PlayOneShot(pauseSfx);
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
            Color newColor = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(alpha, aValue, t));
            image.GetComponent<Image>().color = newColor;
            if (t >= 0.9)
            {
                if (imgProperty)
                    newColor = new Color(0, 0, 0, 0);
                else
                    newColor = new Color(1, 1, 1, 0);
                image.GetComponent<Image>().color = newColor;
                diagSys = GetComponent<DialogueSystem>();
                diagSys.saveConfirmedOptions.SetActive(false);
                Items.SetActive(false);
                gameUI.SetActive(true);
                Equipment.SetActive(false);
                Journal.SetActive(false);
                Options.SetActive(false);
                Status.SetActive(false);
                pauseBanner.SetActive(false);
                isSave = false;
                isStatus = false;
                mouseVisible = false;
                audioSrc = GetComponent<AudioSource>();
                audioSrc.volume = 1;
                audioSrc.pitch = 1;
                audioSrc.PlayOneShot(pauseSfx);
                gameMenu.SetActive(false);
            }
            else
                image.GetComponent<Image>().enabled = true;
            yield return null;
        }

    }
}
