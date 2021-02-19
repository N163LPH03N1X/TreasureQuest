using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DoorSystem : MonoBehaviour
{
    OptSystem optSystem = new OptSystem();
    public int currentDoor;
    AudioSource audioSrc;
    Animator anim;
    bool needsToOpen;
    bool needsToClose;
    public bool isLocked;
    bool isDoorMoving;
    bool inTerritory;
    GameObject playerController;
    GameObject player;
    public AudioClip opendoorSfx;
    public AudioClip unLockSfx;
    AudioSource audioSrc2;
    public float maxHeight;
    public float minHeight;
    public float openSpeed;
    GameObject doorObj;
    float closeTime = 0;
    public float closeTimer;
    bool dialogueActive;
    bool isChecked;
    bool KeyUsed; GameObject popupCanvas;
    GameObject newInteraction;
    Text intText;
    bool interactionActive;
    public GameObject interactionObj;
    public GameObject doorStatusPrefab;
    public GameObject ps;
    public bool isMoonDoor;
    public bool isOpenReverse;
    public bool isDebris;
    bool psActive;
    bool startDoor = true;
    bool playerInDoor = false;
    bool isOpen;

    private void Awake()
    {
        CheckDoorSystem(currentDoor);
    }
    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        doorObj = transform.GetChild(0).gameObject;
        audioSrc2 = doorObj.GetComponent<AudioSource>();
        if (isDebris)
            ps.SetActive(false);
    }
    void Update()
    {
        if (interactionActive && newInteraction != null)
        {
            Vector3 statusPos = Camera.main.WorldToScreenPoint(interactionObj.transform.position);
            newInteraction.transform.position = statusPos;
            newInteraction.transform.localScale = new Vector3(1, 1, 1);
        }
        if (isLocked)
            CheckDoorSystem(currentDoor);

        if (closeTime > 0)
        {
            closeTime -= Time.deltaTime;
        }
        else if (closeTime < 0)
        {
            needsToOpen = false;
            closeTime = 0;
        }

        OpenDoor();
        if (inTerritory && !isDoorMoving)
        {
            playerController = GameObject.FindGameObjectWithTag("Player");
            InteractionSystem interaction = playerController.GetComponent<InteractionSystem>();
            if (optSystem.Input.GetButtonDown("Submit") && !PauseGame.isPaused && !ShopSystem.isShop && !dialogueActive && !CharacterSystem.isCarrying)
            {
                if (isLocked)
                {
                    if(!isMoonDoor)
                        interaction.DialogueInteraction(true, "Need a Key.");
                    else
                        interaction.DialogueInteraction(true, "The Path is Sealed.");
                    SetupPopupCanvas(false, null);
                    dialogueActive = true;
                }

                else if (!isLocked)
                {
                  
                    interaction.DialogueInteraction(false, null);
                    SetupPopupCanvas(false, null);
                    needsToOpen = true;
                }
            }
            else if (optSystem.Input.GetButtonDown("Submit") && dialogueActive && !CharacterSystem.isCarrying)
            {

                interaction.DialogueInteraction(false, null);
                SetupPopupCanvas(false, null);
                dialogueActive = false;
            }
        }
    }
    public void SetupPopupCanvas(bool active, string message)
    {
        if (active)
        {
            if (newInteraction != null)
                Destroy(newInteraction.gameObject);
            popupCanvas = GameObject.Find("Core/Player/PopUpCanvas/PlayerUIStorage");
            newInteraction = Instantiate(doorStatusPrefab, transform.position, Quaternion.identity);
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
    public void OpenDoor()
    {
        if (needsToOpen)
        {
            if (needsToClose)
            {
                isDoorMoving = true;
                if (isOpenReverse)
                {
                    if (isDebris)
                    {
                        if (!psActive)
                        {
                            audioSrc.clip = opendoorSfx;
                            audioSrc.Play();
                            ps.SetActive(true);
                            psActive = true;
                        }

                    }
                    doorObj.transform.Translate(Vector3.down * Time.deltaTime * openSpeed);
                    if (doorObj.transform.localPosition.y < minHeight)
                    {
                        Vector3 newPosition = doorObj.transform.localPosition;
                        newPosition.y = minHeight;
                        doorObj.transform.localPosition = newPosition;
                        if (newPosition.y == minHeight)
                        {
                            if (isDebris)
                            {
                                if (psActive)
                                {
                                    audioSrc.Stop();
                                    ps.SetActive(false);
                                    psActive = false;
                                }
                            }
                            needsToClose = false;
                            audioSrc.Stop();
                            closeTime = closeTimer;
                            isOpen = true;
                        }
                    }
                }
                else
                {
                    if (isDebris)
                    {
                        if (!psActive)
                        {
                            audioSrc.clip = opendoorSfx;
                            audioSrc.Play();
                            ps.SetActive(true);
                            psActive = true;
                        }
                    }
                    doorObj.transform.Translate(Vector3.up * Time.deltaTime * openSpeed);
                    if (doorObj.transform.localPosition.y > maxHeight)
                    {
                        Vector3 newPosition = doorObj.transform.localPosition;
                        newPosition.y = maxHeight;
                        doorObj.transform.localPosition = newPosition;
                        if (newPosition.y == maxHeight)
                        {
                            isOpen = false;
                            if (isDebris)
                            {
                                if (psActive)
                                {
                                    audioSrc.Stop();
                                    ps.SetActive(false);
                                    psActive = false;
                                }
                            }
                            needsToClose = false;
                            audioSrc.Stop();
                            closeTime = closeTimer;

                        }
                    }
                }
            }

        }
        else if (!needsToOpen)
        {
            if (!needsToClose)
            {
                if (isOpenReverse)
                {
                    if (isDebris && !playerInDoor)
                    {
                        if (!psActive)
                        {
                            audioSrc.clip = opendoorSfx;
                            audioSrc.Play();
                            ps.SetActive(true);
                            psActive = true;
                        }
                    }
                    doorObj.transform.Translate(Vector3.up * Time.deltaTime * openSpeed);
                    if (doorObj.transform.localPosition.y > maxHeight)
                    { 
                        Vector3 newPosition = doorObj.transform.localPosition;
                        newPosition.y = maxHeight;
                        doorObj.transform.localPosition = newPosition;
                        if (newPosition.y == maxHeight)
                        {
                            isOpen = false;
                            if (isDebris)
                            {
                                if (psActive)
                                {
                                    audioSrc.Stop();
                                    ps.SetActive(false);
                                    psActive = false;
                                }
                            }
                            audioSrc.Stop();
                            needsToClose = true;
                            isDoorMoving = false;
                        }

                    }
                }
                else
                {
                    if (isDebris)
                    {
                        if (!psActive)
                        {
                            audioSrc.clip = opendoorSfx;
                            audioSrc.Play();
                            ps.SetActive(true);
                            psActive = true;
                        }
                    }
                    doorObj.transform.Translate(Vector3.down * Time.deltaTime * openSpeed);
                    if (doorObj.transform.localPosition.y < minHeight)
                    {
                        Vector3 newPosition = doorObj.transform.localPosition;
                        newPosition.y = minHeight;
                        doorObj.transform.localPosition = newPosition;
                        if (newPosition.y == minHeight)
                        {
                            isOpen = true;
                            if (isDebris)
                            {
                                if (psActive)
                                {
                                    audioSrc.Stop();
                                    ps.SetActive(false);
                                    psActive = false;
                                }
                            }
                            needsToClose = true;
                            isDoorMoving = false;
                        }

                    }
                }
            }

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !CharacterSystem.isCarrying && !isLocked)
        {
            playerInDoor = true;
            if (!needsToClose)
            {
                needsToClose = true;
                needsToOpen = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isDoorMoving && !CharacterSystem.isCarrying)
        {
          
            InteractionSystem interaction = other.gameObject.GetComponent<InteractionSystem>();
            SetupPopupCanvas(true, "Open");
            inTerritory = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !CharacterSystem.isCarrying)
        {
            InteractionSystem interaction = other.gameObject.GetComponent<InteractionSystem>();
            interaction.DialogueInteraction(false, null);
            SetupPopupCanvas(false, null);
            inTerritory = false;
        }
    }
    public void DoorStatus(bool locked)
    {
        isChecked = true;
        isLocked = locked;
    }
    public void UnlockDoorAuto()
    {
        ObjectSystem objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
        objectSystem.SetDoorActive(currentDoor);
        needsToOpen = true;
    }
    public void UnlockDoor()
    {
        ObjectSystem objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
        objectSystem.SetDoorActive(currentDoor);
    }
    public void CheckDoorSystem(int door)
    {
        if (!isChecked)
        {
            if (door == 1)
            {
                if (ObjectSystem.door1)
                {
                    isChecked = true;
                    DoorStatus(false);
                    UnlockDoor();
                }
            }
            else if (door == 2)
            {
                if (ObjectSystem.door2)
                {
                    isChecked = true;
                    DoorStatus(false);
                    UnlockDoor();
                }
            }
            else if (door == 3)
            {
                if (ObjectSystem.door3)
                {
                    isChecked = true;
                    DoorStatus(false);
                    UnlockDoor();
                }
            }
            else if (door == 4)
            {
                if (ObjectSystem.door4)
                {
                    isChecked = true;
                    DoorStatus(false);
                    UnlockDoor();
                }
            }
            else if (door == 5)
            {
                if (ObjectSystem.door5)
                {
                    isChecked = true;
                    DoorStatus(false);
                    UnlockDoor();
                }
            }
            else if (door == 6)
            {
                if (ObjectSystem.door6)
                {
                    isChecked = true;
                    DoorStatus(false);
                    UnlockDoor();
                }
            }
            else if (door == 7)
            {
                if (ObjectSystem.door7)
                {
                    isChecked = true;
                    DoorStatus(false);
                    UnlockDoor();
                }
            }
            else if (door == 8)
            {
                if (ObjectSystem.door8)
                {
                    isChecked = true;
                    DoorStatus(false);
                    UnlockDoor();
                }
            }
            else if (door == 9)
            {
                if (ObjectSystem.door9)
                {
                    isChecked = true;
                    DoorStatus(false);
                    UnlockDoor();
                }
            }
            else if (door == 10)
            {
                if (ObjectSystem.door10)
                {
                    isChecked = true;
                    DoorStatus(false);
                    UnlockDoor();
                }
            }
            else if (door == 11)
            {
                if (ObjectSystem.door11)
                {
                    isChecked = true;
                    DoorStatus(false);
                    UnlockDoor();
                }
            }
            else if (door == 12)
            {
                if (ObjectSystem.door12)
                {
                    isChecked = true;
                    DoorStatus(false);
                    UnlockDoor();
                }
            }
            else if (door == 13)
            {
                if (ObjectSystem.door13)
                {
                    isChecked = true;
                    DoorStatus(false);
                    UnlockDoor();
                }
            }
            else if (door == 14)
            {
                if (ObjectSystem.door14)
                {
                    isChecked = true;
                    DoorStatus(false);
                    UnlockDoor();
                }
            }
            else if (door == 15)
            {
                if (ObjectSystem.door15)
                {
                    isChecked = true;
                    DoorStatus(false);
                    UnlockDoor();
                }
            }
        }
    }
}
