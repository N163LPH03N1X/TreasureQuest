using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{

    [Header("X Settings")]
    public bool MovementX;
    public bool ReverseMovementX;
    public float MovementSpeedX = 3;
    public float AboveOrBelowZeroX = 0;
    public float LeftAndRightAmount = 5;
    [Space]
    [Header("Y Settings")]
    public bool MovementY;
    public bool ReverseMovementY;
    public float MovementSpeedY = 3;
    public float AboveOrBelowZeroY = 0;
    public float UpAndDownAmount = 5;
    [Space]
    [Header("Z Settings")]
    public bool MovementZ;
    public bool ReverseMovementZ;
    public float MovementSpeedZ = 3;
    public float AboveOrBelowZeroZ = 0;
    public float ForwardAndBackAmount = 5;
    [Space]
    [Header("Rotation")]
    public Vector3 rotations = new Vector3(0, 0, 0);
    private Vector3 startLoc;
    public bool HoverEnabled;

    // Use this for initialization
    void Start()
    {

        startLoc = transform.localPosition;

    }

    // Update is called once per frame
    void Update()
    {
        if (HoverEnabled)
        {
            Vector3 newPosition;
            newPosition = startLoc;

            if (MovementX)
            {
                ReverseMovementX = false;
                newPosition.x = startLoc.x + ((Mathf.Sin(Time.time * MovementSpeedX) + AboveOrBelowZeroX) / LeftAndRightAmount);
            }
            else if (ReverseMovementX)
            {
                MovementX = false;
                newPosition.x = startLoc.x - ((Mathf.Sin(Time.time * MovementSpeedX) - AboveOrBelowZeroX) / LeftAndRightAmount);
            }

            if (MovementY)
            {
                ReverseMovementY = false;
                newPosition.y = startLoc.y + ((Mathf.Sin(Time.time * MovementSpeedY) + AboveOrBelowZeroY) / UpAndDownAmount);
            }
            else if (ReverseMovementY)
            {
                MovementY = false;
                newPosition.y = startLoc.y - ((Mathf.Sin(Time.time * MovementSpeedY) - AboveOrBelowZeroY) / UpAndDownAmount);
            }

            if (MovementZ)
            {
                ReverseMovementZ = false;
                newPosition.z = startLoc.z + ((Mathf.Sin(Time.time * MovementSpeedZ) + AboveOrBelowZeroZ) / ForwardAndBackAmount);
            }
            else if (ReverseMovementZ)
            {
                MovementZ = false;
                newPosition.z = startLoc.z - ((Mathf.Sin(Time.time * MovementSpeedZ) - AboveOrBelowZeroZ) / ForwardAndBackAmount);
            }
            transform.localPosition = newPosition;
        }
        transform.Rotate(new Vector3(rotations.x, rotations.y, rotations.z) * Time.deltaTime);
    }
}
