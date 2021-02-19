using UnityEngine.UI;
using UnityEngine;

public class MimicEncounter : MonoBehaviour
{
    OptSystem optSystem = new OptSystem();
    bool isOpen;
    public bool inTerritory;
    AudioSource audioSrc;
    public AudioClip chestOpenSfx;

    GameObject playerController;
    BoxCollider col;
    public GameObject MimicEnemy;
    EnemySystem enemySys;

    GameObject popupCanvas;
    GameObject newInteraction;
    Text intText;
    bool interactionActive;
    public GameObject interactionObj;
    public GameObject chestStatusPrefab;

    void Update()
    {
        if (interactionActive && newInteraction != null)
        {
            Vector3 statusPos = Camera.main.WorldToScreenPoint(interactionObj.transform.position);
            newInteraction.transform.position = statusPos;
            newInteraction.transform.localScale = new Vector3(1, 1, 1);
        }
        if (inTerritory)
        {
            if (optSystem.Input.GetButtonDown("Submit") && !isOpen && !PauseGame.isPaused)
            {
                isOpen = true;
                audioSrc = GetComponent<AudioSource>();
                playerController = GameObject.FindGameObjectWithTag("Player");
                col = GetComponent<BoxCollider>();
                CharacterSystem playChar = playerController.GetComponent<CharacterSystem>();
                inTerritory = false;
                if(!JewelSystem.isJewelEnabled)
                    playChar.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                col.enabled = false;
                enemySys = MimicEnemy.GetComponent<EnemySystem>();
                enemySys.MimicHide(2);
                audioSrc.PlayOneShot(chestOpenSfx);
                SetupPopupCanvas(false, null);
            }
        }
    }
    public void SetupPopupCanvas(bool active, string message)
    {
        if (active)
        {
            if (newInteraction != null)
                Destroy(newInteraction.gameObject);
            popupCanvas = GameObject.Find("Core/Player/PopUpCanvas/PlayerUIStorage");
            newInteraction = Instantiate(chestStatusPrefab, transform.position, Quaternion.identity);
            newInteraction.transform.SetParent(popupCanvas.transform);
            Transform interactionObj = newInteraction.GetComponentInChildren<Transform>().Find("InteractionText");
            intText = interactionObj.GetComponent<Text>();
            intText.enabled = true;
            intText.text = message;
            interactionActive = true;
        }
        else
        {
            if (newInteraction != null)
                Destroy(newInteraction.gameObject);
            interactionActive = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isOpen)
        {
            inTerritory = true;
            SetupPopupCanvas(true, "Open");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inTerritory = false;
            InteractionSystem interaction = other.gameObject.GetComponent<InteractionSystem>();
            interaction.DialogueInteraction(false, null);
            SetupPopupCanvas(false, null);
        }
    }
}
