using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GI_Events[] allInteractables;
    public Image retical;

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
            event_.OnLook += ChangeColour;
        }
    }

    void ChangeColour(Color col)
    {
        retical.color = col;
    }
}
