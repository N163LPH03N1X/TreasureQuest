using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CarePackage : MonoBehaviour
{
    OptSystem optSystem = new OptSystem();
    AudioSource audioSrc;
    public AudioClip pickupSfx;
    public GetItem getItem;
    bool inRange = false;
    ItemSystem itemSys;
    GameObject popupCanvas;
    GameObject newInteraction;
    Text intText;
    bool interactionActive;
    public GameObject interactionObj;
    public GameObject packageStatusPrefab;
    GameObject playerController;
    bool dialogueActive = true;
    string message;
    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (interactionActive && newInteraction != null)
        {
            Vector3 statusPos = Camera.main.WorldToScreenPoint(interactionObj.transform.position);
            newInteraction.transform.position = statusPos;
            newInteraction.transform.localScale = new Vector3(1, 1, 1);
        }
        if (inRange)
        {
            if(optSystem.Input.GetButtonDown("Submit") && !PauseGame.isPaused && !ShopSystem.isShop && dialogueActive && !CharacterSystem.isCarrying && !CharacterSystem.isJumping)
            {
                playerController = GameObject.FindGameObjectWithTag("Player");
                InteractionSystem interaction = playerController.GetComponent<InteractionSystem>();
                dialogueActive = false;
                GiveItem(getItem);
                audioSrc.PlayOneShot(pickupSfx);
                interaction.DialogueInteraction(true, "You Found " + message);
                SetupPopupCanvas(false, null);
            }
            else if (optSystem.Input.GetButtonDown("Submit") && !PauseGame.isPaused && !ShopSystem.isShop && !dialogueActive && !CharacterSystem.isCarrying && !CharacterSystem.isJumping)
            {
                playerController = GameObject.FindGameObjectWithTag("Player");
                InteractionSystem interaction = playerController.GetComponent<InteractionSystem>();
                SetupPopupCanvas(false, null);
                interaction.DialogueInteraction(false, null);
                CharacterSystem charSys = playerController.GetComponent<CharacterSystem>();
                charSys.PlayerInteraction(false);
                Destroy(gameObject);

            }
        }
    }
    public void SetupPopupCanvas(bool active, string message)
    {
        if (active)
        {
            if (newInteraction != null)
                Destroy(newInteraction.gameObject);
            popupCanvas = GameObject.Find("Core/Player/PopUpCanvas/PlayerUIStorage");
            newInteraction = Instantiate(packageStatusPrefab, transform.position, Quaternion.identity);
            newInteraction.transform.SetParent(popupCanvas.transform);
            Transform interactionObj = newInteraction.GetComponentInChildren<Transform>().Find("InteractionText");
            intText = interactionObj.GetComponent<Text>();
            intText.enabled = true;
            intText.text = message;
            interactionActive = true;
        }
        else
        {
            if (newInteraction != null)
                Destroy(newInteraction.gameObject);
            interactionActive = false;
        }
    }
    public enum GetItem { lifeberry, rixile, nicirplant, mindremedy, petrishroom, reliefointment}
    void GiveItem(GetItem item)
    {
        itemSys = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemSystem>();
        switch (item)
        {
            case GetItem.lifeberry:
                {
                    message = "LifeBerry";
                    itemSys.GetItem(ItemSystem.Item.LifeBerry, 1);
                    break;
                }
            case GetItem.rixile:
                {
                    message = "Rixile";
                    itemSys.GetItem(ItemSystem.Item.Rixile, 1);
                    break;
                }
            case GetItem.nicirplant:
                {
                    message = "Nicir Plant";
                    itemSys.GetItem(ItemSystem.Item.NicirPlant, 1);
                    break;
                }
            case GetItem.mindremedy:
                {
                    message = "Mind Remedy";
                    itemSys.GetItem(ItemSystem.Item.MindRemedy, 1);
                    break;
                }
            case GetItem.petrishroom:
                {
                    message = "Petri-Shroom";
                    itemSys.GetItem(ItemSystem.Item.PetriShroom, 1);
                    break;
                }
            case GetItem.reliefointment:
                {
                    message = "Relief Ointment";
                    itemSys.GetItem(ItemSystem.Item.ReliefOintment, 1);
                    break;
                }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CharacterSystem charSys = other.gameObject.GetComponent<CharacterSystem>();
            charSys.PlayerInteraction(true);
            SetupPopupCanvas(true, "Take");
            inRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
         
            CharacterSystem charSys = other.gameObject.GetComponent<CharacterSystem>();
            charSys.PlayerInteraction(false);
            SetupPopupCanvas(false, null);
            inRange = false;
        }
    }
}
