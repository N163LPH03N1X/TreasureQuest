using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public enum ArmorActive { dense, helix, apex, mythic, legendary }
public enum BootActive { trail, hover, cinder, storm, medical}

public class PlayerSystem : MonoBehaviour
{
    public static Vector3 playerTransform;
    public ArmorActive armorActive;
    public BootActive bootActive;
    public HeartCases heartCases;
    public EffectCases effectSlots;
    public PlaySounds playSound;
    public ArmorUI armorUI;
    public BootUI bootUI;

    public GameObject dialogueBanner;
    public GameObject player;
    public AudioSource audioSrc;
    public AudioSource musicAudioSrc;

    public static int playerHealth = 5;
    public static int playerVitality = 5;
    public static int playerGold = 0;
    public static bool isDead = false;
    bool isGameOver;

    public Text heartText;
    public Text statusHeartText;
    public Text goldText;
    public Text statusGoldText;
    public Image damageFlash;
    public Image weakflash;
    public Image poisonFlash;
    public Image confusedFlash;
    public Image paralyzedFlash;
    public Image silenceFlash;
    public Image goldFlash;
    public Image itemInvinceFlash;
    public Image enemyDestroyFlash;
    public Image healFlash;
    public Image blackScreen;
    CharacterSystem characterCtrl;
    SwordSystem swordSyst;

    public GameObject PlayerUI;
    UnderWaterSystem underWaterSys;

    float spikeFlashTimer = 1f;
    float spikeFlashTime = 0.1f;
    float swampFlashTimer = 1f;
    float swampFlashTime = 0.1f;
    float damageFlashTimer = 0.2f;
    float damageFlashTime;
    float itemFlashTimer = 0.2f;
    float itemFlashTime;
    float enemyFlashTimer = 0.2f;
    float enemyFlashTime;
    float healFlashTimer = 0.2f;
    float healFlashTime;
    float invincibleFlashTimer = 3f;
    float invincibleFlashTime;
    float goldFlashTimer = 0.2f;
    float goldFlashTime;
    float poisonFlashTimer = 5f;
    float poisonFlashTime = 0.1f;
    int poisonCounter;
    float paralyzedTimer = 15f;
    float paralyzedTime;
    float recoveryTimer = 5f;
    float recoveryTime;

    public static bool isPoisoned;
    public static bool isConfused;
    public static bool isParalyzed;
    public static bool isSilenced;

    bool isInteraction;
    bool isGold;
    bool isItem;
    bool isHeal;
    bool isLowHealth;
    bool isPaused;
    bool isDisabled;
    bool isDamaged;
    bool isSwamped;
    bool isSpiked;
    bool isEnemyDestroyed;

    public static Vector3 lastPosition;
    public static Quaternion lastRotation;

    bool slot1Used = false;
    bool slot2Used = false;
    bool slot3Used = false;
    bool slot4Used = false;

    bool poisonActive = false;
    bool confusedActive = false;
    bool paralyzedActive = false;
    bool silencedActive = false;

    public static bool isInvincible;
    bool invincible;

    bool SetCounterPoi = false;
    bool SetCounterPar = false;
    bool SetCounterCon = false;
    bool SetCounterSil = false;

    int SlotCounter = 4;
    int assignedSlotPo;
    int assignedSlotPa;
    int assignedSlotCo;
    int assignedSlotSi;


    IEnumerator fadeAudio = null;
    IEnumerator screenBlack = null;

    public static bool denseArmorEnabled = true;
    public static bool helixArmorEnabled = false;
    public static bool apexArmorEnabled = false;
    public static bool mythicArmorEnabled = false;
    public static bool legendaryArmorEnabled = false;

    public static bool helixArmorPickedUp = false;
    public static bool apexArmorPickedUp = false;
    public static bool mythicArmorPickedUp = false;
    public static bool legendaryArmorPickedUp = false;

    public static bool trailBootEnabled = true;
    public static bool hoverBootEnabled = false;
    public static bool cinderBootEnabled = false;
    public static bool stormBootEnabled = false;
    public static bool medicalBootEnabled = false;

    public static bool hoverBootPickedUp = false;
    public static bool cinderBootPickedUp = false;
    public static bool stormBootPickedUp = false;
    public static bool medicalBootPickedUp = false;

    void Start()
    {
       
        if (SceneSystem.isNewGame)
        {
            SelectArmor(armorActive);
            SelectBoots(bootActive);
        }
        SetupArmorUI();
        SetupBootUI();
        SavePosition();
        ReEvaluateHearts();
        SetHeartHover();
    }
    void Update()
    {
        if (!isDead && !PauseGame.isPaused && !SceneSystem.isDisabled)
        {
            if (!JewelSystem.greenJewelEnabled && !JewelSystem.isJewelActive)
                Time.timeScale = 1;
        }
        
        //=================================================Timers===================================================//
        //MedicalBoots
        if (medicalBootEnabled && playerHealth < playerVitality)
            SetUpdateTimers(StatusEffects.medicals);
        //PlayerInvincible
        if (isInvincible)
            SetUpdateTimers(StatusEffects.invincible);
        //PickedUpItem
        if (isItem)
            SetUpdateTimers(StatusEffects.item);
        //KilledEnemy
        if (isEnemyDestroyed)
            SetUpdateTimers(StatusEffects.enemy);
        //PlayerHealed
        if (isHeal)
            SetUpdateTimers(StatusEffects.heal);
        //PlayerDamaged
        if (isDamaged)
            SetUpdateTimers(StatusEffects.damaged);
        //PickedUpGold
        if (isGold)
            SetUpdateTimers(StatusEffects.gold);
        //PlayerWeakend
        if (isLowHealth)
            SetUpdateTimers(StatusEffects.lowHealth);
        else if (!isLowHealth)
        {
            weakflash.enabled = false;
            playSound.lowHealthSrc.clip = null;
            playSound.lowHealthSrc.loop = false;
            playSound.lowHealthSrc.Stop();
        }
        //Poison
        if (isPoisoned)
            SetUpdateTimers(StatusEffects.poisoned);
        else if (!isPoisoned && SetCounterPoi)
        {
            poisonActive = false;
            effectSlots.poisoned.enabled = false;
            SlotCounter++;
            SlotSystem();
            SetCounterPoi = false;
        }
        //Confusion
        if (isConfused)
            SetUpdateTimers(StatusEffects.confused);
        else if (!isConfused && SetCounterCon)
        {
            confusedActive = false;
            effectSlots.confused.enabled = false;
            SlotCounter++;
            SlotSystem();
            SetCounterCon = false;
        }
        //Silenced
        if (isSilenced)
            SetUpdateTimers(StatusEffects.silenced);
        else if (!isSilenced && SetCounterSil)
        {
            silencedActive = false;
            effectSlots.silenced.enabled = false;
            SlotCounter++;
            SlotSystem();
            SetCounterSil = false;
        }
        //Paralyzed
        if (isParalyzed)
            SetUpdateTimers(StatusEffects.paralyzed);
        else if (!isParalyzed && SetCounterPar)
        {
            paralyzedActive = false;
            effectSlots.paralyzed.enabled = false;
            SlotCounter++;
            SlotSystem();
            SetCounterPar = false;
        }
        //Stepped in Swamp
        if (isSwamped && !stormBootEnabled)
            SetUpdateTimers(StatusEffects.swamped);
        //Stepped on Spikes
        if (isSpiked && !hoverBootEnabled)
            SetUpdateTimers(StatusEffects.spiked);
    }
    //================================================SlotAssignment============================================//
    public void SlotSystem()
    {
        if (SlotCounter == 1)
            slot4Used = false;
        else if (SlotCounter == 2)
        {
            slot3Used = false;
            slot4Used = false;
        }
        else if (SlotCounter == 3)
        {
            slot2Used = false;
            slot3Used = false;
            slot4Used = false;
        }
        else if (SlotCounter == 4)
        {
            slot1Used = false;
            slot2Used = false;
            slot3Used = false;
            slot4Used = false;
        }

        AssignSlot();

        if (poisonActive)
        {
            if (assignedSlotPo == 1)
                slot1Used = true;
            else if (assignedSlotPo == 2)
                slot2Used = true;
            else if (assignedSlotPo == 3)
                slot3Used = true;
            else if (assignedSlotPo == 4)
                slot4Used = true;
        }
        else if (!poisonActive)
        {
            if (assignedSlotPo == 1)
            {
                assignedSlotPo = 0;
                slot1Used = false;
            }
            else if (assignedSlotPo == 2)
            {
                assignedSlotPo = 0;
                slot2Used = false;
            }
            else if (assignedSlotPo == 3)
            {
                assignedSlotPo = 0;
                slot3Used = false;
            }
            else if (assignedSlotPo == 4)
            {
                assignedSlotPo = 0;
                slot4Used = false;
            }
        }
        if (confusedActive)
        {
            if (assignedSlotCo == 1)
                slot1Used = true;
            else if (assignedSlotCo == 2)
                slot2Used = true;
            else if (assignedSlotCo == 3)
                slot3Used = true;
            else if (assignedSlotCo == 4)
                slot4Used = true;
        }
        else if (!confusedActive)
        {
            if (assignedSlotCo == 1)
            {
                assignedSlotCo = 0;
                slot1Used = false;
            }
            else if (assignedSlotCo == 2)
            {
                assignedSlotCo = 0;
                slot2Used = false;
            }
            else if (assignedSlotCo == 3)
            {
                assignedSlotCo = 0;
                slot3Used = false;
            }
            else if (assignedSlotCo == 4)
            {
                assignedSlotCo = 0;
                slot4Used = false;
            }
        }
        if (silencedActive)
        {
            if (assignedSlotSi == 1)
                slot1Used = true;
            else if (assignedSlotSi == 2)
                slot2Used = true;
            else if (assignedSlotSi == 3)
                slot3Used = true;
            else if (assignedSlotSi == 4)
                slot4Used = true;
        }
        else if (!silencedActive)
        {
            if (assignedSlotSi == 1)
            {
                assignedSlotSi = 0;
                slot1Used = false;
            }
            else if (assignedSlotSi == 2)
            {
                assignedSlotSi = 0;
                slot2Used = false;
            }
            else if (assignedSlotSi == 3)
            {
                assignedSlotSi = 0;
                slot3Used = false;
            }
            else if (assignedSlotSi == 4)
            {
                assignedSlotSi = 0;
                slot4Used = false;
            }
        }
        if (paralyzedActive)
        {
            if (assignedSlotPa == 1)
                slot1Used = true;
            else if (assignedSlotPa == 2)
                slot2Used = true;
            else if (assignedSlotPa == 3)
                slot3Used = true;
            else if (assignedSlotPa == 4)
                slot4Used = true;
        }
        else if (!paralyzedActive)
        {
            if (assignedSlotPa == 1)
            {
                assignedSlotPa = 0;
                slot1Used = false;
            }
            else if (assignedSlotPa == 2)
            {
                assignedSlotPa = 0;
                slot2Used = false;
            }
            else if (assignedSlotPa == 3)
            {
                assignedSlotPa = 0;
                slot3Used = false;
            }
            else if (assignedSlotPa == 4)
            {
                assignedSlotPa = 0;
                slot4Used = false;
            }
        }
    }
    public void AssignSlot()
    {
        //===========================================Slot1==========================================//
        if (!slot1Used)
        {
            //PoisonedOn
            if (isPoisoned && !poisonActive)
            {

                poisonActive = true;
                assignedSlotPo = 1;
                effectSlots.poisoned.GetComponent<RectTransform>().anchoredPosition = effectSlots.Slot1.GetComponent<RectTransform>().anchoredPosition;
                effectSlots.poisoned.enabled = true;
            }
            //ParalyzedOn
            else if (isParalyzed && !paralyzedActive)
            {
                paralyzedActive = true;
                assignedSlotPa = 1;
                effectSlots.paralyzed.GetComponent<RectTransform>().anchoredPosition = effectSlots.Slot1.GetComponent<RectTransform>().anchoredPosition;
                effectSlots.paralyzed.enabled = true;
            }
            //ConfusedOn
            else if (isConfused && !confusedActive)
            {
                confusedActive = true;
                assignedSlotCo = 1;
                effectSlots.confused.GetComponent<RectTransform>().anchoredPosition = effectSlots.Slot1.GetComponent<RectTransform>().anchoredPosition;
                effectSlots.confused.enabled = true;
            }
            //SilencedOn
            else if (isSilenced && !silencedActive)
            {
                silencedActive = true;
                assignedSlotSi = 1;
                effectSlots.silenced.GetComponent<RectTransform>().anchoredPosition = effectSlots.Slot1.GetComponent<RectTransform>().anchoredPosition;
                effectSlots.silenced.enabled = true;
            }
        }
        //===========================================Slot2==========================================//
        if (!slot2Used)
        {
            //PoisonedOn
            if (isPoisoned && !poisonActive)
            {
                poisonActive = true;
                assignedSlotPo = 2;
                effectSlots.poisoned.GetComponent<RectTransform>().anchoredPosition = effectSlots.Slot2.GetComponent<RectTransform>().anchoredPosition;
                effectSlots.poisoned.enabled = true;
            }
            //ParalyzedOn
            else if (isParalyzed && !paralyzedActive)
            {
                paralyzedActive = true;
                assignedSlotPa = 2;
                effectSlots.paralyzed.GetComponent<RectTransform>().anchoredPosition = effectSlots.Slot2.GetComponent<RectTransform>().anchoredPosition;
                effectSlots.paralyzed.enabled = true;
            }
            //ConfusedOn
            else if (isConfused && !confusedActive)
            {
                confusedActive = true;
                assignedSlotCo = 2;
                effectSlots.confused.GetComponent<RectTransform>().anchoredPosition = effectSlots.Slot2.GetComponent<RectTransform>().anchoredPosition;
                effectSlots.confused.enabled = true;
            }
            //SilencedOn
            else if (isSilenced && !silencedActive)
            {
                silencedActive = true;
                assignedSlotSi = 2;
                effectSlots.silenced.GetComponent<RectTransform>().anchoredPosition = effectSlots.Slot2.GetComponent<RectTransform>().anchoredPosition;
                effectSlots.silenced.enabled = true;
            }
        }

        //===========================================Slot3==========================================//
        if (!slot3Used)
        {
            //PoisonedOn
            if (isPoisoned && !poisonActive)
            {
                poisonActive = true;
                assignedSlotPo = 3;
                effectSlots.poisoned.GetComponent<RectTransform>().anchoredPosition = effectSlots.Slot3.GetComponent<RectTransform>().anchoredPosition;
                effectSlots.poisoned.enabled = true;
            }
            //ParalyzedOn
            else if (isParalyzed && !paralyzedActive)
            {
                paralyzedActive = true;
                assignedSlotPa = 3;
                effectSlots.paralyzed.GetComponent<RectTransform>().anchoredPosition = effectSlots.Slot3.GetComponent<RectTransform>().anchoredPosition;
                effectSlots.paralyzed.enabled = true;
            }
            //ConfusedOn
            else if (isConfused && !confusedActive)
            {
                confusedActive = true;
                assignedSlotCo = 3;
                effectSlots.confused.GetComponent<RectTransform>().anchoredPosition = effectSlots.Slot3.GetComponent<RectTransform>().anchoredPosition;
                effectSlots.confused.enabled = true;
            }
            //SilencedOn
            else if (isSilenced && !silencedActive)
            {
                silencedActive = true;
                assignedSlotSi = 3;
                effectSlots.silenced.GetComponent<RectTransform>().anchoredPosition = effectSlots.Slot3.GetComponent<RectTransform>().anchoredPosition;
                effectSlots.silenced.enabled = true;
            }
        }
        //===========================================Slot4==========================================//
        if (!slot4Used)
        {
            //PoisonedOn
            if (isPoisoned && !poisonActive)
            {
                poisonActive = true;
                assignedSlotPo = 4;
                effectSlots.poisoned.GetComponent<RectTransform>().anchoredPosition = effectSlots.Slot4.GetComponent<RectTransform>().anchoredPosition;
                effectSlots.poisoned.enabled = true;
            }
            //ParalyzedOn
            else if (isParalyzed && !paralyzedActive)
            {
                paralyzedActive = true;
                assignedSlotPa = 4;
                effectSlots.paralyzed.GetComponent<RectTransform>().anchoredPosition = effectSlots.Slot4.GetComponent<RectTransform>().anchoredPosition;
                effectSlots.paralyzed.enabled = true;
            }
            //ConfusedOn
            else if (isConfused && !confusedActive)
            {
                confusedActive = true;
                assignedSlotCo = 4;
                effectSlots.confused.GetComponent<RectTransform>().anchoredPosition = effectSlots.Slot4.GetComponent<RectTransform>().anchoredPosition;
                effectSlots.confused.enabled = true;
            }
            //SilencedOn
            else if (isSilenced && !silencedActive)
            {
                silencedActive = true;
                assignedSlotSi = 4;
                effectSlots.silenced.GetComponent<RectTransform>().anchoredPosition = effectSlots.Slot4.GetComponent<RectTransform>().anchoredPosition;
                effectSlots.silenced.enabled = true;
            }
        }
    }
    public void TurnOffSpikeDamage()
    {
        isSpiked = false;
    }
    public enum StatusEffects { enemy, heal, item, gold, poisoned, silenced, paralyzed, confused, damaged, lowHealth, invincible, swamped, spiked, medicals }
    public void SetUpdateTimers(StatusEffects effects)
    {
        switch (effects)
        {
            case StatusEffects.medicals:
                {
                    if (CharacterSystem.isMoving)
                    {
                        if (recoveryTime > 0)
                            recoveryTime -= Time.deltaTime;
                        else if (recoveryTime < 0)
                        {
                            PlayerRecovery(1);
                            recoveryTime = recoveryTimer;
                        }
                    }
                    break;
                }
            case StatusEffects.lowHealth:
                {
                    if (playerHealth <= 3)
                    {
                        weakflash.enabled = true;

                        if (playerHealth <= 1)
                        {
                            playSound.lowHealthSrc.pitch = 1.5f;
                            weakflash.color = new Color(weakflash.color.r, weakflash.color.g, weakflash.color.b, Mathf.PingPong(Time.unscaledTime * 10, 1));
                        }
                        else
                        {
                            playSound.lowHealthSrc.pitch = 1f;
                            weakflash.color = new Color(weakflash.color.r, weakflash.color.g, weakflash.color.b, Mathf.PingPong(Time.unscaledTime, 1));
                        }
                    }
                    else if (playerHealth > 3)
                    {
                        weakflash.enabled = false;
                        isLowHealth = false;
                    }
                    break;
                }
            case StatusEffects.damaged:
                {
                    if (damageFlashTime > 0)
                    {
                        damageFlash.enabled = true;
                        damageFlashTime -= Time.deltaTime;
                    }
                    else if (damageFlashTime < 0)
                    {
                        damageFlash.enabled = false;
                        isDamaged = false;
                        damageFlashTime = damageFlashTimer;
                    }
                    break;
                }
            case StatusEffects.gold:
                {
                    if (goldFlashTime > 0)
                    {
                        goldFlash.enabled = true;
                        goldFlashTime -= Time.deltaTime;
                    }
                    if (goldFlashTime < 0)
                    {
                        goldFlashTime = goldFlashTimer;
                        isGold = false;
                        goldFlash.enabled = false;
                    }
                    break;
                }
            case StatusEffects.swamped:
                {

                    if (swampFlashTime > 0)
                        swampFlashTime -= Time.deltaTime;
                    else if (swampFlashTime < 0)
                    {
                        PlayerDamage(1, false);
                        swampFlashTime = swampFlashTimer;
                    }
                    break;
                }
            case StatusEffects.spiked:
                {

                    if (spikeFlashTime > 0)
                        spikeFlashTime -= Time.deltaTime;
                    else if (spikeFlashTime < 0)
                    {
                        PlayerDamage(1, false);
                        spikeFlashTime = spikeFlashTimer;
                    }
                    break;
                }
            case StatusEffects.poisoned:
                {
                    if (!SetCounterPoi)
                    {
                        SlotCounter--;
                        SlotSystem();
                        SetCounterPoi = true;
                    }
                    if (poisonCounter > 0)
                    {
                        if (poisonFlashTime > 0)
                        {
                            poisonFlash.enabled = false;
                            poisonFlashTime -= Time.deltaTime;
                        }
                        else if (poisonFlashTime < 0)
                        {
                            poisonFlash.enabled = true;
                            poisonCounter--;
                            PlayerDamage(1, true);
                            poisonFlashTime = poisonFlashTimer;
                            if (poisonCounter <= 0)
                                ApplyPlayerEffect(PlayerEffectStats.poisonMax, false);
                        }
                    }
                    break;
                }
            case StatusEffects.confused:
                {
                    if (!SetCounterCon)
                    {
                        SlotCounter--;
                        SlotSystem();
                        confusedFlash.enabled = true;
                        SetCounterCon = true;
                    }
                    break;
                }
            case StatusEffects.silenced:
                {
                    if (!SetCounterSil)
                    {
                        SlotCounter--;
                        SlotSystem();
                        silenceFlash.enabled = true;
                        SetCounterSil = true;
                    }
                    break;
                }
            case StatusEffects.paralyzed:
                {
                    if (!SetCounterPar)
                    {
                        SlotCounter--;
                        SlotSystem();
                        paralyzedFlash.enabled = true;
                        SetCounterPar = true;
                    }
                    if (paralyzedTime > 0)
                        paralyzedTime -= Time.deltaTime;
                    else if (paralyzedTime < 0)
                    {
                        PlayerDamage(500, false);
                        ApplyPlayerEffect(PlayerEffectStats.paralyzed, false);
                    }
                    break;
                }
            case StatusEffects.invincible:
                {
                    if (invincibleFlashTime > 0)
                    {
                        Physics.IgnoreLayerCollision(1, 9);
                        invincibleFlashTime -= Time.deltaTime;
                        itemInvinceFlash.enabled = true;
                        itemInvinceFlash.color = new Color(itemInvinceFlash.color.r, itemInvinceFlash.color.g, itemInvinceFlash.color.b, Mathf.PingPong(Time.time, 1));
                    }
                    else if (invincibleFlashTime < 0)
                    {
                        isInvincible = false;
                        itemInvinceFlash.enabled = false;
                        invincibleFlashTime = invincibleFlashTimer;
                    }
                    break;
                }
            case StatusEffects.item:
                {
                    if (itemFlashTime > 0)
                    {
                        itemInvinceFlash.enabled = true;
                        itemFlashTime -= Time.deltaTime;
                    }
                    else if (itemFlashTime < 0)
                    {
                        itemInvinceFlash.enabled = false;
                        isItem = false;
                        itemFlashTime = itemFlashTimer;
                    }
                    break;
                }
            case StatusEffects.enemy:
                {
                    if (enemyFlashTime > 0)
                    {
                        enemyDestroyFlash.enabled = true;
                        enemyFlashTime -= Time.deltaTime;
                    }
                    else if (enemyFlashTime < 0)
                    {
                        enemyDestroyFlash.enabled = false;
                        isEnemyDestroyed = false;
                        enemyFlashTime = enemyFlashTimer;
                    }
                    break;
                }
            case StatusEffects.heal:
                {
                    if (healFlashTime > 0)
                    {
                        healFlash.enabled = true;
                        healFlashTime -= Time.deltaTime;
                    }
                    else if (healFlashTime < 0)
                    {
                        healFlash.enabled = false;
                        isHeal = false;
                        healFlashTime = healFlashTimer;
                    }
                    break;
                }
        }
    }
    //=================================================PlayerHearts=============================================//
    public void PlayerDamage(int amount, bool poisoned)
    {
        //DialogueSystem dialogueSystem = GetComponent<DialogueSystem>();
        //dialogueSystem

        if (!isInvincible && !invincible && !isDead)
        {

            if (!poisoned)
            {
                damageFlashTime = damageFlashTimer;
                isDamaged = true;
            }
            if (helixArmorEnabled)
            {
                amount = Mathf.RoundToInt(amount / 2);
                if (amount < 1)
                    amount = 1;
                playerHealth -= amount;
            }
            else if (apexArmorEnabled)
            {
                amount = Mathf.RoundToInt(amount / 3);
                if (amount < 1)
                    amount = 1;
                playerHealth -= amount;
            }
            else if (mythicArmorEnabled)
            {
                amount = Mathf.RoundToInt(amount / 4);
                if (amount < 1)
                    amount = 1;
                playerHealth -= amount;
            }
            else if (legendaryArmorEnabled)
            {
                amount = Mathf.RoundToInt(amount / 5);
                if (amount < 1)
                    amount = 1;
                playerHealth -= amount;
            }
            else if (denseArmorEnabled)
                playerHealth -= amount;

            audioSrc.pitch = UnityEngine.Random.Range(0.8f, 1);
            AudioClip clipStore = null;
            int randomNum = UnityEngine.Random.Range(1, 4);
            if (randomNum == 1)
                clipStore = playSound.hurt[0];
            if (randomNum == 2)
                clipStore = playSound.hurt[1];
            if (randomNum == 3)
                clipStore = playSound.hurt[2];
            if (randomNum == 4)
                clipStore = playSound.hurt[3];

            audioSrc.PlayOneShot(clipStore);

            if (playerHealth <= 3 && playerHealth > 0 && !isLowHealth)
            {
                playSound.lowHealthSrc.clip = playSound.healthLow;
                playSound.lowHealthSrc.loop = true;
                if (SceneSystem.inTransition)
                    playSound.lowHealthSrc.mute = true;
                else
                    playSound.lowHealthSrc.mute = false;
                playSound.lowHealthSrc.Play();
                isLowHealth = true;
            }
            else if (playerHealth < 1)
            {
                underWaterSys = GameObject.FindGameObjectWithTag("Head").GetComponent<UnderWaterSystem>();
                underWaterSys.AirBannerActive(false);
                dialogueBanner.SetActive(false);
                playerHealth = 0;
                if (ItemSystem.lifeBerryEmpty)
                {
                    PlayerUI.SetActive(false);
                    if (fadeAudio != null)
                        StopCoroutine(fadeAudio);
                    musicAudioSrc.Stop();
                }
                else if (!ItemSystem.lifeBerryEmpty)
                {
                    if (fadeAudio != null)
                        StopCoroutine(fadeAudio);
                    fadeAudio = AudioFadeOut(musicAudioSrc);
                    StartCoroutine(fadeAudio);
                }
                isDamaged = false;
                damageFlashTime = damageFlashTimer;
                isLowHealth = false;
                if (screenBlack != null)
                    StopCoroutine(screenBlack);
                screenBlack = FadeIn(0, .3f, blackScreen, true);
                StartCoroutine(screenBlack);
                SlotCounter = 4;
                damageFlash.enabled = false;
                weakflash.enabled = false;
                healFlash.enabled = false;
                goldFlash.enabled = false;
                itemInvinceFlash.enabled = false;
                ApplyPlayerEffect(PlayerEffectStats.poisonMin, false);
                ApplyPlayerEffect(PlayerEffectStats.poisonMax, false);
                ApplyPlayerEffect(PlayerEffectStats.confuse, false);
                ApplyPlayerEffect(PlayerEffectStats.paralyzed, false);
                ApplyPlayerEffect(PlayerEffectStats.silence, false);
                audioSrc.pitch = UnityEngine.Random.Range(0.8f, 1);
                audioSrc.PlayOneShot(playSound.death);
                GameObject enemyUIObjs = GameObject.Find("Core/Player/PopUpCanvas/EnemyUIStorage");
                foreach (Transform child in enemyUIObjs.transform)
                {
                    child.gameObject.SetActive(false);
                }
                DialogueSystem diagSys = GetComponent<DialogueSystem>();
                diagSys.SetButtonPress(3);
                PlayerDied(true);
            }
            SetHealthHeartRatio();
            ApplyHealthOrVitality();
        }
    }
    public void PlayerDied(bool active)
    {

            isDead = active;
        if(isDead)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
    public void PlayerRevived()
    {
        GameObject enemyUIObjs = GameObject.Find("Core/Player/PopUpCanvas/EnemyUIStorage");
        foreach (Transform child in enemyUIObjs.transform)
            child.gameObject.SetActive(true);
        audioSrc.volume = 1;
        audioSrc.pitch = 0.8f;
        audioSrc.PlayOneShot(playSound.revive);
        if (screenBlack != null)
            StopCoroutine(screenBlack);
        screenBlack = FadeOut(0, 0.3f, blackScreen, true);
        StartCoroutine(screenBlack);
        healFlashTime = healFlashTimer;
        isHeal = true;
        isDead = false;
        invincibleFlashTime = invincibleFlashTimer;
        isInvincible = true;
        SceneSystem sceneSystem = transform.parent.GetComponent<SceneSystem>();
        sceneSystem.EnablePlayer(true);
        if (fadeAudio != null)
            StopCoroutine(fadeAudio);
        fadeAudio = AudioFadeIn(musicAudioSrc);
        StartCoroutine(fadeAudio);
        if (UnderWaterSystem.isSwimming)
        {
            underWaterSys = GameObject.FindGameObjectWithTag("Head").GetComponent<UnderWaterSystem>();
            underWaterSys.AirBannerActive(true);
        }

    }
    public void PlayerInvincible(bool active)
    {
        invincible = active;
    }
    public void PlayerRecovery(int amount)
    {
        healFlashTime = healFlashTimer;
        if(!ShopSystem.isSleeping)
            isHeal = true;
        playerHealth += amount;
        if (playerHealth >= playerVitality)
            playerHealth = playerVitality;
        SetHeartHover();
        SetHealthHeartRatio();
        ApplyHealthOrVitality();
    }
    public void AddVitality(int amount)
    {
        bool healPlayer = true;

        itemFlashTime = itemFlashTimer;
        isItem = true;
        playerVitality += amount;
        if (playerVitality > 100)
        {
            playerVitality = 100;
            healPlayer = false;
        }
        else if (playerVitality <= 100 && healPlayer)
        {
            ReEvaluateHearts();
            PlayerRecovery(1);
        }
        SetHealthHeartRatio();
        ApplyHealthOrVitality();
        SetHeartHover();
    }
    public void RemoveVitality(int amount)
    {
        bool healPlayer = true;
        if (playerVitality <= 5)
        {
            playerHealth = 5;
            playerHealth = 5;
            healPlayer = false;
        }
        else
            playerVitality -= amount;

        if (playerHealth > playerVitality)
            playerHealth = playerVitality;
        if (playerVitality >= 5 && healPlayer)
            ReEvaluateHearts();
        SetHealthHeartRatio();
        ApplyHealthOrVitality();
    }
    public void ApplyHealthOrVitality()
    {
        heartText.text = playerHealth + "/" + playerVitality.ToString();
        statusHeartText.text = playerHealth + "/" + playerVitality.ToString();
    }
    public enum Heart { six, seven, eight, nine, ten, eleven, twentyone, thirtyone, fourtyone, fiftyone }
    public void EnableHeartsSystem(Heart count, bool True)
    {
        if (True)
        {
            switch (count)
            {
                case Heart.six:
                    {
                        heartCases.enableSHeart6.SetActive(true);
                        heartCases.enableStatusSHeart6.SetActive(true);
                        break;
                    }
                case Heart.seven:
                    {
                        heartCases.enableSHeart7.SetActive(true);
                        heartCases.enableStatusSHeart7.SetActive(true);
                        break;
                    }
                case Heart.eight:
                    {
                        heartCases.enableSHeart8.SetActive(true);
                        heartCases.enableStatusSHeart8.SetActive(true);
                        break;
                    }
                case Heart.nine:
                    {
                        heartCases.enableSHeart9.SetActive(true);
                        heartCases.enableStatusSHeart9.SetActive(true);
                        break;
                    }
                case Heart.ten:
                    {
                        heartCases.enableSHeart10.SetActive(true);
                        heartCases.enableStatusSHeart10.SetActive(true);
                        break;
                    }
                case Heart.eleven:
                    {
                        heartCases.enableMHeart1.SetActive(true);
                        heartCases.enableStatusMHeart1.SetActive(true);
                        break;
                    }
                case Heart.twentyone:
                    {
                        heartCases.enableMHeart2.SetActive(true);
                        heartCases.enableStatusMHeart2.SetActive(true);
                        break;
                    }
                case Heart.thirtyone:
                    {
                        heartCases.enableMHeart3.SetActive(true);
                        heartCases.enableStatusMHeart3.SetActive(true);
                        break;
                    }
                case Heart.fourtyone:
                    {
                        heartCases.enableMHeart4.SetActive(true);
                        heartCases.enableStatusMHeart4.SetActive(true);
                        break;
                    }
                case Heart.fiftyone:
                    {
                        heartCases.enableLHeart1.SetActive(true);
                        heartCases.enableStatusLHeart1.SetActive(true);
                        break;
                    }
            }
        }
        else
        {
            switch (count)
            {
                case Heart.six:
                    {
                        heartCases.enableSHeart6.SetActive(false);
                        heartCases.enableStatusSHeart6.SetActive(false);
                        break;
                    }
                case Heart.seven:
                    {
                        heartCases.enableSHeart7.SetActive(false);
                        heartCases.enableStatusSHeart7.SetActive(false);
                        break;
                    }
                case Heart.eight:
                    {
                        heartCases.enableSHeart8.SetActive(false);
                        heartCases.enableStatusSHeart8.SetActive(false);
                        break;
                    }
                case Heart.nine:
                    {
                        heartCases.enableSHeart9.SetActive(false);
                        heartCases.enableStatusSHeart9.SetActive(false);
                        break;
                    }
                case Heart.ten:
                    {
                        heartCases.enableSHeart10.SetActive(false);
                        heartCases.enableStatusSHeart10.SetActive(false);
                        break;
                    }
                case Heart.eleven:
                    {
                        heartCases.enableMHeart1.SetActive(false);
                        heartCases.enableStatusMHeart1.SetActive(false);
                        break;
                    }
                case Heart.twentyone:
                    {
                        heartCases.enableMHeart2.SetActive(false);
                        heartCases.enableStatusMHeart2.SetActive(false);
                        break;
                    }
                case Heart.thirtyone:
                    {
                        heartCases.enableMHeart3.SetActive(false);
                        heartCases.enableStatusMHeart3.SetActive(false);
                        break;
                    }
                case Heart.fourtyone:
                    {
                        heartCases.enableMHeart4.SetActive(false);
                        heartCases.enableStatusMHeart4.SetActive(false);
                        break;
                    }
                case Heart.fiftyone:
                    {
                        heartCases.enableLHeart1.SetActive(false);
                        heartCases.enableStatusLHeart1.SetActive(false);
                        break;
                    }
            }
        }
    }
    public void ReEvaluateHearts()
    {
        if (playerVitality < 6)
            EnableHeartsSystem(Heart.six, false);
        else if (playerVitality >= 6)
            EnableHeartsSystem(Heart.six, true);

        if (playerVitality < 7)
            EnableHeartsSystem(Heart.seven, false);
        else if (playerVitality >= 7)
            EnableHeartsSystem(Heart.seven, true);

        if (playerVitality < 8)
            EnableHeartsSystem(Heart.eight, false);
        else if (playerVitality >= 8)
            EnableHeartsSystem(Heart.eight, true);

        if (playerVitality < 9)
            EnableHeartsSystem(Heart.nine, false);
        else if (playerVitality >= 9)
            EnableHeartsSystem(Heart.nine, true);

        if (playerVitality < 10)
            EnableHeartsSystem(Heart.ten, false);
        else if (playerVitality >= 10)
            EnableHeartsSystem(Heart.ten, true);

        if (playerVitality < 11)
            EnableHeartsSystem(Heart.eleven, false);
        else if (playerVitality >= 11)
            EnableHeartsSystem(Heart.eleven, true);

        if (playerVitality < 21)
            EnableHeartsSystem(Heart.twentyone, false);
        else if (playerVitality >= 21)
            EnableHeartsSystem(Heart.twentyone, true);

        if (playerVitality < 31)
            EnableHeartsSystem(Heart.thirtyone, false);
        else if (playerVitality >= 31)
            EnableHeartsSystem(Heart.thirtyone, true);

        if (playerVitality < 41)
            EnableHeartsSystem(Heart.fourtyone, false);
        else if (playerVitality >= 41)
            EnableHeartsSystem(Heart.fourtyone, true);

        if (playerVitality < 51)
            EnableHeartsSystem(Heart.fiftyone, false);
        else if (playerVitality >= 51)
            EnableHeartsSystem(Heart.fiftyone, true);
    }
    public void SetHealthHeartRatio()
    {
        int[] tenHearts = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        int[] twentyHearts = { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
        int[] thirtyHearts = { 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
        int[] fourtyHearts = { 31, 32, 33, 34, 35, 36, 37, 38, 39, 40 };
        int[] fiftyHearts = { 41, 42, 43, 44, 45, 46, 47, 48, 49, 50 };
        int[] sixtyHearts = { 51, 52, 53, 54, 55, 56, 57, 58, 59, 60 };
        int[] seventyHearts = { 61, 62, 63, 64, 65, 66, 67, 68, 69, 70 };
        int[] eightyHearts = { 71, 72, 73, 74, 75, 76, 77, 78, 79, 80 };
        int[] ninetyHearts = { 81, 82, 83, 84, 85, 86, 87, 88, 89, 90 };
        int[] hundredHearts = { 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };

        for (int h = 0; h < 1; h++)
        {
            for (int th = 0; th < tenHearts.Length; th++)
            {
                if (tenHearts[th] == playerHealth)
                {
                    if (playerHealth == 0)
                    {
                        //Player//
                        heartCases.sHeart1.enabled = false;
                        heartCases.sHeart2.enabled = false;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;
                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;
                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = false;
                        heartCases.statusSHeart2.enabled = false;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;
                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;
                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 1)
                    {
                        //Player//
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = false;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;
                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = false;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;
                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;
                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 2)
                    {
                        //Player//
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;
                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;
                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;
                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;
                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 3)
                    {
                        //Player//
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;
                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;
                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;
                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;
                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 4)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;
                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;
                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;
                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;
                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 5)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;
                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;
                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;
                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;
                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 6)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;
                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;
                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;
                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;
                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 7)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;
                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;
                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;
                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;
                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 8)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;
                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;
                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;
                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;
                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 9)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = false;
                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;
                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = false;
                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;
                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 10)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = true;
                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;
                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = true;
                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;
                        heartCases.statusLHeart1.enabled = false;
                    }
                }
            }
            for (int th = 0; th < twentyHearts.Length; th++)
            {
                if (twentyHearts[th] == playerHealth)
                {
                    if (playerHealth == 11)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = false;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = false;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 12)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 13)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 14)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 15)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 16)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 17)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 18)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 19)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 20)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = true;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = true;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                }
            }
            for (int th = 0; th < thirtyHearts.Length; th++)
            {
                if (thirtyHearts[th] == playerHealth)
                {
                    if (playerHealth == 21)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = false;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = false;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 22)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 23)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 24)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 25)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 26)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 27)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 28)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 29)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 30)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = true;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = true;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                }
            }
            for (int fh = 0; fh < fourtyHearts.Length; fh++)
            {
                if (fourtyHearts[fh] == playerHealth)
                {
                    if (playerHealth == 31)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = false;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = false;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 32)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 33)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }

                    else if (playerHealth == 34)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 35)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 36)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 37)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 38)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 39)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 40)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = true;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = true;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = false;
                    }
                }
            }
            for (int fh = 0; fh < fiftyHearts.Length; fh++)
            {
                if (fiftyHearts[fh] == playerHealth)
                {
                    if (playerHealth == 41)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = false;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = false;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 42)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 43)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 44)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 45)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 46)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 47)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 48)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 49)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = false;
                    }
                    else if (playerHealth == 50)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = true;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = false;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = true;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = false;
                    }
                }
            }
            for (int sh = 0; sh < sixtyHearts.Length; sh++)
            {
                if (sixtyHearts[sh] == playerHealth)
                {
                    if (playerHealth == 51)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = false;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = false;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 52)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 53)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 54)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 55)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 56)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 57)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 58)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 59)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 60)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = true;

                        heartCases.mHeart1.enabled = false;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = true;

                        heartCases.statusMHeart1.enabled = false;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                }
            }
            for (int sh = 0; sh < seventyHearts.Length; sh++)
            {
                if (seventyHearts[sh] == playerHealth)
                {
                    if (playerHealth == 61)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = false;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = false;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 62)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 63)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 64)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 65)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 66)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 67)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 68)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 69)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 70)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = true;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = false;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = true;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = false;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                }
            }
            for (int eh = 0; eh < eightyHearts.Length; eh++)
            {
                if (eightyHearts[eh] == playerHealth)
                {
                    if (playerHealth == 71)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = false;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = false;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 72)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 73)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 74)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 75)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 76)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 77)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 78)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 79)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 80)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = true;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = false;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = true;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = false;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                }
            }
            for (int nh = 0; nh < ninetyHearts.Length; nh++)
            {
                if (ninetyHearts[nh] == playerHealth)
                {
                    if (playerHealth == 81)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = false;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = false;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 82)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 83)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 84)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 85)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 86)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 87)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 88)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 89)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 90)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = true;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = false;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = true;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = false;

                        heartCases.statusLHeart1.enabled = true;
                    }
                }
            }
            for (int huh = 0; huh < hundredHearts.Length; huh++)
            {
                if (hundredHearts[huh] == playerHealth)
                {
                    if (playerHealth == 91)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = false;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = false;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 92)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = false;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = false;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 93)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = false;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = false;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 94)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = false;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = false;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 95)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = false;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = false;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 96)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = false;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = false;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 97)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = false;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = false;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 98)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = false;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = false;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 99)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = false;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = false;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = true;
                    }
                    else if (playerHealth == 100)
                    {
                        heartCases.sHeart1.enabled = true;
                        heartCases.sHeart2.enabled = true;
                        heartCases.sHeart3.enabled = true;
                        heartCases.sHeart4.enabled = true;
                        heartCases.sHeart5.enabled = true;
                        heartCases.sHeart6.enabled = true;
                        heartCases.sHeart7.enabled = true;
                        heartCases.sHeart8.enabled = true;
                        heartCases.sHeart9.enabled = true;
                        heartCases.sHeart10.enabled = true;

                        heartCases.mHeart1.enabled = true;
                        heartCases.mHeart2.enabled = true;
                        heartCases.mHeart3.enabled = true;
                        heartCases.mHeart4.enabled = true;

                        heartCases.lHeart1.enabled = true;

                        //Status//
                        heartCases.statusSHeart1.enabled = true;
                        heartCases.statusSHeart2.enabled = true;
                        heartCases.statusSHeart3.enabled = true;
                        heartCases.statusSHeart4.enabled = true;
                        heartCases.statusSHeart5.enabled = true;
                        heartCases.statusSHeart6.enabled = true;
                        heartCases.statusSHeart7.enabled = true;
                        heartCases.statusSHeart8.enabled = true;
                        heartCases.statusSHeart9.enabled = true;
                        heartCases.statusSHeart10.enabled = true;

                        heartCases.statusMHeart1.enabled = true;
                        heartCases.statusMHeart2.enabled = true;
                        heartCases.statusMHeart3.enabled = true;
                        heartCases.statusMHeart4.enabled = true;

                        heartCases.statusLHeart1.enabled = true;
                    }
                }
            }
        }
        SetHeartHover();
    }
    public void SetHeartHover()
    {
        int[] first = { 1, 11, 21, 31, 41, 51, 61, 71, 81, 91 };
        int[] second = { 2, 12, 22, 32, 42, 52, 62, 72, 82, 92 };
        int[] third = { 3, 13, 23, 33, 43, 53, 63, 73, 83, 93 };
        int[] fourth = { 4, 14, 24, 34, 44, 54, 64, 74, 84, 94 };
        int[] fifth = { 5, 15, 25, 35, 45, 55, 65, 75, 85, 95 };
        int[] sixth = { 6, 16, 26, 36, 46, 56, 66, 76, 86, 96 };
        int[] seventh = { 7, 17, 27, 37, 47, 57, 67, 77, 87, 97 };
        int[] eighth = { 8, 18, 28, 38, 48, 58, 68, 78, 88, 98 };
        int[] nineth = { 9, 19, 29, 39, 49, 59, 69, 79, 89, 99 };
        int[] tenth = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

        UIOnHover hoverHeart1 = heartCases.enableSHeartHover1.GetComponent<UIOnHover>();
        UIOnHover hoverHeart2 = heartCases.enableSHeartHover2.GetComponent<UIOnHover>();
        UIOnHover hoverHeart3 = heartCases.enableSHeartHover3.GetComponent<UIOnHover>();
        UIOnHover hoverHeart4 = heartCases.enableSHeartHover4.GetComponent<UIOnHover>();
        UIOnHover hoverHeart5 = heartCases.enableSHeartHover5.GetComponent<UIOnHover>();
        UIOnHover hoverHeart6 = heartCases.enableSHeartHover6.GetComponent<UIOnHover>();
        UIOnHover hoverHeart7 = heartCases.enableSHeartHover7.GetComponent<UIOnHover>();
        UIOnHover hoverHeart8 = heartCases.enableSHeartHover8.GetComponent<UIOnHover>();
        UIOnHover hoverHeart9 = heartCases.enableSHeartHover9.GetComponent<UIOnHover>();
        UIOnHover hoverHeart10 = heartCases.enableSHeartHover10.GetComponent<UIOnHover>();
        for (int h = 0; h < 1; h++)
        {
            for (int hf = 0; hf < first.Length; hf++)
            {
                if (first[hf] == playerHealth)
                {
                    hoverHeart1.StartPulse();
                    hoverHeart2.StopPulse();
                    hoverHeart3.StopPulse();
                    hoverHeart4.StopPulse();
                    hoverHeart5.StopPulse();
                    hoverHeart6.StopPulse();
                    hoverHeart7.StopPulse();
                    hoverHeart8.StopPulse();
                    hoverHeart9.StopPulse();
                    hoverHeart10.StopPulse();
                }
            }
            for (int hf = 0; hf < second.Length; hf++)
            {
                if (second[hf] == playerHealth)
                {
                    hoverHeart1.StopPulse();
                    hoverHeart2.StartPulse();
                    hoverHeart3.StopPulse();
                    hoverHeart4.StopPulse();
                    hoverHeart5.StopPulse();
                    hoverHeart6.StopPulse();
                    hoverHeart7.StopPulse();
                    hoverHeart8.StopPulse();
                    hoverHeart9.StopPulse();
                    hoverHeart10.StopPulse();
                }
            }
            for (int hf = 0; hf < third.Length; hf++)
            {
                if (third[hf] == playerHealth)
                {
                    hoverHeart1.StopPulse();
                    hoverHeart2.StopPulse();
                    hoverHeart3.StartPulse();
                    hoverHeart4.StopPulse();
                    hoverHeart5.StopPulse();
                    hoverHeart6.StopPulse();
                    hoverHeart7.StopPulse();
                    hoverHeart8.StopPulse();
                    hoverHeart9.StopPulse();
                    hoverHeart10.StopPulse();
                }
            }
            for (int hf = 0; hf < fourth.Length; hf++)
            {
                if (fourth[hf] == playerHealth)
                {
                    hoverHeart1.StopPulse();
                    hoverHeart2.StopPulse();
                    hoverHeart3.StopPulse();
                    hoverHeart4.StartPulse();
                    hoverHeart5.StopPulse();
                    hoverHeart6.StopPulse();
                    hoverHeart7.StopPulse();
                    hoverHeart8.StopPulse();
                    hoverHeart9.StopPulse();
                    hoverHeart10.StopPulse();
                }
            }
            for (int hf = 0; hf < fifth.Length; hf++)
            {
                if (fifth[hf] == playerHealth)
                {
                    hoverHeart1.StopPulse();
                    hoverHeart2.StopPulse();
                    hoverHeart3.StopPulse();
                    hoverHeart4.StopPulse();
                    hoverHeart5.StartPulse();
                    hoverHeart6.StopPulse();
                    hoverHeart7.StopPulse();
                    hoverHeart8.StopPulse();
                    hoverHeart9.StopPulse();
                    hoverHeart10.StopPulse();
                }
            }
            for (int hf = 0; hf < sixth.Length; hf++)
            {
                if (sixth[hf] == playerHealth)
                {
                    hoverHeart1.StopPulse();
                    hoverHeart2.StopPulse();
                    hoverHeart3.StopPulse();
                    hoverHeart4.StopPulse();
                    hoverHeart5.StopPulse();
                    hoverHeart6.StartPulse();
                    hoverHeart7.StopPulse();
                    hoverHeart8.StopPulse();
                    hoverHeart9.StopPulse();
                    hoverHeart10.StopPulse();
                }
            }
            for (int hf = 0; hf < seventh.Length; hf++)
            {
                if (seventh[hf] == playerHealth)
                {
                    hoverHeart1.StopPulse();
                    hoverHeart2.StopPulse();
                    hoverHeart3.StopPulse();
                    hoverHeart4.StopPulse();
                    hoverHeart5.StopPulse();
                    hoverHeart6.StopPulse();
                    hoverHeart7.StartPulse();
                    hoverHeart8.StopPulse();
                    hoverHeart9.StopPulse();
                    hoverHeart10.StopPulse();
                }
            }
            for (int hf = 0; hf < eighth.Length; hf++)
            {
                if (eighth[hf] == playerHealth)
                {
                    hoverHeart1.StopPulse();
                    hoverHeart2.StopPulse();
                    hoverHeart3.StopPulse();
                    hoverHeart4.StopPulse();
                    hoverHeart5.StopPulse();
                    hoverHeart6.StopPulse();
                    hoverHeart7.StopPulse();
                    hoverHeart8.StartPulse();
                    hoverHeart9.StopPulse();
                    hoverHeart10.StopPulse();
                }
            }
            for (int hf = 0; hf < nineth.Length; hf++)
            {
                if (nineth[hf] == playerHealth)
                {
                    hoverHeart1.StopPulse();
                    hoverHeart2.StopPulse();
                    hoverHeart3.StopPulse();
                    hoverHeart4.StopPulse();
                    hoverHeart5.StopPulse();
                    hoverHeart6.StopPulse();
                    hoverHeart7.StopPulse();
                    hoverHeart8.StopPulse();
                    hoverHeart9.StartPulse();
                    hoverHeart10.StopPulse();
                }
            }
            for (int hf = 0; hf < tenth.Length; hf++)
            {
                if (tenth[hf] == playerHealth)
                {
                    hoverHeart1.StopPulse();
                    hoverHeart2.StopPulse();
                    hoverHeart3.StopPulse();
                    hoverHeart4.StopPulse();
                    hoverHeart5.StopPulse();
                    hoverHeart6.StopPulse();
                    hoverHeart7.StopPulse();
                    hoverHeart8.StopPulse();
                    hoverHeart9.StopPulse();
                    hoverHeart10.StartPulse();
                }
            }
        }
    }
    //=================================================PlayerMoney=============================================//
    public void AddGold(int amount)
    {
        playerGold += amount;
        if (!DebugSystem.goldMode)
        {
            audioSrc.volume = 1;
            audioSrc.pitch = 1;
            audioSrc.PlayOneShot(playSound.gold);
            goldFlashTime = goldFlashTimer;
            isGold = true;
        }
      
        if (playerGold > 99999)
            playerGold = 99999;
        ApplyGold();
    }
    public void RemoveGold(int amount)
    {
        playerGold -= amount;
        if (playerGold < 0)
            playerGold = 0;
        ApplyGold();
    }
    public void ApplyGold()
    {
        goldText.text = playerGold + "G".ToString();
        statusGoldText.text = playerGold + "G".ToString();
    }
    public void FlashEnemyDestroyed()
    {
        isEnemyDestroyed = true;
    }
    public IEnumerator FadeIn(float aValue, float aTime, Image image, bool imgProperty)
    {
        float alpha = image.GetComponent<Image>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.fixedDeltaTime / aTime)
        {
            Color newColor = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(alpha, aValue, t));
            image.GetComponent<Image>().color = newColor;
            if (t >= 0.9)
            {
                if (imgProperty)
                    newColor = new Color(0, 0, 0, 1);
                else
                    newColor = new Color(1, 1, 1, 1);
                image.GetComponent<Image>().color = newColor;

            }
            else
            {
                image.GetComponent<Image>().enabled = true;
            }

            yield return null;
        }

    }
    public IEnumerator FadeOut(float aValue, float aTime, Image image, bool imgProperty)
    {
        float alpha = image.GetComponent<Image>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.fixedDeltaTime / aTime)
        {
            Color newColor = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(alpha, aValue, t));
            image.GetComponent<Image>().color = newColor;
            if (t >= 0.9)
            {
                if (imgProperty)
                    newColor = new Color(0, 0, 0, 0);
                else
                    newColor = new Color(1, 1, 1, 0);
                image.GetComponent<Image>().color = newColor;


            }
            else
            {
                image.GetComponent<Image>().enabled = true;
            }
            yield return null;
        }

    }
    public IEnumerator AudioFadeOut(AudioSource audio)
    {
        audio.volume = 1.0f;
        float MinVol = 0;
        for (float f = 1f; f > MinVol; f -= 0.05f)
        {
            audio.volume = f;
            yield return new WaitForSecondsRealtime(.1f);
            if (f <= 0.1)
            {

                audio.Pause();
                audio.volume = MinVol;
                break;
            }
        }
    }
    public IEnumerator AudioFadeIn(AudioSource audio)
    {
        audio.volume = 0.0f;
        float MaxVol = 1;
        if (!audio.isPlaying)
            audio.Play();
        else
            audio.UnPause();
        for (float f = 0f; f < MaxVol; f += 0.05f)
        {
            audio.volume = f;
            yield return new WaitForSecondsRealtime(.1f);
            if (f >= 0.9)
            {
                audio.volume = MaxVol;
                break;
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Darkness"))
        {
            StartCoroutine(FadeIn(1, 0.1f, blackScreen, true));
            audioSrc.volume = 1;
            audioSrc.pitch = 1;
            audioSrc.PlayOneShot(playSound.nextArea);
        }
        if (other.gameObject.CompareTag("Vitality"))
        {
            AddVitality(1);
            PlayerRecovery(1);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Damage"))
        {
            PlayerDamage(18, false);
        }
        if (other.gameObject.CompareTag("Recovery"))
        {
            PlayerRecovery(100);
        }
        if (other.gameObject.CompareTag("Gold"))
        {
            AddGold(1);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Heal"))
        {
            PlayerRecovery(100);
            audioSrc.PlayOneShot(playSound.revive);
        }
        if (other.gameObject.CompareTag("CheckPoint"))
        {
            SavePosition();
        }
        if (other.gameObject.CompareTag("Helix"))
        {
            audioSrc.PlayOneShot(playSound.pickup);
            helixArmorPickedUp = true;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Apex"))
        {
            audioSrc.PlayOneShot(playSound.pickup);
            apexArmorPickedUp = true;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Mythic"))
        {
            audioSrc.PlayOneShot(playSound.pickup);
            mythicArmorPickedUp = true;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Legendary"))
        {
            audioSrc.PlayOneShot(playSound.pickup);
            legendaryArmorPickedUp = true;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Storms"))
        {
            audioSrc.PlayOneShot(playSound.pickup);
            stormBootPickedUp = true;
            Destroy(other.gameObject);
        }
        SetupArmorUI();
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Darkness"))
            StartCoroutine(FadeOut(0, 0.5f, blackScreen, true));
    }
    public void LoadPlayerSystem(int health, int vitality, int gold, Vector3 position, Quaternion rotation)
    {
        playerHealth = health;
        playerVitality = vitality;
        playerGold = gold;
        ApplyGold();
        ReEvaluateHearts();
        playerTransform.position = position;
        playerTransform.rotation = rotation;
        SetHealthHeartRatio();
        ApplyHealthOrVitality();
        SetHeartHover();
    }
    public void SavePosition()
    {
        playerTransform = transform.position;
        lastPosition = playerTransform.position;
        lastRotation = playerTransform.rotation;
    }
    public void LoadPosition()
    {
        playerTransform.position = lastPosition;
        playerTransform.rotation = lastRotation;
        transform = playerTransform;
    }
    //=================================================PlayerStats=============================================//
    public enum PlayerEffectStats { poisonMin, poisonMax, confuse, paralyzed, silence, swamped, spiked }
    public void ApplyPlayerEffect(PlayerEffectStats stat, bool on)
    {
        if (on)
        {
            switch (stat)
            {
                case PlayerEffectStats.poisonMin:
                    {
                        if (!isPoisoned)
                        {
                            poisonCounter = UnityEngine.Random.Range(5, 10);
                            isPoisoned = true;
                        }

                        break;
                    }
                case PlayerEffectStats.poisonMax:
                    {
                        if (!isPoisoned)
                        {
                            poisonCounter = 100;
                            isPoisoned = true;
                        }
                        break;
                    }
                case PlayerEffectStats.confuse:
                    {
                        if (!isConfused)
                        {
                            characterCtrl = GetComponent<CharacterSystem>();
                            characterCtrl.ConfusePlayer(true);
                            isConfused = true;
                        }
                        break;
                    }
                case PlayerEffectStats.paralyzed:
                    {
                        if (!isParalyzed)
                        {
                            characterCtrl = GetComponent<CharacterSystem>();
                            characterCtrl.ParalyzedPlayer(true);
                            isParalyzed = true;
                        }
                        break;
                    }
                case PlayerEffectStats.silence:
                    {
                        if (!isSilenced)
                        {
                            swordSyst = GetComponent<SwordSystem>();
                            swordSyst.SilencedPlayer(true);
                            isSilenced = true;
                        }
                        break;

                    }
                case PlayerEffectStats.swamped:
                    {
                        if (!isSwamped)
                            isSwamped = true;
                        break;
                    }
                case PlayerEffectStats.spiked:
                    {
                        if (!isSpiked)
                            isSpiked = true;
                        break;
                    }
            }
        }
        else
        {
            switch (stat)
            {
                case PlayerEffectStats.poisonMin:
                    {
                        if (isPoisoned)
                        {
                            isPoisoned = false;
                            poisonFlash.enabled = false;
                            poisonCounter = 0;
                            poisonFlashTime = 0.3f;
                        }
                        break;
                    }
                case PlayerEffectStats.poisonMax:
                    {
                        if (isPoisoned)
                        {
                            isPoisoned = false;
                            poisonFlash.enabled = false;
                            poisonCounter = 0;
                            poisonFlashTime = 0.3f;
                        }
                        break;
                    }
                case PlayerEffectStats.confuse:
                    {
                        if (isConfused)
                        {
                            isConfused = false;
                            confusedFlash.enabled = false;
                            characterCtrl = GetComponent<CharacterSystem>();
                            characterCtrl.ConfusePlayer(false);
                        }
                        break;
                    }
                case PlayerEffectStats.paralyzed:
                    {
                        if (isParalyzed)
                        {
                            isParalyzed = false;
                            paralyzedFlash.enabled = false;
                            characterCtrl = GetComponent<CharacterSystem>();
                            characterCtrl.ParalyzedPlayer(false);
                            paralyzedTime = paralyzedTimer;
                        }
                        break;
                    }
                case PlayerEffectStats.silence:
                    {
                        if (isSilenced)
                        {
                            isSilenced = false;
                            silenceFlash.enabled = false;
                            swordSyst = GetComponent<SwordSystem>();
                            swordSyst.SilencedPlayer(false);
                        }
                        break;
                    }
                case PlayerEffectStats.swamped:
                    {
                        if (isSwamped)
                        {
                            swampFlashTime = 0.1f;
                            isSwamped = false;
                        }
                        break;
                    }
                case PlayerEffectStats.spiked:
                    {
                        if (isSpiked)
                        {
                            spikeFlashTime = 0.1f;
                            isSpiked = false;
                        }
                        break;
                    }
            }
        }

    }
    public void resetDeath()
    {
        isDead = false;
    }
    public void ResetPlayer()
    {
        ObjectSystem objectSystem = GameObject.Find("Core").GetComponent<ObjectSystem>();
        objectSystem.UnBeatTheGame();
        TimeSystem timeSys = GameObject.Find("Core/TimeSystem/System").GetComponent<TimeSystem>();
        timeSys.resetTimeSystem();
        ItemSystem items = GetComponent<ItemSystem>();
        items.resetItems();
        swordSyst = GetComponent<SwordSystem>();
        swordSyst.ResetSwords();
        ResetArmor();
        ResetBoots();
        JewelSystem jewelSys = GetComponent<JewelSystem>();
        jewelSys.ResetJewels();
        playerTransform = transform;
        damageFlashTime = damageFlashTimer;
        paralyzedTime = paralyzedTimer;
        goldFlashTime = goldFlashTimer;
        poisonFlashTime = 0.3f;
        playerGold = 0;
        playerHealth = 5;
        playerVitality = 5;
        isLowHealth = false;
        playSound.lowHealthSrc.clip = null;
        playSound.lowHealthSrc.loop = false;
        ApplyGold();
        ReEvaluateHearts();
        isDead = false;
        SlotCounter = 4;
        damageFlash.enabled = false;
        weakflash.enabled = false;
        ApplyPlayerEffect(PlayerEffectStats.poisonMin, false);
        ApplyPlayerEffect(PlayerEffectStats.poisonMax, false);
        ApplyPlayerEffect(PlayerEffectStats.confuse, false);
        ApplyPlayerEffect(PlayerEffectStats.paralyzed, false);
        ApplyPlayerEffect(PlayerEffectStats.silence, false);
        SetHealthHeartRatio();
        ApplyHealthOrVitality();
    }
    public void GotItem()
    {
        itemFlashTime = itemFlashTimer;
        isItem = true;
    }
    //=================================================PlayerArmor=============================================//
    public void SelectArmor(ArmorActive type)
    {
        switch (type)
        {
            case ArmorActive.dense:
                {
                    denseArmorEnabled = true;
                    helixArmorEnabled = false;
                    apexArmorEnabled = false;
                    mythicArmorEnabled = false;
                    legendaryArmorEnabled = false;

                    armorUI.denseUI.enabled = true;
                    armorUI.denseButton.interactable = true;

                    armorUI.statusArmorIcon.sprite = armorUI.denseUI.sprite;
                    armorUI.statusArmorIcon.enabled = true;

                    armorUI.statusArmorText.text = "Dense Armor";
                    break;
                }
            case ArmorActive.helix:
                {
                    if (helixArmorPickedUp)
                    {
                        denseArmorEnabled = false;
                        helixArmorEnabled = true;
                        apexArmorEnabled = false;
                        mythicArmorEnabled = false;
                        legendaryArmorEnabled = false;

                        armorUI.helixUI.enabled = true;
                        armorUI.helixButton.interactable = true;

                        armorUI.statusArmorIcon.sprite = armorUI.helixUI.sprite;
                        armorUI.statusArmorIcon.enabled = true;

                        armorUI.statusArmorText.text = "Helix Armor";
                    }
                    break;
                }
            case ArmorActive.apex:
                {
                    if (apexArmorPickedUp)
                    {
                        denseArmorEnabled = false;
                        helixArmorEnabled = false;
                        apexArmorEnabled = true;
                        mythicArmorEnabled = false;
                        legendaryArmorEnabled = false;

                        armorUI.apexUI.enabled = true;
                        armorUI.apexButton.interactable = true;

                        armorUI.statusArmorIcon.sprite = armorUI.apexUI.sprite;
                        armorUI.statusArmorIcon.enabled = true;

                        armorUI.statusArmorText.text = "Apex Armor";
                    }
                    break;
                }
            case ArmorActive.mythic:
                {
                    if (mythicArmorPickedUp)
                    {
                        denseArmorEnabled = false;
                        helixArmorEnabled = false;
                        apexArmorEnabled = false;
                        mythicArmorEnabled = true;
                        legendaryArmorEnabled = false;

                        armorUI.mythicUI.enabled = true;
                        armorUI.mythicButton.interactable = true;

                        armorUI.statusArmorIcon.sprite = armorUI.mythicUI.sprite;
                        armorUI.statusArmorIcon.enabled = true;

                        armorUI.statusArmorText.text = "Mythic Armor";
                    }
                    break;
                }
            case ArmorActive.legendary:
                {
                    if (legendaryArmorPickedUp)
                    {
                        denseArmorEnabled = false;
                        helixArmorEnabled = false;
                        apexArmorEnabled = false;
                        mythicArmorEnabled = false;
                        legendaryArmorEnabled = true;

                        armorUI.legendaryUI.enabled = true;
                        armorUI.legendaryButton.interactable = true;

                        armorUI.statusArmorIcon.sprite = armorUI.legendaryUI.sprite;
                        armorUI.statusArmorIcon.enabled = true;

                        armorUI.statusArmorText.text = "Legendary Armor";
                    }
                    break;
                }
        }
        SetupArmorUI();
    }
    public void ArmorSelection(int num)
    {
        if (num == 0)
            SelectArmor(ArmorActive.dense);
        if (num == 1)
            SelectArmor(ArmorActive.helix);
        if (num == 2)
            SelectArmor(ArmorActive.apex);
        if (num == 3)
            SelectArmor(ArmorActive.mythic);
        if (num == 4)
            SelectArmor(ArmorActive.legendary);
    }
    public void SetupArmorUI()
    {
        if (helixArmorPickedUp)
        {
            armorUI.helixUI.enabled = true;
            armorUI.helixButton.interactable = true;
        }
        else
        {
            armorUI.helixUI.enabled = false;
            armorUI.helixButton.interactable = false;
        }

        if (apexArmorPickedUp)
        {
            armorUI.apexUI.enabled = true;
            armorUI.apexButton.interactable = true;
        }
        else
        {
            armorUI.apexUI.enabled = false;
            armorUI.apexButton.interactable = false;
        }
        if (mythicArmorPickedUp)
        {
            armorUI.mythicUI.enabled = true;
            armorUI.mythicButton.interactable = true;
        }
        else
        {
            armorUI.mythicUI.enabled = false;
            armorUI.mythicButton.interactable = false;
        }
        if (legendaryArmorPickedUp)
        {
            armorUI.legendaryUI.enabled = true;
            armorUI.legendaryButton.interactable = true;
        }
        else
        {
            armorUI.legendaryUI.enabled = false;
            armorUI.legendaryButton.interactable = false;
        }
    }
    public void ResetArmor()
    {
        SelectArmor(ArmorActive.dense);
        apexArmorPickedUp = false;
        helixArmorPickedUp = false;
        mythicArmorPickedUp = false;
        legendaryArmorPickedUp = false;
        SetupArmorUI();
    }
    public void LoadArmorSettings(bool Helix, bool helixEnabled, bool Apex, bool apexEnabled, bool Mythic, bool mythicEnabled, bool Legendary, bool legendaryEnabled)
    {
        helixArmorPickedUp = Helix;
        apexArmorPickedUp = Apex;
        mythicArmorPickedUp = Mythic;
        legendaryArmorPickedUp = Legendary;

        helixArmorEnabled = helixEnabled;
        apexArmorEnabled = apexEnabled;
        mythicArmorEnabled = mythicEnabled;
        legendaryArmorEnabled = legendaryEnabled;

        if (helixArmorEnabled)
            SelectArmor(ArmorActive.helix);
        else if (apexArmorEnabled)
            SelectArmor(ArmorActive.apex);
        else if (mythicArmorEnabled)
            SelectArmor(ArmorActive.mythic);
        else if (legendaryArmorEnabled)
            SelectArmor(ArmorActive.legendary);
        else
            SelectArmor(ArmorActive.dense);
    }
    public void GetArmor(ArmorActive type)
    {
        switch (type)
        {
            case ArmorActive.helix:
                {
                    helixArmorPickedUp = true;
                    SelectArmor(ArmorActive.helix);
                    break;
                }
            case ArmorActive.apex:
                {
                    apexArmorPickedUp = true;
                    SelectArmor(ArmorActive.apex);
                    break;
                }
            case ArmorActive.mythic:
                {
                    mythicArmorPickedUp = true;
                    SelectArmor(ArmorActive.mythic);
                    break;
                }
            case ArmorActive.legendary:
                {
                    legendaryArmorPickedUp = true;
                    SelectArmor(ArmorActive.legendary);
                    break;
                }
        }
        SetupArmorUI();
    }
    //=================================================PlayerBoots=============================================//
    public void SelectBoots(BootActive type)
    {
        switch (type)
        {
            case BootActive.trail:
                {
                    trailBootEnabled = true;
                    hoverBootEnabled = false;
                    cinderBootEnabled = false;
                    stormBootEnabled = false;
                    medicalBootEnabled = false;

                    bootUI.trailUI.enabled = true;
                    bootUI.trailButton.interactable = true;

                    bootUI.statusBootIcon.sprite = bootUI.trailUI.sprite;
                    bootUI.statusBootIcon.enabled = true;

                    bootUI.statusBootText.text = "Trail Blazers";
                    break;
                }

            case BootActive.hover:
                {
                    if (hoverBootPickedUp)
                    {
                        trailBootEnabled = false;
                        hoverBootEnabled = true;
                        cinderBootEnabled = false;
                        stormBootEnabled = false;
                        medicalBootEnabled = false;

                        bootUI.hoverUI.enabled = true;
                        bootUI.hoverButton.interactable = true;

                        bootUI.statusBootIcon.sprite = bootUI.hoverUI.sprite;
                        bootUI.statusBootIcon.enabled = true;

                        bootUI.statusBootText.text = "Hovers";
                    }
                    break;
                }

            case BootActive.cinder:
                {
                    if (cinderBootPickedUp)
                    {
                        trailBootEnabled = false;
                        hoverBootEnabled = false;
                        cinderBootEnabled = true;
                        stormBootEnabled = false;
                        medicalBootEnabled = false;

                        bootUI.cinderUI.enabled = true;
                        bootUI.cinderButton.interactable = true;

                        bootUI.statusBootIcon.sprite = bootUI.cinderUI.sprite;
                        bootUI.statusBootIcon.enabled = true;

                        bootUI.statusBootText.text = "Cinders";
                    }
                    break;
                }

            case BootActive.storm:
                {
                    if (stormBootPickedUp)
                    {
                        trailBootEnabled = false;
                        hoverBootEnabled = false;
                        cinderBootEnabled = false;
                        stormBootEnabled = true;
                        medicalBootEnabled = false;

                        bootUI.stormUI.enabled = true;
                        bootUI.stormButton.interactable = true;

                        bootUI.statusBootIcon.sprite = bootUI.stormUI.sprite;
                        bootUI.statusBootIcon.enabled = true;

                        bootUI.statusBootText.text = "Storms";
                    }
                    break;
                }

            case BootActive.medical:
                {
                    if (medicalBootPickedUp)
                    {
                        trailBootEnabled = false;
                        hoverBootEnabled = false;
                        cinderBootEnabled = false;
                        stormBootEnabled = false;
                        medicalBootEnabled = true;

                        bootUI.medicalUI.enabled = true;
                        bootUI.medicalButton.interactable = true;

                        bootUI.statusBootIcon.sprite = bootUI.medicalUI.sprite;
                        bootUI.statusBootIcon.enabled = true;

                        bootUI.statusBootText.text = "Medicals";
                        recoveryTime = recoveryTimer;
                    }
                    break;
                }
        }
        SetupBootUI();
    }
    public void SetupBootUI()
    {
        if (hoverBootPickedUp)
        {
            bootUI.hoverUI.enabled = true;
            bootUI.hoverButton.interactable = true;
        }
        else
        {
            bootUI.hoverUI.enabled = false;
            bootUI.hoverButton.interactable = false;
        }

        if (cinderBootPickedUp)
        {
            bootUI.cinderUI.enabled = true;
            bootUI.cinderButton.interactable = true;
        }
        else
        {
            bootUI.cinderUI.enabled = false;
            bootUI.cinderButton.interactable = false;
        }
        if (stormBootPickedUp)
        {
            bootUI.stormUI.enabled = true;
            bootUI.stormButton.interactable = true;
        }
        else
        {
            bootUI.stormUI.enabled = false;
            bootUI.stormButton.interactable = false;
        }
        if (medicalBootPickedUp)
        {
            bootUI.medicalUI.enabled = true;
            bootUI.medicalButton.interactable = true;
        }
        else
        {
            bootUI.medicalUI.enabled = false;
            bootUI.medicalButton.interactable = false;
        }
    }
    public void ResetBoots()
    {
        SelectBoots(BootActive.trail);
        hoverBootPickedUp = false;
        cinderBootPickedUp = false;
        stormBootPickedUp = false;
        medicalBootPickedUp = false;
        SetupBootUI();
    }
    public void BootSelection(int num)
    {
        if (num == 0)
            SelectBoots(BootActive.trail);
        if (num == 1)
            SelectBoots(BootActive.hover);
        if (num == 2)
            SelectBoots(BootActive.cinder);
        if (num == 3)
            SelectBoots(BootActive.storm);
        if (num == 4)
            SelectBoots(BootActive.medical);
    }
    public void LoadBootSettings(bool Hover, bool hoverEnabled, bool Cinder, bool cinderEnabled, bool Storm, bool stormEnabled, bool Medical, bool medicalEnabled)
    { 
        hoverBootPickedUp = Hover;
        cinderBootPickedUp = Cinder;
        stormBootPickedUp = Storm;
        medicalBootPickedUp = Medical;

        hoverBootEnabled = hoverEnabled;
        cinderBootEnabled = cinderEnabled;
        stormBootEnabled = stormEnabled;
        medicalBootEnabled = medicalEnabled;

        if (hoverBootEnabled)
            SelectBoots(BootActive.hover);
        else if (cinderBootEnabled)
            SelectBoots(BootActive.cinder);
        else if (stormBootEnabled)
            SelectBoots(BootActive.storm);
        else if (medicalBootEnabled)
            SelectBoots(BootActive.medical);
        else
            SelectBoots(BootActive.trail);
    }
    public void GetBoots(BootActive type)
    {
        switch (type)
        {
            case BootActive.hover:
                {
                    hoverBootPickedUp = true;
                    SelectBoots(BootActive.hover);
                    break;
                }
            case BootActive.cinder:
                {
                    cinderBootPickedUp = true;
                    SelectBoots(BootActive.cinder);
                    break;
                }
            case BootActive.storm:
                {
                    stormBootPickedUp = true;
                    SelectBoots(BootActive.storm);
                    break;
                }
            case BootActive.medical:
                {
                    medicalBootPickedUp = true;
                    SelectBoots(BootActive.medical);
                    break;
                }
        }
        SetupBootUI();
    }
}
[Serializable]
public struct HeartCases
{
    [Header("Primary Hearts 1-10")]
    public Image sHeart1;
    public GameObject enableSHeartHover1;
    public Image sHeart2;
    public GameObject enableSHeartHover2;
    public Image sHeart3;
    public GameObject enableSHeartHover3;
    public Image sHeart4;
    public GameObject enableSHeartHover4;
    public Image sHeart5;
    public GameObject enableSHeartHover5;
    public Image sHeart6;
    public GameObject enableSHeart6;
    public GameObject enableSHeartHover6;
    public Image sHeart7;
    public GameObject enableSHeart7;
    public GameObject enableSHeartHover7;
    public Image sHeart8;
    public GameObject enableSHeart8;
    public GameObject enableSHeartHover8;
    public Image sHeart9;
    public GameObject enableSHeart9;
    public GameObject enableSHeartHover9;
    public Image sHeart10;
    public GameObject enableSHeart10;
    public GameObject enableSHeartHover10;
    [Space]
    [Header("Secondary Hearts 1-4")]
    public Image mHeart1;
    public GameObject enableMHeart1;
    public Image mHeart2;
    public GameObject enableMHeart2;
    public Image mHeart3;
    public GameObject enableMHeart3;
    public Image mHeart4;
    public GameObject enableMHeart4;
    [Space]
    [Header("Final Heart 1")]
    public Image lHeart1;
    public GameObject enableLHeart1;
    [Space]

    [Header("Status Primary Hearts 1-10")]
    public Image statusSHeart1;
    public Image statusSHeart2;
    public Image statusSHeart3;
    public Image statusSHeart4;
    public Image statusSHeart5;
    public Image statusSHeart6;
    public GameObject enableStatusSHeart6;
    public Image statusSHeart7;
    public GameObject enableStatusSHeart7;
    public Image statusSHeart8;
    public GameObject enableStatusSHeart8;
    public Image statusSHeart9;
    public GameObject enableStatusSHeart9;
    public Image statusSHeart10;
    public GameObject enableStatusSHeart10;
    [Space]
    [Header("Status Secondary Hearts 1-4")]
    public Image statusMHeart1;
    public GameObject enableStatusMHeart1;
    public Image statusMHeart2;
    public GameObject enableStatusMHeart2;
    public Image statusMHeart3;
    public GameObject enableStatusMHeart3;
    public Image statusMHeart4;
    public GameObject enableStatusMHeart4;
    [Space]
    [Header("Status Final Heart 1")]
    public Image statusLHeart1;
    public GameObject enableStatusLHeart1;
}
[Serializable]
public struct EffectCases
{
    [Header("Effect Slots")]
    public GameObject Slot1;
    public GameObject Slot2;
    public GameObject Slot3;
    public GameObject Slot4;
    [Space]
    [Header("Effects")]
    public Image poisoned;
    public Image confused;
    public Image paralyzed;
    public Image silenced;

    
    
}
[Serializable]
public struct ArmorUI
{
    public Image statusArmorIcon;
    public Text statusArmorText;
    public Image denseUI;
    public Image helixUI;
    public Image apexUI;
    public Image mythicUI;
    public Image legendaryUI;

    public Button denseButton;
    public Button helixButton;
    public Button apexButton;
    public Button mythicButton;
    public Button legendaryButton;
}

[Serializable]
public struct BootUI
{
    public Image statusBootIcon;
    public Text statusBootText;
    public Image trailUI;
    public Image hoverUI;
    public Image cinderUI;
    public Image stormUI;
    public Image medicalUI;

    public Button trailButton;
    public Button hoverButton;
    public Button cinderButton;
    public Button stormButton;
    public Button medicalButton;
}
[Serializable]
public struct PlaySounds
{
    public AudioSource lowHealthSrc;
    public AudioClip[] hurt;
    public AudioClip healthLow;
    public AudioClip death;
    public AudioClip gold;
    public AudioClip nextArea;
    public AudioClip revive;
    public AudioClip pickup;
}
