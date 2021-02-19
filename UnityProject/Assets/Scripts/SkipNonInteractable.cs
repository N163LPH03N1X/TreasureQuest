using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkipNonInteractable : MonoBehaviour, ISelectHandler
{
    OptSystem optSystem = new OptSystem();
    private Selectable m_Selectable;
    private Button b_Pointable;
    StorySystem story;
    // Use this for initialization
    void Awake()
    {
        m_Selectable = GetComponent<Selectable>();

        story = GameObject.FindGameObjectWithTag("Player").GetComponent<StorySystem>();
    }
   public void SelectCurrentSelected()
    {
        m_Selectable.Select();
    }
    public void OnSelect(BaseEventData evData)
    {
        // Don't apply skipping unless we are not interactable.
        if (m_Selectable.interactable) return;

        // Check if the user navigated to this selectable.
        if (optSystem.Input.GetAxis("DHorizontal") < 0)
        {
            Selectable select = m_Selectable.FindSelectableOnLeft();
            if (select == null || !select.gameObject.activeInHierarchy)
                select = m_Selectable.FindSelectableOnRight();
            StartCoroutine(DelaySelect(select));
        }
        else if (optSystem.Input.GetAxis("DHorizontal") > 0)
        {
            Selectable select = m_Selectable.FindSelectableOnRight();
            if (select == null || !select.gameObject.activeInHierarchy)
                select = m_Selectable.FindSelectableOnLeft();
            StartCoroutine(DelaySelect(select));
        }
        else if (optSystem.Input.GetAxis("DVertical") < 0)
        {
            Selectable select = m_Selectable.FindSelectableOnDown();
            if (select == null || !select.gameObject.activeInHierarchy)
                select = m_Selectable.FindSelectableOnUp();
            StartCoroutine(DelaySelect(select));
        }
        else if (optSystem.Input.GetAxis("DVertical") > 0)
        {
            Selectable select = m_Selectable.FindSelectableOnUp();
            if (select == null || !select.gameObject.activeInHierarchy)
                select = m_Selectable.FindSelectableOnDown();
            StartCoroutine(DelaySelect(select));
        }
    }
    public void SelectBookType(int num)
    {
        b_Pointable = GetComponent<Button>();
        GameObject child = b_Pointable.transform.GetChild(1).gameObject;
        string ID = child.GetComponent<Text>().text;
        if (ItemSystem.StoryMenuOpen && b_Pointable.interactable)
        {
            if (num == 1)
                story.SetupBookUI(StorySystem.Book.decoded, ID);
            else if (num == 2)
                story.SetupBookUI(StorySystem.Book.history, ID);
            else if (num == 3)
                story.SetupBookUI(StorySystem.Book.discovery, ID);
            else
                story.SetupBookUI(StorySystem.Book.none, null);

        }
    }
        // Delay the select until the end of the frame.
        // If we do not, the current object will be selected instead.
        private IEnumerator DelaySelect(Selectable select)
    {
        yield return new WaitForEndOfFrame();

        if (select != null || !select.gameObject.activeInHierarchy)
            select.Select();
        else
            Debug.LogWarning("Please make sure your explicit navigation is configured correctly.");
    }
}
