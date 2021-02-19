using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MiscObjInt : MonoBehaviour
{
    OptSystem optSystem = new OptSystem();
    public string message;
    public StoryType story;
    public int currentStory;
    public string buttonText;
    GameObject popupCanvas;
    GameObject newInteraction;
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
                    CharacterSystem characterSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterSystem>();
                    characterSystem.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                    if (OpenStory && !StorySystem.isReading)
                    {
                        GameObject playerController = GameObject.FindGameObjectWithTag("Player");
                        SelectStory(story);
                        StorySystem storySystem = playerController.GetComponent<StorySystem>();
                        storySystem.StartStory(true);

                    }
                    else if (!OpenStory)
                    {
                        if (addDialogue)
                        {
                            GameObject playerController = GameObject.FindGameObjectWithTag("Player");
                            InteractionSystem interaction = playerController.GetComponent<InteractionSystem>();
                            interaction.DialogueInteraction(true, message);
                            if (useInstant)
                                interaction.DialogueFill();
                        }

                    }
                    active = true;
                    SetupPopupCanvas(false, null);
                }
                else if (optSystem.Input.GetButtonDown("Submit") && !PauseGame.isPaused && active && !OpenStory)
                {
                   
                    if (addDialogue)
                    {
                        GameObject playerController = GameObject.FindGameObjectWithTag("Player");
                        InteractionSystem interaction = playerController.GetComponent<InteractionSystem>();
                        interaction.DialogueInteraction(false, null);
                    }
                    if (useEvent)
                        TriggerEvent(eventNum);
                    SetupPopupCanvas(false, null);
                    active = false;
                }
                else if (optSystem.Input.GetButtonUp("Submit") && OpenStory && active && !StorySystem.isReading && StorySystem.isBook)
                {
                    if (useEvent)
                        TriggerEvent(eventNum);
                    active = false;
                    SetupPopupCanvas(true, buttonText);
                }
                else if(optSystem.Input.GetButtonDown("Submit") && OpenStory && active && StorySystem.isReading && !StorySystem.isBook)
                {
                    if (useEvent)
                        TriggerEvent(eventNum);
                    GameObject playerController = GameObject.FindGameObjectWithTag("Player");
                    StorySystem storySystem = playerController.GetComponent<StorySystem>();
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
                        GameObject playerController = GameObject.FindGameObjectWithTag("Player");
                        InteractionSystem interaction = playerController.GetComponent<InteractionSystem>();
                        interaction.DialogueInteraction(true, message);
                        if (useInstant)
                            interaction.DialogueFill();
                    }
                    SetupPopupCanvas(false, null);
                    active = true;
                }
                else if (optSystem.Input.GetButtonDown("Cancel") && !PauseGame.isPaused && active)
                {
                    if (addDialogue)
                    {
                        GameObject playerController = GameObject.FindGameObjectWithTag("Player");
                        InteractionSystem interaction = playerController.GetComponent<InteractionSystem>();
                        interaction.DialogueInteraction(false, null);
                    }
                    if (useEvent)
                        TriggerEvent(eventNum);
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
            if (num == 1 && !ObjectSystem.event1)
            {
                SwordSystem swordSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<SwordSystem>();
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
            popupCanvas = GameObject.Find("Core/Player/PopUpCanvas/PlayerUIStorage");
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
        StorySystem storySystem = GameObject.FindGameObjectWithTag("Player").GetComponent<StorySystem>();

        if (!isChecked && OpenStory)
        {
            if (story == 1)
            {
                if (ObjectSystem.entry1)
                {
                    storySystem.SetupEntryUI(StorySystem.Story.fisherman);
                    isChecked = true;
                }
            }
            else if (story == 2)
            {
                if (ObjectSystem.entry2)
                {
                    storySystem.SetupEntryUI(StorySystem.Story.Plaque1);
                    isChecked = true;
                }
            }
            else if (story == 3)
            {
                if (ObjectSystem.entry3)
                {
                    storySystem.SetupEntryUI(StorySystem.Story.Plaque2);
                    isChecked = true;
                }
            }
            else if (story == 4)
            {
                if (ObjectSystem.entry4)
                {
                    storySystem.SetupEntryUI(StorySystem.Story.Plaque3);
                    isChecked = true;
                }
            }
            else if (story == 5)
            {
                if (ObjectSystem.entry5)
                {
                    storySystem.SetupEntryUI(StorySystem.Story.Plaque4);
                    isChecked = true;
                }
            }
            else if (story == 6)
            {
                if (ObjectSystem.entry6)
                {
                    storySystem.SetupEntryUI(StorySystem.Story.Plaque5);
                    isChecked = true;
                }
            }
            else if (story == 7)
            {
                if (ObjectSystem.entry7)
                {
                    storySystem.SetupEntryUI(StorySystem.Story.Plaque6);
                    isChecked = true;
                }
            }
            //else if (story == 8)
            //{
            //    if (ObjectSystem.entry8)
            //    {
            //        storySystem.SetupEntryUI(StorySystem.Story.fisherman);
            //        isChecked = true;
            //    }
            //}
            //else if (story == 9)
            //{
            //    if (ObjectSystem.entry9)
            //    {
            //        storySystem.SetupEntryUI(StorySystem.Story.fisherman);
            //        isChecked = true;
            //    }
            //}
            //else if (story == 10)
            //{
            //    if (ObjectSystem.entry10)
            //    {
            //        storySystem.SetupEntryUI(StorySystem.Story.fisherman);
            //        isChecked = true;
            //    }
            //}
            //else if (story == 11)
            //{
            //    if (ObjectSystem.entry11)
            //    {
            //        storySystem.SetupEntryUI(StorySystem.Story.fisherman);
            //        isChecked = true;
            //    }
            //}

        }
    }
}
