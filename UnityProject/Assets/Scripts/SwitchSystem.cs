using UnityEngine;
using System.Collections;
public enum SwitchType { Timed, Permanent, Instantiate, Release, Event }

public class SwitchSystem : MonoBehaviour
{
    public SwitchType switchType;
    public SwitchEventType eventType;
    public int currentSwitch;
    public float setTime;
    float timer;
    AudioSource audioSrc;
    public AudioClip switchSfx;
    public AudioClip switchSfxTask;
    public Transform instantiatePos;
    public GameObject[] turnOnObjs;
    public GameObject[] turnOffObjs;
    public GameObject objPrefab;

    Renderer rend;
    Color orgColor;
    public bool On;
    public bool released;
    public bool active = true;
    bool isChecked = false;
    public bool QuakeEvent;
    public bool mainTask;
    public bool isSilent;
    public bool isSplit;
    IEnumerator effectRoutine;

    private void Awake()
    {
        CheckSwitchSystem(currentSwitch);
    }

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();
        orgColor = rend.material.color;
        timer = setTime;
      
        foreach (GameObject gameObj in turnOnObjs)
            gameObj.SetActive(false);
        if (!isSplit)
        {
            foreach (GameObject gameObj in turnOffObjs)
                gameObj.SetActive(true);
        }
        
    }

    void Update()
    {
        if (On && !active)
        {
            Color color = Color.red;
            float f = 1f;
            GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            GetComponent<Renderer>().material.color = color;
            GetComponent<Renderer>().material.SetColor("_EmissionColor", color * f);
            SetSwitchType(switchType);
        }
        else if(!On && active)
        {
            float f = 0.1f;
            GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            GetComponent<Renderer>().material.color = orgColor;
            GetComponent<Renderer>().material.SetColor("_EmissionColor", orgColor * f);
            if (released)
            {
                foreach (GameObject gameObj in turnOnObjs)
                    gameObj.SetActive(false);
                foreach (GameObject gameObj in turnOffObjs)
                    gameObj.SetActive(true);
                released = false;
            }
            active = false;
        }
        if (currentSwitch != 0)
            CheckSwitchSystem(currentSwitch);
    }
    public void SetSwitchType(SwitchType type)
    {
        if (QuakeEvent)
        {
            SwordSystem swordSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<SwordSystem>();
            effectRoutine = swordSystem.QuakeEffect(6);
            StartCoroutine(effectRoutine);
            QuakeEvent = false;
        }

        ObjectSystem objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
        switch (type)
        {
            case SwitchType.Instantiate:
                {
                    if (!active)
                    {
                        Instantiate(objPrefab, instantiatePos.position, instantiatePos.rotation);
                        isChecked = true;
                        active = true;
                    }
                    break;
                }
            case SwitchType.Timed:
                {
                    if (timer > 0)
                    {
                        timer -= Time.deltaTime;
                            foreach (GameObject gameObj in turnOnObjs)
                                gameObj.SetActive(true);
                            foreach (GameObject gameObj in turnOffObjs)
                                gameObj.SetActive(false);
                    }
                    else if (timer < 0)
                    {
                        On = false;
                        foreach (GameObject gameObj in turnOnObjs)
                            gameObj.SetActive(false);
                        foreach (GameObject gameObj in turnOffObjs)
                            gameObj.SetActive(true);
                        timer = setTime;
                        active = true;
                    }
                    break;
                }
            case SwitchType.Permanent:
                {
                    foreach (GameObject gameObj in turnOnObjs)
                    {
                        if(gameObj != null)
                            gameObj.SetActive(true);
                    }
                        
                    foreach (GameObject gameObj in turnOffObjs)
                        gameObj.SetActive(false);
                    isChecked = true;
                    active = true;
                    break;
                }
            case SwitchType.Release:
                {
                    released = true;
                    foreach (GameObject gameObj in turnOnObjs)
                        gameObj.SetActive(true);
                    foreach (GameObject gameObj in turnOffObjs)
                        gameObj.SetActive(false);
                    active = true;
                    break;
                }
            case SwitchType.Event:
                {
                    active = true;
                    isChecked = true;
                    SelectSwitchEvent(eventType);
                        
                    break;
                   
                }
        }
        objectSystem.SetActiveObject(currentSwitch, ObjectSystem.gameSwitch);
    }
    public enum SwitchEventType { Dungeon1Water, ActivateEnemies, OverWorldWoodWall, ActivateTorch, OverWorldWindWell }

    public void SelectSwitchEvent(SwitchEventType type)
    {
        ObjectSystem objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
        
        switch (type)
        {
            case SwitchEventType.Dungeon1Water:
                {
                    ObjectEventSystem oEvent = objPrefab.GetComponent<ObjectEventSystem>();
                    oEvent.StartEvent();
                    foreach (GameObject gameObj in turnOnObjs)
                        gameObj.SetActive(true);
                    foreach (GameObject gameObj in turnOffObjs)
                        gameObj.SetActive(false);
                    active = true;
                    break;
                }
            case SwitchEventType.ActivateEnemies:
                {
                    foreach (GameObject gameObj in turnOnObjs)
                    {
                        if (gameObj != null)
                        {
                            EnemySystem EnemySys = gameObj.GetComponent<EnemySystem>();
                            EnemySys.TurnOnReal(true);
                        }
                    }
                    active = true;
                    break;
                }
            case SwitchEventType.OverWorldWoodWall:
                {
                    objectSystem.SetActiveObject(2, ObjectSystem.gameEvent);
                    active = true;
                    break;
                }
            case SwitchEventType.ActivateTorch:
                {
                    foreach (GameObject gameObj in turnOnObjs)
                    {
                        gameObj.SetActive(true);
                        ParticleSystem ps = gameObj.GetComponent<ParticleSystem>();
                        ps.Play();
                    }
                    EventCounterActivate torchObj = objPrefab.GetComponent<EventCounterActivate>();
                    torchObj.SetEventCounter(1);
                    active = true;
                    break;
                }
            case SwitchEventType.OverWorldWindWell:
                {
                    ObjectEventSystem oEvent = objPrefab.GetComponent<ObjectEventSystem>();
                    oEvent.StartEvent();
                    objectSystem.SetActiveObject(5, ObjectSystem.gameEvent);
                    foreach (GameObject gameObj in turnOnObjs)
                    {
                        if (gameObj != null)
                            gameObj.SetActive(true);
                    }

                    foreach (GameObject gameObj in turnOffObjs)
                        gameObj.SetActive(false);

                    active = true;
                    break;
                }


        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !On)
        {
            On = true;
            if(!isSilent)
                audioSrc.PlayOneShot(switchSfx);
            if(mainTask)
                audioSrc.PlayOneShot(switchSfxTask);
            

        }
        else if (other.gameObject.CompareTag("KillBlock") && !On)
        {
            On = true;
            if (!isSilent)
                audioSrc.PlayOneShot(switchSfx);
        }

    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && released)
        {
            On = false;
        }
        else if (other.gameObject.CompareTag("KillBlock") && released)
        {
            On = false;
        }
    }
    public void TurnOnSwitch(bool quake)
    {
        QuakeEvent = quake;
        active = false;
        On = true;
        isChecked = true;
    }
    public void CheckSwitchSystem(int switchNum)
    {
        if (!isChecked)
        {
            for(int s = 0; s < ObjectSystem.gameSwitch.Length; s++)
            {
                if (s == switchNum)
                {
                    if (ObjectSystem.gameSwitch[s])
                    {
                        TurnOnSwitch(false);
                        break;
                    }
                }
            }
        }
    }
}
