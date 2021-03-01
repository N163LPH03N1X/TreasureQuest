using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;



public class PeopleSystem : MonoBehaviour
{
    OptSystem optSystem = new OptSystem();
    public Material sourceMat;
    public EventType selectEvent;
    public PeopleType personType;
    public FixedPositions positionType;
    public TownShop town;
    public Texture[] eyeMats;
    public Renderer[] bodyParts;
    public MeshRenderer[] eyeRends;
    GameObject popupCanvas;
    GameObject newInteraction;
    Text intText;
    bool interactionActive;
    public GameObject interactionObj;
    public GameObject peopleStatusPrefab;
    public DayConversation dayConvo;
    public NightConversation nightConvo;
    public EventFinishedConversation eventConvo;
    public GameObject peopleObj;
    Vector3 PersonPos;
    Quaternion PersonRot;
    NavMeshAgent nav;
    Vector3 playerPosition;
    public Transform[] personPos;
    Animator anim;
    Vector3 setPos;
    GameObject playerController;
    GameObject player;
    public float stateDuration = 2;
    public int curState;
    int currentPos = 0;
    public float moveSpeed;
    public float damping;
    bool inRange;
    public float resetTime;

    public bool isRandom;
    public bool isFixed;
    public bool isShop;
    public bool isStationed;
    public bool isTalking;

    

    public bool dialogueActive;

    bool messageD1Finished;
    bool messageD2Finished;
    bool messageD3Finished;
    bool messageD4Finished;
    bool messageD5Finished;
    bool messageD6Finished;
    bool messageN1Finished;
    bool messageN2Finished;
    bool messageN3Finished;
    bool messageN4Finished;
    bool messageN5Finished;
    bool messageN6Finished;
    bool shopMessageFinished;



    bool duskcliffActive;
    bool windacreActive;
    bool skulkcoveActive;
    bool shopIsOpen;
    bool isFilled;
    bool isEventFinished;


    void Start()
    {
        isFilled = true;
        anim = GetComponent<Animator>();
        nav = peopleObj.GetComponent<NavMeshAgent>();
        nav.speed = moveSpeed;
        SelectPerson(personType);
        if (isFixed)
            FixedMovement(positionType);
        else if (isStationed)
        {
            PersonRot = peopleObj.transform.rotation;
            PersonPos = peopleObj.transform.position;
        }
        if (isShop)
            SetActiveTown(town);
    }
    void Update()
    {
       
        if (interactionActive && newInteraction != null)
        {
            Vector3 statusPos = Camera.main.WorldToScreenPoint(interactionObj.transform.position);
            newInteraction.transform.position = statusPos;
            newInteraction.transform.localScale = new Vector3(1, 1, 1);
        }
        if (isRandom && !isTalking)
            RandomMovement();
        else if (isFixed && !isTalking)
        {
            StateMovement(false, MoveDirection.Fixed);
        }
        else if (isStationed && !isTalking)
        {
            peopleObj.transform.rotation = PersonRot;
            peopleObj.transform.position = PersonPos;
            SetAnimation(PeopleAnimation.Talk, false);
            SetAnimation(PeopleAnimation.Walk, false);
            SetAnimation(PeopleAnimation.Idle, true);
        }
        else if (isShop && !isTalking)
        {
            SetAnimation(PeopleAnimation.Talk, false);
            SetAnimation(PeopleAnimation.Walk, false);
            SetAnimation(PeopleAnimation.Idle, true);
        }
        else if (isTalking)
        {
            StateMovement(false, MoveDirection.Talk);
            isRandom = false;
            isFixed = false;
            isStationed = false;
        }
        if (inRange)
        {
            if (!SceneSystem.inTransition && !ShopSystem.isSleeping && optSystem.Input.GetButtonDown("Submit") && !PauseGame.isPaused && !ShopSystem.isShop && dialogueActive && !CharacterSystem.isCarrying)
            {
                playerController = GameObject.FindGameObjectWithTag("Player");
                CharacterSystem playChar = playerController.GetComponent<CharacterSystem>();
                playChar.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                SetAnimation(PeopleAnimation.Talk, true);
                SetAnimation(PeopleAnimation.Walk, false);
                SetAnimation(PeopleAnimation.Idle, false);
                SetupPopupCanvas(false, null);
                isTalking = true;
                if (!isEventFinished)
                    CheckForEventType(selectEvent);
                DialogueSystem dialogue = GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueSystem>();
                if (isFilled && !isShop)
                {
                    isFilled = false;
                    if (TimeSystem.isShophours)
                        StartConversation("Day");
                    else
                        StartConversation("Night");
                }
                else if (!isFilled && !isShop)
                {
                    if (dialogue.dialogueText.text != dialogue.messages.popMessage)
                    {
                        InteractionSystem interaction = GameObject.FindGameObjectWithTag("Player").GetComponent<InteractionSystem>();
                        interaction.DialogueFill();
                    }
                    else if(dialogue.dialogueText.text == dialogue.messages.popMessage)
                    {
                        if (TimeSystem.isShophours)
                            StartConversation("Day");
                        else
                            StartConversation("Night");
                    }
                    isFilled = true;
                }
                else if (isShop)
                {
                    if (TimeSystem.isShophours)
                        StartConversation("Day");
                    else
                        StartConversation("Night");
                }
              
            }
            else if (!dialogueActive)
            {
                playerController = GameObject.FindGameObjectWithTag("Player");
                InteractionSystem interaction = playerController.GetComponent<InteractionSystem>();
                SetupPopupCanvas(false, null);
                interaction.DialogueInteraction(false, null);
                ResetPerson();
            }
        }
    }
    public enum EventType { EventNone, Event1, Event2, Event3, Event4, Event5}
    public void CheckForEventType(EventType type)
    {
        switch (type)
        {
            case EventType.Event1:
                {
                    if (ObjectSystem.gameEvent[0])
                        ChangeConversation();
                    break;
                }
            case EventType.Event2:
                {
                    if (ObjectSystem.gameEvent[1])
                        ChangeConversation();
                    break;
                }
            case EventType.Event3:
                {
                    if (ObjectSystem.gameEvent[2])
                        ChangeConversation();
                    break;
                }
            case EventType.Event4:
                {
                    if (ObjectSystem.gameEvent[3])
                        ChangeConversation();
                    break;
                }
            case EventType.Event5:
                {
                    if (ObjectSystem.gameEvent[4])
                        ChangeConversation();
                    break;
                }
        }
    }
    public enum PeopleType { ShopKeep, Random, Fixed, Stationary }
    public enum FixedPositions { Two, Four }
    public enum TownShop { DuskCliff, Windacre, SkulkCove }
    public void SetupPopupCanvas(bool active, string message)
    {
        if (active)
        {
            if (newInteraction != null)
                Destroy(newInteraction.gameObject);
            popupCanvas = GameObject.Find("Core/Player/PopUpCanvas/PlayerUIStorage");
            newInteraction = Instantiate(peopleStatusPrefab, transform.position, Quaternion.identity);
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
    public void ChangeConversation()
    {
        dayConvo.message1 = eventConvo.message1;
        dayConvo.message2 = eventConvo.message2;
        dayConvo.message3 = eventConvo.message3;
        dayConvo.message4 = eventConvo.message4;
        dayConvo.message5 = eventConvo.message5;
        dayConvo.message6 = eventConvo.message6;
        nightConvo.message1 = eventConvo.message1;
        nightConvo.message2 = eventConvo.message2;
        nightConvo.message3 = eventConvo.message3;
        nightConvo.message4 = eventConvo.message4;
        nightConvo.message5 = eventConvo.message5;
        nightConvo.message6 = eventConvo.message6;
        isEventFinished = true;
    }
    public void StartConversation(string time)
    {
        playerController = GameObject.FindGameObjectWithTag("Player");
        InteractionSystem interaction = playerController.GetComponent<InteractionSystem>();
        if (time == "Day")
        {
            if (!isShop)
            {
                if (!messageD1Finished && dayConvo.message1 != "")
                {
                    interaction.DialogueInteraction(true, dayConvo.message1);
                    messageD1Finished = true;
                }
                else if (!messageD2Finished && dayConvo.message2 != "")
                {
                    interaction.DialogueInteraction(true, dayConvo.message2);
                    messageD2Finished = true;
                }
                else if (!messageD3Finished && dayConvo.message3 != "")
                {
                    interaction.DialogueInteraction(true, dayConvo.message3);
                    messageD3Finished = true;
                }
                else if (!messageD4Finished && dayConvo.message4 != "")
                {
                    interaction.DialogueInteraction(true, dayConvo.message4);
                    messageD4Finished = true;
                }
                else if (!messageD5Finished && dayConvo.message5 != "")
                {
                    interaction.DialogueInteraction(true, dayConvo.message5);
                    messageD5Finished = true;
                }
                else if (!messageD6Finished && dayConvo.message6 != "")
                {
                    interaction.DialogueInteraction(true, dayConvo.message6);
                    messageD6Finished = true;
                }
                else
                {
                    SetupPopupCanvas(false, null);
                    interaction.DialogueInteraction(false, null);
                    dialogueActive = false;
                    ResetPerson();
                }
            }
            else if (isShop && !ShopSystem.isShop && TimeSystem.isShophours)
            {
                if (duskcliffActive)
                {
                    if (ShopSystem.vitalityBought[0])
                    {
                        if (!shopMessageFinished && dayConvo.shopDMessageVitality != "")
                        {
                            interaction.DialogueInteraction(true, dayConvo.shopDMessageVitality);
                            shopMessageFinished = true;
                        }
                        else if (shopMessageFinished && !shopIsOpen)
                            OpenShop(town);
                        else
                        {
                            dialogueActive = false;
                            ResetPerson();
                        }
                    }
                    else if (!ShopSystem.vitalityBought[0])
                    {
                        if (!shopMessageFinished && dayConvo.shopDMessage != "")
                        {
                            interaction.DialogueInteraction(true, dayConvo.shopDMessage);
                            shopMessageFinished = true;
                        }
                        else if (shopMessageFinished && !shopIsOpen)
                            OpenShop(town);
                        else
                        {
                            dialogueActive = false;
                            ResetPerson();
                        }
                    }
                }
                else if (windacreActive)
                {
                    if (ShopSystem.vitalityBought[1])
                    {
                        if (!shopMessageFinished && dayConvo.shopWMessageVitality != "")
                        {
                            interaction.DialogueInteraction(true, dayConvo.shopWMessageVitality);
                            shopMessageFinished = true;
                        }
                        else if (shopMessageFinished && !shopIsOpen)
                            OpenShop(town);
                        else
                        {
                            dialogueActive = false;
                            ResetPerson();
                        }
                    }
                    else if (!ShopSystem.vitalityBought[1])
                    {
                        if (!shopMessageFinished && dayConvo.shopWMessage != "")
                        {
                            interaction.DialogueInteraction(true, dayConvo.shopWMessage);
                            shopMessageFinished = true;
                        }
                        else if (shopMessageFinished && !shopIsOpen)
                            OpenShop(town);
                        else
                        {
                            GameObject player = GameObject.FindGameObjectWithTag("Player");
                            InteractionSystem intSys = player.GetComponent<InteractionSystem>();
                            SetupPopupCanvas(false, null);
                            interaction.DialogueInteraction(false, null);

                            dialogueActive = false;
                            ResetPerson();
                        }
                    }
                }
            }
           
            
        }
        else if(time == "Night")
        {
            if (!isShop)
            {
                if (!messageN1Finished && nightConvo.message1 != "")
                {
                    interaction.DialogueInteraction(true, nightConvo.message1);
                    messageN1Finished = true;
                }
                else if (!messageN2Finished && nightConvo.message2 != "")
                {
                    interaction.DialogueInteraction(true, nightConvo.message2);
                    messageN2Finished = true;
                }
                else if (!messageN3Finished && nightConvo.message3 != "")
                {
                    interaction.DialogueInteraction(true, nightConvo.message3);
                    messageN3Finished = true;
                }
                else if (!messageN4Finished && nightConvo.message4 != "")
                {
                    interaction.DialogueInteraction(true, nightConvo.message4);
                    messageN4Finished = true;
                }
                else if (!messageN5Finished && nightConvo.message5 != "")
                {
                    interaction.DialogueInteraction(true, nightConvo.message5);
                    messageN5Finished = true;
                }
                else if (!messageN6Finished && nightConvo.message6 != "")
                {
                    interaction.DialogueInteraction(true, nightConvo.message6);
                    messageN6Finished = true;
                }
                else
                {
                    SetupPopupCanvas(false, null);
                    interaction.DialogueInteraction(false, null);
                    dialogueActive = false;
                    ResetPerson();
                }
            }
        }
    }
    public void SetActiveTown(TownShop town)
    {
        switch (town)
        {
            case TownShop.DuskCliff:
                {
                    skulkcoveActive = false;
                    windacreActive = false;
                    duskcliffActive = true;
                    break;
                }
            case TownShop.Windacre:
                {
                    skulkcoveActive = false;
                    windacreActive = true;
                    duskcliffActive = false;
                    break;
                }
            case TownShop.SkulkCove:
                {
                    skulkcoveActive = true;
                    windacreActive = false;
                    duskcliffActive = false;
                    break;
                }
        }
    }
    public void CloseTheShop()
    {
        shopIsOpen = false;
    }
    public void OpenShop(TownShop town)
    {
        playerController = GameObject.FindGameObjectWithTag("Player");
        ShopSystem shopSys = playerController.GetComponent<ShopSystem>();

        switch (town)
        {
            case TownShop.DuskCliff:
                {
                    shopSys.SelectShop(Shop.DuskCliff, 4);
                    shopSys.BrowseTheShop();
                    break;
                }
            case TownShop.Windacre:
                {
                    shopSys.SelectShop(Shop.WindAcre, 4);
                    shopSys.BrowseTheShop();
                    break;
                }
            case TownShop.SkulkCove:
                {
                    shopSys.SelectShop(Shop.SkulkCove, 6);
                    shopSys.BrowseTheShop();
                    break;
                }
        }
        shopIsOpen = true;
    }
    public void UpdateTransforms()
    {
        Vector3 updatePos = peopleObj.transform.position;
        personPos[0].position = new Vector3(personPos[0].position.x, updatePos.y, personPos[0].position.z);
        personPos[1].position = new Vector3(personPos[1].position.x, updatePos.y, personPos[1].position.z);
        personPos[2].position = new Vector3(personPos[2].position.x, updatePos.y, personPos[2].position.z);
        personPos[3].position = new Vector3(personPos[3].position.x, updatePos.y, personPos[3].position.z);
    }
    public void SelectPerson(PeopleType type)
    {
        switch (type)
        {
            case PeopleType.Random:
                {
                    isStationed = false;
                    isFixed = false;
                    isShop = false;
                    isRandom = true;
                    break;
                }
            case PeopleType.ShopKeep:
                {
                    isStationed = false;
                    isFixed = false;
                    isShop = true;
                    isRandom = false;
                    break;
                }
            case PeopleType.Fixed:
                {
                    isStationed = false;
                    isFixed = true;
                    isShop = false;
                    isRandom = false;
                    break;
                }
            case PeopleType.Stationary:
                {
                   
                    isStationed = true;
                    isFixed = false;
                    isShop = false;
                    isRandom = false;
                    break;
                }
        }
    }
    public void RandomMovement()
    {
        if (stateDuration > 0)
        {
            stateDuration -= Time.deltaTime;
            if (curState == 1)
                StateMovement(true, MoveDirection.None);
            else if (curState == 2)
                StateMovement(true, MoveDirection.None);
            else if (curState == 3)
                StateMovement(true, MoveDirection.None);
            else if (curState == 4)
                StateMovement(true, MoveDirection.None);
            else if (curState == 5)
                StateMovement(false, MoveDirection.Up);
            else if (curState == 6)
                StateMovement(false, MoveDirection.Down);
            else if (curState == 7)
                StateMovement(false, MoveDirection.Left);
            else if (curState == 8)
                StateMovement(false, MoveDirection.Right);
        }
        else if (stateDuration < 0)
        {
            curState = UnityEngine.Random.Range(1, 9);
            stateDuration = UnityEngine.Random.Range(1, 5);
            Invoke("RandomMovement", 0.1f);
        }
    }
    public enum MoveDirection { Up, Down, Left, Right, None, Talk, Fixed}
    public void StateMovement(bool active, MoveDirection direction)
    {
        if (active)
        {
            nav.isStopped = true;
            nav.speed = 0.0f;
            SetAnimation(PeopleAnimation.Idle, true);
            SetAnimation(PeopleAnimation.Walk, false);
            SetAnimation(PeopleAnimation.Talk, false);
        }
        else
        {
            switch (direction)
            {
                case MoveDirection.Up:
                    {
                        nav.speed = moveSpeed;
                        nav.isStopped = false;
                        Vector3 dir;
                        SetAnimation(PeopleAnimation.Idle, false);
                        SetAnimation(PeopleAnimation.Walk, true);
                        SetAnimation(PeopleAnimation.Talk, false);
                        nav.updateRotation = true;
                        dir = Vector3.forward;
                        nav.Move(dir * (moveSpeed * Time.deltaTime));
                        Vector3 _direction = dir;
                        Quaternion _lookRotation = Quaternion.LookRotation(_direction);
                        peopleObj.transform.rotation = new Quaternion(0, _lookRotation.y, 0, _lookRotation.w);
                        break;
                    }
                case MoveDirection.Down:
                    {
                        nav.speed = moveSpeed;
                        nav.isStopped = false;
                        Vector3 dir;
                        SetAnimation(PeopleAnimation.Idle, false);
                        SetAnimation(PeopleAnimation.Walk, true);
                        SetAnimation(PeopleAnimation.Talk, false);
                        nav.updateRotation = true;
                        dir = Vector3.back;
                        nav.Move(dir * (moveSpeed * Time.deltaTime));
                        Vector3 _direction = dir;
                        Quaternion _lookRotation = Quaternion.LookRotation(_direction);
                        peopleObj.transform.rotation = new Quaternion(0, _lookRotation.y, 0, _lookRotation.w);
                        break;
                    }
                case MoveDirection.Left:
                    {
                        nav.speed = moveSpeed;
                        nav.isStopped = false;
                        Vector3 dir;
                        SetAnimation(PeopleAnimation.Idle, false);
                        SetAnimation(PeopleAnimation.Walk, true);
                        SetAnimation(PeopleAnimation.Talk, false);
                        nav.updateRotation = true;
                        dir = Vector3.left;
                        nav.Move(dir * (moveSpeed * Time.deltaTime));
                        Vector3 _direction = dir;
                        Quaternion _lookRotation = Quaternion.LookRotation(_direction);
                        peopleObj.transform.rotation = new Quaternion(0, _lookRotation.y, 0, _lookRotation.w);
                        break;
                    }
                case MoveDirection.Right:
                    {
                        nav.speed = moveSpeed;
                        nav.isStopped = false;
                        Vector3 dir;
                        SetAnimation(PeopleAnimation.Idle, false);
                        SetAnimation(PeopleAnimation.Walk, true);
                        SetAnimation(PeopleAnimation.Talk, false);
                        nav.updateRotation = true;
                        dir = Vector3.right;
                        nav.Move(dir * (moveSpeed * Time.deltaTime));
                        Vector3 _direction = dir;
                        Quaternion _lookRotation = Quaternion.LookRotation(_direction);
                        peopleObj.transform.rotation = new Quaternion(0, _lookRotation.y, 0, _lookRotation.w);
                        break;
                    }
                case MoveDirection.None:
                    {
                        break;
                    }
                case MoveDirection.Talk:
                    {
                        nav.speed = 0;
                        nav.velocity = Vector3.zero;
                        nav.isStopped = true;
                        SetAnimation(PeopleAnimation.Idle, false);
                        SetAnimation(PeopleAnimation.Walk, false);
                        SetAnimation(PeopleAnimation.Talk, true);
                        nav.updateRotation = true;
                        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
                        var lookPos = playerPosition - peopleObj.transform.position;
                        var rotation = Quaternion.LookRotation(lookPos);
                        peopleObj.transform.rotation = new Quaternion (0, rotation.y, 0, rotation.w);

                        break;
                    }
                case MoveDirection.Fixed:
                    {
                        nav.speed = moveSpeed;
                        nav.isStopped = false;
                        nav.SetDestination(setPos);
                        SetAnimation(PeopleAnimation.Idle, false);
                        SetAnimation(PeopleAnimation.Walk, true);
                        SetAnimation(PeopleAnimation.Talk, false);
                        UpdateTransforms();
                        nav.updateRotation = true;
                        break;
                    }
            }
         
        }
    }
    IEnumerator DelayedAction(UnityAction action, float delay)
    { 
            yield return new WaitForSeconds(delay);
            action.Invoke();
    }
    public void FixedMovement(FixedPositions pos)
    {
        switch (pos)
        {
            case FixedPositions.Two:
                {
                    if (peopleObj.transform.position == personPos[0].position)
                        currentPos = 1;
                    else if (peopleObj.transform.position == personPos[1].position)
                        currentPos = 0;
                    setPos = personPos[currentPos].position;
                    StartCoroutine(DelayedAction(delegate { FixedMovement(pos); }, resetTime));
                    break;
                }

            case FixedPositions.Four:
                {
                    if (peopleObj.transform.position == personPos[0].position)
                        currentPos = 1;
                    else if (peopleObj.transform.position == personPos[1].position)
                        currentPos = 2;
                    else if (peopleObj.transform.position == personPos[2].position)
                        currentPos = 3;
                    else if (peopleObj.transform.position == personPos[3].position)
                        currentPos = 0;
                    setPos = personPos[currentPos].position;

                    StartCoroutine(DelayedAction(delegate { FixedMovement(pos); }, resetTime));
                    break;
                }
        }
    }
    public enum PeopleAnimation { Walk, Run, Idle, Talk, Shop}
    public void SetAnimation(PeopleAnimation anim, bool active)
    {
        if (active)
        {
            switch (anim)
            {
                case PeopleAnimation.Walk:
                    {
                        this.anim.SetBool("Walk", true);
                        this.anim.SetFloat("MoveSpeed", 1f);
                        break;
                    }

                case PeopleAnimation.Run:
                    {
                        this.anim.SetFloat("MoveSpeed", 2f);
                        this.anim.SetBool("Walk", true);
                        break;
                    }
                case PeopleAnimation.Idle:
                    {
                        this.anim.SetBool("Idle", true);
                        break;
                    }
                case PeopleAnimation.Talk:
                    {
                        this.anim.SetBool("Talk", true);

                        break;
                    }
                case PeopleAnimation.Shop:
                    {
                        this.anim.SetBool("Shop", true);
                        break;
                    }
            }
        }
        else
        {
            switch (anim)
            {
                case PeopleAnimation.Walk:
                    {
                        this.anim.SetBool("Walk", false);
                        break;
                    }

                case PeopleAnimation.Run:
                    {
                        this.anim.SetFloat("MoveSpeed", 0f);
                        this.anim.SetBool("Walk", false);
                        break;
                    }
                case PeopleAnimation.Idle:
                    {
                        this.anim.SetBool("Idle", false);
                        break;
                    }
                case PeopleAnimation.Talk:
                    {
                        this.anim.SetBool("Talk", false);
                        break;
                    }
                case PeopleAnimation.Shop:
                    {
                        this.anim.SetBool("Shop", false);
                        break;
                    }
            }
        }
    }
   
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isTalking && !isShop)
        {
            CharacterSystem charSys = other.gameObject.GetComponent<CharacterSystem>();
            charSys.PlayerInteraction(true);
            InteractionSystem interaction = other.gameObject.GetComponent<InteractionSystem>();
            SetupPopupCanvas(true, "Talk");
            inRange = true;
        }
        if (other.gameObject.CompareTag("Player") && !isTalking && TimeSystem.isShophours && isShop)
        {
            CharacterSystem charSys = other.gameObject.GetComponent<CharacterSystem>();
            charSys.PlayerInteraction(true);
            InteractionSystem interaction = other.gameObject.GetComponent<InteractionSystem>();
            SetupPopupCanvas(true, "Buy");
            inRange = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InteractionSystem interaction = other.gameObject.GetComponent<InteractionSystem>();
            CharacterSystem charSys = other.gameObject.GetComponent<CharacterSystem>();
            charSys.PlayerInteraction(false);
            SetupPopupCanvas(false, null);
            interaction.DialogueInteraction(false, null);
            inRange = false;
            dialogueActive = false;
            ResetPerson();
        }
    }
    public void ResetPerson()
    {
        isFilled = true;
        dialogueActive = true;
        isTalking = false;
        messageD1Finished = false;
        messageD2Finished = false;
        messageD3Finished = false;
        messageD4Finished = false;
        messageD5Finished = false;
        messageD6Finished = false;
        messageN1Finished = false;
        messageN2Finished = false;
        messageN3Finished = false;
        messageN4Finished = false;
        messageN5Finished = false;
        messageN6Finished = false;
        shopMessageFinished = false;
        SelectPerson(personType);
        if(isFixed)
            FixedMovement(positionType);
        if (isShop)
        {
            PersonRot = peopleObj.transform.rotation;
            peopleObj.transform.rotation = PersonRot;
        }

    }
}
[Serializable]
public struct DayConversation
{
    public string message1;
    public string message2;
    public string message3;
    public string message4;
    public string message5;
    public string message6;

    public string shopDMessage;
    public string shopDMessageVitality;

    public string shopWMessage;
    public string shopWMessageVitality;
}
[Serializable]
public struct NightConversation
{
    public string message1;
    public string message2;
    public string message3;
    public string message4;
    public string message5;
    public string message6;

}
[Serializable]
public struct EventFinishedConversation
{
    public string message1;
    public string message2;
    public string message3;
    public string message4;
    public string message5;
    public string message6;

}