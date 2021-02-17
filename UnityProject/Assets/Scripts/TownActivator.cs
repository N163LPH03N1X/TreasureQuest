using UnityEngine;

public class TownActivator : MonoBehaviour
{
    private void Start()
    {
        SwitchOnObjects(false);
    }
    public GameObject[] activeObjects;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            SwitchOnObjects(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            SwitchOnObjects(false);
    }
    public void SwitchOnObjects(bool active)
    {
        if (active)
            foreach(GameObject obj in activeObjects)
                obj.SetActive(true);
        else
            foreach (GameObject obj in activeObjects)
                obj.SetActive(false);
    }
    
}
