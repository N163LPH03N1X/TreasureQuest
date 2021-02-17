using UnityEngine;

public class DemonMaskObj : MonoBehaviour
{
    // Start is called before the first frame update
    bool active;
    public GameObject[] shutOffObjs;
    public GameObject[] turnOnObjs;
    void Start()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
        foreach (GameObject obj in shutOffObjs)
            obj.SetActive(true);
        foreach (GameObject obj in turnOnObjs)
            obj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(ItemSystem.demonMaskEnabled && !active)
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(true);
            foreach (GameObject obj in shutOffObjs)
                obj.SetActive(false);
            foreach (GameObject obj in turnOnObjs)
                obj.SetActive(true);
            active = true;
        }
        else if(!ItemSystem.demonMaskEnabled && active)
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
            foreach (GameObject obj in shutOffObjs)
                obj.SetActive(true);
            foreach (GameObject obj in turnOnObjs)
                obj.SetActive(false);
            active = false;
        }
            
    }
}
