﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public enum Item
{
    Vitality, Gold5, Gold10, Gold20, Gold50, LifeBerry, Rixile, NicirPlant, MindRemedy, PetriShroom, ReliefOintment,
    TerraIdol, MagicIdol, key, DemonMask, redJewel, blueJewel, greenJewel, yellowJewel, purpleJewel, flareSword, 
    lightingSword, frostSword, terraSword, hoverBoots, cinderBoots, stormBoots, medicalBoots, helixArmor, apexArmor, 
    mythicArmor, legendaryArmor, MoonPearl,  MarkOfEtymology, IDCard, LifeChime, SpiritRich, PowerBand, Pendant, 
    SpellBind, DeathBind, Demo
}

public class ChestSystem : MonoBehaviour
{

    public Item item;
    public int currentChest;
    public int itemAmount = 1;
    Animator anim;
    bool isOpen;
    bool inTerritory;
    GameObject playerController;
    GameObject player;
    public AudioClip gotItemSfx;
    public AudioClip openSfx;
    AudioSource chestAudioSrc;
    bool isChecked = false;
    public GameObject money;
    GameObject popupCanvas;
    GameObject newInteraction;
    Text intText;
    bool interactionActive;
    public GameObject interactionObj;
    public GameObject chestStatusPrefab;
    bool close;
    bool Quit;
    bool preview;

    private void Awake()
    {
        CheckChestSystem(currentChest);
    }
    void Update()
    {
        if (interactionActive && newInteraction != null)
        {
            Vector3 statusPos = Camera.main.WorldToScreenPoint(interactionObj.transform.position);
            newInteraction.transform.position = statusPos;
            newInteraction.transform.localScale = new Vector3(1, 1, 1);
        }
        if(Input.GetButtonDown("Submit") && Quit)
        {
            InteractionSystem interaction = GameObject.FindGameObjectWithTag("Player").GetComponent<InteractionSystem>();
            interaction.DialogueInteraction(false, null);
            SceneSystem sceneSys = GameObject.Find("Core/Player").GetComponent<SceneSystem>();
            sceneSys.QuitTheGame();
            Quit = false;
        }
        else if(Input.GetButtonDown("Submit") && close)
        {
            InteractionSystem interaction = GameObject.FindGameObjectWithTag("Player").GetComponent<InteractionSystem>();
            interaction.DialogueInteraction(false, null);
            close = false;
        }
        else if (Input.GetButtonDown("Submit") && preview)
        {
            playerController = GameObject.FindGameObjectWithTag("Player");
            InteractionSystem interaction = playerController.GetComponent<InteractionSystem>();
            DialogueSystem dialogue = playerController.GetComponent<DialogueSystem>();
            dialogue.OpenItemPreview(false, DialogueSystem.ItemPreviewType.none);
            interaction.DialogueInteraction(false, null);
            preview = false;
            close = false;
        }
        if (inTerritory)
        {
            if (Input.GetButtonDown("Submit") && !isOpen && !PauseGame.isPaused && !ShopSystem.isShop && !CharacterSystem.isCarrying && !CharacterSystem.isClimbing)
            {
                isOpen = true;
                playerController = GameObject.FindGameObjectWithTag("Player");
                player = GameObject.Find("Core/Player/");
                SceneSystem sceneSys = player.GetComponent<SceneSystem>();
                CharacterSystem playChar = playerController.GetComponent<CharacterSystem>();
                chestAudioSrc = GetComponent<AudioSource>();
                chestAudioSrc.volume = 1f;
                chestAudioSrc.pitch = 1f;
                chestAudioSrc.PlayOneShot(openSfx);
                sceneSys.EnablePlayer(false);
                anim = GetComponent<Animator>();
                anim.SetBool("Open", true);
                inTerritory = false;
                playChar.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                SetupPopupCanvas(false, null);

            }
        }
        if (!isOpen && !isChecked)
            CheckChestSystem(currentChest);
    }
    public void SetupPopupCanvas(bool active, string message)
    {
        if (active)
        {
            if (newInteraction != null)
                Destroy(newInteraction.gameObject);
            popupCanvas = GameObject.Find("Core/Player/PopUpCanvas/PlayerUIStorage");
            newInteraction = Instantiate(chestStatusPrefab, transform.position, Quaternion.identity);
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isOpen)
        {
            inTerritory = true;
            SetupPopupCanvas(true, "Open");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inTerritory = false;
            InteractionSystem interaction = other.gameObject.GetComponent<InteractionSystem>();
            interaction.DialogueInteraction(false, null);
            SetupPopupCanvas(false, null);
        }
    }
    public void SelectItem(Item type)
    {
        playerController = GameObject.FindGameObjectWithTag("Player");
        PlayerSystem playSys = playerController.GetComponent<PlayerSystem>();
        InteractionSystem interaction = playerController.GetComponent<InteractionSystem>();
        DialogueSystem dialogue = playerController.GetComponent<DialogueSystem>();
        ItemSystem itemSys = playerController.GetComponent<ItemSystem>();
        JewelSystem jewelSys = playerController.GetComponent<JewelSystem>();
        SwordSystem swordSys = playerController.GetComponent<SwordSystem>();
        PlayerSystem playersys = playerController.GetComponent<PlayerSystem>();
        anim = GetComponent<Animator>();
        ObjectSystem objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
        CoreObject coreObj = GameObject.Find("Core").GetComponent<CoreObject>();

        switch (type)
        {
            case Item.Demo:
                {
                    currentChest = 0;
                    money.SetActive(false);
                    playSys.musicAudioSrc.Pause();
                    chestAudioSrc = GetComponent<AudioSource>();
                    chestAudioSrc.volume = 0.8f;
                    chestAudioSrc.pitch = 1f;
                    chestAudioSrc.PlayOneShot(gotItemSfx);
                    interaction.DialogueInteraction(true, "Thank you for Playing Treasure Quest!");
                    anim.StopPlayback();
                    jewelSys.GetJewel(JewelActive.red);
                    Quit = true;
                    break;
                }
            case Item.Gold5:
                {
                    if (PlayerSystem.playerGold >= 99999)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "Your wallet is full.");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.gold5);
                        preview = true;
                        playSys.AddGold(5);
                        interaction.DialogueInteraction(true, "You got 5 Golds.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.Gold10:
                {
                    if (PlayerSystem.playerGold >= 99999)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "Your wallet is full.");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.gold10);
                        preview = true;
                        playSys.AddGold(10);
                        interaction.DialogueInteraction(true, "You got 10 Golds.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.Gold20:
                {
                    if (PlayerSystem.playerGold >= 99999)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "Your wallet is full.");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.gold20);
                        preview = true;
                        playSys.AddGold(20);
                        interaction.DialogueInteraction(true, "You got 20 Golds.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.Gold50:
                {
                    if (PlayerSystem.playerGold >= 99999)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "Your wallet is full.");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.gold50);
                        preview = true;
                        playSys.AddGold(50);
                        interaction.DialogueInteraction(true, "You got 50 Golds.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.LifeBerry:
                {
                    if(ItemSystem.lifeBerryAmt >= 9)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "LifeBerry, but your bag is full.");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.lifeberry);
                        preview = true;
                        itemSys.GetItem(ItemSystem.Item.LifeBerry, itemAmount);
                        if(itemAmount > 1)
                            interaction.DialogueInteraction(true, "You got LifeBerry(" + itemAmount + ").");
                        else
                            interaction.DialogueInteraction(true, "You got LifeBerry.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.Rixile:
                {
                    if (ItemSystem.rixileAmt >= 9)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "Rixile, but your bag is full.");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.rixile);
                        preview = true;
                        itemSys.GetItem(ItemSystem.Item.Rixile, itemAmount);
                        if (itemAmount > 1)
                            interaction.DialogueInteraction(true, "You got Rixile(" + itemAmount + ").");
                        else
                            interaction.DialogueInteraction(true, "You got Rixile.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.NicirPlant:
                {
                    if (ItemSystem.nicirPlantAmt >= 9)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "Nicir Plant, but your bag is full.");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.nicirplant);
                        preview = true;
                        itemSys.GetItem(ItemSystem.Item.NicirPlant, itemAmount);
                        if (itemAmount > 1)
                            interaction.DialogueInteraction(true, "You got Nicir Plant(" + itemAmount + ").");
                        else
                            interaction.DialogueInteraction(true, "You got Nicir Plant.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.MindRemedy:
                {
                    if (ItemSystem.mindRemedyAmt >= 9)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "Mind Remedy, but your bag is full.");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.mindremedy);
                        preview = true;
                        itemSys.GetItem(ItemSystem.Item.MindRemedy, itemAmount);
                        if (itemAmount > 1)
                            interaction.DialogueInteraction(true, "You got Mind Remedy(" + itemAmount + ").");
                        else
                            interaction.DialogueInteraction(true, "You got Mind Remedy.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.PetriShroom:
                {
                    if (ItemSystem.petriShroomAmt >= 9)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "Petri-Shroom, but your bag is full.");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.petrishroom);
                        preview = true;
                        itemSys.GetItem(ItemSystem.Item.PetriShroom, itemAmount);
                        if (itemAmount > 1)
                            interaction.DialogueInteraction(true, "You got Petri-Shroom(" + itemAmount + ").");
                        else
                            interaction.DialogueInteraction(true, "You got Petri-Shroom.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.ReliefOintment:
                {
                    if (ItemSystem.reliefOintmentAmt >= 9)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "Relief Ointment, but your bag is full.");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.reliefointment);
                        preview = true;
                        itemSys.GetItem(ItemSystem.Item.ReliefOintment, itemAmount);
                        if (itemAmount > 1)
                            interaction.DialogueInteraction(true, "You got Relief Ointment(" + itemAmount + ").");
                        else
                            interaction.DialogueInteraction(true, "You got Relief Ointment.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.TerraIdol:
                {
                    if (ItemSystem.terraIdolAmt >= 9)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "Terra Idol, but your bag is full.");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.terraidol);
                        preview = true;
                        itemSys.GetItem(ItemSystem.Item.TerraIdol, itemAmount);
                        if (itemAmount > 1)
                            interaction.DialogueInteraction(true, "You got Terra Idol(" + itemAmount + ").");
                        else
                            interaction.DialogueInteraction(true, "You got Terra Idol.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.MagicIdol:
                {
                    if (ItemSystem.magicIdolAmt >= 9)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "Magic Idol, but your bag is full.");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.magicidol);
                        preview = true;
                        itemSys.GetItem(ItemSystem.Item.MagicIdol, itemAmount);
                        if (itemAmount > 1)
                            interaction.DialogueInteraction(true, "You got Magic Idol(" + itemAmount + ").");
                        else
                            interaction.DialogueInteraction(true, "You got Magic Idol.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.key:
                {
                    if (ItemSystem.keyAmt >= 9)
                    {
                        interaction.DialogueInteraction(true, "Key, but your bag is full.");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        isOpen = false;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.key);
                        preview = true;
                        itemSys.GetItem(ItemSystem.Item.key, itemAmount);
                        if (itemAmount > 1)
                            interaction.DialogueInteraction(true, "You got Key(" + itemAmount + ").");
                        else
                            interaction.DialogueInteraction(true, "You got Key.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.DemonMask:
                {
                    if (ItemSystem.demonMaskPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Demon Mask.");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.demonmask);
                        preview = true;
                        itemSys.GetItem(ItemSystem.Item.DemonMask, 0);
                        interaction.DialogueInteraction(true, "You got Demon Mask.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.MoonPearl:
                {
                    if (ItemSystem.moonPearlPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Moon Pearl.");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.moonpearl);
                        preview = true;
                        itemSys.GetItem(ItemSystem.Item.MoonPearl, 0);
                        interaction.DialogueInteraction(true, "You got Moon Pearl.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.MarkOfEtymology:
                {
                    if (ItemSystem.markOfEtymologyPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Mark of Etymology.");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.markofetymology);
                        preview = true;
                        itemSys.GetItem(ItemSystem.Item.MarkofEtymology, 0);
                        interaction.DialogueInteraction(true, "You got Mark of Etymology.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.Vitality:
                {
                    if (PlayerSystem.playerVitality >= 100)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "Vitality Essence is maxed out");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.vitalityEssence);
                        preview = true;
                        playSys.AddVitality(1);
                        interaction.DialogueInteraction(true, "You got Vitality Essence.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.redJewel:
                {
                    if (JewelSystem.redJewelPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Jewel of Power");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.powerjewel);
                        preview = true;
                        jewelSys.GetJewel(JewelActive.red);
                        interaction.DialogueInteraction(true, "You got Jewel of Power.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.blueJewel:
                {
                    if (JewelSystem.blueJewelPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Jewel of Spirit");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.spiritjewel);
                        preview = true;
                        jewelSys.GetJewel(JewelActive.blue);
                        interaction.DialogueInteraction(true, "You got Jewel of Spirit.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.greenJewel:
                {
                    if (JewelSystem.greenJewelPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Jewel of Time");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.timejewel);
                        preview = true;
                        jewelSys.GetJewel(JewelActive.green);
                        interaction.DialogueInteraction(true, "You got Jewel of Time.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.yellowJewel:
                {
                    if (JewelSystem.yellowJewelPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Jewel of Light");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.lightjewel);
                        preview = true;
                        jewelSys.GetJewel(JewelActive.yellow);
                        interaction.DialogueInteraction(true, "You got Jewel of Light.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.purpleJewel:
                {
                    if (JewelSystem.purpleJewelPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Jewel of Dark");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.darkjewel);
                        preview = true;
                        jewelSys.GetJewel(JewelActive.purple);
                        interaction.DialogueInteraction(true, "You got Jewel of Dark.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.flareSword:
                {
                    if (SwordSystem.flareSwordPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Flare Sword");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.flaresword);
                        preview = true;
                        swordSys.GetSword(SwordActive.flare);
                        interaction.DialogueInteraction(true, "You got Flare Sword.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.lightingSword:
                {
                    if (SwordSystem.lightningSwordPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Lightning Sword");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.lightningsword);
                        preview = true;
                        swordSys.GetSword(SwordActive.lightning);
                        interaction.DialogueInteraction(true, "You got Lightning Sword.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.frostSword:
                {
                    if (SwordSystem.frostSwordPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Frost Sword");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.frostsword);
                        preview = true;
                        swordSys.GetSword(SwordActive.frost);
                        interaction.DialogueInteraction(true, "You got Frost Sword.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.terraSword:
                {
                    if (SwordSystem.terraSwordPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Terra Sword");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.terrasword);
                        preview = true;
                        swordSys.GetSword(SwordActive.terra);
                        interaction.DialogueInteraction(true, "You got Terra Sword.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.helixArmor:
                {
                    if (PlayerSystem.helixArmorPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Helix Armor");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.helixarmor);
                        preview = true;
                        playersys.GetArmor(ArmorActive.helix);
                        interaction.DialogueInteraction(true, "You got Helix Armor.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.apexArmor:
                {
                    if (PlayerSystem.apexArmorPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Apex Armor");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.apexarmor);
                        preview = true;
                        playersys.GetArmor(ArmorActive.apex);
                        interaction.DialogueInteraction(true, "You got Apex Armor.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.mythicArmor:
                {
                    if (PlayerSystem.mythicArmorPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Mythic Armor");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.mythicarmor);
                        preview = true;
                        playersys.GetArmor(ArmorActive.mythic);
                        interaction.DialogueInteraction(true, "You got Mythic Armor.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.legendaryArmor:
                {
                    if (PlayerSystem.legendaryArmorPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Legendary Armor");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.legendaryarmor);
                        preview = true;
                        playersys.GetArmor(ArmorActive.legendary);
                        interaction.DialogueInteraction(true, "You got Legendary Armor.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.hoverBoots:
                {
                    if (PlayerSystem.hoverBootPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Hovers");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.hovers);
                        preview = true;
                        playersys.GetBoots(BootActive.hover);
                        interaction.DialogueInteraction(true, "You got Hovers.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.cinderBoots:
                {
                    if (PlayerSystem.cinderBootPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Cinders");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.cinders);
                        preview = true;
                        playersys.GetBoots(BootActive.cinder);
                        interaction.DialogueInteraction(true, "You got Cinders.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.stormBoots:
                {
                    if (PlayerSystem.stormBootPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Storms");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.storms);
                        preview = true;
                        playersys.GetBoots(BootActive.storm);
                        interaction.DialogueInteraction(true, "You got Storms.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
            case Item.medicalBoots:
                {
                    if (PlayerSystem.medicalBootPickedUp)
                    {
                        isOpen = false;
                        interaction.DialogueInteraction(true, "You already have Medicals");
                        anim = GetComponent<Animator>();
                        anim.SetBool("Open", false);
                        close = true;
                        return;
                    }
                    else
                    {
                        money.SetActive(false);
                        playSys.musicAudioSrc.Pause();
                        chestAudioSrc = GetComponent<AudioSource>();
                        chestAudioSrc.volume = 0.8f;
                        chestAudioSrc.pitch = 1f;
                        chestAudioSrc.PlayOneShot(gotItemSfx);
                        dialogue.OpenItemPreview(true, DialogueSystem.ItemPreviewType.medicals);
                        preview = true;
                        playersys.GetBoots(BootActive.medical);
                        interaction.DialogueInteraction(true, "You got Medicals.");
                        anim.StopPlayback();
                        objectSystem.SetChestActive(currentChest);
                        coreObj.SetGameProgress(0.2f);
                    }
                    break;
                }
             

        }
      
    }
    public void InstantiateItem()
    {
        if(!isChecked)
            SelectItem(item);
    }
    public void OpenChest()
    {
        anim.SetBool("Open", true);
        isOpen = true;
        isChecked = true;
        money.SetActive(false);
    }
    public void CheckChestSystem(int chest)
    {
        anim = GetComponent<Animator>();
        if (!isChecked && !isOpen)
        {
            if (chest == 1)
            {
                if (ObjectSystem.chest1)
                    OpenChest();
            }         
            else if (chest == 2)
            {
                if (ObjectSystem.chest2)
                    OpenChest();
            }
            else if(chest == 3)
            {
                if (ObjectSystem.chest3)
                    OpenChest();
            }
            else if(chest == 4)
            {
                if (ObjectSystem.chest4)
                    OpenChest();
            }
            else if(chest == 5)
            {
                if (ObjectSystem.chest5)
                    OpenChest();
            }
            else if(chest == 6)
            {
                if (ObjectSystem.chest6)
                    OpenChest();
            }
            else if(chest == 7)
            {
                if (ObjectSystem.chest7)
                    OpenChest();
            }
            else if(chest == 8)
            {
                if (ObjectSystem.chest8)
                    OpenChest();
            }
            else if (chest == 9)
            {
                if (ObjectSystem.chest9)
                    OpenChest();
            }
            else if (chest == 10)
            {
                if (ObjectSystem.chest10)
                    OpenChest();
            }
            else if (chest == 11)
            {
                if (ObjectSystem.chest11)
                    OpenChest();
            }
            else if (chest == 12)
            {
                if (ObjectSystem.chest12)
                    OpenChest();
            }
            else if (chest == 13)
            {
                if (ObjectSystem.chest13)
                    OpenChest();
            }
            else if (chest == 14)
            {
                if (ObjectSystem.chest14)
                    OpenChest();
            }
            else if (chest == 15)
            {
                if (ObjectSystem.chest15)
                    OpenChest();
            }
            else if (chest == 16)
            {
                if (ObjectSystem.chest16)
                    OpenChest();
            }
            else if (chest == 17)
            {
                if (ObjectSystem.chest17)
                    OpenChest();
            }
            else if (chest == 18)
            {
                if (ObjectSystem.chest18)
                    OpenChest();
            }
            else if (chest == 19)
            {
                if (ObjectSystem.chest19)
                    OpenChest();
            }
            else if (chest == 20)
            {
                if (ObjectSystem.chest20)
                    OpenChest();
            }
            else if (chest == 21)
            {
                if (ObjectSystem.chest21)
                    OpenChest();
            }
            else if (chest == 22)
            {
                if (ObjectSystem.chest22)
                    OpenChest();
            }
            else if (chest == 23)
            {
                if (ObjectSystem.chest23)
                    OpenChest();
            }
            else if (chest == 24)
            {
                if (ObjectSystem.chest24)
                    OpenChest();
            }
            else if (chest == 25)
            {
                if (ObjectSystem.chest25)
                    OpenChest();
            }
            else if (chest == 26)
            {
                if (ObjectSystem.chest26)
                    OpenChest();
            }
            else if (chest == 27)
            {
                if (ObjectSystem.chest27)
                    OpenChest();
            }
            else if (chest == 28)
            {
                if (ObjectSystem.chest28)
                    OpenChest();
            }
            else if (chest == 29)
            {
                if (ObjectSystem.chest29)
                    OpenChest();
            }
            else if (chest == 30)
            {
                if (ObjectSystem.chest30)
                    OpenChest();
            }
            else if (chest == 31)
            {
                if (ObjectSystem.chest31)
                    OpenChest();
            }
            else if (chest == 32)
            {
                if (ObjectSystem.chest32)
                    OpenChest();
            }
            else if (chest == 33)
            {
                if (ObjectSystem.chest33)
                    OpenChest();
            }
            else if (chest == 34)
            {
                if (ObjectSystem.chest34)
                    OpenChest();
            }
            else if (chest == 35)
            {
                if (ObjectSystem.chest35)
                    OpenChest();
            }
            else if (chest == 36)
            {
                if (ObjectSystem.chest36)
                    OpenChest();
            }
            else if (chest == 37)
            {
                if (ObjectSystem.chest37)
                    OpenChest();
            }
            else if (chest == 38)
            {
                if (ObjectSystem.chest38)
                    OpenChest();
            }
            else if (chest == 39)
            {
                if (ObjectSystem.chest39)
                    OpenChest();
            }
            else if (chest == 40)
            {
                if (ObjectSystem.chest40)
                    OpenChest();
            }
            else if (chest == 41)
            {
                if (ObjectSystem.chest41)
                    OpenChest();
            }
            else if (chest == 42)
            {
                if (ObjectSystem.chest42)
                    OpenChest();
            }
            else if (chest == 43)
            {
                if (ObjectSystem.chest43)
                    OpenChest();
            }
            else if (chest == 44)
            {
                if (ObjectSystem.chest44)
                    OpenChest();
            }
            else if (chest == 45)
            {
                if (ObjectSystem.chest45)
                    OpenChest();
            }
            else if (chest == 46)
            {
                if (ObjectSystem.chest46)
                    OpenChest();
            }
            else if (chest == 47)
            {
                if (ObjectSystem.chest47)
                    OpenChest();
            }
            else if (chest == 48)
            {
                if (ObjectSystem.chest48)
                    OpenChest();
            }
            else if (chest == 49)
            {
                if (ObjectSystem.chest49)
                    OpenChest();
            }
            else if (chest == 50)
            {
                if (ObjectSystem.chest50)
                    OpenChest();
            }
            else if (chest == 51)
            {
                if (ObjectSystem.chest51)
                    OpenChest();
            }
            else if (chest == 52)
            {
                if (ObjectSystem.chest52)
                    OpenChest();
            }
            else if (chest == 53)
            {
                if (ObjectSystem.chest53)
                    OpenChest();
            }
            else if (chest == 54)
            {
                if (ObjectSystem.chest54)
                    OpenChest();
            }
            else if (chest == 55)
            {
                if (ObjectSystem.chest55)
                    OpenChest();
            }
            else if (chest == 56)
            {
                if (ObjectSystem.chest56)
                    OpenChest();
            }
            else if (chest == 57)
            {
                if (ObjectSystem.chest57)
                    OpenChest();
            }
            else if (chest == 58)
            {
                if (ObjectSystem.chest58)
                    OpenChest();
            }
            else if (chest == 59)
            {
                if (ObjectSystem.chest59)
                    OpenChest();
            }
            else if (chest == 60)
            {
                if (ObjectSystem.chest60)
                    OpenChest();
            }
            else if (chest == 61)
            {
                if (ObjectSystem.chest61)
                    OpenChest();
            }
            else if (chest == 62)
            {
                if (ObjectSystem.chest62)
                    OpenChest();
            }
        }
    }
}
