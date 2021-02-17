using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class InteractionSystem : MonoBehaviour
{
    public Text aButtonText;
    public GameObject aButtonImage;
    public bool useInstant;
    DialogueSystem diagSys;
    IEnumerator routine = null;
    string setMessage;
    void Start()
    {
        diagSys = GetComponent<DialogueSystem>();
        aButtonImage.GetComponent<Image>().enabled = false;
    }

    public void DialogueInteraction(bool active, string message)
    {
        if (active)
        {
            CharacterSystem charSys = GetComponent<CharacterSystem>();
            charSys.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
            diagSys.DialogueBanner.SetActive(true);
            setMessage = message;
            diagSys.messages.popMessage = setMessage;
            if (routine != null)
                StopCoroutine(routine);
            routine = diagSys.AnimateMessage();
            StartCoroutine(routine);
            
        }
        else
        {
            if (routine != null)
                StopCoroutine(routine);
            diagSys.messages.popMessage = null;
            setMessage = null;
            diagSys.dialogueText.text = null;
            diagSys.SetButtonPress(4);
        }
    }
    public void DialogueFill()
    {
        if (routine != null)
            StopCoroutine(routine);
        diagSys.DialogueBanner.SetActive(true);
        diagSys.messages.popMessage = setMessage;
        diagSys.FillMessage(true);
    }

}
