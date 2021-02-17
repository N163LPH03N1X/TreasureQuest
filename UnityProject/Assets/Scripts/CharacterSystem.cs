using System;
using UnityEngine;

public class CharacterSystem : MonoBehaviour
{
    //Call Variables
    Animator anim;
    Vector3 rigidbodyMoveDirection;
    Vector3 rigidbodyStrafeDirection;
    CharacterController characterController;
    AudioSource audioSrc;
    PlayerSystem playST;
    Vector3 contactPoint;
    Rigidbody rb;
    Vector3 lastPlayerPosition;
    Vector3 moveDirection = Vector3.zero;
    Transform myTransform;
    RaycastHit hit;
    //Floats
    float fallStartLevel;
    float gravity;
    float lastFootstepPlayedTime;
    float velocityPollPeriod = 0.2f;
    float currentFootstepsWaitingPeriod;
    float fallingDamageThreshold = 10.0f;
    float horizontalSpeed = 0;
    float verticalSpeed = 0;
    //Ints
    int jumpTimer;
    //Bools
    bool fallDamage = false;


    bool water;
    bool ice;
    bool grass;
    bool metal;
    bool wood;
    bool lava;
    bool playerControl = false;
    bool isInteraction = false;
    //Static Bools
    public static bool isMoving = false;
    public static bool isGrounded = true;
    public static bool isRunning = false;
    public static bool isJumping = false;
    public static bool isClimbing = false;
    public static bool isConfused = false;
    public static bool isParalyzed = false;
    public static bool isIcy = false;

    [Header("Swimming Settings")]
    //=======================Mouse & Keyboard=================================//
    float VerticalMovement;
    public float swimSpeed;
    public float LookSensitivity;
    public float SnappingSpeed;
    public float maxVelocity;
    [Space]
    [Header("OnIce Settings")]
    public float rbWalkSpeed;
    public float rbRunSpeed;
    public float rbPlayerSpeed;
    public float decelerationSpeed;
    public float rbJumpSpeed;
    [Space]
    [Header("On Ground Settings")]
    public PlayerSounds playerSounds;
    public GameObject playerController;
    public AudioSource audioSrc2;

    public float walkSpeed = 6.0f;
    public float runSpeed = 11.0f;
    public float playerSpeed = 6.0f;
    public float jumpSpeed = 8.0f;
    public int antiBunnyHopFactor = 1;
    public float antiBumpFactor = .75f;
    public float gravityPull;
    public float stepSoundRatioA = 0.24f;
    public float stepSoundRatioN = 2.0f;
    public float stepSoundRatioB = 1.68f;
    public float stepSoundRatioC = 159.0f;
    public float timePerStep;
    public float stepsPerSecond;
    
    public float climbHeightFactor;
    public float slideLimit;
    public float slideSpeed = 12.0f;

    public bool isActive;
    //public bool isPlatform = false;
    public bool isFalling = false;
    public bool isMovingForward;
    public bool isMovingBack;
    public bool isIdle;
    public bool isMovingRight;
    public bool isMovingLeft;
    public bool isSwimming;
    public static bool isCarrying;
    public bool airControl = false;
    public bool limitDiagonalSpeed = true;
    public bool slideWhenOverSlopeLimit = false;
    public bool slideOnTaggedObjects = false;

    [Header("Carrying Settings")]
    public Transform carryObjNewPos;
    public bool carryingObj;
    public bool inRange;
    public float Throwforce;
    Rigidbody boxRb;
    GameObject carryObj;
    Transform originalParent;
    bool wasPaused;
    float heightForce;
    float heighttobegin;

    [Header("Ledge Grab Settings")]
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private CineCamera cineCamera;
    [SerializeField]
    private float overlapBoxYTranslation;
    [SerializeField]
    private float overlapBoxForwardTranslation;
    private Transform fpsControllerTransform;
    float currentGrabDistance;
    public float hitAngle = 30.0f;


    public static bool isCinematic;
    private void Start()
    {
        InvokeRepeating("EstimatePlayerVelocity_InvokeRepeating", 1.0f, velocityPollPeriod);
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        audioSrc = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        playerSpeed = 0;
        jumpTimer = antiBunnyHopFactor;
        gravity = gravityPull;
        myTransform = transform;
        heighttobegin = transform.position.y;
        currentGrabDistance = transform.position.y;
        characterController = GetComponent<CharacterController>();
        fpsControllerTransform = transform;

    }
    private void OnDrawGizmosSelected()
    {
        if (fpsControllerTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = fpsControllerTransform.localToWorldMatrix;
            Gizmos.DrawWireCube(new Vector3(.0f, overlapBoxYTranslation,
                overlapBoxForwardTranslation), new Vector3(2.0f, 2.0f, 2.0f));
        }
    }
    void FixedUpdate()
    {
        if(!isCinematic)
            MovePlayer();
    }
    void Update()
    {

        if (isJumping)
        {
            heightForce = transform.position.y - heighttobegin;
        }
        else
        {
            heightForce = 0;
            heighttobegin = transform.position.y;
        }
        if (PauseGame.isPaused && !wasPaused)
        {
            anim.updateMode = AnimatorUpdateMode.Normal;
            wasPaused = true;
        }
        else if (!PauseGame.isPaused && wasPaused)
        {
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
            wasPaused = false;
        }
        if (carryObj == null && isCarrying && anim.GetBool("Carry") == true || carryObj == null && !isCarrying && anim.GetBool("Carry") == true)
        {
            SelectAnimation(PlayerAnimation.Carry, false);
            isCarrying = false;
            SelectAnimation(PlayerAnimation.Rebind, true);
            carryingObj = false;
        }
        if (inRange && !isCarrying && carryObj != null)
        {
            if (Input.GetButtonDown("Sheath") && !carryingObj
                && SwordSystem.UnArmed && !PlayerSystem.isDead && !SceneSystem.isDisabled
                && !PauseGame.isPaused && !SelectionalSystem.isSelecting && !ShopSystem.isShop
                && !DialogueSystem.isDialogueActive && !JewelSystem.isJewelEnabled)
            {
                SelectAnimation(PlayerAnimation.Carry, true);
                isCarrying = true;
                inRange = false;
            }

        }
        else if (Input.GetButtonDown("Sheath") && carryingObj && !inRange
            && SwordSystem.UnArmed && !PlayerSystem.isDead && !SceneSystem.isDisabled
            && !PauseGame.isPaused && !SelectionalSystem.isSelecting && !ShopSystem.isShop
            && !DialogueSystem.isDialogueActive && isJumping)
        {
            SelectAnimation(PlayerAnimation.Carry, false);
            SelectAnimation(PlayerAnimation.Throw, true);

        }
       
        else if (Input.GetButtonDown("Sheath") && carryingObj && !inRange
            && SwordSystem.UnArmed && !PlayerSystem.isDead && !SceneSystem.isDisabled
            && !PauseGame.isPaused && !SelectionalSystem.isSelecting && !ShopSystem.isShop
            && !DialogueSystem.isDialogueActive && !isJumping)
        {
            SelectAnimation(PlayerAnimation.Carry, false);
            SelectAnimation(PlayerAnimation.Drop, true);
        }




        if (SceneSystem.isDisabled)
        {
            isGrounded = false;
            isActive = false;
        }
        if (!PlayerSystem.isDead && !SceneSystem.isDisabled && !PauseGame.isPaused && !SelectionalSystem.isSelecting && !ShopSystem.isShop && !DialogueSystem.isDialogueActive) {
            if (Input.GetAxis("Horizontal") > 0)
                SwitchMovement(MovementType.right);
            else if (Input.GetAxis("Horizontal") < 0)
                SwitchMovement(MovementType.left);
            else if (Input.GetAxis("Vertical") > 0)
                SwitchMovement(MovementType.forward);
            else if (Input.GetAxis("Vertical") < 0)
                SwitchMovement(MovementType.back);
            else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                SwitchMovement(MovementType.none);
            if (!UnderWaterSystem.isSwimming && !isIcy)
            {
                if (Input.GetButton("Sprint") && !isMovingBack && !isCarrying)
                {
                    isRunning = true;
                    if (JewelSystem.blueJewelEnabled && JewelSystem.isJewelActive)
                        playerSpeed = runSpeed * 2;
                    else
                        playerSpeed = runSpeed;
                }
                else
                {
                    isRunning = false;
                    if(JewelSystem.blueJewelEnabled && JewelSystem.isJewelActive)
                        playerSpeed = walkSpeed * 2;
                    else
                        playerSpeed = walkSpeed;
                }
                if (Time.time - lastFootstepPlayedTime > currentFootstepsWaitingPeriod && isMoving && isGrounded && !isJumping)
                {
                    if (isRunning)
                    {
                        audioSrc.volume = UnityEngine.Random.Range(0.5f, 0.8f);
                        audioSrc.pitch = UnityEngine.Random.Range(0.9f, 1f);
                        SetFootStep();
                    }
                    else
                    {
                        audioSrc.volume = UnityEngine.Random.Range(0.5f, 0.8f);
                        audioSrc.pitch = UnityEngine.Random.Range(0.6f, 0.8f);
                        SetFootStep();
                    }
                    if (!isJumping)
                        audioSrc.Play();
                    lastFootstepPlayedTime = Time.time;
                }
            }
        }
    }
    public void SetCinematicCamera(bool active)
    {
        isCinematic = active;
        if (isCinematic)
        {
            SelectAnimation(PlayerAnimation.Rebind, true);
            if (isRunning)
                SelectAnimation(PlayerAnimation.Run, true);
            else if (!isRunning)
                SelectAnimation(PlayerAnimation.Walk, true);

        }
        else
        {
            if (isClimbing)
                SwitchMovement(MovementType.climbStop);
            jumpTimer = 0;
            isJumping = false;
        }

    }
    public void MovePlayer()
    {
        //========================PlayerMovementWalking========================================//
        if (!SceneSystem.isDisabled && !PauseGame.isPaused && !ShopSystem.isShop && !DialogueSystem.isDialogueActive && !PlayerSystem.isDead && !UnderWaterSystem.isSwimming && !isIcy)
        {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            CapsuleCollider capCol = GetComponent<CapsuleCollider>();
            capCol.enabled = false;
            CharacterController charCtrl = GetComponent<CharacterController>();
            charCtrl.enabled = true;

            float inputX;
            float inputY;

            Vector3 horizontalVelocity = characterController.velocity;
            horizontalVelocity = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);

            horizontalSpeed = horizontalVelocity.magnitude;
            verticalSpeed = characterController.velocity.y;
            if (!SelectionalSystem.isSelecting)
            {
                if (isConfused && !isParalyzed)
                {
                    inputX = -Input.GetAxis("Horizontal");
                    inputY = -Input.GetAxis("Vertical");
                }

                else if (isParalyzed)
                {
                    inputX = 0;
                    inputY = 0;
                }
                else
                {
                    inputX = Input.GetAxis("Horizontal");
                    inputY = Input.GetAxis("Vertical");
                }

                float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && limitDiagonalSpeed) ? .7071f : 1.0f;


                if (isGrounded && !isClimbing)
                {
                    fallDamage = false;
                    bool sliding = false;
                    heightForce = 0.0f;
                   
                    if (Physics.Raycast(myTransform.position, -Vector3.up, out hit))
                    {
                        if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
                            sliding = true;
                    }
                    else
                    {
                        Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, out hit);
                        if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
                            sliding = true;
                    }

                    if (isFalling)
                        SwitchMovement(MovementType.notFall);


                    if ((sliding && slideWhenOverSlopeLimit) || (sliding && slideOnTaggedObjects && hit.collider.tag == "Slide"))
                    {
                        Vector3 hitNormal = hit.normal;
                        moveDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
                        Vector3.OrthoNormalize(ref hitNormal, ref moveDirection);
                        moveDirection *= slideSpeed;
                        playerControl = false;
                    }
                    else if (!isClimbing)
                    {

                        moveDirection = new Vector3(inputX, -antiBumpFactor, inputY);

                        moveDirection = myTransform.TransformDirection(moveDirection) * playerSpeed;
                        playerControl = true;
                    }
                    if (!isParalyzed && !isInteraction)
                        SwitchMovement(MovementType.jump);
                }
                else
                {
                    isActive = true;
                    if (!isFalling && !SceneSystem.isDisabled && !isClimbing && !isInteraction && !StorySystem.isReading)
                        SwitchMovement(MovementType.fall);

                    if (airControl && playerControl)
                    {
                        moveDirection.x = inputX * playerSpeed * inputModifyFactor;
                        moveDirection.z = inputY * playerSpeed * inputModifyFactor;
                        moveDirection = myTransform.TransformDirection(moveDirection);
                    }
                }
            }
            if (!isClimbing)
                moveDirection.y -= gravity * Time.unscaledDeltaTime;
            else if (isClimbing)
            {
                if (Input.GetAxis("Vertical") == 1)
                    SwitchMovement(MovementType.climbUp);
                else if (Input.GetAxis("Vertical") == -1)
                    SwitchMovement(MovementType.climbDown);
                else if (Input.GetAxis("Vertical") == 0)
                    SwitchMovement(MovementType.climbHold);
            }
            if (isActive && !SelectionalSystem.isSelecting)
                isGrounded = (characterController.Move(moveDirection * Time.unscaledDeltaTime) & CollisionFlags.Below) != 0;
            else if (isActive && SelectionalSystem.isSelecting)
            {
                moveDirection.x = 0;
                moveDirection.z = 0;
                isGrounded = (characterController.Move(moveDirection * Time.unscaledDeltaTime) & CollisionFlags.Below) != 0;
            }
        }

        //========================PlayerMovementSwimming========================================//

        else if (!SceneSystem.isDisabled && !PauseGame.isPaused && !ShopSystem.isShop && !DialogueSystem.isDialogueActive && !PlayerSystem.isDead && !SelectionalSystem.isSelecting && UnderWaterSystem.isSwimming)
        {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            isGrounded = false;
            isActive = false;
            CharacterController charCtrl = GetComponent<CharacterController>();
            charCtrl.enabled = false;
            CapsuleCollider capCol = GetComponent<CapsuleCollider>();
            capCol.enabled = true;
            capCol.center = new Vector3(0, 1.5f, 0);
            capCol.height = 1f;

            Camera.main.transform.rotation = new Quaternion(0, 0, 0, 0);
            
            float MouseY = Input.GetAxis("Mouse Y");
            float MouseX = Input.GetAxis("Mouse X");

            if (MouseX > 0)
                transform.Rotate(Vector3.down * -MouseX * LookSensitivity * Time.unscaledDeltaTime, Space.Self);
            if (MouseX < 0)
                transform.Rotate(Vector3.up * MouseX * LookSensitivity * Time.unscaledDeltaTime, Space.Self);

            if (MouseY > 0)
                transform.Rotate(Vector3.left * MouseY * LookSensitivity * (SettingsMenu.invertY ? -1 : 1) * Time.unscaledDeltaTime, Space.Self);
            if (MouseY < 0)
                transform.Rotate(Vector3.right * -MouseY * LookSensitivity * (SettingsMenu.invertY ? -1 : 1) * Time.unscaledDeltaTime, Space.Self);
 
            if(Input.GetAxisRaw("Vertical") > 0)
                VerticalMovement = Input.GetAxisRaw("Vertical");
           
            float Axisz = transform.rotation.eulerAngles.z;
            // 0-45------------------------------------------------------------------------------------
            if (Axisz > 4f && Axisz < 41f)
                transform.Rotate(Vector3.back * SnappingSpeed * Time.unscaledDeltaTime, Space.Self);
            // 45-90-----------------------------------------------------------------------------------
            if (Axisz > 49f && Axisz < 86f)
                transform.Rotate(Vector3.forward * SnappingSpeed * Time.unscaledDeltaTime, Space.Self);
            // 90-135----------------------------------------------------------------------------------
            if (Axisz > 94f && Axisz < 131f)
                transform.Rotate(Vector3.back * SnappingSpeed * Time.unscaledDeltaTime, Space.Self);
            // 135 - 180-------------------------------------------------------------------------------
            if (Axisz > 139f && Axisz < 176f)
                transform.Rotate(Vector3.forward * SnappingSpeed * Time.unscaledDeltaTime, Space.Self);
            // 180-225---------------------------------------------------------------------------------
            if (Axisz > 184f && Axisz < 221f)
                transform.Rotate(Vector3.back * SnappingSpeed * Time.unscaledDeltaTime, Space.Self);
            // 225-270---------------------------------------------------------------------------------
            if (Axisz > 229f && Axisz < 266f)
                transform.Rotate(Vector3.forward * SnappingSpeed * Time.unscaledDeltaTime, Space.Self);
            // 270-315---------------------------------------------------------------------------------
            if (Axisz > 274f && Axisz < 311f)
                transform.Rotate(Vector3.back * SnappingSpeed * Time.unscaledDeltaTime, Space.Self);
            // 315-360---------------------------------------------------------------------------------
            if (Axisz > 319f && Axisz < 356f)
                transform.Rotate(Vector3.forward * SnappingSpeed * Time.unscaledDeltaTime, Space.Self);

            rigidbodyMoveDirection = (VerticalMovement * transform.forward);
            if (rb.velocity.sqrMagnitude > maxVelocity)
                rb.velocity *= 0.99f;

            rb.AddForce(rigidbodyMoveDirection * swimSpeed * Time.unscaledDeltaTime);
        }

        //========================PlayerMovementIce========================================//

        else if (!SceneSystem.isDisabled && !PauseGame.isPaused && !ShopSystem.isShop && !DialogueSystem.isDialogueActive && !PlayerSystem.isDead && !SelectionalSystem.isSelecting && !UnderWaterSystem.isSwimming && isIcy)
        {
            CharacterController charCtrl = GetComponent<CharacterController>();
            charCtrl.enabled = false;
            CapsuleCollider capCol = GetComponent<CapsuleCollider>();

            capCol.enabled = true;
            capCol.center = new Vector3(0, 0, -1);
            capCol.height = 7f;

            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.useGravity = true;
            float HorizontalMovement = Input.GetAxisRaw("Horizontal");
            float VerticalMovement = Input.GetAxisRaw("Vertical");

            rigidbodyStrafeDirection = (HorizontalMovement * transform.right);
            rigidbodyMoveDirection = (VerticalMovement * transform.forward);

            float DisstanceToTheGround = capCol.bounds.extents.y;

            bool IsGrounded = Physics.Raycast(transform.position, Vector3.down, DisstanceToTheGround + 0.1f);

            if (Input.GetButtonDown("Jump") && IsGrounded)
            {
                isJumping = true;
                SelectAnimation(PlayerAnimation.Jump, true);
                rb.AddForce(new Vector3(0, rbJumpSpeed, 0), ForceMode.Impulse);
            }
            if (IsGrounded)
            {
                isJumping = false;
                if (isFalling)
                    SwitchMovement(MovementType.notFall);
            }
            else
            {
                if (!isFalling && !SceneSystem.isDisabled && !isClimbing)
                    SwitchMovement(MovementType.fall);
            }
            if (isMoving)
            {
               
                if (Input.GetButton("Sprint") && !isMovingBack)
                {
                    SelectAnimation(PlayerAnimation.Run, true);
                    SelectAnimation(PlayerAnimation.Walk, false);
                    rbPlayerSpeed = rbRunSpeed;
                }
                else
                {
                    SelectAnimation(PlayerAnimation.Run, false);
                    SelectAnimation(PlayerAnimation.Walk, true);
                    rbPlayerSpeed = rbWalkSpeed;
                }
                

                rb.AddForce(rigidbodyStrafeDirection * rbPlayerSpeed * Time.unscaledDeltaTime);
                rb.AddForce(rigidbodyMoveDirection * rbPlayerSpeed * Time.unscaledDeltaTime);

                if (rb.velocity.magnitude > maxVelocity)
                    rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
            }
            else
            {
                Vector3 currentVelocity = rb.velocity;
                Vector3 oppositeVelocity = -currentVelocity;
                if (rb.velocity != Vector3.zero)
                {
                    rb.AddRelativeForce(oppositeVelocity.x * decelerationSpeed * Time.unscaledDeltaTime, oppositeVelocity.y * decelerationSpeed * Time.unscaledDeltaTime, oppositeVelocity.z * decelerationSpeed * Time.unscaledDeltaTime);
                }
            }


        }
    }
    public void setClimbing(bool climb)
    {
        if(climb)
            SwitchMovement(MovementType.climbStart);
        else
            SwitchMovement(MovementType.climbStop);
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        isJumping = false;
        currentGrabDistance = transform.position.y;
        if (!fallDamage)
        {
            if (isJumping)
            {
                audioSrc.clip = playerSounds.land;
                audioSrc.volume = 1f;
                audioSrc.pitch = UnityEngine.Random.Range(0.2f, 1.1f);
                audioSrc.Play();
            }
            else if (!isGrounded)
            {
                audioSrc.clip = playerSounds.land;
                audioSrc.volume = 1f;
                audioSrc.pitch = UnityEngine.Random.Range(0.2f, 1.1f);
                audioSrc.Play();
            }
        }
        if (hit.gameObject.CompareTag("EnemyTop") && !PlayerSystem.isInvincible)
        {

            SetFalling();
            playST = GetComponent<PlayerSystem>();
            ImpactReceiver impact = GetComponent<ImpactReceiver>();
            Vector3 direction = (transform.position - hit.transform.position).normalized;
            int lifeBerry = ItemSystem.lifeBerryAmt;
            if (lifeBerry == 0)
            {
                if (PlayerSystem.playerHealth <= 1)
                    impact.AddImpact(direction, 0);
                else if (PlayerSystem.playerHealth > 1)
                    impact.AddImpact(direction, 200);
            }
            else if (lifeBerry > 0)
                impact.AddImpact(direction, 200);

        }
        if (hit.gameObject.CompareTag("Enemy") && !SceneSystem.isDisabled && !PlayerSystem.isDead && !PlayerSystem.isInvincible)
        {
            playST = GetComponent<PlayerSystem>();
            if (hit.gameObject.GetComponent<EnemySystem>() != null)
            {
                if (hit.gameObject.GetComponent<EnemySystem>().isReal && !hit.gameObject.GetComponent<EnemySystem>().mimicActive)
                {
                    if (hit.gameObject.GetComponent<EnemySystem>().greyBubActive)
                        playST.PlayerDamage(1, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().blueBubActive)
                        playST.PlayerDamage(2, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().pinkBubActive)
                        playST.PlayerDamage(3, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().redBubActive)
                        playST.PlayerDamage(4, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().yellowBubActive)
                        playST.PlayerDamage(5, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().greenBubActive)
                        playST.PlayerDamage(6, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().purpleBubActive)
                        playST.PlayerDamage(7, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().greenMushActive)
                        playST.PlayerDamage(2, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().redMushActive)
                        playST.PlayerDamage(4, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().greyMushActive)
                        playST.PlayerDamage(6, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().blueStatueActive)
                        playST.PlayerDamage(4, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().silverStatueActive)
                        playST.PlayerDamage(8, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().goldStatueActive)
                        playST.PlayerDamage(12, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().whiteSkeletonActive)
                        playST.PlayerDamage(3, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().greenSkeletonActive)
                        playST.PlayerDamage(6, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().redSkeletonActive)
                        playST.PlayerDamage(9, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().darkSkeletonActive)
                        playST.PlayerDamage(12, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().goliathToadActive)
                        playST.PlayerDamage(5, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().chameleonToadActive)
                        playST.PlayerDamage(15, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().devilToadActive)
                        playST.PlayerDamage(30, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().graveYardActive)
                        playST.PlayerDamage(3, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().cemetaryActive)
                        playST.PlayerDamage(18, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().burialGroundsActive)
                        playST.PlayerDamage(33, false);



                    if (!hit.gameObject.GetComponent<EnemySystem>().isMimic)
                    {
                        ImpactReceiver impact = GetComponent<ImpactReceiver>();
                        Vector3 direction = (transform.position - hit.transform.position).normalized;
                        int lifeBerry = ItemSystem.lifeBerryAmt;
                        if (lifeBerry == 0)
                        {
                            if (PlayerSystem.playerHealth <= 1)
                                impact.AddImpact(direction, 0);
                            else if (PlayerSystem.playerHealth > 1)
                                impact.AddImpact(direction, 200);
                        }
                        else if (lifeBerry > 0)
                            impact.AddImpact(direction, 200);
                        SetFalling();
                    }
                    //if (DialogueSystem.isDialogueActive)
                    //{
                    //    InteractionSystem interaction = GetComponent<InteractionSystem>();
                    //    interaction.DialogueInteraction(false, null);
                    //}
                }
                else if (hit.gameObject.GetComponent<EnemySystem>().isReal && hit.gameObject.GetComponent<EnemySystem>().mimicActive)
                {
                    if (hit.gameObject.GetComponent<EnemySystem>().blueMimicActive)
                        playST.PlayerDamage(6, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().greenMimicActive)
                        playST.PlayerDamage(12, false);
                    else if (hit.gameObject.GetComponent<EnemySystem>().orangeMimicActive)
                        playST.PlayerDamage(18, false);
                    ImpactReceiver impact = GetComponent<ImpactReceiver>();
                    Vector3 direction = (transform.position - hit.transform.position).normalized;
                    int lifeBerry = ItemSystem.lifeBerryAmt;
                    if (lifeBerry == 0)
                    {
                        if (PlayerSystem.playerHealth <= 1)
                            impact.AddImpact(direction, 0);
                        else if (PlayerSystem.playerHealth > 1)
                            impact.AddImpact(direction, 200);
                    }
                    else if (lifeBerry > 0)
                        impact.AddImpact(direction, 200);
                    SetFalling();
                    //if (DialogueSystem.isDialogueActive)
                    //{
                    //    InteractionSystem interaction = GetComponent<InteractionSystem>();
                    //    interaction.DialogueInteraction(false, null);
                    //}
                }
            }
            else if (hit.gameObject.GetComponent<GraveYardGhostSystem>() != null)
            {
                playST.PlayerDamage(1, false);
                ImpactReceiver impact = GetComponent<ImpactReceiver>();
                Vector3 direction = (transform.position - hit.transform.position).normalized;
                int lifeBerry = ItemSystem.lifeBerryAmt;
                if (lifeBerry == 0)
                {
                    if (PlayerSystem.playerHealth <= 1)
                        impact.AddImpact(direction, 0);
                    else if (PlayerSystem.playerHealth > 1)
                        impact.AddImpact(direction, 50);
                }
                else if (lifeBerry > 0)
                    impact.AddImpact(direction, 50);
                SetFalling();
                GraveYardGhostSystem ghostSystem = hit.gameObject.GetComponent<GraveYardGhostSystem>();
                ghostSystem.SetHitSound(1);
                ghostSystem.DamageEnemy(1);
                //if (DialogueSystem.isDialogueActive)
                //{
                //    InteractionSystem interaction = GetComponent<InteractionSystem>();
                //    interaction.DialogueInteraction(false, null);
                //}
            }
        }
        if (hit.gameObject.CompareTag("Ball") && !SceneSystem.isDisabled && !PlayerSystem.isDead && !PlayerSystem.isInvincible)
        {
            playST = GetComponent<PlayerSystem>();
            
            if(hit.gameObject.GetComponent<BallSystem>().lvl1)
                playST.PlayerDamage(1, false);
            else if (hit.gameObject.GetComponent<BallSystem>().lvl2)
                playST.PlayerDamage(2, false);
            else if(hit.gameObject.GetComponent<BallSystem>().lvl3)
                playST.PlayerDamage(3, false);
            else if(hit.gameObject.GetComponent<BallSystem>().lvl4)
                playST.PlayerDamage(4, false);
            else if(hit.gameObject.GetComponent<BallSystem>().lvl5)
                playST.PlayerDamage(5, false);

            ImpactReceiver impact = GetComponent<ImpactReceiver>();
            Vector3 direction = (transform.position - hit.transform.position).normalized;
            int lifeBerry = ItemSystem.lifeBerryAmt;
            if (lifeBerry == 0)
            {
                if (PlayerSystem.playerHealth <= 1)
                    impact.AddImpact(direction, 0);
                else if (PlayerSystem.playerHealth > 1)
                    impact.AddImpact(direction, 200);
            }
            else if (lifeBerry > 0)
                impact.AddImpact(direction, 200);
            SetFalling();
        }
        if (hit.gameObject.CompareTag("SpikeBall") && !SceneSystem.isDisabled && !PlayerSystem.isDead && !PlayerSystem.isInvincible)
        {
            playST = GetComponent<PlayerSystem>();

            if (hit.gameObject.GetComponent<SpikeCollision>().lvl1)
                playST.PlayerDamage(1, false);
            else if (hit.gameObject.GetComponent<SpikeCollision>().lvl2)
                playST.PlayerDamage(2, false);
            else if (hit.gameObject.GetComponent<SpikeCollision>().lvl3)
                playST.PlayerDamage(3, false);
            else if (hit.gameObject.GetComponent<SpikeCollision>().lvl4)
                playST.PlayerDamage(4, false);
            else if (hit.gameObject.GetComponent<SpikeCollision>().lvl5)
                playST.PlayerDamage(5, false);

            ImpactReceiver impact = GetComponent<ImpactReceiver>();
            Vector3 direction = (transform.position - hit.transform.position).normalized;
            int lifeBerry = ItemSystem.lifeBerryAmt;
            if (lifeBerry == 0)
            {
                if (PlayerSystem.playerHealth <= 1)
                    impact.AddImpact(direction, 0);
                else if (PlayerSystem.playerHealth > 1)
                    impact.AddImpact(direction, 200);
            }
            else if (lifeBerry > 0)
                impact.AddImpact(direction, 200);
            SetFalling();
        }
        if (hit.gameObject.CompareTag("Shockwave") && !SceneSystem.isDisabled && !PlayerSystem.isDead && !PlayerSystem.isInvincible)
        {
            playST = GetComponent<PlayerSystem>();

            if (hit.gameObject.GetComponent<Shockwave>().lvl1)
                playST.PlayerDamage(1, false);
            else if (hit.gameObject.GetComponent<Shockwave>().lvl2)
                playST.PlayerDamage(2, false);
            else if (hit.gameObject.GetComponent<Shockwave>().lvl3)
                playST.PlayerDamage(3, false);
            else if (hit.gameObject.GetComponent<Shockwave>().lvl4)
                playST.PlayerDamage(4, false);
            else if (hit.gameObject.GetComponent<Shockwave>().lvl5)
                playST.PlayerDamage(5, false);

            ImpactReceiver impact = GetComponent<ImpactReceiver>();
            Vector3 direction = (transform.position - hit.transform.position).normalized;
            int lifeBerry = ItemSystem.lifeBerryAmt;
            if (lifeBerry == 0)
            {
                if (PlayerSystem.playerHealth <= 1)
                    impact.AddImpact(direction, 0);
                else if (PlayerSystem.playerHealth > 1)
                    impact.AddImpact(direction, 200);
            }
            else if (lifeBerry > 0)
                impact.AddImpact(direction, 200);
            SetFalling();
            Destroy(hit.gameObject);
        }
        if (hit.gameObject.CompareTag("SpikeBall") && !SceneSystem.isDisabled && !PlayerSystem.isDead && !PlayerSystem.isInvincible)
        {
            playST = GetComponent<PlayerSystem>();

            ImpactReceiver impact = GetComponent<ImpactReceiver>();
            Vector3 direction = (transform.position - hit.transform.position).normalized;
            int lifeBerry = ItemSystem.lifeBerryAmt;
            if (lifeBerry == 0)
            {
                if (PlayerSystem.playerHealth <= 1)
                    impact.AddImpact(direction, 0);
                else if (PlayerSystem.playerHealth > 1)
                    impact.AddImpact(direction, 200);
            }
            else if (lifeBerry > 0)
                impact.AddImpact(direction, 200);

            if (hit.gameObject.GetComponent<SpikeCollision>().lvl1)
                playST.PlayerDamage(2, false);
            else if (hit.gameObject.GetComponent<SpikeCollision>().lvl2)
                playST.PlayerDamage(4, false);
            else if (hit.gameObject.GetComponent<SpikeCollision>().lvl3)
                playST.PlayerDamage(6, false);
            else if (hit.gameObject.GetComponent<SpikeCollision>().lvl4)
                playST.PlayerDamage(8, false);
            else if (hit.gameObject.GetComponent<SpikeCollision>().lvl5)
                playST.PlayerDamage(10, false);

            SetFalling();
        }
        if (hit.gameObject.CompareTag("Wood"))
            SetFootStepActive(FootElement.wood);
        else if (hit.gameObject.CompareTag("Ice"))
            SetFootStepActive(FootElement.ice);
        else if (hit.gameObject.CompareTag("Metal"))
            SetFootStepActive(FootElement.metal);
        else
        {
            wood = false;
            metal = false;
            ice = false;
        }
        contactPoint = hit.point;

        if (IsCollidingWithClimbable(hit) && Vector3.Angle(-hit.normal, fpsControllerTransform.forward) <= hitAngle && !IsClimbingObstructed())
            cineCamera.TriggerClimbUpAnimation();
        else
            IsClimbingObstructed();


    }
    private bool IsCollidingWithClimbable(ControllerColliderHit hit)
    {
        return !characterController.isGrounded &&
            characterController.collisionFlags == CollisionFlags.CollidedSides &&
            hit.gameObject.CompareTag("Climbable");
    }

    private bool IsClimbingObstructed()
    {
        Collider[] results = new Collider[10];

        return Physics.OverlapBoxNonAlloc(new Vector3(fpsControllerTransform.position.x,
            fpsControllerTransform.position.y + overlapBoxYTranslation, fpsControllerTransform.position.z) +
            fpsControllerTransform.forward * overlapBoxForwardTranslation,
            Vector3.one, results, fpsControllerTransform.rotation, layerMask) != 0;
    }
    public void PlayerPickUpCarryObj(int num)
    {
        if (num == 1)
        {
            if (carryObj != null)
            {
                carryingObj = true;
                BoxCollider objCollide = carryObj.GetComponent<BoxCollider>();
                objCollide.enabled = false;
                carryObj.transform.SetParent(carryObjNewPos);
                carryObj.transform.localPosition = Vector3.zero;
                carryObj.transform.rotation = carryObjNewPos.rotation;
                boxRb = carryObj.GetComponent<Rigidbody>();
                boxRb.useGravity = false;
                boxRb.constraints = RigidbodyConstraints.FreezeAll;

                inRange = false;
            }
        }
        else if(num == 2)
        {
            if (carryObjNewPos.childCount > 0)
            {
                foreach (Transform child in carryObjNewPos)
                {
                    BlockSystem block = child.gameObject.GetComponentInChildren<BlockSystem>();
                    block.ReturnBlock(BlockSystem.ReturnType.Parent);
                }
            }
            carryingObj = false;
            foreach (Transform child in carryObj.transform)
                child.GetComponent<BoxCollider>().enabled = true;
            carryObj.transform.rotation = new Quaternion(0, carryObj.transform.rotation.y, 0, carryObj.transform.rotation.w);
            boxRb = carryObj.GetComponent<Rigidbody>();
            boxRb.useGravity = true;
            boxRb.constraints = RigidbodyConstraints.FreezeRotation;
            float playerHeightForce = heightForce * Throwforce;
            boxRb.AddForceAtPosition(transform.forward * playerHeightForce, transform.position, ForceMode.Impulse);
            BoxCollider objCollide = carryObj.GetComponent<BoxCollider>();
            objCollide.enabled = true;
            carryObj = null;
            isCarrying = false;
        }
        else
        {
            if (carryObjNewPos.childCount > 0)
            {
                foreach (Transform child in carryObjNewPos)
                {
                    BlockSystem block = child.gameObject.GetComponentInChildren<BlockSystem>();
                    block.ReturnBlock(BlockSystem.ReturnType.Parent);
                }
            }
            carryingObj = false;
            foreach (Transform child in carryObj.transform)
                child.GetComponent<BoxCollider>().enabled = true;
            boxRb = carryObj.GetComponent<Rigidbody>();
            boxRb.useGravity = true;

            carryObj.transform.rotation = new Quaternion(0, carryObj.transform.rotation.y, 0, carryObj.transform.rotation.w);
            boxRb.constraints = RigidbodyConstraints.FreezeAll; 
            BoxCollider objCollide = carryObj.GetComponent<BoxCollider>();
            objCollide.enabled = true;
            carryObj = null;
            isCarrying = false;
        }
    }
  
    void OnTriggerEnter(Collider other)
    {
      
        if (other.gameObject.CompareTag("Grass"))
            SetFootStepActive(FootElement.grass);
        else if (other.gameObject.CompareTag("Water") && !isSwimming)
            SetFootStepActive(FootElement.water);
        else if (other.gameObject.CompareTag("Lava"))
            SetFootStepActive(FootElement.lava);
        if (other.gameObject.CompareTag("Story1"))
            PlayerInteraction(true);
        if (other.gameObject.CompareTag("Platform"))
            transform.SetParent(other.transform);
        if (other.gameObject.CompareTag("Interactive"))
            PlayerInteraction(true);
        if (other.gameObject.CompareTag("Door"))
            PlayerInteraction(true);
        if (other.gameObject.CompareTag("Ladder") && !isIcy && !isCarrying)
            SwitchMovement(MovementType.climbStart);
        if (other.gameObject.CompareTag("Carry") && !isCarrying && !isClimbing)
        {
            inRange = true;
            carryObj = other.gameObject;
            originalParent = other.gameObject.transform.parent;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Grass"))
            SetFootStepActive(FootElement.normal);
        else if (other.gameObject.CompareTag("Water"))
            SetFootStepActive(FootElement.normal);
        else if (other.gameObject.CompareTag("Lava"))
            SetFootStepActive(FootElement.normal);

        if (other.gameObject.CompareTag("Ladder") )
            SwitchMovement(MovementType.climbStop);
       
        if (other.gameObject.CompareTag("Platform"))
        {
            GameObject gameCtrl = GameObject.Find("/Core/Player/");
            transform.SetParent(gameCtrl.transform);
        }
        if (other.gameObject.CompareTag("Interactive"))
            PlayerInteraction(false);
        if (other.gameObject.CompareTag("Story1"))
            PlayerInteraction(false);
        if (other.gameObject.CompareTag("Door"))
            PlayerInteraction(false);
        if (other.gameObject.CompareTag("Carry"))
            inRange = false;
    }
    public enum PlayerAnimation { FastSwipe, FastSwipe2, TwoHandSwipe, TwoHandSwipe2, Swim, SwimIdle, Jump, Rebind, Fall, Run, Walk, Armed, Unarmed, Climb, Carry, Drop, Throw, PutAwayJewel, TakeOutJewel, JewelActivated}
    public void SelectAnimation(PlayerAnimation animation, bool True)
    {
        anim = GetComponent<Animator>();
        if (True)
        {
            switch (animation)
            {
                case PlayerAnimation.Unarmed:
                    {
                        anim.SetTrigger("Unarmed");
                        break;
                    }
                case PlayerAnimation.Armed:
                    {
                        anim.SetTrigger("Armed");
                        break;
                    }
                case PlayerAnimation.JewelActivated:
                    {
                        anim.SetTrigger("JewelActivated");
                        break;
                    }
                case PlayerAnimation.PutAwayJewel:
                    {
                        anim.SetTrigger("PutWayJewel");
                        break;
                    }
                case PlayerAnimation.TakeOutJewel:
                    {
                        anim.SetTrigger("TakeOutJewel");
                        break;
                    }
                case PlayerAnimation.FastSwipe:
                    {
                        anim.SetTrigger("FastSwipe");
                        break;
                    }
              
                case PlayerAnimation.Swim:
                    {
                        anim.SetBool("Swim", true);
                        break;
                    }
                case PlayerAnimation.SwimIdle:
                    {
                        anim.SetBool("SwimIdle", true);
                        break;
                    }
                case PlayerAnimation.Jump:
                    {
                        anim.SetBool("Jump", true);
                        break;
                    }
                case PlayerAnimation.Fall:
                    {
                        anim.SetBool("Fall", true);
                        break;
                    }
                case PlayerAnimation.Walk:
                    {
                        if(!isParalyzed)
                            anim.SetBool("Walk", true);

                        break;
                    }
                case PlayerAnimation.Run:
                    {
                        if (!isParalyzed)
                            anim.SetBool("Run", true);
                        break;
                    }
                
                case PlayerAnimation.Climb:
                    {
                        if (!isParalyzed)
                            anim.SetBool("Climb", true);
                        break;
                    }
                case PlayerAnimation.Carry:
                    {
                        if (!isParalyzed && carryObj != null)
                            anim.SetBool("Carry", true);
                        break;
                    }
                case PlayerAnimation.Throw:
                    {
                        if(carryObj != null)
                            anim.SetTrigger("Throw");
                        break;
                    }
                case PlayerAnimation.Drop:
                    {
                        anim.SetTrigger("Drop");
                        break;
                    }
                case PlayerAnimation.Rebind:
                    {
                        anim.Rebind();
                        break;
                    }
            }
        }
        else
        {
            switch (animation)
            {
               
                
                case PlayerAnimation.Swim:
                    {
                        anim.SetBool("Swim", false);
                        break;
                    }
                case PlayerAnimation.SwimIdle:
                    {
                        anim.SetBool("SwimIdle", false);
                        break;
                    }
                case PlayerAnimation.Jump:
                    {
                        anim.SetBool("Jump", false);
                        break;
                    }
                case PlayerAnimation.Fall:
                    {
                        anim.SetBool("Fall", false);
                        break;
                    }
                case PlayerAnimation.Walk:
                    {
                        anim.SetBool("Walk", false);
                        break;
                    }
                case PlayerAnimation.Run:
                    {
                        anim.SetBool("Run", false);
                        break;
                    }
               
                case PlayerAnimation.Climb:
                    {
                        anim.SetBool("Climb", false);
                        break;
                    }
                case PlayerAnimation.Carry:
                    {
                        anim.SetBool("Carry", false);
                        break;
                    }
            }
        }

    }
    void FallingDamageAlert(float fallDistance)
    {
        fallDamage = true;
        if (fallDistance > 55)
        {
            audioSrc2.volume = 1f;
            audioSrc2.pitch = UnityEngine.Random.Range(0.6f, 0.8f);
            audioSrc2.clip = playerSounds.hardLanding;
            audioSrc2.Play();
        }
        if (fallDistance > 35 && fallDistance <= 45)
        {
            audioSrc2.volume = 1f;
            audioSrc2.pitch = UnityEngine.Random.Range(0.6f, 0.8f);
            audioSrc2.clip = playerSounds.hardLanding;
            audioSrc2.Play();
        }
        if (fallDistance > 25 && fallDistance <= 35)
        {
            audioSrc2.volume = 1f;
            audioSrc2.pitch = UnityEngine.Random.Range(0.6f, 0.8f);
            audioSrc2.clip = playerSounds.hardLanding;
            audioSrc2.Play();
        }
    }
    public enum MovementType { forward, back, left, right, none, jump, fall, notFall, climbUp, climbDown, climbHold, climbStart, climbStop }
    public void SwitchMovement(MovementType type)
    {
        switch (type)
        {
            case MovementType.forward:
                {
                    if (!UnderWaterSystem.isSwimming)
                    {
                        if (isRunning)
                        {
                            SelectAnimation(PlayerAnimation.Run, true);
                            SelectAnimation(PlayerAnimation.Walk, false);
                        }

                        else if (!isRunning)
                        {
                            SelectAnimation(PlayerAnimation.Run, false);
                            SelectAnimation(PlayerAnimation.Walk, true);
                        }
                        SelectAnimation(PlayerAnimation.Swim, false);
                        isActive = true;
                    }
                    else
                    {
                        SelectAnimation(PlayerAnimation.Swim, true);
                        SelectAnimation(PlayerAnimation.Fall, false);
                        SelectAnimation(PlayerAnimation.Run, false);
                        SelectAnimation(PlayerAnimation.Walk, false);
                        SelectAnimation(PlayerAnimation.SwimIdle, false);
                    }
                    anim.SetBool("Idle", false);
                    isMoving = true;
                    isIdle = false;
                    isMovingLeft = false;
                    isMovingRight = false;
                    isMovingForward = true;
                    isMovingBack = false;
                    break;
                }
            case MovementType.back:
                {
                    if (!UnderWaterSystem.isSwimming)
                    {
                        if (isRunning)
                        {
                            SelectAnimation(PlayerAnimation.Run, true);
                            SelectAnimation(PlayerAnimation.Walk, false);
                        }

                        else if (!isRunning)
                        {
                            SelectAnimation(PlayerAnimation.Run, false);
                            SelectAnimation(PlayerAnimation.Walk, true);
                        }
                        SelectAnimation(PlayerAnimation.Swim, false);
                        isActive = true;
                    }
                    else
                    {
                        SelectAnimation(PlayerAnimation.Swim, false);
                        SelectAnimation(PlayerAnimation.Fall, false);
                        SelectAnimation(PlayerAnimation.Run, false);
                        SelectAnimation(PlayerAnimation.Walk, false);
                        SelectAnimation(PlayerAnimation.SwimIdle, true);
                    }
                    anim.SetBool("Idle", false);
                    isMoving = true;
                    isIdle = false;
                    isMovingLeft = false;
                    isMovingRight = false;
                    isMovingForward = false;
                    isMovingBack = true;
                    break;
                }
            case MovementType.left:
                {
                    if (!UnderWaterSystem.isSwimming)
                    {
                        if (isRunning)
                        {
                            SelectAnimation(PlayerAnimation.Run, true);
                            SelectAnimation(PlayerAnimation.Walk, false);
                        }

                        else if (!isRunning)
                        {
                            SelectAnimation(PlayerAnimation.Run, false);
                            SelectAnimation(PlayerAnimation.Walk, true);
                        }
                        SelectAnimation(PlayerAnimation.Swim, false);
                        isActive = true;
                    }
                    else
                    {
                        SelectAnimation(PlayerAnimation.Swim, true);
                        SelectAnimation(PlayerAnimation.Fall, false);
                        SelectAnimation(PlayerAnimation.Run, false);
                        SelectAnimation(PlayerAnimation.Walk, false);
                        SelectAnimation(PlayerAnimation.SwimIdle, false);
                    }
                    anim.SetBool("Idle", false);
                    isMoving = true;
                    isIdle = false;
                    isMovingLeft = true;
                    isMovingRight = false;
                    isMovingForward = false;
                    isMovingBack = false;
                    break;
                }
            case MovementType.right:
                {
                    if (!UnderWaterSystem.isSwimming)
                    {
                        if (isRunning)
                        {
                            SelectAnimation(PlayerAnimation.Run, true);
                            SelectAnimation(PlayerAnimation.Walk, false);
                        }

                        else if (!isRunning)
                        {
                            SelectAnimation(PlayerAnimation.Run, false);
                            SelectAnimation(PlayerAnimation.Walk, true);
                        }
                        SelectAnimation(PlayerAnimation.Swim, false);
                        isActive = true;
                    }
                    else
                    {
                        SelectAnimation(PlayerAnimation.Swim, true);
                        SelectAnimation(PlayerAnimation.Fall, false);
                        SelectAnimation(PlayerAnimation.Run, false);
                        SelectAnimation(PlayerAnimation.Walk, false);
                        SelectAnimation(PlayerAnimation.SwimIdle, false);
                     
                    }
                    anim.SetBool("Idle", false);
                    isMoving = true;
                    isIdle = false;
                    isMovingLeft = false;
                    isMovingRight = true;
                    isMovingForward = false;
                    isMovingBack = false;
                    break;
                }
            case MovementType.jump:
                {
                    if (!Input.GetButton("Jump"))
                    {

                        jumpTimer++;
                    }
                    else if (jumpTimer >= antiBunnyHopFactor && !SceneSystem.isDisabled &&!DialogueSystem.isDialogueActive && isInteraction)
                    {
                        PlayerInteraction(false);
                    }
                    else if (jumpTimer >= antiBunnyHopFactor && !isInteraction && !SceneSystem.isDisabled && !DialogueSystem.isDialogueActive )
                    {
                        
                        SelectAnimation(PlayerAnimation.Jump, true);
                        anim.SetBool("Idle", false);
                        isActive = true;
                        isJumping = true;
                        audioSrc2.clip = playerSounds.Jump;
                        audioSrc2.volume = UnityEngine.Random.Range(0.5f, 1f);
                        audioSrc2.pitch = UnityEngine.Random.Range(0.5f, 1.5f);
                        audioSrc2.Play();
                        if (JewelSystem.isJewelActive && JewelSystem.blueJewelEnabled)
                            moveDirection.y = jumpSpeed * 2;
                        else
                            moveDirection.y = jumpSpeed;
                        jumpTimer = 0;
                    }

                    break;
                }
            case MovementType.fall:
                {
                    SelectAnimation(PlayerAnimation.Fall, true);
                    if(!isJumping)
                        SelectAnimation(PlayerAnimation.Jump, false);
  
                    anim.SetBool("Idle", false);
                    gravity = gravityPull;
                    
                    isActive = true;
                    isFalling = true;
                    fallStartLevel = myTransform.position.y;
                    break;
                }
            case MovementType.notFall:
                {
                    SelectAnimation(PlayerAnimation.Jump, false);
                    SelectAnimation(PlayerAnimation.Fall, false);
                    isFalling = false;
                    if (myTransform.position.y < fallStartLevel - fallingDamageThreshold)
                        FallingDamageAlert(fallStartLevel - myTransform.position.y);
                    break;
                }
            case MovementType.climbUp:
                {
                    isActive = true;
                    SelectAnimation(PlayerAnimation.Climb, true);
                    anim.SetFloat("ClimbSpeed", 1f);
                    moveDirection.y = 4;
                    moveDirection.x = 0;
                    if (moveDirection.y > 4f)
                        moveDirection.y = 4f;
                    break;
                }
            case MovementType.climbDown:
                {
                    isActive = true;
                    SelectAnimation(PlayerAnimation.Climb, true);
                    anim.SetFloat("ClimbSpeed", 1);
                   
                    moveDirection -= Vector3.up / climbHeightFactor;
                    if (moveDirection.y < -3f)
                        moveDirection.y = -3f;
                    break;
                }
            case MovementType.climbHold:
                {
                    anim.SetFloat("ClimbSpeed", 0);
                    moveDirection = Vector3.zero;
                    moveDirection.x = 0;
                    break;
                }
            case MovementType.climbStart:
                {
                    isClimbing = true;
                    GameObject shutOffModel = GameObject.Find("Core/Player/PlayerController/Head/PlayerCamera/Recoil/Weapon/Bobbing/Tilt");
                    shutOffModel.SetActive(false);
                    isJumping = false;
                    if (UnderWaterSystem.isSwimming)
                    {
                        UnderWaterSystem underWaterSystem = GameObject.FindGameObjectWithTag("Head").GetComponent<UnderWaterSystem>();
                        underWaterSystem.DebugSwimming(false);
                    }
                    SelectAnimation(PlayerAnimation.Climb, true);
                    SelectAnimation(PlayerAnimation.Jump, false);
                    break;
                }
            case MovementType.climbStop:
                {
                    anim.SetFloat("ClimbSpeed", 1);
                    isClimbing = false;
                    GameObject shutOffModel = GameObject.Find("Core/Player/PlayerController/Head/PlayerCamera/Recoil/Weapon/Bobbing/Tilt");
                    shutOffModel.SetActive(true);
                    SelectAnimation(PlayerAnimation.Climb, false);
                    break;
                }
            case MovementType.none:
                {
                    if (!UnderWaterSystem.isSwimming)
                    {
                        SelectAnimation(PlayerAnimation.Run, false);
                        SelectAnimation(PlayerAnimation.Walk, false);
                        anim.SetBool("Idle", true);
                        SelectAnimation(PlayerAnimation.Swim, false);
                        if (isGrounded)
                        {
                            SelectAnimation(PlayerAnimation.Jump, false);
                            isActive = false;
                        }
                    }
                    else
                    {
                        rb.velocity = Vector3.zero;
                        SelectAnimation(PlayerAnimation.SwimIdle, true);
                        SelectAnimation(PlayerAnimation.Fall, false);
                        SelectAnimation(PlayerAnimation.Run, false);
                        SelectAnimation(PlayerAnimation.Walk, false);
                        SelectAnimation(PlayerAnimation.Swim, false);
                    }
                    isIdle = true;
                    isMoving = false;
                    isMovingLeft = false;
                    isMovingRight = false;
                    isMovingForward = false;
                    isMovingBack = false;
                    break;
                }
        }
    }
    public void EstimatePlayerVelocity_InvokeRepeating()
    {
        float distanceMagnitude = (transform.position - lastPlayerPosition).magnitude;
        lastPlayerPosition = transform.position;
        float estimatedPlayerVelocity = distanceMagnitude / velocityPollPeriod;
        if (estimatedPlayerVelocity < 15.0f)
        {
            currentFootstepsWaitingPeriod = Mathf.Infinity;
            return;
        }
        float mappedPlayerSpeed = estimatedPlayerVelocity / 5.0f; //Convert the speed so that walking speed is about 6
        stepsPerSecond = ((stepSoundRatioA * Mathf.Pow(mappedPlayerSpeed, stepSoundRatioN)) + (stepSoundRatioB * mappedPlayerSpeed) + stepSoundRatioC) / 60.0f;
        timePerStep = (1.0f / stepsPerSecond);
        currentFootstepsWaitingPeriod = timePerStep;
    }
    public enum FootElement { grass, wood, metal, ice, lava, water, normal}
    public void SetFootSound(FootElement element)
    {
        
        switch (element)
        {
            case FootElement.normal:
                {
                    audioSrc.clip = playerSounds.footStep;
                    break;
                }
            case FootElement.grass:
                {
                    int randomInt = UnityEngine.Random.Range(0, playerSounds.footStepGrass.Length);
                    if (randomInt == 0)
                        audioSrc.clip = playerSounds.footStepGrass[0];
                    if (randomInt == 1)
                        audioSrc.clip = playerSounds.footStepGrass[1];
                    if (randomInt == 2)
                        audioSrc.clip = playerSounds.footStepGrass[2];
                    if (randomInt == 3)
                        audioSrc.clip = playerSounds.footStepGrass[3];
                    break;
                }
            case FootElement.wood:
                {
                    int randomInt = UnityEngine.Random.Range(0, playerSounds.footStepWood.Length);
                    if (randomInt == 0)
                        audioSrc.clip = playerSounds.footStepWood[0];
                    if (randomInt == 1)
                        audioSrc.clip = playerSounds.footStepWood[1];
                    if (randomInt == 2)
                        audioSrc.clip = playerSounds.footStepWood[2];
                    break;
                }
            case FootElement.water:
                {
                    int randomInt = UnityEngine.Random.Range(0, playerSounds.footStepWater.Length);
                    if (randomInt == 0)
                        audioSrc.clip = playerSounds.footStepWater[0];
                    if (randomInt == 1)
                        audioSrc.clip = playerSounds.footStepWater[1];
                    if (randomInt == 2)
                        audioSrc.clip = playerSounds.footStepWater[2];
                    break;
                }
            case FootElement.metal:
                {
                    int randomInt = UnityEngine.Random.Range(0, playerSounds.footStepMetal.Length);
                    if (randomInt == 0)
                        audioSrc.clip = playerSounds.footStepMetal[0];
                    if (randomInt == 1)
                        audioSrc.clip = playerSounds.footStepMetal[1];
                    if (randomInt == 2)
                        audioSrc.clip = playerSounds.footStepMetal[2];
                    if (randomInt == 3)
                        audioSrc.clip = playerSounds.footStepMetal[3];
                    break;
                }
            case FootElement.ice:
                {
                    int randomInt = UnityEngine.Random.Range(0, playerSounds.footStepIce.Length);
                    if (randomInt == 0)
                        audioSrc.clip = playerSounds.footStepIce[0];
                    if (randomInt == 1)
                        audioSrc.clip = playerSounds.footStepIce[1];
                    if (randomInt == 2)
                        audioSrc.clip = playerSounds.footStepIce[2];
                    if (randomInt == 3)
                        audioSrc.clip = playerSounds.footStepIce[3];
                  
                    break;
                }
            case FootElement.lava:
                {
                    int randomInt = UnityEngine.Random.Range(0, playerSounds.footStepLava.Length);
                    if (randomInt == 0)
                        audioSrc.clip = playerSounds.footStepLava[0];
                    if (randomInt == 1)
                        audioSrc.clip = playerSounds.footStepLava[1];
                    if (randomInt == 2)
                        audioSrc.clip = playerSounds.footStepLava[2];
                    if (randomInt == 3)
                        audioSrc.clip = playerSounds.footStepLava[3];
                    break;
                }
        }
    }
    public void SetFootStepActive(FootElement element)
    {
        switch (element)
        {
            case FootElement.normal:
                {
                    grass = false;
                    wood = false;
                    water = false;
                    ice = false;
                    metal = false;
                    lava = false;
                    break;
                }
            case FootElement.grass:
                {
                    grass = true;
                    wood = false;
                    water = false;
                    ice = false;
                    metal = false;
                    lava = false;
                    break;
                }
            case FootElement.wood:
                {
                    grass = false;
                    wood = true;
                    water = false;
                    ice = false;
                    metal = false;
                    lava = false;
                    break;
                }
            case FootElement.water:
                {
                    grass = false;
                    wood = false;
                    water = true;
                    ice = false;
                    metal = false;
                    lava = false;
                    break;
                }
            case FootElement.metal:
                {
                    grass = false;
                    wood = false;
                    water = false;
                    ice = false;
                    metal = true;
                    lava = false;
                    break;
                }
            case FootElement.ice:
                {
                    grass = false;
                    wood = false;
                    water = false;
                    ice = true;
                    metal = false;
                    lava = false;
                    break;
                }
            case FootElement.lava:
                {
                    grass = false;
                    wood = false;
                    water = false;
                    ice = false;
                    metal = false;
                    lava = true;
                    break;
                }
        }
    }
    public void SetFootStep()
    {
        if (grass)
            SetFootSound(FootElement.grass);
        else if (wood)
            SetFootSound(FootElement.wood);
        else if (metal)
            SetFootSound(FootElement.metal);
        else if (water)
            SetFootSound(FootElement.water);
        else if (ice)
            SetFootSound(FootElement.ice);
        else if(lava)
            SetFootSound(FootElement.lava);
        else
            SetFootSound(FootElement.normal);

    }
    public void ConfusePlayer(bool confused)
    {
        isConfused = confused;
    }
    public void ParalyzedPlayer(bool paralyzed)
    { 
        isParalyzed = paralyzed;
    }
    public void SetFalling()
    {
        isActive = false;
        isGrounded = false;
    }
    public void SetPosition()
    {
        isActive = true;
        isGrounded = true;
    }
    public void PlayerInteraction(bool interact)
    {
        isInteraction = interact;
    }
}
[Serializable]
public class PlayerSounds
{
    public AudioClip Jump;
    public AudioClip footStep;
    public AudioClip land;
    public AudioClip[] footStepIce;
    public AudioClip[] footStepWood;
    public AudioClip[] footStepWater;
    public AudioClip[] footStepMetal;
    public AudioClip[] footStepGrass;
    public AudioClip[] footStepLava;
    public AudioClip hardLanding;
    public AudioClip lightLanding;

}
