using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public enum SwordActive { mythril, flare, lightning, frost, terra}

public class SwordSystem : MonoBehaviour
{
    public SwordActive swordActive;
    OptSystem optSystem = new OptSystem();

    public SwordUI swordUI;
    CharacterSystem Character;
    Animator anim;
    AudioSource audioSrc;
    public AudioSource audioSrc2;
    public AudioClip BroadSwipe;
    public AudioClip[] playerYellQ;
    public AudioClip cut;
    public AudioClip sheathSfx;
    public AudioClip unSheathSfx;

    public GameObject UnArmedObj;
    public GameObject ArmedObj;

    public GameObject terraShockWave;
    public static bool isAttacking;
    public static bool isSilenced;

    public static bool UnArmed = true;
    public bool WeakAttacked;

    public static bool isMagicIdolActive;
    public static bool isTerraIdolActive;
    public float magicIdolTime;

    int randomYell;
    float putAwayTime;
    public float putAwayTimer = 1.5f;

    public static bool mythrilSwordEnabled = true;
    public static bool flareSwordEnabled = false;
    public static bool lightningSwordEnabled = false;
    public static bool frostSwordEnabled = false;
    public static bool terraSwordEnabled = false;

    public static bool flareSwordPickedUp = false;
    public static bool lightningSwordPickedUp = false;
    public static bool frostSwordPickedUp = false;
    public static bool terraSwordPickedUp = false;

    Image swordBar;
    public Image swordBarMythril;
    public Image swordBarFrost;
    public Image swordBarLightning;
    public Image swordBarFlare;
    public Image swordBarTerra;


    Image swordBarMagic;
    public Image swordBarMagicMythril;
    public Image swordBarMagicFlare;
    public Image swordBarMagicLightning;
    public Image swordBarMagicFrost;
    public Image swordBarMagicTerra;

    public GameObject swordUIMythril;
    public GameObject swordUIFlare;
    public GameObject swordUILightning;
    public GameObject swordUIFrost;
    public GameObject swordUITerra;

    BoxCollider swordCollider;
    public BoxCollider mythrilBladeCollider;
    public BoxCollider flareBladeCollider;
    public BoxCollider lightningBladeCollider;
    public BoxCollider frostBladeCollider;
    public BoxCollider terraBladeCollider;

    public GameObject mythrilSword;
    public GameObject flareSword;
    public GameObject lightningSword;
    public GameObject frostSword;
    public GameObject terraSword;

    public float swordChargeAmt = 0;
    public float chargeSpeed = 7;
    public bool swordFullyCharged;

    float flareEmis;
    float lightningEmis;
    float frostEmis;
    float terraEmis;

    IEnumerator idolTimer = null;
    IEnumerator powerTimer = null;
    float powerTime;
    public float terraPowerTime = 3;
    public float frostPowerTime = 1;
    bool SwordPowerActive = false;
    int ProjectileForce;
    public int flareProjectileForce;
    public int lightningProjectileForce;
    public int frostProjectileForce;

    Vector3 originalPos;
    Transform camTransform;
    public bool isEarthQuake;
    bool ResetCamera;

    // Start is called before the first frame update
    void Start()
    {
        Character = GetComponent<CharacterSystem>();
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        putAwayTime = putAwayTimer;
        if (SceneSystem.isNewGame)
        {
            SelectSword(swordActive);
            SheathSword(false);
        }
        SelectSword(SwordActive.mythril);
        SheathSword(false);
        camTransform = Camera.main.transform;
        originalPos = camTransform.localPosition;
        SetupSwordUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!swordFullyCharged && !mythrilSwordEnabled)
        {
            if (swordChargeAmt < 100)
            {
                swordChargeAmt += Time.unscaledDeltaTime * chargeSpeed;
            }
            if (swordChargeAmt >= 100)
            {
                swordChargeAmt = 100;
                swordFullyCharged = true;
            }
        }
        if (isEarthQuake)
        {
            ShakeScreen(1);
            ResetCamera = true;
        }
        else if (!isEarthQuake && ResetCamera)
        {
            camTransform.localPosition = originalPos;
            ResetCamera = false;
        }
        if (isMagicIdolActive)
        {
            swordBarMagic.enabled = true;
            swordBarMagic.color = new Color(swordBarMagic.color.r, swordBarMagic.color.g, swordBarMagic.color.b, Mathf.PingPong(Time.time, 1));
        }
        else if (!isMagicIdolActive)
            swordBarMagic.enabled = false;
        if (WeakAttacked)
            Character.SelectAnimation(CharacterSystem.PlayerAnimation.FastSwipe, true);
       
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            isAttacking = false;
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            isAttacking = false;
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            isAttacking = false;
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
            isAttacking = false;
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            isAttacking = false;
        if (optSystem.Input.GetButtonDown("Attack") && !CharacterSystem.isClimbing && !CharacterSystem.isCarrying && 
            !UnderWaterSystem.isSwimming && !PauseGame.isPaused && !WeakAttacked && !isAttacking && 
            !SceneSystem.isDisabled && !UnArmed && !isSilenced && !CharacterSystem.isParalyzed && 
            !ShopSystem.isShop && !JewelSystem.isJewelEnabled && !PlayerSystem.isDead)
        {
            randomYell = UnityEngine.Random.Range(0, 4);
            isAttacking = true;
            WeakAttacked = true;
        }
        else
            WeakAttacked = false;
        if (optSystem.Input.GetButton("Sheath") && !CharacterSystem.isJumping && !CharacterSystem.isClimbing && !CharacterSystem.isCarrying && 
            !UnderWaterSystem.isSwimming && !PauseGame.isPaused && !ShopSystem.isShop && !WeakAttacked && 
            !isAttacking && !SceneSystem.isDisabled && !isSilenced && !CharacterSystem.isParalyzed && 
            !PlayerSystem.isDead)
        {
            if (putAwayTime > 0)
                putAwayTime -= Time.unscaledDeltaTime;
            else if (putAwayTime < 0)
            {
                if (UnArmed)
                {
                    audioSrc.pitch = 1;
                    audioSrc.volume = 1;
                    audioSrc.PlayOneShot(unSheathSfx);
                    UnArmed = false;
                    Character.SelectAnimation(CharacterSystem.PlayerAnimation.Armed, true);
                }
                else if (JewelSystem.isJewelEnabled)
                {
                    UnArmed = false;
                    Character.SelectAnimation(CharacterSystem.PlayerAnimation.Armed, true);
                }
                else if (!UnArmed)
                {
                    audioSrc.pitch = 1;
                    audioSrc.volume = 1;
                    audioSrc.PlayOneShot(sheathSfx);
                    UnArmed = true;
                    Character.SelectAnimation(CharacterSystem.PlayerAnimation.Unarmed, true);
                }
                putAwayTime = 0;
            }
        }
        else if (optSystem.Input.GetButtonUp("Sheath"))
            putAwayTime = putAwayTimer;
        handler();
        SwordParticleEmissions();
    }
    public void ActivateSwordCollider(int active)
    {
        if (active == 0)
            swordCollider.enabled = false;
        else
            swordCollider.enabled = true;
    }
    public void AlertObservers(string message)
    {
        if (message.Equals("AttackAnimationStarted"))
            isAttacking = true;
        else if (message.Equals("AttackAnimationEnded"))
            isAttacking = false;
       
    }
    public void SheathSword(bool sound)
    {
        if (sound)
        {
            Character.SelectAnimation(CharacterSystem.PlayerAnimation.Unarmed, true);
            audioSrc.PlayOneShot(sheathSfx);
        }
        UnArmed = true;
        UnArmedObj.SetActive(true);
        ArmedObj.SetActive(false);
        if (JewelSystem.isJewelEnabled)
        {
            JewelSystem jewelSys = GetComponent<JewelSystem>();
            jewelSys.JewelEngaged(0);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ISwordHittable>() != null)
        {
           
            if (PlayerSystem.playerVitality >= 5 && PlayerSystem.playerVitality < 13)
            {
                if (swordFullyCharged)
                {
                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 2);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 1);
            }

            else if (PlayerSystem.playerVitality >= 13 && PlayerSystem.playerVitality < 18)
            {
                if (swordFullyCharged)
                {
                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 3);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 2);
            }

            else if (PlayerSystem.playerVitality >= 18 && PlayerSystem.playerVitality < 23)
            {
                if (swordFullyCharged)
                {
                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 4);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 3);
            }

            else if (PlayerSystem.playerVitality >= 23 && PlayerSystem.playerVitality < 28)
            {
                if (swordFullyCharged)
                {
                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 5);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 4);
            }

            else if (PlayerSystem.playerVitality >= 28 && PlayerSystem.playerVitality < 33)
            {
                if (swordFullyCharged)
                {
                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 6);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 5);
            }

            else if (PlayerSystem.playerVitality >= 33 && PlayerSystem.playerVitality < 38)
            {
                if (swordFullyCharged)
                {

                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 7);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 6);
            }
            else if (PlayerSystem.playerVitality >= 38 && PlayerSystem.playerVitality < 43)
            {
                if (swordFullyCharged)
                {

                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 8);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 7);
            }
            else if (PlayerSystem.playerVitality >= 43 && PlayerSystem.playerVitality < 48)
            {
                if (swordFullyCharged)
                {

                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 9);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 8);
            }
            else if (PlayerSystem.playerVitality >= 48 && PlayerSystem.playerVitality < 53)
            {
                if (swordFullyCharged)
                {

                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 10);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 9);
            }
            else if (PlayerSystem.playerVitality >= 53 && PlayerSystem.playerVitality < 58)
            {
                if (swordFullyCharged)
                {

                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 11);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 10);
            }
            else if (PlayerSystem.playerVitality >= 58 && PlayerSystem.playerVitality < 63)
            {
                if (swordFullyCharged)
                {

                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 12);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 11);
            }
            else if (PlayerSystem.playerVitality >= 63 && PlayerSystem.playerVitality < 68)
            {
                if (swordFullyCharged)
                {

                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 13);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 12);
            }
            else if (PlayerSystem.playerVitality >= 68 && PlayerSystem.playerVitality < 73)
            {
                if (swordFullyCharged)
                {

                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 14);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 13);
            }
            else if (PlayerSystem.playerVitality >= 73 && PlayerSystem.playerVitality < 78)
            {
                if (swordFullyCharged)
                {

                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 15);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 14);
            }

            else if (PlayerSystem.playerVitality >= 78 && PlayerSystem.playerVitality < 83)
            {
                if (swordFullyCharged)
                {

                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 16);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 15);
            }
            else if (PlayerSystem.playerVitality >= 83 && PlayerSystem.playerVitality < 88)
            {
                if (swordFullyCharged)
                {

                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 17);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 16);
            }
            else if (PlayerSystem.playerVitality >= 88 && PlayerSystem.playerVitality < 93)
            {
                if (swordFullyCharged)
                {

                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 18);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 17);
            }
            else if (PlayerSystem.playerVitality >= 93 && PlayerSystem.playerVitality < 96)
            {
                if (swordFullyCharged)
                {

                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 19);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 18);
            }

            else if (PlayerSystem.playerVitality >= 96 && PlayerSystem.playerVitality < 100)
            {
                if (swordFullyCharged)
                {

                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 20);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 19);
            }
            else if (PlayerSystem.playerVitality >= 100)
            {
                if (swordFullyCharged)
                {
                    if (other.gameObject.GetComponent<EnemySystem>() != null)
                    {
                        EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
                        if (flareSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.fire, true);
                        else if (lightningSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.electric, true);
                        else if (frostSwordEnabled)
                            enemySystem.ElementDamage(EnemySystem.Element.ice, true);
                    }
                    other.gameObject.SendMessage("OnGetHitBySword", 21);
                }
                else
                    other.gameObject.SendMessage("OnGetHitBySword", 20);
            }
            ActivateSwordCollider(0);

        }
        if (other.gameObject.CompareTag("Flare"))
        {
            audioSrc.PlayOneShot(unSheathSfx);
            flareSwordPickedUp = true;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Lightning"))
        {
            audioSrc.PlayOneShot(unSheathSfx);
            lightningSwordPickedUp = true;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Frost"))
        {
            audioSrc.PlayOneShot(unSheathSfx);
            frostSwordPickedUp = true;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Terra"))
        {
            audioSrc.PlayOneShot(unSheathSfx);
            terraSwordPickedUp = true;
            Destroy(other.gameObject);
        }
        SetupSwordUI();
    }
    public void ResetSwordPower()
    {
        if (!isMagicIdolActive)
        {
            swordChargeAmt = 0;
            swordFullyCharged = false;
            if (flareSwordEnabled)
            {
                swordUI.flarePartSys.Clear();
            }
            else if (lightningSwordEnabled)
            {
             swordUI.lightningPartSys.Clear();
            }
            else if (frostSwordEnabled)
            {
                swordUI.frostPartSys.Clear();
            }
            else if (terraSwordEnabled)
            {
                swordUI.terraPartSys.Clear();
            }
        }
    }
    public void ResetAttacked()
    {
        isAttacking = false;
    }
    public void SetUnarmed(int num)
    {
        if (num == 1)
        {
            UnArmedObj.SetActive(true);
            ArmedObj.SetActive(false);
            UnArmed = true;
        }
        else
        {
            UnArmedObj.SetActive(false);
            ArmedObj.SetActive(true);
        }
    }
    public void PlaySound()
    {
        if (randomYell == 0)
            audioSrc2.clip = playerYellQ[0];
        else if (randomYell == 1)
            audioSrc2.clip = playerYellQ[1];
        else if (randomYell == 2)
            audioSrc2.clip = playerYellQ[2];
        else
            audioSrc2.clip = playerYellQ[3];
        audioSrc2.volume = 1f;
        audioSrc2.pitch = UnityEngine.Random.Range(0.75f, 0.9f);
        audioSrc.volume = 1f;
        audioSrc.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        audioSrc2.Play();
        if (mythrilSwordEnabled)
            audioSrc.PlayOneShot(BroadSwipe);
    }
    public void SilencedPlayer(bool True)
    {
        if (True)
            isSilenced = true;
        else
            isSilenced = false;
    }
    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
    void handler()
    {
        if (mythrilSwordEnabled)
        {
            swordBar = swordBarMythril;
            swordBar.fillAmount = 1;
        }
        else if (flareSwordEnabled)
            swordBar = swordBarFlare;
        else if (lightningSwordEnabled)
            swordBar = swordBarLightning;
        else if (frostSwordEnabled)
            swordBar = swordBarFrost;
        else if (terraSwordEnabled)
            swordBar = swordBarTerra;
        else swordBar = null;

        if (swordBar != null && !mythrilSwordEnabled)
            swordBar.fillAmount = Map(swordChargeAmt, 0, 100, 0, 1);
    }
    IEnumerator MagicIdolTimer()
    {
        if (isMagicIdolActive)
        {
            yield return new WaitForSeconds(magicIdolTime);
            isMagicIdolActive = false;
            swordFullyCharged = false;
            swordChargeAmt = 0;
            StopCoroutine(idolTimer);
            idolTimer = null;
        }
    }
    public IEnumerator SwordPowerTime()
    {
        if (swordFullyCharged && !SwordPowerActive)
        {
            SwordPowerActive = true;
            if (flareSwordEnabled)
                InitSwordPower(SwordActive.flare);
            else if (lightningSwordEnabled)
                InitSwordPower(SwordActive.lightning);
            else if (frostSwordEnabled)
            {
                powerTime = frostPowerTime;
                InitSwordPower(SwordActive.frost);
            }
            else if (terraSwordEnabled)
            {
                powerTime = terraPowerTime;
                isEarthQuake = true;
                InitSwordPower(SwordActive.terra);
            }
            yield return new WaitForSecondsRealtime(powerTime);
            isTerraIdolActive = false;
            isEarthQuake = false;
            SwordPowerActive = false;
            if (powerTimer != null)
                StopCoroutine(powerTimer);
            powerTimer = null;
        }
      
    }
    public void ShutOffQuake()
    {
        isEarthQuake = false;
    }
    public IEnumerator QuakeEffect(float time)
    {
        isEarthQuake = true;
        yield return new WaitForSeconds(time);
        isEarthQuake = false;
    }
    public IEnumerator IdlePowerTime()
    {
        if (isTerraIdolActive)
        {
            if (PauseGame.isPaused)
                yield return new WaitUntil(() => PauseGame.isPaused == false);
            if (ShopSystem.isShop)
                yield return new WaitUntil(() => ShopSystem.isShop == false);
            powerTime = terraPowerTime;
            isEarthQuake = true;
            InitSwordPower(SwordActive.terra);
            yield return new WaitForSeconds(powerTime);
            isTerraIdolActive = false;
            isEarthQuake = false;
            SwordPowerActive = false;
            if(powerTimer != null)
                StopCoroutine(powerTimer);
            powerTimer = null;
        }
    }
    public void MaxOutSword(bool active)
    {
        if (active)
        {
            isMagicIdolActive = true;
            swordChargeAmt = 100;
            swordFullyCharged = true;
        }
        else
        {
            isMagicIdolActive = false;
            swordChargeAmt = 0;
            swordFullyCharged = false;
        }
    }
    public void MagicIdolUsed(bool True)
    {
        if (True)
        {
            isMagicIdolActive = true;
            swordChargeAmt = 100;
            swordFullyCharged = true;
            idolTimer = MagicIdolTimer();
            StartCoroutine(idolTimer);
        }
        else
        {
            isMagicIdolActive = false;
            if(idolTimer != null)
                StopCoroutine(idolTimer);
            idolTimer = null;
            swordFullyCharged = false;
            swordChargeAmt = 0;
        }

    }
    public void TerraIdolUsed()
    {
        isTerraIdolActive = true;
        powerTimer = IdlePowerTime();
        StartCoroutine(powerTimer);
       
    }
    public void SelectSword(SwordActive type)
    {
        if (!isMagicIdolActive)
        {
            swordChargeAmt = 0;
            swordFullyCharged = false;
        }
        switch (type)
        {
            case SwordActive.mythril:
                {
                    mythrilSword.SetActive(true);
                    flareSword.SetActive(false);
                    lightningSword.SetActive(false);
                    frostSword.SetActive(false);
                    terraSword.SetActive(false);

                    mythrilSwordEnabled = true;
                    flareSwordEnabled = false;
                    lightningSwordEnabled = false;
                    frostSwordEnabled = false;
                    terraSwordEnabled = false;

                    swordUIMythril.SetActive(true);
                    swordUIFlare.SetActive(false);
                    swordUILightning.SetActive(false);
                    swordUIFrost.SetActive(false);
                    swordUITerra.SetActive(false);

                    swordCollider = mythrilBladeCollider;
                    swordBarMagic = swordBarMagicMythril;

                    swordUI.mythrilUI.enabled = true;
                    swordUI.mythrilButton.interactable = true;

                    swordUI.statusSwordIcon.sprite = swordUI.mythrilUI.sprite;
                    swordUI.statusSwordIcon.enabled = true;

                    swordUI.statusSwordText.text = "Mythril Sword";

                    break;
                }
            case SwordActive.flare:
                {
                    if (flareSwordPickedUp)
                    {
                        mythrilSword.SetActive(false);
                        flareSword.SetActive(true);
                        lightningSword.SetActive(false);
                        frostSword.SetActive(false);
                        terraSword.SetActive(false);

                        mythrilSwordEnabled = false;
                        flareSwordEnabled = true;
                        lightningSwordEnabled = false;
                        frostSwordEnabled = false;
                        terraSwordEnabled = false;

                        swordUIMythril.SetActive(false);
                        swordUIFlare.SetActive(true);
                        swordUILightning.SetActive(false);
                        swordUIFrost.SetActive(false);
                        swordUITerra.SetActive(false);

                        swordCollider = flareBladeCollider;
                        swordBarMagic = swordBarMagicFlare;

                        swordUI.flareUI.enabled = true;
                        swordUI.flareButton.interactable = true;

                        swordUI.statusSwordIcon.sprite = swordUI.flareUI.sprite;
                        swordUI.statusSwordIcon.enabled = true;

                        swordUI.statusSwordText.text = "Flare Sword";
                    }
                    break;
                }
            case SwordActive.lightning:
                {
                    if (lightningSwordPickedUp)
                    {
                        mythrilSword.SetActive(false);
                        flareSword.SetActive(false);
                        lightningSword.SetActive(true);
                        frostSword.SetActive(false);
                        terraSword.SetActive(false);

                        mythrilSwordEnabled = false;
                        flareSwordEnabled = false;
                        lightningSwordEnabled = true;
                        frostSwordEnabled = false;
                        terraSwordEnabled = false;

                        swordUIMythril.SetActive(false);
                        swordUIFlare.SetActive(false);
                        swordUILightning.SetActive(true);
                        swordUIFrost.SetActive(false);
                        swordUITerra.SetActive(false);

                        swordCollider = lightningBladeCollider;
                        swordBarMagic = swordBarMagicLightning;

                        swordUI.lightningUI.enabled = true;
                        swordUI.lightningButton.interactable = true;

                        swordUI.statusSwordIcon.sprite = swordUI.lightningUI.sprite;
                        swordUI.statusSwordIcon.enabled = true;

                        swordUI.statusSwordText.text = "Lightning Sword";
                    }
                    break;
                }
            case SwordActive.frost:
                {
                    if (frostSwordPickedUp)
                    {
                        mythrilSword.SetActive(false);
                        flareSword.SetActive(false);
                        lightningSword.SetActive(false);
                        frostSword.SetActive(true);
                        terraSword.SetActive(false);

                        mythrilSwordEnabled = false;
                        flareSwordEnabled = false;
                        lightningSwordEnabled = false;
                        frostSwordEnabled = true;
                        terraSwordEnabled = false;

                        swordUIMythril.SetActive(false);
                        swordUIFlare.SetActive(false);
                        swordUILightning.SetActive(false);
                        swordUIFrost.SetActive(true);
                        swordUITerra.SetActive(false);

                        swordCollider = frostBladeCollider;
                        swordBarMagic = swordBarMagicFrost;

                        swordUI.frostUI.enabled = true;
                        swordUI.frostButton.interactable = true;

                        ProjectileForce = frostProjectileForce;

                        swordUI.statusSwordIcon.sprite = swordUI.frostUI.sprite;
                        swordUI.statusSwordIcon.enabled = true;

                        swordUI.statusSwordText.text = "Frost Sword";
                    }
                    break;
                }
            case SwordActive.terra:
                {
                    if (terraSwordPickedUp)
                    {
                        mythrilSword.SetActive(false);
                        flareSword.SetActive(false);
                        lightningSword.SetActive(false);
                        frostSword.SetActive(false);
                        terraSword.SetActive(true);

                        mythrilSwordEnabled = false;
                        flareSwordEnabled = false;
                        lightningSwordEnabled = false;
                        frostSwordEnabled = false;
                        terraSwordEnabled = true;

                        swordUIMythril.SetActive(false);
                        swordUIFlare.SetActive(false);
                        swordUILightning.SetActive(false);
                        swordUIFrost.SetActive(false);
                        swordUITerra.SetActive(true);

                        swordCollider = terraBladeCollider;
                        swordBarMagic = swordBarMagicTerra;

                        swordUI.terraUI.enabled = true;
                        swordUI.terraButton.interactable = true;

                        swordUI.statusSwordIcon.sprite = swordUI.terraUI.sprite;
                        swordUI.statusSwordIcon.enabled = true;

                        swordUI.statusSwordText.text = "Terra Sword";
                    }
                    break;
                }
        }
        SetupSwordUI();
    }
    public void SwordSelection(int num)
    {
        if (num == 0)
            SelectSword(SwordActive.mythril);
        else if (num == 1)
            SelectSword(SwordActive.flare);
        else if (num == 2)
            SelectSword(SwordActive.lightning);
        else if (num == 3)
            SelectSword(SwordActive.frost);
        else if (num == 4)
            SelectSword(SwordActive.terra);
    }
    public void InitSwordPower(SwordActive swordtype)
    {
        if (swordFullyCharged)
        {
            switch (swordtype)
            {
                case SwordActive.flare:
                    {
                        break;
                    }
                case SwordActive.lightning:
                    {
                        break;
                    }
                case SwordActive.frost:
                    {
                        int randomNum = UnityEngine.Random.Range(0, 2);
                        AudioClip clip = null;
                        if(randomNum == 0)
                            clip = swordUI.iceClips[0];
                        else if (randomNum == 1)
                            clip = swordUI.iceClips[1];
                        else if (randomNum == 2)
                            clip = swordUI.iceClips[2];
                        audioSrc.PlayOneShot(clip);
                        Rigidbody Temporary_RigidBody;
                        GameObject Temporary_Bullet_Handler;
                        Temporary_Bullet_Handler = Instantiate(swordUI.FrostPowerPrefab, swordUI.swordEmitter[2].position, swordUI.swordEmitter[2].transform.rotation) as GameObject;
                        Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();
                        Temporary_RigidBody.AddForce(swordUI.swordEmitter[2].transform.forward * ProjectileForce);
                        Destroy(Temporary_Bullet_Handler, 2.0f);
                        break;
                    }
                case SwordActive.terra:
                    {
                        GameObject projectile = Instantiate(swordUI.TerraPowerPrefab, swordUI.swordEmitter[3].position, swordUI.TerraPowerPrefab.transform.rotation) as GameObject;
                        //GameObject shockWave = Instantiate(terraShockWave, transform.position, transform.rotation, transform) as GameObject;
                        Destroy(projectile, 3);
                       
                        break;
                    }
            }

        }
        else if (isTerraIdolActive)
        {
            switch (swordtype)
            {
                case SwordActive.terra:
                    {
                        GameObject projectile = Instantiate(swordUI.TerraPowerPrefab, swordUI.swordEmitter[3].position, swordUI.TerraPowerPrefab.transform.rotation) as GameObject;
                        Destroy(projectile, 3);
                        break;
                    }
            }
        }
    }
    public void SwordParticleEmissions()
    {
        if (flareSwordEnabled)
        {
            var emission = swordUI.flarePartSys.emission;
            flareEmis = swordChargeAmt * 2;
            emission.rateOverTime = flareEmis;
        }
        else if (lightningSwordEnabled)
        {
            var emission = swordUI.lightningPartSys.emission;
            lightningEmis = swordChargeAmt * 5;
            emission.rateOverTime = lightningEmis;
        }
        else if (frostSwordEnabled)
        {
            var emission = swordUI.frostPartSys.emission;
            frostEmis = swordChargeAmt * 500;
            emission.rateOverTime = frostEmis;
        }
        else if (terraSwordEnabled)
        {
            var emission = swordUI.terraPartSys.emission;
            terraEmis = swordChargeAmt * 2;
            emission.rateOverTime = terraEmis;
        }
    }
    public void ActivateSwordPower()
    {
        powerTimer = SwordPowerTime();
        StartCoroutine(powerTimer);
    }
    public void ShakeScreen(float Strength)
    {
        camTransform.transform.localPosition = originalPos + UnityEngine.Random.insideUnitSphere * Strength;
    }
    public void LoadSwordSettings(bool unArmed, bool Flare, bool flareEnabled, bool Lightning, bool lightningEnabled, bool Frost, bool frostEnabled, bool Terra, bool terraEnabled)
    {
        UnArmed = unArmed;

        flareSwordPickedUp = Flare;
        lightningSwordPickedUp = Lightning;
        frostSwordPickedUp = Frost;
        terraSwordPickedUp = Terra;

        flareSwordEnabled = flareEnabled;
        lightningSwordEnabled = lightningEnabled;
        frostSwordEnabled = frostEnabled;
        terraSwordEnabled = terraEnabled;

        if (flareSwordEnabled)
            SelectSword(SwordActive.flare);
        else if (lightningSwordEnabled)
            SelectSword(SwordActive.lightning);
        else if (frostSwordEnabled)
            SelectSword(SwordActive.frost);
        else if (terraSwordEnabled)
            SelectSword(SwordActive.terra);
        else
            SelectSword(SwordActive.mythril);

        if (!UnArmed && !JewelSystem.isJewelEnabled)
        {
            UnArmedObj.SetActive(false);
            ArmedObj.SetActive(true);
        }
        else if(UnArmed && !JewelSystem.isJewelEnabled)
        {
            UnArmedObj.SetActive(true);
            ArmedObj.SetActive(false);
        }
    }
    public void SetupSwordUI()
    {
        if (flareSwordPickedUp)
        {
            swordUI.flareUI.enabled = true;
            swordUI.flareButton.interactable = true;
        } 
        else
        {
            swordUI.flareUI.enabled = false;
            swordUI.flareButton.interactable = false;
        }

        if (lightningSwordPickedUp)
        {
            swordUI.lightningUI.enabled = true;
            swordUI.lightningButton.interactable = true;
        }
        else
        {
            swordUI.lightningUI.enabled = false;
            swordUI.lightningButton.interactable = false;
        }
        if (frostSwordPickedUp)
        {
            swordUI.frostUI.enabled = true;
            swordUI.frostButton.interactable = true;
        }
        else
        {
            swordUI.frostUI.enabled = false;
            swordUI.frostButton.interactable = false;
        }
        if (terraSwordPickedUp)
        {
            swordUI.terraUI.enabled = true;
            swordUI.terraButton.interactable = true;
        }
        else
        {
            swordUI.terraUI.enabled = false;
            swordUI.terraButton.interactable = false;
        }
    }
    public void ResetSwords()
    {
        SelectSword(SwordActive.mythril);
        SheathSword(false);
        flareSwordPickedUp = false;
        lightningSwordPickedUp = false;
        frostSwordPickedUp = false;
        terraSwordPickedUp = false;
        SetupSwordUI();
        MagicIdolUsed(false);
        isTerraIdolActive = false;
        isEarthQuake = false;
        SwordPowerActive = false;
        if (powerTimer != null)
            StopCoroutine(powerTimer);
        powerTimer = null;
    }
    public void GetSword(SwordActive type)
    {
        switch (type)
        {
            case SwordActive.flare:
                {
                    flareSwordPickedUp = true;
                    SelectSword(SwordActive.flare);
                    break;
                }
            case SwordActive.lightning:
                {
                    lightningSwordPickedUp = true;
                    SelectSword(SwordActive.lightning);
                    break;
                }
            case SwordActive.frost:
                {
                    frostSwordPickedUp = true;
                    SelectSword(SwordActive.frost);
                    break;
                }
            case SwordActive.terra:
                {
                    terraSwordPickedUp = true;
                    SelectSword(SwordActive.terra);
                    break;
                }
        }
        SetupSwordUI();
    }
}
[Serializable]
public struct SwordUI
{
    public Image statusSwordIcon;
    public Text statusSwordText;
    public Image mythrilUI;
    public Image flareUI;
    public Image lightningUI;
    public Image frostUI;
    public Image terraUI;

    public Button mythrilButton;
    public Button flareButton;
    public Button lightningButton;
    public Button frostButton;
    public Button terraButton;

    public GameObject FlarePowerPrefab;
    public GameObject LightningPowerPrefab;
    public GameObject FrostPowerPrefab;
    public GameObject TerraPowerPrefab;
    public Transform[] swordEmitter;

    public ParticleSystem flarePartSys;
    public ParticleSystem lightningPartSys;
    public ParticleSystem frostPartSys;
    public ParticleSystem terraPartSys;

    public AudioClip[] iceClips;

}
