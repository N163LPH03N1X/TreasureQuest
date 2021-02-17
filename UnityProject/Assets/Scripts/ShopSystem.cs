using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum Shop { DuskCliff, WindAcre, SkulkCove}

public class ShopSystem : MonoBehaviour
{
    private Selectable m_Selectable;
    public Sprites sprites;
    bool currentGold;
    PlayerSystem playSys;
    ItemSystem itemSys;
    DialogueSystem diagSys;
    InteractionSystem interaction;

    public Text shopName;

    public Text specialItemName;
    public Text specialPromotion;
    public Image specialIcon;

    public Text[] onSaleDisplays;
    public Text[] itemsInStock;
    public Image[] slotIcons;
    public Text[] itemNames;
    public Text[] itemDiscriptions;
    public Text[] itemPrices;
    public Button[] purchaseButtons;
    public GameObject[] inventorySlots;

    public static bool vitality1Bought = false;
    public static bool vitality2Bought = false;
    public static bool vitality3Bought = false;

    public Text confirmText;
    string confirmMessage;
    int confirmCounter = 0;
    IEnumerator animateConfirm;
    bool resetConfirmText = false;

    public GameObject ShopMenu;
    public GameObject shopWindow;
    public GameObject shopBanner;
    public Image backGround;

    AudioSource audioSrc;
    public AudioClip pauseSfx;
    public AudioClip purchaseSfx;
    IEnumerator fadeImage = null;
    public bool mouseVisible = false;
    public Button browseButton;
    public Button leaveButton;
    public bool backButton;
    public bool isBrowsing;
    public static bool isShop;
    PeopleSystem peopleSys;

    public Items[] curItem = new Items[6];
    public int[] curPrice;
    UnityEvent eventHandler = new UnityEvent();
    UnityAction[] actions = new UnityAction[6];

    bool dialogueActive = false;
    bool finishedDialogue = false;
    IEnumerator screenRoutine = null;
    
    public static bool isSleeping;

    Transform besideBedPos;
    void Start()
    {
        interaction = GetComponent<InteractionSystem>();
        for (int AL = 0; AL <= purchaseButtons.Length - 1; AL++)
        {
            SetUpButtonListeners(AL);
        }
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            mouseVisible = !mouseVisible;
        if (isSleeping)
            StayAtTheInn();
       

        if (isShop)
        { 
            if (Input.GetButtonDown("Cancel") && !backButton)
            {
                BackOut();
                backButton = true;
            }
            else if (Input.GetButtonUp("Cancel"))
                backButton = false;
        }
        if(!PauseGame.isPaused && !DebugSystem.toggleCodeList)
            EnableMouse(mouseVisible);
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
    public void SetUpButtonListeners(int slotNum)
    {
        eventHandler = purchaseButtons[slotNum].onClick;
        eventHandler.AddListener(delegate { ButtonListener(slotNum); });
    }
    public void ButtonListener(int slotNum)
    {
        PurchaseItem(curItem[slotNum], slotNum, curPrice[slotNum]);
    }
    public enum Items { Vitality1, Vitality2, Vitality3, LifeBerry, Rixile, NicirPlant, MindRemedy, PetriShroom, ReliefOintment, TerraIdol, MagicIdol, InnTicket }
    public void SelectItem(Items type, int slotNum, int price, int discount, bool specialItem, bool onSale, int stock)
    {
     
        if (onSale)
        {
            price = price - discount;
            onSaleDisplays[slotNum].text = "(On Sale!)";
            onSaleDisplays[slotNum].color = Color.yellow;
            itemPrices[slotNum].text = price + "G".ToString();
            itemPrices[slotNum].color = Color.yellow;
        }
        else
        {
            onSaleDisplays[slotNum].text = null;
            itemPrices[slotNum].color = Color.white;
            itemPrices[slotNum].text = price + "G".ToString();
        }
        switch (type)
        {
            case Items.Vitality1:
                {
                    curItem[slotNum] = Items.Vitality1;
                    curPrice[slotNum] = price;
                    if (specialItem)
                    {
                        specialPromotion.text = "Special item in stock!";
                        specialItemName.text = "[Vitality Essence]";
                        specialIcon.sprite = sprites.vitalitySprite;
                    }
                    slotIcons[slotNum].sprite = sprites.vitalitySprite;
                    itemNames[slotNum].text = "[Vitality Essence]";
                    itemDiscriptions[slotNum].text = "A special reviving essence that permanently boosts the amount of hit points by 1.";
                    SetStockValue(slotNum, stock);
                
                    break;
                }
            case Items.Vitality2:
                {
                    curItem[slotNum] = Items.Vitality2;
                    curPrice[slotNum] = price;
                    if (specialItem)
                    {
                        specialPromotion.text = "Special item in stock!";
                        specialItemName.text = "[Vitality Essence]";
                        specialIcon.sprite = sprites.vitalitySprite;
                    }
                    slotIcons[slotNum].sprite = sprites.vitalitySprite;
                    itemNames[slotNum].text = "[Vitality Essence]";
                    itemDiscriptions[slotNum].text = "A special reviving essence that permanently boosts the amount of hit points by 1.";
                    SetStockValue(slotNum, stock);

                    break;
                }
            case Items.Vitality3:
                {
                    curItem[slotNum] = Items.Vitality3;
                    curPrice[slotNum] = price;
                    if (specialItem)
                    {
                        specialPromotion.text = "Special item in stock!";
                        specialItemName.text = "[Vitality Essence]";
                        specialIcon.sprite = sprites.vitalitySprite;
                    }
                    slotIcons[slotNum].sprite = sprites.vitalitySprite;
                    itemNames[slotNum].text = "[Vitality Essence]";
                    itemDiscriptions[slotNum].text = "A special reviving essence that permanently boosts the amount of hit points by 1.";
                    SetStockValue(slotNum, stock);

                    break;
                }
            case Items.LifeBerry:
                {
                    curItem[slotNum] = Items.LifeBerry;
                    curPrice[slotNum] = price;
                    if (specialItem)
                    {
                        specialPromotion.text = "Sale of the Week!";
                        specialItemName.text = "[Life Berry]";
                        specialIcon.sprite = sprites.lifeBerrySprite;
                    }
                    slotIcons[slotNum].sprite = sprites.lifeBerrySprite;
                    itemNames[slotNum].text = "[Life Berry]";
                    itemDiscriptions[slotNum].text = "Magical berries that can be mixed together to make a life reviving potion, revives and heals half of the players health.";
                    SetStockValue(slotNum, stock);
                   
                    break;
                }
            case Items.Rixile:
                {
                    curItem[slotNum] = Items.Rixile;
                    curPrice[slotNum] = price;
                    if (specialItem)
                    {
                        specialPromotion.text = "Sale of the Week!";
                        specialItemName.text = "[Rixile]";
                        specialIcon.sprite = sprites.rixileSprite;
                    }
                    slotIcons[slotNum].sprite = sprites.rixileSprite;
                    itemNames[slotNum].text = "[Rixile]";
                    itemDiscriptions[slotNum].text = "Brewed with the finest life berries and medicine. This strong potion fills players health.";
                    SetStockValue(slotNum, stock);

                    break;
                }
            case Items.NicirPlant:
                {
                    curItem[slotNum] = Items.NicirPlant;
                    curPrice[slotNum] = price;
                    if (specialItem)
                    {
                        specialPromotion.text = "Sale of the Week!";
                        specialItemName.text = "[Nicir Plant]";
                        specialIcon.sprite = sprites.nicirPlantSprite;
                    }
                    slotIcons[slotNum].sprite = sprites.nicirPlantSprite;
                    itemNames[slotNum].text = "[Nicir Plant]";
                    itemDiscriptions[slotNum].text = "One of the Most Poisonous plants found in the wild. It can be brewed to make a powerful antidote.";
                    SetStockValue(slotNum, stock);

                    break;
                }
            case Items.MindRemedy:
                {
                    curItem[slotNum] = Items.MindRemedy;
                    curPrice[slotNum] = price;
                    if (specialItem)
                    {
                        specialPromotion.text = "Sale of the Week!";
                        specialItemName.text = "[Mind Remedy]";
                        specialIcon.sprite = sprites.mindRemedySprite;
                    }
                    slotIcons[slotNum].sprite = sprites.mindRemedySprite;
                    itemNames[slotNum].text = "[Mind Remedy]";
                    itemDiscriptions[slotNum].text = "A Mysterious Potion that repairs a twixed mind. This item removes confusion.";
                    SetStockValue(slotNum, stock);
                    break;
                }
            case Items.PetriShroom:
                {
                    curItem[slotNum] = Items.PetriShroom;
                    curPrice[slotNum] = price;
                    if (specialItem)
                    {
                        specialPromotion.text = "Sale of the Week!";
                        specialItemName.text = "[Petri-Shroom]";
                        specialIcon.sprite = sprites.PetriShroomSprite;
                    }
                    slotIcons[slotNum].sprite = sprites.PetriShroomSprite;
                    itemNames[slotNum].text = "[Petri-Shroom]";
                    itemDiscriptions[slotNum].text = "A rare mushroom picked in a damp withered forest and mashed into a medicine. It has the power to cure paralysis.";
                    SetStockValue(slotNum, stock);
                    break;
                }
            case Items.ReliefOintment:
                {
                    curItem[slotNum] = Items.ReliefOintment;
                    curPrice[slotNum] = price;
                    if (specialItem)
                    {
                        specialPromotion.text = "Sale of the Week!";
                        specialItemName.text = "[Relief Ointment]";
                        specialIcon.sprite = sprites.reliefOintmentSprite;
                    }
                    slotIcons[slotNum].sprite = sprites.reliefOintmentSprite;
                    itemNames[slotNum].text = "[Relief Ointment]";
                    itemDiscriptions[slotNum].text = "A special medicinal ointment that relieves muscle stigma. Using this item removes silence.";
                    SetStockValue(slotNum, stock);
                    break;
                }
            case Items.TerraIdol:
                {
                    curItem[slotNum] = Items.TerraIdol;
                    curPrice[slotNum] = price;
                    if (specialItem)
                    {
                        specialPromotion.text = "Sale of the Week!";
                        specialItemName.text = "[Terra Idol]";
                        specialIcon.sprite = sprites.terraIdolSprite;
                    }
                    slotIcons[slotNum].sprite = sprites.terraIdolSprite;
                    itemNames[slotNum].text = "[Terra Idol]";
                    itemDiscriptions[slotNum].text = "A spiritual tablet said to have been crafted by a terraformed goddess, believe to have caused catastrophic earth quakes.";
                    SetStockValue(slotNum, stock);
                    break;
                }
            case Items.MagicIdol:
                {
                    curItem[slotNum] = Items.MagicIdol;
                    curPrice[slotNum] = price;
                    if (specialItem)
                    {
                        specialPromotion.text = "Sale of the Week!";
                        specialItemName.text = "[Magic Idol]";
                        specialIcon.sprite = sprites.magicIdolSprite;
                    }
                    slotIcons[slotNum].sprite = sprites.magicIdolSprite;
                    itemNames[slotNum].text = "[Magic Idol]";
                    itemDiscriptions[slotNum].text = "A magical tablet crafted by a mystical black smith, believed to give its user outstanding sword abilites.";
                    SetStockValue(slotNum, stock);
                    break;
                }
            case Items.InnTicket:
                {
                    curItem[slotNum] = Items.InnTicket;
                    curPrice[slotNum] = price;
                    if (specialItem)
                    {
                        specialPromotion.text = "Sale of the Week!";
                        specialItemName.text = "[Inn Ticket]";
                        specialIcon.sprite = sprites.innTicketSprite;
                    }
                    slotIcons[slotNum].sprite = sprites.innTicketSprite;
                    itemNames[slotNum].text = "[Inn Ticket]";
                    itemDiscriptions[slotNum].text = "Purchase a ticket to stay one night in the inn. Passes one day and replenishes health to full.";
                    itemsInStock[slotNum].text = null;
                    break;
                }
        }
        actions[slotNum] = new UnityAction(() => PurchaseItem(curItem[slotNum], slotNum, curPrice[slotNum]));
    }
    public void SelectShop(Shop name, int totalNumSlots)
    {
        switch (name)
        {
            case Shop.DuskCliff:
                {

                    foreach (GameObject obj in inventorySlots)
                        obj.SetActive(true);
                    for (int i = inventorySlots.Length - 1; i >= totalNumSlots; i--)
                        inventorySlots[i].SetActive(false);

                    besideBedPos = GameObject.Find("Core/Positions/DuskSleepPos").transform;

                    shopName.text = "Dusk Cliff Shop";
                    if (vitality1Bought)
                    {
                        SelectItem(Items.Vitality1, 0, 150, 0, false, false, 0);
                        SelectItem(Items.InnTicket, 1, 20, 0, false, false, 0);
                        SelectItem(Items.LifeBerry, 2, 15, 4, true, true, 9 - ItemSystem.lifeBerryAmt);
                        SelectItem(Items.NicirPlant, 3, 20, 0, false, false, 9 - ItemSystem.nicirPlantAmt);
                    }
                    else
                    {
                        SelectItem(Items.Vitality1, 0, 150, 0, true, false, 1);
                        SelectItem(Items.InnTicket, 1, 20, 0, false, false, 0);
                        SelectItem(Items.LifeBerry, 2, 15, 4, false, true, 9 - ItemSystem.lifeBerryAmt);
                        SelectItem(Items.NicirPlant, 3, 20, 0, false, false, 9 - ItemSystem.nicirPlantAmt);
                    }

                    break;
                }
            case Shop.WindAcre:
                {
                    foreach (GameObject obj in inventorySlots)
                        obj.SetActive(true);
                    for (int i = inventorySlots.Length - 1; i >= totalNumSlots; i--)
                        inventorySlots[i].SetActive(false);

                    besideBedPos = GameObject.Find("Core/Positions/WindSleepPos").transform;

                    shopName.text = "Wind Acre Shop";
                    if (vitality2Bought)
                    {
                        SelectItem(Items.Vitality2, 0, 300, 0, false, false, 0);
                        SelectItem(Items.InnTicket, 1, 20, 0, false, false, 0);
                        SelectItem(Items.LifeBerry, 2, 15, 0, false, false, 9 - ItemSystem.lifeBerryAmt);
                        SelectItem(Items.NicirPlant, 3, 20, 5, true, true, 9 - ItemSystem.nicirPlantAmt);
                    }
                    else
                    {
                        SelectItem(Items.Vitality2, 0, 300, 0, true, false, 1);
                        SelectItem(Items.InnTicket, 1, 20, 0, false, false, 0);
                        SelectItem(Items.LifeBerry, 2, 15, 0, false, false, 9 - ItemSystem.lifeBerryAmt);
                        SelectItem(Items.NicirPlant, 3, 20, 5, false, true, 9 - ItemSystem.nicirPlantAmt);
                    }
                    break;
                }
            case Shop.SkulkCove:
                {
                    foreach (GameObject obj in inventorySlots)
                        obj.SetActive(true);
                    for (int i = inventorySlots.Length - 1; i >= totalNumSlots; i--)
                        inventorySlots[i].SetActive(false);

                    //besideBedPos = GameObject.Find("Core/Positions/SkulkSleepPos").transform;

                    shopName.text = "Skulk Cove Shop";
                    if (vitality3Bought)
                    {
                        SelectItem(Items.Vitality3, 0, 350, 0, false, false, 0);
                        SelectItem(Items.InnTicket, 1, 20, 0, false, false, 0);
                        SelectItem(Items.LifeBerry, 2, 15, 0, false, false, 9 - ItemSystem.lifeBerryAmt);
                        SelectItem(Items.NicirPlant, 3, 20, 0, false, false, 9 - ItemSystem.nicirPlantAmt);
                        SelectItem(Items.TerraIdol, 4, 50, 0, false, false, 9 - ItemSystem.terraIdolAmt);
                        SelectItem(Items.MagicIdol, 5, 50, 5, true, true, 9 - ItemSystem.magicIdolAmt);
                    }
                    else
                    {
                        SelectItem(Items.Vitality3, 0, 350, 0, true, false, 1);
                        SelectItem(Items.InnTicket, 1, 20, 0, false, false, 0);
                        SelectItem(Items.LifeBerry, 2, 15, 0, false, false, 9 - ItemSystem.lifeBerryAmt);
                        SelectItem(Items.NicirPlant, 3, 20, 0, false, false, 9 - ItemSystem.nicirPlantAmt);
                        SelectItem(Items.TerraIdol, 4, 50, 0, false, false, 9 - ItemSystem.terraIdolAmt);
                        SelectItem(Items.MagicIdol, 5, 50, 5, false, true, 9 - ItemSystem.magicIdolAmt);
                    }
                    break;
                }
        }
    }
    public void SetStockValue(int slotNum, int stock)
    {
        if (stock == 0)
        {
            itemsInStock[slotNum].text = "(Out of stock!)";
            itemsInStock[slotNum].color = Color.red;
        }
        else if (stock == 1)
        {
            itemsInStock[slotNum].text = "(Last one in stock!)";
            itemsInStock[slotNum].color = Color.red;
        }
        else
        {
            itemsInStock[slotNum].text = "Stock " + stock.ToString();
            itemsInStock[slotNum].color = Color.white;
        }
    }
    public void PurchaseItem(Items item, int slotNum, int price)
    {
        playSys = GetComponent<PlayerSystem>();
        itemSys = GetComponent<ItemSystem>();
        confirmText.text = "";
        confirmMessage = confirmText.text;
        confirmCounter += 1;
        if (confirmCounter > 2)
        {
            confirmCounter = 1;
        }

        switch (item)
        {
            case Items.Vitality1:
                {
                    if (confirmCounter == 1)
                        confirmMessage = " Would you like to purchase Vitality Essence?";
                    else if (confirmCounter == 2)
                    {
                        int orgGold = PlayerSystem.playerGold;
                        PlayerSystem.playerGold -= price;
                        if (PlayerSystem.playerGold < 0)
                        {
                            PlayerSystem.playerGold = orgGold;
                            if (!vitality1Bought)
                                confirmMessage = "You don't have enough gold!";
                            else
                                confirmMessage = "Item is out of stock!";
                        }
                        else if (vitality1Bought)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "Item is out of stock!";
                        }

                        else if (PlayerSystem.playerVitality >= 100)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "Item is maxed out!";
                        }
                        else
                        {
                            audioSrc.PlayOneShot(purchaseSfx);
                            SetStockValue(slotNum, 0);
                            vitality1Bought = true;
                            confirmMessage = "Thank you!";
                            playSys.AddVitality(1);
                        }
                    }
                    break;
                }
            case Items.Vitality2:
                {
                    if (confirmCounter == 1)
                        confirmMessage = " Would you like to purchase Vitality Essence?";
                    else if (confirmCounter == 2)
                    {
                        int orgGold = PlayerSystem.playerGold;
                        PlayerSystem.playerGold -= price;
                        if (PlayerSystem.playerGold < 0)
                        {
                            PlayerSystem.playerGold = orgGold;
                            if (!vitality2Bought)
                                confirmMessage = "You don't have enough gold!";
                            else
                                confirmMessage = "Item is out of stock!";
                        }
                        else if (vitality2Bought)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "Item is out of stock!";
                        }

                        else if (PlayerSystem.playerVitality >= 100)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "Item is maxed out!";
                        }
                        else
                        {
                            audioSrc.PlayOneShot(purchaseSfx);
                            SetStockValue(slotNum, 0);
                            vitality2Bought = true;
                            confirmMessage = "Thank you!";
                            playSys.AddVitality(1);
                        }
                    }
                    break;
                }
            case Items.Vitality3:
                {
                    if (confirmCounter == 1)
                        confirmMessage = " Would you like to purchase Vitality Essence?";
                    else if (confirmCounter == 2)
                    {
                        int orgGold = PlayerSystem.playerGold;
                        PlayerSystem.playerGold -= price;
                        if (PlayerSystem.playerGold < 0)
                        {
                            PlayerSystem.playerGold = orgGold;
                            if (!vitality3Bought)
                                confirmMessage = "You don't have enough gold!";
                            else
                                confirmMessage = "Item is out of stock!";
                        }
                        else if (vitality3Bought)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "Item is out of stock!";
                        }

                        else if (PlayerSystem.playerVitality >= 100)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "Item is maxed out!";
                        }
                        else
                        {
                            audioSrc.PlayOneShot(purchaseSfx);
                            SetStockValue(slotNum, 0);
                            vitality2Bought = true;
                            confirmMessage = "Thank you!";
                            playSys.AddVitality(1);
                        }
                    }
                    break;
                }
            case Items.LifeBerry:
                {
                    if (confirmCounter == 1)
                        confirmMessage = " Would you like to purchase Life Berry?";
                    else if (confirmCounter == 2)
                    {
                        int orgGold = PlayerSystem.playerGold;
                        PlayerSystem.playerGold -= price;
                        if (PlayerSystem.playerGold < 0)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "You don't have enough gold!";
                        }
                        else if (ItemSystem.lifeBerryAmt >= 9)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "Item is out of stock!";
                        }
                        else
                        {
                            audioSrc.PlayOneShot(purchaseSfx);
                            confirmMessage = "Thank you!";
                            itemSys.GetItem(ItemSystem.Item.LifeBerry, 1);
                            SetStockValue(slotNum, 9 - ItemSystem.lifeBerryAmt);
                        }
                    }
                    break;
                }
            case Items.Rixile:
                {
                    if (confirmCounter == 1)
                        confirmMessage = " Would you like to purchase Rixile?";
                    else if (confirmCounter == 2)
                    {
                        int orgGold = PlayerSystem.playerGold;
                        PlayerSystem.playerGold -= price;
                        if (PlayerSystem.playerGold < 0)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "You don't have enough gold!";
                        }
                        else if (ItemSystem.rixileAmt >= 9)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "Item is out of stock!";
                        }
                        else
                        {
                            audioSrc.PlayOneShot(purchaseSfx);
                            confirmMessage = "Thank you!";
                            itemSys.GetItem(ItemSystem.Item.Rixile, 1);
                            SetStockValue(slotNum, 9 - ItemSystem.rixileAmt);
                        }
                    }
                    break;
                }
            case Items.NicirPlant:
                {
                    if (confirmCounter == 1)
                        confirmMessage = " Would you like to purchase Nicir Plant?";
                    else if (confirmCounter == 2)
                    {
                        int orgGold = PlayerSystem.playerGold;
                        PlayerSystem.playerGold -= price;
                        if (PlayerSystem.playerGold < 0)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "You don't have enough gold!";
                        }
                        else if (ItemSystem.nicirPlantAmt >= 9)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "Item is out of stock!";
                        }
                        else
                        {
                            audioSrc.PlayOneShot(purchaseSfx);
                            confirmMessage = "Thank you!";
                            itemSys.GetItem(ItemSystem.Item.NicirPlant, 1);
                            SetStockValue(slotNum, 9 - ItemSystem.nicirPlantAmt);
                        }
                    }
                    break;
                }
            case Items.MindRemedy:
                {
                    if (confirmCounter == 1)
                        confirmMessage = " Would you like to purchase Mind Remedy?";
                    else if (confirmCounter == 2)
                    {
                        int orgGold = PlayerSystem.playerGold;
                        PlayerSystem.playerGold -= price;
                        if (PlayerSystem.playerGold < 0)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "You don't have enough gold!";
                        }
                        else if (ItemSystem.mindRemedyAmt >= 9)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "Item is out of stock!";
                        }
                        else
                        {
                            audioSrc.PlayOneShot(purchaseSfx);
                            confirmMessage = "Thank you!";
                            itemSys.GetItem(ItemSystem.Item.MindRemedy, 1);
                            SetStockValue(slotNum, 9 - ItemSystem.mindRemedyAmt);
                        }
                    }
                    break;
                }
            case Items.PetriShroom:
                {
                    if (confirmCounter == 1)
                        confirmMessage = " Would you like to purchase Petri-Shroom?";
                    else if (confirmCounter == 2)
                    {
                        int orgGold = PlayerSystem.playerGold;
                        PlayerSystem.playerGold -= price;
                        if (PlayerSystem.playerGold < 0)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "You don't have enough gold!";
                        }
                        else if (ItemSystem.petriShroomAmt >= 9)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "Item is out of stock!";
                        }
                        else
                        {
                            audioSrc.PlayOneShot(purchaseSfx);
                            confirmMessage = "Thank you!";
                            itemSys.GetItem(ItemSystem.Item.PetriShroom, 1);
                            SetStockValue(slotNum, 9 - ItemSystem.petriShroomAmt);
                        }
                    }
                    break;
                }
            case Items.ReliefOintment:
                {
                    if (confirmCounter == 1)
                        confirmMessage = " Would you like to purchase Relief Ointment?";
                    else if (confirmCounter == 2)
                    {
                        int orgGold = PlayerSystem.playerGold;
                        PlayerSystem.playerGold -= price;
                        if (PlayerSystem.playerGold < 0)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "You don't have enough gold!";
                        }
                        else if (ItemSystem.reliefOintmentAmt >= 9)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "Item is out of stock!";
                        }
                        else
                        {
                            audioSrc.PlayOneShot(purchaseSfx);
                            confirmMessage = "Thank you!";
                            itemSys.GetItem(ItemSystem.Item.ReliefOintment, 1);
                            SetStockValue(slotNum, 9 - ItemSystem.reliefOintmentAmt);
                        }
                    }
                    break;
                }
            case Items.TerraIdol:
                {
                    if (confirmCounter == 1)
                        confirmMessage = " Would you like to purchase Terra Idol?";
                    else if (confirmCounter == 2)
                    {
                        int orgGold = PlayerSystem.playerGold;
                        PlayerSystem.playerGold -= price;
                        if (PlayerSystem.playerGold < 0)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "You don't have enough gold!";
                        }
                        else if (ItemSystem.terraIdolAmt >= 9)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "Item is out of stock!";
                        }
                        else
                        {
                            audioSrc.PlayOneShot(purchaseSfx);
                            confirmMessage = "Thank you!";
                            itemSys.GetItem(ItemSystem.Item.TerraIdol, 1);
                            SetStockValue(slotNum, 9 - ItemSystem.terraIdolAmt);
                        }
                    }
                    break;
                }
            case Items.MagicIdol:
                {
                    if (confirmCounter == 1)
                        confirmMessage = " Would you like to purchase Magic Idol?";
                    else if (confirmCounter == 2)
                    {
                        int orgGold = PlayerSystem.playerGold;
                        PlayerSystem.playerGold -= price;
                        if (PlayerSystem.playerGold < 0)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "You don't have enough gold!";
                        }
                        else if (ItemSystem.magicIdolAmt >= 9)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "Item is out of stock!";
                        }
                        else
                        {
                            audioSrc.PlayOneShot(purchaseSfx);
                            confirmMessage = "Thank you!";
                            itemSys.GetItem(ItemSystem.Item.MagicIdol, 1);
                            SetStockValue(slotNum, 9 - ItemSystem.magicIdolAmt);
                        }
                    }
                    break;
                }
            case Items.InnTicket:
                {
                    if (confirmCounter == 1)
                        confirmMessage = " Would you like to purchase an Inn Ticket?";
                    else if (confirmCounter == 2)
                    {
                        int orgGold = PlayerSystem.playerGold;
                        PlayerSystem.playerGold -= price;
                        if (PlayerSystem.playerGold < 0)
                        {
                            PlayerSystem.playerGold = orgGold;
                            confirmMessage = "You don't have enough gold!";
                        }
                        else
                        {
                            audioSrc.PlayOneShot(purchaseSfx);
                            confirmMessage = "Thank you!";
                            isSleeping = true;
                        }
                    }
                    break;
                }
        }
        
        playSys.ApplyGold();
        if (animateConfirm != null)
            StopCoroutine(animateConfirm);
        animateConfirm = AnimateConfirmation();
        StartCoroutine(animateConfirm);
    }
    public void StayAtTheInn()
    {
        if (isShop)
            BrowseTheShop();
        if (!dialogueActive)
        {
            GameObject playerController = GameObject.FindGameObjectWithTag("Player");
            InteractionSystem interaction = playerController.GetComponent<InteractionSystem>();
            SceneSystem sceneSys = GameObject.Find("Player").GetComponent<SceneSystem>();
            CharacterSystem playChar = playerController.GetComponent<CharacterSystem>();
            playChar.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
            sceneSys.EnablePlayer(false);
            interaction.DialogueInteraction(true, "Enjoy your stay!");
            dialogueActive = true;
        }
        else if (Input.GetButtonDown("Submit") && dialogueActive && !finishedDialogue)
        {
            SceneSystem sceneSys = GameObject.Find("Player").GetComponent<SceneSystem>();
            InteractionSystem interaction = GameObject.FindGameObjectWithTag("Player").GetComponent<InteractionSystem>();
            interaction.DialogueInteraction(false, null);
            sceneSys.EnablePlayer(false);
            if (screenRoutine != null)
                StopCoroutine(screenRoutine);
            screenRoutine = FadeScreenIn(1.0f, 4.0f, sceneSys.blackScreenGame, true);
            StartCoroutine(screenRoutine);
            finishedDialogue = true;
        }
    }
    public IEnumerator FadeScreenIn(float aValue, float aTime, Image image, bool imgProperty)
    {
        float alpha = image.GetComponent<Image>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
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

                CharacterSystem charSys = GetComponent<CharacterSystem>();
                charSys.SetPosition();
                transform.position = besideBedPos.position;
                transform.rotation = besideBedPos.rotation;
                PlayerSystem playSys = GetComponent<PlayerSystem>();
                playSys.PlayerRecovery(100);
                yield return new WaitForSeconds(3);
               
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
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(alpha, aValue, t));
            image.GetComponent<Image>().color = newColor;
            if (t >= 0.9)
            {

                SceneSystem sceneSys = GameObject.Find("Player").GetComponent<SceneSystem>();
                sceneSys.EnablePlayer(true);
                isSleeping = false;
                dialogueActive = false;
                finishedDialogue = false;
                if (imgProperty)
                    newColor = new Color(0, 0, 0, 0);
                else
                    newColor = new Color(1, 1, 1, 0);
                image.GetComponent<Image>().color = newColor;
            }
            else
            {
                image.GetComponent<Image>().enabled = true;
            }
            yield return null;
        }

    }
    IEnumerator AnimateConfirmation()
    {
        if (confirmCounter == 1)
        {
            foreach (char letter in confirmMessage.ToCharArray())
            {
                if (confirmCounter == 2 || resetConfirmText)
                {
                    confirmText.text = null;
                    break;
                }
                else if (!resetConfirmText)
                {
                    confirmText.text += letter;
                    yield return 0;
                    yield return new WaitForSecondsRealtime(0);
                }
            }
        }
        else if (confirmCounter == 2)
        {
            foreach (char letter in confirmMessage.ToCharArray())
            {
                if (confirmCounter == 1 || resetConfirmText)
                {

                    confirmText.text = null;
                    break;
                }
                else if (!resetConfirmText)
                {

                    confirmText.text += letter;
                    yield return 0;
                    yield return new WaitForSecondsRealtime(0);
                }
            }
        }
    }
    public void ResetConfirmation()
    {
        if (animateConfirm != null)
            StopCoroutine(animateConfirm);
        confirmText.text = null;
        confirmCounter = 0;
        confirmMessage = null;
    }
    public void BrowseTheShop()
    { 
        isShop = !isShop;
        interaction.DialogueInteraction(false, null);
        GameObject PopupCanvas = GameObject.Find("Core/Player/PopUpCanvas");
        PopupCanvas.SetActive(!isShop);
        Time.timeScale = isShop ? 0 : 1;
        if (isShop)
        {
            ShopMenu.SetActive(true);
            if (fadeImage != null)
                StopCoroutine(fadeImage);
            fadeImage = FadeIn(0.5f, 0.2f, backGround, false);
            StartCoroutine(fadeImage);
        }
        else
        {

            if (fadeImage != null)
                StopCoroutine(fadeImage);
            fadeImage = FadeOut(0.0f, 0.2f, backGround, false);
            StartCoroutine(fadeImage);

        }
    }
    public void SelectItems()
    {
        isBrowsing = true;
        m_Selectable = purchaseButtons[0].GetComponent<Selectable>();
        m_Selectable.Select();
    }
    public void BackOut()
    {
        if (isShop)
        {
            if (isBrowsing)
            {
                m_Selectable = leaveButton.GetComponent<Selectable>();
                isBrowsing = false;
            }
            else
            {
                BrowseTheShop();
                peopleSys.ResetPerson();
            }
            if (m_Selectable != null)
                m_Selectable.Select();
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
                    newColor = new Color(1, 1, 1, 0.5f);
                image.GetComponent<Image>().color = newColor;
                mouseVisible = true;
                shopBanner.SetActive(true);
                m_Selectable = browseButton.GetComponent<Selectable>();
                m_Selectable.Select();
                audioSrc = GetComponent<AudioSource>();
                audioSrc.volume = 1;
                audioSrc.pitch = 1;
                audioSrc.PlayOneShot(pauseSfx);
                if (fadeImage != null)
                    StopCoroutine(fadeImage);
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
                isShop = false;
                isBrowsing = false;
                mouseVisible = false;
                shopBanner.SetActive(false);
                shopWindow.SetActive(false);
                audioSrc = GetComponent<AudioSource>();
                audioSrc.volume = 1;
                audioSrc.pitch = 1;
                audioSrc.PlayOneShot(pauseSfx);
                ShopMenu.SetActive(false);
                if(peopleSys != null)
                    peopleSys.CloseTheShop();
                if (fadeImage != null)
                    StopCoroutine(fadeImage);
            }
            else
                image.GetComponent<Image>().enabled = true;
            yield return null;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Person"))
            peopleSys = other.gameObject.GetComponent<PeopleSystem>();
    }
   public void LoadVitalityBought(bool vitality1, bool vitality2, bool vitality3)
    {
        vitality1Bought = vitality1;
        vitality2Bought = vitality2;
        vitality3Bought = vitality3;
    }
}
[Serializable]
public struct Sprites
{
    public Sprite vitalitySprite;
    public Sprite lifeBerrySprite;
    public Sprite rixileSprite;
    public Sprite nicirPlantSprite;
    public Sprite mindRemedySprite;
    public Sprite PetriShroomSprite;
    public Sprite reliefOintmentSprite;
    public Sprite terraIdolSprite;
    public Sprite magicIdolSprite;
    public Sprite innTicketSprite;
}
