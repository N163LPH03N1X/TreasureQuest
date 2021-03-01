using System.Collections;
using UnityEngine;

public enum Events { Water, Door, Totem}

public class ObjectEventSystem : MonoBehaviour
{
    public bool isTemple1BossDoor;
    public bool isTemple1MainDoor;
    public bool isTemple1Stairs;
    public bool isTemple1Totem;

    AudioSource audioSrc;
    public AudioClip doorSlamFx;
    public AudioClip rumbleFx;
    public bool debug;
    public Events events;
    public bool raise;
    public bool lower;
    public ParticleSystem ps;
    public float maxHeight;
    public float minHeight;

    public float speed;

    public GameObject[] turnOffBoxes;
    public GameObject[] turnOnBoxes;

    float soundtime = 2;
    float soundtimer;

    Vector3 orgPos;
    bool isTotem;
    bool isWater;
    bool isDoor;
    bool isRumbling;
    bool audioActive;
    IEnumerator routine;
    bool isCheckStatus = false;


    private void Start()
    {
        orgPos = transform.position;
        soundtimer = soundtime;
        SelectEvent(events);
        audioSrc = GetComponent<AudioSource>();
        if (isDoor)
            ps.Stop();
        else if (isTotem)
        {
            foreach (GameObject objects in turnOnBoxes)
                objects.SetActive(false);
        }
        if (debug)
            StartEvent();

    }
    public void Update()
    {
        if (!isCheckStatus)
        {
            if (isTemple1MainDoor)
            {
                if (ObjectSystem.gameSwitch[8])
                {
                    StartSilentEvent();
                    isCheckStatus = true;
                }
            }
            else if (isTemple1BossDoor)
            {
                if (ObjectSystem.gameSwitch[15])
                {
                    StartSilentEvent();
                    isCheckStatus = true;
                }
            }
            else if (isTemple1Stairs)
            {
                if (ObjectSystem.gameSwitch[12] && ObjectSystem.gameSwitch[13])
                {
                    StartSilentEvent();
                    isCheckStatus = true;
                }
            }
            else if (isTemple1Totem)
            {
                if (ObjectSystem.gameSwitch[14])
                {
                    StartSilentEvent();
                    isCheckStatus = true;
                }
            }
        }
        if (isRumbling && !isCheckStatus)
        {
            if (soundtimer > 0)
            {
                soundtimer -= Time.deltaTime;
                if (!audioActive)
                {

                    audioSrc.PlayOneShot(rumbleFx);
                    audioActive = true;
                }
            }
            else if (soundtimer < 0)
            {

                audioActive = false;
                soundtimer = soundtime;
            }
        }
       
    }
    public void SelectEvent(Events type)
    {
        switch (type)
        {
            case Events.Water:
                {
                    isTotem = false;
                    isWater = true;
                    isDoor = false;
                    break;
                }
            case Events.Door:
                {
                    isTotem = false;
                    isWater = false;
                    isDoor = true;
                    break;
                }
            case Events.Totem:
                {
                    isTotem = true;
                    isWater = false;
                    isDoor = false;
                    break;
                }
        }
    }
    public IEnumerator EventRoutine()
    {
        if (lower)
        {
            while (transform.localPosition.y > minHeight)
            {
                yield return new WaitUntil(() => DialogueSystem.isDialogueActive == false);
                transform.Translate(Vector3.down * Time.deltaTime * speed);
                yield return new WaitForEndOfFrame();
            }
            if (transform.localPosition.y <= minHeight && isWater)
            {
                yield return new WaitUntil(() => DialogueSystem.isDialogueActive == false);
                foreach (GameObject objects in turnOffBoxes)
                    objects.SetActive(false);
                foreach(GameObject objects in turnOnBoxes)
                    objects.SetActive(true);
            }
            else if (transform.localPosition.y <= minHeight && isDoor)
            {
                if (isRumbling && !isCheckStatus)
                    audioSrc.PlayOneShot(doorSlamFx);
                isRumbling = false;
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    SwordSystem swordSys = player.GetComponent<SwordSystem>();
                    swordSys.ShutOffQuake();
                }
                else
                    yield return null;
                ps.Stop();
                soundtimer = soundtime;
            }
        }
        else if (raise)
        {
            while (transform.localPosition.y < maxHeight)
            {
                yield return new WaitUntil(() => DialogueSystem.isDialogueActive == false);
                transform.Translate(Vector3.up * Time.deltaTime * speed);
                yield return new WaitForEndOfFrame();
            }
            if (transform.localPosition.y >= maxHeight && isTotem)
            {
                yield return new WaitUntil(() => DialogueSystem.isDialogueActive == false);
                foreach (GameObject objects in turnOffBoxes)
                    objects.SetActive(false);
                foreach (GameObject objects in turnOnBoxes)
                    objects.SetActive(true);
                if (isRumbling && !isCheckStatus)
                    audioSrc.PlayOneShot(doorSlamFx);
                isRumbling = false;
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    SwordSystem swordSys = player.GetComponent<SwordSystem>();
                    swordSys.ShutOffQuake();
                }
                else
                    yield return null;
            }
            if (transform.localPosition.y >= maxHeight && isDoor)
            {
                if (isRumbling && !isCheckStatus)
                    audioSrc.PlayOneShot(doorSlamFx);
                isRumbling = false;
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                SwordSystem swordSys = player.GetComponent<SwordSystem>();
                swordSys.ShutOffQuake();
                soundtimer = soundtime;
      
            }
        }

    }
    public void StartSilentEvent()
    {
        isRumbling = false;
        if (routine != null)
            StopCoroutine(routine);
        routine = EventRoutine();
        StartCoroutine(routine);
    }
    public void StartEvent()
    {
        isRumbling = true;
        if(isDoor)
            ps.Play();
        if (routine != null)
            StopCoroutine(routine);
        routine = EventRoutine();
        StartCoroutine(routine);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Head") && !PlayerSystem.stormBootEnabled)
        {
            transform.position = orgPos;
        }
    }
   
}
