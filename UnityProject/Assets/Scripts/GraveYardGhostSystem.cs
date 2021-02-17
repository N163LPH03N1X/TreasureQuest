using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GraveYardGhostSystem : MonoBehaviour, ISwordHittable
{
    public Ghost ghost;
    NavMeshAgent nav;
    Vector3 player;
    int health = 1;
    bool isDying;
    AudioSource audioSrc;
    public AudioClip death;
    public AudioClip possess;
    MeshRenderer meshRend;
    ImpactReceiver impactreceiver;
    PlayerSystem playerSystem;
    SphereCollider sCollider;
    float fadePerSecond = 0.5f;
    Vector3 startingSize = new Vector3(0.01f, 0.01f, 0.01f);
    Vector3 finalSize = new Vector3(1, 1, 1);
    bool isGrowning = true;
    GameObject ghostParent;
    bool hitPlayer = false;
    bool hitNone = false;
    void Start()
    {
        ghostParent = transform.parent.gameObject;
        nav = ghostParent.GetComponent<NavMeshAgent>();
        meshRend = GetComponent<MeshRenderer>();
        sCollider = GetComponent<SphereCollider>();
        transform.localScale = startingSize;
        ghostParent.transform.rotation = new Quaternion(ghostParent.transform.rotation.x, 180, ghostParent.transform.rotation.z, ghostParent.transform.rotation.w);
        SetGhost(ghost);
    }
    public enum Ghost { Level1, Level2, Level3, Level4 }
    public void SetGhost(Ghost type)
    {
        switch (type)
        {
            case Ghost.Level1:
                {
                    nav.speed = 7f;
                    Color color = new Color(1f, 1f, 1f, 0.15f);
                    meshRend.material.color = color;
                    break;
                }
            case Ghost.Level2:
                {
                    nav.speed = 10f;
                    Color color = new Color(0.36f, 0.85f, 1f, 0.15f);
                    meshRend.material.color = color;
                    break;
                }
            case Ghost.Level3:
                {
                    nav.speed = 15f;
                    Color color = new Color(0.6f, 1f, 0.7f, 0.15f);
                    meshRend.material.color = color;
                    break;
                }
            case Ghost.Level4:
                {
                    nav.speed = 20f;
                    Color color = new Color(0.5f, 0.5f, 1f, 0.15f);
                    meshRend.material.color = color;
                    break;
                }
        }
    }
    public void ScaleOverTime(float speed)
    {
        Vector3 adjScale = new Vector3();
        adjScale.x = Mathf.Clamp(transform.localScale.x + speed, startingSize.x, finalSize.x);
        adjScale.y = Mathf.Clamp(transform.localScale.y + speed, startingSize.y, finalSize.y);
        adjScale.z = Mathf.Clamp(transform.localScale.z + speed, startingSize.z, finalSize.z);
        transform.localScale = adjScale;
       
    }
      
    // Update is called once per frame
    void Update()
    {
        if (isGrowning)
        {
            ScaleOverTime(0.1f);
            if (transform.localScale.magnitude >= 1)
                isGrowning = false;
        }
        if (!PlayerSystem.isDead)
        {
            if (nav.isStopped)
            {
                nav.isStopped = false;
                nav.updateRotation = true;
                nav.updatePosition = true;
            }
            player = GameObject.FindGameObjectWithTag("Player").transform.position;
            nav.SetDestination(player);

        }
        
        else
        {
            nav.isStopped = true;
            nav.updateRotation = false;
            nav.updatePosition = false;
        }
        if (isDying)
        {
            nav.isStopped = true;
            nav.updateRotation = false;
            nav.updatePosition = false;
            FadeEnemy();
        }



    }
 
    public void OnGetHitBySword(int hitValue)
    {
        DamageEnemy(hitValue);
    }
    public void DamageEnemy(int amount)
    {

        if (!isDying)
        {
            health -= amount;
            if (health < 1)
            {
                sCollider.enabled = false;
                if (!hitPlayer && !hitNone)
                {
                    audioSrc = GetComponent<AudioSource>();
                    audioSrc.PlayOneShot(death);
                }
                isDying = true;
                health = 1;
            }

        }
    }
    public void FadeEnemy()
    {
        SetupMaterialWithBlendMode(meshRend.material, BlendMode.Fade);
        Color color = meshRend.material.color;
        meshRend.material.color = new Color(color.r, color.g, color.b, color.a - (fadePerSecond * Time.deltaTime));
        if (meshRend.material.color.a < 0.15f)
            Destroy(transform.parent.gameObject, 1);
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
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !SceneSystem.isDisabled && !PlayerSystem.isDead && !PlayerSystem.isInvincible)
        {
            CharacterSystem characterCtrl = collision.gameObject.GetComponent<CharacterSystem>();

            impactreceiver = collision.gameObject.GetComponent<ImpactReceiver>();
            Vector3 direction = (collision.transform.position - transform.position).normalized;


            int lifeBerry = ItemSystem.lifeBerryAmt;
            if (lifeBerry == 0)
            {
                if (PlayerSystem.playerHealth <= 1)
                    impactreceiver.AddImpact(direction, 30);
                else if (PlayerSystem.playerHealth > 1)
                    impactreceiver.AddImpact(direction, 50);
            }
            else if (lifeBerry > 0)
                impactreceiver.AddImpact(direction, 50);
            playerSystem = collision.gameObject.GetComponent<PlayerSystem>();
            audioSrc = GetComponent<AudioSource>();
            audioSrc.PlayOneShot(possess);
            SetHitSound(1);
            playerSystem.PlayerDamage(1, false);
            characterCtrl.SetFalling();
         
            DamageEnemy(1);
        }
        
    }
    public void SetHitSound(int num)
    {
        if (num == 0)
            hitNone = true;
        else if (num == 1)
            hitPlayer = true;
    }
}
