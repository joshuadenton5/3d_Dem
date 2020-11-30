using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GI_Events[] allInteractables;
    [SerializeField] private Image retical;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        allInteractables = FindObjectsOfType<GI_Events>();
        retical.color = Color.grey;
        SetUpListeners();
    }

    void SetUpListeners()
    {
        foreach(GI_Events event_ in allInteractables)
        {
            event_.OnLook += ChangeReticalColour;
        }
    }

    void ChangeReticalColour(Color col)
    {
        retical.color = col;
    }
}
