using UnityEngine;
public class BoulderDrop : MonoBehaviour
{
    public Direction direction;
    public float maxVelocity;
    public float gravityForce = 9.81f;
    public float speed;
    public float orgSpeed;
    float dropSpeed = 80f;
    Rigidbody rb;
    Vector3 orgPos;
    AudioSource audioSrc;
    ImpactReceiver impact;
    PlayerSystem playST;
    ConstantForce gravity;
    bool X;
    bool Z;
    bool startMoving = false;
    public DamageLevel level;
    public bool lvl5;
    public bool lvl4;
    public bool lvl3;
    public bool lvl2;
    public bool lvl1;
    int damageLevel;
    

    void Start()
    {
        gravity = gameObject.GetComponent<ConstantForce>();
        rb = GetComponent<Rigidbody>();
        audioSrc = GetComponent<AudioSource>();
        orgPos = transform.localPosition;
        orgSpeed = speed;
        speed = dropSpeed;

    }
    void Update()
    {
        if (startMoving)
        {
            if (Z)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationZ;
                Move(Vector3.forward);
            }
            else if (!Z)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationZ;
                Move(Vector3.back);
            }
            else if (X)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX;
                Move(Vector3.right);
            }
            else if (!X)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX;
                Move(Vector3.left);
            }
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            Move(Vector3.down);
          
        }
    }
    public enum DamageLevel { One, Two, Three, Four, Five }
    public enum Direction { left, right, forward, back }
    public void Move(Vector3 direction)
    {
        rb.AddForce(direction * speed, ForceMode.Acceleration);
        if (rb.velocity.magnitude > maxVelocity)
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        gravity.force = new Vector3(0, gravityForce, 0);
       
    }
    public void SetDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.forward:
                {
                    Z = true;
                   
                    break;
                }
            case Direction.back:
                {
                    Z = false;
                
                    break;
                }
            case Direction.left:
                {
                    X = true;
                   
                    break;
                }
            case Direction.right:
                {
                    X = false;
     
                    break;
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
            collision.gameObject.SendMessage("OnGetHitBySword", 100);
        if (collision.gameObject.CompareTag("BoulderEnd"))
        {
            transform.localPosition = orgPos;
            speed = dropSpeed;
            audioSrc.Stop();
            startMoving = false;
        }
        else if (collision.gameObject.CompareTag("BoulderStart"))
        {
            speed = orgSpeed;
            startMoving = true;
            audioSrc.Play();
            SetDirection(direction);
        }
        if (collision.gameObject.CompareTag("Player") && !SceneSystem.isDisabled && !PlayerSystem.isDead && !PlayerSystem.isInvincible)
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

            SpikeDamageLevel(level);
            playST.PlayerDamage(damageLevel, false);
        }
    }
}
