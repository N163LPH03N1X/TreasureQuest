using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class ItemSystem : MonoBehaviour
{
    OptSystem optSystem = new OptSystem();
    public GameObject itemWindow;
    public GameObject equipmentWindow;
    public GameObject optionWindow;
    public GameObject entriesWindow;
    public static bool isDoorLocked;
    private Selectable m_Selectable;
    public AudioSource audioSrc;
    public AudioClip demonMaskOff;
    PlayerSystem playSys;
    SwordSystem swordSys;

    public Button mythrilSword;

    bool itemsSelected = false;
    public Button itemsButton;

    bool equipmentSelected = false;
    public Button equipmentButton;

    bool optionsSelected = false;
    public Button optionsButton;

    bool entriesSelected = false;
    public Button entriesButton;


    public Toggle FullScreenTog;
    bool backButton = false;
    GameObject doorObj;
    bool isLocked;
    public static bool inDoorTerritory;
    public float flashTime;
    float flashTimer = 0.1f;

    bool isHealed;
    public Image healFlash;
    bool isPoisoned;
    public Image poisonFlash;
    bool isConfused;
    public Image confusedFlash;
    bool isSilenced;
    public Image silencedFlash;
    bool isParalyzed;
    public Image paralyzedFlash;
    bool isItemed;
    public Image itemFlash;

    IEnumerator routine;
    IEnumerator demonRoutine;
    IEnumerator keyRoutine;

    [Header("LifeBerry")]
    public static int lifeBerryAmt = 0;
    public Text lifeBerryValue;
    public Text lifeBerryUIValue;
    public Image lifeBerryIcon;
    public Button lifeBerryButton;
    public static bool lifeBerryEmpty;
    [Space]
    [Header("Rixile")]
    public static int rixileAmt = 0;
    public Text rixileValue;
    public Image rixileIcon;
    public Button rixileButton;
    public static bool rixileEmpty;
    [Space]
    [Header("NicirPlant")]
    public static int nicirPlantAmt = 0;
    public Text nicirPlantValue;
    public Image nicirPlantIcon;
    public Button nicirPlantButton;
    public static bool nicirPlantEmpty;
    [Space]
    [Header("MindRemedy")]
    public static int mindRemedyAmt = 0;
    public Text mindRemedyValue;
    public Image mindRemedyIcon;
    public Button mindRemedyButton;
    public static bool mindRemedyEmpty;
    [Space]
    [Header("PetriShroom")]
    public static int petriShroomAmt = 0;
    public Text petriShroomValue;
    public Image petriShroomIcon;
    public Button petriShroomButton;
    public static bool petriShroomEmpty;
    [Space]
    [Header("ReliefOintment")]
    public static int reliefOintmentAmt = 0;
    public Text reliefOintmentValue;
    public Image reliefOintmentIcon;
    public Button reliefOintmentButton;
    public static bool reliefOintmentEmpty;
    [Space]
    [Header("Terra Idol")]
    public static int terraIdolAmt = 0;
    public Text terraIdolValue;
    public Image terraIdolIcon;
    public Button terraIdolButton;
    public static bool terraIdolEmpty;
    [Space]
    [Header("Magic Idol")]
    public static int magicIdolAmt = 0;
    public Text magicIdolValue;
    public Image magicIdolIcon;
    public Button magicIdolButton;
    public static bool magicIdolEmpty;
    [Space]
    [Header("Key")]
    public static int keyAmt = 0;
    public Text keyValue;
    public Image keyIcon;
    public Button keyButton;
    public static bool keyEmpty;
    [Space]
    [Header("Demon Mask")]
    public static bool demonMaskPickedUp = false;
    public Image demonMaskIcon;
    public Button demonMaskButton;
    public static bool demonMaskEnabled;
    public Image demonMaskBar;
    public GameObject demonMaskStatus;
    public float demonMasktime = 30f;
    public float demonMasktimer;
    bool maskOn = false;
    GameObject rotation;
    RectTransform spin;
    [Space]
    [Header("Moon Pearl")]
    public static bool moonPearlPickedUp = false;
    public Image moonPearlIcon;
    public Button moonPearlButton;
    [Space]
    [Header("Mark of Etymology")]
    public static bool markOfEtymologyPickedUp = false;
    public Image markOfEtymologyIcon;
    public Button markOfEtymologyButton;
    public static bool markOfEtymologyEnabled = false;

    public Button journalEntryButton;
    public static bool StoryMenuOpen = false;

    public GameObject SelectionBar;

    // Start is called before the first frame update
    void Start()
    {
        playSys = GetComponent<PlayerSystem>();
        swordSys = GetComponent<SwordSystem>();
        PresetAllItems();
        flashTime = flashTimer;
        rotation = GameObject.Find("Core/Player/PlayerCanvas/GameUI/Effects/DemonMask/Rotation");
        spin = rotation.GetComponent<RectTransform>();
        demonMasktimer = demonMasktime;
    }
    private void Update()
    {
        DemonEnabled(demonMaskEnabled);
        if (optSystem.Input.GetButtonDown("Cancel") && !backButton && PauseGame.isPaused && !ShopSystem.isShop)
        {
            BackOut();
            backButton = true;
        }
        else if (optSystem.Input.GetButtonUp("Cancel"))
            backButton = false;
    }
    public enum Item { LifeBerry, Rixile, NicirPlant, MindRemedy, PetriShroom, ReliefOintment,
                         TerraIdol, MagicIdol, key, DemonMask, MoonPearl, MarkofEtymology, IDCard, LifeChime,
                            SpiritRich, PowerBand, Pendant, SpellBind, DeathBind }
    public void GetItem(Item item, int amount)
    {
        playSys.GotItem();
        switch (item)
        {
            case Item.LifeBerry:
                {
                    lifeBerryAmt += amount;
                    if (lifeBerryAmt > 9)
                    {
                        lifeBerryAmt = 9;
                    }
                    EnableItem(Item.LifeBerry);
                    break;
                }
            case Item.Rixile:
                {
                    rixileAmt += amount;
                    if (rixileAmt > 9)
                    {
                        rixileAmt = 9;
                    }
                    EnableItem(Item.Rixile);
                    break;
                }
            case Item.NicirPlant:
                {
                    nicirPlantAmt += amount;
                    if (nicirPlantAmt > 9)
                    {
                        nicirPlantAmt = 9;
                    }
                    EnableItem(Item.NicirPlant);
                    break;
                }
            case Item.MindRemedy:
                {
                    mindRemedyAmt += amount;
                    if (mindRemedyAmt > 9)
                    {
                        mindRemedyAmt = 9;
                    }
                    EnableItem(Item.MindRemedy);
                    break;
                }
            case Item.PetriShroom:
                {
                    petriShroomAmt += amount;
                    if (petriShroomAmt > 9)
                    {
                        petriShroomAmt = 9;
                    }
                    EnableItem(Item.PetriShroom);
                    break;
                }
            case Item.ReliefOintment:
                {
                    reliefOintmentAmt += amount;
                    if (reliefOintmentAmt > 9)
                    {
                        reliefOintmentAmt = 9;
                    }
                    EnableItem(Item.ReliefOintment);
                    break;
                }
            case Item.TerraIdol:
                {
                    terraIdolAmt += amount;
                    if (terraIdolAmt > 9)
                    {
                        terraIdolAmt = 9;
                    }
                    EnableItem(Item.TerraIdol);
                    break;
                }
            case Item.MagicIdol:
                {
                    magicIdolAmt += amount;
                    if (magicIdolAmt > 9)
                    {
                        magicIdolAmt = 9;
                    }
                    EnableItem(Item.MagicIdol);
                    break;
                }
            case Item.key:
                {
                    keyAmt += amount;
                    if (keyAmt > 9)
                    {
                        keyAmt = 9;
                    }
                    EnableItem(Item.key);
                    break;
                }
            case Item.DemonMask:
                {
                    demonMaskPickedUp = true;
                    EnableItem(Item.DemonMask);
                    break;
                }
            case Item.MoonPearl:
                {
                    moonPearlPickedUp = true;
                    EnableItem(Item.MoonPearl);
                    break;
                }
            case Item.MarkofEtymology:
                {
                    markOfEtymologyPickedUp = true;
                    EnableItem(Item.MarkofEtymology);
                    break;
                }
        }
        SetItemValue();
    }
    public void UseItem(Item item, int amount)
    {
        switch (item)
        {
            case Item.LifeBerry:
                {
                    lifeBerryAmt -= amount;
                    if(lifeBerryAmt >= 0)
                    {
                        isHealed = true;
                        routine = Flash();
                        StartCoroutine(routine);
                        if (PlayerSystem.playerVitality > 11)
                            playSys.PlayerRecovery(11 + (Mathf.RoundToInt(PlayerSystem.playerVitality - 11) / 2));
                        else
                            playSys.PlayerRecovery(11);
                    }
                    else if (lifeBerryAmt < 0)
                    {
                        lifeBerryAmt = 0;
                    }
                    EnableItem(Item.LifeBerry);
                    if (PlayerSystem.isDead)
                    {
                        PlayerSystem playST = GetComponent<PlayerSystem>();
                        playST.PlayerRevived();
                    }
                    break;
                }
            case Item.Rixile:
                {
                    rixileAmt -= amount;
                    if (rixileAmt >= 0)
                    {
                        isHealed = true;
                        routine = Flash();
                        StartCoroutine(routine);
                        playSys.PlayerRecovery(100);
                    }
                    else if (rixileAmt < 0)
                        rixileAmt = 0;
                    EnableItem(Item.Rixile);
                    break;
                }
            case Item.NicirPlant:
                {
                    nicirPlantAmt -= amount;
                    if (nicirPlantAmt >= 0)
                    {
                        isPoisoned = true;
                        routine = Flash();
                        StartCoroutine(routine);
                        playSys.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.poisonMin, false);
                        playSys.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.poisonMax, false);
                    }
                    else if (nicirPlantAmt < 0)
                        nicirPlantAmt = 0;
                    EnableItem(Item.NicirPlant);
                    break;
                }
            case Item.MindRemedy:
                {
                  
                    mindRemedyAmt -= amount;
                    if (mindRemedyAmt >= 0)
                    {
                        isConfused = true;
                        routine = Flash();
                        StartCoroutine(routine);
                        playSys.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.confuse, false);
                    }
                    else if (mindRemedyAmt < 0)
                        mindRemedyAmt = 0;
                    EnableItem(Item.MindRemedy);
                    break;
                }
            case Item.PetriShroom:
                {
                    petriShroomAmt -= amount;
                    if (petriShroomAmt >= 0)
                    {
                        isParalyzed = true;
                        routine = Flash();
                        StartCoroutine(routine);
                        playSys.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.paralyzed, false);
                    }
                    else if (petriShroomAmt < 0)
                        petriShroomAmt = 0;
                    EnableItem(Item.PetriShroom);
                    break;
                }
            case Item.ReliefOintment:
                {
                    reliefOintmentAmt -= amount;
                    if (reliefOintmentAmt >= 0)
                    {
                        isSilenced = true;
                        routine = Flash();
                        StartCoroutine(routine);
                        playSys.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.silence, false);
                    }
                    else if (reliefOintmentAmt < 0)
                        reliefOintmentAmt = 0;
                    EnableItem(Item.ReliefOintment);
                    break;
                }
            case Item.TerraIdol:
                {
                    terraIdolAmt -= amount;
                    if (terraIdolAmt >= 0)
                    {
                        isItemed = true;
                        routine = Flash();
                        StartCoroutine(routine);
                        swordSys.TerraIdolUsed();
                    }
                    else if (terraIdolAmt < 0)
                        terraIdolAmt = 0;
                    EnableItem(Item.TerraIdol);
                    break;
                }
            case Item.MagicIdol:
                {
                    magicIdolAmt -= amount;
                    if (magicIdolAmt >= 0)
                    {
                        isItemed = true;
                        routine = Flash();
                        StartCoroutine(routine);
                        swordSys.MagicIdolUsed(true);
                    }
                    else if (magicIdolAmt < 0)
                        magicIdolAmt = 0;
                    EnableItem(Item.MagicIdol);
                    break;
                }
            case Item.key:
                {
                    DoorSystem doorSys = doorObj.GetComponent<DoorSystem>();
                    if (doorSys != null)
                    {
                        if (doorSys.isLocked)
                        {
                            keyAmt -= amount;
                            if (keyAmt >= 0)
                            {
                                if (keyRoutine != null)
                                    StopCoroutine(keyRoutine);
                                keyRoutine = KeyRoutine(KeyType.key);
                                StartCoroutine(keyRoutine);
                            }
                            else if (keyAmt < 0)
                                keyAmt = 0;
                            EnableItem(Item.key);
                        }
                    
                    }
                    break;
                }
            case Item.DemonMask:
                {
                    if(!demonMaskEnabled) demonMaskEnabled = true;
                    else if(demonMaskEnabled) demonMaskEnabled = false;
                    CharacterSystem charSys = GetComponent<CharacterSystem>();
                    if(CharacterSystem.isClimbing)
                        charSys.SwitchMovement(CharacterSystem.MovementType.climbStop);
                    charSys.SetFalling();
                    isItemed = true;
                    routine = Flash();
                    StartCoroutine(routine);
                    break;
                }
            case Item.MoonPearl:
                {
                    DoorSystem doorSys = doorObj.GetComponent<DoorSystem>();
                    if (doorSys != null)
                    {
                        if (doorSys.isLocked && doorSys.isMoonDoor && TimeSystem.isNight)
                        {
                            if (keyRoutine != null)
                                StopCoroutine(keyRoutine);
                            keyRoutine = KeyRoutine(KeyType.MoonPearl);
                            StartCoroutine(keyRoutine);
                        }

                    }
                    break;
                }
            case Item.MarkofEtymology:
                {
                    if (!markOfEtymologyEnabled) markOfEtymologyEnabled = true;
                    else if (markOfEtymologyEnabled) markOfEtymologyEnabled = false;
                    isItemed = true;
                    routine = Flash();
                    StartCoroutine(routine);
                    break;
                }
        }
       
    }
    public void SetItemValue()
    {
        if (!lifeBerryEmpty)
        {
            lifeBerryValue.text = "x" + lifeBerryAmt.ToString();
            lifeBerryUIValue.text = lifeBerryValue.text;
        }
        if (!rixileEmpty)
            rixileValue.text = "x" + rixileAmt.ToString();
        if (!nicirPlantEmpty)
            nicirPlantValue.text = "x" + nicirPlantAmt.ToString();
        if (!mindRemedyEmpty)
            mindRemedyValue.text = "x" + mindRemedyAmt.ToString();
        if (!petriShroomEmpty)
            petriShroomValue.text = "x" + petriShroomAmt.ToString();
        if (!reliefOintmentEmpty)
            reliefOintmentValue.text = "x" + reliefOintmentAmt.ToString();
        if (!terraIdolEmpty)
            terraIdolValue.text = "x" + terraIdolAmt.ToString();
        if (!magicIdolEmpty)
            magicIdolValue.text = "x" + magicIdolAmt.ToString();
        if (!keyEmpty)
            keyValue.text = "x" + keyAmt.ToString();
    }
    public void EnableItem(Item item)
    {
        switch (item)
        {
            case Item.LifeBerry:
                {
                    if (lifeBerryAmt > 0)
                    {
                        lifeBerryEmpty = false;
                        lifeBerryIcon.enabled = true;
                        lifeBerryButton.interactable = true;
                        SetItemValue();
                    }
                    else if (lifeBerryAmt <= 0)
                    {
                        lifeBerryEmpty = true;
                        lifeBerryIcon.enabled = false;
                        m_Selectable = rixileButton.GetComponent<Selectable>();
                        m_Selectable.Select();
                        lifeBerryButton.interactable = false;
                        lifeBerryValue.text = "";
                        lifeBerryUIValue.text = "x" + lifeBerryAmt.ToString();
                    }
                    break;
                }
            case Item.Rixile:
                {
                    if (rixileAmt > 0)
                    {
                        rixileEmpty = false;
                        rixileIcon.enabled = true;
                        rixileButton.interactable = true;
                        SetItemValue();
                    }
                    else
                    {
                        rixileEmpty = true;
                        rixileIcon.enabled = false;
                        m_Selectable = nicirPlantButton.GetComponent<Selectable>();
                        m_Selectable.Select();
                        rixileButton.interactable = false;
                        rixileValue.text = "";
                    }
                    break;
                }
            case Item.NicirPlant:
                {
                    if (nicirPlantAmt > 0)
                    {
                        nicirPlantEmpty = false;
                        nicirPlantIcon.enabled = true;
                        nicirPlantButton.interactable = true;
                        SetItemValue();
                    }
                    else
                    {
                        nicirPlantEmpty = true;
                        nicirPlantIcon.enabled = false;
                        m_Selectable = mindRemedyButton.GetComponent<Selectable>();
                        m_Selectable.Select();
                        nicirPlantButton.interactable = false;
                        nicirPlantValue.text = "";
                    }
                    break;
                }
            case Item.MindRemedy:
                {
                    if (mindRemedyAmt > 0)
                    {
                        mindRemedyEmpty = false;
                        mindRemedyIcon.enabled = true;
                        mindRemedyButton.interactable = true;
                        SetItemValue();
                    }
                    else
                    {
                        mindRemedyEmpty = true;
                        mindRemedyIcon.enabled = false;
                        m_Selectable = petriShroomButton.GetComponent<Selectable>();
                        m_Selectable.Select();
                        mindRemedyButton.interactable = false;
                        mindRemedyValue.text = "";
                    }
                    break;
                }
            case Item.PetriShroom:
                {
                    if (petriShroomAmt > 0)
                    {
                        petriShroomEmpty = false;
                        petriShroomIcon.enabled = true;
                        petriShroomButton.interactable = true;
                        SetItemValue();
                    }
                    else
                    {
                        petriShroomEmpty = true;
                        petriShroomIcon.enabled = false;
                        m_Selectable = reliefOintmentButton.GetComponent<Selectable>();
                        m_Selectable.Select();
                        petriShroomButton.interactable = false;
                        petriShroomValue.text = "";
                    }
                    break;
                }
            case Item.ReliefOintment:
                {
                    if (reliefOintmentAmt > 0)
                    {
                        reliefOintmentEmpty = false;
                        reliefOintmentIcon.enabled = true;
                        reliefOintmentButton.interactable = true;
                        SetItemValue();
                    }
                    else
                    {
                        reliefOintmentEmpty = true;
                        reliefOintmentIcon.enabled = false;
                        m_Selectable = terraIdolButton.GetComponent<Selectable>();
                        m_Selectable.Select();
                        reliefOintmentButton.interactable = false;
                        reliefOintmentValue.text = "";
                    }
                    break;
                }
            case Item.TerraIdol:
                {
                    if (terraIdolAmt > 0)
                    {
                        terraIdolEmpty = false;
                        terraIdolIcon.enabled = true;
                        terraIdolButton.interactable = true;
                        SetItemValue();
                    }
                    else
                    {
                        terraIdolEmpty = true;
                        terraIdolIcon.enabled = false;
                        m_Selectable = magicIdolButton.GetComponent<Selectable>();
                        m_Selectable.Select();
                        terraIdolButton.interactable = false;
                        terraIdolValue.text = "";
                    }
                    break;
                }
            case Item.MagicIdol:
                {
                    if (magicIdolAmt > 0)
                    {
                        magicIdolEmpty = false;
                        magicIdolIcon.enabled = true;
                        magicIdolButton.interactable = true;
                        SetItemValue();
                    }
                    else
                    {
                        magicIdolEmpty = true;
                        magicIdolIcon.enabled = false;
                        m_Selectable = keyButton.GetComponent<Selectable>();
                        m_Selectable.Select();
                        magicIdolButton.interactable = false;
                        magicIdolValue.text = "";
                    }
                    break;
                }
            case Item.key:
                {
                    if (keyAmt > 0)
                    {
                        keyEmpty = false;
                        keyIcon.enabled = true;
                        keyButton.interactable = true;
                        SetItemValue();
                    }
                    else
                    {
                        keyEmpty = true;
                        keyIcon.enabled = false;
                        m_Selectable = demonMaskButton.GetComponent<Selectable>();
                        m_Selectable.Select();
                        keyButton.interactable = false;
                        keyValue.text = "";
                    }
                    break;
                }
            case Item.DemonMask:
                {
                    if (demonMaskPickedUp)
                    {
                        demonMaskIcon.enabled = true;
                        demonMaskButton.interactable = true;
                    }
                    else
                    {
                        demonMaskIcon.enabled = false;
                        m_Selectable = moonPearlButton.GetComponent<Selectable>();
                        m_Selectable.Select();
                        demonMaskButton.interactable = false;
                    }
                    break;
                }
            case Item.MoonPearl:
                {
                    if (moonPearlPickedUp)
                    {
                        moonPearlIcon.enabled = true;
                        moonPearlButton.interactable = true;
                    }
                    else
                    {
                        moonPearlIcon.enabled = false;
                        m_Selectable = markOfEtymologyButton.GetComponent<Selectable>();
                        m_Selectable.Select();
                        moonPearlButton.interactable = false;
                    }
                    break;
                }
            case Item.MarkofEtymology:
                {
                    if (markOfEtymologyPickedUp)
                    {
                        markOfEtymologyIcon.enabled = true;
                        markOfEtymologyButton.interactable = true;
                    }
                    else
                    {
                        markOfEtymologyIcon.enabled = false;
                        m_Selectable = lifeBerryButton.GetComponent<Selectable>();
                        m_Selectable.Select();
                        markOfEtymologyButton.interactable = false;
                    }
                    break;
                }
        }
    }
    public void LoadItemSystem(int lifeBerryNum, int rixileNum, int nicirPlantNum, int mindRemedyNum, int petriShroomNum, 
                               int reliefOintmentNum, int terraIdolNum, int magicIdolNum, int keyNum, bool demonMaskEnabled, bool moonPearlEnabled, bool markOfEtymologyEnabled)
    {
        lifeBerryAmt = lifeBerryNum;
        rixileAmt = rixileNum;
        nicirPlantAmt = nicirPlantNum;
        mindRemedyAmt = mindRemedyNum;
        petriShroomAmt = petriShroomNum;
        reliefOintmentAmt = reliefOintmentNum;
        terraIdolAmt = terraIdolNum;
        magicIdolAmt = magicIdolNum;
        keyAmt = keyNum;
        demonMaskPickedUp = demonMaskEnabled;
        moonPearlPickedUp = moonPearlEnabled;
        markOfEtymologyPickedUp = markOfEtymologyEnabled;
        PresetAllItems();
    }
    public void PresetAllItems()
    {
        EnableItem(Item.LifeBerry);
        EnableItem(Item.Rixile);
        EnableItem(Item.NicirPlant);
        EnableItem(Item.MindRemedy);
        EnableItem(Item.PetriShroom);
        EnableItem(Item.ReliefOintment);
        EnableItem(Item.TerraIdol);
        EnableItem(Item.MagicIdol);
        EnableItem(Item.key);
        EnableItem(Item.DemonMask);
        EnableItem(Item.MoonPearl);
        EnableItem(Item.MarkofEtymology);
    }
    public void resetItems()
    {
        lifeBerryAmt = 0;
        rixileAmt = 0;
        nicirPlantAmt = 0;
        mindRemedyAmt = 0;
        petriShroomAmt = 0;
        reliefOintmentAmt = 0;
        terraIdolAmt = 0;
        magicIdolAmt = 0;
        keyAmt = 0;
        demonMaskPickedUp = false;
        moonPearlPickedUp = false;
        markOfEtymologyPickedUp = false;
        PresetAllItems();
    }
    public void SelectEntries()
    {

        itemsSelected = false;
        equipmentSelected = false;
        entriesSelected = true;
        optionsSelected = false;
        StoryMenuOpen = true;
        m_Selectable = journalEntryButton.GetComponent<Selectable>();
        m_Selectable.Select();
        SelectionBar.SetActive(false);
    }
    public void SelectItems()
    {
        itemsSelected = true;
        equipmentSelected = false;
        entriesSelected = false;
       optionsSelected = false;
        m_Selectable = lifeBerryButton.GetComponent<Selectable>();
        m_Selectable.Select();
        SelectionBar.SetActive(false);
    }
    public void SelectEquipment()
    {
        itemsSelected = false;
        equipmentSelected = true;
        entriesSelected = false;
        optionsSelected = false;
        m_Selectable = mythrilSword.GetComponent<Selectable>();
        m_Selectable.Select();
        SelectionBar.SetActive(false);
    }
    public void SelectOptions()
    {
        itemsSelected = false;
        optionsSelected = true;
        equipmentSelected = false;
        entriesSelected = false;
        m_Selectable = FullScreenTog.GetComponent<Selectable>();
        m_Selectable.Select();
        SelectionBar.SetActive(false);
    }
    public void BackOut()
    {
        SelectionBar.SetActive(true);
        if (!PauseGame.isStatus && !ShopSystem.isShop) {
            if (itemsSelected)
            {
                itemWindow.SetActive(false);
                m_Selectable = itemsButton.GetComponent<Selectable>();
                m_Selectable.Select();
                itemsSelected = false;
            }
            else if (equipmentSelected)
            {
                equipmentWindow.SetActive(false);
                m_Selectable = equipmentButton.GetComponent<Selectable>();
                m_Selectable.Select();
                equipmentSelected = false;
            }
            else if (optionsSelected)
            {
                optionWindow.SetActive(false);
                m_Selectable = optionsButton.GetComponent<Selectable>();
                m_Selectable.Select();
                optionsSelected = false;
            }
            else if (entriesSelected)
            {
                entriesWindow.SetActive(false);
                m_Selectable = entriesButton.GetComponent<Selectable>();
                m_Selectable.Select();
                entriesSelected = false;
            }
            else
            {
                PauseGame unPause = GetComponent<PauseGame>();
                unPause.PauseTheGame();
            }
        }
        else if (PauseGame.isStatus && !ShopSystem.isShop)
        {
            itemsSelected = false;
            optionsSelected = false;
            equipmentSelected = false;
            entriesSelected = false;
            PauseGame pauseMenu = GetComponent<PauseGame>();
            pauseMenu.BackOut();
        }
        StoryMenuOpen = false;
}
    public enum KeyType { key, MoonPearl}
    public IEnumerator KeyRoutine(KeyType type)
    {
        switch (type)
        {
            case KeyType.key:
                {
                    DoorSystem doorSys = doorObj.GetComponent<DoorSystem>();
                    isItemed = true;
                    routine = Flash();
                    StartCoroutine(routine);
                    doorSys.DoorStatus(false);
                    isDoorLocked = doorSys.isLocked;
                    yield return new WaitUntil(() => PauseGame.isPaused == false);
                    doorSys.UnlockDoorAuto();
             
                    break;
                }
            case KeyType.MoonPearl:
                {
                    DoorSystem doorSys = doorObj.GetComponent<DoorSystem>();
                    if (doorSys != null)
                    {
                        if (doorSys.isLocked && doorSys.isMoonDoor && TimeSystem.isNight)
                        {
                            isItemed = true;
                            routine = Flash();
                            StartCoroutine(routine);
                            doorSys.DoorStatus(false);
                            isDoorLocked = doorSys.isLocked;
                            yield return new WaitUntil(() => PauseGame.isPaused == false);
                            doorSys.UnlockDoorAuto();
                        }

                    }
                    break;
                }
        }
       
    }
    public void DemonEnabled(bool maskEnabled)
    {
        if (maskEnabled)
        {
            GameObject demonMaskUI = GameObject.Find("Core/Player/PlayerCanvas/GameUI/Effects/DemonMask");
           
            spin.Rotate(new Vector3(0, 0, 0.5f));
            if (demonMasktimer > 0)
            {
                maskOn = true;
                handler(Item.DemonMask);
                demonMasktimer -= Time.deltaTime;
                demonMaskUI.SetActive(true);
                demonMaskStatus.SetActive(true);
            }
            else if (demonMasktimer < 0)
            {
                CharacterSystem charSys = GetComponent<CharacterSystem>();
                if (CharacterSystem.isClimbing)
                    charSys.SwitchMovement(CharacterSystem.MovementType.climbStop);
                charSys.SetFalling();
                demonMaskStatus.SetActive(false);
                demonMaskUI.SetActive(false);
                demonMaskEnabled = false;
                audioSrc.pitch = 1;
                audioSrc.volume = 1;
                audioSrc.PlayOneShot(demonMaskOff);
                demonMasktimer = demonMasktime;


            }
        }
        else
        {
            if(maskOn)
            {
                GameObject demonMaskUI = GameObject.Find("Core/Player/PlayerCanvas/GameUI/Effects/DemonMask");
                audioSrc.pitch = 1;
                audioSrc.volume = 1;
                audioSrc.PlayOneShot(demonMaskOff);
                demonMasktimer = demonMasktime;
                demonMaskStatus.SetActive(false);
                demonMaskUI.SetActive(false);
                maskOn = false;
            }
        }


    }
    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
    void handler(Item curItem)
    {
        switch (curItem)
        {
            case Item.DemonMask:
                {
                    if (demonMaskBar != null)
                        demonMaskBar.fillAmount = Map(demonMasktimer, 1, demonMasktime, 0, 1);
                    break;
                }
        }
    }
    public IEnumerator Flash()
    {
        if (isHealed)
        {
            while (isHealed)
            {
                healFlash.enabled = true;
                yield return new WaitForSecondsRealtime(flashTime);
                healFlash.enabled = false;
                isHealed = false;
                StopCoroutine(routine);
                break;
            }
        }
        else if (isItemed)
        {
            while (isItemed)
            {
                itemFlash.enabled = true;
                yield return new WaitForSecondsRealtime(flashTime);
                itemFlash.enabled = false;
                isItemed = false;
                StopCoroutine(routine);
                break;
            }
        }
        else if (isConfused)
        {
            while (isConfused)
            {
                confusedFlash.enabled = true;
                yield return new WaitForSecondsRealtime(flashTime);
                confusedFlash.enabled = false;
                isConfused = false;
                StopCoroutine(routine);
                break;
            }
        }
        else if (isPoisoned)
        {
            while (isPoisoned)
            {
                poisonFlash.enabled = true;
                yield return new WaitForSecondsRealtime(flashTime);
                poisonFlash.enabled = false;
                isPoisoned = false;
                StopCoroutine(routine);
                break;
            }
        }
        else if (isSilenced)
        {
            while (isSilenced)
            {
                silencedFlash.enabled = true;
                yield return new WaitForSecondsRealtime(flashTime);
                silencedFlash.enabled = false;
                isSilenced = false;
                StopCoroutine(routine);
                break;
            }
        }
        else if (isParalyzed)
        {
            while (isParalyzed)
            {
                paralyzedFlash.enabled = true;
                yield return new WaitForSecondsRealtime(flashTime);
                paralyzedFlash.enabled = false;
                isParalyzed = false;
                StopCoroutine(routine);
                break;
            }
        }
    }
    public void ShutOffDemonMask()
    {
        demonMaskEnabled = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            isDoorLocked = other.gameObject.GetComponent<DoorSystem>().isLocked;
            if (isDoorLocked)
            {
                inDoorTerritory = true;
                doorObj = other.gameObject;
            }
        }
       
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            inDoorTerritory = false;
            doorObj = null;
        }
    }
}
