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
        if(currentSwitch != 0)
            objectSystem.SetSwitchActive(currentSwitch);
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
                    objectSystem.SetEventActive(2);
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
                    objectSystem.SetEventActive(5);
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
            if (switchNum == 1)
            {
                if (ObjectSystem.switch1)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 2)
            {
                if (ObjectSystem.switch2)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 3)
            {
                if (ObjectSystem.switch3)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 4)
            {
                if (ObjectSystem.switch4)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 5)
            {
                if (ObjectSystem.switch5)
                    TurnOnSwitch(false);
            }
            if (switchNum == 6)
            {
                if (ObjectSystem.switch6)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 7)
            {
                if (ObjectSystem.switch7)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 8)
            {
                if (ObjectSystem.switch8)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 9)
            {
                if (ObjectSystem.switch9)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 10)
            {
                if (ObjectSystem.switch10)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 11)
            {
                if (ObjectSystem.switch11)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 12)
            {
                if (ObjectSystem.switch12)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 13)
            {
                if (ObjectSystem.switch13)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 14)
            {
                if (ObjectSystem.switch14)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 15)
            {
                if (ObjectSystem.switch15)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 16)
            {
                if (ObjectSystem.switch16)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 17)
            {
                if (ObjectSystem.switch17)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 18)
            {
                if (ObjectSystem.switch18)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 19)
            {
                if (ObjectSystem.switch19)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 20)
            {
                if (ObjectSystem.switch20)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 21)
            {
                if (ObjectSystem.switch21)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 22)
            {
                if (ObjectSystem.switch22)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 23)
            {
                if (ObjectSystem.switch23)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 24)
            {
                if (ObjectSystem.switch24)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 25)
            {
                if (ObjectSystem.switch25)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 26)
            {
                if (ObjectSystem.switch26)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 27)
            {
                if (ObjectSystem.switch27)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 28)
            {
                if (ObjectSystem.switch28)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 29)
            {
                if (ObjectSystem.switch29)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 30)
            {
                if (ObjectSystem.switch30)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 31)
            {
                if (ObjectSystem.switch31)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 32)
            {
                if (ObjectSystem.switch32)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 33)
            {
                if (ObjectSystem.switch33)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 34)
            {
                if (ObjectSystem.switch34)
                    TurnOnSwitch(false);
            }
            else if (switchNum == 35)
            {
                if (ObjectSystem.switch35)
                    TurnOnSwitch(false);
            }
        }
    }
}
