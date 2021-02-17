using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSystem : MonoBehaviour
{
    public float xSpinSpeed;
    public float ySpinSpeed;
    public float zSpinSpeed;
    public Text levelTitle;
    Image JewelImage;
    public GameObject previewObj;
    RawImage loadPreview;
    public PreviewImages preImages;
    

    public int randomJewelNum;
    public int randomTipNum;
    public int randomPreImgNum;
    public Text tipText;
    string tipMessage;
   
    public string[] tipMessages;
    float tipSwitchTimer;
    public float setTipTimer;
    public float setImageTimer;
    public float letterTime;
    IEnumerator textRoutine;
    IEnumerator imageRoutine;
    IEnumerator jewelRoutine;
    bool flip;
    void Start()
    {
        //tipMessage = null;
        //JewelImage = GetComponent<Image>();
        //loadPreview = previewObj.GetComponent<RawImage>();
        //tipSwitchTimer = 10;
        //randomJewelNum = UnityEngine.Random.Range(0,5);
        //randomTipNum = UnityEngine.Random.Range(0, 20);
        //randomPreImgNum = UnityEngine.Random.Range(0, 10);
        //RandomizeScreens(randomPreImgNum);
        //RandomizeTips(randomTipNum);
        //jewelRoutine = AnimateJewelFade(setImageTimer, JewelImage, true);
        //imageRoutine = AnimateImageFade(setImageTimer, loadPreview, true);
        //StartCoroutine(imageRoutine);
        //textRoutine = AnimateTipText();
        //StartCoroutine(textRoutine);
        //StartCoroutine(jewelRoutine);

    }

    // Update is called once per frame
    void Update()
    {
        JewelSpin(xSpinSpeed, ySpinSpeed, zSpinSpeed);
        RandomizeJewels(randomJewelNum);

        if (tipSwitchTimer > 0)
        {
            tipSwitchTimer -= Time.fixedDeltaTime;
        }
        else if (tipSwitchTimer < 0)
        {
            RandomizeSystem();
        }
    }
    public void RandomizeSystem()
    {
      
        JewelImage = GetComponent<Image>();
        loadPreview = previewObj.GetComponent<RawImage>();
        if (textRoutine != null)
            StopCoroutine(textRoutine);
        if (imageRoutine != null)
            StopCoroutine(imageRoutine);
        if (jewelRoutine != null)
            StopCoroutine(jewelRoutine);
        tipText.text = null;
        randomTipNum = UnityEngine.Random.Range(0, 20);
        randomPreImgNum = UnityEngine.Random.Range(0, 20);
        RandomizeScreens(randomPreImgNum);
        RandomizeTips(randomTipNum);
        textRoutine = AnimateTipText();
        imageRoutine = AnimateImageFade(setImageTimer, loadPreview, false);
        jewelRoutine = AnimateJewelFade(setImageTimer, JewelImage, false);
        StartCoroutine(imageRoutine);
        StartCoroutine(textRoutine);
        StartCoroutine(jewelRoutine);
        tipSwitchTimer = setTipTimer;
    }
    void JewelSpin(float x, float y, float z)
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.Rotate(x, y, z);
    }
    public void RandomizeScreens(int num)
    {
        if (SceneSystem.scene == 2)
        {
            levelTitle.text = "Conquest Island";
            loadPreview.texture = preImages.overWorldPreviews[num];
        }
        else if (SceneSystem.scene == 1)
        {
            levelTitle.text = "Riverfall Shrine";
            loadPreview.texture = preImages.dungon1Previews[num];
        }
        else if (SceneSystem.scene == 4)
        {
            levelTitle.text = "Abandoned Ruins";
            loadPreview.texture = preImages.dungeon2Previews[num];
        }
        else if (SceneSystem.scene == 3)
        {
            levelTitle.text = "The Cursed Grail";
            loadPreview.texture = preImages.ghostShipPreviews[num];
        }
        else if (SceneSystem.scene == 5)
        {
            levelTitle.text = "Temple of Reign";
            loadPreview.texture = preImages.temple1Previews[num];
        }
        else if (SceneSystem.scene == 6)
        {
            levelTitle.text = "Rafter's Bargain";
            loadPreview.texture = preImages.IntroScenePreviews[num];
        }
        else if (SceneSystem.scene == 7)
        {
            levelTitle.text = "Prototype Level";
            int ranNum = UnityEngine.Random.Range(0, 1);
            loadPreview.texture = preImages.blank[ranNum];
        }

        else if (SceneSystem.scene == 8)
        {
            levelTitle.text = "Dwelling Timber";
            loadPreview.texture = preImages.treePreviews[num];
        }
        else if (SceneSystem.scene == 9)
        {
            levelTitle.text = "Well Pathway";
            loadPreview.texture = preImages.dungeon3Previews[num];
        }
        else
        {
            int ranNum = UnityEngine.Random.Range(0, 1);
            loadPreview.texture = preImages.blank[ranNum];
            levelTitle.text = "Main Menu";
         
        }
        loadPreview.enabled = true;
      

    }
    public void RandomizeTips(int num)
    {
        tipMessage = tipMessages[num];
    }
    public void RandomizeJewels(int num)
    {
        JewelImage.sprite = preImages.Jewels[num];
        JewelImage.enabled = true;
    }
    IEnumerator AnimateTipText()
    {
        foreach (char letter in tipMessage.ToCharArray())
        {
            tipText.text += letter;
            yield return 0;
            yield return new WaitForSecondsRealtime(letterTime);
        }
    }
    IEnumerator AnimateImageFade(float aTime, RawImage image, bool fadeIn)
    {
        float alpha = image.GetComponent<RawImage>().color.a;
        float aValue;
        if (fadeIn)
            aValue = 1.0f;
        else
            aValue = 0.0f;
        for (float t = 0.0f; t < 1.0f; t += Time.fixedDeltaTime / aTime)
        {
            Color newColor = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(alpha, aValue, t));
            image.GetComponent<RawImage>().color = newColor;
            if (t >= 0.9)
            {
                if (fadeIn)
                    newColor = new Color(1, 1, 1, 1);
                else
                {
                    if (imageRoutine != null)
                        StopCoroutine(imageRoutine);
                    imageRoutine = AnimateImageFade(setImageTimer, loadPreview, true);
                    StartCoroutine(imageRoutine);
                    newColor = new Color(1, 1, 1, 0);
                }
                image.GetComponent<RawImage>().color = newColor;
               
            }
            else
                image.GetComponent<RawImage>().enabled = true;
            yield return null;
        }
    }
    IEnumerator AnimateJewelFade(float aTime, Image image, bool fadeIn)
    {
        float alpha = image.GetComponent<Image>().color.a;
        float aValue;
        if (fadeIn)
            aValue = 1.0f;
        else
            aValue = 0.0f;
        for (float t = 0.0f; t < 1.0f; t += Time.fixedDeltaTime / aTime)
        {
            Color newColor = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(alpha, aValue, t));
            image.GetComponent<Image>().color = newColor;
            if (t >= 0.9)
            {
                if (fadeIn)
                    newColor = new Color(1, 1, 1, 1);
                else
                {
                    randomJewelNum = UnityEngine.Random.Range(0, 5);
                    newColor = new Color(1, 1, 1, 0);
                    if (jewelRoutine != null)
                        StopCoroutine(jewelRoutine);
                    jewelRoutine = AnimateJewelFade(setImageTimer, JewelImage, true);
                    StartCoroutine(jewelRoutine);
                }
                image.GetComponent<Image>().color = newColor;

            }
            else
                image.GetComponent<Image>().enabled = true;
            yield return null;
        }
    }
}
[Serializable]
public struct PreviewImages
{
    public Texture[] IntroScenePreviews;
    public Texture[] overWorldPreviews;
    public Texture[] dungon1Previews;
    public Texture[] dungeon2Previews;
    public Texture[] ghostShipPreviews;
    public Texture[] temple1Previews;
    public Texture[] treePreviews;
    public Texture[] dungeon3Previews;


    public Texture[] blank;

    public Sprite[] Jewels;
  
}
