using System;
using UnityEngine;

public enum CheckPoint { DuskCliff, AbandonedBog, WindAcre, WindAcreDock, DuskCliffShop, WindAcreShop, SkulkCove, SkulkCoveShop, ShadowedBog, OverWorld, FishersTrawl, DwellingTimber }

public class CheckPointSystem : MonoBehaviour
{
    SceneSystem sceneSys;
    PlayerSystem playSys;
    public CheckPoint checkpointType;
    public CheckPointMusic checkMusic;
    bool isDuskCliff;
    bool isDuskCliffShop;
    bool isAbandonedBog;
    bool isWindAcre;
    bool isWindAcreShop;
    bool isWindAcreDock;
    bool isSkulkCove;
    bool isSkulkCoveShop;
    bool isShadowedBog;
    bool isFishersTrawl;
    bool isDwellingTimber;

    public void Start()
    {
        SetCheckPoint(checkpointType);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            playSys = other.gameObject.GetComponent<PlayerSystem>();
            playSys.SavePosition();
            sceneSys = GameObject.Find("Core/Player").GetComponent<SceneSystem>();
            if (isDuskCliff)
                sceneSys.SetPlaceName(SceneSystem.Place.duskCliff, checkMusic.duskMusic);
            else if (isDuskCliffShop)
                sceneSys.SetPlaceName(SceneSystem.Place.duskCliffShop, checkMusic.duskMusic);
            else if (isWindAcre)
                sceneSys.SetPlaceName(SceneSystem.Place.windAcre, checkMusic.windMusic);
            else if (isWindAcreShop)
                sceneSys.SetPlaceName(SceneSystem.Place.windacreShop, checkMusic.windMusic);
            else if (isWindAcreDock)
                sceneSys.SetPlaceName(SceneSystem.Place.windAcreDock, checkMusic.overMusic);
            else if (isAbandonedBog)
                sceneSys.SetPlaceName(SceneSystem.Place.abandonedBog, checkMusic.overMusic);
            else if (isSkulkCove)
                sceneSys.SetPlaceName(SceneSystem.Place.skulkCove, checkMusic.skulkMusic);
            else if (isSkulkCoveShop)
                sceneSys.SetPlaceName(SceneSystem.Place.skulkCoveShop, checkMusic.skulkMusic);
            else if (isShadowedBog)
                sceneSys.SetPlaceName(SceneSystem.Place.shadowedBog, checkMusic.overMusic);
            else if (isFishersTrawl)
                sceneSys.SetPlaceName(SceneSystem.Place.fishersTrawl, checkMusic.overMusic);
            else if (isDwellingTimber)
                sceneSys.SetPlaceName(SceneSystem.Place.dwellingTimber, checkMusic.dwellMusic);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playSys = other.gameObject.GetComponent<PlayerSystem>();
            playSys.SavePosition();
            sceneSys = GameObject.Find("Core/Player").GetComponent<SceneSystem>();
            if(isWindAcre)
                sceneSys.SetPlaceName(SceneSystem.Place.overworld, checkMusic.overMusic);
            else if(isDuskCliff)
                sceneSys.SetPlaceName(SceneSystem.Place.overworld, checkMusic.overMusic);
            else if (isSkulkCove)
                sceneSys.SetPlaceName(SceneSystem.Place.overworld, checkMusic.overMusic);
            else if (isDwellingTimber)
                sceneSys.SetPlaceName(SceneSystem.Place.overworld, checkMusic.overMusic);

        }
    }
    public void SetCheckPoint(CheckPoint type)
    {
        switch (type)
        {
            case CheckPoint.DuskCliff:
                {
                    isDuskCliff = true;
                    break;
                }
            case CheckPoint.DuskCliffShop:
                {
                    isDuskCliffShop = true;
                    break;
                }
            case CheckPoint.WindAcreShop:
                {
                    isWindAcreShop = true;
                    break;
                }
            case CheckPoint.SkulkCove:
                {
                    isSkulkCove = true;
                    break;
                }
            case CheckPoint.SkulkCoveShop:
                {
                    isSkulkCoveShop = true;
                    break;
                }
            case CheckPoint.AbandonedBog:
                {
                    isAbandonedBog = true;
                    break;
                }
            case CheckPoint.WindAcre:
                {
                    isWindAcre = true;
                    break;
                }
            case CheckPoint.WindAcreDock:
                {
                    isWindAcreDock = true;
                    break;
                }
            case CheckPoint.FishersTrawl:
                {
                    isFishersTrawl = true;
                    break;
                }
            case CheckPoint.DwellingTimber:
                {
                    isDwellingTimber = true;
                    break;
                }
            case CheckPoint.ShadowedBog:
                {
                    isShadowedBog = true;
                    break;
                }
        }
    }
}
[Serializable]
public struct CheckPointMusic
{
    public AudioClip duskMusic;
    public AudioClip windMusic;
    public AudioClip overMusic;
    public AudioClip skulkMusic;
    public AudioClip dwellMusic;
}
