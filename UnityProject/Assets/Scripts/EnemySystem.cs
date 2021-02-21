using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using System;
public enum EnemyName { GreyBubble, BlueBubble, PinkBubble, RedBubble,
                        YellowBubble, GreenBubble, PurpleBubble, GreenMushroom,
                        RedMushroom, GreyMushroom, BlueStatue, SilverStatue, GoldStatue,
                        WhiteSkeleton, GreenSkeleton, RedSkeleton, DarkSkeleton,
                        BlueMimic, GreenMimic, OrangeMimic, GoliathToad, ChameleonToad,
                        DevilToad, GraveYard, Cemetary, BurialGrounds };
public class EnemySystem : MonoBehaviour, ISwordHittable
{
    Animator anim;
    AudioClip enemySound;
    GameObject player;
    Text Name;
    Image healthBar;
    Image healthBarMask;
    ImpactReceiver enemyImpact;
    Transform NameObj;
    Transform healthBarObj;
    Transform healthBarMaskObj;
    GameObject newStatus;
    GameObject enemyCanvas;
    PlayerSystem playST;
    ImpactReceiver impact;
    GameObject goldPrefab;
    GameObject itemPrefab;
    Color orgColor;
    IEnumerator musicRoutine;
    Vector3 playerPosition;
    ParticleSystem sprayPS;
    GameObject elementSystem;
    float animSpeed;

    bool isBurned;
    int burnDamageCounter;
    float burnTimer = 3f;
    float burnTime;

    bool isShocked;
    float shockTimer = 3f;
    float shockTime;

    public bool isNavActive;
    bool isFrozen;
    float frozeTimer = 5f;
    float frozeTime;
    int healthMax;
    int enemyHealth;
    public int attackState;
    public int currentAttackState;
    bool flashEnemy;
    float flashTimer = 0.1f;
    float flashTime;
    bool isDying;
    float Speed;
    bool isHidden = false;
    int randomEffect;
    bool lifeBerryEmpty;
    bool statusActive;
    bool isMushroom;
    bool isBubble;
    bool isStatue;
    bool isSkeleton;
    Material orgToadMaterial;
    bool setCloak;
    bool setDemon;
    IEnumerator quake;
    bool permanentDark;
    Color orgToadEyeColor;
    bool isMusic;
    public bool isMimic;
    public bool isToad;
    public bool isGraveYard;
    [Header("Enemy Assignment")]
    public EnemySpecial enemySpecial;
    public EnemyName enemyName;
    public MeshRenderer[] meshRends;
    public EnemySoundClips soundClips;
    public ItemDrops dropItem;
    public GameObject Enemy;
    public GameObject EnemyStatusPrefab;

    public NavMeshAgent nav;
    public GameObject AnimatorObj;
    public GameObject statusObj;
    public GameObject onFirePrefab;
    public GameObject onElectricPrefab;
    public GameObject onIcePrefab;
    [Space]
    [Header("Enemy Settings")]
    public bool isOnOffMeshLink;
    public bool MoveAcrossNavMeshesStarted;
    public float navLinkSpeed;
    public float fadePerSecond;
    public int idlestate;
    public float idleDuration = 2;
    public float attackDuration = 2;
    public float damping;
    public bool isKnockedBack;
    public bool isIdle = false;
    public bool isStop = true;
    public bool isAttacking = false;
    public bool isAttacked;
    public bool attackFinished;
    public float awarenessRange;
    public float attacknessRange;
    public float hidingRange;
    public bool isDarkEnemy;
    public bool isLightEnemy;
    bool darkEnemyActive;
    float velForce = 1f;
    float knockBackTime = 0.4f;
    public float knockBackTimer;
    float maxKnockBackVel = 65f;

    [HideInInspector]
    public bool greyBubActive;
    [HideInInspector]
    public bool blueBubActive;
    [HideInInspector]
    public bool pinkBubActive;
    [HideInInspector]
    public bool redBubActive;
    [HideInInspector]
    public bool yellowBubActive;
    [HideInInspector]
    public bool greenBubActive;
    [HideInInspector]
    public bool purpleBubActive;
    [HideInInspector]
    public bool greenMushActive;
    [HideInInspector]
    public bool redMushActive;
    [HideInInspector]
    public bool greyMushActive;
    [HideInInspector]
    public bool blueStatueActive;
    [HideInInspector]
    public bool silverStatueActive;
    [HideInInspector]
    public bool goldStatueActive;
    [HideInInspector]
    public bool whiteSkeletonActive;
    [HideInInspector]
    public bool greenSkeletonActive;
    [HideInInspector]
    public bool redSkeletonActive;
    [HideInInspector]
    public bool darkSkeletonActive;
    [HideInInspector]
    public bool blueMimicActive;
    [HideInInspector]
    public bool greenMimicActive;
    [HideInInspector]
    public bool orangeMimicActive;
    [HideInInspector]
    public bool goliathToadActive;
    [HideInInspector]
    public bool chameleonToadActive;
    [HideInInspector]
    public bool devilToadActive;
    [HideInInspector]
    public bool graveYardActive;
    [HideInInspector]
    public bool cemetaryActive;
    [HideInInspector]
    public bool burialGroundsActive;
    [Space]
    [Header("Mushroom Stuff")]
    public GameObject MushParticleSys;
    [Space]
    [Header("Statue Stuff")]
    public SphereCollider handOne;
    public SphereCollider handTwo;
    public bool isReal;
    public bool isIndestructible;
    [Space]
    [Header("Skeleton Stuff")]
    public BoxCollider sword;
    public BoxCollider shield;
    [Space]
    [Header("Mimic Stuff")]
    public Material[] mimicMaterials;
    public MeshRenderer eyeObj;
    public BoxCollider mimicHands;
    public bool mimicActive = false;
    Color orgEyeColor;
    [Space]
    [Header("Toad Stuff")]
    public Material[] toadMaterials;
    public MeshRenderer[] toadEyeObjs;
    public MeshRenderer[] toadBodyRenders;
    public SphereCollider toadTongue;
    public CapsuleCollider toptoadCol;
    public Material demonToadMaterial;
    public bool isCloaked;
    public GameObject shockwavePrefab;
    [Space]
    [Header("GraveYard Stuff")]
    public GameObject[] ghostPrefab;
    public Transform[] spawnPoints;
    float spawnTimer = 0.1f;
    public float spawnTime;
    GameObject instantGhost;
    GameObject ghostPool;



    public enum EnemySpecial { None, Dark, Light }
    public void SwitchSpecialStatus(EnemySpecial type)
    {
        switch (type)
        {
            case EnemySpecial.None:
                {
                    isDarkEnemy = false;
                    isLightEnemy = false;
                    break;
                }
            case EnemySpecial.Dark:
                {
                    permanentDark = true;
                    isDarkEnemy = true;
                    isLightEnemy = false;
                    break;
                }
            case EnemySpecial.Light:
                {
                    isDarkEnemy = false;
                    isLightEnemy = true;
                    break;
                }

        }
    }
    void Start()
    {
        anim = AnimatorObj.GetComponent<Animator>();
        flashTime = flashTimer;
        nav.speed = Speed;
        SwitchSpecialStatus(enemySpecial);
        SetUpEnemy();
        statusActive = false;
        knockBackTimer = knockBackTime;
        ghostPool = transform.GetChild(0).gameObject;
        spawnTimer = spawnTime;
    }
    void Update()
    {

        if (darkEnemyActive && !JewelSystem.purpleJewelEnabled && !permanentDark || isDarkEnemy && !JewelSystem.isJewelActive && !permanentDark)
        {
            isDarkEnemy = false;
            SetUpEnemy();
            if (statusActive)
                SetupEnemyStatus(true);
            darkEnemyActive = false;
        }

        if (isToad)
        {
            if (!setCloak && isCloaked)
            {
                SetDemonToad(false);
                ChangeDemonToadMaterial(true);
                setCloak = true;
            }
            else if (setCloak && !isCloaked)
            {
                SetDemonToad(true);
                ChangeDemonToadMaterial(false);
                setCloak = false;
            }

            if (!ItemSystem.demonMaskEnabled && isCloaked && setDemon)
            {
                SetDemonToad(false);
                setDemon = false;
            }
            else if (ItemSystem.demonMaskEnabled && isCloaked && !setDemon)
            {
                SetDemonToad(true);
                setDemon = true;
            }
            else if (!isCloaked && setDemon)
            {
                SetDemonToad(true);
                setDemon = false;
            }
        }
        if (isKnockedBack && isReal && !isGraveYard)
        {
            if (knockBackTimer > 0)
            {
                knockBackTimer -= Time.deltaTime;
                isNavActive = false;
                nav.updatePosition = false;
                Vector3 curPos = transform.parent.position;

                Rigidbody enemyRb = GetComponent<Rigidbody>();
                enemyRb.detectCollisions = false;

                Rigidbody rb = transform.parent.GetComponent<Rigidbody>();
                enemyRb.transform.localPosition = Vector3.zero;
                enemyRb.transform.localRotation = Quaternion.identity;
                rb.useGravity = true;
                rb.isKinematic = false;

                rb.constraints = RigidbodyConstraints.None;
                rb.constraints = RigidbodyConstraints.FreezePositionY;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                Vector3 dir = curPos - GameObject.FindGameObjectWithTag("Player").transform.position;
                rb.AddForce(dir * velForce, ForceMode.Impulse);
                nav.velocity = rb.velocity;
                nav.nextPosition = rb.position;
                if (rb.velocity.magnitude > maxKnockBackVel)
                {
                    rb.useGravity = false;
                    rb.isKinematic = true;
                    enemyRb.detectCollisions = true;
                    enemyRb.transform.localPosition = Vector3.zero;
                    enemyRb.transform.localRotation = Quaternion.identity;
                    isNavActive = true;
                    nav.updatePosition = true;
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    rb.velocity = Vector3.zero;
                    nav.velocity = rb.velocity;
                    knockBackTimer = knockBackTime;
                    isKnockedBack = false;
                }
            }
            else if (knockBackTimer < 0)
            {
                Rigidbody enemyRb = GetComponent<Rigidbody>();
                enemyRb.detectCollisions = true;

                isNavActive = true;
                nav.updatePosition = true;

                Rigidbody rb = transform.parent.GetComponent<Rigidbody>();
                enemyRb.transform.localPosition = Vector3.zero;
                enemyRb.transform.localRotation = Quaternion.identity;
                rb.useGravity = false;
                rb.isKinematic = true;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                rb.velocity = Vector3.zero;
                nav.velocity = rb.velocity;
                knockBackTimer = knockBackTime;
                isKnockedBack = false;
            }
        }
        if (!isIdle && !isFrozen && isReal && !PlayerSystem.isDead && !isGraveYard)
        {
            if (isNavActive && !isAttacked)
            {
                playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
                if (playerPosition != null)
                {
                    nav.isStopped = false;
                    nav.speed = Speed;
                    var lookPos = playerPosition - transform.position;
                    lookPos.y = 0;
                    var rotation = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 8);
                    nav.destination = playerPosition;
                }
            }
            else if (!isNavActive && !isAttacked)
            {
                nav.isStopped = true;
                nav.speed = 0;
                nav.destination = transform.position;
            }
            //===========================Skeleton&ToadOnly==================================//
            else if (isAttacked)
            {
                playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
                nav.isStopped = false;
                nav.speed = Speed * 2;
                var lookPos = playerPosition - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 8);
                nav.destination = playerPosition;
            }
        }
        else if (isFrozen)
        {
            if (frozeTime > 0)
            {
                nav.isStopped = true;
                nav.speed = 0;
                frozeTime -= Time.deltaTime;
            }
            else if (frozeTime < 0)
            {
                ElementDamage(Element.ice, false);
                SetAnimation(EnemyAnimation.None, false);
                //anim.Rebind();
                if (isMimic)
                    FlashEnemy(orgColor, orgEyeColor);
                else if (isToad)
                    FlashEnemy(orgColor, orgToadEyeColor);
                else
                    FlashEnemy(orgColor, Color.clear);
            }
        }
        if (isBurned)
        {
            if (burnDamageCounter > 0)
            {
                if (burnTime > 0)
                    burnTime -= Time.deltaTime;
                else if (burnTime < 0)
                {
                    DamageEnemy(1);
                    burnDamageCounter--;
                    burnTime = burnTimer;
                }
            }
            else
                ElementDamage(Element.fire, false);
        }
        if (isShocked)
        {
            if (shockTime > 0)
                shockTime -= Time.deltaTime;
            else if (shockTime < 0)
                ElementDamage(Element.electric, false);
        }
        if (isReal && nav.isOnOffMeshLink && !MoveAcrossNavMeshesStarted)
        {
            StartCoroutine(MoveAcrossNavMeshLink());
            MoveAcrossNavMeshesStarted = true;
        }
        if (statusActive && newStatus != null)
        {
            if (Camera.main != null)
            {
                Vector3 statusPos = Camera.main.WorldToScreenPoint(statusObj.transform.position);
                newStatus.transform.position = statusPos;
                newStatus.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        if (!SceneSystem.isDisabled && !PlayerSystem.isDead && isReal)
        {
            if (!isIndestructible)
            {
                handler();
                if (flashEnemy)
                {
                    if (flashTime > 0)
                    {
                        flashTime -= Time.unscaledDeltaTime;
                        if (isBubble)
                        {
                            if (redBubActive)
                                FlashEnemy(Color.white, Color.clear);
                            else
                                FlashEnemy(Color.red, Color.clear);
                        }
                        else
                        {
                            if (isMimic || isToad)
                                FlashEnemy(Color.red, Color.red);
                            else
                                FlashEnemy(Color.red, Color.clear);
                        }
                    }
                    else if (flashTime < 0)
                    {
                        if (isBubble)
                        {
                            if (isFrozen)
                                if (blueBubActive)
                                    FlashEnemy(Color.blue, Color.clear);
                                else
                                    FlashEnemy(Color.cyan, Color.clear);
                            else
                                FlashEnemy(orgColor, Color.clear);
                        }
                        else if (isMushroom)
                        {
                            if (isFrozen)
                                FlashEnemy(Color.cyan, Color.clear);
                            else
                                FlashEnemy(orgColor, Color.clear);

                        }
                        else if (isStatue)
                        {
                            if (isFrozen)
                                FlashEnemy(Color.cyan, Color.clear);
                            else
                                FlashEnemy(orgColor, Color.clear);
                        }
                        else if (isSkeleton)
                        {
                            if (isFrozen)
                                FlashEnemy(Color.cyan, Color.clear);
                            else
                                FlashEnemy(orgColor, Color.clear);
                        }
                        else if (isMimic)
                        {
                            if (isFrozen)
                                FlashEnemy(Color.cyan, Color.cyan);
                            else
                                FlashEnemy(orgColor, orgEyeColor);
                        }
                        else if (isToad)
                        {
                            if (isFrozen)
                                FlashEnemy(Color.cyan, Color.cyan);
                            else
                                FlashEnemy(orgColor, orgToadEyeColor);
                        }
                        else if (isGraveYard)
                        {
                            if (isFrozen)
                                FlashEnemy(Color.cyan, Color.cyan);
                            else
                                FlashEnemy(orgColor, Color.clear);
                        }
                        flashTime = flashTimer;
                        flashEnemy = false;
                    }
                }
                if (isDying)
                {
                    anim.StopPlayback();
                    NavActive(0);
                    FadeEnemy();
                }
            }
            if (!isFrozen)
            {
                playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
                //===============EnemyStopped========================//
                if (!isAttacking && isStop && !isIdle && !isDying)
                    SetMovement(EnemyMovement.Stop);
                //===============EnemyIdle==========================//
                else if (!isAttacking && !isStop && isIdle && !isDying && !isHidden && !isGraveYard)
                    SetMovement(EnemyMovement.Idle);
                //===============EnemyAttacking=====================//
                else if (isAttacking && !isStop && !isIdle && !isDying)
                    SetMovement(EnemyMovement.Attack);
                //===============EnemyMoving========================//
                else if (!isAttacking && !isStop && !isIdle && !isDying && !isHidden && !isGraveYard)
                    SetMovement(EnemyMovement.Move);
                //==================================SetPlayerRadius=============================================================================//
                if (!isDying && Vector3.Magnitude(gameObject.transform.position - playerPosition) > hidingRange && !isGraveYard)
                {
                    isStop = true;
                    isIdle = false;
                    isAttacking = false;
                }
                else if (Vector3.Magnitude(gameObject.transform.position - playerPosition) <= hidingRange && Vector3.Magnitude(gameObject.transform.position - playerPosition) > awarenessRange && !isGraveYard)
                {

                    if (statusActive && newStatus != null)
                        SetupEnemyStatus(false);
                    isStop = false;
                    isIdle = true;
                    isAttacking = false;
                }
                else if (Vector3.Magnitude(gameObject.transform.position - playerPosition) <= awarenessRange && Vector3.Magnitude(gameObject.transform.position - playerPosition) > attacknessRange && !isGraveYard)
                {
                    if (!statusActive && !isHidden)
                        SetupEnemyStatus(true);
                    else if (statusActive && !isDying)
                    {
                        if (Name != null)
                            Name.enabled = true;
                        if (healthBarMask != null)
                            healthBarMask.enabled = true;
                        if (healthBar != null)
                            healthBar.enabled = true;
                    }
                    if (isToad && !isMusic && !isDying)
                    {
                        soundClips.musicAudioSrc = GameObject.Find("Core/GameMusic&Sound/GameMusic").GetComponent<AudioSource>();
                        soundClips.currentTempleMusic = soundClips.musicAudioSrc.clip;
                        soundClips.musicAudioSrc.clip = soundClips.bossTemple1Music;
                        musicRoutine = AudioFadeIn(soundClips.musicAudioSrc);
                        StartCoroutine(musicRoutine);
                        isMusic = true;
                    }
                    isAttacking = false;
                    isIdle = false;
                    isStop = false;
                }
                else if (Vector3.Magnitude(gameObject.transform.position - playerPosition) <= attacknessRange)
                {
                    if (!statusActive && !isHidden)
                        SetupEnemyStatus(true);
                    isAttacking = true;
                    isIdle = false;
                    isStop = false;
                }
            }
            else if (isFrozen)
                SetMovement(EnemyMovement.frozen);

        }
        else if (!isReal || PlayerSystem.isDead)
        {
            SetMovement(EnemyMovement.Stop);

        }
    }
    public void FlashEnemy(Color type, Color type2)
    {
        if (isBubble)
        {
            for (int r = 0; r < meshRends.Length; r++)
            {
                for (int m = 0; m < meshRends[r].materials.Length; m++)
                {
                    SetupMaterialWithBlendMode(meshRends[r].materials[m], BlendMode.Fade);
                    if (meshRends[r].materials[m].name == "BubbleEnemyClear (Instance)")
                        meshRends[r].materials[m].color = type;
                }
            }
        }
        else if (isMushroom)
        {
            for (int r = 0; r < meshRends.Length; r++)
            {
                for (int m = 0; m < meshRends[r].materials.Length; m++)
                {
                    if (meshRends[r].materials[m].name == "Base (Instance)")
                        meshRends[r].materials[m].color = type;
                }
            }
        }
        else if (isStatue)
        {
            for (int r = 0; r < meshRends.Length; r++)
            {
                for (int m = 0; m < meshRends[r].materials.Length; m++)
                {
                    if (meshRends[r].materials[m].name == "StatueColor (Instance)")
                        meshRends[r].materials[m].color = type;
                }
            }
        }
        else if (isSkeleton)
        {
            for (int r = 0; r < meshRends.Length; r++)
            {
                for (int m = 0; m < meshRends[r].materials.Length; m++)
                {
                    if (meshRends[r].materials[m].name == "Bones (Instance)")
                        meshRends[r].materials[m].color = type;
                }
            }
        }
        else if (isMimic)
        {
            for (int r = 0; r < meshRends.Length; r++)
            {
                for (int m = 0; m < meshRends[r].materials.Length; m++)
                {
                    if (meshRends[r].materials[m].name == "LegsArms (Instance)")
                        meshRends[r].materials[m].color = type;

                    if (blueMimicActive)
                    {
                        if (meshRends[r].materials[m].name == "EyeLvl1 (Instance)")
                            meshRends[r].materials[m].color = type2;
                    }
                    else if (greenMimicActive)
                    {
                        if (meshRends[r].materials[m].name == "EyeLvl2 (Instance)")
                            meshRends[r].materials[m].color = type2;
                    }
                    else if (orangeMimicActive)
                    {
                        if (meshRends[r].materials[m].name == "EyeLvl3 (Instance)")
                            meshRends[r].materials[m].color = type2;
                    }
                }
            }
        }
        else if (isToad)
        {
            for (int r = 0; r < meshRends.Length; r++)
            {
                for (int m = 0; m < meshRends[r].materials.Length; m++)
                {
                    if (meshRends[r].materials[m].name == "ToadLegsArms (Instance)")
                        meshRends[r].materials[m].color = type;

                    if (goliathToadActive)
                    {
                        if (meshRends[r].materials[m].name == "FrogEye (Instance)")
                            meshRends[r].materials[m].color = type2;
                    }
                    else if (chameleonToadActive)
                    {
                        if (meshRends[r].materials[m].name == "FrogEye 1 (Instance)")
                            meshRends[r].materials[m].color = type2;
                    }
                    else if (devilToadActive)
                    {
                        if (meshRends[r].materials[m].name == "FrogEye 2 (Instance)")
                            meshRends[r].materials[m].color = type2;
                    }
                }
            }
        }
        else if (isGraveYard)
        {
            for (int r = 0; r < meshRends.Length; r++)
            {
                for (int m = 0; m < meshRends[r].materials.Length; m++)
                {
                    if (meshRends[r].materials[m].name == "Bones (Instance)")
                        meshRends[r].materials[m].color = type;
                }
            }
        }
    }
    public void FadeEnemy()
    {
        if (isToad)
        {
            for (int r = 0; r < toadBodyRenders.Length; r++)
            {
                for (int m = 0; m < toadBodyRenders[r].materials.Length; m++)
                {
                    SetupMaterialWithBlendMode(toadBodyRenders[r].materials[m], BlendMode.Fade);
                    Color color = toadBodyRenders[r].materials[m].color;
                    toadBodyRenders[r].materials[m].color = new Color(color.r, color.g, color.b, color.a - (fadePerSecond * Time.deltaTime));
                    if (toadBodyRenders[toadBodyRenders.Length - 1].materials[toadBodyRenders[toadBodyRenders.Length - 1].materials.Length - 1].color.a < 0.15f)
                    {
                        Destroy(newStatus);
                        Destroy(Enemy.gameObject, 1);
                    }
                }

            }
        }
        else
        {
            for (int r = 0; r < meshRends.Length; r++)
            {
                for (int m = 0; m < meshRends[r].materials.Length; m++)
                {
                    SetupMaterialWithBlendMode(meshRends[r].materials[m], BlendMode.Fade);
                    Color color = meshRends[r].materials[m].color;
                    meshRends[r].materials[m].color = new Color(color.r, color.g, color.b, color.a - (fadePerSecond * Time.deltaTime));
                    if (meshRends[meshRends.Length - 1].materials[meshRends[meshRends.Length - 1].materials.Length - 1].color.a < 0.15f)
                    {
                        Destroy(newStatus);
                        if (!isMimic)
                            Destroy(Enemy.gameObject);
                        else
                            Destroy(Enemy.gameObject, 1);
                    }
                }

            }
        }

    }
    public IEnumerator MoveAcrossNavMeshLink()
    {
        OffMeshLinkData data = nav.currentOffMeshLinkData;
        nav.updateRotation = false;

        Vector3 startPos = nav.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * nav.baseOffset;
        float duration = (endPos - startPos).magnitude / nav.velocity.magnitude;
        float t = 0.0f;
        float tStep = 2.0f / duration;
        while (t < 1.0f)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t);
            nav.destination = transform.position;
            t += tStep * Time.deltaTime * 13f;
            yield return null;
        }
        transform.position = endPos;
        nav.updateRotation = true;
        nav.CompleteOffMeshLink();
        MoveAcrossNavMeshesStarted = false;

    }
    public enum EnemyAnimation { Damage, Move, Jump, Idle, Hide, UnHide, Attack, Attack2, Attack3, Attack4, Defend, None }
    public void SetAnimation(EnemyAnimation anim, bool True)
    {
        if (True)
        {
            switch (anim)
            {
                case EnemyAnimation.Damage:
                    {
                        this.anim.SetTrigger("Damage");
                        break;
                    }
                case EnemyAnimation.Move:
                    {
                        if (greyBubActive)
                            this.anim.SetFloat("MoveSpeed", 1.1f);
                        else if (blueBubActive)
                            this.anim.SetFloat("MoveSpeed", 1.2f);
                        else if (pinkBubActive)
                            this.anim.SetFloat("MoveSpeed", 1.3f);
                        else if (redBubActive)
                            this.anim.SetFloat("MoveSpeed", 1.4f);
                        else if (yellowBubActive)
                            this.anim.SetFloat("MoveSpeed", 1.5f);
                        else if (greenBubActive)
                            this.anim.SetFloat("MoveSpeed", 1.6f);
                        else if (purpleBubActive)
                            this.anim.SetFloat("MoveSpeed", 1.6f);

                        else if (greenMushActive)
                            this.anim.SetFloat("MoveSpeed", 1.2f);
                        else if (redMushActive)
                            this.anim.SetFloat("MoveSpeed", 1.5f);
                        else if (greyMushActive)
                            this.anim.SetFloat("MoveSpeed", 2f);

                        else if (blueStatueActive)
                            this.anim.SetFloat("MoveSpeed", 2f);
                        else if (silverStatueActive)
                            this.anim.SetFloat("MoveSpeed", 2.5f);
                        else if (goldStatueActive)
                            this.anim.SetFloat("MoveSpeed", 3f);

                        else if (whiteSkeletonActive)
                            this.anim.SetFloat("MoveSpeed", 2f);
                        else if (greenSkeletonActive)
                            this.anim.SetFloat("MoveSpeed", 2.2f);
                        else if (redSkeletonActive)
                            this.anim.SetFloat("MoveSpeed", 2.4f);
                        else if (darkSkeletonActive)
                            this.anim.SetFloat("MoveSpeed", 2.6f);

                        else if (blueMimicActive)
                            this.anim.SetFloat("MoveSpeed", 2f);
                        else if (greenMimicActive)
                            this.anim.SetFloat("MoveSpeed", 2.5f);
                        else if (orangeMimicActive)
                            this.anim.SetFloat("MoveSpeed", 3f);

                        else if (goliathToadActive)
                            this.anim.SetFloat("MoveSpeed", 1f);
                        else if (chameleonToadActive)
                            this.anim.SetFloat("MoveSpeed", 1.1f);
                        else if (devilToadActive)
                            this.anim.SetFloat("MoveSpeed", 1.2f);



                        this.anim.SetBool("Move", true);
                        break;
                    }
                case EnemyAnimation.Jump:
                    {
                        if (greyBubActive)
                            this.anim.SetFloat("MoveSpeed", 1f);
                        else if (blueBubActive)
                            this.anim.SetFloat("MoveSpeed", 1.1f);
                        else if (pinkBubActive)
                            this.anim.SetFloat("MoveSpeed", 1.2f);
                        else if (redBubActive)
                            this.anim.SetFloat("MoveSpeed", 1.3f);
                        else if (yellowBubActive)
                            this.anim.SetFloat("MoveSpeed", 1.4f);
                        else if (greenBubActive)
                            this.anim.SetFloat("MoveSpeed", 1.5f);
                        else if (purpleBubActive)
                            this.anim.SetFloat("MoveSpeed", 1.6f);

                        this.anim.SetBool("Jump", true);
                        break;
                    }
                case EnemyAnimation.Idle:
                    {
                        this.anim.SetBool("Idle", true);
                        break;
                    }
                case EnemyAnimation.Hide:
                    {
                        this.anim.SetBool("Hide", true);
                        break;
                    }
                case EnemyAnimation.UnHide:
                    {
                        this.anim.SetBool("UnHide", true);
                        break;
                    }
                case EnemyAnimation.Attack:
                    {

                        if (greenMushActive)
                            this.anim.SetFloat("MoveSpeed", 1f);
                        else if (redMushActive)
                            this.anim.SetFloat("MoveSpeed", 1.25f);
                        else if (greyMushActive)
                            this.anim.SetFloat("MoveSpeed", 1.5f);

                        else if (blueStatueActive)
                            this.anim.SetFloat("MoveSpeed", 1f);
                        else if (silverStatueActive)
                            this.anim.SetFloat("MoveSpeed", 1.5f);
                        else if (goldStatueActive)
                            this.anim.SetFloat("MoveSpeed", 2f);

                        else if (whiteSkeletonActive)
                            this.anim.SetFloat("MoveSpeed", 1f);
                        else if (greenSkeletonActive)
                            this.anim.SetFloat("MoveSpeed", 1.1f);
                        else if (redSkeletonActive)
                            this.anim.SetFloat("MoveSpeed", 1.2f);
                        else if (darkSkeletonActive)
                            this.anim.SetFloat("MoveSpeed", 1.3f);

                        else if (blueMimicActive)
                            this.anim.SetFloat("MoveSpeed", 1.5f);
                        else if (greenMimicActive)
                            this.anim.SetFloat("MoveSpeed", 1.7f);
                        else if (orangeMimicActive)
                            this.anim.SetFloat("MoveSpeed", 1.9f);

                        else if (goliathToadActive)
                            this.anim.SetFloat("MoveSpeed", 1f);
                        else if (chameleonToadActive)
                            this.anim.SetFloat("MoveSpeed", 1.25f);
                        else if (devilToadActive)
                            this.anim.SetFloat("MoveSpeed", 1.5f);

                        this.anim.SetBool("Attack", true);
                        break;
                    }
                case EnemyAnimation.Attack2:
                    {
                        if (whiteSkeletonActive)
                            this.anim.SetFloat("MoveSpeed", 1f);
                        else if (greenSkeletonActive)
                            this.anim.SetFloat("MoveSpeed", 1.1f);
                        else if (redSkeletonActive)
                            this.anim.SetFloat("MoveSpeed", 1.2f);
                        else if (darkSkeletonActive)
                            this.anim.SetFloat("MoveSpeed", 1.3f);

                        else if (goliathToadActive)
                            this.anim.SetFloat("MoveSpeed", 1f);
                        else if (chameleonToadActive)
                            this.anim.SetFloat("MoveSpeed", 1.1f);
                        else if (devilToadActive)
                            this.anim.SetFloat("MoveSpeed", 1.2f);
                        this.anim.SetBool("Attack2", true);
                        break;
                    }
                case EnemyAnimation.Attack3:
                    {
                        if (goliathToadActive)
                            this.anim.SetFloat("MoveSpeed", 1f);
                        else if (chameleonToadActive)
                            this.anim.SetFloat("MoveSpeed", 1.25f);
                        else if (devilToadActive)
                            this.anim.SetFloat("MoveSpeed", 1.5f);
                        this.anim.SetBool("Attack3", true);
                        break;
                    }
                case EnemyAnimation.Attack4:
                    {
                        if (goliathToadActive)
                            this.anim.SetFloat("MoveSpeed", 1f);
                        else if (chameleonToadActive)
                            this.anim.SetFloat("MoveSpeed", 1.25f);
                        else if (devilToadActive)
                            this.anim.SetFloat("MoveSpeed", 1.5f);

                        this.anim.SetBool("Attack4", true);
                        break;
                    }
                case EnemyAnimation.Defend:
                    {
                        if (whiteSkeletonActive)
                            this.anim.SetFloat("MoveSpeed", 1f);
                        else if (greenSkeletonActive)
                            this.anim.SetFloat("MoveSpeed", 1.1f);
                        else if (redSkeletonActive)
                            this.anim.SetFloat("MoveSpeed", 1.2f);
                        else if (darkSkeletonActive)
                            this.anim.SetFloat("MoveSpeed", 1.3f);


                        this.anim.SetBool("Defend", true);
                        break;
                    }
                case EnemyAnimation.None:
                    {
                        this.anim.speed = 0;
                        break;
                    }
            }
        }
        else
        {
            switch (anim)
            {
                case EnemyAnimation.None:
                    {
                        this.anim.speed = animSpeed;
                        break;
                    }

                case EnemyAnimation.Move:
                    {

                        this.anim.SetBool("Move", false);
                        break;
                    }
                case EnemyAnimation.Jump:
                    {
                        this.anim.SetBool("Jump", false);
                        break;
                    }
                case EnemyAnimation.Idle:
                    {
                        this.anim.SetBool("Idle", false);
                        break;
                    }
                case EnemyAnimation.Hide:
                    {
                        this.anim.SetBool("Hide", false);
                        break;
                    }
                case EnemyAnimation.UnHide:
                    {
                        this.anim.SetBool("UnHide", false);
                        break;
                    }
                case EnemyAnimation.Attack:
                    {
                        this.anim.SetBool("Attack", false);
                        break;
                    }
                case EnemyAnimation.Attack2:
                    {
                        this.anim.SetBool("Attack2", false);
                        break;
                    }
                case EnemyAnimation.Attack3:
                    {
                        this.anim.SetBool("Attack3", false);
                        break;
                    }
                case EnemyAnimation.Attack4:
                    {
                        this.anim.SetBool("Attack4", false);
                        break;
                    }
                case EnemyAnimation.Defend:
                    {
                        this.anim.SetBool("Defend", false);
                        break;
                    }

            }
        }
    }
    public void ActivateHealthStatus(EnemyName name)
    {
        switch (name)
        {
            case EnemyName.GreyBubble:
                {
                    isIndestructible = false;

                    isBubble = true;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = false;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = true;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 40;
                        healthMax = 40;
                        Speed = 4;
                    }
                    else
                    {
                        enemyHealth = 2;
                        healthMax = 2;
                        Speed = 3;
                    }
                    isHidden = false;

                    break;
                }
            case EnemyName.BlueBubble:
                {
                    isIndestructible = false;

                    isBubble = true;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = false;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = true;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 80;
                        healthMax = 80;
                        Speed = 5;
                    }
                    else
                    {
                        enemyHealth = 4;
                        healthMax = 4;
                        Speed = 4;
                    }
                    isHidden = false;

                    break;
                }
            case EnemyName.PinkBubble:
                {
                    isIndestructible = false;

                    isBubble = true;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = false;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = true;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 120;
                        healthMax = 120;
                        Speed = 6;
                    }
                    else
                    {
                        enemyHealth = 6;
                        healthMax = 6;
                        Speed = 5;
                    }
                    isHidden = false;
                    break;
                }
            case EnemyName.RedBubble:
                {
                    isIndestructible = false;

                    isBubble = true;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = false;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = true;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 160;
                        healthMax = 160;
                        Speed = 7;
                    }
                    else
                    {
                        enemyHealth = 8;
                        healthMax = 8;
                        Speed = 6;
                    }
                    isHidden = false;

                    break;
                }
            case EnemyName.YellowBubble:
                {
                    isIndestructible = false;

                    isBubble = true;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = false;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = true;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;


                    if (isDarkEnemy)
                    {
                        enemyHealth = 200;
                        healthMax = 200;
                        Speed = 8;
                    }
                    else
                    {
                        enemyHealth = 10;
                        healthMax = 10;
                        Speed = 7;
                    }
                    isHidden = false;

                    break;
                }
            case EnemyName.GreenBubble:
                {
                    isIndestructible = false;

                    isBubble = true;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = false;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = true;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 240;
                        healthMax = 240;
                        Speed = 9;
                    }
                    else
                    {
                        enemyHealth = 12;
                        healthMax = 12;
                        Speed = 8;
                    }

                    isHidden = false;

                    break;
                }
            case EnemyName.PurpleBubble:
                {
                    isIndestructible = false;

                    isBubble = true;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = false;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = true;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 280;
                        healthMax = 280;
                        Speed = 10;
                    }
                    else
                    {
                        enemyHealth = 14;
                        healthMax = 14;
                        Speed = 9;
                    }
                    isHidden = false;
                    break;
                }
            case EnemyName.GreenMushroom:
                {
                    isIndestructible = false;

                    isBubble = false;
                    isMushroom = true;
                    isStatue = false;
                    isSkeleton = false;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = true;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 60;
                        healthMax = 60;
                        Speed = 8;
                    }
                    else
                    {
                        enemyHealth = 3;
                        healthMax = 3;
                        Speed = 6;
                    }
                    isHidden = true;

                    break;
                }
            case EnemyName.RedMushroom:
                {
                    isIndestructible = false;

                    isBubble = false;
                    isMushroom = true;
                    isStatue = false;
                    isSkeleton = false;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = true;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 120;
                        healthMax = 120;
                        Speed = 10;
                    }
                    else
                    {
                        enemyHealth = 6;
                        healthMax = 6;
                        Speed = 8;
                    }
                    isHidden = true;

                    break;
                }
            case EnemyName.GreyMushroom:
                {
                    isIndestructible = false;

                    isBubble = false;
                    isMushroom = true;
                    isStatue = false;
                    isSkeleton = false;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = true;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 180;
                        healthMax = 180;
                        Speed = 12;
                    }
                    else
                    {
                        enemyHealth = 9;
                        healthMax = 9;
                        Speed = 10;
                    }
                    isHidden = true;

                    break;
                }
            case EnemyName.BlueStatue:
                {
                    isIndestructible = false;

                    isBubble = false;
                    isMushroom = false;
                    isStatue = true;
                    isSkeleton = false;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = true;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 400;
                        healthMax = 400;
                        Speed = 7;
                    }
                    else
                    {
                        enemyHealth = 20;
                        healthMax = 20;
                        Speed = 5;
                    }
                    isHidden = false;

                    break;
                }
            case EnemyName.SilverStatue:
                {
                    isIndestructible = false;

                    isBubble = false;
                    isMushroom = false;
                    isStatue = true;
                    isSkeleton = false;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = true;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 600;
                        healthMax = 600;
                        Speed = 9;
                    }
                    else
                    {
                        enemyHealth = 30;
                        healthMax = 30;
                        Speed = 7;
                    }
                    isHidden = false;
                    break;
                }
            case EnemyName.GoldStatue:
                {
                    if (!isDarkEnemy)
                        isIndestructible = true;

                    isBubble = false;
                    isMushroom = false;
                    isStatue = true;
                    isSkeleton = false;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = true;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 800;
                        healthMax = 800;
                        Speed = 11;
                    }
                    else
                    {
                        enemyHealth = 1;
                        healthMax = 1;
                        Speed = 9;
                    }
                    isHidden = false;

                    break;
                }
            case EnemyName.WhiteSkeleton:
                {
                    isIndestructible = false;

                    isBubble = false;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = true;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = true;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 120;
                        healthMax = 120;
                        Speed = 6;
                    }
                    else
                    {
                        enemyHealth = 6;
                        healthMax = 6;
                        Speed = 4;
                    }
                    isHidden = false;

                    break;
                }
            case EnemyName.GreenSkeleton:
                {
                    isIndestructible = false;

                    isBubble = false;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = true;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = true;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 240;
                        healthMax = 240;
                        Speed = 7;
                    }
                    else
                    {
                        enemyHealth = 12;
                        healthMax = 12;
                        Speed = 5;
                    }
                    isHidden = false;

                    break;
                }
            case EnemyName.RedSkeleton:
                {
                    isIndestructible = false;

                    isBubble = false;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = true;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = true;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 360;
                        healthMax = 360;
                        Speed = 8;
                    }
                    else
                    {
                        enemyHealth = 18;
                        healthMax = 18;
                        Speed = 6;
                    }
                    isHidden = false;

                    break;
                }
            case EnemyName.DarkSkeleton:
                {
                    isIndestructible = false;

                    isBubble = false;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = true;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = true;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 480;
                        healthMax = 480;
                        Speed = 9;
                    }
                    else
                    {
                        enemyHealth = 24;
                        healthMax = 24;
                        Speed = 7;
                    }
                    isHidden = false;

                    break;
                }
            case EnemyName.BlueMimic:
                {
                    isIndestructible = true;

                    isBubble = false;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = false;
                    isMimic = true;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = true;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 200;
                        healthMax = 200;
                        Speed = 7;
                    }
                    else
                    {
                        enemyHealth = 10;
                        healthMax = 10;
                        Speed = 5;
                    }
                    //isHidden = false;

                    break;
                }
            case EnemyName.GreenMimic:
                {
                    isIndestructible = true;

                    isBubble = false;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = false;
                    isMimic = true;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = true;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 320;
                        healthMax = 320;
                        Speed = 10;
                    }
                    else
                    {
                        enemyHealth = 16;
                        healthMax = 16;
                        Speed = 8;
                    }
                    //isHidden = false;



                    break;
                }
            case EnemyName.OrangeMimic:
                {
                    isIndestructible = true;

                    isBubble = false;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = false;
                    isMimic = true;
                    isToad = false;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = true;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 400;
                        healthMax = 400;
                        Speed = 12;
                    }
                    else
                    {
                        enemyHealth = 20;
                        healthMax = 20;
                        Speed = 10;
                    }
                    //isHidden = false;



                    break;
                }
            case EnemyName.GoliathToad:
                {
                    isIndestructible = false;

                    isBubble = false;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = false;
                    isMimic = false;
                    isToad = true;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = true;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    attacknessRange = 50f;

                    Speed = 300;

                    isHidden = true;
                    enemyHealth = 80;
                    healthMax = 80;

                    toptoadCol.enabled = false;

                    break;
                }
            case EnemyName.ChameleonToad:
                {
                    isIndestructible = false;

                    isBubble = false;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = false;
                    isMimic = false;
                    isToad = true;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = true;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    attacknessRange = 50f;
                    Speed = 400;

                    isHidden = true;
                    enemyHealth = 200;
                    healthMax = 200;

                    toptoadCol.enabled = false;

                    break;
                }
            case EnemyName.DevilToad:
                {
                    isIndestructible = false;

                    isBubble = false;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = false;
                    isMimic = false;
                    isToad = true;
                    isGraveYard = false;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = true;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    attacknessRange = 50;
                    Speed = 500;

                    isHidden = true;
                    enemyHealth = 300;
                    healthMax = 300;

                    toptoadCol.enabled = false;

                    break;
                }
            case EnemyName.GraveYard:
                {
                    isIndestructible = false;

                    isBubble = false;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = false;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = true;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = true;
                    cemetaryActive = false;
                    burialGroundsActive = false;

                    attacknessRange = 49;
                    Speed = 0;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 400;
                        healthMax = 400;
                        spawnTime = .5f;
                    }
                    else
                    {
                        enemyHealth = 20;
                        healthMax = 20;
                        spawnTime = 2f;
                    }
                   
                    break;
                }
            case EnemyName.Cemetary:
                {
                    isIndestructible = false;

                    isBubble = false;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = false;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = true;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = true;
                    burialGroundsActive = false;

                    attacknessRange = 49;
                    Speed = 0;

                    if (isDarkEnemy)
                    {
                        enemyHealth = 600;
                        healthMax = 600;
                        spawnTime = .5f;
                    }
                    else
                    {
                        enemyHealth = 30;
                        healthMax = 30;
                        spawnTime = 1.5f;
                    }
                 
                    break;
                }
            case EnemyName.BurialGrounds:
                {
                    isIndestructible = true;

                    isBubble = false;
                    isMushroom = false;
                    isStatue = false;
                    isSkeleton = false;
                    isMimic = false;
                    isToad = false;
                    isGraveYard = true;

                    greyBubActive = false;
                    blueBubActive = false;
                    pinkBubActive = false;
                    redBubActive = false;
                    yellowBubActive = false;
                    greenBubActive = false;
                    purpleBubActive = false;

                    greenMushActive = false;
                    redMushActive = false;
                    greyMushActive = false;

                    blueStatueActive = false;
                    silverStatueActive = false;
                    goldStatueActive = false;

                    whiteSkeletonActive = false;
                    greenSkeletonActive = false;
                    redSkeletonActive = false;
                    darkSkeletonActive = false;

                    blueMimicActive = false;
                    greenMimicActive = false;
                    orangeMimicActive = false;

                    goliathToadActive = false;
                    chameleonToadActive = false;
                    devilToadActive = false;

                    graveYardActive = false;
                    cemetaryActive = false;
                    burialGroundsActive = true;

                    attacknessRange = 49;
                    Speed = 0;

                    if (isDarkEnemy)
                    {
                        isIndestructible = false;
                        enemyHealth = 1000;
                        healthMax = 1000;
                        spawnTime = .5f;
                    }
                    else
                    {
                        enemyHealth = 50;
                        healthMax = 50;
                        spawnTime = 1f;
                    }
                  
                    break;
                }
        }

    }
    public void PlayToadSounds(int num)
    {
        //=====jump=====//
        if (num == 1)
        {
            int rand = UnityEngine.Random.Range(0, 5);
            enemySound = soundClips.toadJump[rand];
        }
        //=====Hit=====//
        else if (num == 2)
        {
            int rand = UnityEngine.Random.Range(0, 2);
            enemySound = soundClips.toadHit[rand];
        }
        //=====Spin=====//
        else if (num == 3)
            enemySound = soundClips.toadSpin;
        //=====Shock=====//
        else if (num == 4)
            enemySound = soundClips.toadShock;
        //=====Lick=====//
        else if (num == 5)
            enemySound = soundClips.toadLick;
        else
            enemySound = soundClips.bloop;
        PlaySounds();
      
    }
    public void PlaySounds()
    {
        AudioSystem.PlayAudioSource(enemySound, 0.9f, 1.1f);
    }
    public void SetUpEnemy()
    {
        ActivateHealthStatus(enemyName);
        foreach (MeshRenderer meshRend in meshRends)
        {
            meshRend.GetComponent<MeshRenderer>();
            if (isBubble)
            {
                if (greyBubActive)
                {
                    goldPrefab = dropItem.gold1;
                    enemySound = soundClips.bloop;

                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {

                            if (meshRends[r].materials[m].name == "BubbleEnemyClearOuter (Instance)" ||
                                meshRends[r].materials[m].name == "BubbleEnemyClear (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = Color.grey;
                            }
                        }
                        if (meshRends[r].material.name == "BubbleEnemyClearOuter (Instance)" ||
                                meshRends[r].material.name == "BubbleEnemyClear (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = Color.grey;

                        }
                        if (meshRends[r].material.name == "BubbleEnemyClear (Instance)")
                            orgColor = meshRends[r].material.color;
                    }
                }
                else if (blueBubActive)
                {
                    goldPrefab = dropItem.gold2;
                    enemySound = soundClips.bloop;
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {

                            if (meshRends[r].materials[m].name == "BubbleEnemyClearOuter (Instance)" ||
                                meshRends[r].materials[m].name == "BubbleEnemyClear (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = Color.cyan;
                            }
                        }
                        if (meshRends[r].material.name == "BubbleEnemyClearOuter (Instance)" ||
                                meshRends[r].material.name == "BubbleEnemyClear (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = Color.cyan;
                        }
                        if (meshRends[r].material.name == "BubbleEnemyClear (Instance)")
                            orgColor = meshRends[r].material.color;
                    }
                }
                else if (pinkBubActive)
                {
                    goldPrefab = dropItem.gold2;
                    enemySound = soundClips.bloop;
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {

                            if (meshRends[r].materials[m].name == "BubbleEnemyClearOuter (Instance)" ||
                                meshRends[r].materials[m].name == "BubbleEnemyClear (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = Color.magenta;
                            }
                        }
                        if (meshRends[r].material.name == "BubbleEnemyClearOuter (Instance)" ||
                                meshRends[r].material.name == "BubbleEnemyClear (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = Color.magenta;
                        }

                        if (meshRends[r].material.name == "BubbleEnemyClear (Instance)")
                            orgColor = meshRends[r].material.color;
                    }
                }
                else if (redBubActive)
                {
                    goldPrefab = dropItem.gold3;
                    enemySound = soundClips.bloop;
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {

                            if (meshRends[r].materials[m].name == "BubbleEnemyClearOuter (Instance)" ||
                                meshRends[r].materials[m].name == "BubbleEnemyClear (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = Color.red;
                            }
                        }
                        if (meshRends[r].material.name == "BubbleEnemyClearOuter (Instance)" ||
                                meshRends[r].material.name == "BubbleEnemyClear (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = Color.red;
                        }
                        if (meshRends[r].material.name == "BubbleEnemyClear (Instance)")
                            orgColor = meshRends[r].material.color;
                    }
                }
                else if (yellowBubActive)
                {
                    goldPrefab = dropItem.gold4;
                    enemySound = soundClips.bloop;
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {

                            if (meshRends[r].materials[m].name == "BubbleEnemyClearOuter (Instance)" ||
                                meshRends[r].materials[m].name == "BubbleEnemyClear (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = Color.yellow;
                            }
                        }
                        if (meshRends[r].material.name == "BubbleEnemyClearOuter (Instance)" ||
                                meshRends[r].material.name == "BubbleEnemyClear (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = Color.yellow;
                        }
                        if (meshRends[r].material.name == "BubbleEnemyClear (Instance)")
                            orgColor = meshRends[r].material.color;
                    }
                }
                else if (greenBubActive)
                {
                    goldPrefab = dropItem.gold5;
                    enemySound = soundClips.bloop;
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {

                            if (meshRends[r].materials[m].name == "BubbleEnemyClearOuter (Instance)" ||
                                meshRends[r].materials[m].name == "BubbleEnemyClear (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = Color.green;
                            }
                        }
                        if (meshRends[r].material.name == "BubbleEnemyClearOuter (Instance)" ||
                                meshRends[r].material.name == "BubbleEnemyClear (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = Color.green;
                        }
                        if (meshRends[r].material.name == "BubbleEnemyClear (Instance)")
                            orgColor = meshRends[r].material.color;
                    }
                }
                else if (purpleBubActive)
                {
                    goldPrefab = dropItem.gold6;
                    enemySound = soundClips.bloop;
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {

                            if (meshRends[r].materials[m].name == "BubbleEnemyClearOuter (Instance)" ||
                                meshRends[r].materials[m].name == "BubbleEnemyClear (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = Color.blue;
                            }
                        }
                        if (meshRends[r].material.name == "BubbleEnemyClearOuter (Instance)" ||
                                meshRends[r].material.name == "BubbleEnemyClear (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = Color.blue;
                        }
                        if (meshRends[r].material.name == "BubbleEnemyClear (Instance)")
                            orgColor = meshRends[r].material.color;
                    }
                }

            }
            else if (isMushroom)
            {
                if (greenMushActive)
                {
                    goldPrefab = dropItem.gold2;
                    enemySound = soundClips.spray;
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {

                            if (meshRends[r].materials[m].name == "Top (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = new Color(0.25f, 0.5f, 0, 1);
                            }
                        }
                        if (meshRends[r].material.name == "Top (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = new Color(0.25f, 0.5f, 0, 1);
                        }
                        if (meshRends[r].material.name == "Base (Instance)")
                            orgColor = meshRends[r].material.color;
                    }
                }
                else if (redMushActive)
                {
                    goldPrefab = dropItem.gold4;
                    enemySound = soundClips.spray;
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {

                            if (meshRends[r].materials[m].name == "Top (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 0, 1);
                            }
                        }
                        if (meshRends[r].material.name == "Top (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = new Color(0.5f, 0, 0, 1);
                        }
                        if (meshRends[r].material.name == "Base (Instance)")
                            orgColor = meshRends[r].material.color;
                    }
                }
                else if (greyMushActive)
                {
                    goldPrefab = dropItem.gold6;
                    enemySound = soundClips.spray;
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {

                            if (meshRends[r].materials[m].name == "Top (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = new Color(0.3f, 0.3f, 0.3f, 1);
                            }
                        }
                        if (meshRends[r].material.name == "Top (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = new Color(0.3f, 0.3f, 0.3f, 1);
                        }
                        if (meshRends[r].material.name == "Base (Instance)")
                            orgColor = meshRends[r].material.color;
                    }
                }
            }
            else if (isStatue)
            {
                if (blueStatueActive)
                {
                    goldPrefab = dropItem.gold7;
                    enemySound = null;
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {

                            if (meshRends[r].materials[m].name == "StatueColor (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = new Color(0.15f, 0.35f, 0.45f, 1);
                            }
                        }
                        if (meshRends[r].material.name == "StatueColor (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = new Color(0.15f, 0.35f, 0.45f, 1);
                        }
                        if (meshRends[r].material.name == "StatueColor (Instance)")
                            orgColor = meshRends[r].material.color;
                    }
                }
                else if (silverStatueActive)
                {
                    goldPrefab = dropItem.gold9;
                    enemySound = null;
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {

                            if (meshRends[r].materials[m].name == "StatueColor (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = new Color(0.5f, 0.5f, 0.5f, 1);
                            }
                        }
                        if (meshRends[r].material.name == "StatueColor (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = new Color(0.5f, 0.5f, 0.5f, 1);
                        }
                        if (meshRends[r].material.name == "StatueColor (Instance)")
                            orgColor = meshRends[r].material.color;
                    }
                }
                else if (goldStatueActive)
                {
                    goldPrefab = null;
                    enemySound = null;
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {

                            if (meshRends[r].materials[m].name == "StatueColor (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = new Color(1f, 0.5f, 0f, 1);
                            }
                        }
                        if (meshRends[r].material.name == "StatueColor (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = new Color(1f, 0.5f, 0f, 1);
                        }
                        if (meshRends[r].material.name == "StatueColor (Instance)")
                            orgColor = meshRends[r].material.color;
                    }
                }
            }
            else if (isSkeleton)
            {
                if (whiteSkeletonActive)
                {
                    goldPrefab = dropItem.gold4;
                    enemySound = soundClips.swipe;
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {
                            if (meshRends[r].materials[m].name == "Bones (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = new Color(0.5f, 0.5f, 0.5f, 1);
                            }
                            if (meshRends[r].materials[m].name == "SkeletonEyes (Instance)")
                            {
                                Color eyeColor;
                                if (isDarkEnemy)
                                    eyeColor = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    eyeColor = new Color(1f, 0.5f, 0, 1);
                                meshRends[r].materials[m].SetColor("_EmissionColor", eyeColor);
                                meshRends[r].materials[m].EnableKeyword("_EmissionColor");
                            }
                        }
                        if (meshRends[r].material.name == "Bones (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = new Color(0.5f, 0.5f, 0.5f, 1);
                            orgColor = meshRends[r].material.color;
                        }
                        if (meshRends[r].material.name == "SkeletonEyes (Instance)")
                        {
                            Color eyeColor;
                            if (isDarkEnemy)
                                eyeColor = new Color(0.5f, 0, 1, 0.5f);
                            else
                                eyeColor = new Color(1f, 0.5f, 0, 1);
                            meshRends[r].material.SetColor("_EmissionColor", eyeColor);
                            meshRends[r].material.EnableKeyword("_EmissionColor");
                        }
                    }
                }
                else if (greenSkeletonActive)
                {
                    goldPrefab = dropItem.gold6;
                    enemySound = soundClips.swipe;
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {
                            if (meshRends[r].materials[m].name == "Bones (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = new Color(0.3f, 0.6f, 0.3f, 1);
                            }
                            if (meshRends[r].materials[m].name == "SkeletonEyes (Instance)")
                            {
                                Color eyeColor;
                                if (isDarkEnemy)
                                    eyeColor = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    eyeColor = new Color(0f, 0.3f, 1, 1);
                                meshRends[r].materials[m].SetColor("_EmissionColor", eyeColor);
                                meshRends[r].materials[m].EnableKeyword("_EmissionColor");
                            }
                        }
                        if (meshRends[r].material.name == "Bones (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = new Color(0.3f, 0.6f, 0.3f, 1);
                            orgColor = meshRends[r].material.color;
                        }
                        if (meshRends[r].material.name == "SkeletonEyes (Instance)")
                        {
                            Color eyeColor;
                            if (isDarkEnemy)
                                eyeColor = new Color(0.5f, 0, 1, 0.5f);
                            else
                                eyeColor = new Color(0f, 0.3f, 1, 1);
                            meshRends[r].material.SetColor("_EmissionColor", eyeColor);
                            meshRends[r].material.EnableKeyword("_EmissionColor");
                        }
                    }
                }
                else if (redSkeletonActive)
                {
                    goldPrefab = dropItem.gold8;
                    enemySound = soundClips.swipe;
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {
                            if (meshRends[r].materials[m].name == "Bones (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = new Color(0.8f, 0.3f, 0.3f, 1);
                            }
                            if (meshRends[r].materials[m].name == "SkeletonEyes (Instance)")
                            {
                                Color eyeColor;
                                if (isDarkEnemy)
                                    eyeColor = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    eyeColor = new Color(0f, 1f, 0.5f, 1);
                                meshRends[r].materials[m].SetColor("_EmissionColor", eyeColor);
                                meshRends[r].materials[m].EnableKeyword("_EmissionColor");
                            }
                        }
                        if (meshRends[r].material.name == "Bones (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = new Color(0.8f, 0.3f, 0.3f, 1);
                            orgColor = meshRends[r].material.color;
                        }
                        if (meshRends[r].material.name == "SkeletonEyes (Instance)")
                        {
                            Color eyeColor;
                            if (isDarkEnemy)
                                eyeColor = new Color(0.5f, 0, 1, 0.5f);
                            else
                                eyeColor = new Color(0f, 1f, 0.5f, 1);
                            meshRends[r].material.SetColor("_EmissionColor", eyeColor);
                            meshRends[r].material.EnableKeyword("_EmissionColor");
                        }
                    }
                }
                else if (darkSkeletonActive)
                {
                    goldPrefab = dropItem.gold12;
                    enemySound = soundClips.swipe;
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {
                            if (meshRends[r].materials[m].name == "Bones (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = new Color(0f, 0.1f, 0.1f, 1);
                            }
                            if (meshRends[r].materials[m].name == "SkeletonEyes (Instance)")
                            {
                                Color eyeColor;
                                if (isDarkEnemy)
                                    eyeColor = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    eyeColor = new Color(1, 0, 0, 1);
                                meshRends[r].materials[m].SetColor("_EmissionColor", eyeColor);
                                meshRends[r].materials[m].EnableKeyword("_EmissionColor");
                            }
                        }
                        if (meshRends[r].material.name == "Bones (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = new Color(0f, 0.1f, 0.1f, 1);
                            orgColor = meshRends[r].material.color;
                        }
                        if (meshRends[r].material.name == "SkeletonEyes (Instance)")
                        {
                            Color eyeColor;
                            if (isDarkEnemy)
                                eyeColor = new Color(0.5f, 0, 1, 0.5f);
                            else
                                eyeColor = new Color(1, 0, 0, 1);
                            meshRends[r].material.SetColor("_EMISSION", eyeColor);
                            meshRends[r].material.EnableKeyword("_EMISSION");
                        }
                    }
                }
            }
            else if (isMimic)
            {
                if (blueMimicActive)
                {
                    goldPrefab = dropItem.gold7;
                    enemySound = null;
                    if (isDarkEnemy)
                        eyeObj.material = new Material(mimicMaterials[3]);
                    else
                        eyeObj.material = new Material(mimicMaterials[0]);
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {
                            if (meshRends[r].materials[m].name == "LegsArms (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = new Color(0.2f, 0.5f, 0.6f, 1);
                            }
                            orgColor = meshRends[r].materials[m].color;
                        }
                        if (meshRends[r].material.name == "EyeLvl1 (Instance)")
                            orgEyeColor = meshRends[r].material.color;
                    }
                }
                else if (greenMimicActive)
                {
                    goldPrefab = dropItem.gold10;
                    enemySound = null;
                    if (isDarkEnemy)
                        eyeObj.material = new Material(mimicMaterials[3]);
                    else
                        eyeObj.material = new Material(mimicMaterials[1]);
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {
                            if (meshRends[r].materials[m].name == "LegsArms (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = new Color(0.5f, 0.6f, 0.3f, 1);
                            }
                            orgColor = meshRends[r].materials[m].color;
                        }
                        if (meshRends[r].material.name == "EyeLvl2 (Instance)")
                            orgEyeColor = meshRends[r].material.color;
                    }
                }
                else if (orangeMimicActive)
                {
                    goldPrefab = dropItem.gold12;
                    enemySound = null;
                    if (isDarkEnemy)
                        eyeObj.material = new Material(mimicMaterials[3]);
                    else
                        eyeObj.material = new Material(mimicMaterials[2]);
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {
                            if (meshRends[r].materials[m].name == "LegsArms (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = new Color(0.7f, 0.4f, 0.2f, 1);
                            }
                            orgColor = meshRends[r].materials[m].color;
                        }
                        if (meshRends[r].material.name == "EyeLvl3 (Instance)")
                            orgEyeColor = meshRends[r].material.color;
                    }
                }
            }
            else if (isToad)
            {
                if (goliathToadActive)
                {
                    goldPrefab = dropItem.gold100;
                    enemySound = null;
                    foreach (MeshRenderer renders in toadEyeObjs)
                        renders.material = new Material(toadMaterials[0]);

                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {
                            if (meshRends[r].materials[m].name == "ToadLegsArms (Instance)")
                            {
                                meshRends[r].materials[m].color = new Color(0.5f, 0f, 1f, 1f);
                                orgColor = meshRends[r].materials[m].color;
                                orgToadMaterial = meshRends[r].materials[m];
                            }
                        }
                        if (meshRends[r].material.name == "ToadEye (Instance)")
                            orgToadEyeColor = meshRends[r].material.color;
                    }
                }
                else if (chameleonToadActive)
                {
                    goldPrefab = dropItem.gold100;
                    enemySound = null;
                    foreach (MeshRenderer renders in toadEyeObjs)
                        renders.material = new Material(toadMaterials[1]);

                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {
                            if (meshRends[r].materials[m].name == "ToadLegsArms (Instance)")
                            {
                                meshRends[r].materials[m].color = new Color(0.7f, 1f, 0f, 1f);
                                orgColor = meshRends[r].materials[m].color;
                                orgToadMaterial = meshRends[r].materials[m];
                            }
                        }
                        if (meshRends[r].material.name == "ToadEye 1 (Instance)")
                            orgToadEyeColor = meshRends[r].material.color;
                    }
                }
                else if (devilToadActive)
                {
                    goldPrefab = dropItem.gold100;
                    enemySound = null;
                    foreach (MeshRenderer renders in toadEyeObjs)
                        renders.material = new Material(toadMaterials[2]);

                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {
                            if (meshRends[r].materials[m].name == "ToadLegsArms (Instance)")
                            {
                                meshRends[r].materials[m].color = new Color(1f, 0f, 0f, 1f);
                                orgColor = meshRends[r].materials[m].color;
                                orgToadMaterial = meshRends[r].materials[m];
                            }
                        }
                        if (meshRends[r].material.name == "ToadEye 2 (Instance)")
                            orgToadEyeColor = meshRends[r].material.color;
                    }
                }
            }
            else if (isGraveYard)
            {
                if (graveYardActive)
                {
                    goldPrefab = dropItem.gold10;
                    enemySound = soundClips.swipe;
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {
                            if (meshRends[r].materials[m].name == "Bones (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = new Color(0.5f, 0.5f, 0.5f, 1);
                            }
                        }
                        if (meshRends[r].material.name == "Bones (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = new Color(0.5f, 0.5f, 0.5f, 1);
                            orgColor = meshRends[r].material.color;
                        }
                    }
                }
                else if (cemetaryActive)
                {
                    goldPrefab = dropItem.gold20;
                    enemySound = soundClips.swipe;
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {
                            if (meshRends[r].materials[m].name == "Bones (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = new Color(0.17f, 0.23f, 0.28f, 1);
                            }
                        }
                        if (meshRends[r].material.name == "Bones (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = new Color(0.17f, 0.23f, 0.28f, 1);
                            orgColor = meshRends[r].material.color;
                        }
                    }
                }
                else if (burialGroundsActive)
                {
                    goldPrefab = dropItem.gold8;
                    enemySound = soundClips.swipe;
                    for (int r = 0; r < meshRends.Length; r++)
                    {
                        for (int m = 0; m < meshRends[r].materials.Length; m++)
                        {
                            if (meshRends[r].materials[m].name == "Bones (Instance)")
                            {
                                if (isDarkEnemy)
                                    meshRends[r].materials[m].color = new Color(0.5f, 0, 1, 0.5f);
                                else
                                    meshRends[r].materials[m].color = new Color(0.3f, 0.3f, 0.25f, 1);
                                meshRends[r].materials[m].SetFloat("_Glossiness", 0.5f);
                            }
                        }
                        if (meshRends[r].material.name == "Bones (Instance)")
                        {
                            if (isDarkEnemy)
                                meshRends[r].material.color = new Color(0.5f, 0, 1, 0.5f);
                            else
                                meshRends[r].material.color = new Color(0.3f, 0.3f, 0.25f, 1);
                            meshRends[r].material.SetFloat("_Glossiness", 0.5f);
                            orgColor = meshRends[r].material.color;
                        }
                    }
                }
            }
            if (blueBubActive || greenBubActive || greyBubActive || pinkBubActive || purpleBubActive || redBubActive || yellowBubActive)
            {
                Color color = meshRend.material.color;
                SetupMaterialWithBlendMode(meshRend.material, BlendMode.Fade);
                meshRend.material.color = new Color(color.r, color.g, color.b, 0.5f);

            }
        }
    }
    public void SetupEnemyStatus(bool True)
    {
        if (True)
        {
            if (newStatus == null)
            {
                enemyCanvas = GameObject.Find("Core/Player/PopUpCanvas/EnemyUIStorage");
                newStatus = Instantiate(EnemyStatusPrefab, transform.position, Quaternion.identity);
                newStatus.transform.SetParent(enemyCanvas.transform);
                NameObj = newStatus.GetComponentInChildren<Transform>().Find("name");
                Name = NameObj.GetComponent<Text>();
                Name.enabled = true;
                healthBarMaskObj = newStatus.GetComponentInChildren<Transform>().Find("healthBarMask");
                healthBarMask = healthBarMaskObj.GetComponent<Image>();
                healthBarMask.enabled = true;
                healthBarObj = newStatus.GetComponentInChildren<Transform>().Find("healthBarMask/healthBar");
                healthBar = healthBarObj.GetComponent<Image>();
                healthBar.enabled = true;
            }

            if (isBubble)
            {
                if (greyBubActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Blob";
                    else
                        Name.text = "Blob";
                }
                else if (blueBubActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Goop";
                    else
                        Name.text = "Goop";
                }
                else if (pinkBubActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Scum";
                    else
                        Name.text = "Scum";
                }
                else if (redBubActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Slime";
                    else
                        Name.text = "Slime";
                }

                else if (yellowBubActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Foam";
                    else
                        Name.text = "Foam";
                }
                else if (greenBubActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Ooze";
                    else
                        Name.text = "Ooze";
                }
                else if (purpleBubActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Amoeba";
                    else
                        Name.text = "Amoeba";
                }

            }
            else if (isMushroom)
            {
                if (greenMushActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Spore";
                    else
                        Name.text = "Spore";
                }
                else if (redMushActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Fungi";
                    else
                        Name.text = "Fungi";
                }
                else if (greyMushActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Peyote";
                    else
                        Name.text = "Peyote";
                }
            }
            else if (isStatue)
            {
                if (blueStatueActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Gulem";
                    else
                        Name.text = "Gulem";
                }
                else if (silverStatueActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Gargoyle";
                    else
                        Name.text = "Gargoyle";
                }
                else if (goldStatueActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Grotesque";
                    else
                        Name.text = "Grotesque";
                }
            }
            else if (isSkeleton)
            {
                if (whiteSkeletonActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Bones";
                    else
                        Name.text = "Bones";
                }
                else if (greenSkeletonActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Decay";
                    else
                        Name.text = "Decay";
                }
                else if (redSkeletonActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Carcass";
                    else
                        Name.text = "Carcass";
                }
                else if (darkSkeletonActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Putrid";
                    else
                        Name.text = "Putrid";
                }
            }
            else if (isMimic)
            {
                if (blueMimicActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Imitation";
                    else
                        Name.text = "Imitation";
                }
                else if (greenMimicActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Masquerade";
                    else
                        Name.text = "Masquerade";
                }
                else if (orangeMimicActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Mimesis";
                    else
                        Name.text = "Mimesis";
                }
            }
            else if (isToad)
            {
                if (goliathToadActive)
                    Name.text = "Ancient: Goliath Toad";
                else if (chameleonToadActive)
                    Name.text = "Guardian: Chameleon Toad";
                else if (devilToadActive)
                    Name.text = "Primordial: Devil Toad";
            }
            else if (isGraveYard)
            {
                if (graveYardActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Grave Yard";
                    else
                        Name.text = "Grave Yard";
                }
                else if (cemetaryActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Cemetary";
                    else
                        Name.text = "Cemetary";
                }
                else if (burialGroundsActive)
                {
                    if (isDarkEnemy)
                        Name.text = "Dark Burial Grounds";
                    else
                        Name.text = "Burial Grounds";
                }
            }
            statusActive = true;
        }
        else
        {
            Destroy(newStatus.gameObject);
            statusActive = false;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, hidingRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, awarenessRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attacknessRange);
    }
    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
    void handler()
    {
        if (healthBar != null)
            healthBar.fillAmount = Map(enemyHealth, 1, healthMax, 0, 1);
    }
    public void DamageEnemy(int amount)
    {
        if (!isIndestructible && isReal && !isDying)
        {
            if (isShocked)
                enemyHealth -= amount * 2;
            else
                enemyHealth -= amount;
            flashEnemy = true;
            AudioSystem.PlayAudioSource(soundClips.hit, 0.8f, 1f);
            if (enemyHealth < 1)
            {
                PlayerSystem playSys = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSystem>();
                playSys.FlashEnemyDestroyed();
                anim.Rebind();
                if (isToad && isMusic)
                {
                    if (musicRoutine != null)
                        StopCoroutine(musicRoutine);
                    soundClips.musicAudioSrc = GameObject.Find("Core/GameMusic&Sound/GameMusic").GetComponent<AudioSource>();
                    soundClips.musicAudioSrc.clip = soundClips.currentTempleMusic;
                    musicRoutine = AudioFadeIn(soundClips.musicAudioSrc);
                    StartCoroutine(musicRoutine);
                    isMusic = false;
                }
                isFrozen = false;
                isShocked = false;
                isBurned = false;
                isIdle = true;
                if (healthBar != null)
                    healthBar.enabled = false;
                if (Name != null)
                    Name.enabled = false;
                isDying = true;
                DropPickup();
                if (isGraveYard)
                    ghostRemoved(true);
                AudioSystem.PlayAudioSource(soundClips.death, 0.8f, 1f);
                if (isMimic)
                {
                    BoxCollider enemyCol = GetComponent<BoxCollider>();
                    enemyCol.enabled = false;
                    Enemy.GetComponent<SphereCollider>().enabled = false;
                }
                else if (isToad)
                {
                    if (goliathToadActive)
                    {
                        GameObject objParent = GameObject.Find("Temple1/NonStatic/TempleBoss");
                        if (objParent != null)
                        {
                            EventActionSystem EAS = objParent.GetComponent<EventActionSystem>();
                            if (EAS != null)
                                EAS.TriggerEvent(4);
                        }

                    }
                    CapsuleCollider enemyCol = GetComponent<CapsuleCollider>();
                    enemyCol.enabled = false;
                    Enemy.GetComponent<CapsuleCollider>().enabled = false;
                }
                else
                {
                    Enemy.GetComponent<SphereCollider>().enabled = false;
                    CapsuleCollider enemyCollider = GetComponent<CapsuleCollider>();
                    enemyCollider.enabled = false;
                }

                enemyHealth = 1;

            }
        }
    }
    public void QuakePlayer(float seconds)
    {

        enemySound = soundClips.toadPound;
        PlaySounds();
        SwordSystem swordSys = GameObject.FindGameObjectWithTag("Player").GetComponent<SwordSystem>();
        if (quake != null)
            StopCoroutine(quake);
        quake = swordSys.QuakeEffect(seconds);
        StartCoroutine(quake);
    }
    public void OnGetHitBySword(int hitValue)
    {
        if (isReal && !isToad && !isGraveYard)
        {
            if (isMimic)
            {
                if (mimicActive)
                {
                    isKnockedBack = true;
                    SetAnimation(EnemyAnimation.Damage, true);
                    DamageEnemy(hitValue);
                }
            }
            else
            {
                isKnockedBack = true;
                SetAnimation(EnemyAnimation.Damage, true);
                DamageEnemy(hitValue);
            }

        }
        else if (isReal && isToad && !isGraveYard)
        {
            if (isCloaked && ItemSystem.demonMaskEnabled)
            {
                isKnockedBack = true;
                SetAnimation(EnemyAnimation.Damage, true);
                DamageEnemy(hitValue);
            }
            else if (!isCloaked)
            {
                isKnockedBack = true;
                SetAnimation(EnemyAnimation.Damage, true);
                DamageEnemy(hitValue);
            }
        }
        else if (isReal && isGraveYard)
            DamageEnemy(hitValue);
    }
    public void DropPickup()
    {
        int[] lifeBerryNum = { 1, 2, 3, 4, 5 };
        int[] rixileNum = { 6, 7, 8 };
        int[] nicirPlantNum = { 9, 10, 11, 12, 13 };
        int[] mindRemedyNum = { 14, 15, 16, 17, 18 };
        int[] petriShroomNum = { 19, 20, 21, 22, 23 };
        int[] reliefOintmentNum = { 24, 25, 26, 27, 28 };
        int RandomNumber = UnityEngine.Random.Range(1, 1000);

        for (int i = 0; i < 1; i++)
        {
            if (RandomNumber < 29)
            {
                for (int LB = 0; LB < lifeBerryNum.Length; LB++)
                {

                    if (lifeBerryNum[LB] == RandomNumber)
                    {
                        if (ItemSystem.lifeBerryAmt < 3)
                        {
                            itemPrefab = dropItem.lifeBerry;
                            Instantiate(itemPrefab, transform.position, Quaternion.identity);
                        }
                        else
                            Instantiate(goldPrefab, transform.position, Quaternion.identity);
                    }
                }
                for (int RX = 0; RX < rixileNum.Length; RX++)
                {
                    if (rixileNum[RX] == RandomNumber)
                    {
                        if (ItemSystem.rixileAmt < 3)
                        {
                            itemPrefab = dropItem.rixile;
                            Instantiate(itemPrefab, transform.position, Quaternion.identity);
                        }
                        else
                            Instantiate(goldPrefab, transform.position, Quaternion.identity);
                    }
                }
                for (int NP = 0; NP < nicirPlantNum.Length; NP++)
                {
                    if (nicirPlantNum[NP] == RandomNumber)
                    {
                        if (ItemSystem.nicirPlantAmt < 3)
                        {
                            itemPrefab = dropItem.nicirPlant;
                            Instantiate(itemPrefab, transform.position, Quaternion.identity);
                        }
                        else
                            Instantiate(goldPrefab, transform.position, Quaternion.identity);
                    }
                }
                for (int MR = 0; MR < mindRemedyNum.Length; MR++)
                {
                    if (mindRemedyNum[MR] == RandomNumber)
                    {
                        if (ItemSystem.mindRemedyAmt < 3)
                        {
                            itemPrefab = dropItem.mindRemedy;
                            Instantiate(itemPrefab, transform.position, Quaternion.identity);
                        }
                        else
                            Instantiate(goldPrefab, transform.position, Quaternion.identity);
                    }
                }
                for (int PS = 0; PS < petriShroomNum.Length; PS++)
                {
                    if (petriShroomNum[PS] == RandomNumber)
                    {
                        if (ItemSystem.petriShroomAmt < 3)
                        {
                            itemPrefab = dropItem.petriShroom;
                            Instantiate(itemPrefab, transform.position, Quaternion.identity);
                        }
                        else
                            Instantiate(goldPrefab, transform.position, Quaternion.identity);
                    }
                }
                for (int RO = 0; RO < reliefOintmentNum.Length; RO++)
                {
                    if (reliefOintmentNum[RO] == RandomNumber)
                    {
                        if (ItemSystem.reliefOintmentAmt < 3)
                        {
                            itemPrefab = dropItem.reliefOintment;
                            Instantiate(itemPrefab, transform.position, Quaternion.identity);
                        }
                        else
                            Instantiate(goldPrefab, transform.position, Quaternion.identity);
                    }
                }
            }
            else
                Instantiate(goldPrefab, transform.position, Quaternion.identity);
        }
    }
    public enum EnemyMovement { Stop, Idle, Move, Attack, frozen }
    public void SetMovement(EnemyMovement status)
    {
        switch (status)
        {
            case EnemyMovement.frozen:
                {
                    SetAnimation(EnemyAnimation.None, true);
                    break;
                }
            case EnemyMovement.Stop:
                {
                    if (isMushroom)
                    {
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Hide, true);
                        SetAnimation(EnemyAnimation.UnHide, false);

                    }

                    else if (isStatue)
                    {
                        nav.isStopped = true;
                        nav.speed = 0;
                        SetAnimation(EnemyAnimation.Idle, true);
                        SetAnimation(EnemyAnimation.Move, false);
                        SetAnimation(EnemyAnimation.Attack, false);


                    }
                    else if (isSkeleton)
                    {
                        isIndestructible = false;
                        SetAnimation(EnemyAnimation.Idle, true);
                        SetAnimation(EnemyAnimation.Move, false);
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Attack2, false);
                        SetAnimation(EnemyAnimation.Defend, false);


                    }
                    if (isMimic)
                    {
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Hide, true);
                        SetAnimation(EnemyAnimation.UnHide, false);

                    }
                    if (isToad)
                    {
                        nav.isStopped = true;
                        nav.speed = 0;
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Attack2, false);
                        SetAnimation(EnemyAnimation.Attack3, false);
                        SetAnimation(EnemyAnimation.Attack4, false);
                        SetAnimation(EnemyAnimation.Idle, true);
                        SetAnimation(EnemyAnimation.UnHide, false);
                        SetAnimation(EnemyAnimation.Move, false);
                    }
                    else if (isGraveYard)
                    {
                        nav.isStopped = true;
                        nav.speed = 0;
                        SetAnimation(EnemyAnimation.Idle, true);
                        SetAnimation(EnemyAnimation.Move, false);
                        SetAnimation(EnemyAnimation.Attack, false);
                    }
                    break;
                }
            case EnemyMovement.Idle:
                {
                    if (!isStatue && !isHidden && !isGraveYard)
                        Idling();
                    else if (isSkeleton)
                    {
                        isIndestructible = false;
                        SetMovement(EnemyMovement.Stop);
                    }
                    else if (isToad)
                    {
                        isIndestructible = false;
                        SetMovement(EnemyMovement.Stop);
                    }
                    else
                        SetMovement(EnemyMovement.Stop);
                    break;
                }
            case EnemyMovement.Move:
                {
                    if (isBubble)
                    {
                        SetAnimation(EnemyAnimation.Jump, false);
                        SetAnimation(EnemyAnimation.Move, true);
                        SetAnimation(EnemyAnimation.Idle, false);

                    }
                    else if (isMushroom)
                    {
                        if (!isHidden)
                            SetAnimation(EnemyAnimation.Move, true);
                        else
                            SetAnimation(EnemyAnimation.UnHide, true);
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Hide, false);
                        SetAnimation(EnemyAnimation.Idle, false);

                    }
                    else if (isStatue)
                    {

                        SetAnimation(EnemyAnimation.Move, true);
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Idle, false);
                    }
                    else if (isSkeleton)
                    {
                        isIndestructible = false;
                        SetAnimation(EnemyAnimation.Move, true);
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Attack2, false);
                        SetAnimation(EnemyAnimation.Defend, false);
                        SetAnimation(EnemyAnimation.Idle, false);
                    }
                    else if (isMimic)
                    {
                        if (!isHidden)
                            SetAnimation(EnemyAnimation.Move, true);
                        else
                            SetAnimation(EnemyAnimation.UnHide, true);

                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Hide, false);
                        SetAnimation(EnemyAnimation.Idle, false);
                    }
                    else if (isToad)
                    {
                        isIndestructible = false;
                        if (!isHidden)
                            SetAnimation(EnemyAnimation.Move, true);
                        else
                            SetAnimation(EnemyAnimation.UnHide, true);
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Attack2, false);
                        SetAnimation(EnemyAnimation.Attack3, false);
                        SetAnimation(EnemyAnimation.Attack4, false);
                        SetAnimation(EnemyAnimation.Hide, false);
                        SetAnimation(EnemyAnimation.Idle, false);
                    }
                    break;
                }
            case EnemyMovement.Attack:
                {
                    Attacking();
                    break;
                }
        }
    }
    public void Attacking()
    {
        if (!isGraveYard)
        {
            if (attackDuration > 0 && !attackFinished)
            {
                attackDuration -= Time.deltaTime;

                //=======================AttackState1============================//
                if (attackState == 1)
                {
                    if (isBubble)
                    {
                        SetAnimation(EnemyAnimation.Jump, true);
                        SetAnimation(EnemyAnimation.Move, false);
                        SetAnimation(EnemyAnimation.Idle, false);
                    }
                    else if (isMushroom)
                    {
                        if (isHidden)
                            SetAnimation(EnemyAnimation.UnHide, true);
                        else if (!isHidden)
                        {

                            SetAnimation(EnemyAnimation.Idle, false);
                            SetAnimation(EnemyAnimation.Hide, false);
                            SetAnimation(EnemyAnimation.Move, false);
                            SetAnimation(EnemyAnimation.UnHide, false);
                            SetAnimation(EnemyAnimation.Attack, true);
                        }
                    }
                    else if (isStatue)
                    {

                        SetAnimation(EnemyAnimation.Attack, true);
                        SetAnimation(EnemyAnimation.Move, false);
                    }
                    else if (isSkeleton)
                    {

                        SetAnimation(EnemyAnimation.Attack, true);
                        SetAnimation(EnemyAnimation.Move, false);
                    }
                    else if (isMimic)
                    {
                        if (mimicActive)
                        {
                            if (isHidden)
                                SetAnimation(EnemyAnimation.UnHide, true);
                            else
                                SetAnimation(EnemyAnimation.Attack, true);
                        }

                        SetAnimation(EnemyAnimation.Idle, false);
                        SetAnimation(EnemyAnimation.Hide, false);
                        SetAnimation(EnemyAnimation.Move, false);
                        SetAnimation(EnemyAnimation.UnHide, false);
                    }
                    else if (isToad)
                    {
                        isIndestructible = true;
                        if (isHidden)
                            SetAnimation(EnemyAnimation.UnHide, true);
                        else if (!isHidden)
                            SetAnimation(EnemyAnimation.Attack, true);
                        SetAnimation(EnemyAnimation.Idle, false);
                        SetAnimation(EnemyAnimation.Hide, false);
                        SetAnimation(EnemyAnimation.Move, false);
                        SetAnimation(EnemyAnimation.UnHide, false);
                        SetAnimation(EnemyAnimation.Attack2, false);
                        SetAnimation(EnemyAnimation.Attack3, false);
                        SetAnimation(EnemyAnimation.Attack4, false);
                    }

                }
                //=======================AttackState2============================//
                else if (attackState == 2)
                {
                    if (isBubble)
                    {

                        SetAnimation(EnemyAnimation.Jump, false);
                        SetAnimation(EnemyAnimation.Move, true);
                        SetAnimation(EnemyAnimation.Idle, false);
                    }
                    else if (isMushroom)
                    {
                        if (isHidden)
                            SetAnimation(EnemyAnimation.UnHide, true);
                        else
                            SetAnimation(EnemyAnimation.Move, true);
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Hide, false);
                        SetAnimation(EnemyAnimation.Idle, false);

                    }
                    else if (isStatue)
                    {
                        SetAnimation(EnemyAnimation.Move, true);
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Idle, false);

                    }
                    else if (isSkeleton)
                    {
                        isIndestructible = false;
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Defend, false);
                        SetAnimation(EnemyAnimation.Attack2, true);
                        SetAnimation(EnemyAnimation.Move, false);
                    }
                    else if (isMimic)
                    {
                        if (mimicActive)
                        {
                            if (isHidden)
                                SetAnimation(EnemyAnimation.UnHide, true);
                            else
                                SetAnimation(EnemyAnimation.Move, true);
                        }
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Hide, false);
                        SetAnimation(EnemyAnimation.Idle, false);
                    }
                    else if (isToad)
                    {
                        isIndestructible = true;
                        if (isHidden)
                            SetAnimation(EnemyAnimation.UnHide, true);
                        else if (!isHidden)
                            SetAnimation(EnemyAnimation.Attack2, true);
                        SetAnimation(EnemyAnimation.Idle, false);
                        SetAnimation(EnemyAnimation.Hide, false);
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Move, false);
                        SetAnimation(EnemyAnimation.Attack3, false);
                        SetAnimation(EnemyAnimation.Attack4, false);
                    }
                }
                //=======================AttackState3============================//
                else if (attackState == 3)
                {
                    if (isBubble)
                    {

                        SetAnimation(EnemyAnimation.Jump, false);
                        SetAnimation(EnemyAnimation.Move, true);
                        SetAnimation(EnemyAnimation.Idle, false);
                    }
                    else if (isMushroom)
                    {
                        if (isHidden)
                            SetAnimation(EnemyAnimation.UnHide, true);
                        else
                            SetAnimation(EnemyAnimation.Move, true);
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Hide, false);
                        SetAnimation(EnemyAnimation.Idle, false);
                    }
                    else if (isStatue)
                    {
                        SetAnimation(EnemyAnimation.Move, true);
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Idle, false);
                    }
                    else if (isSkeleton)
                    {
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Defend, true);
                        SetAnimation(EnemyAnimation.Attack2, false);
                        SetAnimation(EnemyAnimation.Move, false);
                    }
                    else if (isMimic)
                    {

                        if (mimicActive)
                        {
                            if (isHidden)
                                SetAnimation(EnemyAnimation.UnHide, true);
                            else
                                SetAnimation(EnemyAnimation.Move, true);
                        }
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Hide, false);
                        SetAnimation(EnemyAnimation.Idle, false);
                    }
                    else if (isToad)
                    {
                        isIndestructible = true;
                        if (isHidden)
                            SetAnimation(EnemyAnimation.UnHide, true);
                        else if (!isHidden)
                            SetAnimation(EnemyAnimation.Attack3, true);
                        SetAnimation(EnemyAnimation.Idle, false);
                        SetAnimation(EnemyAnimation.Hide, false);
                        SetAnimation(EnemyAnimation.Move, false);
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Attack2, false);
                        SetAnimation(EnemyAnimation.Attack4, false);
                    }
                }
                //=======================AttackState4============================//
                else if (attackState == 4)
                {
                    if (isBubble)
                    {

                        SetAnimation(EnemyAnimation.Jump, false);
                        SetAnimation(EnemyAnimation.Move, true);
                        SetAnimation(EnemyAnimation.Idle, false);
                    }
                    else if (isMushroom)
                    {
                        if (isHidden)
                            SetAnimation(EnemyAnimation.UnHide, true);
                        else
                            SetAnimation(EnemyAnimation.Move, true);
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Hide, false);
                        SetAnimation(EnemyAnimation.Idle, false);
                    }
                    else if (isStatue)
                    {
                        SetAnimation(EnemyAnimation.Move, true);
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Idle, false);
                    }
                    else if (isSkeleton)
                    {
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Defend, true);
                        SetAnimation(EnemyAnimation.Attack2, false);
                        SetAnimation(EnemyAnimation.Move, false);
                    }
                    else if (isMimic)
                    {

                        if (mimicActive)
                        {
                            if (isHidden)
                                SetAnimation(EnemyAnimation.UnHide, true);
                            else
                                SetAnimation(EnemyAnimation.Move, true);
                        }
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Hide, false);
                        SetAnimation(EnemyAnimation.Idle, false);
                    }
                    else if (isToad)
                    {
                        isIndestructible = true;
                        if (isHidden)
                            SetAnimation(EnemyAnimation.UnHide, true);
                        else if (!isHidden)
                            SetAnimation(EnemyAnimation.Attack4, true);
                        SetAnimation(EnemyAnimation.Idle, false);
                        SetAnimation(EnemyAnimation.Hide, false);
                        SetAnimation(EnemyAnimation.Move, false);
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Attack2, false);
                        SetAnimation(EnemyAnimation.Attack3, false);
                    }
                }
                else
                {
                    if (isBubble)
                    {
                        SetAnimation(EnemyAnimation.Jump, false);
                        SetAnimation(EnemyAnimation.Move, true);
                    }
                    else if (isMushroom)
                    {
                        if (isHidden)
                            SetAnimation(EnemyAnimation.UnHide, true);
                        else
                            SetAnimation(EnemyAnimation.Move, true);
                        SetAnimation(EnemyAnimation.Idle, false);
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Hide, false);

                    }
                    else if (isStatue)
                    {
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Move, true);
                        SetAnimation(EnemyAnimation.Idle, false);

                    }
                    else if (isSkeleton)
                    {
                        isIndestructible = false;
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Move, true);
                        SetAnimation(EnemyAnimation.Idle, false);
                        SetAnimation(EnemyAnimation.Defend, false);
                        SetAnimation(EnemyAnimation.Attack2, false);

                    }
                    else if (isMimic)
                    {
                        if (mimicActive)
                        {
                            if (isHidden)
                                SetAnimation(EnemyAnimation.UnHide, true);
                            else
                                SetAnimation(EnemyAnimation.Move, true);
                        }
                        SetAnimation(EnemyAnimation.Idle, false);
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Hide, false);
                        SetAnimation(EnemyAnimation.Damage, false);
                    }
                    else if (isToad)
                    {
                        isIndestructible = false;
                        if (isHidden)
                            SetAnimation(EnemyAnimation.UnHide, true);
                        else if (!isHidden)
                            SetAnimation(EnemyAnimation.Move, true);
                        SetAnimation(EnemyAnimation.Idle, false);
                        SetAnimation(EnemyAnimation.Hide, false);
                        SetAnimation(EnemyAnimation.Attack, false);
                        SetAnimation(EnemyAnimation.Attack4, false);
                        SetAnimation(EnemyAnimation.Attack2, false);
                        SetAnimation(EnemyAnimation.Attack3, false);
                    }
                }
            }
            else if (attackDuration < 0 && !attackFinished)
            {
                currentAttackState = attackState;
                SetRandomAttack();
                attackDuration = UnityEngine.Random.Range(3, 6);
                Invoke("Attacking", 0.1f);
            }
            else if (attackFinished)
            {
                currentAttackState = attackState;
                SetRandomAttack();
                if (isMushroom)
                    attackDuration = UnityEngine.Random.Range(1, 3);
                else if (isToad)
                {
                    if (goliathToadActive)
                        attackDuration = UnityEngine.Random.Range(5, 10);
                    else if (chameleonToadActive)
                        attackDuration = UnityEngine.Random.Range(3, 6);
                    else if (devilToadActive)
                        attackDuration = UnityEngine.Random.Range(1, 4);
                }
                else
                    attackDuration = 0.1f;
                Invoke("Attacking", 1f);
                attackFinished = false;
            }
        }
        else
        {
            if (ghostPool.transform.childCount < 10)
            {
                if (spawnTimer > 0)
                    spawnTimer -= Time.deltaTime;
                else if (spawnTimer < 0)
                {
                    if (graveYardActive && !isDarkEnemy)
                        SummonGhost(Ghost.Level1);
                    else if (cemetaryActive && !isDarkEnemy)
                        SummonGhost(Ghost.Level2);
                    else if (burialGroundsActive && !isDarkEnemy)
                        SummonGhost(Ghost.Level3);
                    else if (isDarkEnemy)
                        SummonGhost(Ghost.Level4);
                    spawnTimer = spawnTime;
                }
            }
        }
    }
    public enum Ghost { Level1, Level2, Level3, Level4}
    public void SummonGhost(Ghost type)
    {
        int position = UnityEngine.Random.Range(0, 6);
        switch (type)
        {
            case Ghost.Level1:
                {
                    instantGhost = Instantiate(ghostPrefab[0], spawnPoints[position].position, Quaternion.identity, ghostPool.transform) as GameObject;
                    break;
                }
            case Ghost.Level2:
                {
                    instantGhost = Instantiate(ghostPrefab[1], spawnPoints[position].position, Quaternion.identity, ghostPool.transform) as GameObject;
                    break;
                }
            case Ghost.Level3:
                {
                    instantGhost = Instantiate(ghostPrefab[2], spawnPoints[position].position, Quaternion.identity, ghostPool.transform) as GameObject;
                    break;
                }
            case Ghost.Level4:
                {
                    instantGhost = Instantiate(ghostPrefab[3], spawnPoints[position].position, Quaternion.identity, ghostPool.transform) as GameObject;
                    break;
                }
        }
        
    }
    public void ghostRemoved(bool all)
    {
        if (all)
        {
            if (ghostPool.transform.childCount > 0)
            {
                foreach (Transform ghost in ghostPool.transform)
                {
                    Destroy(ghost.gameObject);
                }
            }
        }
    }
    public void NavActive(int num)
    {
        if (num == 1)
        {
            isAttacked = false;
            isNavActive = true;
        }

        else if (num == 2)
            isAttacked = true;
        else
        {
            isAttacked = false;
            isNavActive = false;
        }

    }
    public void AttackAnimFinished()
    {
        attackFinished = true;
    }
    public void SetRandomAttack()
    {
        if (isBubble)
        {
            if (!isDarkEnemy)
            {
                if (greyBubActive)
                    attackState = UnityEngine.Random.Range(1, 6);
                else if (blueBubActive)
                    attackState = UnityEngine.Random.Range(1, 5);
                else if (pinkBubActive)
                    attackState = UnityEngine.Random.Range(1, 5);
                else if (redBubActive)
                    attackState = UnityEngine.Random.Range(1, 4);
                else if (yellowBubActive)
                    attackState = UnityEngine.Random.Range(1, 4);
                else if (greenBubActive)
                    attackState = UnityEngine.Random.Range(1, 3);
                else if (purpleBubActive)
                    attackState = UnityEngine.Random.Range(1, 2);
            }
            else
                attackState = UnityEngine.Random.Range(1, 2);
        }
        else if (isMushroom)
        {
            if (!isDarkEnemy)
            {
                if (greenMushActive)
                    attackState = UnityEngine.Random.Range(1, 3);
                else if (redMushActive)
                    attackState = UnityEngine.Random.Range(1, 3);
                else if (greyMushActive)
                    attackState = UnityEngine.Random.Range(1, 3);
            }
            else
                attackState = UnityEngine.Random.Range(1, 2);
        }
        else if (isStatue)
        {
            if (!isDarkEnemy)
            {
                if (blueStatueActive)
                    attackState = UnityEngine.Random.Range(1, 4);
                else if (silverStatueActive)
                    attackState = UnityEngine.Random.Range(1, 3);
                else if (goldStatueActive)
                    attackState = UnityEngine.Random.Range(1, 2);
            }
            else
                attackState = UnityEngine.Random.Range(1, 2);

        }
        else if (isSkeleton)
        {
            if (!isDarkEnemy)
            {
                if (whiteSkeletonActive)
                    attackState = UnityEngine.Random.Range(1, 7);
                else if (greenSkeletonActive)
                    attackState = UnityEngine.Random.Range(1, 6);
                else if (redSkeletonActive)
                    attackState = UnityEngine.Random.Range(1, 5);
                else if (darkSkeletonActive)
                    attackState = UnityEngine.Random.Range(1, 4);
            }
            else
                attackState = UnityEngine.Random.Range(1, 4);
        }
        else if (isMimic)
        {
            if (!isDarkEnemy)
            {
                if (blueMimicActive)
                    attackState = UnityEngine.Random.Range(1, 5);
                else if (greenMimicActive)
                    attackState = UnityEngine.Random.Range(1, 4);
                else if (orangeMimicActive)
                    attackState = UnityEngine.Random.Range(1, 3);
            }
            else
                attackState = UnityEngine.Random.Range(1, 2);
        }
        else if (isToad)
        {
            if (!isDarkEnemy)
            {
                if (goliathToadActive)
                    attackState = UnityEngine.Random.Range(1, 8);
                else if (chameleonToadActive)
                    attackState = UnityEngine.Random.Range(1, 7);
                else if (devilToadActive)
                    attackState = UnityEngine.Random.Range(1, 6);
            }
            else
                attackState = UnityEngine.Random.Range(1, 5);
        }
        if (attackState == currentAttackState)
            attackState += 1;


    }
    public void Idling()
    {
        SetAnimation(EnemyAnimation.Damage, false);
        if (idleDuration > 0)
        {
            Vector3 dir;
            idleDuration -= Time.deltaTime;
            if (idlestate == 1)
            {
                nav.speed = 0.0f;
                SetAnimation(EnemyAnimation.Idle, true);
                SetAnimation(EnemyAnimation.Move, false);
            }
            else if (idlestate == 2)
            {
                SetAnimation(EnemyAnimation.Idle, false);
                SetAnimation(EnemyAnimation.Move, true);
                nav.updateRotation = true;
                dir = Vector3.forward;
                var lookPos = dir;
                var rotation = Quaternion.LookRotation(lookPos);

                lookPos.y = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
                nav.Move(dir * (Speed * Time.deltaTime));
            }
            else if (idlestate == 3)
            {
                SetAnimation(EnemyAnimation.Idle, false);
                SetAnimation(EnemyAnimation.Move, true);
                nav.updateRotation = true;
                dir = Vector3.back;
                var lookPos = dir;
                var rotation = Quaternion.LookRotation(lookPos);

                lookPos.y = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
                nav.Move(dir * (Speed * Time.deltaTime));
            }
            else if (idlestate == 4)
            {
                SetAnimation(EnemyAnimation.Idle, false);
                SetAnimation(EnemyAnimation.Move, true);
                nav.updateRotation = true;
                dir = Vector3.left;
                var lookPos = dir;
                var rotation = Quaternion.LookRotation(lookPos);

                lookPos.y = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
                nav.Move(dir * (Speed * Time.deltaTime));
            }
            else if (idlestate == 5)
            {
                SetAnimation(EnemyAnimation.Idle, false);
                SetAnimation(EnemyAnimation.Move, true);
                nav.updateRotation = true;
                dir = Vector3.right;
                var lookPos = dir;
                var rotation = Quaternion.LookRotation(lookPos);

                lookPos.y = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
                nav.Move(dir * (Speed * Time.deltaTime));
            }
            else if (idlestate == 6)
            {
                nav.speed = 0.0f;
                SetAnimation(EnemyAnimation.Idle, true);
                SetAnimation(EnemyAnimation.Move, false);
            }
            else if (idlestate == 7)
            {
                nav.speed = 0.0f;
                SetAnimation(EnemyAnimation.Idle, true);
                SetAnimation(EnemyAnimation.Move, false);
            }
            else if (idlestate == 8)
            {
                nav.speed = 0.0f;
                SetAnimation(EnemyAnimation.Idle, true);
                SetAnimation(EnemyAnimation.Move, false);
            }

        }
        else if (idleDuration < 0)
        {
            idlestate = UnityEngine.Random.Range(1, 9);
            idleDuration = UnityEngine.Random.Range(1, 10);
            Invoke("Idling", 0.1f);
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !SceneSystem.isDisabled && !PlayerSystem.isDead && !PlayerSystem.isInvincible && isReal)
        {
            CharacterSystem characterCtrl = collision.gameObject.GetComponent<CharacterSystem>();
            impact = collision.gameObject.GetComponent<ImpactReceiver>();
            playST = collision.gameObject.GetComponent<PlayerSystem>();
            if (!isMimic || mimicActive)
            {
                Vector3 direction = (collision.transform.position - transform.position).normalized;
                if (ItemSystem.lifeBerryAmt <= 0)
                {
                    if (PlayerSystem.playerHealth <= 1)
                        impact.AddImpact(direction, 30);
                    else if (PlayerSystem.playerHealth > 1)
                        impact.AddImpact(direction, 200);
                }
                else if (ItemSystem.lifeBerryAmt > 0)
                    impact.AddImpact(direction, 200);
               
            }
            if (greyBubActive)
                playST.PlayerDamage(1, false);
            else if (blueBubActive)
                playST.PlayerDamage(2, false);
            else if (pinkBubActive)
                playST.PlayerDamage(3, false);
            else if (redBubActive)
                playST.PlayerDamage(4, false);
            else if (yellowBubActive)
                playST.PlayerDamage(5, false);
            else if (greenBubActive)
                playST.PlayerDamage(6, false);
            else if (purpleBubActive)
                playST.PlayerDamage(7, false);
            else if (greenMushActive)
                playST.PlayerDamage(2, false);
            else if (redMushActive)
                playST.PlayerDamage(4, false);
            else if (greyMushActive)
                playST.PlayerDamage(6, false);
            else if (blueStatueActive)
                playST.PlayerDamage(4, false);
            else if (silverStatueActive)
                playST.PlayerDamage(8, false);
            else if (goldStatueActive)
                playST.PlayerDamage(12, false);
            else if (whiteSkeletonActive)
                playST.PlayerDamage(3, false);
            else if (greenSkeletonActive)
                playST.PlayerDamage(6, false);
            else if (redSkeletonActive)
                playST.PlayerDamage(9, false);
            else if (darkSkeletonActive)
                playST.PlayerDamage(12, false);
            else if (mimicActive)
            {
                if (blueMimicActive)
                    playST.PlayerDamage(6, false);
                else if (greenMimicActive)
                    playST.PlayerDamage(12, false);
                else if (orangeMimicActive)
                    playST.PlayerDamage(18, false);
            }
            else if (goliathToadActive)
                playST.PlayerDamage(2, false);
            else if (chameleonToadActive)
                playST.PlayerDamage(15, false);
            else if (devilToadActive)
                playST.PlayerDamage(30, false);

            else if (graveYardActive)
                playST.PlayerDamage(3, false);
            else if (cemetaryActive)
                playST.PlayerDamage(18, false);
            else if (burialGroundsActive)
                playST.PlayerDamage(33, false);

            characterCtrl.SetFalling();
            //if (DialogueSystem.isDialogueActive && !PlayerSystem.isDead)
            //{
            //    InteractionSystem interaction = collision.gameObject.GetComponent<InteractionSystem>();
            //    interaction.DialogueInteraction(false, null);
            //}

        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PurpleJewel") && JewelSystem.purpleJewelEnabled && JewelSystem.isJewelActive && !isDarkEnemy && !darkEnemyActive)
        {
            isDarkEnemy = true;
            SetUpEnemy();
            if (statusActive)
            {
                SetupEnemyStatus(true);
            }
            darkEnemyActive = true;
        }
    }
    public enum BlendMode { Opaque, Cutout, Fade, Transparent }
    public static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode)
    {
        switch (blendMode)
        {
            case BlendMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case BlendMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case BlendMode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case BlendMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }
    public void MushroomHide(int num)
    {
        if (num == 1)
            isHidden = true;
        else
            isHidden = false;
    }
    public void ToadHide(int num)
    {
        if (num == 1)
            isHidden = true;
        else
            isHidden = false;
    }
    public void ToadShockwave()
    {
        QuakePlayer(1);
        GameObject projectile = Instantiate(shockwavePrefab, transform.position, Quaternion.identity) as GameObject;
       
        projectile.transform.SetParent(transform);
        Vector3 objPos = new Vector3(projectile.transform.localPosition.x, -4f, projectile.transform.localPosition.z);
        projectile.transform.localPosition = objPos;
    }
    public void MimicHide(int num)
    {
        if (num == 1)
            isHidden = true;
        else if (num == 2)
        {
            isHidden = false;
            mimicActive = true;
            SetupEnemyStatus(true);
            isIndestructible = false;
            SetAnimation(EnemyAnimation.UnHide, true);
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
            CharacterSystem playChar = player.GetComponent<CharacterSystem>();
            playChar.PlayerInteraction(false);
            isHidden = false;
        }
    }
    public void MimicAttackFinished()
    {
        attackState = 2;
        SetAnimation(EnemyAnimation.Move, true);
        SetAnimation(EnemyAnimation.Attack, false);
    }
    public void SprayPlayer(int num)
    {
        if (num == 1)
        {
            MushParticleSys.SetActive(true);
            sprayPS = MushParticleSys.GetComponent<ParticleSystem>();
            sprayPS.Play();
        }
        else
            MushParticleSys.SetActive(false);
    }
    public void EffectPlayer()
    {
        playST = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSystem>();
        if (greenMushActive)
        {
            randomEffect = UnityEngine.Random.Range(0, 20);
            if (randomEffect == 1)
                playST.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.poisonMin, true);
        }
        else if (redMushActive)
        {
            randomEffect = UnityEngine.Random.Range(0, 10);
            if (randomEffect == 1)
                playST.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.poisonMax, true);
        }
        else if (greyMushActive)
        {
            randomEffect = UnityEngine.Random.Range(0, 15);
            if (randomEffect == 1)
                playST.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.poisonMin, true);
            else if (randomEffect == 2)
                playST.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.poisonMax, true);
            else if (randomEffect == 3)
                playST.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.confuse, true);
            else if (randomEffect == 4)
                playST.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.silence, true);
            else if (randomEffect == 5)
                playST.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.paralyzed, true);
        }
    }
    public void ActivateAttackCollider(int num)
    {
        if (num == 1)
        {
            if (isStatue)
                handOne.enabled = true;
            else if (isSkeleton)
            {
                isIndestructible = false;
                sword.enabled = true;
            }
            else if (isMimic)
                mimicHands.enabled = true;
            else if(isToad)
                toadTongue.enabled = true;
        }
        else if (num == 2)
        {
            if (isStatue)
                handTwo.enabled = true;
            else if (isSkeleton)
            {
                isIndestructible = true;
                shield.enabled = true;
            }
        }
        else if (num == 0)
        {
            if (isStatue)
            {
                handOne.enabled = false;
                handTwo.enabled = false;
            }
            else if (isSkeleton)
            {
                isIndestructible = false;
                sword.enabled = false;
                shield.enabled = false;
            }
            else if (isMimic)
                mimicHands.enabled = false;
            else if (isToad)
                toadTongue.enabled = false;
        }
    }
    public void TurnOnReal(bool True)
    {
        if (True)
            isReal = true;
        else
            isReal = false;
    }
    public enum Element { fire, ice, electric }
    public void ElementDamage(Element type, bool True)
    {
        if (True)
        {
            switch (type)
            {
                case Element.fire:
                    {
                        if (!isBurned)
                        {
                            elementSystem = Instantiate(onFirePrefab, transform.position, Quaternion.identity, transform) as GameObject;
                            burnDamageCounter = UnityEngine.Random.Range(2, 6);
                            burnTime = burnTimer;
                            isBurned = true;
                        }
                        break;
                    }
                case Element.electric:
                    {
                        if (!isShocked)
                        {
                            elementSystem = Instantiate(onElectricPrefab, transform.position, Quaternion.identity, transform) as GameObject;
                            shockTime = shockTimer;
                            isShocked = true;
                        }
                        break;
                    }
                case Element.ice:
                    {
                        if (!isFrozen)
                        {
                            animSpeed = anim.speed;
                            elementSystem = Instantiate(onIcePrefab, transform.position, Quaternion.identity, transform) as GameObject;
                            frozeTime = frozeTimer;
                            isFrozen = true;
                        }
                        break;
                    }
            }
        }
        else
        {
            switch (type)
            {
                case Element.fire:
                    {
                        Destroy(elementSystem);
                        isBurned = false;
                        break;
                    }
                case Element.electric:
                    {
                        Destroy(elementSystem);
                        isShocked = false;
                        break;
                    }
                case Element.ice:
                    {
                        anim.speed = animSpeed;
                        Destroy(elementSystem);
                        isFrozen = false;
                        break;
                    }
            }
        }
    }
    public void SetDemonToad(bool materialActive)
    {
        foreach (MeshRenderer mesh in toadBodyRenders)
            mesh.enabled = materialActive;
    }
    public void ChangeDemonToadMaterial(bool changeMaterial)
    {
        if (changeMaterial)
            for(int c = 0; c < 31; c++)
                toadBodyRenders[c].material = new Material(demonToadMaterial);
        else
            for (int c = 0; c < 31; c++)
                toadBodyRenders[c].material = new Material(orgToadMaterial);
    }
    public void CloakToad()
    {
        if (!isCloaked)
        {
            enemySound = soundClips.toadDemonOn;
            PlaySounds();
            isCloaked = true;
        }
        else
        {
            enemySound = soundClips.toadDemonOff;
            PlaySounds();
            isCloaked = false;
        }
    }
    IEnumerator AudioFadeOut(AudioSource audio)
    {
        audio.volume = 1.0f;
        float MinVol = 0;
        for (float f = 1f; f > MinVol; f -= 0.05f)
        {
            audio.volume = f;
            yield return new WaitForSecondsRealtime(.1f);
            if (f <= 0.1)
            {
                audio.Stop();
                audio.volume = MinVol;
            }
        }
    }
    IEnumerator AudioFadeIn(AudioSource audio)
    {
        audio.volume = 0.0f;
        float MaxVol = 1;
        audio.Play();
        for (float f = 0f; f < MaxVol; f += 0.05f)
        {
            audio.volume = f;
            yield return new WaitForSecondsRealtime(.1f);
            if (f >= 0.9)
            {
                audio.volume = MaxVol;
            }
        }
    }
}

[Serializable]

public struct EnemySoundClips

{
   
    public AudioClip bossTemple1Music;
    [HideInInspector]
    public AudioClip currentTempleMusic;
    [HideInInspector]
    public AudioSource musicAudioSrc;

    [Header("Enemy Sounds")]
    public AudioClip death;
    public AudioClip hit;
    public AudioClip bloop;
    public AudioClip spray;
    public AudioClip swipe;
    [Space]
    [Header ("Toad Sounds")]
    public AudioClip[] toadHit;
    public AudioClip[] toadJump;
    public AudioClip toadSpin;
    public AudioClip toadLick;
    public AudioClip toadShock;
    public AudioClip toadDemonOn;
    public AudioClip toadDemonOff;
    public AudioClip toadPound;
}
[Serializable]

public struct ItemDrops
{
    public GameObject gold1;
    public GameObject gold2;
    public GameObject gold3;
    public GameObject gold4;
    public GameObject gold5;
    public GameObject gold6;
    public GameObject gold7;
    public GameObject gold8;
    public GameObject gold9;
    public GameObject gold10;
    public GameObject gold12;
    public GameObject gold20;
    public GameObject gold100;
    public GameObject lifeBerry;
    public GameObject rixile;
    public GameObject nicirPlant;
    public GameObject mindRemedy;
    public GameObject petriShroom;
    public GameObject reliefOintment;
}