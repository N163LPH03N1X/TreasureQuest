using UnityEngine;

public class CarryReturn : MonoBehaviour
{
    BlockSystem block;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("KillBlock"))
        {
            block = other.gameObject.GetComponent<BlockSystem>();
            block.ReturnBlock(BlockSystem.ReturnType.Position);
        }
    }
}
