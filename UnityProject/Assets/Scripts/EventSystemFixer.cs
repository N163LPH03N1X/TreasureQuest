using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class EventSystemFixer : MonoBehaviour
{
    public EventSystem eventSystem;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        button.GetComponent<Button>();
        eventSystem.GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(button.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        button.GetComponent<Button>();
        eventSystem.GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(button.gameObject);
    }
}
