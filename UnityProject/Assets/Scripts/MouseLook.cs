using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Look")]

public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX;
    public float sensitivityY;
    bool invertY = false;
    public float minimumY = -30f;
    public float maximumY = 45f;

    private float rotationY = 0f;

    bool smoothRotation;

    void Start()
    {
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
       
    }
    void Update ()
	{
        sensitivityX = SettingsMenu.SensitivityX;
        sensitivityY = SettingsMenu.SensitivityY;
        invertY = SettingsMenu.invertY;
        smoothRotation = SettingsMenu.smoothRotation;

        if (!SceneSystem.isDisabled)
        {
            if (!CharacterSystem.isCinematic && !PauseGame.isPaused && !ShopSystem.isShop && !CharacterSystem.isClimbing && !CharacterSystem.isParalyzed && !SelectionalSystem.isSelecting && !DialogueSystem.isDialogueActive && !PlayerSystem.isDead)
            {
                if (!UnderWaterSystem.isSwimming)
                {
                    if (CharacterSystem.isConfused)
                    {
                        if (axes == RotationAxes.MouseXAndY)
                        {
                            float rotationX = transform.localEulerAngles.y + -Input.GetAxis("Mouse X") * sensitivityX;

                            rotationY += Input.GetAxis("Mouse Y") * sensitivityY * (CharacterSystem.isConfused ? -1 : 1);
                            if (CharacterSystem.isClimbing)
                                rotationY = Mathf.Clamp(rotationY, 45, 45);
                            else
                                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
                        }

                        else if (axes == RotationAxes.MouseX)
                        {

                            if (smoothRotation)
                            {
                                transform.Rotate(0, -Input.GetAxis("Mouse X") * Time.deltaTime * sensitivityX * 40, 0);
                            }
                            else
                            {
                                transform.Rotate(0, -Input.GetAxis("Mouse X") * sensitivityX, 0);
                            }
                        }
                        else
                        {
                            if (smoothRotation)
                            {
                                rotationY += Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivityY * (CharacterSystem.isConfused ? -1 : 1) * 40;
                                if (CharacterSystem.isClimbing)
                                    rotationY = Mathf.Clamp(rotationY, 45, 45);
                                else
                                    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                            }
                            else
                            {
                                rotationY += Input.GetAxis("Mouse Y") * sensitivityY * (CharacterSystem.isConfused ? -1 : 1);
                                if (CharacterSystem.isClimbing)
                                    rotationY = Mathf.Clamp(rotationY, 45, 45);
                                else
                                    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                            }
                            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
                        }
                    }
                    else
                    {
                        if (axes == RotationAxes.MouseXAndY)
                        {
                            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

                            rotationY += Input.GetAxis("Mouse Y") * sensitivityY * (invertY ? -1 : 1);
                            if (CharacterSystem.isClimbing)
                                rotationY = Mathf.Clamp(rotationY, 45, 45);
                            else
                                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
                        }

                        else if (axes == RotationAxes.MouseX)
                        {

                            if (smoothRotation)
                            {
                                transform.Rotate(0, Input.GetAxis("Mouse X") * Time.deltaTime * sensitivityX * 40, 0);
                            }
                            else
                            {
                                transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
                            }
                        }
                        else
                        {
                            if (smoothRotation)
                            {
                                rotationY += Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivityY * (invertY ? -1 : 1) * 40;
                                if (CharacterSystem.isClimbing)
                                    rotationY = Mathf.Clamp(rotationY, 45, 45);
                                else
                                    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                            }
                            else
                            {
                                rotationY += Input.GetAxis("Mouse Y") * sensitivityY * (invertY ? -1 : 1);
                                if (CharacterSystem.isClimbing)
                                    rotationY = Mathf.Clamp(rotationY, 45, 45);
                                else
                                    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                            }
                            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
                        }
                    }
                }

            }
        }
    }
}