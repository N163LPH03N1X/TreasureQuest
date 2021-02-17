using UnityEngine;

public class BlockSystem : MonoBehaviour
{
    Transform orgParent;
    Vector3 orgPos;
    Quaternion orgRot;
    GameObject blockParent;
    AudioSource audiosrc;
    public AudioClip blockColSfx;
    public float gravityStrength;
    ConstantForce gravity;
    Rigidbody rb;


    void Start()
    {

        blockParent = transform.parent.gameObject;
        orgPos = blockParent.transform.position;
        orgRot = blockParent.transform.rotation;
        gravity = blockParent.GetComponent<ConstantForce>();
        rb = blockParent.GetComponent<Rigidbody>();
        audiosrc = GetComponent<AudioSource>();
        orgParent = blockParent.transform.parent;
    }


    void Update()
    {
      
        gravity.force = new Vector3(0, gravityStrength, 0);
    }
    public enum ReturnType { Position, Parent}
    public void ReturnBlock(ReturnType type)
    {

        switch (type)
        {
            case ReturnType.Parent:
                {
                    blockParent.transform.SetParent(orgParent);
                  
                    break;
                }
            case ReturnType.Position:
                {
                    CharacterSystem charSys = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterSystem>();
                    if (CharacterSystem.isCarrying)
                    {
                        charSys.SelectAnimation(CharacterSystem.PlayerAnimation.Rebind, true);
                        charSys.PlayerPickUpCarryObj(0);

                    }
                    else
                        ReturnBlock(ReturnType.Parent);
                    blockParent.transform.position = orgPos;
                    break;
                }
        }
    }
    
    public void OnCollisionEnter(Collision collision)
    {
        audiosrc.PlayOneShot(blockColSfx);


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Platform"))
            blockParent.transform.SetParent(other.transform);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            GameObject returnObj = GameObject.Find("NonStatic/Carries/CarryObjs");
            blockParent.transform.SetParent(returnObj.transform);
        }
    }


}
