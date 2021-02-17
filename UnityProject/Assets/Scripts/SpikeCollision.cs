using UnityEngine;
using System.Collections;


public class SpikeCollision : MonoBehaviour
{
    AudioSource audioSrc;
    ImpactReceiver impact;
    PlayerSystem playST;
    public DamageLevel level;
    public bool lvl5;
    public bool lvl4;
    public bool lvl3;
    public bool lvl2;
    public bool lvl1;
    int damageLevel;
    bool isSound = false;
    public AudioClip spikeSfx;
    [Header("Spike Settings")]
    public float dropTimer;
    public float raiseTimer;
    public float dropSpeed;
    public float raiseSpeed;
    public float maxHeight;
    public float minHeight;
    public bool drop = true;
    public bool stationed;
    IEnumerator dropRoutine = null;
    bool isPlayerHit;


    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        SpikeDamageLevel(level);
        dropRoutine = StartSpikeDrop(drop);
        if (!stationed)
            StartCoroutine(dropRoutine);
    }
    public enum DamageLevel { One, Two, Three, Four, Five }
    public IEnumerator StartSpikeDrop(bool active)
    {
        if (!active)
        {
            while (transform.localPosition.y < maxHeight)
            {
                transform.Translate(Vector3.up * Time.deltaTime * raiseSpeed);
                if (transform.localPosition.y >= maxHeight)
                {
                    Vector3 setPos = new Vector3 (transform.localPosition.x, maxHeight, transform.localPosition.z);
                    transform.localPosition = setPos;
                    isPlayerHit = false;
                    yield return new WaitForSeconds(dropTimer);
                    drop = true;
                    isSound = false;
                    if (dropRoutine != null)
                        StopCoroutine(dropRoutine);
                    dropRoutine = StartSpikeDrop(drop);
                    StartCoroutine(dropRoutine);
                    break;

                }
                yield return new WaitForEndOfFrame();
            }

        }
        if (active)
        {
            while (transform.localPosition.y > minHeight)
            {
                transform.Translate(Vector3.down * Time.deltaTime * dropSpeed);
                if (transform.localPosition.y <= minHeight)
                {
                    Vector3 setPos = new Vector3(transform.localPosition.x, minHeight, transform.localPosition.z);
                    transform.localPosition = setPos;
                    yield return new WaitForSeconds(raiseTimer);
                    drop = false;
                    if (dropRoutine != null)
                        StopCoroutine(dropRoutine);
                    dropRoutine = StartSpikeDrop(drop);
                    StartCoroutine(dropRoutine);
                    break;
                }
                yield return new WaitForEndOfFrame();

            }
        }
    }
    public void SpikeDamageLevel(DamageLevel level)
    {
        switch (level)
        {
            case DamageLevel.One:
                {
                    lvl1 = true;
                    lvl2 = false;
                    lvl3 = false;
                    lvl4 = false;
                    lvl5 = false;
                    damageLevel = 2;
                    break;
                }
            case DamageLevel.Two:
                {
                    lvl1 = false;
                    lvl2 = true;
                    lvl3 = false;
                    lvl4 = false;
                    lvl5 = false;
                    damageLevel = 4;
                    break;
                }
            case DamageLevel.Three:
                {
                    lvl1 = false;
                    lvl2 = false;
                    lvl3 = true;
                    lvl4 = false;
                    lvl5 = false;
                    damageLevel = 6;
                    break;
                }
            case DamageLevel.Four:
                {
                    lvl1 = false;
                    lvl2 = false;
                    lvl3 = false;
                    lvl4 = true;
                    lvl5 = false;
                    damageLevel = 8;
                    break;
                }
            case DamageLevel.Five:
                {
                    lvl1 = false;
                    lvl2 = false;
                    lvl3 = false;
                    lvl4 = false;
                    lvl5 = true;
                    damageLevel = 10;
                    break;
                }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ISwordHittable>() != null)
        {
            if(collision.gameObject.GetComponent<GraveYardGhostSystem>() != null)
            {
                GraveYardGhostSystem graveSystem = collision.gameObject.GetComponent<GraveYardGhostSystem>();
                graveSystem.SetHitSound(0);
                collision.gameObject.SendMessage("OnGetHitBySword", 100);
            }
            else if(collision.gameObject.GetComponent<EnemySystem>() != null)
                collision.gameObject.SendMessage("OnGetHitBySword", 100);


        }
        if (collision.gameObject.CompareTag("Player") && !SceneSystem.isDisabled && !PlayerSystem.isDead && !PlayerSystem.isInvincible && !isPlayerHit)
        {
            Vector3 direction = (collision.transform.position - transform.position).normalized;

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

            SpikeDamageLevel(level);
            playST.PlayerDamage(damageLevel, false);
            if (!stationed)
            {
                drop = false;
                if (dropRoutine != null)
                    StopCoroutine(dropRoutine);
                dropRoutine = StartSpikeDrop(false);
                StartCoroutine(dropRoutine);
            }
            isPlayerHit = true;
            CharacterSystem characterCtrl = collision.gameObject.GetComponent<CharacterSystem>();
            characterCtrl.SetFalling();
        }
        else
        {
            if (drop && !isSound)
            {
                audioSrc.PlayOneShot(spikeSfx);
                isSound = true;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isPlayerHit = false;
    }

}
