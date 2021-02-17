using System.Collections;
using UnityEngine;

public class EventCounterActivate : MonoBehaviour
{
    public int count;
    public int setCount;
    public CurrentEvent curEvent;
    bool eventFinished;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (count >= setCount && !eventFinished)
        {
            SelectCurrentEvent(curEvent);
            eventFinished = true;
        }
    }
    public enum CurrentEvent { SpiralStairs }
    public void SelectCurrentEvent(CurrentEvent eventCur)
    {
        switch (eventCur)
        {
            case CurrentEvent.SpiralStairs:
                {

                    if (!ObjectSystem.event3)
                    {
                        SwordSystem swordSys = GameObject.FindGameObjectWithTag("Player").GetComponent<SwordSystem>();
                        IEnumerator routine = swordSys.QuakeEffect(5.5f);
                        StartCoroutine(routine);
                    }
                    else if (ObjectSystem.event3)
                    {
                        AudioSource audioSrc = GetComponent<AudioSource>();
                        audioSrc.mute = true;
                    }
                    EventActionSystem EAS = GetComponent<EventActionSystem>();
                    EAS.TriggerEvent(3);
                   
                    break;
                }
        }
    }
    public void SetEventCounter(int amount)
    {
        count += amount;
    }
}
