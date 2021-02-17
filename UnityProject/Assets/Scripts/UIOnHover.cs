using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOnHover : MonoBehaviour {

    // Grow parameters
    public float approachSpeed = 0.02f;
    public float growthBound = 2f;
    public float shrinkBound = 0.5f;
    private float currentRatio = 1;
    public bool uiImage;
    public bool uiText;
    public bool uiRawImage;
    public bool wasDisabled;
    // The text object we're trying to manipulate
    private Image image;
    private Text text;
    private RawImage rawImage;

    // And something to do the manipulating
    private IEnumerator pulse = null;
    private bool keepGoing = true;
    public bool StartUIHover;
    bool alreadygoing;

    // Attach the coroutine

    private void OnEnable()
    {
        if (wasDisabled)
        {
            if (uiImage)
                image = gameObject.GetComponent<Image>();
            if (uiText)
                text = gameObject.GetComponent<Text>();
            if (uiRawImage)
                rawImage = gameObject.GetComponent<RawImage>();
            if (StartUIHover)
                InitPulse(true);
        }
      
    }
    void Awake()
    {
        if (uiImage)
            image = gameObject.GetComponent<Image>();
        if (uiText)
            text = gameObject.GetComponent<Text>();
        if (uiRawImage)
            rawImage = gameObject.GetComponent<RawImage>();
        if (StartUIHover)
            InitPulse(true);
    }

    IEnumerator PulseImage()
    {
        while (keepGoing)
        {
            while (currentRatio != growthBound)
            {
                currentRatio = Mathf.MoveTowards(currentRatio, growthBound, approachSpeed);
                image.transform.localScale = Vector3.one * currentRatio;
                yield return new WaitForEndOfFrame();
            }
            while (currentRatio != shrinkBound)
            {
                currentRatio = Mathf.MoveTowards(currentRatio, shrinkBound, approachSpeed);
                image.transform.localScale = Vector3.one * currentRatio;
                yield return new WaitForEndOfFrame();
            }
        }
    }
    IEnumerator PulseText()
    {
        while (keepGoing)
        {
            while (currentRatio != growthBound)
            {
                currentRatio = Mathf.MoveTowards(currentRatio, growthBound, approachSpeed);
                text.transform.localScale = Vector3.one * currentRatio;
                yield return new WaitForEndOfFrame();
            }
            while (currentRatio != shrinkBound)
            {
                currentRatio = Mathf.MoveTowards(currentRatio, shrinkBound, approachSpeed);
                text.transform.localScale = Vector3.one * currentRatio;
                yield return new WaitForEndOfFrame();
            }
        }
    }
    IEnumerator PulseRawImage()
    {
        while (keepGoing)
        {
            while (currentRatio != growthBound)
            {
                currentRatio = Mathf.MoveTowards(currentRatio, growthBound, approachSpeed);
                rawImage.transform.localScale = Vector3.one * currentRatio;
                yield return new WaitForEndOfFrame();
            }
            while (currentRatio != shrinkBound)
            {
                currentRatio = Mathf.MoveTowards(currentRatio, shrinkBound, approachSpeed);
                rawImage.transform.localScale = Vector3.one * currentRatio;
                yield return new WaitForEndOfFrame();
            }
        }
    }
    public void StartPulse()
    {
        keepGoing = true;
        if (!alreadygoing)
        {
            if (uiImage && isActiveAndEnabled)
            {
                pulse = PulseImage();
                StartCoroutine(pulse);
            }
            if (uiText && isActiveAndEnabled)
            {
                pulse = PulseText();
                StartCoroutine(pulse);
            }
            if (uiRawImage && isActiveAndEnabled)
            {
                pulse = PulseRawImage();
                StartCoroutine(pulse);
            }
            alreadygoing = true;
        }
    }
    public void StopPulse()
    {
        if (pulse != null)
            StopCoroutine(pulse);
        keepGoing = false;
        alreadygoing = false;
    }
    public void InitPulse(bool True)
    {
        if (True)
        {
            keepGoing = true;
            if (uiImage && isActiveAndEnabled)
            {
                pulse = PulseImage();
                StartCoroutine(pulse);
            }
            if (uiText && isActiveAndEnabled)
            {
                pulse = PulseText();
                StartCoroutine(pulse);
            }
            if (uiRawImage && isActiveAndEnabled)
            {
                pulse = PulseRawImage();
                StartCoroutine(pulse);
            }
            True = false;
        }
       
    }
}