using UnityEngine;
public class HoldCharacter : MonoBehaviour
{
    GameObject gameCtrl;
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.transform.parent = gameObject.transform;

        }
    }
    void OnTriggerExit(Collider col)
    {
        gameCtrl = GameObject.Find("/Core/Player/");
        col.gameObject.transform.parent = gameCtrl.transform;
    }
}