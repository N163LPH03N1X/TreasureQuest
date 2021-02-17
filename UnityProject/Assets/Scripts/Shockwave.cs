using UnityEngine;
using System.Collections;

public class Shockwave : MonoBehaviour
{
    public AudioClip shockWaveSound;
    AudioSource audiosrc;
    public ShockType shockType;
    public Level damageLevel;
    bool isPlayer;
    bool isEnemy;
    bool isJewel;
    public float growthSpeed = 1.0f;
    public float fadePerSecond = 1.0f;
    int damageLvl;
    IEnumerator startShockwave;
    Color waveColor;
    Color shockColor;
    MeshRenderer meshRend;
    MeshCollider meshCol;
    SphereCollider sphereCol;
    ImpactReceiver impact;
    PlayerSystem playST;
    GameObject player;
    Vector3 originalScale =  new Vector3(0.01f, 0.01f, 0.01f);
    public Vector3 destinationScale = new Vector3(4.0f, 4.0f, 4.0f);
    [HideInInspector]
    public bool lvl5;
    [HideInInspector]
    public bool lvl4;
    [HideInInspector]
    public bool lvl3;
    [HideInInspector]
    public bool lvl2;
    [HideInInspector]
    public bool lvl1;

    public enum ShockType { Enemy, Player, Jewel, terraSword}

    void Start()
    {
        
        if (!isJewel)
        {
            meshCol = GetComponent<MeshCollider>();
        }
        else
            sphereCol = GetComponent<SphereCollider>();
        meshRend = GetComponent<MeshRenderer>();
        shockColor = meshRend.material.color;
        transform.localScale = originalScale;
        DamageLevel(damageLevel);
        SelectShockType(shockType);
        startShockwave = ScaleOverTime(5);
        StartCoroutine(startShockwave);
        audiosrc = GameObject.Find("Core/GameMusic&Sound/GameSound").GetComponent<AudioSource>();
        audiosrc.PlayOneShot(shockWaveSound);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            startShockwave = ScaleOverTime(5);
            StartCoroutine(startShockwave);
        }
    }
    public void SelectShockType(ShockType type)
    {
        switch (type)
        {
            case ShockType.Enemy:
                {
                    float f = 1f;
                    shockColor = new Color(0.6f, 0.8f, 1, shockColor.a);
                    meshRend.material.EnableKeyword("_EMISSION");
                    meshRend.material.color = shockColor;
                    meshRend.material.SetColor("_EmissionColor", Color.blue * f);
                    meshCol = GetComponent<MeshCollider>();
                    meshCol.isTrigger = false;
                    isEnemy = true;
                    isPlayer = false;
                    isJewel = false;
                    break;
                }
            case ShockType.Player:
                {
                    Color color = Color.red;
                    float f = 1f;
                    shockColor = new Color(1, 0.6f, 0.6f, shockColor.a);
                    GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                    meshRend.material.color = shockColor;
                    GetComponent<Renderer>().material.SetColor("_EmissionColor", color * f);
                    meshCol = GetComponent<MeshCollider>();
                    meshCol.isTrigger = true;
                    isEnemy = false;
                    isPlayer = true;
                    isJewel = false;
                    break;

                }
            case ShockType.Jewel:
                {
                    shockColor = new Color(0.2f, 0f, 0.5f, 0.5f);
                    GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                    meshRend.material.color = shockColor;
                    sphereCol = GetComponent<SphereCollider>();
                    sphereCol.isTrigger = true;
                    isEnemy = false;
                    isPlayer = false;
                    isJewel = true;
                    break;
                }
            case ShockType.terraSword:
                {
                    isEnemy = false;
                    isPlayer = false;
                    isJewel = false;
                    break;
                }
        }
    }
    IEnumerator ScaleOverTime(float time)
    {
        float currentTime = 0.0f;
        do
        {
            transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
            currentTime += Time.deltaTime * growthSpeed;
            yield return null;
        }
        while (currentTime <= time);
        if(!isJewel)
            Destroy(gameObject);
    }
    public enum Level { One, Two, Three, Four, Five }
    public void DamageLevel(Level level)
    {
        switch (level)
        {
            case Level.One:
                {
                    lvl1 = true;
                    lvl2 = false;
                    lvl3 = false;
                    lvl4 = false;
                    lvl5 = false;
                    damageLvl = 1;
                    break;
                }
            case Level.Two:
                {
                    lvl1 = false;
                    lvl2 = true;
                    lvl3 = false;
                    lvl4 = false;
                    lvl5 = false;
                    damageLvl = 2;
                    break;
                }
            case Level.Three:
                {
                    lvl1 = false;
                    lvl2 = false;
                    lvl3 = true;
                    lvl4 = false;
                    lvl5 = false;
                    damageLvl = 3;
                    break;
                }
            case Level.Four:
                {
                    lvl1 = false;
                    lvl2 = false;
                    lvl3 = false;
                    lvl4 = true;
                    lvl5 = false;
                    damageLvl = 4;
                    break;
                }
            case Level.Five:
                {
                    lvl1 = false;
                    lvl2 = false;
                    lvl3 = false;
                    lvl4 = false;
                    lvl5 = true;
                    damageLvl = 5;
                    break;
                }
        }

    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !SceneSystem.isDisabled && !PlayerSystem.isDead && !PlayerSystem.isInvincible && isEnemy)
        {
            Vector3 direction = (collision.transform.position - transform.position).normalized;
            CharacterSystem characterCtrl = collision.gameObject.GetComponent<CharacterSystem>();
            characterCtrl.SetFalling();
            impact = collision.gameObject.GetComponent<ImpactReceiver>();

            int lifeBerry = ItemSystem.lifeBerryAmt;
            if (lifeBerry == 0)
            {
                if (PlayerSystem.playerHealth <= 1)
                    impact.AddImpact(direction, 30);
                else if (PlayerSystem.playerHealth > 1)
                    impact.AddImpact(direction, 200);
            }
            else if (lifeBerry > 0)
                impact.AddImpact(direction, 200);
            playST = collision.gameObject.GetComponent<PlayerSystem>();
        
            playST.PlayerDamage(damageLvl, false);
            Destroy(gameObject);
           
        }
    
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ISwordHittable>() != null && isPlayer)
        {
            
            EnemySystem enemySystem = other.gameObject.GetComponent<EnemySystem>();
            if (PlayerSystem.playerVitality >= 5 && PlayerSystem.playerVitality < 13)
                other.gameObject.SendMessage("OnGetHitBySword", 1);
            else if (PlayerSystem.playerVitality >= 13 && PlayerSystem.playerVitality < 18)
                other.gameObject.SendMessage("OnGetHitBySword", 2);
            else if (PlayerSystem.playerVitality >= 18 && PlayerSystem.playerVitality < 23)
                other.gameObject.SendMessage("OnGetHitBySword", 3);
            else if (PlayerSystem.playerVitality >= 23 && PlayerSystem.playerVitality < 28)
                other.gameObject.SendMessage("OnGetHitBySword", 4);
            else if (PlayerSystem.playerVitality >= 28 && PlayerSystem.playerVitality < 33)
                other.gameObject.SendMessage("OnGetHitBySword", 5);
            else if (PlayerSystem.playerVitality >= 33 && PlayerSystem.playerVitality < 38)
                other.gameObject.SendMessage("OnGetHitBySword", 6);
            else if (PlayerSystem.playerVitality >= 38 && PlayerSystem.playerVitality < 43)
                other.gameObject.SendMessage("OnGetHitBySword", 7);
            else if (PlayerSystem.playerVitality >= 43 && PlayerSystem.playerVitality < 48)
                other.gameObject.SendMessage("OnGetHitBySword", 8);
            else if (PlayerSystem.playerVitality >= 48 && PlayerSystem.playerVitality < 53)
                other.gameObject.SendMessage("OnGetHitBySword", 9);
            else if (PlayerSystem.playerVitality >= 53 && PlayerSystem.playerVitality < 58)
                other.gameObject.SendMessage("OnGetHitBySword", 10);
            else if (PlayerSystem.playerVitality >= 58 && PlayerSystem.playerVitality < 63)
                other.gameObject.SendMessage("OnGetHitBySword", 11);
            else if (PlayerSystem.playerVitality >= 63 && PlayerSystem.playerVitality < 68)
                other.gameObject.SendMessage("OnGetHitBySword", 12);
            else if (PlayerSystem.playerVitality >= 68 && PlayerSystem.playerVitality < 73)
                other.gameObject.SendMessage("OnGetHitBySword", 13);
            else if (PlayerSystem.playerVitality >= 73 && PlayerSystem.playerVitality < 78)
                other.gameObject.SendMessage("OnGetHitBySword", 14);
            else if (PlayerSystem.playerVitality >= 78 && PlayerSystem.playerVitality < 83)
                other.gameObject.SendMessage("OnGetHitBySword", 15);
            else if (PlayerSystem.playerVitality >= 83 && PlayerSystem.playerVitality < 88)
                other.gameObject.SendMessage("OnGetHitBySword", 16);
            else if (PlayerSystem.playerVitality >= 88 && PlayerSystem.playerVitality < 93)
                other.gameObject.SendMessage("OnGetHitBySword", 17);
            else if (PlayerSystem.playerVitality >= 93 && PlayerSystem.playerVitality < 96)
                other.gameObject.SendMessage("OnGetHitBySword", 18);
            else if (PlayerSystem.playerVitality >= 96 && PlayerSystem.playerVitality < 100)
                other.gameObject.SendMessage("OnGetHitBySword", 19);
            else if (PlayerSystem.playerVitality >= 100)
                other.gameObject.SendMessage("OnGetHitBySword", 20);
        }
    }
}
