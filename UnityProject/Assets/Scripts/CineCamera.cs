using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineCamera : MonoBehaviour
{
     [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private CharacterSystem firstPersonController;    
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Camera clipCamera;
    GameObject cameraObj;
    private Camera cinematicCamera;
    private Animator cinematicCamAnimator;
    GameObject playerController;

    private void Start()
    {
        cinematicCamera = GetComponent<Camera>();
        cinematicCamAnimator = GetComponent<Animator>();
        playerController = GameObject.FindGameObjectWithTag("Player");
    }

    public void TriggerClimbUpAnimation()
    {
        cameraObj = GameObject.Find("Core/Player/PlayerController/Head/PlayerCamera");
        cameraObj.transform.localRotation = Quaternion.identity;
       
        mainCamera.enabled = false;
        clipCamera.enabled = false;
        StartCoroutine(InitClimbUpAnimation());
    }

    public void MoveCharacterToCinematicPosition()
    {        
        playerController.transform.position = new Vector3(transform.position.x, transform.position.y - 3, transform.position.z);

        cinematicCamera.enabled = false;
        cinematicCamera.depth = 0;
        clipCamera.enabled = true;
        mainCamera.enabled = true;
        GameObject headObj = GameObject.Find("Core/Player/PlayerController/Head");
        headObj.transform.rotation = Quaternion.identity;
        StartCoroutine(WaitBeforeCharacterControllerEnable());
        firstPersonController.enabled = true;
    }

    private IEnumerator WaitBeforeCharacterControllerEnable()
    {
        yield return new WaitForSeconds(1f);
        characterController.enabled = true;
    }

    private IEnumerator InitClimbUpAnimation()
    {
        yield return new WaitForEndOfFrame();

        characterController.enabled = false;
        firstPersonController.enabled = false;
        cinematicCamera.enabled = true;
        cinematicCamera.depth = 3;
        cinematicCamAnimator.SetTrigger("ClimbUp");
    }
    public void EnableMovement(int num)
    {

        CharacterSystem characterSystem = playerController.GetComponent<CharacterSystem>();
        if(num == 1)
            characterSystem.SetCinematicCamera(true);
        else
            characterSystem.SetCinematicCamera(false);
    }
}
