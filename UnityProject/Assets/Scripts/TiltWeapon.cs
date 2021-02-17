using UnityEngine;

public class TiltWeapon : MonoBehaviour
{
    public float MoveAmount = 1;
    public float MoveSpeed = 1;
    public GameObject Gun;
    float MoveOnX;
    float MoveOnY;
    Vector3 defaultPos;
    Vector3 NewGunPos;
    public bool ONOFF;
    void Start ()
    {
        defaultPos = transform.localPosition;
	}

	void Update ()
    {
        if (!SceneSystem.isDisabled)
        {
            if (!PauseGame.isPaused && !ShopSystem.isShop && !CharacterSystem.isParalyzed && !CharacterSystem.isClimbing && !SelectionalSystem.isSelecting && !DialogueSystem.isDialogueActive && !PlayerSystem.isDead)
            {
                MoveOnX = Input.GetAxis("Mouse X") * Time.deltaTime * MoveAmount;
                MoveOnY = Input.GetAxis("Mouse Y") * Time.deltaTime * MoveAmount;

                if (ONOFF == true)
                {
                    NewGunPos = new Vector3(defaultPos.x + MoveOnX, defaultPos.y + MoveOnY, defaultPos.z);
                    Gun.transform.localPosition = Vector3.Lerp(Gun.transform.localPosition, NewGunPos, MoveSpeed * Time.deltaTime);
                }
                else
                {
                    ONOFF = false;
                    Gun.transform.localPosition = Vector3.Lerp(Gun.transform.localPosition, defaultPos, MoveSpeed * Time.deltaTime);
                }
            }
        }
    }
}
