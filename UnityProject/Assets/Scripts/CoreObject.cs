using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using System.Collections;

public class CoreObject : MonoBehaviour
{
    IEnumerator routine;

    public AudioClip[] menuFx;
    public static CoreObject control;
    public static PlayerData playerData;
    public Transform gamePlayer;
    PlayerSystem playerSystem;
    SceneSystem sceneSystem;
    ObjectSystem objectSystem;
    ItemSystem itemSystem;
    SwordSystem swordSystem;
    TimeSystem timeSystem;
    JewelSystem jewelSystem;
    ShopSystem shopSystem;
    StorySystem storySystem;
    InteractionSystem interactionSystem;
    DialogueSystem dialogueSystem;
    public Texture defaultTexture;
    public Image savingImage;
    public static bool isSaving;
    public float savingTimer = 3;
    float savingTime;
    public static bool[] newGame = new bool[4] { true, true, true, true };
    public static bool[] fileActive = new bool[4] { false, false, false, false };
    public RawImage[] fileScreenShot;
    public Text[] fileText;
    public static bool isLoaded;

    string SAVE_FILE;

    string[] SAVE_FILENAME = new string[4] { "/SAVEGAME_1", "/SAVEGAME_2", "/SAVEGAME_3", "/SAVEGAME_4" };
    string[] LOAD_FILENAME = new string[4] { null, null, null, null };

    public string FILE_EXTENSION = ".TQG";
    public string PIC_EXTENSION = ".PNG";

    string LOAD_IMAGENAME;
    string[] LOAD_IMAGENAMES = new string[4] { "LOADIMAGE_1", "LOADIMAGE_2", "LOADIMAGE_3", "LOADIMAGE_4" };

    RawImage screenShot;
    public static int scene;

    public float gameProgress = 0;
    public Text progressPercentText;

    void Awake()
    {
        playerSystem = gamePlayer.GetComponent<PlayerSystem>();
        interactionSystem = gamePlayer.GetComponent<InteractionSystem>();
        itemSystem = gamePlayer.GetComponent<ItemSystem>();
        swordSystem = gamePlayer.GetComponent<SwordSystem>();
        jewelSystem = gamePlayer.GetComponent<JewelSystem>();
        shopSystem = gamePlayer.GetComponent<ShopSystem>();
        storySystem = gamePlayer.GetComponent<StorySystem>();
        objectSystem = GetComponent<ObjectSystem>();
        sceneSystem = gamePlayer.transform.parent.GetComponent<SceneSystem>();
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
        playerData = new PlayerData();
        
    }
    private void Start()
    {

        for (int s = 0; s < LOAD_IMAGENAMES.Length; s++)
        {
            if (!File.Exists(Application.dataPath + "/" + LOAD_IMAGENAMES[s] + PIC_EXTENSION))
                File.Create(Application.dataPath + "/" + LOAD_IMAGENAMES[s] + PIC_EXTENSION);
        }
        for(int s = 0; s < SAVE_FILENAME.Length; s++)
        {
            if (!File.Exists(Application.dataPath + SAVE_FILENAME[s] + FILE_EXTENSION))
                newGame[s] = true;
        }
        for(int t = 0; t < SAVE_FILENAME.Length; t++)
        {
            fileScreenShot[t].texture = defaultTexture;
        }
        savingTime = savingTimer;
        ApplyGameProgress();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            SaveTheGame();
        else if (Input.GetKeyDown(KeyCode.F2))
            LoadGame();
       
        if (isSaving)
        {
            Color newColor = new Color(savingImage.color.r, savingImage.color.g, savingImage.color.b, Mathf.PingPong(Time.unscaledTime, 1));
            savingImage.color = newColor;
            if (savingTime > 0)
            {
                savingImage.enabled = true;
                savingTime -= Time.fixedDeltaTime;
            }
            else if (savingTime < 0)
            {
                savingImage.enabled = false;
                AudioSystem.PlayAudioSource(menuFx[2], 1, 1);
                SaveGame();
                isSaving = false;
                savingTime = savingTimer;
            }
        }
        else
            savingImage.enabled = false;


        for(int ng = 0; ng < newGame.Length; ng++)
        {
            if (!newGame[ng])
                fileText[ng].text = null;
            else if (newGame[ng])
                fileText[ng].text = "Empty Slot";
        }
    }
  
    public void Loading(bool True)
    {
        if (True)
            isLoaded = true;
        else
            isLoaded = false;
    }
    public void SetFileSelection(int num)
    {
        for(int fa = 0; fa < 4; fa++)
        {
            if (fa == num) fileActive[fa] = true;
            else fileActive[fa] = false;
        }
    }
    public void SaveGame()
    {
        for(int fa = 0; fa < 4; fa++)
        {
            if (fileActive[fa])
            {
                newGame[fa] = false;
                SAVE_FILE = SAVE_FILENAME[fa];
                LOAD_IMAGENAME = LOAD_IMAGENAMES[fa];
                fileScreenShot[fa].enabled = true;
                screenShot = fileScreenShot[fa];
                break;
            }
        }
        ScreenCapture.CaptureScreenshot(Application.dataPath + "/" + LOAD_IMAGENAME + PIC_EXTENSION);
        string path = Application.dataPath + "/" + LOAD_IMAGENAME + PIC_EXTENSION;
        var loadImageFile = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(127, 120);
        texture.LoadImage(loadImageFile);
        if(screenShot != null)
            screenShot.texture = texture;
        Stream stream = File.Create(Application.dataPath + SAVE_FILE + FILE_EXTENSION);
        BinaryFormatter bf = new BinaryFormatter();
        Debug.Log("File Saved To: " + stream);
        playerSystem.SavePosition();
        //============================GameTime================================================//
        playerData.totalGameTime = TimeSystem.totalGameTime;
        playerData.day = TimeSystem.day;
        playerData.hour = TimeSystem.hour;
        playerData.minute = TimeSystem.minute;
        playerData.currentTimeOfDay = TimeSystem.CurrentTimeOfDay;
        playerData.timeInterval = TimeSystem.timeInterval;
        playerData.isNight = TimeSystem.isNight;
        playerData.isRain = TimeSystem.isRain;
        playerData.dayNightDisplay = TimeSystem.dayNightDisplay;
        //============================ObjectSystem============================================//
        playerData.gameProgress = gameProgress;
        //StoreVitality========================//
        playerData.storeVitality1 = ShopSystem.vitality1Bought;
        playerData.storeVitality2 = ShopSystem.vitality2Bought;
        playerData.storeVitality3 = ShopSystem.vitality3Bought;
        //Chests===============================//
        playerData.chest1 = ObjectSystem.chest1;
        playerData.chest2 = ObjectSystem.chest2;
        playerData.chest3 = ObjectSystem.chest3;
        playerData.chest4 = ObjectSystem.chest4;
        playerData.chest5 = ObjectSystem.chest5;
        playerData.chest6 = ObjectSystem.chest6;
        playerData.chest7 = ObjectSystem.chest7;
        playerData.chest8 = ObjectSystem.chest8;

        playerData.chest9 = ObjectSystem.chest9;
        playerData.chest10 = ObjectSystem.chest10;
        playerData.chest11 = ObjectSystem.chest11;
        playerData.chest12 = ObjectSystem.chest12;

        playerData.chest13 = ObjectSystem.chest13;
        playerData.chest14 = ObjectSystem.chest14;
        playerData.chest15 = ObjectSystem.chest15;
        playerData.chest16 = ObjectSystem.chest16;
        playerData.chest17 = ObjectSystem.chest17;
        playerData.chest18 = ObjectSystem.chest18;
        playerData.chest19 = ObjectSystem.chest19;
        playerData.chest20 = ObjectSystem.chest20;
        playerData.chest21 = ObjectSystem.chest21;
        playerData.chest22 = ObjectSystem.chest22;

        playerData.chest23 = ObjectSystem.chest23;
        playerData.chest24 = ObjectSystem.chest24;
        playerData.chest25 = ObjectSystem.chest25;
        playerData.chest26 = ObjectSystem.chest26;
        playerData.chest27 = ObjectSystem.chest27;
        playerData.chest28 = ObjectSystem.chest28;
        playerData.chest29 = ObjectSystem.chest29;
        playerData.chest30 = ObjectSystem.chest30;
        playerData.chest31 = ObjectSystem.chest31;
        playerData.chest32 = ObjectSystem.chest32;
        playerData.chest33 = ObjectSystem.chest33;
        playerData.chest34 = ObjectSystem.chest34;

        playerData.chest35 = ObjectSystem.chest35;
        playerData.chest36 = ObjectSystem.chest36;
        playerData.chest37 = ObjectSystem.chest37;
        playerData.chest38 = ObjectSystem.chest38;
        playerData.chest39 = ObjectSystem.chest39;
        playerData.chest40 = ObjectSystem.chest40;
        playerData.chest41 = ObjectSystem.chest41;
        playerData.chest42 = ObjectSystem.chest42;
        playerData.chest43 = ObjectSystem.chest43;
        playerData.chest44 = ObjectSystem.chest44;
        playerData.chest45 = ObjectSystem.chest45;
        playerData.chest46 = ObjectSystem.chest46;
        playerData.chest47 = ObjectSystem.chest47;
        playerData.chest48 = ObjectSystem.chest48;
        playerData.chest49 = ObjectSystem.chest49;
        playerData.chest50 = ObjectSystem.chest50;
        playerData.chest51 = ObjectSystem.chest51;
        playerData.chest52 = ObjectSystem.chest52;

        playerData.chest53 = ObjectSystem.chest53;
        playerData.chest54 = ObjectSystem.chest54;
        playerData.chest55 = ObjectSystem.chest55;
        playerData.chest56 = ObjectSystem.chest56;
        playerData.chest57 = ObjectSystem.chest57;

        playerData.chest58 = ObjectSystem.chest58;
        playerData.chest59 = ObjectSystem.chest59;
        playerData.chest60 = ObjectSystem.chest60;
        playerData.chest61 = ObjectSystem.chest61;
        playerData.chest62 = ObjectSystem.chest62;

        //Switches=============================//
        playerData.switch1 = ObjectSystem.switch1;
        playerData.switch2 = ObjectSystem.switch2;
        playerData.switch3 = ObjectSystem.switch3;
        playerData.switch4 = ObjectSystem.switch4;
        playerData.switch5 = ObjectSystem.switch5;
        playerData.switch6 = ObjectSystem.switch6;
        playerData.switch7 = ObjectSystem.switch7;
        playerData.switch8 = ObjectSystem.switch8;
        playerData.switch9 = ObjectSystem.switch9;
        playerData.switch10 = ObjectSystem.switch10;
        playerData.switch11 = ObjectSystem.switch11;
        playerData.switch12 = ObjectSystem.switch12;
        playerData.switch13 = ObjectSystem.switch13;
        playerData.switch14 = ObjectSystem.switch14;
        playerData.switch15 = ObjectSystem.switch15;
        playerData.switch16 = ObjectSystem.switch16;
        playerData.switch17 = ObjectSystem.switch17;
        playerData.switch18 = ObjectSystem.switch18;
        playerData.switch19 = ObjectSystem.switch19;
        playerData.switch20 = ObjectSystem.switch20;
        playerData.switch21 = ObjectSystem.switch21;
        playerData.switch22 = ObjectSystem.switch22;
        playerData.switch23 = ObjectSystem.switch23;
        playerData.switch24 = ObjectSystem.switch24;
        playerData.switch25 = ObjectSystem.switch25;
        playerData.switch26 = ObjectSystem.switch26;
        playerData.switch27 = ObjectSystem.switch27;
        playerData.switch28 = ObjectSystem.switch28;
        playerData.switch29 = ObjectSystem.switch29;
        playerData.switch30 = ObjectSystem.switch30;
        playerData.switch31 = ObjectSystem.switch31;
        playerData.switch32 = ObjectSystem.switch32;
        playerData.switch33 = ObjectSystem.switch33;
        playerData.switch34 = ObjectSystem.switch34;
        playerData.switch35 = ObjectSystem.switch35;


        //Enemies==============================//
        playerData.enemySection1 = ObjectSystem.enemySection1;
        playerData.enemySection2 = ObjectSystem.enemySection2;
        playerData.enemySection3 = ObjectSystem.enemySection3;
        playerData.enemySection4 = ObjectSystem.enemySection4;
        playerData.enemySection5 = ObjectSystem.enemySection5;
        playerData.enemySection6 = ObjectSystem.enemySection6;
        playerData.enemySection7 = ObjectSystem.enemySection7;
        playerData.enemySection8 = ObjectSystem.enemySection8;
        playerData.enemySection9 = ObjectSystem.enemySection9;
        playerData.enemySection10 = ObjectSystem.enemySection10;
        playerData.enemySection11 = ObjectSystem.enemySection11;
        playerData.enemySection12 = ObjectSystem.enemySection12;
        playerData.enemySection13 = ObjectSystem.enemySection13;
        playerData.enemySection14 = ObjectSystem.enemySection14;
        playerData.enemySection15 = ObjectSystem.enemySection15;
        playerData.enemySection16 = ObjectSystem.enemySection16;
        playerData.enemySection17 = ObjectSystem.enemySection17;
        playerData.enemySection18 = ObjectSystem.enemySection18;


        //Doors================================//
        playerData.door1 = ObjectSystem.door1;
        playerData.door2 = ObjectSystem.door2;
        playerData.door3 = ObjectSystem.door3;
        playerData.door4 = ObjectSystem.door4;
        playerData.door5 = ObjectSystem.door5;
        playerData.door6 = ObjectSystem.door6;
        playerData.door7 = ObjectSystem.door7;
        playerData.door8 = ObjectSystem.door8;
        playerData.door9 = ObjectSystem.door9;
        playerData.door10 = ObjectSystem.door10;
        playerData.door11 = ObjectSystem.door11;
        playerData.door12 = ObjectSystem.door12;
        playerData.door13 = ObjectSystem.door13;
        playerData.door14 = ObjectSystem.door14;
        playerData.door15 = ObjectSystem.door15;
        //Events===============================//
        playerData.event1 = ObjectSystem.event1;
        playerData.event2 = ObjectSystem.event2;
        playerData.event3 = ObjectSystem.event3;
        playerData.event4 = ObjectSystem.event4;
        playerData.event5 = ObjectSystem.event5;

        //Entries===============================//
        playerData.entry1 = ObjectSystem.entry1;
        playerData.entry2 = ObjectSystem.entry2;
        playerData.entry3 = ObjectSystem.entry3;
        playerData.entry4 = ObjectSystem.entry4;
        playerData.entry5 = ObjectSystem.entry5;
        playerData.entry6 = ObjectSystem.entry6;
        playerData.entry7 = ObjectSystem.entry7;
        playerData.entry8 = ObjectSystem.entry8;

        //============================PlayerSystem============================================//
        playerData.playerPosition.x = PlayerSystem.lastPosition.x;
        playerData.playerPosition.y = PlayerSystem.lastPosition.y;
        playerData.playerPosition.z = PlayerSystem.lastPosition.z;
        playerData.playerRotation.x = PlayerSystem.lastRotation.x;
        playerData.playerRotation.y = PlayerSystem.lastRotation.y;
        playerData.playerRotation.z = PlayerSystem.lastRotation.z;
        playerData.playerRotation.w = PlayerSystem.lastRotation.w;
        playerData.playerHealth = PlayerSystem.playerHealth;
        playerData.playerVitality = PlayerSystem.playerVitality;
        playerData.playerGold = PlayerSystem.playerGold;
        //===========================ItemSystem==============================================//
        playerData.lifeBerryAmt = ItemSystem.lifeBerryAmt;
        playerData.rixileAmt = ItemSystem.rixileAmt;
        playerData.nicirPlantAmt = ItemSystem.nicirPlantAmt;
        playerData.mindRemedyAmt = ItemSystem.mindRemedyAmt;
        playerData.petriShroomAmt = ItemSystem.petriShroomAmt;
        playerData.reliefOintmentAmt = ItemSystem.reliefOintmentAmt;
        playerData.terraIdolAmt = ItemSystem.terraIdolAmt;
        playerData.magicIdolAmt = ItemSystem.magicIdolAmt;
        playerData.keyAmt = ItemSystem.keyAmt;
        playerData.demonMaskPickedUp = ItemSystem.demonMaskPickedUp;
        playerData.moonPearlPickedUp = ItemSystem.moonPearlPickedUp;
        playerData.markOfEtymologyPickedUp = ItemSystem.markOfEtymologyPickedUp;
        //===========================SceneSystem==============================================//
        playerData.scene = SceneSystem.scene;
        //===========================SwordSystem==============================================//
        playerData.UnArmed = SwordSystem.UnArmed;
        playerData.flareSwordPickedUp = SwordSystem.flareSwordPickedUp;
        playerData.lightningSwordPickedUp = SwordSystem.lightningSwordPickedUp;
        playerData.frostSwordPickedUp = SwordSystem.frostSwordPickedUp;
        playerData.terraSwordPickedUp = SwordSystem.terraSwordPickedUp;
        playerData.flareSwordEnabled = SwordSystem.flareSwordEnabled;
        playerData.lightningSwordEnabled = SwordSystem.lightningSwordEnabled;
        playerData.frostSwordEnabled = SwordSystem.frostSwordEnabled;
        playerData.terraSwordEnabled = SwordSystem.terraSwordEnabled;
        //===========================JewelSystem==============================================//
        playerData.isJewelEnabled = JewelSystem.isJewelEnabled;
        playerData.redJewelPickedUp = JewelSystem.redJewelPickedUp;
        playerData.blueJewelPickedUp = JewelSystem.blueJewelPickedUp;
        playerData.greenJewelPickedUp = JewelSystem.greenJewelPickedUp;
        playerData.yellowJewelPickedUp = JewelSystem.yellowJewelPickedUp;
        playerData.purpleJewelPickedUp = JewelSystem.purpleJewelPickedUp;
        playerData.redJewelEnabled = JewelSystem.redJewelEnabled;
        playerData.blueJewelEnabled = JewelSystem.blueJewelEnabled;
        playerData.greenJewelEnabled = JewelSystem.greenJewelEnabled;
        playerData.yellowJewelEnabled = JewelSystem.yellowJewelEnabled;
        playerData.purpleJewelEnabled = JewelSystem.purpleJewelEnabled;
        //===========================ArmorSystem==============================================//
        playerData.helixArmorPickedUp = PlayerSystem.helixArmorPickedUp;
        playerData.apexArmorPickedUp = PlayerSystem.apexArmorPickedUp;
        playerData.mythicArmorPickedUp = PlayerSystem.mythicArmorPickedUp;
        playerData.legendaryArmorPickedUp = PlayerSystem.legendaryArmorPickedUp;
        playerData.helixArmorEnabled = PlayerSystem.helixArmorEnabled;
        playerData.apexArmorEnabled = PlayerSystem.apexArmorEnabled;
        playerData.mythicArmorEnabled = PlayerSystem.mythicArmorEnabled;
        playerData.legendaryArmorEnabled = PlayerSystem.legendaryArmorEnabled;
        //===========================BootSystem==============================================//
        playerData.hoverBootsPickedUp = PlayerSystem.hoverBootPickedUp;
        playerData.cinderbootsPickedUp = PlayerSystem.cinderBootPickedUp;
        playerData.stormBootsPickedUp = PlayerSystem.stormBootPickedUp;
        playerData.medicalBootsPickedUp = PlayerSystem.medicalBootPickedUp;
        playerData.hoverBootsEnabled = PlayerSystem.hoverBootEnabled;
        playerData.cinderBootsEnabled = PlayerSystem.cinderBootEnabled;
        playerData.stormbootsEnabled = PlayerSystem.stormBootEnabled;
        playerData.medicalbootsEnabled = PlayerSystem.medicalBootEnabled;
        bf.Serialize(stream, playerData);
        stream.Close();

        QuitWithSave();

    }
    public IEnumerator QuitWait()
    {
        yield return new WaitForSecondsRealtime(1);
        interactionSystem.DialogueInteraction(true, "Game saved successfully!");
        yield return new WaitForSecondsRealtime(1);
        interactionSystem.DialogueInteraction(false, null);
        dialogueSystem.PressButton(DialogueSystem.ButtonType.quit);
        yield break;
    }
    public void QuitWithoutSave()
    {
        dialogueSystem.PressButton(DialogueSystem.ButtonType.quit);
    }
    public void QuitWithSave()
    {
        if (routine != null)
            StopCoroutine(routine);
        routine = QuitWait();
        StartCoroutine(routine);

    }
    public void SaveTheGame()
    {
        if (!isSaving && !SceneSystem.isDisabled && PauseGame.isPaused)
            isSaving = true;
    }
    public void LoadGame()
    {
        for(int sf = 0; sf < 4; sf++)
        {
            if (File.Exists(Application.dataPath + SAVE_FILENAME[sf] + FILE_EXTENSION) && !newGame[sf] && fileActive[sf])
                SAVE_FILE = SAVE_FILENAME[sf];
        }
        if (SAVE_FILE != null)
        {
            Stream stream = File.Open(Application.dataPath + SAVE_FILE + FILE_EXTENSION, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            playerData = (PlayerData)bf.Deserialize(stream);
            stream.Close();
            timeSystem.LoadTimeOfDay(playerData.totalGameTime, playerData.currentTimeOfDay, playerData.timeInterval, playerData.day, playerData.hour,
                                     playerData.minute, playerData.isNight, playerData.isRain, playerData.dayNightDisplay);

            playerSystem.LoadPlayerSystem(playerData.playerHealth, playerData.playerVitality, playerData.playerGold, 
                                          playerData.playerPosition, playerData.playerRotation);

            itemSystem.LoadItemSystem(playerData.lifeBerryAmt, playerData.rixileAmt, playerData.nicirPlantAmt, playerData.mindRemedyAmt, playerData.petriShroomAmt,
                                      playerData.reliefOintmentAmt, playerData.terraIdolAmt, playerData.magicIdolAmt, playerData.keyAmt, playerData.demonMaskPickedUp, 
                                      playerData.moonPearlPickedUp, playerData.markOfEtymologyPickedUp);

            swordSystem.LoadSwordSettings(playerData.UnArmed, playerData.flareSwordPickedUp, playerData.flareSwordEnabled, playerData.lightningSwordPickedUp, 
                                          playerData.lightningSwordEnabled, playerData.frostSwordPickedUp, playerData.frostSwordEnabled, 
                                          playerData.terraSwordPickedUp, playerData.terraSwordEnabled);

            playerSystem.LoadArmorSettings(playerData.helixArmorPickedUp, playerData.helixArmorEnabled, playerData.apexArmorPickedUp, 
                                           playerData.apexArmorEnabled, playerData.mythicArmorPickedUp, playerData.mythicArmorEnabled, 
                                           playerData.legendaryArmorPickedUp, playerData.legendaryArmorEnabled);

            playerSystem.LoadBootSettings(playerData.hoverBootsPickedUp, playerData.hoverBootsEnabled, playerData.cinderbootsPickedUp,
                                          playerData.cinderBootsEnabled, playerData.stormBootsPickedUp, playerData.stormbootsEnabled,
                                          playerData.medicalBootsPickedUp, playerData.medicalbootsEnabled);

            jewelSystem.LoadJewels(playerData.isJewelEnabled, playerData.redJewelPickedUp, playerData.redJewelEnabled, playerData.blueJewelPickedUp, 
                                   playerData.blueJewelEnabled, playerData.greenJewelPickedUp, playerData.greenJewelEnabled, playerData.yellowJewelPickedUp, 
                                   playerData.yellowJewelEnabled, playerData.purpleJewelPickedUp, playerData.purpleJewelEnabled);

            shopSystem.LoadVitalityBought(playerData.storeVitality1, playerData.storeVitality2, playerData.storeVitality3);

            SetGameProgress(playerData.gameProgress);

            objectSystem.LoadSwitches(1, playerData.switch1);
            objectSystem.LoadSwitches(2, playerData.switch2);
            objectSystem.LoadSwitches(3, playerData.switch3);
            objectSystem.LoadSwitches(4, playerData.switch4);
            objectSystem.LoadSwitches(5, playerData.switch5);

            objectSystem.LoadSwitches(6, playerData.switch6);
            objectSystem.LoadSwitches(7, playerData.switch7);
            objectSystem.LoadSwitches(8, playerData.switch8);
            objectSystem.LoadSwitches(9, playerData.switch9);
            objectSystem.LoadSwitches(10, playerData.switch10);
            objectSystem.LoadSwitches(11, playerData.switch11);
            objectSystem.LoadSwitches(12, playerData.switch12);
            objectSystem.LoadSwitches(13, playerData.switch13);
            objectSystem.LoadSwitches(14, playerData.switch14);
            objectSystem.LoadSwitches(15, playerData.switch15);
            objectSystem.LoadSwitches(16, playerData.switch16);
            objectSystem.LoadSwitches(17, playerData.switch17);
            objectSystem.LoadSwitches(18, playerData.switch18);
            objectSystem.LoadSwitches(19, playerData.switch19);
            objectSystem.LoadSwitches(20, playerData.switch20);
            objectSystem.LoadSwitches(21, playerData.switch21);
            objectSystem.LoadSwitches(22, playerData.switch22);
            objectSystem.LoadSwitches(23, playerData.switch23);
            objectSystem.LoadSwitches(24, playerData.switch24);
            objectSystem.LoadSwitches(25, playerData.switch25);
            objectSystem.LoadSwitches(26, playerData.switch26);
            objectSystem.LoadSwitches(27, playerData.switch27);
            objectSystem.LoadSwitches(28, playerData.switch28);
            objectSystem.LoadSwitches(29, playerData.switch29);
            objectSystem.LoadSwitches(30, playerData.switch30);
            objectSystem.LoadSwitches(31, playerData.switch31);
            objectSystem.LoadSwitches(32, playerData.switch32);
            objectSystem.LoadSwitches(33, playerData.switch33);
            objectSystem.LoadSwitches(34, playerData.switch34);
            objectSystem.LoadSwitches(35, playerData.switch35);


            objectSystem.LoadEnemies(1, playerData.enemySection1);
            objectSystem.LoadEnemies(2, playerData.enemySection2);
            objectSystem.LoadEnemies(3, playerData.enemySection3);
            objectSystem.LoadEnemies(4, playerData.enemySection4);
            objectSystem.LoadEnemies(5, playerData.enemySection5);
            objectSystem.LoadEnemies(6, playerData.enemySection6);
            objectSystem.LoadEnemies(7, playerData.enemySection7);
            objectSystem.LoadEnemies(8, playerData.enemySection8);
            objectSystem.LoadEnemies(9, playerData.enemySection9);
            objectSystem.LoadEnemies(10, playerData.enemySection10);
            objectSystem.LoadEnemies(11, playerData.enemySection11);
            objectSystem.LoadEnemies(12, playerData.enemySection12);
            objectSystem.LoadEnemies(13, playerData.enemySection13);
            objectSystem.LoadEnemies(14, playerData.enemySection14);
            objectSystem.LoadEnemies(15, playerData.enemySection15);
            objectSystem.LoadEnemies(16, playerData.enemySection16);
            objectSystem.LoadEnemies(17, playerData.enemySection17);
            objectSystem.LoadEnemies(18, playerData.enemySection18);

            objectSystem.LoadChests(1, playerData.chest1);
            objectSystem.LoadChests(2, playerData.chest2);
            objectSystem.LoadChests(3, playerData.chest3);
            objectSystem.LoadChests(4, playerData.chest4);
            objectSystem.LoadChests(5, playerData.chest5);
            objectSystem.LoadChests(6, playerData.chest6);
            objectSystem.LoadChests(7, playerData.chest7);
            objectSystem.LoadChests(8, playerData.chest8);

            objectSystem.LoadChests(9, playerData.chest9);
            objectSystem.LoadChests(10, playerData.chest10);
            objectSystem.LoadChests(11, playerData.chest11);
            objectSystem.LoadChests(12, playerData.chest12);

            objectSystem.LoadChests(13, playerData.chest13);
            objectSystem.LoadChests(14, playerData.chest14);
            objectSystem.LoadChests(15, playerData.chest15);
            objectSystem.LoadChests(16, playerData.chest16);
            objectSystem.LoadChests(17, playerData.chest17);
            objectSystem.LoadChests(18, playerData.chest18);
            objectSystem.LoadChests(19, playerData.chest19);
            objectSystem.LoadChests(20, playerData.chest20);
            objectSystem.LoadChests(21, playerData.chest21);
            objectSystem.LoadChests(22, playerData.chest22);

            objectSystem.LoadChests(23, playerData.chest23);
            objectSystem.LoadChests(24, playerData.chest24);
            objectSystem.LoadChests(25, playerData.chest25);
            objectSystem.LoadChests(26, playerData.chest26);
            objectSystem.LoadChests(27, playerData.chest27);
            objectSystem.LoadChests(28, playerData.chest28);
            objectSystem.LoadChests(29, playerData.chest29);
            objectSystem.LoadChests(30, playerData.chest30);
            objectSystem.LoadChests(31, playerData.chest31);
            objectSystem.LoadChests(32, playerData.chest32);
            objectSystem.LoadChests(33, playerData.chest33);
            objectSystem.LoadChests(34, playerData.chest34);

            objectSystem.LoadChests(35, playerData.chest35);
            objectSystem.LoadChests(36, playerData.chest36);
            objectSystem.LoadChests(37, playerData.chest37);
            objectSystem.LoadChests(38, playerData.chest38);
            objectSystem.LoadChests(39, playerData.chest39);
            objectSystem.LoadChests(40, playerData.chest40);
            objectSystem.LoadChests(41, playerData.chest41);
            objectSystem.LoadChests(42, playerData.chest42);
            objectSystem.LoadChests(43, playerData.chest43);
            objectSystem.LoadChests(44, playerData.chest44);
            objectSystem.LoadChests(45, playerData.chest45);
            objectSystem.LoadChests(46, playerData.chest46);
            objectSystem.LoadChests(47, playerData.chest47);
            objectSystem.LoadChests(48, playerData.chest48);
            objectSystem.LoadChests(49, playerData.chest49);
            objectSystem.LoadChests(50, playerData.chest50);
            objectSystem.LoadChests(51, playerData.chest51);
            objectSystem.LoadChests(52, playerData.chest52);
            objectSystem.LoadChests(53, playerData.chest53);
            objectSystem.LoadChests(54, playerData.chest54);
            objectSystem.LoadChests(55, playerData.chest55);
            objectSystem.LoadChests(56, playerData.chest56);
            objectSystem.LoadChests(57, playerData.chest57);
            objectSystem.LoadChests(59, playerData.chest58);
            objectSystem.LoadChests(59, playerData.chest59);
            objectSystem.LoadChests(60, playerData.chest60);
            objectSystem.LoadChests(61, playerData.chest61);
            objectSystem.LoadChests(62, playerData.chest62);

            objectSystem.LoadDoors(1, playerData.door1);
            objectSystem.LoadDoors(2, playerData.door2);
            objectSystem.LoadDoors(3, playerData.door3);
            objectSystem.LoadDoors(4, playerData.door4);
            objectSystem.LoadDoors(5, playerData.door5);
            objectSystem.LoadDoors(6, playerData.door6);
            objectSystem.LoadDoors(7, playerData.door7);
            objectSystem.LoadDoors(8, playerData.door8);
            objectSystem.LoadDoors(9, playerData.door9);
            objectSystem.LoadDoors(10, playerData.door10);
            objectSystem.LoadDoors(11, playerData.door11);
            objectSystem.LoadDoors(12, playerData.door12);
            objectSystem.LoadDoors(13, playerData.door13);
            objectSystem.LoadDoors(14, playerData.door14);
            objectSystem.LoadDoors(15, playerData.door15);

            objectSystem.LoadEvents(1, playerData.event1);
            objectSystem.LoadEvents(2, playerData.event2);
            objectSystem.LoadEvents(3, playerData.event3);
            objectSystem.LoadEvents(4, playerData.event4);
            objectSystem.LoadEvents(5, playerData.event5);


            objectSystem.LoadJournals(1, playerData.entry1);
            objectSystem.LoadJournals(2, playerData.entry2);
            objectSystem.LoadJournals(3, playerData.entry3);
            objectSystem.LoadJournals(4, playerData.entry4);
            objectSystem.LoadJournals(5, playerData.entry5);
            objectSystem.LoadJournals(6, playerData.entry6);
            objectSystem.LoadJournals(7, playerData.entry7);
            objectSystem.LoadJournals(8, playerData.entry8);

            storySystem.LoadPreviousEntries();
        }
    }
    public void LoadScene()
    {
        for (int sf = 0; sf < 4; sf++)
        {
            if (File.Exists(Application.dataPath + SAVE_FILENAME[sf] + FILE_EXTENSION) && !newGame[sf] && fileActive[sf])
                SAVE_FILE = SAVE_FILENAME[sf];
        }
        if (SAVE_FILE != null)
        {
            Stream stream = File.Open(Application.dataPath + SAVE_FILE + FILE_EXTENSION, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            playerData = (PlayerData)bf.Deserialize(stream);
            stream.Close();
        }
        sceneSystem.LoadSceneSystem(playerData.scene);
    }
    public void LoadFileExisting()
    {
        for (int sf = 0; sf < 4; sf++)
        {
            if (File.Exists(Application.dataPath + SAVE_FILENAME[sf] + FILE_EXTENSION))
            {
                newGame[sf] = false;
                fileText[sf].text = null;
                if (File.Exists(Application.dataPath + "/" + LOAD_IMAGENAMES[sf] + PIC_EXTENSION))
                {
                    fileScreenShot[sf].enabled = true;
                    screenShot = fileScreenShot[sf];
                    string path = Application.dataPath + "/" + LOAD_IMAGENAMES[sf] + PIC_EXTENSION;
                    var loadImageFile = File.ReadAllBytes(path);
                    Texture2D texture = new Texture2D(127, 120);
                    texture.LoadImage(loadImageFile);
                    screenShot.texture = texture;
                }
            }
                
        }
    }
    public void DeleteExistingFile(int num)
    {
        if (SelectComponents.FileDeletion)
        {

            for(int fa = 0; fa < 4; fa++)
            {
                if(fa == num)
                {
                    if (MMTransition.fileOpen[fa])
                    {
                        if (File.Exists(Application.dataPath + SAVE_FILENAME[fa] + FILE_EXTENSION))
                        {
                            File.Delete(Application.dataPath + SAVE_FILENAME[fa] + FILE_EXTENSION);
                            AudioSystem.PlayAudioSource(menuFx[0], 1, 1);
                        }
                        else if (!File.Exists(Application.dataPath + SAVE_FILENAME[fa] + FILE_EXTENSION))
                            AudioSystem.PlayAudioSource(menuFx[1], 1, 1);
                        if (File.Exists(Application.dataPath + "/" + LOAD_IMAGENAMES[fa] + PIC_EXTENSION))
                        {
                            newGame[fa] = true;
                            fileScreenShot[fa].texture = defaultTexture;
                            File.Delete(Application.dataPath + "/" + LOAD_IMAGENAMES[fa] + PIC_EXTENSION);

                        }
                    }
                }
            }
        }
    }
    public void ResetActiveFiles()
    {
        for (int fa = 0; fa < 4; fa++)
            fileActive[fa] = false;
    }
    public void SetGameProgress(float amount)
    {
        gameProgress += amount;
        ApplyGameProgress();
    }
    public void ApplyGameProgress()
    {
        progressPercentText.text = Mathf.FloorToInt(gameProgress).ToString() + "%";
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
    [Serializable]
    public class PlayerData
    {
        public float totalGameTime;

        public int day;
        public int hour;
        public int minute;
        public int timeInterval;
        public float currentTimeOfDay;
        public bool isNight;
        public bool isRain;
        public string dayNightDisplay;

        public float gameProgress;

        public SerializableQuaternion playerRotation;
        public SerializableVector3 playerPosition;

        public int playerHealth;
        public int playerVitality;
        public int playerGold;

        public bool storeVitality1;
        public bool storeVitality2;
        public bool storeVitality3;

        public int lifeBerryAmt;
        public int rixileAmt;
        public int nicirPlantAmt;
        public int mindRemedyAmt;
        public int petriShroomAmt;
        public int reliefOintmentAmt;
        public int terraIdolAmt;
        public int magicIdolAmt;
        public int keyAmt;
        public bool demonMaskPickedUp;
        public bool moonPearlPickedUp;
        public bool markOfEtymologyPickedUp;

        public bool UnArmed;

        public bool flareSwordPickedUp;
        public bool lightningSwordPickedUp;
        public bool frostSwordPickedUp;
        public bool terraSwordPickedUp;

        public bool flareSwordEnabled;
        public bool lightningSwordEnabled;
        public bool frostSwordEnabled;
        public bool terraSwordEnabled;

        public bool helixArmorPickedUp;
        public bool apexArmorPickedUp;
        public bool mythicArmorPickedUp;
        public bool legendaryArmorPickedUp;

        public bool helixArmorEnabled;
        public bool apexArmorEnabled;
        public bool mythicArmorEnabled;
        public bool legendaryArmorEnabled;

        public bool hoverBootsPickedUp;
        public bool cinderbootsPickedUp;
        public bool stormBootsPickedUp;
        public bool medicalBootsPickedUp;

        public bool hoverBootsEnabled;
        public bool cinderBootsEnabled;
        public bool stormbootsEnabled;
        public bool medicalbootsEnabled;

        public bool redJewelPickedUp;
        public bool blueJewelPickedUp;
        public bool greenJewelPickedUp;
        public bool yellowJewelPickedUp;
        public bool purpleJewelPickedUp;

        public bool redJewelEnabled;
        public bool blueJewelEnabled;
        public bool greenJewelEnabled;
        public bool yellowJewelEnabled;
        public bool purpleJewelEnabled;

        public bool isJewelEnabled;

        public int scene;

        public bool switch1;
        public bool switch2;
        public bool switch3;
        public bool switch4;
        public bool switch5;
        public bool switch6;
        public bool switch7;
        public bool switch8;
        public bool switch9;
        public bool switch10;
        public bool switch11;
        public bool switch12;
        public bool switch13;
        public bool switch14;
        public bool switch15;
        public bool switch16;
        public bool switch17;
        public bool switch18;
        public bool switch19;
        public bool switch20;
        public bool switch21;
        public bool switch22;
        public bool switch23;
        public bool switch24;
        public bool switch25;
        public bool switch26;
        public bool switch27;
        public bool switch28;
        public bool switch29;
        public bool switch30;
        public bool switch31;
        public bool switch32;
        public bool switch33;
        public bool switch34;
        public bool switch35;

        public bool enemySection1;
        public bool enemySection2;
        public bool enemySection3;
        public bool enemySection4;
        public bool enemySection5;
        public bool enemySection6;
        public bool enemySection7;
        public bool enemySection8;
        public bool enemySection9;
        public bool enemySection10;
        public bool enemySection11;
        public bool enemySection12;
        public bool enemySection13;
        public bool enemySection14;
        public bool enemySection15;
        public bool enemySection16;
        public bool enemySection17;
        public bool enemySection18;

        public bool chest1;
        public bool chest2;
        public bool chest3;
        public bool chest4;
        public bool chest5;
        public bool chest6;
        public bool chest7;
        public bool chest8;
        public bool chest9;
        public bool chest10;
        public bool chest11;
        public bool chest12;

        public bool chest13;
        public bool chest14;
        public bool chest15;
        public bool chest16;
        public bool chest17;
        public bool chest18;
        public bool chest19;
        public bool chest20;
        public bool chest21;
        public bool chest22;

        public bool chest23;
        public bool chest24;
        public bool chest25;
        public bool chest26;
        public bool chest27;
        public bool chest28;
        public bool chest29;
        public bool chest30;
        public bool chest31;
        public bool chest32;
        public bool chest33;
        public bool chest34;

        public bool chest35;
        public bool chest36;
        public bool chest37;
        public bool chest38;
        public bool chest39;
        public bool chest40;
        public bool chest41;
        public bool chest42;
        public bool chest43;
        public bool chest44;
        public bool chest45;
        public bool chest46;
        public bool chest47;
        public bool chest48;
        public bool chest49;
        public bool chest50;
        public bool chest51;
        public bool chest52;

        public bool chest53;
        public bool chest54;
        public bool chest55;
        public bool chest56;
        public bool chest57;

        public bool chest58;
        public bool chest59;
        public bool chest60;
        public bool chest61;
        public bool chest62;

        public bool door1;

        public bool door2;
        public bool door3;
        public bool door4;

        public bool door5;
        public bool door6;

        public bool door7;
        public bool door8;
        public bool door9;
        public bool door10;
        public bool door11;
        public bool door12;
        public bool door13;
        public bool door14;
        public bool door15;

        public bool event1;
        public bool event2;
        public bool event3;
        public bool event4;
        public bool event5;

        public bool entry1;
        public bool entry2;
        public bool entry3;
        public bool entry4;
        public bool entry5;
        public bool entry6;
        public bool entry7;
        public bool entry8;
        public bool entry9;
    }
    [Serializable]
    public struct SerializableVector3
    {
        public float x;
        public float y;
        public float z;
        public SerializableVector3(float rX, float rY, float rZ)
        {
            x = rX;
            y = rY;
            z = rZ;
        }
        public override string ToString()
        {
            return String.Format("[{0}, {1}, {2}]", x, y, z);
        }
        public static implicit operator Vector3(SerializableVector3 rValue)
        {
            return new Vector3(rValue.x, rValue.y, rValue.z);
        }
        public static implicit operator SerializableVector3(Vector3 rValue)
        {
            return new SerializableVector3(rValue.x, rValue.y, rValue.z);
        }
    }
    [Serializable]
    public struct SerializableQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public SerializableQuaternion(float rX, float rY, float rZ, float rW)
        {
            x = rX;
            y = rY;
            z = rZ;
            w = rW;
        }
        public override string ToString()
        {
            return String.Format("[{0}, {1}, {2}, {3}]", x, y, z, w);
        }
        public static implicit operator Quaternion(SerializableQuaternion rValue)
        {
            return new Quaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }
        public static implicit operator SerializableQuaternion(Quaternion rValue)
        {
            return new SerializableQuaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }
    }
}
