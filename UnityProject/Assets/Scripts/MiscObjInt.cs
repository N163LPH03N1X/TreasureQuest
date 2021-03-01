using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class MiscObjInt : MonoBehaviour
{
    OptSystem optSystem = new OptSystem();
    public string message;
    public StoryType story;
    public int currentStory;
    public string buttonText;

    Text intText;
    bool interactionActive;
    public GameObject interactionObj;
    public GameObject statusPrefab;
    bool inRange;
    bool active;
    public bool OpenStory;
    public bool addDialogue;
    public bool useBButton;
    public bool useAButton;
    public bool useInstant;
    public bool useEvent;
    public int eventNum;
    public GameObject eventObject;
    bool isChecked = false;
    IEnumerator effectRoutine = null;
    InteractionSystem interaction;
    StorySystem storySystem;
    CharacterSystem characterSystem;
    Transform popupCanvas;
    GameObject newInteraction;
    SwordSystem swordSystem;
    public enum StoryType { fisherman, plaque1, plaque2, plaque3, plaque4, plaque5, plaque6, sign1, sign2, sign3, sign4, code1, code2, ghostShip }

    public void SelectStory(StoryType story)
    {
        StorySystem storySystem = GameObject.FindGameObjectWithTag("Player").GetComponent<StorySystem>();
        switch (story)
        {
            case StoryType.fisherman:
                {
                    storySystem.ChangeStory(StorySystem.Story.fisherman);
                    break;
                }
            case StoryType.ghostShip:
                {
                    storySystem.ChangeStory(StorySystem.Story.ghostShip);
                    break;
                }
            case StoryType.plaque1:
                {
                    storySystem.ChangeStory(StorySystem.Story.Plaque1);
                    break;
                }
            case StoryType.plaque2:
                {
                    storySystem.ChangeStory(StorySystem.Story.Plaque2);
                    break;
                }
            case StoryType.plaque3:
                {
                    storySystem.ChangeStory(StorySystem.Story.Plaque3);
                    break;
                }
            case StoryType.plaque4:
                {
                    storySystem.ChangeStory(StorySystem.Story.Plaque4);
                    break;
                }
            case StoryType.plaque5:
                {
                    storySystem.ChangeStory(StorySystem.Story.Plaque5);
                    break;
                }
            case StoryType.plaque6:
                {
                    storySystem.ChangeStory(StorySystem.Story.Plaque6);
                    break;
                }
            case StoryType.sign1:
                {
                    storySystem.ChangeStory(StorySystem.Story.Sign1);
                    break;
                }
            case StoryType.sign2:
                {
                    storySystem.ChangeStory(StorySystem.Story.Sign2);
                    break;
                }
            case StoryType.sign3:
                {
                    storySystem.ChangeStory(StorySystem.Story.Sign3);
                    break;
                }
            case StoryType.sign4:
                {
                    storySystem.ChangeStory(StorySystem.Story.Sign4);
                    break;
                }
            case StoryType.code1:
                {
                    storySystem.ChangeStory(StorySystem.Story.codeBook1);
                    break;
                }
            case StoryType.code2:
                {
                    storySystem.ChangeStory(StorySystem.Story.codeBook2);
                    break;
                }
        }
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
            if (useAButton && !useBButton)
            {
                if (optSystem.Input.GetButtonDown("Submit") && !PauseGame.isPaused && !active)
                {
                    characterSystem = PlayerSystem.playerTransform.GetComponent<CharacterSystem>();
                    characterSystem.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                    if (OpenStory && !StorySystem.isReading)
                    {
                        SelectStory(story);
                        storySystem.StartStory(true);

                    }
                    else if (!OpenStory)
                    {
                        if (addDialogue)
                        {
                            interaction = PlayerSystem.playerTransform.GetComponent<InteractionSystem>();
                            interaction.DialogueInteraction(true, message);
                            if (useInstant) interaction.DialogueFill();
                        }
                    }
                    active = true;
                    SetupPopupCanvas(false, null);
                }
                else if (optSystem.Input.GetButtonDown("Submit") && !PauseGame.isPaused && active && !OpenStory)
                {
                    interaction = PlayerSystem.playerTransform.GetComponent<InteractionSystem>();
                    if (addDialogue) interaction.DialogueInteraction(false, null);
                    if (useEvent) TriggerEvent(eventNum);
                    SetupPopupCanvas(false, null);
                    active = false;
                }
                else if (optSystem.Input.GetButtonUp("Submit") && OpenStory && active && !StorySystem.isReading && StorySystem.isBook)
                {
                    if (useEvent) TriggerEvent(eventNum);
                    active = false;
                    SetupPopupCanvas(true, buttonText);
                }
                else if(optSystem.Input.GetButtonDown("Submit") && OpenStory && active && StorySystem.isReading && !StorySystem.isBook)
                {
                    storySystem = PlayerSystem.playerTransform.GetComponent<StorySystem>();
                    if (useEvent) TriggerEvent(eventNum);
                    storySystem.StartStory(false);
                    active = false;
                    SetupPopupCanvas(true, buttonText);
                }
            }
            else if (useBButton && !useAButton && !OpenStory)
            {
                if (optSystem.Input.GetButtonDown("Cancel") && !PauseGame.isPaused && !active)
                {
                    if (addDialogue)
                    {
                        interaction = PlayerSystem.playerTransform.GetComponent<InteractionSystem>();
                        interaction.DialogueInteraction(true, message);
                        if (useInstant) interaction.DialogueFill();
                    }
                    SetupPopupCanvas(false, null);
                    active = true;
                }
                else if (optSystem.Input.GetButtonDown("Cancel") && !PauseGame.isPaused && active)
                {
                    interaction = PlayerSystem.playerTransform.GetComponent<InteractionSystem>();
                    if (addDialogue) interaction.DialogueInteraction(false, null);
                    if (useEvent) TriggerEvent(eventNum);
                    SetupPopupCanvas(false, null);
                    active = false;
                }
            }
            CheckStorySystem(currentStory);
        }
    }
    public void TriggerEvent(int num)
    {
        if (eventObject != null && num != 0)
        {
            EventActionSystem EAS = eventObject.GetComponent<EventActionSystem>();
            if (num == 1 && !ObjectSystem.gameEvent[0])
            {
                swordSystem = PlayerSystem.playerTransform.GetComponent<SwordSystem>();
                effectRoutine = swordSystem.QuakeEffect(8);
                StartCoroutine(effectRoutine);
            }
            EAS.TriggerEvent(num);

        }

    }
    public void SetupPopupCanvas(bool active, string message)
    {
        if (active)
        {
            if (newInteraction != null)
                Destroy(newInteraction.gameObject);
            popupCanvas = PlayerSystem.playerTransform.parent.GetChild(1).GetChild(1);
            newInteraction = Instantiate(statusPrefab, transform.position, Quaternion.identity);
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
        if (other.gameObject.CompareTag("Player"))
        {
            inRange = true;
            SetupPopupCanvas(true, buttonText);
        }
        if (other.gameObject.CompareTag("KillBlock"))
        {
            Transform parentObj = other.gameObject.transform.parent;
            Rigidbody rb = parentObj.GetComponent<Rigidbody>();
            rb.maxDepenetrationVelocity = 7;
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
    public void CheckStorySystem(int story)
    {
        storySystem = PlayerSystem.playerTransform.GetComponent<StorySystem>();
        if (!isChecked && OpenStory)
        {
            if (story == 0) if (ObjectSystem.gameEntry[0]) storySystem.SetupEntryUI(StorySystem.Story.fisherman);
            else if (story == 1) if (ObjectSystem.gameEntry[1]) storySystem.SetupEntryUI(StorySystem.Story.Plaque1);
            else if (story == 2) if (ObjectSystem.gameEntry[2]) storySystem.SetupEntryUI(StorySystem.Story.Plaque2);
            else if (story == 3) if (ObjectSystem.gameEntry[3]) storySystem.SetupEntryUI(StorySystem.Story.Plaque3); 
            else if (story == 4) if (ObjectSystem.gameEntry[4]) storySystem.SetupEntryUI(StorySystem.Story.Plaque4);
            else if (story == 5) if (ObjectSystem.gameEntry[5]) storySystem.SetupEntryUI(StorySystem.Story.Plaque5);
            else if (story == 6) if (ObjectSystem.gameEntry[6]) storySystem.SetupEntryUI(StorySystem.Story.Plaque6);
        }
    }
}
