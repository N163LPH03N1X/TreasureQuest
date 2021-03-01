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
        dialogueSystem = gamePlayer.GetComponent<DialogueSystem>();
        objectSystem = GetComponent<ObjectSystem>();
        timeSystem = transform.GetChild(0).GetChild(0).GetComponent<TimeSystem>();
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
        playerSystem.SaveCoordinates();
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
        playerData.gameVitality = ShopSystem.vitalityBought;
        //Chests===============================//
        playerData.gameChest = ObjectSystem.gameChest;
        //Switches=============================//
        playerData.gameSwitch = ObjectSystem.gameSwitch;
        //Enemies==============================//
        playerData.gameEnemySection = ObjectSystem.gameEnemySection;
        //Doors================================//
        playerData.gameDoor = ObjectSystem.gameDoor;
        //Events===============================//
        playerData.gameEvent = ObjectSystem.gameEvent;
        //Entries===============================//
        playerData.gameEntry = ObjectSystem.gameEntry;
        //============================PlayerSystem============================================//
        playerData.playerPosition = PlayerSystem.lastPlayerPosition;
        playerData.playerRotation = PlayerSystem.lastPlayerRotation;
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

            shopSystem.LoadVitalityBought(playerData.gameVitality);
            SetGameProgress(playerData.gameProgress);

            ObjectSystem.gameSwitch = playerData.gameSwitch;
            ObjectSystem.gameEnemySection = playerData.gameEnemySection;
            ObjectSystem.gameChest = playerData.gameChest;
            ObjectSystem.gameDoor = playerData.gameDoor;
            ObjectSystem.gameEvent = playerData.gameEvent;
            ObjectSystem.gameEntry = playerData.gameEntry;
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

        public bool[] gameVitality;
        public bool[] gameSwitch;
        public bool[] gameEnemySection;
        public bool[] gameChest;
        public bool[] gameDoor;
        public bool[] gameEvent;
        public bool[] gameEntry;
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
            return string.Format("[{0}, {1}, {2}]", x, y, z);
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
            return string.Format("[{0}, {1}, {2}, {3}]", x, y, z, w);
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
