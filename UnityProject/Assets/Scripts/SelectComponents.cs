using UnityEngine.UI;
using UnityEngine;

public class SelectComponents : MonoBehaviour
{
    private Selectable m_Selectable;
    public Image fileBg;
    public Sprite[] backgrounds;
    public Button startButton;
    public Button deleteButton;
    public Button File1Button;

    public static bool FileDeletion;

    bool backButton = false;
    private void Update()
    {
        if (!MMTransition.file1Open && !MMTransition.file2Open && !MMTransition.file3Open && !MMTransition.file4Open)
        {
            if (Input.GetButtonDown("Cancel") && !backButton)
            {
                Backout();
                backButton = true;
            }
            else if (Input.GetButtonDown("Cancel"))
                backButton = false;
        }
        else if(MMTransition.file1Open || MMTransition.file2Open  || MMTransition.file3Open || MMTransition.file4Open)
        {
            backButton = true;
        }
    }

    public void clickStartButton()
    {
        FileDeletion = false;
        fileBg.sprite = backgrounds[0];
        m_Selectable = File1Button.GetComponent<Selectable>();
        m_Selectable.Select();
    }
    public void Backout()
    {
        if (!FileDeletion)
            m_Selectable = startButton.GetComponent<Selectable>();
        else
            m_Selectable = deleteButton.GetComponent<Selectable>();
        m_Selectable.Select();
    }
    public void ClickDeleteButton()
    {
        FileDeletion = true;
        fileBg.sprite = backgrounds[1];
        m_Selectable = File1Button.GetComponent<Selectable>();
        m_Selectable.Select();
    }
}
