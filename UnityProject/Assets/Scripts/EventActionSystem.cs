using UnityEngine;
using System.Collections;

public class EventActionSystem : MonoBehaviour
{
    public int currentEvent;
    public bool[] triggerEvent = new bool[5];
    [Header ("Pedastal - Event 1")]
    public int maxDistance;
    [Space]
    [Header("Open Wood Wall - Event 2")]
    Vector3 newEvent2Position = new Vector3(303f, 84.5f, 1908f);
    [Space]
    [Header("Open Well Entrance - Event 5")]
    public GameObject[] event5ObjectsOn;
    public GameObject[] event5ObjectsOff;
    [Space]
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
        if (triggerEvent[0])
        {
            if(currentEvent == 0)
                transform.Translate(Vector3.right * Time.deltaTime * 1);
            if (transform.localPosition.x > maxDistance)
            {
                Vector3 newPosition = transform.localPosition;
                newPosition.x = maxDistance;
                transform.localPosition = newPosition;
                if (newPosition.x == maxDistance)
                {
                    objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
                    objectSystem.SetActiveObject(currentEvent, ObjectSystem.gameEvent);
                    isChecked = true;
                    triggerEvent[0] = false;
                }
            }
        }
        else if (triggerEvent[1])
        {
            if (currentEvent == 1)
            {
                transform.position = newEvent2Position;
                BoxCollider box = GetComponent<BoxCollider>();
                box.enabled = false;
                objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
                objectSystem.SetActiveObject(currentEvent, ObjectSystem.gameEvent);
                isChecked = true;
                triggerEvent[1] = false;
            }
        }
        else if (triggerEvent[2])
        {
            if (currentEvent == 2)
            {
                ObjectEventSystem raiseStairs = GetComponent<ObjectEventSystem>();
                raiseStairs.StartEvent();
                objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
                objectSystem.SetActiveObject(currentEvent, ObjectSystem.gameEvent);
                isChecked = true;
                triggerEvent[2] = false;
            }
        }
        else if (triggerEvent[3])
        {
            if (currentEvent == 3)
            {
                ObjectEventSystem raiseTotem = transform.GetChild(4).GetComponent<ObjectEventSystem>();
                raiseTotem.StartEvent();
                objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
                objectSystem.SetActiveObject(currentEvent, ObjectSystem.gameEvent);
                QuakePlayer(5);
                isChecked = true;
                triggerEvent[3] = false;
            }
        }
        else if (triggerEvent[4])
        {
            if (currentEvent == 4)
            {
                for (int eOb = 0; eOb < event5ObjectsOn.Length; eOb++)
                    event5ObjectsOn[eOb].SetActive(true);
                for (int eOb = 0; eOb < event5ObjectsOff.Length; eOb++)
                    event5ObjectsOff[eOb].SetActive(true);
                objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
                objectSystem.SetActiveObject(currentEvent, ObjectSystem.gameEvent);
                isChecked = true;
                triggerEvent[4] = false;
            }
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
        for (int s = 0; s < ObjectSystem.gameEvent.Length; s++)
            if (s == num)
                if (ObjectSystem.gameEvent[s]) { TriggerEvent(s); isChecked = true; }
    }
    public void TriggerEvent(int num)
    {
        triggerEvent[num] = true;
    }
}
