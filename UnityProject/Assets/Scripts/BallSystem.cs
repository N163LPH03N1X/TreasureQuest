using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.AI;



public class BallSystem : MonoBehaviour
{
    NavMeshAgent nav;
    ImpactReceiver impact;
    PlayerSystem playST;
    GameObject player;

    IEnumerator moveRoutine;
    public Level level;
    public NumberOfPositions positions;
    public MoveType moveType;
    public ObjectType objType;
    public Transform[] ballPosition;
    Vector3 setPos;
    public int currentPos = 0;
    int damageLevel;
    public float resetTime;
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
    bool isMoving = false;
    bool switchPosition;
    bool alreadySwitched;
    public bool disableDamage;
    public void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        StartMovement(positions, moveType);
  
    }
    public enum NumberOfPositions { None, Position2, Position4 }
    public enum ObjectType { Ball, Spike }
    public enum Level { One, Two, Three, Four, Five }
    public enum MoveType { X, Z, XZ }
    IEnumerator DelayedAction( UnityAction action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

    void Update()
    {
        if (isMoving)
        {
            nav.SetDestination(setPos);
            
        }
    }
 
    
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
                    damageLevel = 1;
                    break;
                }
            case Level.Two:
                {
                    lvl1 = false;
                    lvl2 = true;
                    lvl3 = false;
                    lvl4 = false;
                    lvl5 = false;
                    damageLevel = 2;
                    break;
                }
            case Level.Three:
                {
                    lvl1 = false;
                    lvl2 = false;
                    lvl3 = true;
                    lvl4 = false;
                    lvl5 = false;
                    damageLevel = 3;
                    break;
                }
            case Level.Four:
                {
                    lvl1 = false;
                    lvl2 = false;
                    lvl3 = false;
                    lvl4 = true;
                    lvl5 = false;
                    damageLevel = 4;
                    break;
                }
            case Level.Five:
                {
                    lvl1 = false;
                    lvl2 = false;
                    lvl3 = false;
                    lvl4 = false;
                    lvl5 = true;
                    damageLevel = 5;
                    break;
                }
        }

    }
    public void StartMovement(NumberOfPositions pos, MoveType move)
    {
        switch (pos)
        {
            case NumberOfPositions.None:
                {
                    isMoving = false;
                    setPos = transform.position;
                    moveRoutine = DelayedAction(delegate { StartMovement(pos, move); }, 0);
                    StartCoroutine(moveRoutine);
                    break;
                }
            case NumberOfPositions.Position2:
                {
                    isMoving = true;
                    switch (move)
                    {
                        case MoveType.X:
                            {
                                if (transform.position.x == ballPosition[0].position.x )
                                {
                                    currentPos = 1;
                                    alreadySwitched = false;
                                }
                                else if (transform.position.x == ballPosition[1].position.x)
                                {
                                    currentPos = 0;
                                    alreadySwitched = false;
                                }
                                else if (switchPosition && !alreadySwitched)
                                {
                                    if (currentPos == 0)
                                        currentPos = 1;
                                    else if (currentPos == 1)
                                        currentPos = 0;
                                    switchPosition = false;
                                    alreadySwitched = true;

                                }
                                setPos = ballPosition[currentPos].position;
                                moveRoutine = DelayedAction(delegate { StartMovement(pos, move); }, 0);
                                StartCoroutine(moveRoutine);
                                break;
                            }
                        case MoveType.Z:
                            {
                                if (transform.position.z == ballPosition[0].position.z)
                                {
                                    currentPos = 1;
                                    alreadySwitched = false;
                                }
                                else if (transform.position.z == ballPosition[1].position.z)
                                {
                                    currentPos = 0;
                                    alreadySwitched = false;
                                }
                                else if (switchPosition && !alreadySwitched)
                                {
                                    if (currentPos == 0)
                                        currentPos = 1;
                                    else if (currentPos == 1)
                                        currentPos = 0;
                                    switchPosition = false;
                                    alreadySwitched = true;

                                }
                                setPos = ballPosition[currentPos].position;
                                moveRoutine = DelayedAction(delegate { StartMovement(pos, move); }, 0);
                                StartCoroutine(moveRoutine);
                                break;
                            }
                    }
                    break;
                }
            
            case NumberOfPositions.Position4:
                {
                    isMoving = true;

                    switch (move)
                    {
                        case MoveType.XZ:
                            {
                                if (transform.position.z == ballPosition[0].position.z && transform.position.x == ballPosition[0].position.x)
                                {
                                    currentPos = 1;
                                    alreadySwitched = false;
                                }
                                else if (transform.position.z == ballPosition[1].position.z && transform.position.x == ballPosition[1].position.x)
                                {
                                    currentPos = 2;
                                    alreadySwitched = false;
                                }
                                else if (transform.position.z == ballPosition[2].position.z && transform.position.x == ballPosition[2].position.x)
                                {
                                    currentPos = 3;
                                    alreadySwitched = false;
                                }
                                else if (transform.position.z == ballPosition[3].position.z && transform.position.x == ballPosition[3].position.x)
                                {
                                    currentPos = 0;
                                    alreadySwitched = false;
                                }
                                else if (switchPosition && !alreadySwitched)
                                {
                                    if (currentPos == 0)
                                        currentPos = 1;
                                    else if (currentPos == 1)
                                        currentPos = 2;
                                    else if (currentPos == 2)
                                        currentPos = 3;
                                    else if (currentPos == 3)
                                        currentPos = 0;
                                    switchPosition = false;
                                    alreadySwitched = true;

                                }
                                setPos = ballPosition[currentPos].position;
                                moveRoutine = DelayedAction(delegate { StartMovement(pos, move); }, 0);
                                StartCoroutine(moveRoutine);
                                break;
                            }
                    }
                    break;
                }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !SceneSystem.isDisabled && !PlayerSystem.isDead && !PlayerSystem.isInvincible && !disableDamage)
        {
            switchPosition = true;
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
            DamageLevel(level);
            playST.PlayerDamage(damageLevel, false);
            if (moveRoutine != null)
                StopCoroutine(moveRoutine);
            StartMovement(positions, moveType);

        }
        else if (collision.gameObject.CompareTag("Carry") && !switchPosition)
        {
            Rigidbody objRb = collision.gameObject.GetComponent<Rigidbody>();
            objRb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            if (moveRoutine != null)
                StopCoroutine(moveRoutine);
            StartMovement(positions, moveType);
            switchPosition = true;

        }
        else if (collision.gameObject.CompareTag("Platform") && !switchPosition)
        {
            Rigidbody objRb = collision.gameObject.GetComponent<Rigidbody>();
            objRb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            switchPosition = true;
            if(moveRoutine != null)
                StopCoroutine(moveRoutine);
            StartMovement(positions, moveType);
        }
    }

    public void SelectObjectType(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.Ball:
                {
                    break;
                }
            case ObjectType.Spike:
                {
                    SphereCollider col = GetComponent<SphereCollider>();
                    col.enabled = false;
                    break;
                }
        }
    }
    

}
