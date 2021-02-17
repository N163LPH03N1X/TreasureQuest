using UnityEngine;

public class OrangePlatformSystem : MonoBehaviour
{
    Animator anim;
    public Move moveType;
    public float moveSpeed = 1f;
    void Start()
    {
        anim = GetComponent<Animator>();
        SetMovement(moveType);
    }
    public enum Move { Idle, Dungeon3, Dungeon3Two }
    public void SetMovement(Move type)
    {
        switch (type)
        {
            case Move.Idle:
                {
                    anim.SetBool("Dungeon3", false);
                    break;
                }
            case Move.Dungeon3:
                {
                    anim.SetBool("Dungeon3", true);
                    break;
                }
            case Move.Dungeon3Two:
                {
                    anim.SetBool("Dungeon3.2", true);
                    break;
                }
        }
        anim.SetFloat("MoveSpeed", moveSpeed);
    }
}
