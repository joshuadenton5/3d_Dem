using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GI_Events[] allInteractables;
    [SerializeField] private Image retical;
    [SerializeField] private Text onScreenText;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        allInteractables = FindObjectsOfType<GI_Events>();
        retical.color = Color.grey;
        onScreenText.text = "";
        SetUpListeners();
    }

    void SetUpListeners()
    {
        foreach(GI_Events event_ in allInteractables)
        {
            event_.OnLook += ChangeReticalColour;
        }
    }

    void ChangeReticalColour(Color col, string text)
    {
        retical.color = col;
        onScreenText.text = text;
    }
}
