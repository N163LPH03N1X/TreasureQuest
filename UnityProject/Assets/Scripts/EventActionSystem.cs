using UnityEngine;
using System.Collections;

public class EventActionSystem : MonoBehaviour
{
    public int currentEvent;
    [Header ("Pedastal - Event 1")]
    public int maxDistance;
    public bool triggerEvent1 = false;
    [Space]
    [Header("Open Wood Wall - Event 2")]
    public bool triggerEvent2 = false;
    Vector3 newEvent2Position = new Vector3(303f, 84.5f, 1908f);
    [Space]
    [Header("Raise Spiral Stairs - Event 3")]
    public bool triggerEvent3 = false;
    [Space]
    [Header("Raise Totem Boss Temple 1 - Event 4")]
    public bool triggerEvent4 = false;
    [Space]
    [Header("Open Well Entrance - Event 5")]
    public bool triggerEvent5 = false;
    public GameObject[] event5ObjectsOn;
    public GameObject[] event5ObjectsOff;
    IEnumerator quake;
    bool isChecked;
    ObjectSystem objectSystem;

    private void Awake()
    {
        CheckEventSystem(currentEvent);
    }
    private void Start()
    {
        if (event5ObjectsOn.Length != 0)
        {
            foreach (GameObject obj in event5ObjectsOn)
            {
                obj.SetActive(false);
            }
        }
        if (event5ObjectsOff.Length != 0)
        {
            foreach (GameObject obj in event5ObjectsOff)
            {
                obj.SetActive(true);
            }
        }
    }
    void Update()
    {

        if (triggerEvent1)
        {
            transform.Translate(Vector3.right * Time.deltaTime * 1);
            if (transform.localPosition.x > maxDistance)
            {
                Vector3 newPosition = transform.localPosition;
                newPosition.x = maxDistance;
                transform.localPosition = newPosition;
                if (newPosition.x == maxDistance)
                {
                    objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
                    if (currentEvent != 0)
                        objectSystem.SetEventActive(currentEvent);
                    isChecked = true;
                    triggerEvent1 = false;
                }
            }
        }
        else if (triggerEvent2)
        {
            transform.position = newEvent2Position;
            BoxCollider box = GetComponent<BoxCollider>();
            box.enabled = false;
            objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
            if (currentEvent != 0)
                objectSystem.SetEventActive(currentEvent);
            isChecked = true;
            triggerEvent2 = false;
        }
        else if (triggerEvent3)
        {
            ObjectEventSystem raiseStairs = GetComponent<ObjectEventSystem>();
            raiseStairs.StartEvent();
            objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
            if (currentEvent != 0)
                objectSystem.SetEventActive(currentEvent);
            isChecked = true;
            triggerEvent3 = false;
        }
        else if (triggerEvent4)
        {
            ObjectEventSystem raiseTotem = transform.GetChild(4).GetComponent<ObjectEventSystem>();
            raiseTotem.StartEvent();
            objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
            if (currentEvent != 0)
                objectSystem.SetEventActive(currentEvent);
            QuakePlayer(5);
            isChecked = true;
            triggerEvent4 = false;
        }
        else if (triggerEvent5)
        {
            if (event5ObjectsOn.Length != 0)
            {
                foreach (GameObject obj in event5ObjectsOn)
                {
                    obj.SetActive(true);
                }
            }
            if (event5ObjectsOff.Length != 0)
            {
                foreach (GameObject obj in event5ObjectsOff)
                {
                    obj.SetActive(false);
                }
            }
            objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
            if (currentEvent != 0)
                objectSystem.SetEventActive(currentEvent);
            isChecked = true;
            triggerEvent5 = false;
        }
        if (!isChecked)
            CheckEventSystem(currentEvent);
    }
    public void QuakePlayer(float seconds)
    {
        SwordSystem swordSys = GameObject.FindGameObjectWithTag("Player").GetComponent<SwordSystem>();
        if (quake != null)
            StopCoroutine(quake);
        quake = swordSys.QuakeEffect(seconds);
        StartCoroutine(quake);
    }
    public void CheckEventSystem(int num)
    {
        if(num == 1)
        {
            if (ObjectSystem.event1)
            {
                TriggerEvent(1);
                isChecked = true;
            }
        }
        else if (num == 2)
        {
            if (ObjectSystem.event2)
            {
                TriggerEvent(2);
                isChecked = true;
            }
        }
        else if (num == 3)
        {
            if (ObjectSystem.event3)
            {
                TriggerEvent(3);
                isChecked = true;
            }
        }
        else if (num == 4)
        {
            if (ObjectSystem.event4)
            {
                TriggerEvent(4);
                isChecked = true;
            }
        }
        else if (num == 5)
        {
            if (ObjectSystem.event5)
            {
                TriggerEvent(5);
                isChecked = true;
            }
        }
       
    }
    public void TriggerEvent(int num)
    {
        if(num == 1)
            triggerEvent1 = true;
        else if (num == 2)
            triggerEvent2 = true;
        else if (num == 3)
            triggerEvent3 = true;
        else if (num == 4)
            triggerEvent4 = true;
        else if (num == 5)
            triggerEvent5 = true;
    }
}
