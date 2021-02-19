using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DebugSystem : MonoBehaviour
{
    public AudioSource M_audioSrc;
    PlayerSystem playerSystem;
    SwordSystem swordSystem;
    JewelSystem jewelSystem;
    ItemSystem itemSystem;
    SceneSystem sceneSystem;
    ShopSystem shopSystem;
    CharacterSystem characterSystem;
    UnderWaterSystem underWaterSystem;
    PauseGame pauseGame;
    StorySystem storySystem;
    ObjectSystem objectSystem;
    public AudioClip[] soundFx;

    [Header("Main Debug Code")]
    private string[] debugCode = new string[7];
    [Header("Master Codes")]
    private string[] giveEverything = new string[8];
    private string[] loseEverything = new string[8];
    private string[] beatEverything = new string[8];
    private string[] resetEverything = new string[8];
    private string[] invincibilityOn = new string[8];
    private string[] invincibilityOff = new string[8];
    private string[] maxedOutSwordOn = new string[8];
    private string[] maxedOutSwordOff = new string[8];
    [Header("Sword Codes")]
    private string[] giveAllSwords = new string[9];
    private string[] giveFlareSword = new string[10];
    private string[] giveLightningSword = new string[14];
    private string[] giveFrostSword = new string[10];
    private string[] giveTerraSword = new string[10];
    [Header("Armor Codes")]
    private string[] giveAllArmor = new string[8];
    private string[] giveHelixArmor = new string[10];
    private string[] giveApexArmor = new string[9];
    private string[] giveMythicArmor = new string[11];
    private string[] giveLegendaryArmor = new string[14];
    [Header("Boot Codes")]
    private string[] giveAllBoots = new string[8];
    private string[] giveHoverBoots = new string[9];
    private string[] giveCinderBoots = new string[10];
    private string[] giveStormBoots = new string[9];
    private string[] giveMedicalBoots = new string[11];
    [Header("Jewel Codes")]
    private string[] giveAllJewels = new string[9];
    private string[] giveRedJewel = new string[8];
    private string[] giveBlueJewel = new string[9];
    private string[] giveGreenJewel = new string[10];
    private string[] giveYellowJewel = new string[11];
    private string[] givePurpleJewel = new string[11];
    [Header("Item Codes")]
    private string[] giveAllItems = new string[8];
    private string[] giveLifeBerry = new string[9];
    private string[] giveRixile = new string[6];
    private string[] giveNicirPlant = new string[10];
    private string[] giveMindRemedy = new string[10];
    private string[] givePetrishroom = new string[11];
    private string[] giveReliefOintment = new string[14];
    private string[] giveMagicIdol = new string[9];
    private string[] giveTerraIdol = new string[9];
    private string[] giveKey = new string[3];
    private string[] giveDemonMask = new string[9];
    private string[] giveMoonPearl = new string[9];
    private string[] giveMarkOfEtymology = new string[15];

    [Header("Player Codes")]
    private string[] addMaxAir = new string[7];
    private string[] addMaxGold = new string[7];
    private string[] setGold = new string[7];
    private string[] addMaxHealth = new string[9];
    private string[] setHealth = new string[9];
    private string[] addMaxVitality = new string[11];
    private string[] setVitality = new string[11];
    private string[] setCamera = new string[11];
    private string[] setSwimming = new string[11];
    private string[] killPlayer = new string[11];
    private string[] savePlayerPos = new string[11];
    private string[] loadPlayerPos = new string[11];
    [Header("Status Effect Codes")]
    private string[] allEffectsOn = new string[12];
    private string[] allEffectsOff = new string[13];
    private string[] addPoisonMinStatusEffect = new string[11];
    private string[] removePoisonMinStatusEffect = new string[12];
    private string[] addPoisonMaxStatusEffect = new string[11];
    private string[] removePoisonMaxStatusEffect = new string[12];
    private string[] addConfusedStatusEffect = new string[10];
    private string[] removeConfusedStatusEffect = new string[11];
    private string[] addSilencedStatusEffect = new string[10];
    private string[] removeSilencedStatusEffect = new string[11];
    private string[] addParalyzedStatusEffect = new string[11];
    private string[] removeParalyzedStatusEffect = new string[12];
    [Header("Open Shop Codes")]
    private string[] openShopDuskCliff = new string[11];
    private string[] openShopWindAcre = new string[11];
    private string[] openShopSkulkCove = new string[11];
    [Header("Scene Codes")]
    private string[] goToTitle = new string[11];
    private string[] goToIntro = new string[11];
    private string[] goToOverWorld = new string[11];
    private string[] goToDungeon1 = new string[11];
    private string[] goToDungeon2 = new string[11];
    private string[] goToDungeon3 = new string[11];
    private string[] goToTemple1 = new string[11];
    private string[] goToGhostShip = new string[11];
    private string[] goToProtoType = new string[11];
    private string[] goToDwellingTimber = new string[11];
    [Header("Journal Codes")]
    private string[] giveAllJournals = new string[15];
    private string[] giveHistory1 = new string[12];
    private string[] giveHistory2 = new string[12];
    private string[] giveHistory3 = new string[12];
    private string[] giveHistory4 = new string[12];
    private string[] giveHistory5 = new string[12];
    private string[] giveHistory6 = new string[12];
    private string[] giveDiscovery1 = new string[14];
    private string[] giveDiscovery2 = new string[14];
    private string[] giveCodeBook1 = new string[13];
    private string[] giveCodeBook2 = new string[13];
    [Header("Teleport Codes")]
    private string[] goToDuskCliff = new string[11];
    private string[] goToWindAcre = new string[11];

    int debugIndex;

    int giveAllIndex;
    int loseAllIndex;
    int beatGameIndex;
    int resetGameIndex;
    int invincOnIndex;
    int invincOffIndex;
    int maxSwordOnIndex;
    int maxSwordOffIndex;

    int allSwordIndex;
    int flareIndex;
    int lightningIndex;
    int frostIndex;
    int terraIndex;

    int allArmorIndex;
    int helixIndex;
    int apexIndex;
    int mythicIndex;
    int legendaryIndex;

    int allBootIndex;
    int hoverIndex;
    int cinderIndex;
    int stormIndex;
    int medicalIndex;

    int allJewelIndex;
    int redIndex;
    int blueIndex;
    int greenIndex;
    int yellowIndex;
    int purpleIndex;

    int allItemIndex;
    int lifeberryIndex;
    int rixileIndex;
    int nicirplantIndex;
    int mindremedyIndex;
    int petrishroomIndex;
    int reliefointmentIndex;
    int magicidolIndex;
    int terraidolIndex;
    int keyIndex;
    int demonmaskIndex;
    int moonpearlIndex;
    int markofetymologyIndex;

    int allEffectsOnIndex;
    int allEffectsOffIndex;
    int poisonMinOnIndex;
    int poisonMinOffIndex;
    int poisonMaxOnIndex;
    int poisonMaxOffIndex;
    int confusedOnIndex;
    int confusedOffIndex;
    int silencedOnIndex;
    int silencedOffIndex;
    int paralyzedOnIndex;
    int paralyzedOffIndex;

    int maxHealthIndex;
    int setHealthIndex;
    int maxVitalityIndex;
    int setVitalityIndex;
    int maxAirIndex;
    int maxGoldIndex;
    int setGoldIndex;
    int setSwimmingIndex;
    int setCameraIndex;
    int killIndex;
    int savePosIndex;
    int loadPosIndex;

    int shopDuskCliffIndex;
    int shopWindAcreIndex;
    int shopSkulkCoveIndex;

    int titleIndex;
    int introIndex;
    int overIndex;
    int dungeon1Index;
    int dungeon2Index;
    int temple1Index;
    int ghostIndex;
    int prototypeIndex;
    int dwellingIndex;
    int dungeon3Index;

    int allJournalsIndex;
    int discoveryJournal1Index;
    int discoveryJournal2Index;
    int historyJournal1Index;
    int historyJournal2Index;
    int historyJournal3Index;
    int historyJournal4Index;
    int historyJournal5Index;
    int historyJournal6Index;
    int codeJournal1Index;
    int codeJournal2Index;


    public static bool isDebugMode;
    bool healthMode;
    bool CodeAdded;
    bool vitalityMode;
    public static bool goldMode;
    bool cameraMode;
    bool swimmingMode;
    bool isInvincible;
    public static bool toggleCodeList;

    public string modeText;
    public Text statusText;
    Vector3 textOrgPos;
    public GameObject CodeList;
    IEnumerator statusRoutine = null;
    IEnumerator audioRoutine = null;
    IEnumerator startRoutine = null;
    public Font curFont;
    string fpsLabel = "";
    string screenSize = "";
    string date = "";
    float count;
    private GUIStyle guiStyle = new GUIStyle();
    private GUIStyle guiStyle2 = new GUIStyle();
    private GUIStyle guiStyle3 = new GUIStyle();
    private GUIStyle guiStyle4 = new GUIStyle();
    private GUIStyle guiStyle5 = new GUIStyle();
    private GUIStyle guiStyle6 = new GUIStyle();

    string TimeofDay;
    string[] weekday  = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
    string weather;
    string totalPlayTime;
    public string debugText;
    int gameSeconds;
    int gameMinutes;
    int gameHours;
    int gameDays;

    
    IEnumerator StartGUI()
    {
        GUI.depth = 2;
        while (true)
        {
            if (Time.timeScale == 1)
            {
                yield return new WaitForSeconds(0.1f);
                count = (1 / Time.deltaTime);
                fpsLabel = "Frames Per Second: " + (Mathf.Round(count));
            }
            else
            {
                fpsLabel = "Pause";
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    void OnGUI()
    {
        if (isDebugMode && !cameraMode)
        {
            //System
            SetTime(TimeSystem.totalGameTime);
            screenSize = "Current Resolution: " + Screen.width + " x " + Screen.height.ToString();
            date = "Date: " + System.DateTime.Now.ToString("HH:mm dd MMMM, yyyy");
            guiStyle.font = curFont;
            guiStyle.fontSize = 15;
            guiStyle.normal.textColor = Color.green;
            guiStyle.alignment = TextAnchor.LowerRight;
            GUI.Label(new Rect(-20, -30, Screen.width, Screen.height), fpsLabel, guiStyle);
            GUI.Label(new Rect(-20, -50, Screen.width, Screen.height), screenSize, guiStyle);
            GUI.Label(new Rect(-20, -70, Screen.width, Screen.height), date, guiStyle);
            GUI.Label(new Rect(-20, -90, Screen.width, Screen.height), "[System Settings]", guiStyle);

            //Game
            guiStyle2.font = curFont;
            guiStyle2.fontSize = 15;
            guiStyle2.normal.textColor = Color.white;
            guiStyle2.alignment = TextAnchor.LowerLeft;
            TimeofDay = string.Format("Time of Day: " + "{0:0}:{1:00}", TimeSystem.hour, TimeSystem.minute) + " " + TimeSystem.dayNightDisplay;
            if (TimeSystem.isRain)
                weather = string.Format("Weather: " + "Rain");
            else
                weather = string.Format("Weather: " + "Clear");
            GUI.Label(new Rect(20, -110, Screen.width, Screen.height), "[Game Settings]", guiStyle2);
            GUI.Label(new Rect(20, -90, Screen.width, Screen.height), TimeofDay, guiStyle2);
            GUI.Label(new Rect(20, -70, Screen.width, Screen.height), weather, guiStyle2);
            GUI.Label(new Rect(20, -50, Screen.width, Screen.height), "Day of Week: " + weekday[TimeSystem.day], guiStyle2);
            GUI.Label(new Rect(20, -30, Screen.width, Screen.height), "Total Game Time: " + totalPlayTime, guiStyle2);

            //Debug
            debugText = "Debug Mode (Press [F1] For code List)";
            guiStyle3.font = curFont;
            guiStyle3.fontSize = 15;
            guiStyle3.normal.textColor = new Color(0, 1, 0, Mathf.PingPong(Time.time, 1));
            guiStyle3.alignment = TextAnchor.LowerCenter;
            GUI.Label(new Rect(0, -30, Screen.width, Screen.height), debugText, guiStyle3);

           
           if(healthMode && !vitalityMode && !goldMode && !cameraMode)
            {
                modeText = "Health Mode ([Equals] to Increase, [Minus] to Decrease, [BackSpace] to Quit)";
                guiStyle4.font = curFont;
                guiStyle4.fontSize = 15;
                guiStyle4.normal.textColor = new Color(1, 0, 0, Mathf.PingPong(Time.time, 1));
                guiStyle4.alignment = TextAnchor.LowerCenter;
                GUI.Label(new Rect(0, -50, Screen.width, Screen.height), modeText, guiStyle4);
            }
           else if(!healthMode && vitalityMode && !goldMode && !cameraMode)
            {
                modeText = "Vitality Mode ([Equals] to Increase, [Minus] to Decrease, [BackSpace] to Quit)";
                guiStyle5.font = curFont;
                guiStyle5.fontSize = 15;
                guiStyle5.normal.textColor = new Color(0, 0, 1, Mathf.PingPong(Time.time, 1));
                guiStyle5.alignment = TextAnchor.LowerCenter;
                GUI.Label(new Rect(0, -50, Screen.width, Screen.height), modeText, guiStyle5);
            }
            else if (!healthMode && !vitalityMode && goldMode && !cameraMode)
            {
                modeText = "Gold Mode ([Equals] to Increase, [Minus] to Decrease, [BackSpace] to Quit)";
                guiStyle5.font = curFont;
                guiStyle5.fontSize = 15;
                guiStyle5.normal.textColor = new Color(1, 0.92f, 0.016f, Mathf.PingPong(Time.time, 1));
                guiStyle5.alignment = TextAnchor.LowerCenter;
                GUI.Label(new Rect(0, -50, Screen.width, Screen.height), modeText, guiStyle5);
            }
        }
    }
    public void SetTime(float amount)
    {
        gameSeconds = Mathf.FloorToInt(amount % 60);
        gameMinutes = Mathf.FloorToInt((amount / 60) % 60);
        gameHours = Mathf.FloorToInt((amount / 3600f) % 24);
        gameDays = Mathf.FloorToInt(amount / 86400);
        totalPlayTime = string.Format("({0:00}:{1:00}:{2:00}:{3:00})", gameDays, gameHours, gameMinutes, gameSeconds);

    }
    void Start()
    {
        startRoutine = StartGUI();
        StartCoroutine(startRoutine);
        sceneSystem = GameObject.Find("Core/Player").GetComponent<SceneSystem>();
        underWaterSystem = GameObject.FindGameObjectWithTag("Head").GetComponent<UnderWaterSystem>();
        objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
        playerSystem = GetComponent<PlayerSystem>();
        shopSystem = GetComponent<ShopSystem>();
        swordSystem = GetComponent<SwordSystem>();
        itemSystem = GetComponent<ItemSystem>();
        jewelSystem = GetComponent<JewelSystem>();
        characterSystem = GetComponent<CharacterSystem>();
        storySystem = GetComponent<StorySystem>();
        debugCode = new string[7] { "t", "q", "d", "e", "b", "u", "g" };

        giveEverything = new string[7] { "c", "h", "e", "a","t", "e", "r" };
        loseEverything = new string[7] { "l", "o", "y", "a","l","t","y" };
        beatEverything = new string[7] { "e", "n", "d", "g","a","m","e" };
        resetEverything = new string[9] { "s", "t", "a", "r","t","o","v", "e", "r" };
        invincibilityOn = new string[12] { "i", "n", "v", "i", "n", "c", "i" , "b", "l", "e", "o", "n" };
        invincibilityOff = new string[13] { "i", "n", "v", "i", "n", "c", "i" , "b", "l", "e", "o", "f", "f" };
        maxedOutSwordOn = new string[13] { "s", "w", "o", "r", "d", "m", "a", "s", "t", "e", "r", "o", "n" };
        maxedOutSwordOff = new string[14] { "s", "w", "o", "r", "d", "m", "a", "s", "t", "e", "r", "o", "f", "f" };

        giveAllSwords = new string[9] { "a", "l", "l", "s", "w", "o", "r", "d", "s" };
        giveFlareSword = new string[10] { "f", "l", "a", "r", "e", "s", "w", "o", "r", "d" };
        giveLightningSword = new string[14] { "l", "i", "g", "h", "t", "n", "i", "n", "g", "s", "w", "o", "r", "d" };
        giveFrostSword = new string[10] { "f", "r", "o", "s", "t", "s", "w", "o", "r", "d" };
        giveTerraSword = new string[10] { "t", "e", "r", "r", "a", "s", "w", "o", "r", "d" };

        giveAllArmor = new string[8] { "a", "l", "l", "a", "r", "m", "o", "r" };
        giveHelixArmor = new string[10] { "h", "e", "l", "i", "x", "a", "r", "m", "o", "r" };
        giveApexArmor = new string[9] { "a", "p", "e", "x", "a", "r", "m", "o", "r" };
        giveMythicArmor = new string[11] { "m", "y", "t", "h", "i", "c", "a", "r", "m", "o", "r" };
        giveLegendaryArmor = new string[14] { "l", "e", "g", "e", "n", "d", "a", "r", "y", "a", "r", "m", "o", "r" };

        giveAllBoots = new string[8] { "a", "l", "l", "b", "o", "o", "t", "s" };
        giveHoverBoots = new string[9] { "h", "o", "v", "e", "r", "b", "o", "o", "t" };
        giveCinderBoots = new string[10] { "c", "i", "n", "d", "e", "r", "b", "o", "o", "t" };
        giveStormBoots = new string[9] { "s", "t", "o", "r", "m", "b", "o", "o", "t" };
        giveMedicalBoots = new string[11] { "m", "e", "d", "i", "c", "a", "l", "b", "o", "o", "t" };

        giveAllJewels = new string[9] { "a", "l", "l", "j", "e", "w", "e", "l", "s" };
        giveRedJewel = new string[8] { "r", "e", "d", "j", "e", "w", "e", "l" };
        giveBlueJewel = new string[9] { "b", "l", "u", "e", "j", "e", "w", "e", "l" };
        giveGreenJewel = new string[10] { "g", "r", "e", "e", "n", "j", "e", "w", "e", "l" };
        giveYellowJewel = new string[11] { "y", "e", "l", "l", "o", "w", "j", "e", "w", "e", "l" };
        givePurpleJewel = new string[11] { "p", "u", "r", "p", "l", "e", "j", "e", "w", "e", "l" };

        giveAllJournals = new string[9] { "s", "t", "o", "r", "y", "t", "i", "m", "e" };
        giveHistory1 = new string[3] { "a", "h", "0" };
        giveHistory2 = new string[3] { "a", "h", "1" };
        giveHistory3 = new string[3] { "a", "h", "2" };
        giveHistory4 = new string[3] { "a", "h", "3" };
        giveHistory5 = new string[3] { "a", "h", "4" };
        giveHistory6 = new string[3] { "a", "h", "5" };
        giveDiscovery1 = new string[3] { "i", "d", "0" };
        giveDiscovery2 = new string[3] { "i", "d", "1" };
        giveCodeBook1 = new string[3] { "d", "s", "0" };
        giveCodeBook2 = new string[3] { "d", "s", "1" };

        giveAllItems = new string[8] { "a", "l", "l", "i", "t", "e", "m", "s" };
        giveLifeBerry = new string[9] { "l", "i", "f", "e", "b", "e", "r", "r", "y" };
        giveRixile = new string[6] { "r", "i", "x", "i", "l", "e"};
        giveNicirPlant = new string[10] { "n", "i", "c", "i", "r", "p", "l", "a", "n", "t" };
        giveMindRemedy = new string[10] { "m", "i", "n", "d", "r", "e", "m", "e", "d", "y" };
        givePetrishroom = new string[11] { "p", "e", "t", "r", "i", "s", "h", "r", "o", "o", "m" };
        giveReliefOintment = new string[14] { "r", "e", "l", "i", "e", "f", "o", "i", "n", "t", "m", "e" ,"n" ,"t" };
        giveMagicIdol = new string[9] { "m", "a", "g", "i", "c", "i", "d", "o", "l" };
        giveTerraIdol = new string[9] { "t", "e", "r", "r", "a", "i", "d", "o", "l" };
        giveKey = new string[3] {"k", "e", "y"};
        giveDemonMask = new string[9] {"d", "e", "m", "o", "n", "m", "a", "s", "k" };
        giveMoonPearl = new string[9] {"m", "o", "o", "n", "p", "e", "a", "r", "l" };
        giveMarkOfEtymology = new string[15] { "m", "a", "r", "k", "o", "f", "e", "t", "y", "m", "o", "l", "o", "g", "y" };

        allEffectsOn = new string[12] { "a", "l", "l", "e", "f", "f", "e", "c", "t", "s", "o", "n" };
        allEffectsOff = new string[13] { "a", "l", "l", "e", "f", "f", "e", "c", "t", "s", "o", "f", "f" };
        addPoisonMinStatusEffect = new string[11] { "p", "o", "i", "s", "o", "n", "m", "i", "n", "o", "n"};
        removePoisonMinStatusEffect = new string[12] { "p", "o", "i", "s", "o", "n", "m", "i", "n", "o", "f", "f" };
        addPoisonMaxStatusEffect = new string[11] { "p", "o", "i", "s", "o", "n", "m", "a", "x", "o", "n" };
        removePoisonMaxStatusEffect = new string[12] { "p", "o", "i", "s", "o", "n", "m", "a", "x", "o", "f", "f" };
        addConfusedStatusEffect = new string[10] { "c", "o", "n", "f", "u", "s", "e", "d", "o", "n" };
        removeConfusedStatusEffect = new string[11] { "c", "o", "n", "f", "u", "s", "e", "d", "o", "f", "f" };
        addSilencedStatusEffect = new string[10] { "s", "i", "l", "e", "n", "c", "e", "d", "o", "n" };
        removeSilencedStatusEffect = new string[11] { "s", "i", "l", "e", "n", "c", "e", "d", "o", "f", "f" };
        addParalyzedStatusEffect = new string[11] { "p", "a", "r", "a", "l", "y", "z", "e", "d", "o", "n" };
        removeParalyzedStatusEffect = new string[12] { "p", "a", "r", "a", "l", "y", "z", "e", "d", "o", "f", "f" };

        addMaxHealth = new string[9] { "m", "a", "x", "h", "e", "a", "l", "t", "h" };
        setHealth = new string[9] { "s", "e", "t", "h", "e", "a", "l", "t", "h" };
        addMaxVitality = new string[11] { "m", "a", "x", "v", "i", "t", "a", "l", "i", "t", "y" };
        setVitality = new string[11] { "s", "e", "t", "v", "i", "t", "a", "l", "i", "t", "y" };
        addMaxGold = new string[7] { "m", "a", "x", "g", "o", "l", "d" };
        addMaxAir =  new string[6] { "m", "a", "x", "a", "i", "r" };
        setGold = new string[7] { "s", "e", "t", "g", "o", "l", "d" };
        setCamera = new string[6] { "c", "a", "m", "e", "r", "a" };
        setSwimming = new string[8] { "s", "w", "i", "m", "m", "i", "n", "g" };
        killPlayer = new string[4] { "k", "i", "l", "l" };
        savePlayerPos = new string[12] { "s", "a", "v", "e", "p", "o", "s", "i", "t", "i", "o", "n" };
        loadPlayerPos = new string[12] { "l", "o", "a", "d", "p", "o", "s", "i", "t", "i", "o", "n" };

        openShopDuskCliff = new string[13] { "s", "h", "o", "p", "d", "u", "s", "k", "c", "l", "i", "f", "f" };
        openShopWindAcre = new string[12] { "s", "h", "o", "p", "w", "i", "n", "d", "a", "c", "r", "e" };
        openShopSkulkCove = new string[13] { "s", "h", "o", "p", "s", "k", "u", "l", "k", "c", "o", "v", "e" };

        goToTitle = new string[10] { "l", "o", "a", "d", "s", "c", "e", "n", "e", "0" };
        goToIntro = new string[10] { "l", "o", "a", "d", "s", "c", "e", "n", "e", "6" };
        goToOverWorld = new string[10] { "l", "o", "a", "d", "s", "c", "e", "n", "e", "2" };
        goToDungeon1 = new string[10] { "l", "o", "a", "d", "s", "c", "e", "n", "e", "1" };
        goToDungeon2 = new string[10] { "l", "o", "a", "d", "s", "c", "e", "n", "e", "4" };
        goToDungeon3 = new string[10] { "l", "o", "a", "d", "s", "c", "e", "n", "e", "9" };
        goToGhostShip = new string[10] { "l", "o", "a", "d", "s", "c", "e", "n", "e", "3" };
        goToTemple1 = new string[10] { "l", "o", "a", "d", "s", "c", "e", "n", "e", "5" };
        goToProtoType = new string[10] { "l", "o", "a", "d", "s", "c", "e", "n", "e", "7" };
        goToDwellingTimber = new string[10] { "l", "o", "a", "d", "s", "c", "e", "n", "e", "8" };



        statusText.text = null;
        textOrgPos = statusText.GetComponent<RectTransform>().anchoredPosition;
    }
    private void Update()
    {
        if (isInvincible)
            Physics.IgnoreLayerCollision(1, 9);
    }
    public void ResetAllIndexes()
    {
        debugIndex = 0;
        giveAllIndex = 0;
        loseAllIndex = 0;
        beatGameIndex = 0;
        resetGameIndex = 0;
        invincOnIndex = 0;
        invincOffIndex = 0;
        maxSwordOnIndex = 0;
        maxSwordOffIndex = 0;
        allSwordIndex = 0;
        flareIndex = 0;
        lightningIndex = 0;
        frostIndex = 0;
        terraIndex = 0;
        allArmorIndex = 0;
        helixIndex = 0;
        apexIndex = 0;
        mythicIndex = 0;
        legendaryIndex = 0;
        allBootIndex = 0;
        hoverIndex = 0;
        cinderIndex = 0;
        stormIndex = 0;
        medicalIndex = 0;

        allJewelIndex = 0;
        redIndex = 0;
        blueIndex = 0;
        greenIndex = 0;
        yellowIndex = 0;
        purpleIndex = 0;

        allItemIndex = 0;
        lifeberryIndex = 0;
        rixileIndex = 0;
        nicirplantIndex = 0;
        mindremedyIndex = 0;
        petrishroomIndex = 0;
        reliefointmentIndex = 0;
        magicidolIndex = 0;
        terraidolIndex = 0;
        keyIndex = 0;
        demonmaskIndex = 0;
        moonpearlIndex = 0;
        markofetymologyIndex = 0;

        allEffectsOnIndex = 0;
        allEffectsOffIndex = 0;
        poisonMinOnIndex = 0;
        poisonMinOffIndex = 0;
        poisonMaxOnIndex = 0;
        poisonMaxOffIndex = 0;
        confusedOnIndex = 0;
        confusedOffIndex = 0;
        silencedOnIndex = 0;
        silencedOffIndex = 0;
        paralyzedOnIndex = 0;
        paralyzedOffIndex = 0;

        allJournalsIndex = 0;
        discoveryJournal1Index = 0;
        discoveryJournal2Index = 0;
        historyJournal1Index = 0;
        historyJournal2Index = 0;
        historyJournal3Index = 0;
        historyJournal4Index = 0;
        historyJournal5Index = 0;
        historyJournal6Index = 0;
        codeJournal1Index = 0;
        codeJournal2Index = 0;

        maxHealthIndex = 0;
        setHealthIndex = 0;
        maxVitalityIndex = 0;
        setVitalityIndex = 0;
        maxAirIndex = 0;
        maxGoldIndex = 0;
        setGoldIndex = 0;
        setSwimmingIndex = 0;
        setCameraIndex = 0;
        killIndex = 0;
        savePosIndex = 0;
        loadPosIndex = 0;

        shopDuskCliffIndex = 0;
        shopWindAcreIndex = 0;
        shopSkulkCoveIndex = 0;

        titleIndex = 0;
        introIndex = 0;
        overIndex = 0;
        dungeon1Index = 0;
        dungeon2Index = 0;
        temple1Index = 0;
        ghostIndex = 0;
        prototypeIndex = 0;
        dwellingIndex = 0;
        dungeon3Index = 0;
    }
    public void TurnOffSwimmingMode()
    {
        swimmingMode = false;
    }
    void LateUpdate()
    {
        if (UnderWaterSystem.isSwimming && !swimmingMode)
            swimmingMode = true;
        //else if (!UnderWaterSystem.isSwimming && swimmingMode)
        //    swimmingMode = false;

        if (Input.anyKeyDown && !isDebugMode && !SceneSystem.inTransition)
        {
            if (Input.GetKeyDown(debugCode[debugIndex]))
            {
                debugIndex++;
                if (debugIndex == debugCode.Length)
                {
                    AudioSystem.PlayAudioSource(soundFx[0], 1, 1);
                    isDebugMode = true;
                    sceneSystem.SetPlaceName(SceneSystem.Place.debug, null);
                    ResetAllIndexes();
                }
            }
            else
                debugIndex = 0;
        }
        
        else if(Input.anyKeyDown && isDebugMode && !healthMode && !vitalityMode && !goldMode && !cameraMode && !swimmingMode && !SceneSystem.inTransition)
        {
            if (Input.GetKeyDown(debugCode[debugIndex]))
            {
                debugIndex++;
                if (debugIndex >= debugCode.Length)
                {
                    ShutOffDebugMode();
                    ResetAllIndexes();
                }
            }
            else
                debugIndex = 0;
            //============================CodeList=============================//
            if (Input.GetKeyDown(KeyCode.F1) && !toggleCodeList)
            {
                EnableMouse(true);
                CodeList.SetActive(true);
                toggleCodeList = true;
                sceneSystem.EnablePlayer(false);
                AudioSystem.PlayAudioSource(soundFx[73], 1, 1);
                ResetAllIndexes();
            }
            else if (Input.GetKeyDown(KeyCode.F1) && toggleCodeList)
            {
                EnableMouse(false);
                sceneSystem.EnablePlayer(true);
                CodeList.SetActive(false);
                toggleCodeList = false;
                AudioSystem.PlayAudioSource(soundFx[74], 1, 1);
            }

            //============================Master================================//
            if (Input.GetKeyDown(giveEverything[giveAllIndex]))
            {
                giveAllIndex++;
                if (giveAllIndex == giveEverything.Length)
                    EnableCode(Code.Everything);
            }
            else
                giveAllIndex = 0;
            if (Input.GetKeyDown(loseEverything[loseAllIndex]))
            {
                loseAllIndex++;
                if (loseAllIndex == loseEverything.Length)
                    EnableCode(Code.Nothing);
            }
            else
                loseAllIndex = 0;
            if (Input.GetKeyDown(beatEverything[beatGameIndex]))
            {
                beatGameIndex++;
                if (beatGameIndex == beatEverything.Length)
                    EnableCode(Code.BeatGame);
            }
            else
                beatGameIndex = 0;
            if (Input.GetKeyDown(resetEverything[resetGameIndex]))
            {
                resetGameIndex++;
                if (resetGameIndex == resetEverything.Length)
                    EnableCode(Code.ResetGame);
            }
            else
                resetGameIndex = 0;
            if (Input.GetKeyDown(invincibilityOn[invincOnIndex]))
            {
                invincOnIndex++;
                if (invincOnIndex == invincibilityOn.Length)
                    EnableCode(Code.InvincibleOn);
            }
            else
                invincOnIndex = 0;
            if (Input.GetKeyDown(invincibilityOff[invincOffIndex]))
            {
                invincOffIndex++;
                if (invincOffIndex == invincibilityOff.Length)
                    EnableCode(Code.InvincibleOff);
            }
            else
                invincOffIndex = 0;
            if (Input.GetKeyDown(maxedOutSwordOn[maxSwordOnIndex]))
            {
                maxSwordOnIndex++;
                if (maxSwordOnIndex == maxedOutSwordOn.Length)
                    EnableCode(Code.MaxedOutSwordOn);
            }
            else
                maxSwordOnIndex = 0;
            if (Input.GetKeyDown(maxedOutSwordOff[maxSwordOffIndex]))
            {
                maxSwordOffIndex++;
                if (maxSwordOffIndex == maxedOutSwordOff.Length)
                    EnableCode(Code.MaxedOutSwordOff);
            }
            else
                maxSwordOffIndex = 0;
            //============================Swords================================//
            if (Input.GetKeyDown(giveAllSwords[allSwordIndex]))
            {
                allSwordIndex++;
                if (allSwordIndex == giveAllSwords.Length)
                    EnableCode(Code.AllSwords);
            }
            else
                allSwordIndex = 0;
            if (Input.GetKeyDown(giveFlareSword[flareIndex]))
            {
                flareIndex++;
                if (flareIndex == giveFlareSword.Length)
                    EnableCode(Code.FlareSword);
            }
            else
                flareIndex = 0;
            if (Input.GetKeyDown(giveLightningSword[lightningIndex]))
            {
                lightningIndex++;
                if (lightningIndex == giveLightningSword.Length)
                    EnableCode(Code.LightningSword);
            }
            else
                lightningIndex = 0;
            if (Input.GetKeyDown(giveFrostSword[frostIndex]))
            {
                frostIndex++;
                if (frostIndex == giveFrostSword.Length)
                    EnableCode(Code.FrostSword);
            }
            else
                frostIndex = 0;
            if (Input.GetKeyDown(giveTerraSword[terraIndex]))
            {
                terraIndex++;
                if (terraIndex == giveTerraSword.Length)
                    EnableCode(Code.TerraSword);
            }
            else
                terraIndex = 0;
            //============================Armor================================//
            if (Input.GetKeyDown(giveAllArmor[allArmorIndex]))
            {
                allArmorIndex++;
                if (allArmorIndex == giveAllArmor.Length)
                    EnableCode(Code.AllArmor);
            }
            else
                allArmorIndex = 0;
            if (Input.GetKeyDown(giveHelixArmor[helixIndex]))
            {
                helixIndex++;
                if (helixIndex == giveHelixArmor.Length)
                    EnableCode(Code.HelixArmor);
            }
            else
                helixIndex = 0;
            if (Input.GetKeyDown(giveApexArmor[apexIndex]))
            {
                apexIndex++;
                if (apexIndex == giveApexArmor.Length)
                    EnableCode(Code.ApexArmor);
            }
            else
                apexIndex = 0;
            if (Input.GetKeyDown(giveMythicArmor[mythicIndex]))
            {
                mythicIndex++;
                if (mythicIndex == giveMythicArmor.Length)
                    EnableCode(Code.MythicArmor);
            }
            else
                mythicIndex = 0;
            if (Input.GetKeyDown(giveLegendaryArmor[legendaryIndex]))
            {
                legendaryIndex++;
                if (legendaryIndex == giveLegendaryArmor.Length)
                    EnableCode(Code.LegendaryArmor);
            }
            else
                legendaryIndex = 0;
            //============================Boots================================//
            if (Input.GetKeyDown(giveAllBoots[allBootIndex]))
            {
                allBootIndex++;
                if (allBootIndex == giveAllBoots.Length)
                    EnableCode(Code.AllBoots);
            }
            else
                allBootIndex = 0;
            if (Input.GetKeyDown(giveHoverBoots[hoverIndex]))
            {
                hoverIndex++;
                if (hoverIndex == giveHoverBoots.Length)
                    EnableCode(Code.HoverBoots);
            }
            else
                hoverIndex = 0;
            if (Input.GetKeyDown(giveCinderBoots[cinderIndex]))
            {
                cinderIndex++;
                if (cinderIndex == giveCinderBoots.Length)
                    EnableCode(Code.CinderBoots);
            }
            else
                cinderIndex = 0;
            if (Input.GetKeyDown(giveStormBoots[stormIndex]))
            {
                stormIndex++;
                if (stormIndex == giveStormBoots.Length)
                    EnableCode(Code.StormBoots);
            }
            else
                stormIndex = 0;
            if (Input.GetKeyDown(giveMedicalBoots[medicalIndex]))
            {
                medicalIndex++;
                if (medicalIndex == giveMedicalBoots.Length)
                    EnableCode(Code.MedicalBoots);
            }
            else
                medicalIndex = 0;
            //============================Jewels================================//
            if (Input.GetKeyDown(giveAllJewels[allJewelIndex]))
            {
                allJewelIndex++;
                if (allJewelIndex == giveAllJewels.Length)
                    EnableCode(Code.AllJewels);
            }
            else
                allJewelIndex = 0;
            if (Input.GetKeyDown(giveRedJewel[redIndex]))
            {
                redIndex++;
                if (redIndex == giveRedJewel.Length)
                    EnableCode(Code.RedJewel);
            }
            else
                redIndex = 0;
            if (Input.GetKeyDown(giveBlueJewel[blueIndex]))
            {
                blueIndex++;
                if (blueIndex == giveBlueJewel.Length)
                    EnableCode(Code.BlueJewel);
            }
            else
                blueIndex = 0;
            if (Input.GetKeyDown(giveGreenJewel[greenIndex]))
            {
                greenIndex++;
                if (greenIndex == giveGreenJewel.Length)
                    EnableCode(Code.GreenJewel);
            }
            else
                greenIndex = 0;
            if (Input.GetKeyDown(giveYellowJewel[yellowIndex]))
            {
                yellowIndex++;
                if (yellowIndex == giveYellowJewel.Length)
                    EnableCode(Code.YellowJewel);
            }
            else
                yellowIndex = 0;
            if (Input.GetKeyDown(givePurpleJewel[purpleIndex]))
            {
                purpleIndex++;
                if (purpleIndex == givePurpleJewel.Length)
                    EnableCode(Code.PurpleJewel);
            }
            else
                purpleIndex = 0;
            //============================Items================================//
            if (Input.GetKeyDown(giveAllItems[allItemIndex]))
            {
                allItemIndex++;
                if (allItemIndex == giveAllItems.Length)
                    EnableCode(Code.AllItems);
            }
            else
                allItemIndex = 0;
            if (Input.GetKeyDown(giveLifeBerry[lifeberryIndex]))
            {
                lifeberryIndex++;
                if (lifeberryIndex == giveLifeBerry.Length)
                    EnableCode(Code.LifeBerry);
            }
            else
                lifeberryIndex = 0;
            if (Input.GetKeyDown(giveRixile[rixileIndex]))
            {
                rixileIndex++;
                if (rixileIndex == giveRixile.Length)
                    EnableCode(Code.Rixile);
            }
            else
                rixileIndex = 0;
            if (Input.GetKeyDown(giveNicirPlant[nicirplantIndex]))
            {
                nicirplantIndex++;
                if (nicirplantIndex == giveNicirPlant.Length)
                    EnableCode(Code.NicirPlant);
            }
            else
                nicirplantIndex = 0;
            if (Input.GetKeyDown(giveMindRemedy[mindremedyIndex]))
            {
                mindremedyIndex++;
                if (mindremedyIndex == giveMindRemedy.Length)
                    EnableCode(Code.MindRemedy);
            }
            else
                mindremedyIndex = 0;
            if (Input.GetKeyDown(givePetrishroom[petrishroomIndex]))
            {
                petrishroomIndex++;
                if (petrishroomIndex == givePetrishroom.Length)
                    EnableCode(Code.PetriShroom);
            }
            else
                petrishroomIndex = 0;
            if (Input.GetKeyDown(giveReliefOintment[reliefointmentIndex]))
            {
                reliefointmentIndex++;
                if (reliefointmentIndex == giveReliefOintment.Length)
                    EnableCode(Code.ReliefOinment);
            }
            else
                reliefointmentIndex = 0;
            if (Input.GetKeyDown(giveTerraIdol[terraidolIndex]))
            {
                terraidolIndex++;
                if (terraidolIndex == giveTerraIdol.Length)
                    EnableCode(Code.TerraIdol);
            }
            else
                terraidolIndex = 0;
            if (Input.GetKeyDown(giveMagicIdol[magicidolIndex]))
            {
                magicidolIndex++;
                if (magicidolIndex == giveMagicIdol.Length)
                    EnableCode(Code.MagicIdol);
            }
            else
                magicidolIndex = 0;
            if (Input.GetKeyDown(giveKey[keyIndex]))
            {
                keyIndex++;
                if (keyIndex == giveKey.Length)
                    EnableCode(Code.Key);
            }
            else
                keyIndex = 0;
            if (Input.GetKeyDown(giveDemonMask[demonmaskIndex]))
            {
                demonmaskIndex++;
                if (demonmaskIndex == giveDemonMask.Length)
                    EnableCode(Code.DemonMask);
            }
            else
                demonmaskIndex = 0;
            if (Input.GetKeyDown(giveMoonPearl[moonpearlIndex]))
            {
                moonpearlIndex++;
                if (moonpearlIndex == giveMoonPearl.Length)
                    EnableCode(Code.MoonPearl);
            }
            else
                moonpearlIndex = 0;
            if (Input.GetKeyDown(giveMarkOfEtymology[markofetymologyIndex]))
            {
                markofetymologyIndex++;
                if (markofetymologyIndex == giveMarkOfEtymology.Length)
                    EnableCode(Code.MarkOfEtymology);
            }
            else
                markofetymologyIndex = 0;
            //============================JournalEntries================================//
            if (Input.GetKeyDown(giveAllJournals[allJournalsIndex]))
            {
                allJournalsIndex++;
                if (allJournalsIndex == giveAllJournals.Length)
                    EnableCode(Code.AllJournals);
            }
            else
                allJournalsIndex = 0;
            if (Input.GetKeyDown(giveHistory1[historyJournal1Index]))
            {
                historyJournal1Index++;
                if (historyJournal1Index == giveHistory1.Length)
                    EnableCode(Code.History1);
            }
            else
                historyJournal1Index = 0;
            if (Input.GetKeyDown(giveHistory2[historyJournal2Index]))
            {
                historyJournal2Index++;
                if (historyJournal2Index == giveHistory2.Length)
                    EnableCode(Code.History2);
            }
            else
                historyJournal2Index = 0;
            if (Input.GetKeyDown(giveHistory3[historyJournal3Index]))
            {
                historyJournal3Index++;
                if (historyJournal3Index == giveHistory3.Length)
                    EnableCode(Code.History3);
            }
            else
                historyJournal3Index = 0;
            if (Input.GetKeyDown(giveHistory4[historyJournal4Index]))
            {
                historyJournal4Index++;
                if (historyJournal4Index == giveHistory4.Length)
                    EnableCode(Code.History4);
            }
            else
                historyJournal4Index = 0;
            if (Input.GetKeyDown(giveHistory5[historyJournal5Index]))
            {
                historyJournal5Index++;
                if (historyJournal5Index == giveHistory5.Length)
                    EnableCode(Code.History5);
            }
            else
                historyJournal5Index = 0;
            if (Input.GetKeyDown(giveHistory6[historyJournal6Index]))
            {
                historyJournal6Index++;
                if (historyJournal6Index == giveHistory6.Length)
                    EnableCode(Code.History6);
            }
            else
                historyJournal6Index = 0;
            if (Input.GetKeyDown(giveDiscovery1[discoveryJournal1Index]))
            {
                discoveryJournal1Index++;
                if (discoveryJournal1Index == giveDiscovery1.Length)
                    EnableCode(Code.Discovery1);
            }
            else
                discoveryJournal1Index = 0;
            if (Input.GetKeyDown(giveDiscovery2[discoveryJournal2Index]))
            {
                discoveryJournal2Index++;
                if (discoveryJournal2Index == giveDiscovery2.Length)
                    EnableCode(Code.Discovery2);
            }
            else
                discoveryJournal2Index = 0;
            if (Input.GetKeyDown(giveCodeBook1[codeJournal1Index]))
            {
                codeJournal1Index++;
                if (codeJournal1Index == giveCodeBook1.Length)
                    EnableCode(Code.CodeBook1);
            }
            else
                codeJournal1Index = 0;
            if (Input.GetKeyDown(giveCodeBook2[codeJournal2Index]))
            {
                codeJournal2Index++;
                if (codeJournal2Index == giveCodeBook2.Length)
                    EnableCode(Code.CodeBook2);
            }
            else
                codeJournal2Index = 0;
            //============================Effects================================//

            //===All
            if (Input.GetKeyDown(allEffectsOn[allEffectsOnIndex]))
            {
                allEffectsOnIndex++;
                if (allEffectsOnIndex == allEffectsOn.Length)
                    EnableCode(Code.AllEffectsOn);
            }
            else
                allEffectsOnIndex = 0;
            if (Input.GetKeyDown(allEffectsOff[allEffectsOffIndex]))
            {
                allEffectsOffIndex++;
                if (allEffectsOffIndex == allEffectsOff.Length)
                    EnableCode(Code.AllEffectsOff);
            }
            else
                allEffectsOffIndex = 0;
            //===PoisonMin
            if (Input.GetKeyDown(addPoisonMinStatusEffect[poisonMinOnIndex]))
            {
                poisonMinOnIndex++;
                if (poisonMinOnIndex == addPoisonMinStatusEffect.Length)
                    EnableCode(Code.PoisonMinOn);
            }
            else
                poisonMinOnIndex = 0;
            if (Input.GetKeyDown(removePoisonMinStatusEffect[poisonMinOffIndex]))
            {
                poisonMinOffIndex++;
                if (poisonMinOffIndex == removePoisonMinStatusEffect.Length)
                    EnableCode(Code.PoisonMinOff);
            }
            else
                poisonMinOffIndex = 0;
            //===PoisonMax
            if (Input.GetKeyDown(addPoisonMaxStatusEffect[poisonMaxOnIndex]))
            {
                poisonMaxOnIndex++;
                if (poisonMaxOnIndex == addPoisonMaxStatusEffect.Length)
                    EnableCode(Code.PoisonMaxOn);
            }
            else
                poisonMaxOnIndex = 0;
            if (Input.GetKeyDown(removePoisonMaxStatusEffect[poisonMaxOffIndex]))
            {
                poisonMaxOffIndex++;
                if (poisonMaxOffIndex == removePoisonMaxStatusEffect.Length)
                    EnableCode(Code.PoisonMaxOff);
            }
            else
                poisonMaxOffIndex = 0;
            //===Confused
            if (Input.GetKeyDown(addConfusedStatusEffect[confusedOnIndex]))
            {
                confusedOnIndex++;
                if (confusedOnIndex == addConfusedStatusEffect.Length)
                    EnableCode(Code.ConfuseOn);
            }
            else
                confusedOnIndex = 0;
            if (Input.GetKeyDown(removeConfusedStatusEffect[confusedOffIndex]))
            {
                confusedOffIndex++;
                if (confusedOffIndex == removeConfusedStatusEffect.Length)
                    EnableCode(Code.ConfuseOff);
            }
            else
                confusedOffIndex = 0;
            //===Silenced
            if (Input.GetKeyDown(addSilencedStatusEffect[silencedOnIndex]))
            {
                silencedOnIndex++;
                if (silencedOnIndex == addSilencedStatusEffect.Length)
                    EnableCode(Code.SilenceOn);
            }
            else
                silencedOnIndex = 0;
            if (Input.GetKeyDown(removeSilencedStatusEffect[silencedOffIndex]))
            {
                silencedOffIndex++;
                if (silencedOffIndex == removeSilencedStatusEffect.Length)
                    EnableCode(Code.SilenceOff);
            }
            else
                silencedOffIndex = 0;
            //===Paralyzed
            if (Input.GetKeyDown(addParalyzedStatusEffect[paralyzedOnIndex]))
            {
                paralyzedOnIndex++;
                if (paralyzedOnIndex == addParalyzedStatusEffect.Length)
                    EnableCode(Code.ParalyzedOn);
            }
            else
                paralyzedOnIndex = 0;
            if (Input.GetKeyDown(removeParalyzedStatusEffect[paralyzedOffIndex]))
            {
                paralyzedOffIndex++;
                if (paralyzedOffIndex == removeParalyzedStatusEffect.Length)
                    EnableCode(Code.ParalyzedOff);
            }
            else
                paralyzedOffIndex = 0;

            //============================Player================================//
            if (Input.GetKeyDown(addMaxHealth[maxHealthIndex]))
            {
                maxHealthIndex++;
                if (maxHealthIndex == addMaxHealth.Length)
                    EnableCode(Code.HealthMax);
            }
            else
                maxHealthIndex = 0;
            if (Input.GetKeyDown(setHealth[setHealthIndex]))
            {
                setHealthIndex++;
                if (setHealthIndex == setHealth.Length)
                    EnableCode(Code.SetHealth);
            }
            else
                setHealthIndex = 0;
            if (Input.GetKeyDown(addMaxVitality[maxVitalityIndex]))
            {
                maxVitalityIndex++;
                if (maxVitalityIndex == addMaxVitality.Length)
                    EnableCode(Code.VitalityMax);
            }
            else
                maxVitalityIndex = 0;
            if (Input.GetKeyDown(setVitality[setVitalityIndex]))
            {
                setVitalityIndex++;
                if (setVitalityIndex == setVitality.Length)
                    EnableCode(Code.SetVitality);
            }
            else
                setVitalityIndex = 0;
   
            if (Input.GetKeyDown(addMaxGold[maxGoldIndex]))
            {
                maxGoldIndex++;
                if (maxGoldIndex == addMaxGold.Length)
                    EnableCode(Code.GoldMax);
            }
            else
                maxGoldIndex = 0;

            if (Input.GetKeyDown(setGold[setGoldIndex]))
            {
                setGoldIndex++;
                if (setGoldIndex == setGold.Length)
                    EnableCode(Code.SetGold);
            }
            else
                setGoldIndex = 0;
            if (Input.GetKeyDown(setCamera[setCameraIndex]))
            {
                setCameraIndex++;
                if (setCameraIndex == setCamera.Length)
                    EnableCode(Code.CameraOn);
            }
            else
                setCameraIndex = 0;
            if (Input.GetKeyDown(setSwimming[setSwimmingIndex]))
            {
                setSwimmingIndex++;
                if (setSwimmingIndex == setSwimming.Length)
                    EnableCode(Code.SwimmingOn);
            }
            else
                setSwimmingIndex = 0;
            if (Input.GetKeyDown(killPlayer[killIndex]))
            {
                killIndex++;
                if (killIndex == killPlayer.Length)
                    EnableCode(Code.Kill);
            }
            else
                killIndex = 0;
            if (Input.GetKeyDown(savePlayerPos[savePosIndex]))
            {
                savePosIndex++;
                if (savePosIndex == savePlayerPos.Length)
                    EnableCode(Code.SavePos);
            }
            else
                savePosIndex = 0;
            if (Input.GetKeyDown(loadPlayerPos[loadPosIndex]))
            {
                loadPosIndex++;
                if (loadPosIndex == loadPlayerPos.Length)
                    EnableCode(Code.LoadPos);
            }
            else
                loadPosIndex = 0;
            //============================Shop================================//
            if (Input.GetKeyDown(openShopDuskCliff[shopDuskCliffIndex]))
            {
                shopDuskCliffIndex++;
                if (shopDuskCliffIndex == openShopDuskCliff.Length)
                    EnableCode(Code.ShopDuskCliff);
            }
            else
                shopDuskCliffIndex = 0;
            if (Input.GetKeyDown(openShopWindAcre[shopWindAcreIndex]))
            {
                shopWindAcreIndex++;
                if (shopWindAcreIndex == openShopWindAcre.Length)
                    EnableCode(Code.ShopWindAcre);
            }
            else
                shopWindAcreIndex = 0;
            if (Input.GetKeyDown(openShopSkulkCove[shopSkulkCoveIndex]))
            {
                shopSkulkCoveIndex++;
                if (shopSkulkCoveIndex == openShopSkulkCove.Length)
                    EnableCode(Code.ShopSkulkcove);
            }
            else
                shopSkulkCoveIndex = 0;
            //============================Scene================================//
            if (Input.GetKeyDown(goToTitle[titleIndex]))
            {
                titleIndex++;
                if (titleIndex == goToTitle.Length)
                    EnableCode(Code.Title);
            }
            else
                titleIndex = 0;
            if (Input.GetKeyDown(goToIntro[introIndex]))
            {
                introIndex++;
                if (introIndex == goToIntro.Length)
                    EnableCode(Code.Intro);
            }
            else
                introIndex = 0;
            if (Input.GetKeyDown(goToOverWorld[overIndex]))
            {
                overIndex++;
                if (overIndex == goToOverWorld.Length)
                    EnableCode(Code.Over);
            }
            else
                overIndex = 0;
            if (Input.GetKeyDown(goToDungeon1[dungeon1Index]))
            {
                dungeon1Index++;
                if (dungeon1Index == goToDungeon1.Length)
                    EnableCode(Code.Dungeon1);
            }
            else
                dungeon1Index = 0;
            if (Input.GetKeyDown(goToDungeon2[dungeon2Index]))
            {
                dungeon2Index++;
                if (dungeon2Index == goToDungeon2.Length)
                    EnableCode(Code.Dungeon2);
            }
            else
                dungeon2Index = 0;
            if (Input.GetKeyDown(goToDungeon3[dungeon3Index]))
            {
                dungeon3Index++;
                if (dungeon3Index == goToDungeon3.Length)
                    EnableCode(Code.Dungeon3);
            }
            else
                dungeon3Index = 0;
            if (Input.GetKeyDown(goToGhostShip[ghostIndex]))
            {
                ghostIndex++;
                if (ghostIndex == goToGhostShip.Length)
                    EnableCode(Code.GhostShip);
            }
            else
                ghostIndex = 0;
            if (Input.GetKeyDown(goToTemple1[temple1Index]))
            {
                temple1Index++;
                if (temple1Index == goToTemple1.Length)
                    EnableCode(Code.Temple1);
            }
            else
                temple1Index = 0;
            if (Input.GetKeyDown(goToProtoType[prototypeIndex]))
            {
                prototypeIndex++;
                if (prototypeIndex == goToProtoType.Length)
                    EnableCode(Code.Prototype);
            }
            else
                prototypeIndex = 0;
            if (Input.GetKeyDown(goToDwellingTimber[dwellingIndex]))
            {
                dwellingIndex++;
                if (dwellingIndex == goToDwellingTimber.Length)
                    EnableCode(Code.DwellingTimber);
            }
            else
                dwellingIndex = 0;
        }
        else if(isDebugMode && healthMode && !vitalityMode && !goldMode && !cameraMode && !swimmingMode && !SceneSystem.inTransition)
        {
            if (Input.GetKeyDown(KeyCode.Equals) && !CodeAdded)
            {
                playerSystem.PlayerRecovery(1);
                CodeAdded = true;
            }
            else if (Input.GetKeyDown(KeyCode.Minus) && !CodeAdded)
            {
                playerSystem.PlayerDamage(1, false);
                CodeAdded = true;
            }
            else if (Input.GetKeyUp(KeyCode.Equals))
                CodeAdded = false;
            else if (Input.GetKeyUp(KeyCode.Minus))
                CodeAdded = false;
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                healthMode = false;
                CodeAdded = false;
            }
          
        }
        else if (isDebugMode && !healthMode && vitalityMode && !goldMode && !cameraMode && !swimmingMode && !SceneSystem.inTransition)
        {
            if (Input.GetKeyDown(KeyCode.Equals) && !CodeAdded)
            {
                playerSystem.AddVitality(1);
                CodeAdded = true;
            }
            else if (Input.GetKeyDown(KeyCode.Minus) && !CodeAdded)
            {
                playerSystem.RemoveVitality(1);
                CodeAdded = true;
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                vitalityMode = false;
                CodeAdded = false;
            }
            else if (Input.GetKeyUp(KeyCode.Minus))
                CodeAdded = false;
            else if (Input.GetKeyUp(KeyCode.Equals))
                CodeAdded = false;
        }
        else if (isDebugMode && !healthMode && !vitalityMode && goldMode && !cameraMode && !swimmingMode && !SceneSystem.inTransition)
        {
            if (Input.GetKey(KeyCode.Equals))
                playerSystem.AddGold(1);
            else if (Input.GetKey(KeyCode.Minus))
                playerSystem.RemoveGold(1);
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                goldMode = false;
            }
        }
        else if (Input.anyKeyDown && isDebugMode && !healthMode && !vitalityMode && !goldMode && cameraMode && !swimmingMode && !SceneSystem.inTransition)
        {
            if (Input.GetKeyDown(setCamera[setCameraIndex]))
            {
                setCameraIndex++;
                if (setCameraIndex == setCamera.Length)
                    EnableCode(Code.CameraOff);
            }
            else
                setCameraIndex = 0;
        }
        else if (Input.anyKeyDown && isDebugMode && !healthMode && !vitalityMode && !goldMode && !cameraMode && swimmingMode && !SceneSystem.inTransition)
        {
            if (Input.GetKeyDown(setSwimming[setSwimmingIndex]))
            {
                setSwimmingIndex++;
                if (setSwimmingIndex == setSwimming.Length)
                    EnableCode(Code.SwimmingOff);
            }
            else
                setSwimmingIndex = 0;
            if (Input.GetKeyDown(addMaxAir[maxAirIndex]))
            {
                maxAirIndex++;
                if (maxAirIndex == addMaxAir.Length)
                    EnableCode(Code.AirMax);
            }
            else
                maxAirIndex = 0;
        }
    }
    public IEnumerator FadeTextOut(float timeFactor, Text currentText)
    {
        currentText.color = new Color(currentText.color.r, currentText.color.g, currentText.color.b, 1);
        currentText.GetComponent<RectTransform>().anchoredPosition = textOrgPos;
        while (currentText.color.a > 0.0f)
        {
            currentText.enabled = true;
            RectTransform move = currentText.GetComponent<RectTransform>();

            move.transform.Translate(Vector3.up * -Time.fixedDeltaTime * 10);
            currentText.color = new Color(currentText.color.r, currentText.color.g, currentText.color.b, currentText.color.a - (Time.fixedDeltaTime / timeFactor));
            yield return null;
        }
        if (currentText.color.a <= 0.0f)
        {
          
            currentText.GetComponent<Text>().enabled = false;
            currentText.GetComponent<RectTransform>().anchoredPosition = textOrgPos;
        }
    }
    public enum Code { Everything, Nothing, InvincibleOn, InvincibleOff, MaxedOutSwordOn, MaxedOutSwordOff, AllSwords, AllArmor, AllBoots, AllJewels, AllItems, FlareSword, LightningSword, FrostSword, TerraSword,
                       HelixArmor, ApexArmor, MythicArmor, LegendaryArmor, HoverBoots, CinderBoots, StormBoots, MedicalBoots,
                       RedJewel, BlueJewel, GreenJewel, YellowJewel, PurpleJewel, LifeBerry, Rixile, NicirPlant, MindRemedy, PetriShroom,
                       ReliefOinment, MagicIdol, TerraIdol, Key, DemonMask, MoonPearl, MarkOfEtymology, AllEffectsOn, AllEffectsOff,
                       PoisonMinOn, PoisonMinOff, PoisonMaxOn, PoisonMaxOff, ConfuseOn, ConfuseOff, SilenceOn, SilenceOff, ParalyzedOn,
                       ParalyzedOff, HealthMax, SetHealth, VitalityMax, SetVitality, AirMax, GoldMax, SetGold, CameraOn, CameraOff, Kill, SavePos,
                       LoadPos, ShopDuskCliff, ShopWindAcre, ShopSkulkcove, Title, Intro, Over, Dungeon1, Dungeon2, Dungeon3, GhostShip, Temple1, 
                       Prototype, DwellingTimber, SwimmingOn, SwimmingOff, AllJournals, History1, History2, History3, History4, History5, History6, 
                       Discovery1, Discovery2, CodeBook1, CodeBook2, BeatGame, ResetGame }
    public void EnableCode(Code type)
    {
        ResetAllIndexes();
        switch (type)
        {
            case Code.Everything:
                {
                    AudioSystem.PlayAudioSource(soundFx[1], 1, 1);
                    statusText.text = "Give Everything";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.AddVitality(95);
                    playerSystem.PlayerRecovery(94);
                    playerSystem.AddGold(99999);

                    playerSystem.GetBoots(BootActive.hover);
                    playerSystem.GetBoots(BootActive.cinder);
                    playerSystem.GetBoots(BootActive.storm);
                    playerSystem.GetBoots(BootActive.medical);

                    playerSystem.GetArmor(ArmorActive.helix);
                    playerSystem.GetArmor(ArmorActive.apex);
                    playerSystem.GetArmor(ArmorActive.mythic);
                    playerSystem.GetArmor(ArmorActive.legendary);

                    swordSystem.GetSword(SwordActive.flare);
                    swordSystem.GetSword(SwordActive.lightning);
                    swordSystem.GetSword(SwordActive.frost);
                    swordSystem.GetSword(SwordActive.terra);

                    jewelSystem.GetJewel(JewelActive.red);
                    jewelSystem.GetJewel(JewelActive.blue);
                    jewelSystem.GetJewel(JewelActive.green);
                    jewelSystem.GetJewel(JewelActive.yellow);
                    jewelSystem.GetJewel(JewelActive.purple);

                    itemSystem.GetItem(ItemSystem.Item.LifeBerry, 9);
                    itemSystem.GetItem(ItemSystem.Item.Rixile, 9);
                    itemSystem.GetItem(ItemSystem.Item.NicirPlant, 9);
                    itemSystem.GetItem(ItemSystem.Item.MindRemedy, 9);
                    itemSystem.GetItem(ItemSystem.Item.PetriShroom, 9);
                    itemSystem.GetItem(ItemSystem.Item.ReliefOintment, 9);
                    itemSystem.GetItem(ItemSystem.Item.MagicIdol, 9);
                    itemSystem.GetItem(ItemSystem.Item.TerraIdol, 9);
                    itemSystem.GetItem(ItemSystem.Item.key, 9);
                    itemSystem.GetItem(ItemSystem.Item.DemonMask, 0);
                    itemSystem.GetItem(ItemSystem.Item.MoonPearl, 0);
                    itemSystem.GetItem(ItemSystem.Item.MarkofEtymology, 0);
                    storySystem.GiveAllJournals(true);
                    
                    break;
                }
            case Code.Nothing:
                {
                    AudioSystem.PlayAudioSource(soundFx[91], 1, 1);
                    statusText.text = "Lose Everything";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    storySystem.GiveAllJournals(false);
                    playerSystem.ResetPlayer();
                   
                    break;
                }
            case Code.BeatGame:
                {
                    AudioSystem.PlayAudioSource(soundFx[76], 1, 1);
                    statusText.text = "Game Levels Beat and Items Obtained ";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    objectSystem.BeatTheGame();
                    break;
                }
            case Code.ResetGame:
                {
                    AudioSystem.PlayAudioSource(soundFx[79], 1, 1);
                    statusText.text = "Game Levels Reset and Items Restored";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    objectSystem.UnBeatTheGame();
                    break;
                }
            case Code.InvincibleOn:
                {
                    AudioSystem.PlayAudioSource(soundFx[2], 1, 1);
                    statusText.text = "Invincibility On";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    isInvincible = true;
                   
                    break;
                }
            case Code.InvincibleOff:
                {
                    AudioSystem.PlayAudioSource(soundFx[3], 1, 1);
                    statusText.text = "Invincibility Off";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    isInvincible = false;

                    break;
                }
            case Code.MaxedOutSwordOn:
                {
                    AudioSystem.PlayAudioSource(soundFx[4], 1, 1);

                    statusText.text = "Infinite Sword Power On";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    swordSystem.MaxOutSword(true);
                    break;
                }
            case Code.MaxedOutSwordOff:
                {
                    AudioSystem.PlayAudioSource(soundFx[5], 1, 1);
                    statusText.text = "Infinite Sword Power Off";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    swordSystem.MaxOutSword(false);
                    break;
                }
            //-------------------------------Sword----------------------------------//
            case Code.AllSwords:
                {
                    AudioSystem.PlayAudioSource(soundFx[6], 1, 1);
                    statusText.text = "All Swords Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    swordSystem.GetSword(SwordActive.flare);
                    swordSystem.GetSword(SwordActive.lightning);
                    swordSystem.GetSword(SwordActive.frost);
                    swordSystem.GetSword(SwordActive.terra);
                    break;
                }
            case Code.FlareSword:
                {
                    AudioSystem.PlayAudioSource(soundFx[7], 1, 1);
                    statusText.text = "Flare Sword Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    swordSystem.GetSword(SwordActive.flare);
                    break;
                }
            case Code.LightningSword:
                {
                    AudioSystem.PlayAudioSource(soundFx[8], 1, 1);
                    statusText.text = "Lightning Sword Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    swordSystem.GetSword(SwordActive.lightning);
                    break;
                }
            case Code.FrostSword:
                {
                    AudioSystem.PlayAudioSource(soundFx[9], 1, 1);
                    statusText.text = "Frost Sword Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    swordSystem.GetSword(SwordActive.frost);
                    break;
                }
            case Code.TerraSword:
                {
                    AudioSystem.PlayAudioSource(soundFx[10], 1, 1);
                    statusText.text = "Terra Sword Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    swordSystem.GetSword(SwordActive.terra);
                    break;
                }
            //-------------------------------Journals----------------------------------//
            case Code.AllJournals:
                {
                    AudioSystem.PlayAudioSource(soundFx[75], 1, 1);
                    statusText.text = "All Journals Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    storySystem.GiveAllJournals(true);
                    break;
                }
            case Code.Discovery1:
                {
                    AudioSystem.PlayAudioSource(soundFx[78], 1, 1);
                    statusText.text = "Island Discovery - ID-0 Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    storySystem.giveJournal(1);
                    break;
                }
            case Code.History1:
                {
                    AudioSystem.PlayAudioSource(soundFx[89], 1, 1);
                    statusText.text = "Ancient History - AH-0 Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    storySystem.giveJournal(2);
                    break;
                }
            case Code.History2:
                {
                    AudioSystem.PlayAudioSource(soundFx[86], 1, 1);
                    statusText.text = "Ancient History - AH-1 Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    storySystem.giveJournal(3);
                    break;
                }
            case Code.History3:
                {
                    AudioSystem.PlayAudioSource(soundFx[84], 1, 1);
                    statusText.text = "Ancient History - AH-2 Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    storySystem.giveJournal(4);
                    break;
                }
            case Code.History4:
                {
                    AudioSystem.PlayAudioSource(soundFx[85], 1, 1);
                    statusText.text = "Ancient History - AH-3 Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    storySystem.giveJournal(5);
                    break;
                }
            case Code.History5:
                {
                    AudioSystem.PlayAudioSource(soundFx[87], 1, 1);
                    statusText.text = "Ancient History - AH-4 Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    storySystem.giveJournal(6);
                    break;
                }
            case Code.History6:
                {
                    AudioSystem.PlayAudioSource(soundFx[88], 1, 1);
                    statusText.text = "Ancient History - AH-5 Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    storySystem.giveJournal(7);
                    break;
                }
            case Code.CodeBook1:
                {
                    AudioSystem.PlayAudioSource(soundFx[93], 1, 1);
                    statusText.text = "Decoded Script - DS-0 Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    storySystem.giveJournal(8);
                    break;
                }
            case Code.CodeBook2:
                {
                    AudioSystem.PlayAudioSource(soundFx[93], 1, 1);
                    statusText.text = "Decoded Script - DS-1 Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    storySystem.giveJournal(9);
                    break;
                }
            case Code.Discovery2:
                {
                    AudioSystem.PlayAudioSource(soundFx[82], 1, 1);
                    statusText.text = "Island discovery - ID-1 Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    storySystem.giveJournal(10);
                    break;
                }
            //-------------------------------Armor----------------------------------//
            case Code.AllArmor:
                {
                    AudioSystem.PlayAudioSource(soundFx[11], 1, 1);
                    statusText.text = "All Armor Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.GetArmor(ArmorActive.helix);
                    playerSystem.GetArmor(ArmorActive.apex);
                    playerSystem.GetArmor(ArmorActive.mythic);
                    playerSystem.GetArmor(ArmorActive.legendary);
                    break;
                }
            case Code.HelixArmor:
                {
                    AudioSystem.PlayAudioSource(soundFx[12], 1, 1);
                    statusText.text = "Helix Armor Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.GetArmor(ArmorActive.helix);
                    break;
                }
            case Code.ApexArmor:
                {
                    AudioSystem.PlayAudioSource(soundFx[13], 1, 1);
                    statusText.text = "Apex Armor Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.GetArmor(ArmorActive.apex);
                    break;
                }
            case Code.MythicArmor:
                {
                    AudioSystem.PlayAudioSource(soundFx[14], 1, 1);
                    statusText.text = "Mythic Armor Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.GetArmor(ArmorActive.mythic);
                    break;
                }
            case Code.LegendaryArmor:
                {
                    AudioSystem.PlayAudioSource(soundFx[15], 1, 1);
                    statusText.text = "Legendary Armor Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.GetArmor(ArmorActive.legendary);
                    break;
                }
            //-------------------------------Boot----------------------------------//
            case Code.AllBoots:
                {
                    AudioSystem.PlayAudioSource(soundFx[16], 1, 1);
                    statusText.text = "All Boots Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.GetBoots(BootActive.hover);
                    playerSystem.GetBoots(BootActive.cinder);
                    playerSystem.GetBoots(BootActive.storm);
                    playerSystem.GetBoots(BootActive.medical);
                    break;
                }
            case Code.HoverBoots:
                {
                    AudioSystem.PlayAudioSource(soundFx[17], 1, 1);
                    statusText.text = "Hovers Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.GetBoots(BootActive.hover);
                    break;
                }
            case Code.CinderBoots:
                {
                    AudioSystem.PlayAudioSource(soundFx[18], 1, 1);
                    statusText.text = "Cinders Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.GetBoots(BootActive.cinder);
                    break;
                }
            case Code.StormBoots:
                {
                    AudioSystem.PlayAudioSource(soundFx[19], 1, 1);
                    statusText.text = "Storms Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.GetBoots(BootActive.storm);
                    break;
                }
            case Code.MedicalBoots:
                {
                    AudioSystem.PlayAudioSource(soundFx[20], 1, 1);
                    statusText.text = "Medicals Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.GetBoots(BootActive.medical);
                    break;
                }
            //-------------------------------Jewel----------------------------------//
            case Code.AllJewels:
                {
                    AudioSystem.PlayAudioSource(soundFx[21], 1, 1);
                    statusText.text = "All Jewels Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    jewelSystem.GetJewel(JewelActive.red);
                    jewelSystem.GetJewel(JewelActive.blue);
                    jewelSystem.GetJewel(JewelActive.green);
                    jewelSystem.GetJewel(JewelActive.yellow);
                    jewelSystem.GetJewel(JewelActive.purple);
                    break;
                }
            case Code.RedJewel:
                {
                    AudioSystem.PlayAudioSource(soundFx[22], 1, 1);
                    statusText.text = "Jewel of Power Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    jewelSystem.GetJewel(JewelActive.red);
                    break;
                }
            case Code.BlueJewel:
                {
                    AudioSystem.PlayAudioSource(soundFx[23], 1, 1);
                    statusText.text = "Jewel of Spirit Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    jewelSystem.GetJewel(JewelActive.blue);
                    break;
                }
            case Code.GreenJewel:
                {
                    AudioSystem.PlayAudioSource(soundFx[24], 1, 1);
                    statusText.text = "Jewel of Time Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    jewelSystem.GetJewel(JewelActive.green);
                    break;
                }
            case Code.YellowJewel:
                {
                    AudioSystem.PlayAudioSource(soundFx[25], 1, 1);
                    statusText.text = "Jewel of Light Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    jewelSystem.GetJewel(JewelActive.yellow);
                    break;
                }
            case Code.PurpleJewel:
                {
                    AudioSystem.PlayAudioSource(soundFx[26], 1, 1);
                    statusText.text = "Jewel of Dark Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    jewelSystem.GetJewel(JewelActive.purple);
                    break;
                }
            //-------------------------------Item----------------------------------//
            case Code.AllItems:
                {
                    AudioSystem.PlayAudioSource(soundFx[27], 1, 1);
                    statusText.text = "Give All Items";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    itemSystem.GetItem(ItemSystem.Item.LifeBerry, 9);
                    itemSystem.GetItem(ItemSystem.Item.Rixile, 9);
                    itemSystem.GetItem(ItemSystem.Item.NicirPlant, 9);
                    itemSystem.GetItem(ItemSystem.Item.MindRemedy, 9);
                    itemSystem.GetItem(ItemSystem.Item.PetriShroom, 9);
                    itemSystem.GetItem(ItemSystem.Item.ReliefOintment, 9);
                    itemSystem.GetItem(ItemSystem.Item.MagicIdol, 9);
                    itemSystem.GetItem(ItemSystem.Item.TerraIdol, 9);
                    itemSystem.GetItem(ItemSystem.Item.key, 9);
                    itemSystem.GetItem(ItemSystem.Item.DemonMask, 0);
                    itemSystem.GetItem(ItemSystem.Item.MoonPearl, 0);
                    itemSystem.GetItem(ItemSystem.Item.MarkofEtymology, 0);

                    break;
                }
            case Code.LifeBerry:
                {
                    AudioSystem.PlayAudioSource(soundFx[28], 1, 1);
                    statusText.text = "Max Life Berries";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    itemSystem.GetItem(ItemSystem.Item.LifeBerry, 9);
                    break;
                }
            case Code.Rixile:
                {
                    AudioSystem.PlayAudioSource(soundFx[29], 1, 1);
                    statusText.text = "Max Rixiles";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    itemSystem.GetItem(ItemSystem.Item.Rixile, 9);
                    break;
                }
            case Code.NicirPlant:
                {
                    AudioSystem.PlayAudioSource(soundFx[30], 1, 1);
                    statusText.text = "Max Nicir Plants";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    itemSystem.GetItem(ItemSystem.Item.NicirPlant, 9);
                    break;
                }
            case Code.MindRemedy:
                {
                    AudioSystem.PlayAudioSource(soundFx[31], 1, 1);
                    statusText.text = "Max Mind Remedies";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    itemSystem.GetItem(ItemSystem.Item.MindRemedy, 9);
                    break;
                }
            case Code.PetriShroom:
                {
                    AudioSystem.PlayAudioSource(soundFx[32], 1, 1);
                    statusText.text = "Max Petri-Shrooms";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    itemSystem.GetItem(ItemSystem.Item.PetriShroom, 9);
                    break;
                }
            case Code.ReliefOinment:
                {
                    AudioSystem.PlayAudioSource(soundFx[33], 1, 1);
                    statusText.text = "Max Relief Ointments";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    itemSystem.GetItem(ItemSystem.Item.ReliefOintment, 9);
                    break;
                }
            case Code.TerraIdol:
                {
                    AudioSystem.PlayAudioSource(soundFx[34], 1, 1);
                    statusText.text = "Max Terra Idols";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    itemSystem.GetItem(ItemSystem.Item.TerraIdol, 9);
                    break;
                }
            case Code.MagicIdol:
                {
                    AudioSystem.PlayAudioSource(soundFx[35], 1, 1);
                    statusText.text = "Max Magic Idols";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    itemSystem.GetItem(ItemSystem.Item.MagicIdol, 9);
                    break;
                }
            case Code.Key:
                {
                    AudioSystem.PlayAudioSource(soundFx[36], 1, 1);
                    statusText.text = "Max Keys";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    itemSystem.GetItem(ItemSystem.Item.key, 9);
                    break;
                }
            case Code.DemonMask:
                {
                    AudioSystem.PlayAudioSource(soundFx[37], 1, 1);
                    statusText.text = "Demon Mask Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    itemSystem.GetItem(ItemSystem.Item.DemonMask, 0);
                    break;
                }
            case Code.MoonPearl:
                {
                    AudioSystem.PlayAudioSource(soundFx[38], 1, 1);
                    statusText.text = "Moon Pearl Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    itemSystem.GetItem(ItemSystem.Item.MoonPearl, 0);
                    break;
                }
            case Code.MarkOfEtymology:
                {
                    AudioSystem.PlayAudioSource(soundFx[80], 1, 1);
                    statusText.text = "Mark of Etymology Unlocked";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    itemSystem.GetItem(ItemSystem.Item.MarkofEtymology, 0);
                    break;
                }
            //-------------------------------Effects----------------------------------//
            case Code.AllEffectsOn:
                {
                    AudioSystem.PlayAudioSource(soundFx[39], 1, 1);
                    statusText.text = "All Status Effects On";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.poisonMax, true);
                    playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.confuse, true);
                    playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.silence, true);
                    playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.paralyzed, true);
                    break;
                }
            case Code.AllEffectsOff:
                {
                    AudioSystem.PlayAudioSource(soundFx[40], 1, 1);
                    statusText.text = "All Status Effects Off";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.poisonMax, false);
                    playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.confuse, false);
                    playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.silence, false);
                    playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.paralyzed, false);
                    break;
                }
            case Code.PoisonMinOn:
                {
                    AudioSystem.PlayAudioSource(soundFx[41], 1, 1);
                    statusText.text = "Minimally Poisoned On";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.poisonMin, true);
                    break;
                }
            case Code.PoisonMinOff:
                {
                    AudioSystem.PlayAudioSource(soundFx[42], 1, 1);
                    statusText.text = "Minimally Poisoned Off";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.poisonMin, false);
                    break;
                }
            case Code.PoisonMaxOn:
                {
                    AudioSystem.PlayAudioSource(soundFx[43], 1, 1);
                    statusText.text = "Maximally Poisoned On";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.poisonMax, true);
                    break;
                }
            case Code.PoisonMaxOff:
                {
                    AudioSystem.PlayAudioSource(soundFx[44], 1, 1);
                    statusText.text = "Maximally Poisoned Off";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.poisonMax, false);
                    break;
                }
            case Code.ConfuseOn:
                {
                    AudioSystem.PlayAudioSource(soundFx[45], 1, 1);
                    statusText.text = "Confusion On";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.confuse, true);
                    break;
                }
            case Code.ConfuseOff:
                {
                    AudioSystem.PlayAudioSource(soundFx[46], 1, 1);
                    statusText.text = "Confusion Off";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.confuse, false);
                    break;
                }
            case Code.SilenceOn:
                {
                    AudioSystem.PlayAudioSource(soundFx[47], 1, 1);
                    statusText.text = "Silenced On";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.silence, true);
                    break;
                }
            case Code.SilenceOff:
                {
                    AudioSystem.PlayAudioSource(soundFx[48], 1, 1);
                    statusText.text = "Silenced Off";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.silence, false);
                    break;
                }
            case Code.ParalyzedOn:
                {
                    AudioSystem.PlayAudioSource(soundFx[49], 1, 1);
                    statusText.text = "Paralysis On";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.paralyzed, true);
                    break;
                }
            case Code.ParalyzedOff:
                {
                    AudioSystem.PlayAudioSource(soundFx[50], 1, 1);
                    statusText.text = "Paralysis Off";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.paralyzed, false);
                    break;
                }
            //-------------------------------Player----------------------------------//
            case Code.HealthMax:
                {
                    AudioSystem.PlayAudioSource(soundFx[51], 1, 1);
                    statusText.text = "Full Health";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.PlayerRecovery(100);
                    break;
                }
            case Code.SetHealth:
                {
                    AudioSystem.PlayAudioSource(soundFx[52], 1, 1);
                    statusText.text = "Adjust Health";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    healthMode = true;
                    break;
                }
            case Code.VitalityMax:
                {
                    AudioSystem.PlayAudioSource(soundFx[53], 1, 1);
                    statusText.text = "Full Vitality";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.AddVitality(95);
                    break;
                }
            case Code.SetVitality:
                {
                    AudioSystem.PlayAudioSource(soundFx[54], 1, 1);
                    statusText.text = "Adjust Vitality";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    vitalityMode = true;
                    break;
                }
            case Code.GoldMax:
                {
                    AudioSystem.PlayAudioSource(soundFx[55], 1, 1);
                    statusText.text = "Full Gold";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.AddGold(99999);
                    break;
                }
            case Code.AirMax:
                {
                    AudioSystem.PlayAudioSource(soundFx[81], 1, 1);
                    statusText.text = "Air Tank";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    underWaterSystem.ResetAir() ;
                    break;
                }
            case Code.SetGold:
                {
                    AudioSystem.PlayAudioSource(soundFx[56], 1, 1);
                    statusText.text = "Adjust Gold";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    goldMode = true;
                    break;
                }
            case Code.CameraOn:
                {
                    AudioSystem.PlayAudioSource(soundFx[57], 1, 1);
                    cameraMode = true;
                    GameObject playerArms = GameObject.Find("Core/Player/PlayerController/Head/PlayerCamera/Recoil");
                    GameObject playerCanvas = GameObject.Find("Core/Player/PlayerCanvas/GameUI");
                    playerArms.SetActive(false);
                    playerCanvas.SetActive(false);
                    break;
                }
            case Code.CameraOff:
                {
                    AudioSystem.PlayAudioSource(soundFx[58], 1, 1);
                    cameraMode = false;
                    GameObject playerArms = GameObject.Find("Core/Player/PlayerController/Head/PlayerCamera/Recoil");
                    GameObject playerCanvas = GameObject.Find("Core/Player/PlayerCanvas/GameUI");
                    playerArms.SetActive(true);
                    playerCanvas.SetActive(true);
                    break;
                }
            case Code.SwimmingOn:
                {
                    AudioSystem.PlayAudioSource(soundFx[83], 1, 1);
                    swimmingMode = true;
                    underWaterSystem.DebugSwimming(true);
                    break;
                }
            case Code.SwimmingOff:
                {
                    AudioSystem.PlayAudioSource(soundFx[92], 1, 1);
                    swimmingMode = false;
                    underWaterSystem.DebugSwimming(false);
                    underWaterSystem.ResetAir();
                    break;
                }
            case Code.Kill:
                {
                    AudioSystem.PlayAudioSource(soundFx[59], 1, 1);
                    statusText.text = "Player Dead";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.PlayerDamage(500, false);
                    break;
                }
            case Code.SavePos:
                {
                    AudioSystem.PlayAudioSource(soundFx[60], 1, 1);
                    statusText.text = "Position Saved";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    playerSystem.SavePosition();
                    break;
                }
            case Code.LoadPos:
                {
                    AudioSystem.PlayAudioSource(soundFx[61], 1, 1);
                    statusText.text = "Position Loaded";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    characterSystem = GetComponent<CharacterSystem>();
                    characterSystem.SetPosition();
                    sceneSystem.EnablePlayer(false);
                    playerSystem.LoadPosition();
                    break;
                }
            //-------------------------------Shop----------------------------------//
            case Code.ShopDuskCliff:
                {
                    AudioSystem.PlayAudioSource(soundFx[62], 1, 1);
                    statusText.text = "Buy From DuskCliff";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    shopSystem.SelectShop(Shop.DuskCliff, 4);
                    shopSystem.BrowseTheShop();
                    break;
                }
            case Code.ShopWindAcre:
                {
                    AudioSystem.PlayAudioSource(soundFx[63], 1, 1);
                    statusText.text = "Buy From WindAcre";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    shopSystem.SelectShop(Shop.WindAcre, 4);
                    shopSystem.BrowseTheShop();
                    break;
                }
            case Code.ShopSkulkcove:
                {
                    AudioSystem.PlayAudioSource(soundFx[64], 1, 1);
                    statusText.text = "Skulkcove Not Available";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    //shopSystem.SelectShop(Shop.SkulkCove, 6);
                    //shopSystem.BrowseTheShop();
                    break;
                }
            //-------------------------------Scene----------------------------------//
            case Code.Title:
                {
                    AudioSystem.PlayAudioSource(soundFx[65], 1, 1);
                    GameObject enemyUIObjs = GameObject.Find("Core/Player/PopUpCanvas/EnemyUIStorage");
                    foreach (Transform child in enemyUIObjs.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    characterSystem.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                    sceneSystem.gameUI.SetActive(false);
                    IEnumerator blackScreen = sceneSystem.FadeIn(1.0f, 2.0f, sceneSystem.blackScreenGame, true);
                    StartCoroutine(blackScreen);
                    if (M_audioSrc.isPlaying)
                    {
                        if (audioRoutine != null)
                            StopCoroutine(audioRoutine);
                        audioRoutine = AudioFadeOut(M_audioSrc);
                        StartCoroutine(audioRoutine);
                    }
                    statusText.text = "Loading Main Menu";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    sceneSystem.QuitTheGame();
                    break;
                }
            case Code.Intro:
                {
                    AudioSystem.PlayAudioSource(soundFx[66], 1, 1);
                    GameObject enemyUIObjs = GameObject.Find("Core/Player/PopUpCanvas/EnemyUIStorage");
                    foreach (Transform child in enemyUIObjs.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    characterSystem.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                    sceneSystem.gameUI.SetActive(false);
                    IEnumerator blackScreen = sceneSystem.FadeIn(1.0f, 2.0f, sceneSystem.blackScreenGame, true);
                    StartCoroutine(blackScreen);

                    
                    if (M_audioSrc.isPlaying)
                    {
                        if (audioRoutine != null)
                            StopCoroutine(audioRoutine);
                        audioRoutine = AudioFadeOut(M_audioSrc);
                        StartCoroutine(audioRoutine);
                    }
                    statusText.text = "Loading Rafter's Bargain";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    sceneSystem.StartScene(SceneSystem.Level.Intro);
                    break;
                }
            case Code.Over:
                {
                    AudioSystem.PlayAudioSource(soundFx[67], 1, 1);
                    GameObject enemyUIObjs = GameObject.Find("Core/Player/PopUpCanvas/EnemyUIStorage");
                    foreach (Transform child in enemyUIObjs.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    characterSystem.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                    sceneSystem.gameUI.SetActive(false);
                    IEnumerator blackScreen = sceneSystem.FadeIn(1.0f, 2.0f, sceneSystem.blackScreenGame, true);
                    StartCoroutine(blackScreen);

                    
                    if (M_audioSrc.isPlaying)
                    {
                        if (audioRoutine != null)
                            StopCoroutine(audioRoutine);
                        audioRoutine = AudioFadeOut(M_audioSrc);
                        StartCoroutine(audioRoutine);
                    }
                    statusText.text = "Loading Conquest Island";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    sceneSystem.StartScene(SceneSystem.Level.OverWorld);
                    break;
                }
            case Code.Dungeon1:
                {
                    AudioSystem.PlayAudioSource(soundFx[68], 1, 1);
                    GameObject enemyUIObjs = GameObject.Find("Core/Player/PopUpCanvas/EnemyUIStorage");
                    foreach (Transform child in enemyUIObjs.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    characterSystem.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                    sceneSystem.gameUI.SetActive(false);
                    IEnumerator blackScreen = sceneSystem.FadeIn(1.0f, 2.0f, sceneSystem.blackScreenGame, true);
                    StartCoroutine(blackScreen);

                    
                    if (M_audioSrc.isPlaying)
                    {
                        if (audioRoutine != null)
                            StopCoroutine(audioRoutine);
                        audioRoutine = AudioFadeOut(M_audioSrc);
                        StartCoroutine(audioRoutine);
                    }
                    statusText.text = "Loading Riverfall Shrine";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    sceneSystem.StartScene(SceneSystem.Level.DungeonOne);
                    break;
                }
            case Code.Dungeon2:
                {
                    AudioSystem.PlayAudioSource(soundFx[69], 1, 1);
                    GameObject enemyUIObjs = GameObject.Find("Core/Player/PopUpCanvas/EnemyUIStorage");
                    foreach (Transform child in enemyUIObjs.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    characterSystem.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                    sceneSystem.gameUI.SetActive(false);
                    IEnumerator blackScreen = sceneSystem.FadeIn(1.0f, 2.0f, sceneSystem.blackScreenGame, true);
                    StartCoroutine(blackScreen);

                    
                    if (M_audioSrc.isPlaying)
                    {
                        if (audioRoutine != null)
                            StopCoroutine(audioRoutine);
                        audioRoutine = AudioFadeOut(M_audioSrc);
                        StartCoroutine(audioRoutine);
                    }
                    statusText.text = "Loading Abandoned Ruins";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    sceneSystem.StartScene(SceneSystem.Level.DungeonTwo);
                    break;
                }
            case Code.Dungeon3:
                {
                    AudioSystem.PlayAudioSource(soundFx[90], 1, 1);
                    GameObject enemyUIObjs = GameObject.Find("Core/Player/PopUpCanvas/EnemyUIStorage");
                    foreach (Transform child in enemyUIObjs.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    characterSystem.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                    sceneSystem.gameUI.SetActive(false);
                    IEnumerator blackScreen = sceneSystem.FadeIn(1.0f, 2.0f, sceneSystem.blackScreenGame, true);
                    StartCoroutine(blackScreen);

                    
                    if (M_audioSrc.isPlaying)
                    {
                        if (audioRoutine != null)
                            StopCoroutine(audioRoutine);
                        audioRoutine = AudioFadeOut(M_audioSrc);
                        StartCoroutine(audioRoutine);
                    }
                    statusText.text = "Loading Well Pathway";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    sceneSystem.StartScene(SceneSystem.Level.DungeonThree);
                    break;
                }
            case Code.DwellingTimber:
                {
                    AudioSystem.PlayAudioSource(soundFx[77], 1, 1);
                    GameObject enemyUIObjs = GameObject.Find("Core/Player/PopUpCanvas/EnemyUIStorage");
                    foreach (Transform child in enemyUIObjs.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    characterSystem.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                    sceneSystem.gameUI.SetActive(false);
                    IEnumerator blackScreen = sceneSystem.FadeIn(1.0f, 2.0f, sceneSystem.blackScreenGame, true);
                    StartCoroutine(blackScreen);

                    
                    if (M_audioSrc.isPlaying)
                    {
                        if (audioRoutine != null)
                            StopCoroutine(audioRoutine);
                        audioRoutine = AudioFadeOut(M_audioSrc);
                        StartCoroutine(audioRoutine);
                    }
                    statusText.text = "Loading Dwelling Timber";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    sceneSystem.StartScene(SceneSystem.Level.DwellingTimber);
                    break;
                }
            case Code.GhostShip:
                {
                    AudioSystem.PlayAudioSource(soundFx[70], 1, 1);
                    GameObject enemyUIObjs = GameObject.Find("Core/Player/PopUpCanvas/EnemyUIStorage");
                    foreach (Transform child in enemyUIObjs.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    characterSystem.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                    sceneSystem.gameUI.SetActive(false);
                    IEnumerator blackScreen = sceneSystem.FadeIn(1.0f, 2.0f, sceneSystem.blackScreenGame, true);
                    StartCoroutine(blackScreen);

                    
                    if (M_audioSrc.isPlaying)
                    {
                        if (audioRoutine != null)
                            StopCoroutine(audioRoutine);
                        audioRoutine = AudioFadeOut(M_audioSrc);
                        StartCoroutine(audioRoutine);
                    }
                    statusText.text = "Loading The Cursed Grail";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    sceneSystem.StartScene(SceneSystem.Level.GhostShip);
                    break;
                }
            case Code.Temple1:
                {
                    AudioSystem.PlayAudioSource(soundFx[71], 1, 1);
                    GameObject enemyUIObjs = GameObject.Find("Core/Player/PopUpCanvas/EnemyUIStorage");
                    foreach (Transform child in enemyUIObjs.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    characterSystem.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                    sceneSystem.gameUI.SetActive(false);
                    IEnumerator blackScreen = sceneSystem.FadeIn(1.0f, 2.0f, sceneSystem.blackScreenGame, true);
                    StartCoroutine(blackScreen);
                    if (M_audioSrc.isPlaying)
                    {
                        if (audioRoutine != null)
                            StopCoroutine(audioRoutine);
                        audioRoutine = AudioFadeOut(M_audioSrc);
                        StartCoroutine(audioRoutine);
                    }
                    statusText.text = "Loading Temple of Reign";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    sceneSystem.StartScene(SceneSystem.Level.TempleOne);
                    break;
                }
            case Code.Prototype:
                {
                    AudioSystem.PlayAudioSource(soundFx[72], 1, 1);
                    characterSystem.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                    sceneSystem.gameUI.SetActive(false);
                    IEnumerator blackScreen = sceneSystem.FadeIn(1.0f, 2.0f, sceneSystem.blackScreenGame, true);
                    StartCoroutine(blackScreen);
                    if (M_audioSrc.isPlaying)
                    {
                        if (audioRoutine != null)
                            StopCoroutine(audioRoutine);
                        audioRoutine = AudioFadeOut(M_audioSrc);
                        StartCoroutine(audioRoutine);
                    }
                    statusText.text = "Loading N163LPH03N1X's Army";
                    if (statusRoutine != null)
                        StopCoroutine(statusRoutine);
                    statusRoutine = FadeTextOut(3, statusText);
                    StartCoroutine(statusRoutine);
                    sceneSystem.StartScene(SceneSystem.Level.Prototype);
                    break;
                }
        }
        playerSystem.PlayerInvincible(isInvincible);
    }
    public void ShutOffDebugMode()
    {
        isDebugMode = false;
        vitalityMode = false;
        healthMode = false;
        goldMode = false;
        cameraMode = false;
        swimmingMode = false;
        EnableMouse(false);
        CodeList.SetActive(false);
        toggleCodeList = false;
        sceneSystem.EnablePlayer(true);
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

                audio.Pause();
                audio.volume = MinVol;
                break;
            }
        }
    }
    public IEnumerator AudioFadeIn(AudioSource audio)
    {
        audio.volume = 0.0f;
        float MaxVol = 1;
        audio.UnPause();
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
  
}
