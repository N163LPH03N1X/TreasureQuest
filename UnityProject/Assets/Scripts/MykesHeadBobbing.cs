using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MykesHeadBobbing : MonoBehaviour {

    private float timer = 0.0f;
    OptSystem optSystem = new OptSystem();
    public static float bobbingSpeed;
    public static float bobbingAmount;

    public float bobbingRunSpeed;
    public float bobbingRunAmount;

    public float bobbingWalkSpeed;
    public float bobbingWalkAmount;

    float midpoint = 0f;
    bool isGrounded = true;
    bool isRunning;
    bool isMoving;
    bool isDisabled;
    bool isPaused;
    bool isPetrified;
    bool isClimbing;

    void Update()
    {
        isMoving = CharacterSystem.isMoving;
        isGrounded = CharacterSystem.isGrounded;
        isRunning = CharacterSystem.isRunning;
        isDisabled = SceneSystem.isDisabled;
        isPaused = PauseGame.isPaused;
        isPetrified = CharacterSystem.isParalyzed;
        isClimbing = CharacterSystem.isClimbing;

        if (!isDisabled && !isPaused && !isPetrified && !isClimbing && !SelectionalSystem.isSelecting)
        {
            float waveslice = 1.0f;
            float horizontal = optSystem.Input.GetAxis("Horizontal");
            float vertical = optSystem.Input.GetAxis("Vertical");

            if (isRunning)
            {
                bobbingSpeed = bobbingRunSpeed;
                bobbingAmount = bobbingRunAmount;
            }
            else if (!isRunning)
            {
                bobbingSpeed = bobbingWalkSpeed;
                bobbingAmount = bobbingWalkAmount;
            }



            if (isMoving && isGrounded)
            {
                Vector3 cSharpConversion = transform.localPosition;
                if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
                {
                    timer = 0.0f;
                }
                else
                {
                    waveslice = Mathf.Sin(timer);
                    timer = timer + bobbingSpeed;
                    if (timer > Mathf.PI * 2)
                    {
                        timer = timer - (Mathf.PI * 2);
                    }
                }
                if (waveslice != 0)
                {
                    float translateChange = waveslice * bobbingAmount;
                    float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
                    totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
                    translateChange = totalAxes * translateChange;
                    cSharpConversion.y = midpoint + translateChange;
                }
                else
                {
                    cSharpConversion.y = midpoint;
                }

                transform.localPosition = cSharpConversion;
            }
        }
    }
}