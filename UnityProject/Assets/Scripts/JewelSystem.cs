using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public enum JewelActive {none, red, blue, green, yellow, purple }

public class JewelSystem : MonoBehaviour
{
    public AudioSource audioSrc;
    public AudioClip shockwaveFx;
    CharacterSystem charSys;
    GameObject rotation;
    GameObject projectile;
    RectTransform spin;
    float jewelChargeAmt;
    float jewelChargeSpeed;
    float redjewelFlashTimer = 0.2f;
    float redJewelFlashTime;
    public static bool isJewelEnabled = false;
    public static bool isJewelActive = false;

    float putAwayTime;
    float putAwayTimer = 0.5f;
    bool initJewelPower;
    bool hasJewel = false;
    bool jewelOn = false;
    bool redjewelUsed = false;
    bool yellowJewelUsed = false;
    bool flashJewel = false;
    bool returnTime;

    [Header("UI & Obj Assignment")]
    public JewelActive jewelActive;
    public GameObject[] playerModels;
    public JewelUI jewelUI;
    public Image jewelBar;
    public Image jewelIcon;
    public Sprite[] jewelIcons;
    public GameObject[] Jewels;
    public ParticleSystem[] jewelParticles;
    public float particleEmissionStrength;
    public GameObject jewelStatus;

    [Space]
    [Header("Red jewel")]
    
    public GameObject redJewelPowerPrefab;
    public bool jewelRedFullyCharged;
    public float redJewelChargeAmt;
    public float redJewelChargeSpeed;
    public float redJewelDrainSpeed;
    public static bool redJewelEnabled = false;
    public static bool redJewelPickedUp = false;
    [Space]
    [Header("Blue jewel")]
    public bool jewelBlueFullyCharged;
    public float blueJewelChargeAmt;
    public float blueJewelChargeSpeed;
    public float blueJewelDrainSpeed;
    public static bool blueJewelEnabled = false;
    public static bool blueJewelPickedUp = false;
    [Space]
    [Header("Green jewel")]
    public bool jewelGreenFullyCharged;
    public float greenJewelChargeAmt;
    public float greenJewelChargeSpeed;
    public float greenJewelDrainSpeed;
    public static bool greenJewelEnabled = false;
    public static bool greenJewelPickedUp = false;
    public float slowDownFactor = 0.01f;
    public float slowDownLength = 2f;
    [Space]
    [Header("Yellow jewel")]
    public bool jewelYellowFullyCharged;
    public float yellowJewelChargeAmt;
    public float yellowJewelChargeSpeed;
    public static bool yellowJewelEnabled = false;
    public static bool yellowJewelPickedUp = false;

    [Space]
    [Header("Purple jewel")]
    public GameObject purpleJewelPowerPrefab;
    public bool jewelPurpleFullyCharged;
    public float purpleJewelChargeAmt;
    public float purpleJewelChargeSpeed;
    public float purpleJewelDrainSpeed;
    public static bool purpleJewelEnabled = false;
    public static bool purpleJewelPickedUp = false;


    private void Start()
    {
        rotation = GameObject.Find("Core/Player/PlayerCanvas/GameUI/Effects/JewelSpin/Rotation");
        spin = rotation.GetComponent<RectTransform>();
        charSys = GetComponent<CharacterSystem>();
        putAwayTime = putAwayTimer;
        redJewelFlashTime = redjewelFlashTimer;
        if (SceneSystem.isNewGame)
        {
            SelectJewel(jewelActive);
        }
        //SelectJewel(jewelActive);
        SetupJewelUI();
    }

    void Update()
    {
 
     
        if (hasJewel && isJewelEnabled && !initJewelPower && !isJewelActive && !SelectionalSystem.isSelecting && !PlayerSystem.isDead)
        {
            if(Input.GetAxisRaw("Power") == 1 && !CharacterSystem.isCarrying && !PauseGame.isPaused && !SwordSystem.isAttacking && !SceneSystem.isDisabled && !SwordSystem.isSilenced && !CharacterSystem.isParalyzed && !ShopSystem.isShop || 
                Input.GetAxisRaw("Power") == -1 && !CharacterSystem.isCarrying && !PauseGame.isPaused && !SwordSystem.isAttacking && !SceneSystem.isDisabled && !SwordSystem.isSilenced && !CharacterSystem.isParalyzed && !ShopSystem.isShop ||
                     Input.GetButton("Power") && !CharacterSystem.isCarrying && !PauseGame.isPaused && !SwordSystem.isAttacking && !SceneSystem.isDisabled && !SwordSystem.isSilenced && !CharacterSystem.isParalyzed && !ShopSystem.isShop)
            {
                charSys.SelectAnimation(CharacterSystem.PlayerAnimation.JewelActivated, true);
                initJewelPower = true;
            }
        }
        else if(SelectionalSystem.isSelecting)
            initJewelPower = false;
        if (hasJewel && Input.GetButton("Engage") && !PlayerSystem.isDead && !CharacterSystem.isCarrying && !PauseGame.isPaused && !SwordSystem.isAttacking && !SceneSystem.isDisabled && !SwordSystem.isSilenced && !CharacterSystem.isParalyzed && !ShopSystem.isShop) 
        {
            if (putAwayTime > 0)
                putAwayTime -= Time.unscaledDeltaTime;
            else if (putAwayTime < 0)
            {
                if (!isJewelEnabled)
                    charSys.SelectAnimation(CharacterSystem.PlayerAnimation.TakeOutJewel, true);
                else if (isJewelEnabled)
                    charSys.SelectAnimation(CharacterSystem.PlayerAnimation.PutAwayJewel, true);
                putAwayTime = 0;
            }
        }
        else if (Input.GetButtonUp("Engage"))
            putAwayTime = putAwayTimer;
       

        if (returnTime && !PauseGame.isPaused)
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale = 1;
                AudioSource audioSrc = GameObject.Find("Core/GameMusic&Sound/GameMusic").GetComponent<AudioSource>();
                audioSrc.pitch = Time.timeScale;
                returnTime = false;
            }
        }
        ChargeJewel();
        Handler();
        JewelEnabled(isJewelActive);
    }
    public void SlowMotion()
    {
        if (!PauseGame.isPaused)
        {
            returnTime = false;
            Time.timeScale -= slowDownFactor * Time.unscaledDeltaTime;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0.5f, 1);
            AudioSource audioSrc = GameObject.Find("Core/GameMusic&Sound/GameMusic").GetComponent<AudioSource>();
            audioSrc.pitch = Time.timeScale;
        }
    }
    public void JewelEnabled(bool jewelEnabled)
    {
        if (jewelEnabled && !redJewelEnabled)
        {
            GameObject jewelSpinUI = GameObject.Find("Core/Player/PlayerCanvas/GameUI/Effects/JewelSpin");
            jewelOn = true;
            Image jewelSpinUIColor = jewelSpinUI.GetComponent<Image>();
           
            if (blueJewelEnabled)
                jewelSpinUIColor.color = Color.blue;
            else if (greenJewelEnabled)
                jewelSpinUIColor.color = Color.green;
            else if (yellowJewelEnabled)
                jewelSpinUIColor.color = Color.yellow;
            else if (purpleJewelEnabled)
                jewelSpinUIColor.color = new Color(0.2f, 0f, 0.5f, 1f);
            spin.Rotate(new Vector3(0, 0, 0.5f));
            jewelSpinUI.SetActive(true);
        }
        else if (redJewelEnabled && !redjewelUsed && flashJewel)
        {
            GameObject jewelSpinUI = GameObject.Find("Core/Player/PlayerCanvas/GameUI/Effects/JewelSpin");
            jewelOn = true;
            Image jewelSpinUIColor = jewelSpinUI.GetComponent<Image>();
            if (redJewelFlashTime > 0)
            {
                redJewelFlashTime -= Time.deltaTime;
                jewelSpinUIColor.color = Color.red;
            }
            else if (redJewelFlashTime < 0)
            {
                redJewelFlashTime = redjewelFlashTimer;
                redjewelUsed = true;
                flashJewel = false;

            }
            spin.Rotate(new Vector3(0, 0, 0.5f));
            jewelSpinUI.SetActive(true);
        }
        else
        {
            if (jewelOn)
            {
                GameObject jewelSpinUI = GameObject.Find("Core/Player/PlayerCanvas/GameUI/Effects/JewelSpin");
                jewelSpinUI.SetActive(false);
                jewelOn = false;
            }
        }
    }
    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
    public void Handler()
    {
        jewelBar.fillAmount = Map(jewelChargeAmt, 0, 100, 0, 1);
    }
    public void SetJewelActive()
    {
        for (int j = 0; j < Jewels.Length; j++)
        {
            if (redJewelEnabled)
                if (j != 0)
                    Jewels[j].SetActive(false);
                else
                    Jewels[j].SetActive(true);
            else if(blueJewelEnabled)
                if (j != 1)
                    Jewels[j].SetActive(false);
                else
                    Jewels[j].SetActive(true);
            else if (greenJewelEnabled)
                if (j != 2)
                    Jewels[j].SetActive(false);
                else
                    Jewels[j].SetActive(true);
            else if (yellowJewelEnabled)
                if (j != 3)
                    Jewels[j].SetActive(false);
                else
                    Jewels[j].SetActive(true);
            else if (purpleJewelEnabled)
                if (j != 4)
                    Jewels[j].SetActive(false);
                else
                    Jewels[j].SetActive(true);

        }
    }
    public void SelectJewel(JewelActive jewel)
    {
        
        switch (jewel)
        {
            case JewelActive.none:
                {

                    jewelIcon.enabled = false;
                    jewelBar.enabled = false;
                    redJewelEnabled = false;
                    blueJewelEnabled = false;
                    greenJewelEnabled = false;
                    yellowJewelEnabled = false;
                    purpleJewelEnabled = false;
                    jewelChargeSpeed = 0;
                    hasJewel = false;
                    jewelUI.statusJewelIconUI.sprite = null;
                    jewelUI.statusJewelIconUI.enabled = false;

                    jewelUI.statusJewelText.text = null;

                    
                    break;
                }
            case JewelActive.red:
                {
                    if (redJewelPickedUp)
                    {
                        hasJewel = true;
                        jewelIcon.enabled = true;
                        jewelBar.enabled = true;
                        jewelIcon.sprite = jewelIcons[0];
                        jewelBar.color = Color.red;
                        redJewelEnabled = true;
                        blueJewelEnabled = false;
                        greenJewelEnabled = false;
                        yellowJewelEnabled = false;
                        purpleJewelEnabled = false;
                        jewelChargeSpeed = redJewelChargeSpeed;

                        jewelUI.statusJewelIconUI.sprite = jewelUI.redJewelUI.sprite;
                        jewelUI.statusJewelIconUI.enabled = true;

                        jewelUI.statusJewelText.text = "Jewel of Power";
                       
                    }
                    break;
                }
            case JewelActive.blue:
                {
                    if (blueJewelPickedUp)
                    {
                        hasJewel = true;
                        jewelIcon.enabled = true;
                        jewelBar.enabled = true;
                        jewelIcon.sprite = jewelIcons[1];
                        jewelBar.color = Color.blue;
                        redJewelEnabled = false;
                        blueJewelEnabled = true;
                        greenJewelEnabled = false;
                        yellowJewelEnabled = false;
                        purpleJewelEnabled = false;
                        jewelChargeSpeed = blueJewelChargeSpeed;

                        jewelUI.statusJewelIconUI.sprite = jewelUI.blueJewelUI.sprite;
                        jewelUI.statusJewelIconUI.enabled = true;

                        jewelUI.statusJewelText.text = "Jewel of Spirit";
                    }
                    break;
                }
            case JewelActive.green:
                {
                    if (greenJewelPickedUp)
                    {
                        hasJewel = true;
                        jewelIcon.enabled = true;
                        jewelBar.enabled = true;
                        jewelIcon.sprite = jewelIcons[2];                       
                        jewelBar.color = Color.green;
                        redJewelEnabled = false;
                        blueJewelEnabled = false;
                        greenJewelEnabled = true;
                        yellowJewelEnabled = false;
                        purpleJewelEnabled = false;
                        jewelChargeSpeed = greenJewelChargeSpeed;

                        jewelUI.statusJewelIconUI.sprite = jewelUI.greenJewelUI.sprite;
                        jewelUI.statusJewelIconUI.enabled = true;

                        jewelUI.statusJewelText.text = "Jewel of Time";
                    }
                    break;
                }
            case JewelActive.yellow:
                {
                    if (yellowJewelPickedUp)
                    {
                        hasJewel = true;
                        jewelIcon.enabled = true;
                        jewelBar.enabled = true;
                        jewelIcon.sprite = jewelIcons[3];                      
                        jewelBar.color = Color.yellow;
                        redJewelEnabled = false;
                        blueJewelEnabled = false;
                        greenJewelEnabled = false;
                        yellowJewelEnabled = true;
                        purpleJewelEnabled = false;
                        jewelChargeSpeed = yellowJewelChargeSpeed;

                        jewelUI.statusJewelIconUI.sprite = jewelUI.yellowJewelUI.sprite;
                        jewelUI.statusJewelIconUI.enabled = true;

                        jewelUI.statusJewelText.text = "Jewel of Light";
                    }
                    break;
                }
            case JewelActive.purple:
                {
                    if (purpleJewelPickedUp)
                    {
                        hasJewel = true;
                        jewelIcon.enabled = true;
                        jewelBar.enabled = true;
                        jewelIcon.sprite = jewelIcons[4];                      
                        jewelBar.color = new Color(0.2f, 0f, 0.5f, 1f);
                        redJewelEnabled = false;
                        blueJewelEnabled = false;
                        greenJewelEnabled = false;
                        yellowJewelEnabled = false;
                        purpleJewelEnabled = true;
                        jewelChargeSpeed = purpleJewelChargeSpeed;

                        jewelUI.statusJewelIconUI.sprite = jewelUI.purpleJewelUI.sprite;
                        jewelUI.statusJewelIconUI.enabled = true;

                        jewelUI.statusJewelText.text = "Jewel of Dark";
                    }
                    break;
                }
        }
        SetJewelActive();
        SetupJewelUI();
    }
    public void JewelAlertObservers(string message)
    {
        if (message.Equals("PowerStarted"))
            initJewelPower = true;
        else if (message.Equals("PowerEnded"))
            initJewelPower = false;

    }
    public void SetupJewelUI()
    {
        if (redJewelPickedUp)
        {
            jewelUI.gameProgressJewelUIs[0].enabled = true;
            jewelUI.redJewelUI.enabled = true;
            jewelUI.redJewelButton.interactable = true;
        }
        else
        {
            jewelUI.gameProgressJewelUIs[0].enabled = false;
            jewelUI.redJewelUI.enabled = false;
            jewelUI.redJewelButton.interactable = false;
        }
        if (blueJewelPickedUp)
        {
            jewelUI.gameProgressJewelUIs[1].enabled = true;
            jewelUI.blueJewelUI.enabled = true;
            jewelUI.blueJewelButton.interactable = true;
        }
        else
        {
            jewelUI.gameProgressJewelUIs[1].enabled = false;
            jewelUI.blueJewelUI.enabled = false;
            jewelUI.blueJewelButton.interactable = false;
        }
        if (greenJewelPickedUp)
        {
            jewelUI.gameProgressJewelUIs[2].enabled = true;
            jewelUI.greenJewelUI.enabled = true;
            jewelUI.greenJewelButton.interactable = true;
        }
        else
        {
            jewelUI.gameProgressJewelUIs[2].enabled = false;
            jewelUI.greenJewelUI.enabled = false;
            jewelUI.greenJewelButton.interactable = false;
        }
        if (yellowJewelPickedUp)
        {
            jewelUI.gameProgressJewelUIs[3].enabled = true;
            jewelUI.yellowJewelUI.enabled = true;
            jewelUI.yellowJewelButton.interactable = true;
        }
        else
        {
            jewelUI.gameProgressJewelUIs[3].enabled = false;
            jewelUI.yellowJewelUI.enabled = false;
            jewelUI.yellowJewelButton.interactable = false;
        }
        if (purpleJewelPickedUp)
        {
            jewelUI.gameProgressJewelUIs[4].enabled = true;
            jewelUI.purpleJewelUI.enabled = true;
            jewelUI.purpleJewelButton.interactable = true;
        }
        else
        {
            jewelUI.gameProgressJewelUIs[4].enabled = false;
            jewelUI.purpleJewelUI.enabled = false;
            jewelUI.purpleJewelButton.interactable = false;
        }
    }
    public void JewelEngaged(int num)
    {
        if (num == 1)
        {
            isJewelEnabled = true;
            jewelStatus.SetActive(true);
            for (int pm = 0; pm < playerModels.Length; pm++)
            {
                if (pm != 0)
                    playerModels[pm].SetActive(false);
                else
                    playerModels[pm].SetActive(true);
            }
        }
        else
        {
            jewelStatus.SetActive(false);
            isJewelEnabled = false;
            for (int pm = 0; pm < playerModels.Length; pm++)
            {
                if (pm == 0)
                    playerModels[0].SetActive(false);
                else
                {
                    if(SwordSystem.UnArmed)
                        playerModels[1].SetActive(true);
                    else if(!SwordSystem.UnArmed)
                        playerModels[2].SetActive(true);
                }
            }
        }

    }
    public void GetJewel(JewelActive type)
    {
        switch (type)
        {
            case JewelActive.red:
                {
                    redJewelPickedUp = true;
                    SelectJewel(JewelActive.red);
                    break;
                }
            case JewelActive.blue:
                {
                    blueJewelPickedUp = true;
                    SelectJewel(JewelActive.blue);
                    break;
                }
            case JewelActive.green:
                {
                    greenJewelPickedUp = true;
                    SelectJewel(JewelActive.green);
                    break;
                }
            case JewelActive.yellow:
                {
                    yellowJewelPickedUp = true;
                    SelectJewel(JewelActive.yellow);
                    break;
                }
            case JewelActive.purple:
                {
                    purpleJewelPickedUp = true;
                    SelectJewel(JewelActive.purple);
                    break;
                }
               
        }
        JewelEngaged(1);
        hasJewel = true;
        SetupJewelUI();
    }
   
    public void UseJewel()
    {
        if (!isJewelActive)
        {
            if (redJewelEnabled)
                InitJewelPower(JewelActive.red);
            else if (blueJewelEnabled)
                InitJewelPower(JewelActive.blue);
            else if (greenJewelEnabled)
                InitJewelPower(JewelActive.green);
            else if (yellowJewelEnabled)
                InitJewelPower(JewelActive.yellow);
            else if (purpleJewelEnabled)
                InitJewelPower(JewelActive.purple);
        }
    }
    public void InitJewelPower(JewelActive jewel)
    {
        switch (jewel)
        {
            case JewelActive.red:
                {
                    if (jewelRedFullyCharged)
                    {
                        redJewelChargeAmt -= 100;
                        projectile = Instantiate(redJewelPowerPrefab, transform.position, Quaternion.identity) as GameObject;
                        projectile.transform.SetParent(transform);
                        Vector3 objPos = new Vector3(projectile.transform.localPosition.x, -1f, projectile.transform.localPosition.z);
                        projectile.transform.localPosition = objPos;
                        jewelRedFullyCharged = false;
                    
                        redjewelUsed = false;
                        flashJewel = true;
                    }
                    else
                        Debug.Log("Not Enough Red Jewel Power");
                    break;
                }
            case JewelActive.blue:
                {
                    if (jewelBlueFullyCharged)
                    {
                        jewelBlueFullyCharged = false;
                        isJewelActive = true;
                    }
                    else
                        Debug.Log("Not Enough Blue Jewel Power");
                    break;
                }
            case JewelActive.green:
                {
                    if (jewelGreenFullyCharged)
                    {
                        jewelGreenFullyCharged = false;
                        isJewelActive = true;
                    }
                    else
                        Debug.Log("Not Enough Green Jewel Power");
                    break;
                }
            case JewelActive.yellow:
                {
                    if (SceneSystem.overWorldEntered && !isJewelActive || TimeSystem.isDebug && !isJewelActive)
                    {
                       
                        
                        if (jewelYellowFullyCharged)
                        {
                            TimeSystem timeSystem = GameObject.Find("Core/TimeSystem/System").GetComponent<TimeSystem>();
                            yellowJewelChargeAmt -= 100;
                            jewelYellowFullyCharged = false;
                            isJewelActive = true;
                            timeSystem.SetJewelInterval(true);
                        }
                        else
                            Debug.Log("Not Enough Yellow Jewel Power");
                    }
                    break;
                }
            case JewelActive.purple:
                {
                    if (jewelPurpleFullyCharged)
                    {
                        projectile = Instantiate(purpleJewelPowerPrefab, transform.position, Quaternion.identity) as GameObject;
                        projectile.transform.SetParent(transform);
                        Vector3 objPos = new Vector3(projectile.transform.localPosition.x, -1f, projectile.transform.localPosition.z);
                        projectile.transform.localPosition = objPos;
                        jewelPurpleFullyCharged = false;
                        isJewelActive = true;
                    }
                    else
                        Debug.Log("Not Enough Purple Jewel Power");
                    break;
                }
        }
    }
    public void PlayJewelSound()
    {
        if (redJewelEnabled && jewelRedFullyCharged)
        {
            audioSrc.pitch = UnityEngine.Random.Range(0.8f, 1);
            audioSrc.volume = 1;
            audioSrc.PlayOneShot(shockwaveFx);
        }
        else if(blueJewelEnabled && jewelBlueFullyCharged)
        {
            audioSrc.pitch = UnityEngine.Random.Range(0.8f, 1);
            audioSrc.volume = 1;
            audioSrc.PlayOneShot(shockwaveFx);
        }
        else if (greenJewelEnabled && jewelGreenFullyCharged)
        {
            audioSrc.pitch = UnityEngine.Random.Range(0.8f, 1);
            audioSrc.volume = 1;
            audioSrc.PlayOneShot(shockwaveFx);
        }
        else if (yellowJewelEnabled && jewelYellowFullyCharged)
        {
            audioSrc.pitch = UnityEngine.Random.Range(0.8f, 1);
            audioSrc.volume = 1;
            audioSrc.PlayOneShot(shockwaveFx);
        }
        else if (purpleJewelEnabled && jewelPurpleFullyCharged)
        {
            audioSrc.pitch = UnityEngine.Random.Range(0.8f, 1);
            audioSrc.volume = 1;
            audioSrc.PlayOneShot(shockwaveFx);
        }

    }
    public void ChargeJewel()
    {
        if (redJewelEnabled)
        {
            if (!jewelRedFullyCharged && !isJewelActive)
            {
                if (redJewelChargeAmt < 100)
                    redJewelChargeAmt += Time.unscaledDeltaTime * jewelChargeSpeed;
                if (redJewelChargeAmt >= 100)
                {
                    var jewelREmission = jewelParticles[0].emission;
                    jewelREmission.rateOverTime = redJewelChargeAmt * particleEmissionStrength;
                    var jewelRLightRange = jewelParticles[0].lights.rangeMultiplier;
                    jewelRLightRange = redJewelChargeAmt / 16.66666666666667f;
                    redJewelChargeAmt = 100;
                    jewelRedFullyCharged = true;
                    initJewelPower = false;
                }
                else
                {
                    var jewelREmission = jewelParticles[0].emission;
                    jewelREmission.rateOverTime = 0;
                    var jewelRLightRange = jewelParticles[0].lights.rangeMultiplier;
                    jewelRLightRange = 0;
                }
            }
            jewelChargeAmt = redJewelChargeAmt;
           
            
        }
        else if (blueJewelEnabled)
        {
            if (!jewelBlueFullyCharged && !isJewelActive)
            {
                if (blueJewelChargeAmt < 100)
                    blueJewelChargeAmt += Time.unscaledDeltaTime * jewelChargeSpeed;
                if (blueJewelChargeAmt >= 100)
                {
                    var jewelBEmission = jewelParticles[1].emission;
                    jewelBEmission.rateOverTime = blueJewelChargeAmt * particleEmissionStrength;
                    var jewelBLightRange = jewelParticles[1].lights.rangeMultiplier;
                    jewelBLightRange = blueJewelChargeAmt / 16.66666666666667f; ;
                    blueJewelChargeAmt = 100;
                    initJewelPower = false;
                    jewelBlueFullyCharged = true;
                }
                else
                {
                    var jewelBEmission = jewelParticles[1].emission;
                    jewelBEmission.rateOverTime = 0;
                    var jewelBLightRange = jewelParticles[1].lights.rangeMultiplier;
                    jewelBLightRange = 0;
                }

            }
            else if (isJewelActive)
            {
                if(blueJewelChargeAmt > 0)
                    blueJewelChargeAmt -= Time.unscaledDeltaTime * blueJewelDrainSpeed;
                else if (blueJewelChargeAmt < 0)
                {
                    blueJewelChargeAmt = 0;
                    jewelBlueFullyCharged = false;
                    isJewelActive = false;
                }
            }
            jewelChargeAmt = blueJewelChargeAmt;
        }
        else if (greenJewelEnabled)
        {
            if (!jewelGreenFullyCharged && !isJewelActive)
            {
                if (greenJewelChargeAmt < 100)
                    greenJewelChargeAmt += Time.unscaledDeltaTime * jewelChargeSpeed;
                if (greenJewelChargeAmt >= 100)
                {
                    var jewelGEmission = jewelParticles[2].emission;
                    jewelGEmission.rateOverTime = greenJewelChargeAmt * particleEmissionStrength;
                    var jewelGLightRange = jewelParticles[2].lights.rangeMultiplier;
                    jewelGLightRange = greenJewelChargeAmt / 16.66666666666667f; 
                    greenJewelChargeAmt = 100;
                    initJewelPower = false;
                    jewelGreenFullyCharged = true;
                }
                else
                {
                    var jewelGEmission = jewelParticles[2].emission;
                    jewelGEmission.rateOverTime = 0;
                    var jewelGLightRange = jewelParticles[2].lights.rangeMultiplier;
                    jewelGLightRange = 0;
                }
            }
            else if (isJewelActive)
            {
                if (greenJewelChargeAmt > 0)
                {
                    greenJewelChargeAmt -= Time.unscaledDeltaTime * greenJewelDrainSpeed;
                    SlowMotion();
                }
                else if (greenJewelChargeAmt < 0)
                {
                    returnTime = true;
                    greenJewelChargeAmt = 0;
                    jewelGreenFullyCharged = false;
                    isJewelActive = false;
                }
            }
            jewelChargeAmt = greenJewelChargeAmt;
        }
        else if (yellowJewelEnabled)
        {
            if (!jewelYellowFullyCharged && !isJewelActive && SceneSystem.overWorldEntered || !jewelYellowFullyCharged && !isJewelActive && TimeSystem.isDebug)
            {
                if (yellowJewelChargeAmt < 100)
                    yellowJewelChargeAmt += Time.unscaledDeltaTime * jewelChargeSpeed;
                if (yellowJewelChargeAmt >= 100)
                {
                    yellowJewelChargeAmt = 100;
                    initJewelPower = false;
                    jewelYellowFullyCharged = true;
                    var jewelYEmission = jewelParticles[3].emission;
                    jewelYEmission.rateOverTime = yellowJewelChargeAmt * particleEmissionStrength;
                    var jewelYLightRange = jewelParticles[3].lights.rangeMultiplier;
                    jewelYLightRange = yellowJewelChargeAmt / 16.66666666666667f;
                }
                else
                {
                    var jewelYEmission = jewelParticles[3].emission;
                    jewelYEmission.rateOverTime = 0;
                    var jewelYLightRange = jewelParticles[3].lights.rangeMultiplier;
                    jewelYLightRange = 0;
                }
            }
            else if (isJewelActive && !yellowJewelUsed)
            {
                yellowJewelUsed = true;
                TimeSystem timeSys = GameObject.Find("Core/TimeSystem/System").GetComponent<TimeSystem>();
                timeSys.SetTimeSpecifics();
            }
            jewelChargeAmt = yellowJewelChargeAmt;

        }
        else if (purpleJewelEnabled)
        {
            if (!jewelPurpleFullyCharged && !isJewelActive)
            {
                if (purpleJewelChargeAmt < 100)
                    purpleJewelChargeAmt += Time.unscaledDeltaTime * jewelChargeSpeed;
                if (purpleJewelChargeAmt >= 100)
                {
                    var jewelPEmission = jewelParticles[4].emission;
                    jewelPEmission.rateOverTime = purpleJewelChargeAmt * particleEmissionStrength;
                    var jewelPLightRange = jewelParticles[4].lights.rangeMultiplier;
                    jewelPLightRange = purpleJewelChargeAmt / 16.66666666666667f;
                    purpleJewelChargeAmt = 100;
                    initJewelPower = false;
                    initJewelPower = false;
                    jewelPurpleFullyCharged = true;
                }
                else
                {
                    var jewelPEmission = jewelParticles[4].emission;
                    jewelPEmission.rateOverTime = 0;
                    var jewelPLightRange = jewelParticles[4].lights.rangeMultiplier;
                    jewelPLightRange = 0;
                }
            }
            else if (isJewelActive)
            {
                if (purpleJewelChargeAmt > 0)
                {
                    purpleJewelChargeAmt -= Time.unscaledDeltaTime * purpleJewelDrainSpeed;
                }
                else if (purpleJewelChargeAmt < 0)
                {
                    Destroy(projectile.gameObject);
                    purpleJewelChargeAmt = 0;
                    jewelPurpleFullyCharged = false;
                    isJewelActive = false;
                }
            }
            jewelChargeAmt = purpleJewelChargeAmt;
        }
    }
    public void ShutOffJewelActive()
    {
        if(yellowJewelUsed)
            yellowJewelUsed = false;
        if (greenJewelEnabled && isJewelActive)
            returnTime = true;
        isJewelActive = false;
    }
    public void ResetJewels()
    {
        hasJewel = false;
        redJewelEnabled = false;
        redJewelPickedUp = false;

        blueJewelEnabled = false;
        blueJewelPickedUp = false;

        greenJewelEnabled = false;
        greenJewelPickedUp = false;

        yellowJewelEnabled = false;
        yellowJewelPickedUp = false;

        purpleJewelEnabled = false;
        purpleJewelPickedUp = false;

        isJewelEnabled = false;
        isJewelActive = false;

        redJewelChargeAmt = 0;
        blueJewelChargeAmt = 0;
        greenJewelChargeAmt = 0;
        yellowJewelChargeAmt = 0;
        purpleJewelChargeAmt = 0;

        jewelRedFullyCharged = false;
        jewelBlueFullyCharged = false;
        jewelGreenFullyCharged = false;
        jewelYellowFullyCharged = false;
        jewelPurpleFullyCharged = false;

        SetupJewelUI();
        JewelEngaged(0);
    }
    public void LoadJewels(bool jewelEnabled, bool red, bool redEnabled, bool blue, bool blueEnabled, bool green, bool greenEnabled, bool yellow, bool yellowEnabled, bool purple, bool purpleEnabled)
    {
        isJewelEnabled = jewelEnabled;

        redJewelPickedUp = red;
        blueJewelPickedUp = blue;
        greenJewelPickedUp = green;
        yellowJewelPickedUp = yellow;
        purpleJewelPickedUp = purple;

        redJewelEnabled = redEnabled;
        blueJewelEnabled = blueEnabled;
        greenJewelEnabled = greenEnabled;
        yellowJewelEnabled = yellowEnabled;
        purpleJewelEnabled = purpleEnabled;

        redJewelChargeAmt = 0;
        blueJewelChargeAmt = 0;
        greenJewelChargeAmt = 0;
        yellowJewelChargeAmt = 0;
        purpleJewelChargeAmt = 0;

        jewelRedFullyCharged = false;
        jewelBlueFullyCharged = false;
        jewelGreenFullyCharged = false;
        jewelYellowFullyCharged = false;
        jewelPurpleFullyCharged = false;

        isJewelActive = false;

        if (redJewelEnabled)
            SelectJewel(JewelActive.red);
        else if (blueJewelEnabled)
            SelectJewel(JewelActive.blue);
        else if (greenJewelEnabled)
            SelectJewel(JewelActive.green);
        else if (yellowJewelEnabled)
            SelectJewel(JewelActive.yellow);
        else if (purpleJewelEnabled)
            SelectJewel(JewelActive.purple);
        else
            SelectJewel(JewelActive.none);

        if (jewelEnabled)
        {
            hasJewel = true;
            JewelEngaged(1);
        }
        else
            JewelEngaged(0);
    }
}
[Serializable]
public struct JewelUI
{
    public Image statusJewelIconUI;
    public Text statusJewelText;

    public Image[] gameProgressJewelUIs;

    public Image redJewelUI;
    public Image blueJewelUI;
    public Image greenJewelUI;
    public Image yellowJewelUI;
    public Image purpleJewelUI;

    public Button redJewelButton;
    public Button blueJewelButton;
    public Button greenJewelButton;
    public Button yellowJewelButton;
    public Button purpleJewelButton;
}
