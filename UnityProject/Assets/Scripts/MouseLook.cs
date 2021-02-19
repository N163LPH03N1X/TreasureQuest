using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Look")]

public class MouseLook : MonoBehaviour {

    OptSystem optSystem = new OptSystem();
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
                            float rotationX = transform.localEulerAngles.y + -optSystem.Input.GetAxis("MouseX") * sensitivityX;

                            rotationY += optSystem.Input.GetAxis("MouseY") * sensitivityY * (CharacterSystem.isConfused ? -1 : 1);
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
                                transform.Rotate(0, -optSystem.Input.GetAxis("MouseX") * Time.deltaTime * sensitivityX * 40, 0);
                            }
                            else
                            {
                                transform.Rotate(0, -optSystem.Input.GetAxis("MouseX") * sensitivityX, 0);
                            }
                        }
                        else
                        {
                            if (smoothRotation)
                            {
                                rotationY += optSystem.Input.GetAxis("MouseY") * Time.deltaTime * sensitivityY * (CharacterSystem.isConfused ? -1 : 1) * 40;
                                if (CharacterSystem.isClimbing)
                                    rotationY = Mathf.Clamp(rotationY, 45, 45);
                                else
                                    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                            }
                            else
                            {
                                rotationY += optSystem.Input.GetAxis("MouseY") * sensitivityY * (CharacterSystem.isConfused ? -1 : 1);
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
                            float rotationX = transform.localEulerAngles.y + optSystem.Input.GetAxis("MouseX") * sensitivityX;

                            rotationY += optSystem.Input.GetAxis("MouseY") * sensitivityY * (invertY ? -1 : 1);
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
                                transform.Rotate(0, optSystem.Input.GetAxis("MouseX") * Time.deltaTime * sensitivityX * 40, 0);
                            }
                            else
                            {
                                transform.Rotate(0, optSystem.Input.GetAxis("MouseX") * sensitivityX, 0);
                            }
                        }
                        else
                        {
                            if (smoothRotation)
                            {
                                rotationY += optSystem.Input.GetAxis("MouseY") * Time.deltaTime * sensitivityY * (invertY ? -1 : 1) * 40;
                                if (CharacterSystem.isClimbing)
                                    rotationY = Mathf.Clamp(rotationY, 45, 45);
                                else
                                    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                            }
                            else
                            {
                                rotationY += optSystem.Input.GetAxis("MouseY") * sensitivityY * (invertY ? -1 : 1);
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