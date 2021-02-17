using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CurShop { DuskCliff, WindAcre }
public class TownDoorSystem : MonoBehaviour
{
    public bool isShopDoor;
    public CurShop currentShop;
    Transform OutsideOfShopPos;
    Animator anim;
    public GameObject shopTrigger;
    GameObject popupCanvas;
    GameObject newInteraction;
    Text intText;
    bool interactionActive;
    public GameObject interactionObj;
    public GameObject doorStatusPrefab;
    IEnumerator screenRoutine = null;
    AudioSource audioSrc;
    public AudioClip openSfx;
    public AudioClip closeSfx;
    bool inRange;
    bool isOpen;
    bool dialogueActive;

    void Start()
    {
        SelectShop(currentShop);
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (interactionActive && newInteraction != null)
        {
            Vector3 statusPos = Camera.main.WorldToScreenPoint(interactionObj.transform.position);
            newInteraction.transform.position = statusPos;
            newInteraction.transform.localScale = new Vector3(1, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.U) && isShopDoor)
        {
          
            GameObject playerController = GameObject.FindGameObjectWithTag("Player");
            playerController.transform.position = OutsideOfShopPos.position;
            playerController.transform.rotation = OutsideOfShopPos.rotation;
        }

        if (isShopDoor)
        {
            
            if (!PauseGame.isPaused && shopTrigger.GetComponent<ShopTrigger>().EnteredShop && !TimeSystem.isShophours && !dialogueActive)
            {
                GameObject playerController = GameObject.FindGameObjectWithTag("Player");
                InteractionSystem interaction = playerController.GetComponent<InteractionSystem>();
                SceneSystem sceneSys = GameObject.Find("Player").GetComponent<SceneSystem>();
                CharacterSystem playChar = playerController.GetComponent<CharacterSystem>();
                playChar.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                sceneSys.EnablePlayer(false);
                interaction.DialogueInteraction(true, "Sorry we're closed now!");
                dialogueActive = true;
            }
            else if(!shopTrigger.GetComponent<ShopTrigger>().EnteredShop && !TimeSystem.isShophours)
                anim.SetBool("Open", false);
            else if(Input.GetButtonDown("Submit") && shopTrigger.GetComponent<ShopTrigger>().EnteredShop && !TimeSystem.isShophours && dialogueActive)
            {
                SceneSystem sceneSys = GameObject.Find("Player").GetComponent<SceneSystem>();
                sceneSys.EnablePlayer(false);
                if (screenRoutine != null)
                    StopCoroutine(screenRoutine);
                screenRoutine = FadeScreenIn(1.0f, 4.0f, sceneSys.blackScreenGame, true);
                StartCoroutine(screenRoutine);
            }
        }

        if (inRange )
        {
            if (Input.GetButtonDown("Submit") && !PauseGame.isPaused && !ShopSystem.isShop && !isOpen && !dialogueActive)
            {
                if (!isShopDoor)
                {
                    GameObject playerController = GameObject.FindGameObjectWithTag("Player");
                    SetupPopupCanvas(false, null);
                    anim.SetBool("Open", true);
                }
                else
                {
                    if (TimeSystem.isShophours)
                        anim.SetBool("Open", true);
                    else if (!TimeSystem.isShophours)
                    {
                        GameObject playerController = GameObject.FindGameObjectWithTag("Player");
                        InteractionSystem interaction = playerController.GetComponent<InteractionSystem>();
                        CharacterSystem playChar = playerController.GetComponent<CharacterSystem>();
                        playChar.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                        interaction.DialogueInteraction(true, "Store opens at 6AM.");
                        SetupPopupCanvas(false, null);
                        dialogueActive = true;
                    }
                }
            }
            else if (Input.GetButtonDown("Submit") && dialogueActive)
            {
                GameObject playerController = GameObject.FindGameObjectWithTag("Player");
                InteractionSystem interaction = playerController.GetComponent<InteractionSystem>();
                interaction.DialogueInteraction(false, null);
                dialogueActive = false;

            }
            else if (Input.GetButtonDown("Submit") && isOpen)
                anim.SetBool("Open", false);
        }
    }

    public void SetupPopupCanvas(bool active,string message)
    {
        if (active)
        {
            if (newInteraction != null)
                Destroy(newInteraction.gameObject);
            popupCanvas = GameObject.Find("Core/Player/PopUpCanvas/PlayerUIStorage");
            newInteraction = Instantiate(doorStatusPrefab, transform.position, Quaternion.identity);
            newInteraction.transform.SetParent(popupCanvas.transform);
            Transform interactionObj = newInteraction.GetComponentInChildren<Transform>().Find("InteractionText");
            intText = interactionObj.GetComponent<Text>();
            intText.enabled = true;
            intText.text = message;
            interactionActive = true;
        }
        else
        {
            if(newInteraction != null)
                Destroy(newInteraction.gameObject);
            interactionActive = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inRange = true;
            if (isOpen)
            {
               SetupPopupCanvas(true, "Close");
            }
            else
            {
                SetupPopupCanvas(true, "Open");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inRange = false;
            SetupPopupCanvas(false, null);
            InteractionSystem interaction = other.gameObject.GetComponent<InteractionSystem>();
            interaction.DialogueInteraction(false, null);
        }
    }
    public IEnumerator FadeScreenIn(float aValue, float aTime, Image image, bool imgProperty)
    {
        float alpha = image.GetComponent<Image>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.fixedDeltaTime / aTime)
        {
            Color newColor = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(alpha, aValue, t));
            image.GetComponent<Image>().color = newColor;
            if (t >= 0.9)
            {
                SceneSystem sceneSys = GameObject.Find("Player").GetComponent<SceneSystem>();
                sceneSys.EnablePlayer(false);
                if (imgProperty)
                    newColor = new Color(0, 0, 0, 1);
                else
                    newColor = new Color(1, 1, 1, 1);
                image.GetComponent<Image>().color = newColor;

                GameObject playerController = GameObject.FindGameObjectWithTag("Player");
                CharacterSystem charSys = playerController.GetComponent<CharacterSystem>();
                charSys.SetPosition();
                playerController.transform.position = OutsideOfShopPos.position;
                playerController.transform.rotation = OutsideOfShopPos.rotation;

               
                
                if (screenRoutine != null)
                    StopCoroutine(screenRoutine);
                screenRoutine = FadeScreenOut(0.0f, 4.0f, sceneSys.blackScreenGame, true);
                StartCoroutine(screenRoutine);
            }
            else
            {
                image.GetComponent<Image>().enabled = true;
            }

            yield return null;
        }

    }
    public IEnumerator FadeScreenOut(float aValue, float aTime, Image image, bool imgProperty)
    {
        float alpha = image.GetComponent<Image>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.fixedDeltaTime / aTime)
        {
            Color newColor = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(alpha, aValue, t));
            image.GetComponent<Image>().color = newColor;
            if (t >= 0.9)
            {
              
                SceneSystem sceneSys = GameObject.Find("Player").GetComponent<SceneSystem>();
                sceneSys.EnablePlayer(true);
                if (imgProperty)
                    newColor = new Color(0, 0, 0, 0);
                else
                    newColor = new Color(1, 1, 1, 0);
                image.GetComponent<Image>().color = newColor;

                shopTrigger.GetComponent<ShopTrigger>().LeftShop();
            }
            else
            {
                image.GetComponent<Image>().enabled = true;
            }
            yield return null;
        }

    }
    public void AlertObservers(string message)
    {
        if (message.Equals("DoorOpen"))
        {
            if (inRange)
                SetupPopupCanvas(true, "Close");
            else
                SetupPopupCanvas(false, null);
            isOpen = true;
        }
        else if (message.Equals("DoorClose"))
        {
            if (inRange)
                SetupPopupCanvas(true, "Open");
            else
                SetupPopupCanvas(false, null);
           
            isOpen = false;
        }

    }
    
    
    public void SelectShop(CurShop name)
    {
        switch (name)
        {
            case CurShop.DuskCliff:
                {
                    OutsideOfShopPos = GameObject.Find("Core/Positions/DuskShopPos").transform;
                    break;
                }
            case CurShop.WindAcre:
                {
                    OutsideOfShopPos = GameObject.Find("Core/Positions/WindShopPos").transform;
                    break;
                }
        }
    }
    public void PlaySound()
    {
        audioSrc = GetComponent<AudioSource>();
        if (!isOpen)
            audioSrc.PlayOneShot(openSfx);
        else
            audioSrc.PlayOneShot(closeSfx);
    }
}
