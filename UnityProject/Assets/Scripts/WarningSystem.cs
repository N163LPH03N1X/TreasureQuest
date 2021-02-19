using UnityEngine;

public enum Warning { Lava, Ice, Water}
public class WarningSystem : MonoBehaviour
{
    OptSystem optSystem = new OptSystem();
    public Warning warningType;
    public GameObject blockOffObj;
    BoxCollider blockOff;
    DialogueSystem dialogueSystem;
    bool entered;

    bool lava;
    bool ice;
    bool water;
    
    private void Start()
    {
        SwitchWarning(warningType);
    }




    private void Update()
    {
        if (optSystem.Input.GetButtonDown("Submit") && entered)
        {
            InteractionSystem interactSys = GameObject.FindGameObjectWithTag("Player").GetComponent<InteractionSystem>();
            blockOff = blockOffObj.GetComponent<BoxCollider>();
            blockOff.enabled = true;
            interactSys.DialogueInteraction(false, null);
            entered = false;
        }
    }
    public void SwitchWarning(Warning type)
    {
        switch (type)
        {
            case Warning.Lava:
                {
                    ice = false;
                    lava = true;
                    water = false;
                    break;
                }
            case Warning.Ice:
                {
                    ice = true;
                    lava = false;
                    water = false;
                    break;
                }
            case Warning.Water:
                {
                    ice = false;
                    lava = false;
                    water = true;
                    break;
                }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (water || ice)
        {
            if (other.gameObject.CompareTag("Player") && PlayerSystem.stormBootEnabled)
            {
                blockOff = blockOffObj.GetComponent<BoxCollider>();
                blockOff.enabled = false;
            }
            else if (other.gameObject.CompareTag("Player") && !PlayerSystem.stormBootEnabled)
            {
                InteractionSystem interactSys = other.gameObject.GetComponent<InteractionSystem>();
                if (ice)
                    interactSys.DialogueInteraction(true, "Can't Climb Ice.");
                else if (water)
                    interactSys.DialogueInteraction(true, "Can't Swim.");

                blockOff = blockOffObj.GetComponent<BoxCollider>();
                blockOff.enabled = true;
                entered = true;
            }
        }
        else if (lava)
        {
            if (other.gameObject.CompareTag("Player") && PlayerSystem.cinderBootEnabled)
            {
                blockOff = blockOffObj.GetComponent<BoxCollider>();
                blockOff.enabled = false;
            }
            else if (other.gameObject.CompareTag("Player") && !PlayerSystem.cinderBootEnabled)
            {
                InteractionSystem interactSys = other.gameObject.GetComponent<InteractionSystem>();
                if (lava)
                    interactSys.DialogueInteraction(true, "Can't walk through lava.");
                blockOff = blockOffObj.GetComponent<BoxCollider>();
                blockOff.enabled = true;
                entered = true;
            }
        }
       

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            blockOff = blockOffObj.GetComponent<BoxCollider>();
            blockOff.enabled = true;
            InteractionSystem interactSys = other.gameObject.GetComponent<InteractionSystem>();
            interactSys.DialogueInteraction(false, null);
            entered = false;
        }
    }
}
