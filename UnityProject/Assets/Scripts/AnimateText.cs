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
        textMessage = GetComponent<Text>();
        textMessage.text = null;
        fileDeletion = SelectComponents.FileDeletion;
        if (!fileDeletion)
        {
            if (num == 0)
                message = "Would you like to load File 1?";
            else if (num == 1)
                message = "Would you like to load File 2?";
            else if (num == 2)
                message = "Would you like to load File 3?";
            else if (num == 3)
                message = "Would you like to load File 4?";
            reset = false;
        }

        else if (fileDeletion)
        {
            if (num == 0)
                message = "Would you like to delete File 1?";
            else if (num == 1)
                message = "Would you like to delete File 2?";
            else if (num == 2)
                message = "Would you like to delete File 3?";
            else if (num == 2)
                message = "Would you like to delete File 4?";
            reset = false;
        }
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
            if (num == 0 && !CoreObject.newGameOne)
                message = "File 1 has been deleted...";
            else if (num == 0 && CoreObject.newGameOne)
                message = "No file found...";
            if (num == 1 && !CoreObject.newGameTwo)
                message = "File 2 has been deleted...";
            else if (num == 1 && CoreObject.newGameTwo)
                message = "No file found...";
            if (num == 2 && !CoreObject.newGameThree)
                message = "File 1 has been deleted...";
            else if (num == 2 && CoreObject.newGameThree)
                message = "No file found...";
            if (num == 3 && !CoreObject.newGameFour)
                message = "File 1 has been deleted...";
            else if (num == 3 && CoreObject.newGameFour)
                message = "No file found...";
            reset = false;
            if (routine != null)
                StopCoroutine(routine);
            routine = Animate();
            StartCoroutine(routine);
        }
        
    }
}
