using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnimateText : MonoBehaviour
{
    // Start is called before the first frame update
    string message;
    public AudioSource audioSrc;
    public AudioClip scrollSound;
    bool fileDeletion;
    Text textMessage;
    bool reset;
    IEnumerator routine;


    void Start()
    {
        
    }
    IEnumerator Animate()
    {
        textMessage = GetComponent<Text>();
        textMessage.text = null;
        foreach (char letter in message.ToCharArray())
        {
            if (reset)
                break;
            else
            {
                audioSrc.volume = 0.7f;
                audioSrc.pitch = Random.Range(0.9f, 1.1f);
                audioSrc.PlayOneShot(scrollSound);
                textMessage.text += letter;
                yield return 0;
                yield return new WaitForSeconds(.01f);
            }
        }
    }
    public void StartAnimating(int num)
    {
        reset = false;
        textMessage = GetComponent<Text>();
        textMessage.text = null;
        fileDeletion = SelectComponents.FileDeletion;
        string[] subject = new string[2] { "load", "delete" };
        int index = 0;
        if (!fileDeletion) index = 0;
        else if (fileDeletion) index = 1;
        for (int m = 0; m < 4; m++) if (m == num) message = "Would you like to " + subject[index] + "File " + (m + 1) + "?";
        if (routine != null)
            StopCoroutine(routine);
        routine = Animate();
        StartCoroutine(routine);
    }
    public void StopAnimating()
    {
        textMessage = GetComponent<Text>();
        if (routine != null)
            StopCoroutine(routine);
        textMessage.text = null;
        reset = true;
    }
    public void DeletedFileOnClick(int num)
    {
        
        fileDeletion = SelectComponents.FileDeletion;
        if (fileDeletion)
        {
            textMessage = GetComponent<Text>();
            textMessage.text = null;
            for(int ng = 0; ng < 4; ng++)
            {
                if (num == ng)
                {
                    if (!CoreObject.newGame[ng]) message = "File " + (ng + 1) + "has been deleted...";
                    else if (CoreObject.newGame[ng]) message = "No file found...";
                }
            }
            reset = false;
            if (routine != null)
                StopCoroutine(routine);
            routine = Animate();
            StartCoroutine(routine);
        }
        
    }
}
