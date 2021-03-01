using UnityEngine;
using UnityEngine.UI;
public class StorySystem : MonoBehaviour
{
    private Selectable m_Selectable;
    private ObjectSystem objectSystem;

    [Header("Story UI Settings")]
    public Text journalEntryText;
    public Text bookTitle;
    public Text bookDescription;

    public Button[] decodedScriptBooks;
    public Image[] decodedScriptIcons;
    public Text[] decodedScriptValues;

    public Button[] islandHistoryBooks;
    public Image[] islandHistoryIcons;
    public Text[] islandHistoryValues;

    public Button[] ancientDiscoveryBooks;
    public Image[] ancientDiscoveryIcons;
    public Text[] ancientDiscoveryValues;

    public GameObject journalStatus;

    [Header("Book Settings")]
    public GameObject book;
    public Text leftChapter;
    public Text rightChapter;
    public Text leftDialogue;
    public Text rightDialogue;
    public Button bookPageNext;
    public Button bookPageBack;
    [Space]
    [Header("Plaque Settings")]
    public GameObject plaque;
    public Text centerDialogue;
    public Text centerChapter;
    public Button aButtonPlaque;
    [Space]
    [Header("Sign Settings")]
    public GameObject sign;
    int signNum;
    public Sprite[] signPictures;
    public Image picture;
    
    [Space]
    [Header("CodeBook Settings")]
    public GameObject codeBook;
    int codeNum;
    public Sprite[] decypheredPictures;
    public Sprite[] scrambledPictures;
    public Image decyphered;
    public Image scrambled;
    public Button aButtonCode;
    [Space]
    public Button aButtonSign;
    public GameObject StoryObject;
    public Image background;
    public Sprite[] backgroundImages;

    int totalNumberOfPages;
    string[] page = new string[8];
    string[] chapter = new string[2];

    int pageNumber = 1;
    public static bool isReading = false;
    
    public static bool isBook;
    bool isPlaque;
    bool isSign;
    bool isCodeBook;

    public static int journalEntry = 0;

    public GameObject contentStructure;

    public void StartStory(bool active)
    {
        if (active)
        {
                      
            OpenStory(true);
            isReading = true;
            if (isBook)
            {
                pageNumber = 1;
                SetBook(pageNumber);
            }
            else if (isPlaque)
                SetPlaque();
            else if (isSign)
                SetSign();
            else if (isCodeBook)
                SetCodeBook();
        }
        else
        {
            OpenStory(false);
            isReading = false;
            pageNumber = 0;
        }
    }
    public enum Story { fisherman, Plaque1, Plaque2, Plaque3, Plaque4, Plaque5, Plaque6, Sign1, Sign2, Sign3, Sign4, codeBook1, codeBook2, ghostShip }
    public void ChangeStory(Story type)
    {
        objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
        switch (type)
        {
            //==========================================Books=================================//
            case Story.fisherman:
                {
                    isBook = true;
                    isPlaque = false;
                    isSign = false;
                    isCodeBook = false;
                    book.SetActive(true);
                    plaque.SetActive(false);
                    sign.SetActive(false);
                    codeBook.SetActive(false);
                    background.sprite = backgroundImages[0];
                    totalNumberOfPages = 2;
                    if (totalNumberOfPages > 1)
                        bookPageBack.enabled = true;
                    else
                        bookPageBack.enabled = false;
                    chapter[0] = "*Journal Entry* [0.2] (45/5/2) Fishing Contest";
                    chapter[1] = "Fishing Contest - Continued...";

                    page[0] = "During the time fishing season was at its peak, I bought a special lure to catch one of the finest hassian for the biggest fish contest. " +
                                  "After my fifth cast with no luck, I happened to give up all hope of winning the prize.";
                    page[1] = "All of a sudden my hook got caught, immediately I started to reel in. It felt like my lucky day I couldnt tell what it was but it sure was heavy. " +
                                  "As I reeled it closer, there was a very strange, red glow following my hook. " +
                                  "Once I got it close enough to the ledge I noticed that it was a sword of some sort. It was nothing I've ever seen before in all my years fishing.";
                    page[2] = "I was disappointed that I didnt catch a big fish and I wasn't sure what to do with this old sword relic. " +
                                  "I knew of a secret cave underneath a bridge nearby, so I secretly stored it in a chest until I found a use for it. " + 
                                  "The cave was completely submerged under water so without the proper equipment I knew no one would find it.";
                    page[3] = "Fishing Contest. [END OF ENTRY]";

                    if (!ObjectSystem.gameEntry[0])
                    {
                        journalEntry++;
                        journalStatus.SetActive(true);
                        objectSystem.SetActiveObject(0, ObjectSystem.gameEntry);
                        SetupEntryUI(Story.fisherman);
                    }
                    break;
                }
            case Story.ghostShip:
                {
                    isBook = true;
                    isPlaque = false;
                    isSign = false;
                    isCodeBook = false;
                    book.SetActive(true);
                    plaque.SetActive(false);
                    sign.SetActive(false);
                    codeBook.SetActive(false);
                    background.sprite = backgroundImages[0];
                    totalNumberOfPages = 2;
                    if (totalNumberOfPages > 1)
                        bookPageBack.enabled = true;
                    else
                        bookPageBack.enabled = false;
                    chapter[0] = "*Journal Entry* [0.5] (65/68/18) Ship Sightings";
                    chapter[1] = "Ship Sightings - Continued...";

                    page[0] = "There were tales of unforseen ships scavaging the seas, " +
                        "rumors heard that there were four death ships that only appeared at night off the coastline of conquest island. " +
                        "I discovered that one of the ships appeared in the same spot every thursday of that week at a coastline, " +
                        "another one scaled the island when it was storming.";
                    page[1] = "I have investigated skulk cove for some clues on the where abouts of " +
                        "the other two ships and came up with nothing but until I met an old man in the town " +
                        "that had faint knowledge of a map. Apparently he knew of an ancient ship map that could reveal the locations of the ships, " +
                        "but appointed that the map was a only legend and disappeared long after the great war.";
                    page[2] = "The map only revealed four artifacts and if holding one of them, would reveal one of the ships. " +
                        "I have left my house in search of these ancient artifacts and more history about this secret discovery.";
                    page[3] = "Ship Sightings. [END OF ENTRY]";

                    if (!ObjectSystem.gameEntry[9])
                    {
                        journalEntry++;
                        journalStatus.SetActive(true);
                        objectSystem.SetActiveObject(9, ObjectSystem.gameEntry);
                        SetupEntryUI(Story.ghostShip);
                    }
                    break;
                }
            //==========================================Plaques=================================//
            case Story.Plaque1:
                {
                    isBook = false;
                    isPlaque = true;
                    isSign = false;
                    isCodeBook = false;
                    book.SetActive(false);
                    plaque.SetActive(true);
                    sign.SetActive(false);
                    codeBook.SetActive(false);
                    background.sprite = backgroundImages[1];
                    chapter[0] = "[-The Seekers-]";
                    page[0] = "The Seekers were determined unworthy souls that glanced at the thought of riches, " +
                              "they are blinded by the future role in play if riches has been sought. " +
                              "Not only did they carelessly put themselves in creatures bane, " +
                              "they look puzzled as if the unsolved would be locked away for eternity.";

                    if (!ObjectSystem.gameEntry[1])
                    {
                        journalEntry++;
                        journalStatus.SetActive(true);
                        objectSystem.SetActiveObject(1, ObjectSystem.gameEntry);
                        SetupEntryUI(Story.Plaque1);
                    }

                    break;
                }
            case Story.Plaque2:
                {
                    isBook = false;
                    isPlaque = true;
                    isSign = false;
                    isCodeBook = false;
                    book.SetActive(false);
                    plaque.SetActive(true);
                    sign.SetActive(false);
                    codeBook.SetActive(false);
                    background.sprite = backgroundImages[1];
                    chapter[0] = "[-The Healers-]";
                    page[0] = "The Healers have brought whispers of light and hope for those who are worn, " +
                              "That light may bring magic that can be understood as life itself, not only was it resourceful, " +
                              "but it could allow one to be completely impervious from damage.";
                    if (!ObjectSystem.gameEntry[2])
                    {
                        journalEntry++;
                        journalStatus.SetActive(true);
                        objectSystem.SetActiveObject(2, ObjectSystem.gameEntry);
                        SetupEntryUI(Story.Plaque2);
                    }
                    break;
                }
            case Story.Plaque3:
                {
                    isBook = false;
                    isPlaque = true;
                    isSign = false;
                    isCodeBook = false;
                    book.SetActive(false);
                    plaque.SetActive(true);
                    sign.SetActive(false);
                    codeBook.SetActive(false);
                    background.sprite = backgroundImages[1];
                    chapter[0] = "[-The Ancients-]";
                    page[0] = "The Ancients can be feared by the off guard warrior, " +
                              "if unprepared the foe would likely be petrified to the danger it brings, " +
                              "loosing all hope to the wreckless bounty." +
                              "There may be hope if desired to conquer ones inner struggle, " +
                              "shall they be rewarded with absolute power.";
                    if (!ObjectSystem.gameEntry[3])
                    {
                        journalEntry++;
                        journalStatus.SetActive(true);
                        objectSystem.SetActiveObject(3, ObjectSystem.gameEntry);
                        SetupEntryUI(Story.Plaque3);
                    }
                    break;
                }
            case Story.Plaque4:
                {
                    isBook = false;
                    isPlaque = true;
                    isSign = false;
                    isCodeBook = false;
                    book.SetActive(false);
                    plaque.SetActive(true);
                    sign.SetActive(false);
                    codeBook.SetActive(false);
                    background.sprite = backgroundImages[1];
                    chapter[0] = "[-The Dark Ones-]";
                    page[0] = "The Dark Ones were well traveled through time. " +
                              "Where time itself can change, the dark ones may reveal what once was." +
                              "Choosing to wear the demons possession, the wearer can see deep into the darkness, " +
                              "and will travel among space and time itself, completely shrouded in veil.";
                    if (!ObjectSystem.gameEntry[4])
                    {
                        journalEntry++;
                        journalStatus.SetActive(true);
                        objectSystem.SetActiveObject(4, ObjectSystem.gameEntry);
                        SetupEntryUI(Story.Plaque4);
                    }
                    break;
                }
            case Story.Plaque5:
                {
                    isBook = false;
                    isPlaque = true;
                    isSign = false;
                    isCodeBook = false;
                    book.SetActive(false);
                    plaque.SetActive(true);
                    sign.SetActive(false);
                    codeBook.SetActive(false);
                    background.sprite = backgroundImages[1];
                    chapter[0] = "[-The Light Bringers-]";
                    page[0] = "There were two Light Bringers, " +
                              "conjoined would shed light to any brave foe who has the courage to overcome death itself. " +
                              "If the task was completed, it would ignite the path and guide ones courage within. " +
                              "To conquer the next role in judgement, where there was once light, the Dark Ones can aid what once was.";
                    if (!ObjectSystem.gameEntry[5])
                    {
                        journalEntry++;
                        journalStatus.SetActive(true);
                        objectSystem.SetActiveObject(5, ObjectSystem.gameEntry);
                        SetupEntryUI(Story.Plaque5);
                    }
                    break;
                }
            case Story.Plaque6:
                {
                    isBook = false;
                    isPlaque = true;
                    isSign = false;
                    isCodeBook = false;
                    book.SetActive(false);
                    plaque.SetActive(true);
                    sign.SetActive(false);
                    codeBook.SetActive(false);
                    background.sprite = backgroundImages[1];
                    chapter[0] = "[-The Religious Ones-]";
                    page[0] = "The Religious ones were clever, in depth and retained holyness to the Dark Ones." +
                              "Using the perplexed souls of the sacred mask they could shroud themselves and transform living material from time and space, " +
                              "completely unseen to the eyes of the originals. " +
                              "If the originals seeked for evil, the barriers will be forever unobtainable";
                    if (!ObjectSystem.gameEntry[6])
                    {
                        journalEntry++;
                        journalStatus.SetActive(true);
                        objectSystem.SetActiveObject(6, ObjectSystem.gameEntry);
                        SetupEntryUI(Story.Plaque6);
                    }
                    break;
                }
            //==========================================Signs=================================//
            case Story.Sign1:
                {
                    isBook = false;
                    isPlaque = false;
                    isSign = true;
                    isCodeBook = false;
                    book.SetActive(false);
                    plaque.SetActive(false);
                    sign.SetActive(true);
                    codeBook.SetActive(false);
                    signNum = 0;
                    background.sprite = backgroundImages[2];
                    break;
                }
            case Story.Sign2:
                {
                    isBook = false;
                    isPlaque = false;
                    isSign = true;
                    isCodeBook = false;
                    book.SetActive(false);
                    plaque.SetActive(false);
                    sign.SetActive(true);
                    codeBook.SetActive(false);
                    signNum = 1;
                    background.sprite = backgroundImages[2];
                    break;
                }
            case Story.Sign3:
                {
                    isBook = false;
                    isPlaque = false;
                    isSign = true;
                    isCodeBook = false;
                    book.SetActive(false);
                    plaque.SetActive(false);
                    sign.SetActive(true);
                    codeBook.SetActive(false);
                    signNum = 2;
                    background.sprite = backgroundImages[2];
                    break;
                }
            case Story.Sign4:
                {
                    isBook = false;
                    isPlaque = false;
                    isSign = true;
                    isCodeBook = false;
                    book.SetActive(false);
                    plaque.SetActive(false);
                    sign.SetActive(true);
                    codeBook.SetActive(false);
                    signNum = 3;
                    background.sprite = backgroundImages[2];
                    break;
                }
            case Story.codeBook1:
                {
                    isBook = false;
                    isPlaque = false;
                    isSign = false;
                    isCodeBook = true;
                    book.SetActive(false);
                    plaque.SetActive(false);
                    sign.SetActive(false);
                    codeBook.SetActive(true);
                    codeNum = 0;
                    background.sprite = backgroundImages[3];
                    if (!ObjectSystem.gameEntry[7])
                    {
                        journalEntry++;
                        journalStatus.SetActive(true);
                        objectSystem.SetActiveObject(7, ObjectSystem.gameEntry);
                        SetupEntryUI(Story.codeBook1);
                    }
                    break;
                }
            case Story.codeBook2:
                {
                    isBook = false;
                    isPlaque = false;
                    isSign = false;
                    isCodeBook = true;
                    book.SetActive(false);
                    plaque.SetActive(false);
                    sign.SetActive(false);
                    codeBook.SetActive(true);
                    codeNum = 1;
                    background.sprite = backgroundImages[3];
                    if (!ObjectSystem.gameEntry[8])
                    {
                        journalEntry++;
                        journalStatus.SetActive(true);
                        objectSystem.SetActiveObject(8, ObjectSystem.gameEntry);
                        SetupEntryUI(Story.codeBook2);
                    }
                    break;
                }

        }
        journalEntryText.text = journalEntry + "/10".ToString();
    }
    public void NextPage()
    {
        pageNumber++;
        if (pageNumber > totalNumberOfPages)
        {
            StartStory(false);
            m_Selectable = ancientDiscoveryBooks[0].GetComponent<Selectable>();
            m_Selectable.Select();
        }
        else if (pageNumber <= totalNumberOfPages)
            SetBook(pageNumber);
    }
    public void BackPage()
    {
        pageNumber--;
        if (pageNumber < 1)
        {
            pageNumber = 1;
            SetBook(pageNumber);
        }
        else
            SetBook(pageNumber);
    }
    public void OpenStory(bool active)
    {
        StoryObject.SetActive(active);
        SceneSystem sceneSystem = GameObject.Find("Core/Player").GetComponent<SceneSystem>();
        sceneSystem.EnablePlayer(!active);
    }
    public void SetBook(int num)
    {
        if (num == 1)
        {
            leftChapter.text = chapter[0];
            leftDialogue.text = page[0];
            rightDialogue.text = page[1];
            bookPageBack.interactable = false;
            m_Selectable = bookPageNext.GetComponent<Selectable>();
            m_Selectable.Select();
        }
        else if (num == 2)
        {
            leftChapter.text = chapter[1];
            leftDialogue.text = page[2];
            rightDialogue.text = page[3];
            bookPageBack.interactable = true;
        }


    }
    public void SetPlaque()
    {
        centerChapter.text = chapter[0];
        centerDialogue.text = page[0];
        
    }
    public void SetSign()
    {
        picture.sprite = signPictures[signNum];
    }
    public void SetCodeBook()
    {
        if (ItemSystem.markOfEtymologyEnabled)
        {
            scrambled.enabled = false;
            decyphered.enabled = true;
            decyphered.sprite = decypheredPictures[codeNum];
        }
        else if (!ItemSystem.markOfEtymologyEnabled)
        {
            scrambled.enabled = true;
            decyphered.enabled = false;
            scrambled.sprite = scrambledPictures[codeNum];
        }
    }
    public enum Book { decoded, history, discovery, none }
    public void SetupBookUI(Book type, string ID)
    {
        switch (type)
        {
            case Book.decoded:
                {
                    bookTitle.text = "[-Decoded Script-] " + ID;
                    bookDescription.text = "Deciphered text decoded with the Mark of Etymology. Uncover sacred words of the creator";
                    break;
                }
            case Book.discovery:
                {
                    bookTitle.text = "[-Island Discovery-] " + ID;
                    bookDescription.text = "Information of the people on conquest Island, discover hidden points, unmarked territories and sub quests.";
                    break;
                }
            case Book.history:
                {
                    bookTitle.text = "[-Ancient History-] " + ID;
                    bookDescription.text = "History of ther island, its resources and in which it came to be. Uncover rich secrets undiscovered for centuries.";
                    break;
                }
            case Book.none:
                {
                    bookTitle.text = null;
                    bookDescription.text = null;
                    break;
                }
        }
    }

    //================OnClick======================//
    public void OpenDiscoveryButton(int num)
    {
        if(num == 1)
            ChangeStory(Story.fisherman);
        if (num == 2)
            ChangeStory(Story.ghostShip);
        StartStory(true);
       
    }
    public void OpenHistoryButton(int num)
    {
        if (num == 1)
            ChangeStory(Story.Plaque1);
        else if (num == 2)
            ChangeStory(Story.Plaque2);
        else if (num == 3)
            ChangeStory(Story.Plaque3);
        else if (num == 4)
            ChangeStory(Story.Plaque4);
        else if (num == 5)
            ChangeStory(Story.Plaque5);
        else if (num == 6)
            ChangeStory(Story.Plaque6);
      
        StartStory(true);
        m_Selectable = aButtonPlaque.GetComponent<Selectable>();
        m_Selectable.Select();
    }
    public void OpenDecodedButton(int num)
    {
        if (num == 1)
            ChangeStory(Story.codeBook1);
        if (num == 2)
            ChangeStory(Story.codeBook2);
        StartStory(true);
        m_Selectable = aButtonCode.GetComponent<Selectable>();
        m_Selectable.Select();
    }
    public void SetupEntryUI(Story type)
    {
        switch (type)
        {
            case Story.fisherman:
                {
                    if (ObjectSystem.gameEntry[0])
                    {
                        ancientDiscoveryIcons[0].enabled = true;
                        ancientDiscoveryBooks[0].interactable = true;
                        ancientDiscoveryValues[0].text = "ID-0";
                       
                    }
                    else
                    {
                        ancientDiscoveryIcons[0].enabled = false;
                        ancientDiscoveryBooks[0].interactable = false;
                        ancientDiscoveryValues[0].text = null;
                    }
                    break;
                }
            case Story.ghostShip:
                {
                    if (ObjectSystem.gameEntry[9])
                    {
                        ancientDiscoveryIcons[1].enabled = true;
                        ancientDiscoveryBooks[1].interactable = true;
                        ancientDiscoveryValues[1].text = "ID-1";

                    }
                    else
                    {
                        ancientDiscoveryIcons[1].enabled = false;
                        ancientDiscoveryBooks[1].interactable = false;
                        ancientDiscoveryValues[1].text = null;
                    }
                    break;
                }
            case Story.Plaque1:
                {
                    if (ObjectSystem.gameEntry[1])
                    {
                        islandHistoryIcons[0].enabled = true;
                        islandHistoryBooks[0].interactable = true;
                        islandHistoryValues[0].text = "AH-0";
       
                    }
                    else
                    {
                        islandHistoryIcons[0].enabled = false;
                        islandHistoryBooks[0].interactable = false;
                        islandHistoryValues[0].text = null;
                    }
                    break;
                }
            case Story.Plaque2:
                {
                    if (ObjectSystem.gameEntry[2])
                    {
                        islandHistoryIcons[1].enabled = true;
                        islandHistoryBooks[1].interactable = true;
                        islandHistoryValues[1].text = "AH-1";
             
                    }
                    else
                    {
                        islandHistoryIcons[1].enabled = false;
                        islandHistoryBooks[1].interactable = false;
                        islandHistoryValues[1].text = null;
                    }
                    break;
                }
            case Story.Plaque3:
                {
                    if (ObjectSystem.gameEntry[3])
                    {
                        islandHistoryIcons[2].enabled = true;
                        islandHistoryBooks[2].interactable = true;
                        islandHistoryValues[2].text = "AH-2";

                    }
                    else
                    {
                        islandHistoryIcons[2].enabled = false;
                        islandHistoryBooks[2].interactable = false;
                        islandHistoryValues[2].text = null;
                    }
                    break;
                }
            case Story.Plaque4:
                {
                    if (ObjectSystem.gameEntry[4])
                    {
                        islandHistoryIcons[3].enabled = true;
                        islandHistoryBooks[3].interactable = true;
                        islandHistoryValues[3].text = "AH-3";
 
                    }
                    else
                    {
                        islandHistoryIcons[3].enabled = false;
                        islandHistoryBooks[3].interactable = false;
                        islandHistoryValues[3].text = null;
                    }
                    break;
                }
            case Story.Plaque5:
                {
                    if (ObjectSystem.gameEntry[5])
                    {
                        islandHistoryIcons[4].enabled = true;
                        islandHistoryBooks[4].interactable = true;
                        islandHistoryValues[4].text = "AH-4";
             
                    }
                    else
                    {
                        islandHistoryIcons[4].enabled = false;
                        islandHistoryBooks[4].interactable = false;
                        islandHistoryValues[4].text = null;
                    }
                    break;
                }
            case Story.Plaque6:
                {
                    if (ObjectSystem.gameEntry[6])
                    {
                        islandHistoryIcons[5].enabled = true;
                        islandHistoryBooks[5].interactable = true;
                        islandHistoryValues[5].text = "AH-5";
        
                    }
                    else
                    {
                        islandHistoryIcons[5].enabled = false;
                        islandHistoryBooks[5].interactable = false;
                        islandHistoryValues[5].text = null;
                    }
                    break;
                }
            case Story.codeBook1:
                {
                    if (ObjectSystem.gameEntry[7])
                    {
                        decodedScriptIcons[0].enabled = true;
                        decodedScriptBooks[0].interactable = true;
                        decodedScriptValues[0].text = "DS-0";

                    }
                    else
                    {
                        decodedScriptIcons[0].enabled = false;
                        decodedScriptBooks[0].interactable = false;
                        decodedScriptValues[0].text = null;
                    }
                    break;
                }
            case Story.codeBook2:
                {
                    if (ObjectSystem.gameEntry[8])
                    {
                        decodedScriptIcons[1].enabled = true;
                        decodedScriptBooks[1].interactable = true;
                        decodedScriptValues[1].text = "DS-1";

                    }
                    else
                    {
                        decodedScriptIcons[1].enabled = false;
                        decodedScriptBooks[1].interactable = false;
                        decodedScriptValues[1].text = null;
                    }
                    break;
                }
        }
        
    }
    public void giveJournal(int num)
    {
        objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
        objectSystem.SetActiveObject(num, ObjectSystem.gameEntry);
        LoadPreviousEntries();
        journalEntry++;
        journalEntryText.text = journalEntry + "/10".ToString();
        journalStatus.SetActive(true);
    }
    public void GiveAllJournals(bool on)
    {
        objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
        if (on)
        {
            for(int en = 0; en < ObjectSystem.gameEntry.Length; en++)
                objectSystem.SetActiveObject(en, ObjectSystem.gameEntry);
            LoadPreviousEntries();
            journalEntry = 10;
            journalEntryText.text = journalEntry + "/10".ToString();
            journalStatus.SetActive(true);
        }
        else
        {
            for (int en = 0; en < ObjectSystem.gameEntry.Length; en++)
                objectSystem.SetDeActiveObject(en, ObjectSystem.gameEntry);
            LoadPreviousEntries();
            journalEntry = 0;
            journalEntryText.text = journalEntry + "/10".ToString();
            journalStatus.SetActive(false);
            SetupBookUI(Book.none, null);
        }
    }
    public void LoadPreviousEntries()
    {
        SetupEntryUI(Story.fisherman);
        SetupEntryUI(Story.Plaque1);
        SetupEntryUI(Story.Plaque2);
        SetupEntryUI(Story.Plaque3);
        SetupEntryUI(Story.Plaque4);
        SetupEntryUI(Story.Plaque5);
        SetupEntryUI(Story.Plaque6);
        SetupEntryUI(Story.codeBook1);
        SetupEntryUI(Story.codeBook2);
        SetupEntryUI(Story.ghostShip);

    }
}
